using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.Data;
using System.Text;
using iTextSharp.text.pdf;
using System.IO;

namespace MeetingMinder.Web
{
    public partial class AgendaMaster : System.Web.UI.Page
    {
        /// <summary>
        /// Page laod Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                //bind agenda list
                BindAgendaItems();
            }

            if (Request.QueryString["agenda"] != null)
            {
                string agendaName = Convert.ToString(Request.QueryString["agenda"]);
                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                string FilePath = Server.MapPath(savePath + agendaName + ".pdf");
                string AgendaKey = "";
                DataSet ds = MM.Data.AgendaDataProvider.Instance.GetAgendKeysByName(agendaName + ".pdf");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AgendaKey = ds.Tables[0].Rows[0]["AgendaKey"].ToString();
                }
                PdfReader reader = null;
                if (AgendaKey == "")
                {
                    reader = new PdfReader(FilePath);
                }
                else
                {
                    string PdfPassword = EncryptionHelper.GetPassword(agendaName + ".pdf", AgendaKey);

                    reader = new PdfReader(FilePath, new System.Text.ASCIIEncoding().GetBytes(PdfPassword));
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfStamper stamper = new PdfStamper(reader, memoryStream);
                    stamper.Close();
                    reader.Close();

                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    // string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                    // Response.WriteFile(FilePath);
                    Response.BinaryWrite(memoryStream.ToArray());
                    Response.Flush();
                    Response.End();
                }

            }
        }

        /// <summary>
        /// Bind agenda items
        /// </summary>
        private void BindAgendaItems()
        {
            try
            {
                //check Meeting id
                if (Session["AgendaOrder"] != null)//(Request.QueryString["id"] != null)
                {
                string strMeetingId = Convert.ToString(Session["AgendaOrder"]); // Convert.ToString(Request.QueryString["id"]);

                    MeetingDomain objMeeting = MeetingDataProvider.Instance.GetMeetingWithEntity(Guid.Parse(strMeetingId));

                    string EntityName = objMeeting.EntityName;
                    string EntityId = objMeeting.EntityId.ToString();
                    string ForumId = objMeeting.ForumId.ToString();
                    string ForumName = objMeeting.ForumName;
                    string Meeting = objMeeting.MeetingDate + " " + objMeeting.MeetingVenue + " " + objMeeting.MeetingTime;

                    Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                    ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home<a>&nbsp;/&nbsp;<a href =EntityMaster.aspx>" + EntityName + "</a>&nbsp;/&nbsp;<a href=forummaster.aspx?id=" + EntityId + ">" + ForumName + "</a>&nbsp;/&nbsp;<a href=MeetingMaster.aspx?id=" + ForumId + ">" + Meeting + "</a>";

                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetAgendabyMeetingId(meetingId).OrderBy(p => p.AgendaOrder).ToList();

                        if (objAgentList.Count > 0)
                        {
                            //get only parent agenda
                            var objParentAgenda = from agend in objAgentList
                                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                  select agend;

                            //bind drop down with agendas
                            ddlAgenda.DataSource = objParentAgenda;
                            ddlAgenda.DataBind();
                            ddlAgenda.DataTextField = "AgendaName";
                            ddlAgenda.DataValueField = "AgendaId";
                            ddlAgenda.DataBind();

                            //get only sub agenda
                            var subAgendaList = from agend in objAgentList
                                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                select agend;

                            //get agenda name/note and agenda id
                            var agendaName = (from agend in objParentAgenda

                                              select (new
                                              {
                                                  AgendaName = agend.AgendaName,
                                                  AgendaId = agend.AgendaId,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();
                            //Dictionary<string, int> objSerial = new Dictionary<string, int>();
                            //Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";

                                strMenu.Append("<div id='accordion'><ol id='sort'>");
                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber + " : ";
                                    }

                                    ClassificationNew = agendaName[i].Classifications;
                                    Guid agendaId = agendaName[i].AgendaId;
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            strMenu.Append("<li id=" + agendaId + " class='group' >");
                                            strMenu.Append("<h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                            strMenu.Append("<h3>" + agendaName[i].AgendaName + "</h3>");
                                        }
                                        else
                                        {
                                            strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + "</h3>");

                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + "</h3>");
                                    }
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName,
                                                             AgendaId = subAgenda.AgendaId,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText

                                                         })).ToList();

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                  
                                        strMenu.Append("<div><ol style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            string serialNo = "";
                                            serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                            if (serialNo.Trim().Length > 0)
                                            {
                                                serialNo = serialNo + " : ";
                                            }
                                            //if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                            //{
                                            //    if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                            //    {
                                            //        if (subAgendaName[j].SerialNumberType != "Other")
                                            //        {
                                            //            objSerial[subAgendaName[j].SerialNumber] += 1;
                                            //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        if (subAgendaName[j].SerialNumberType != "Other")
                                            //        {
                                            //            objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                            //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //}

                                            strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + serialNo + subAgendaName[j].AgendaName);// + "</li>");
                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName,
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                
                                                //attach sub sub agenda to parent agenda list element
                                                strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:50px;'>");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    string serialNoSub = "";
                                                    serialNoSub = CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText);
                                                    if (serialNoSub.Trim().Length > 0)
                                                    {
                                                        serialNoSub = serialNoSub + " : ";
                                                    }
                                                    //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                                    //{
                                                    //    if (objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))//objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                                    //    {
                                                    //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                                    //        {
                                                    //            objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";


                                                    //            objSerial[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                                    //        {
                                                    //            objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";

                                                    //            objSerial.Add(subSubAgendaName[y].SerialNumber, 1);
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                    //        }
                                                    //    }
                                                    //}
                                                    strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + serialNoSub + subSubAgendaName[y].AgendaName);
                                                }
                                                strMenu.Append("</ol>");
                                            }
                                            strMenu.Append("</li>");
                                        }
                                        strMenu.Append("</ol></div></li>");
                                    }

                                }
                                strMenu.Append("</ol></div>");
                            }
                            lblList.Text = strMenu.ToString();
                            //  lblList.Text = strMenuList;
                        }

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid Agenda search";
                        Error.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("MeetingMaster.aspx");
                }
            }
            catch (Exception ex)
            {

                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }
        /// <summary>
        /// Insert Agenda details
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                AgendaDomain objAgenda = new AgendaDomain();
                objAgenda.AgendaName = txtAgenda.Text;
                objAgenda.AgendaNote = txtAgenda.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UploadedAgendaNote = txtAgenda.Text;
                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ViewState["MeetingId"]));
                //Update
                if (hdnAgendaId != null && hdnAgendaId.Value != "")
                {

                }
                //Insert
                else
                {
                    UserAcess objUser = new UserAcess();
                    //check add permission
                    // if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                    if (objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        objAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        AgendaDataProvider.Instance.Insert(objAgenda);
                        ((Label)Info.FindControl("lblName")).Text = "Agenda inserted successfully";

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                Info.Visible = true;
                BindAgendaItems();
            }
            catch (Exception ex)
            {

                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Insert sub agenda Agenda details
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubAgendaSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                AgendaDomain objAgenda = new AgendaDomain();

                objAgenda.AgendaName = txtSubAgenda.Text;
                objAgenda.AgendaNote = txtSubAgenda.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UploadedAgendaNote = txtSubAgenda.Text;


                objAgenda.ParentAgendaId = Guid.Parse(ddlAgenda.SelectedValue);

                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ViewState["MeetingId"]));
                //Update
                if (hdnAgendaId != null && hdnAgendaId.Value != "")
                {

                }
                //Insert
                else
                {
                    UserAcess objUser = new UserAcess();
                    //check edit permission
                    if (objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        objAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        AgendaDataProvider.Instance.Insert(objAgenda);
                        ((Label)Info.FindControl("lblName")).Text = "Sub Agenda inserted successfully";

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                Info.Visible = true;
                BindAgendaItems();
            }
            catch (Exception ex)
            {

                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// save order of agenda items
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //get order of agenda items

                string UserId = Convert.ToString(Session["UserId"]);
                StringBuilder strAgendaIds = new StringBuilder(",");
                StringBuilder strAgendaOrder = new StringBuilder(",");
                string idOrders = hdnListItems.Value;
                if (idOrders.Length > 0)
                {
                    // string sql = "";
                    idOrders = idOrders.Remove(idOrders.Length - 1);
                    string[] arryOrderIds = idOrders.Split(',');
                    for (int i = 1; i <= arryOrderIds.Count(); i++)
                    {
                        strAgendaOrder.Append(i + ",");
                        strAgendaIds.Append(arryOrderIds[i - 1] + ",");
                        //sql += " UPDATE Agenda set AgendaOrder= " + i + " WHERE AgendaId = '" + arryOrderIds[i - 1] + "'  UPDATE Agenda set AgendaOrder= " + i + " WHERE ParentAgendaId = '" + arryOrderIds[i - 1] + "' ";
                    }

                    bool bUpdate = AgendaDataProvider.Instance.UpdateAgendaOrders(strAgendaOrder.ToString(), strAgendaIds.ToString(), Guid.Parse(UserId));
                    if (!bUpdate)
                    {
                        BindAgendaItems();
                        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Agenda order changed successfully!');';</script>'");
                    }
            
             
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Upload Agenda details
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnUpAgendaSubmit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Back button click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //string url = Convert.ToString(ViewState["previousUrl"]);
            //if (url != null && url != "")
            //{
            //    Response.Redirect(url);
            //}
            //else
            //{
            //    Response.Redirect("default.aspx");
            //}
            string strMeetingId = Convert.ToString(Session["AgendaOrder"]); //Convert.ToString(Request.QueryString["id"]);
            Response.Redirect("CreateAgenda.aspx");
            //if (Request.QueryString["id"] != null)
            //{
            //    string strMeetingId = Convert.ToString(Request.QueryString["id"]);
            //    Response.Redirect("CreateAgenda.aspx?id=" + strMeetingId);
            //}
            //else
            //{
            //    Response.Redirect("CreateAgenda.aspx");
            //}
        }
    }
}