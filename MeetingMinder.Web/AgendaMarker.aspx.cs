using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.IO;
using System.Data;
using System.Text;

namespace MeetingMinder.Web
{
    public partial class AgendaMarker : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                BindYear();
            }
            if (Request.QueryString["file"] != null)
            {
                try
                {
                    string fileName = Convert.ToString(Request.QueryString["file"]);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                    //Set the appropriate ContentType.
                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.WriteFile(FilePath);
                    Response.End();
                }
                catch (Exception ex)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    Error.Visible = true;

                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                }
            }

            if (!IsPostBack)
            {
                //Bind Entity List
                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

            }
        }


        /// <summary>
        /// Bind Year to drop down 
        /// </summary>

        private void BindYear()
        {
            int currentYear = DateTime.Now.Year;
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
            for (int i = 2012; i <= currentYear+1; i++)
            {

                ddlYear.Items.Add(i.ToString());
            }
        }

        /// <summary>
        /// Bind Entity list to drop down 
        /// </summary>
        private void BindEntity()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); ;
                ddlEntity.DataBind();
                ddlEntity.DataTextField = "EntityName";
                ddlEntity.DataValueField = "EntityId";
                ddlEntity.DataBind();
                ddlEntity.Items.Insert(0, new ListItem("Select Entity", "0"));
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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                ddlMeeting.Items.Clear();
                lblList.Visible = false;
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                if (strForumId != "0")
                {
                    BindMeeting(strForumId);

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
        /// Bind Meetings list to drop down
        /// </summary>
        /// <param name="strForumId">string specifying strForumId</param>
        private void BindMeeting(string strForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).Where(p => DateTime.Parse(p.MeetingDate).Year == Convert.ToInt32(ddlYear.SelectedItem.Text)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting > dtToday)
                        //{
                        //ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                        ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));


                        //}
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
                    Error.Visible = true;
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

        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlEntity.SelectedValue;
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                lblList.Visible = false;
                if (strEntityId != "0")
                {
                    BindForum(strEntityId);
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
        /// Bind forums to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                Guid entityId;
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();
                    //IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);                    
                    ddlForum.DataSource = objForum;
                    ddlForum.DataBind();
                    ddlForum.DataTextField = "ForumName";
                    ddlForum.DataValueField = "ForumId";
                    ddlForum.DataBind();
                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
                    Error.Visible = true;
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


        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        /// 
        //protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        lblList.Text = "";
        //        lblList.Visible = true;
        //        btnPrint.Visible = false;
        //        string strMeetingId = ddlMeeting.SelectedValue;
        //        if (strMeetingId != "0")
        //        {
        //            Guid meetingId;
        //            if (Guid.TryParse(strMeetingId, out meetingId))
        //            {
        //                ViewState["MeetingId"] = meetingId;

        //                StringBuilder strMenu = new StringBuilder("");
        //                IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

        //                if (objAgentList.Count > 0)
        //                {
        //                    //get only parent agenda
        //                    var objParentAgenda = from agend in objAgentList
        //                                          where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                          select agend;



        //                    //get only sub agenda
        //                    var subAgendaList = from agend in objAgentList
        //                                        where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                        select agend;

        //                    //get agenda name/note and agenda id
        //                    var agendaName = (from agend in objParentAgenda

        //                                      select (new
        //                                      {
        //                                          AgendaName = agend.AgendaName,
        //                                          AgendaId = agend.AgendaId,
        //                                          UplaodedAgenda = agend.UploadedAgendaNote

        //                                      })).Distinct().ToList();

        //                    if (agendaName.Count() > 0)
        //                    {
        //                        strMenu.Append("<div id='accordion'><ol id='sort'>");
        //                        for (int i = 0; i <= agendaName.Count() - 1; i++)
        //                        {
        //                            Guid agendaId = agendaName[i].AgendaId;

        //                            //get sub agenda for parent agenda 
        //                            var subAgendaName = (from subAgenda in subAgendaList
        //                                                 where (subAgenda.ParentAgendaId == agendaId)
        //                                                 select (new
        //                                                 {
        //                                                     AgendaName = subAgenda.AgendaName,
        //                                                     AgendaId = subAgenda.AgendaId,
        //                                                     UplaodedAgenda = subAgenda.UploadedAgendaNote
        //                                                 })).ToList();

        //                            if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
        //                            {
        //                                strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");
        //                            }
        //                            else
        //                            {
        //                                strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + agendaName[i].AgendaName + "</h3>");
        //                            }



        //                            //attach sub agenda to parent agenda list element
        //                            if (subAgendaName.Count() > 0)
        //                            {
        //                                strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
        //                                for (int j = 0; j <= subAgendaName.Count() - 1; j++)
        //                                {
        //                                    if (subAgendaName[j].UplaodedAgenda.Length > 0)
        //                                    {
        //                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><a href=ViewAgenda.aspx?file=" + subAgendaName[j].UplaodedAgenda + "  Target='_blank'> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + "</a>");
        //                                    }
        //                                    else
        //                                    {
        //                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + ">" + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName);// + "</li>");
        //                                    }
        //                                    Guid subAgendaId = subAgendaName[j].AgendaId;
        //                                    //Get sub sub agenda
        //                                    var subSubAgendaName = (from subAgenda in subAgendaList
        //                                                            where (subAgenda.ParentAgendaId == subAgendaId)
        //                                                            select (new
        //                                                            {
        //                                                                AgendaName = subAgenda.AgendaName,
        //                                                                AgendaId = subAgenda.AgendaId,
        //                                                                UplaodedAgenda = subAgenda.UploadedAgendaNote
        //                                                            })).ToList();
        //                                    if (subSubAgendaName.Count() > 0)
        //                                    {
        //                                        //attach sub sub agenda to parent agenda list element
        //                                        strMenu.Append("<ol class=inddrag  style='list-style: upper-alpha outside none;margin-left:50px;'>");
        //                                        for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
        //                                        {
        //                                            if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
        //                                            {
        //                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><a href=ViewAgenda.aspx?file=" + subSubAgendaName[y].UplaodedAgenda + "  Target='_blank'> " + subSubAgendaName[y].AgendaName + "</a>");
        //                                            }
        //                                            else
        //                                            {
        //                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + subSubAgendaName[y].AgendaName);
        //                                            }
        //                                        }
        //                                        strMenu.Append("</ol>");
        //                                    }
        //                                    strMenu.Append("</li>");
        //                                }
        //                                strMenu.Append("</ol></div></li>");
        //                            }

        //                        }
        //                        strMenu.Append("</ol></div>");
        //                    }
        //                    lblList.Text = strMenu.ToString();
        //                    btnPrint.Visible = true;

        //                    //  lblList.Text = strMenuList;
        //                }
        //                else
        //                {
        //                    lblList.Text = "No agenda uploaded or approved";
        //                    btnPrint.Visible = false;
        //                }
        //            }

        //            else
        //            {
        //                ((Label)Error.FindControl("lblError")).Text = "Invalid Agenda search";
        //                Error.Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);
        //    }
        //}

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblList.Text = "";
                lblList.Visible = true;
                btnUpdateStatus.Visible = false;
                string strMeetingId = ddlMeeting.SelectedValue;
                StringBuilder strAllAgenda = new StringBuilder("");
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedPublishedAgendabyMeetingId(meetingId);

                        IList<MarkedAgendaDomain> objMarkedAgenda = MarkedAgendaDataProvider.Instance.GetMarkedAgendaByMeeting(Guid.Parse(ddlMeeting.SelectedValue));

                        List<Guid> objUsersAgendaIds = new List<Guid>();
                        if (objMarkedAgenda.Count > 0)
                        {
                            objUsersAgendaIds = (from ids in objMarkedAgenda  where ids.IsRead == true select ids.AgendaId).ToList<Guid>();

                            IEnumerable<string> strVals = (from ids in objMarkedAgenda where ids.IsRead == true select ids.AgendaId.ToString());
                            hdnAgenda.Value = string.Join(",", strVals.ToArray());
                          //  ViewState["AgendaIds"] = strVals.ToList();

                        }

                        if (objAgentList.Count > 0)
                        {
                            //MeetingDomain objMeet = new MeetingDomain();
                            hdnMeeting.Value = objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " + objAgentList[0].MeetingVenue;

                            //objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
                            //ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
                            //get only parent agenda
                            var objParentAgenda = from agend in objAgentList
                                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                  select agend;



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
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                             
                                string ClassificationOld = "";
                                string ClassificationNew = "";
                                strMenu.Append("<div id='accordion'><ol class='ddrag'>");
                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;
                                    ClassificationNew = agendaName[i].Classifications;
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName,
                                                             AgendaId = subAgenda.AgendaId,
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType

                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            strMenu.Append("<h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }
                                        //else
                                        //{
                                        //    strMenu.Append("<h2><b>Unclassified</b></h2>");

                                        //}
                                    }

                                    string Presenter = "";

                                    if (agendaName[i].Presenter.Trim().Length > 0)
                                    {
                                        Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
                                    }

                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber + " : ";
                                    }

                                    if (agendaName[i].UplaodedAgenda.Length > 0)
                                    {
                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");



                                        //for (int ii = 0; ii <= agendaNames.Count() - 1; ii++)
                                        //{
                                        //    if (!strDeletedAgenda.Contains(agendaNames[ii].Trim()))
                                        //    {
                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ii].Trim()) ? "Agenda.pdf" : agendaPdfs[ii].Trim();
                                        //        //    strMenu.Append("<a  href=ViewAgenda.aspx?file=" + agendaNames[ii].Trim() + " Target='_blank' >" + pdfName + " </a> <br/> ");
                                        //        strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ii].Trim().Substring(0, agendaNames[ii].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> <br/> ");
                                        //    }
                                        //}

                                        bool SendPdfFile = false;
                                        foreach (string str in agendaNames)
                                        {
                                            if (!strDeletedAgenda.Contains(str))
                                            {
                                                SendPdfFile = true;
                                            }
                                        }
                                        string markAgenda = "";
                                        string AgendaAccess = "";
                                        if (objUsersAgendaIds.Contains(agendaId))
                                        {
                                            AgendaAccess = " checked='checked' ";
                                        }
                                        if (agendaNames.Count() > 0 && SendPdfFile && subAgendaName.Count() == 0)
                                        {
                                            strAllAgenda.Append(agendaId+",");
                                            markAgenda = " <input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";
                                        }

                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + markAgenda + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");

                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                        {
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                        }
                                    }
                                    else
                                    {
                                        string markAgenda = "";
                                        string AgendaAccess = "";
                                        if (objUsersAgendaIds.Contains(agendaId))
                                        {
                                            AgendaAccess = " checked='checked' ";
                                        }
                                        if (subAgendaName.Count() == 0)
                                        {
                                            strAllAgenda.Append(agendaId + ",");
                                             markAgenda = " <input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";
                                
                                        }
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + markAgenda + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            Presenter = "";
                                            string serialNo = "";

                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                            }

                                            if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                            {
                                                if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                                {
                                                    if (subAgendaName[j].SerialNumberType != "Other")
                                                    {
                                                        objSerial[subAgendaName[j].SerialNumber] += 1;
                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                                    }
                                                    else
                                                    {
                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
                                                    }
                                                }
                                                else
                                                {
                                                    if (subAgendaName[j].SerialNumberType != "Other")
                                                    {
                                                        objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                                    }
                                                    else
                                                    {
                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
                                                    }
                                                }
                                            }

                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName,
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType

                                                                    })).ToList();

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                                string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                                string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                                bool SendPdfFile = false;
                                                foreach (string str in agendaNames)
                                                {
                                                    if (!strDeletedAgenda.Contains(str))
                                                    {
                                                        SendPdfFile = true;
                                                    }
                                                }

                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><a href=ViewAgenda.aspx?file=" + subAgendaName[j].UplaodedAgenda + "  Target='_blank'> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + "</a>");                                      
                                                string markAgenda = "";
                                                string AgendaAccess = "";
                                                if (objUsersAgendaIds.Contains(subAgendaName[j].AgendaId))
                                                {
                                                    AgendaAccess = " checked='checked' ";
                                                }
                                                if (agendaNames.Count() > 0 && SendPdfFile && subSubAgendaName.Count() == 0)
                                                {
                                                    strAllAgenda.Append(subAgendaName[j].AgendaId + ",");
                                                    markAgenda = " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";
                                                }
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + markAgenda + serialNo + subAgendaName[j].AgendaName + Presenter + "");
                                                //for (int ll = 0; ll <= agendaNames.Count() - 1; ll++)
                                                //{
                                                //    if (!strDeletedAgenda.Contains(agendaNames[ll].Trim()))
                                                //    {
                                                //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ll].Trim()) ? "Agenda.pdf" : agendaPdfs[ll].Trim();
                                                //        strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ll].Trim().Substring(0, agendaNames[ll].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                //    }

                                                //}
                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                }
                                            }
                                            else
                                            {
                                                string markAgenda = "";
                                                string AgendaAccess = "";
                                                if (objUsersAgendaIds.Contains(subAgendaName[j].AgendaId))
                                                {
                                                    AgendaAccess = " checked='checked' ";
                                                }
                                                if (subSubAgendaName.Count() == 0)
                                                {
                                                    strAllAgenda.Append(subAgendaName[j].AgendaId + ",");
                                                    markAgenda = " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";

                                                }

                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + markAgenda + serialNo + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                            }

                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";

                                                    if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                                    {
                                                        if (objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                                        {
                                                            if (subSubAgendaName[y].SerialNumberType != "Other")
                                                            {
                                                                objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                                                serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
                                                            }
                                                            else
                                                            {
                                                                serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (subSubAgendaName[y].SerialNumberType != "Other")
                                                            {
                                                                objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                                                serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
                                                            }
                                                            else
                                                            {
                                                                serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                            }
                                                        }
                                                    }


                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><a href=ViewAgenda.aspx?file=" + subSubAgendaName[y].UplaodedAgenda + "  Target='_blank'> " + subSubAgendaName[y].AgendaName + "</a>");


                                                        //for (int pp = 0; pp <= agendaNames.Count() - 1; pp++)
                                                        //{
                                                        //    if (!strDeletedAgenda.Contains(agendaNames[pp].Trim()))
                                                        //    {
                                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[pp].Trim()) ? "Agenda.pdf" : agendaPdfs[pp].Trim();
                                                        //        //strMenu.Append("<br/> <a  href=ViewAgenda.aspx?file=" + agendaNames[pp].Trim() + " Target='_blank' >" + pdfName + " </a> ");
                                                        //        strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[pp].Trim().Substring(0, agendaNames[pp].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                        //    }
                                                        //}

                                                        bool SendPdfFile = false;
                                                        foreach (string str in agendaNames)
                                                        {
                                                            if (!strDeletedAgenda.Contains(str))
                                                            {
                                                                SendPdfFile = true;
                                                            }
                                                        }
                                                        string markAgenda = "";
                                                        string AgendaAccess = "";
                                                        if (objUsersAgendaIds.Contains(subSubAgendaName[y].AgendaId))
                                                        {
                                                            AgendaAccess = " checked='checked' ";
                                                        }
                                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                                        {
                                                            strAllAgenda.Append(subSubAgendaName[y].AgendaId + ",");
                                                            markAgenda = " <input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";
                                                        }
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + markAgenda + serialNoSub + subSubAgendaName[y].AgendaName + Presenter + "");

                                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                                        {
                                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                            strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string markAgenda = "";
                                                        string AgendaAccess = "";
                                                        if (objUsersAgendaIds.Contains(subSubAgendaName[y].AgendaId))
                                                        {
                                                            AgendaAccess = " checked='checked' ";
                                                        }
                                                        
                                                            strAllAgenda.Append(subSubAgendaName[y].AgendaId + ",");
                                                            markAgenda = " <input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> ";

                                                        

                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + markAgenda + serialNoSub + subSubAgendaName[y].AgendaName + Presenter);
                                                    }
                                                }
                                                strMenu.Append("</ol>");
                                            }
                                            else
                                            {
                                                //   strMenu.Append("</li>");
                                            }
                                            strMenu.Append("</li>");
                                        }
                                        strMenu.Append("</ol></div></li>");
                                    }
                                    else
                                    {
                                        strMenu.Append("</li>");
                                    }

                                }
                                strMenu.Append("</ol></div>");
                                ViewState["AllAgenda"] = strAllAgenda.ToString();
                                lblList.Text = strMenu.ToString();
                                btnUpdateStatus.Visible = true;
                            }
                            else
                            {
                                lblList.Text = "No agenda uploaded or approved";
                                btnUpdateStatus.Visible = false;
                            }

                            //  lblList.Text = strMenuList;
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                            btnUpdateStatus.Visible = false;
                        }
                    }

                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid Agenda search";
                        Error.Visible = true;
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
        /// View uploaded file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["Member"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.WriteFile(FilePath);
                Response.End();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMeeting.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please select Meeting";
                    Error.Visible = true;
                    return;
                }

                UserAcess objUser = new UserAcess();
                MarkedAgendaDomain objUserAgenda = new MarkedAgendaDomain();
                string AgendaIds = hdnAgenda.Value;
                string[] arrSelectedAgenda = AgendaIds.Split(',');

                string strAllAgenda = Convert.ToString(ViewState["AllAgenda"]);
                char c = ',';
                if(strAllAgenda.EndsWith(c.ToString()))
                {
                    strAllAgenda = strAllAgenda.Substring(0, strAllAgenda.Length - 1);
                }

                string[] arrAllAgenda = strAllAgenda.Split(',');

                string[] unReadArray = arrAllAgenda.Except(arrSelectedAgenda).ToArray();
                string strUnreadAgenda = string.Join(",", unReadArray); 

                objUserAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objUserAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objUserAgenda.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);

                StringBuilder sbAdd = new StringBuilder(",");
                StringBuilder sbDelete = new StringBuilder(",");

                //if (ViewState["AgendaIds"] != null)
                //{
                //    if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                //        Error.Visible = true;
                //        return;
                //    }
                //    // List<Guid> objAgendaIds = (List<Guid>)ViewState["AgendaIds"];
                //    List<string> objAgendaIds = (List<string>)ViewState["AgendaIds"];
                //    List<string> objRemovedAgendaIds = new List<string>();
                //    // objRemovedAgendaIds = objAgendaIds; 
                //    List<string> objInsertIds = hdnAgenda.Value.Split(',').ToList<string>();
                //    List<string> objDeleteList = objAgendaIds.Except(objInsertIds).ToList();
                //    List<string> objAddList = objInsertIds.Except(objAgendaIds).ToList();

                //    foreach (string objId in objAddList)
                //    {
                //        sbAdd.Append(objId.ToString() + ",");
                //    }

                //    foreach (string objId in objDeleteList)
                //    {
                //        sbDelete.Append(objId.ToString() + ",");
                //    }
                //    bool status = MarkedAgendaDataProvider.Instance.Update(objUserAgenda, sbAdd.ToString(), sbDelete.ToString());
                //    ((Label)Info.FindControl("lblName")).Text = "Agenda access updated sucessfully ";
                //    Info.Visible = true;
                //    ddlMeeting_SelectedIndexChanged(sender, e);
                    
                //}

                //else
                //{
                    if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                        return;
                    }
                    bool status = false;
                    if (AgendaIds != "")
                    {
                        AgendaIds = "," + AgendaIds + ",";
                    }

                    if (strUnreadAgenda != "")
                    {
                        strUnreadAgenda = "," + strUnreadAgenda + ",";
                    }
                        status = MarkedAgendaDataProvider.Instance.Insert(objUserAgenda, AgendaIds ,strUnreadAgenda);
                        ((Label)Info.FindControl("lblName")).Text = "Agenda status marked sucessfully ";
                        Info.Visible = true;
                        ddlMeeting_SelectedIndexChanged(sender, e);
                   
                    
                //}
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

    }
}