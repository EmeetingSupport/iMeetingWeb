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
using System.Text.RegularExpressions;
//using Microsoft.Office.Interop.Word;

namespace MeetingMinder.Web
{
    public partial class ViewAgenda : System.Web.UI.Page
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
            for (int i = 2012; i <= currentYear + 1; i++)
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
                btnPrint.Visible = false;
                btnExport.Visible = false;
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
                    if (ddlYear.SelectedValue != "0")
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

        protected void ddlMeeting_SelectedIndexChanged_old_25Nov2016(object sender, EventArgs e)
        {
            try
            {
                lblList.Text = "";
                lblList.Visible = true;
                btnPrint.Visible = false;
                btnExport.Visible = false;
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        if (objAgentList.Count > 0)
                        {
                            //MeetingDomain objMeet = new MeetingDomain();
                            hdnMeeting.Value = objAgentList[0].MeetingVenue + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " " + objAgentList[0].MeetingTime;
                            hdnMeetingNumber.Value = objAgentList[0].MeetingNumber;
                            hdnMeetingDate.Value = Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd/MM/yyyy");
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
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
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
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
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
                                    //for (int i = 0; i < agendaName.Count(); i++)
                                    //{
                                    strAgendaNames.Append("<label><b>" + SerialNumber.Replace(':', ' ').TrimEnd(' ') + ". 	" + agendaName[i].AgendaName + " </b></label> <br /> ");
                                    //}
                                    hdnAgendaNames.Value = strAgendaNames.ToString();

                                    if (agendaName[i].UplaodedAgenda.Length > 0)
                                    {
                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");

                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");

                                        //for (int ii = 0; ii <= agendaNames.Count() - 1; ii++)
                                        //{
                                        //    if (!strDeletedAgenda.Contains(agendaNames[ii].Trim()))
                                        //    {
                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ii].Trim()) ? "Agenda.pdf" : agendaPdfs[ii].Trim();
                                        //        //    strMenu.Append("<a  href=ViewAgenda.aspx?file=" + agendaNames[ii].Trim() + " Target='_blank' >" + pdfName + " </a>  <br />  ");
                                        //        strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ii].Trim().Substring(0, agendaNames[ii].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a>  <br />  ");
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
                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                        {
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
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

                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter + "");
                                                //for (int ll = 0; ll <= agendaNames.Count() - 1; ll++)
                                                //{
                                                //    if (!strDeletedAgenda.Contains(agendaNames[ll].Trim()))
                                                //    {
                                                //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ll].Trim()) ? "Agenda.pdf" : agendaPdfs[ll].Trim();
                                                //        strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ll].Trim().Substring(0, agendaNames[ll].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                //    }

                                                //}
                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                }
                                            }
                                            else
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
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
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
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

                                                    //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                                    //{
                                                    //    if (objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                                    //    {
                                                    //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                                    //        {
                                                    //            objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
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
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                    //        }
                                                    //    }
                                                    //}


                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><a href=ViewAgenda.aspx?file=" + subSubAgendaName[y].UplaodedAgenda + "  Target='_blank'> " + subSubAgendaName[y].AgendaName + "</a>");

                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter + "");

                                                        //for (int pp = 0; pp <= agendaNames.Count() - 1; pp++)
                                                        //{
                                                        //    if (!strDeletedAgenda.Contains(agendaNames[pp].Trim()))
                                                        //    {
                                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[pp].Trim()) ? "Agenda.pdf" : agendaPdfs[pp].Trim();
                                                        //        //strMenu.Append(" <br />  <a  href=ViewAgenda.aspx?file=" + agendaNames[pp].Trim() + " Target='_blank' >" + pdfName + " </a> ");
                                                        //        strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[pp].Trim().Substring(0, agendaNames[pp].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
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
                                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                                        {
                                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                            strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
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

                            }
                            lblList.Text = strMenu.ToString();
                            btnPrint.Visible = true;

                            //  lblList.Text = strMenuList;
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                            btnPrint.Visible = false;
                        }


                        if (objAgentList.Count > 0)
                        {

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
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;

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
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();


                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber;
                                    }
                                    SerialNumber = SerialNumber.Replace(':', ' ');
                                    strAllAgenda.Append("<p><b>" + SerialNumber + " <span style='text-decoration:underline;'>" + agendaName[i].AgendaName + "</span></b></p>");




                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();

                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            //Presenter = "";
                                            string serialNo = "";

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

                                            strAllAgenda.Append("<p>" + SerialNumber.TrimEnd(' ') + "-" + serialNo + subAgendaName[j].AgendaName + "</p>");

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
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element

                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {

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

                                                    strAllAgenda.Append("<p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + serialNoSub + subSubAgendaName[y].AgendaName + "</p>");

                                                }

                                            }


                                        }

                                    }

                                }

                            }

                            hdnAllAgenda.Value = strAllAgenda.ToString();
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

        protected void ddlMeeting_SelectedIndexChanged11(object sender, EventArgs e)
        {
            try
            {
                lblList.Text = "";
                lblList.Visible = true;
                btnPrint.Visible = false;
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        if (objAgentList.Count > 0)
                        {

                            hdnMeeting.Value = objAgentList[0].MeetingVenue + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " " + objAgentList[0].MeetingTime;
                            hdnMeetingNumber.Value = objAgentList[0].MeetingNumber;
                            hdnMeetingDate.Value = Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd/MM/yyyy");

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
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";

                                strMenu.Append(@" <br /> <div class='divPrint' style='margin:15px;'>
        <div  style='font-size: 11px;'   align='right'><b><div style='font-size: 11pt;'>" + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" Meeting No. " + objAgentList[0].MeetingNumber + @" dated " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd MMM yyyy") + @"</div></b></div>
         <br /> 
        <div  align='right' ><b><u style='font-size: 14pt;'>Private & Confidential</u></b></div>
         <br /> 
        <div style='width:100%;' align='center'>
            <h3><b><div style='font-size: 24pt;font-weight: normal;text-align: center;text-transform: uppercase;' >" + Session["EntityName"].ToString() + @" </div> </b></h3>
           <div style='font-size: 14pt;'> Agenda for the meeting of the " + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" to be held at " + objAgentList[0].MeetingVenue + @" on <u>" + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + @" at " + objAgentList[0].MeetingTime + @"
        </u></div></div>
         <br /> 
         <div>
            <b>
                The Agenda items are arranged as per the following format:</b>  <br />  ");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
                                }

                                strMenu.Append(@" </div>  <br />  <div style='width:100%;'>");

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
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }

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

                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ".  <u>" + agendaName[i].AgendaName + "</u></b> <br /> ");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");



                                        bool SendPdfFile = false;
                                        foreach (string str in agendaNames)
                                        {
                                            if (!strDeletedAgenda.Contains(str))
                                            {
                                                SendPdfFile = true;
                                            }
                                        }
                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                        {
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append(" <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a><br/> ");
                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ". <u>" + agendaName[i].AgendaName + "</u></b><br/>");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }

                                    strMenu.Append(" <br /> ");

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        strMenu.Append("<table  cellspacing='0' cellpadding='0' border='0' style='border-width:0;border-collapse: collapse;border: 0px white solid;width:100%'>");
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

                                                strMenu.Append(@"<tr style='border: 1px white solid;'><td style='border: 1px white solid;width:12%; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</td><td style='border: 1px white solid;width:3%;'></td><td style='border: 1px white solid;width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter + "");

                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> <br/>");
                                                }
                                            }
                                            else
                                            {
                                                strMenu.Append(@"<tr  style='border: 1px white solid;'><td style='border: 1px white solid;width:12%; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</td><td style='border: 1px white solid;width:3%;'></td><td style='border: 1px white solid;width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
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
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                strMenu.Append(" <table class='subAgenda' cellspacing='0' cellpadding='0' border='0' style='border-width:0;border-collapse: collapse;border: 1px white solid;width:100%' >");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";



                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');

                                                        // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter + "");
                                                        strMenu.Append("<tr style='border: 1px white solid;'> <td style='border: 1px white solid;width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='border: 1px white solid;width:3%;'></td>  <td style='border: 1px white solid;width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);


                                                        bool SendPdfFile = false;
                                                        foreach (string str in agendaNames)
                                                        {
                                                            if (!strDeletedAgenda.Contains(str))
                                                            {
                                                                SendPdfFile = true;
                                                            }
                                                        }
                                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                                        {
                                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                            strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
                                                        strMenu.Append("<tr  style='border: 1px white solid;'> <td style='border: 1px white solid;width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='border: 1px white solid;width:3%;'></td>  <td style='border: 1px white solid;width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);
                                                    }
                                                    strMenu.Append(" </td></tr>");
                                                }
                                                // strMenu.Append("</ol>");
                                                strMenu.Append("</table>");

                                            }
                                            else
                                            {
                                                strMenu.Append(" </td>  </tr>");
                                            }
                                            //strMenu.Append("</li>");
                                            strMenu.Append(@"</td></tr>");
                                        }
                                        //strMenu.Append("</ol></div></li>");
                                        strMenu.Append("<table> <br />  <br /> ");

                                    }
                                    else
                                    {
                                        // strMenu.Append("</li>");
                                    }

                                }
                                //     strMenu.Append("</ol></div>");
                                strMenu.Append(@" </div> <div> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
        <table style='width:100%'>
            <tr>
                <td style='width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px; margin-left: -8px;'><b>MUMBAI, (date)</b> </div></td>
                <td style='width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
            </tr>
        </table></div>");

                            }
                            lblList.Text = strMenu.ToString();
                            btnPrint.Visible = true;

                            var strBody = new StringBuilder();

                            strBody.Append("<html " +
                             "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                             "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                              "xmlns='http://www.w3.org/TR/REC-html40'>" +
                              "<head><title>Time</title>");


                            strBody.Append("<style>" +
                             "<!-- /* Style Definitions */" +
                             "@page Section1" +
                             "   {size:8.5in 11.0in; " +
                             "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                             "   mso-header-margin:.5in; " +
                             "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                             " div.Section1" +
                             "   {page:Section1;}" +
                             "-->" +
                             "</style></head>");

                            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                             "<div class=Section1>");
                            strBody.Append("<style type='text/css'> .divPrint b {font-size: 14pt !important;}  .divPrint,.divPrint table,.subAgenda td{color:#000!important;font-family:times new roman}#accordion li{margin-left:20px;list-style:none}table tr td,table tr th{border:none!important}.tab tr:last-child>td{border:1px solid #000!important}.divPrint,.divPrint table{font-size:14pt}.subAgenda td{font-size:11pt!important}b{font-weight:700!important}</style>");
                            strBody.Append(strMenu.ToString());
                            strBody.Append("</div></body></html>");

                            //Force this content to be downloaded 
                            //as a Word document with the name of your choice
                            Response.AppendHeader("Content-Type", "application/msword");
                            Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");

                            Response.Write(strBody.ToString());
                            Response.End();
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                            btnPrint.Visible = false;
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


        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblList.Text = "";
                lblList.Visible = true;
                btnPrint.Visible = false;
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        if (objAgentList.Count > 0)
                        {

                            hdnMeeting.Value = objAgentList[0].MeetingVenue + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " " + objAgentList[0].MeetingTime;
                            hdnMeetingNumber.Value = objAgentList[0].MeetingNumber;
                            hdnMeetingDate.Value = Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd/MM/yyyy");

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
                                                  AgendaName = agend.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                  AgendaId = agend.AgendaId,
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";

                                strMenu.Append(@" <br /> <div class='divPrint' style='margin:15px;'>
        <div  style='font-size: 11px;'   align='right'><b><div style='font-size: 11pt;'>" + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" Meeting No. " + objAgentList[0].MeetingNumber + @" dated " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd MMM yyyy") + @"</div></b></div>
         <br /> 
        <div  align='right' ><b><u style='font-size: 14pt;'>Private & Confidential</u></b></div>
         <br /> 
        <div style='width:100%;' align='center'>
            <h3><b><div style='font-size: 24pt;font-weight: normal;text-align: center;text-transform: uppercase;' >" + Session["EntityName"].ToString() + @" </div> </b></h3>
           <div style='font-size: 14pt;'> Agenda for the meeting of the " + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" to be held at " + objAgentList[0].MeetingVenue + @" on <u>" + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + @" at " + objAgentList[0].MeetingTime + @"
        </u></div></div>
         <br /> 
         <div>
            <b>
                The Agenda items are arranged as per the following format:</b>  <br />  ");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
                                }

                                strMenu.Append(@" </div>  <br />  <div style='width:100%;'>");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;
                                    ClassificationNew = agendaName[i].Classifications;
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                             AgendaId = subAgenda.AgendaId,
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }

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

                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ".  <u>" + agendaName[i].AgendaName + "</u></b> <br /> ");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");



                                        bool SendPdfFile = false;
                                        foreach (string str in agendaNames)
                                        {
                                            if (!strDeletedAgenda.Contains(str))
                                            {
                                                SendPdfFile = true;
                                            }
                                        }
                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                        {
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append(" <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a><br/> ");
                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<b>" + agendaName[i].SerialNumber + ". <u>" + agendaName[i].AgendaName + "</u></b><br/>");
                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }

                                    strMenu.Append(" <br /> ");

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        strMenu.Append("<table>");
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

                                                strMenu.Append(@"<tr><td style='width:12%; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</td><td style='width:3%;'></td><td style='width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter + "");

                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> <br/>");
                                                }
                                            }
                                            else
                                            {
                                                strMenu.Append(@"<tr><td style='width:12%; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</td><td style='width:3%;'></td><td style='width:87%;'> " + subAgendaName[j].AgendaName);
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                            }

                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                strMenu.Append(" <table class='subAgenda' cellspacing='0' >");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";



                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');

                                                        // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter + "");
                                                        strMenu.Append("<tr> <td style='width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='width:3%;'></td>  <td style='width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);


                                                        bool SendPdfFile = false;
                                                        foreach (string str in agendaNames)
                                                        {
                                                            if (!strDeletedAgenda.Contains(str))
                                                            {
                                                                SendPdfFile = true;
                                                            }
                                                        }
                                                        if (agendaNames.Count() > 0 && SendPdfFile)
                                                        {
                                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                            strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
                                                        strMenu.Append("<tr> <td style='width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='width:3%;'></td>  <td style='width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);
                                                    }
                                                    strMenu.Append(" </td></tr>");
                                                }
                                                // strMenu.Append("</ol>");
                                                strMenu.Append("</table>");

                                            }
                                            else
                                            {
                                                strMenu.Append(" </td>  </tr>");
                                            }
                                            //strMenu.Append("</li>");
                                            strMenu.Append(@"</td></tr>");
                                        }
                                        //strMenu.Append("</ol></div></li>");
                                        strMenu.Append("<table> <br />  <br /> ");

                                    }
                                    else
                                    {
                                        // strMenu.Append("</li>");
                                    }

                                }
                                //     strMenu.Append("</ol></div>");


                                //commented on 9July19
                                strMenu.Append(@" </div>");
                                //                                strMenu.Append(@" </div> <div> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
                                //        <table style='width:100%'>
                                //            <tr>
                                //                <td style='width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
                                //<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px; margin-left: -8px;'><b>MUMBAI, (date)</b> </div></td>
                                //                <td style='width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
                                //            </tr>
                                //        </table></div>");

                                //end
                            }
                            lblList.Text = strMenu.ToString();
                            btnPrint.Visible = true;
                            btnExport.Visible = true;
                            //btnExport_Click(sender, e);
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                            btnPrint.Visible = false;
                            btnExport.Visible = false;
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

        protected void btnExport_Click_HTMLExport(object sender, EventArgs e)
        {

            try
            {
                lblList.Text = "";
                lblList.Visible = true;

                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        StringBuilder strAgendaNames = new StringBuilder();
                        StringBuilder strAllAgenda = new StringBuilder();
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        if (objAgentList.Count > 0)
                        {

                            hdnMeeting.Value = objAgentList[0].MeetingVenue + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " " + objAgentList[0].MeetingTime;
                            hdnMeetingNumber.Value = objAgentList[0].MeetingNumber;
                            hdnMeetingDate.Value = Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd/MM/yyyy");

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
                                                  AgendaName = agend.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                  AgendaId = agend.AgendaId,
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";

                                strMenu.Append(@" <br /> <div class='divPrint' style='margin:15px;'>
        <div  style='font-size: 11px;'   align='right'><b><div style='font-size: 11pt;'>" + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" Meeting No. " + objAgentList[0].MeetingNumber + @" dated " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd MMM yyyy") + @"</div></b></div>
         <br /> 
        <div  align='right' ><b><u style='font-size: 14pt;'>Private & Confidential</u></b></div>
         <br /> 
        <div style='width:100%;' align='center'>
            <h3><b><div style='font-size: 24pt;font-weight: normal;text-align: center;text-transform: uppercase;' >" + Session["EntityName"].ToString().ToUpper() + @" </div> </b></h3>
           <div style='font-size: 14pt;'> Agenda for the meeting of the " + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" to be held at " + objAgentList[0].MeetingVenue + @" on <u>" + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + @" at " + objAgentList[0].MeetingTime + @"
        </u></div></div>
         <br /> 
         <div>
            <b style='font-family:times new roman;font-size: 14pt;'>
                The Agenda items are arranged as per the following format:</b>  <br />  ");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    strMenu.Append("<b  style='font-family:times new roman;font-size: 14pt;'>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
                                }

                                strMenu.Append(@" </div>  <br />  <div style='width:100%;font-family:times new roman;font-size: 14pt;'>");

                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;
                                    ClassificationNew = agendaName[i].Classifications;
                                    //get sub agenda for parent agenda 
                                    var subAgendaName = (from subAgenda in subAgendaList
                                                         where (subAgenda.ParentAgendaId == agendaId)
                                                         select (new
                                                         {
                                                             AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                             AgendaId = subAgenda.AgendaId,
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }

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



                                    //if (agendaName[i].UplaodedAgenda.Length > 0)
                                    //{
                                    //    string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                    //    string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                    //    string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                    //    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".  <u>" + agendaName[i].AgendaName + "</u></b> <br /> ");
                                    //    //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");



                                    //    bool SendPdfFile = false;
                                    //    foreach (string str in agendaNames)
                                    //    {
                                    //        if (!strDeletedAgenda.Contains(str))
                                    //        {
                                    //            SendPdfFile = true;
                                    //        }
                                    //    }
                                    //    if (agendaNames.Count() > 0 && SendPdfFile)
                                    //    {
                                    //        string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                    //        strMenu.Append(" <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a></br> ");
                                    //    }
                                    //}
                                    //else
                                    //{
                                    strMenu.Append("<b style='font-family:times new roman;font-size: 14pt;'>" + agendaName[i].SerialNumber + ". <u style='font-family:times new roman;font-size: 14pt;'>" + agendaName[i].AgendaName + "</u></b><br/>");
                                    //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    //}

                                    strMenu.Append(" <br /> ");

                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        // strMenu.Append("<table>");
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

                                            //if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            //{
                                            //    string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                            //    string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                            //    string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                            //    bool SendPdfFile = false;
                                            //    foreach (string str in agendaNames)
                                            //    {
                                            //        if (!strDeletedAgenda.Contains(str))
                                            //        {
                                            //            SendPdfFile = true;
                                            //        }
                                            //    }

                                            //    strMenu.Append(@" <div style='width:12%;float: left;width: 100px;height: 20px; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</div> <div style='margin-left: 108px;width:87%;'> " + subAgendaName[j].AgendaName);
                                            //    //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter + "");

                                            //    if (agendaNames.Count() > 0 && SendPdfFile)
                                            //    {
                                            //        string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            //        strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> </br>");
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //   strMenu.Append(@" <div style='vertical-align: top; width: 100px;height: 20px;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</div> <div style='margin-left: 108px;width:87%;'> " + subAgendaName[j].AgendaName);
                                            string subAgendaTitle = subAgendaName[j].AgendaName;
                                            if (subAgendaName[j].AgendaName.Length > 57)
                                            {
                                                string len = "57";
                                                subAgendaTitle = Regex.Replace(subAgendaTitle, ".{" + len + "}", "$0 &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;");
                                            }
                                            strMenu.Append(@" <div style='vertical-align: top;text-align:justify;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "&emsp;&emsp;&emsp;&emsp;" + subAgendaTitle);

                                            //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                            // }

                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                //  strMenu.Append(" <table class='subAgenda' cellspacing='0' >");

                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";



                                                    //if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    //{
                                                    //    string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                    //    string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                    //    string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');

                                                    //    // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter + "");
                                                    //    strMenu.Append("  <div style='width:8%;padding:0;margin:0;vertical-align:top;float: left;width: 100px;height: 20px;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + "  <div style='width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);


                                                    //    bool SendPdfFile = false;
                                                    //    foreach (string str in agendaNames)
                                                    //    {
                                                    //        if (!strDeletedAgenda.Contains(str))
                                                    //        {
                                                    //            SendPdfFile = true;
                                                    //        }
                                                    //    }
                                                    //    if (agendaNames.Count() > 0 && SendPdfFile)
                                                    //    {
                                                    //        string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    //        strMenu.Append(" <br />  <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
                                                    string subSubAgendaTitle = subSubAgendaName[y].AgendaName;
                                                    if (subSubAgendaName[y].AgendaName.Length > 54)
                                                    {
                                                        string len = "54";
                                                        subSubAgendaTitle = Regex.Replace(subSubAgendaTitle, ".{" + len + "}", "$0 &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;");
                                                    }
                                                    strMenu.Append(" <div style='padding:0;margin-left:180px;vertical-align:top;font-family:times new roman;font-size: 11pt;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + subSubAgendaTitle);
                                                    //}
                                                    strMenu.Append(" </div> ");
                                                }
                                                // strMenu.Append("</ol>");
                                                //  strMenu.Append("</table>");
                                                strMenu.Append(" </div> ");
                                            }
                                            else
                                            {
                                                strMenu.Append(" </div>  ");
                                            }
                                            //strMenu.Append("</li>");

                                        }

                                        //strMenu.Append("</ol></div></li>");
                                        // strMenu.Append("<table> <br />  <br /> ");

                                    }
                                    else
                                    {
                                        // strMenu.Append("</li>");
                                    }

                                }
                                // strMenu.Append(@"</div>");
                                //     strMenu.Append("</ol></div>");
                                strMenu.Append(@" </div> <div style='font-family:times new roman;font-size:14pt;'> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
        </div> <div style='width: 100%;font-size:14pt;font-family:times new roman;'>
          Central Board Secretariat  &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; <b style='font-family:times new roman;font-size:14pt;'>     General Manager &amp; </b> 
</div>    <div style='width: 100%;font-size:14pt;font-family:times new roman;'>
        Corporate Centre  &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp; <b style='font-family:times new roman;font-size:14pt;'>   Secretary, Central Board   </b>
    </div>   
    <br />
   <div style='width: 100%;'> <b style='font-family:times new roman;font-size: 14pt;'>MUMBAI, (date)</b>    </div>   ");

                            }
                            //  lblList.Text = strMenu.ToString();
                            btnPrint.Visible = true;

                            var strBody = new StringBuilder();

                            strBody.Append("<html " +
                             "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                             "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                              "xmlns='http://www.w3.org/TR/REC-html40'>" +
                              "<head><title>Time</title>");


                            strBody.Append("<style>" +
                             "<!-- /* Style Definitions */" +
                             "@page Section1" +
                             "   {size:8.5in 11.0in; " +
                             "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                             "   mso-header-margin:.5in; " +
                             "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                             " div.Section1" +
                             "   {page:Section1;}" +
                             "-->" +
                             "</style></head>");

                            strBody.Append("<body lang=EN-US style='tab-interval:.5in'>" +
                             "<div class=Section1>");
                            strBody.Append("<style type='text/css'>  .divPrint b {font-size: 14pt !important;}  .divPrint,.divPrint table,.subAgenda td{color:#000!important;font-family:times new roman}#accordion li{margin-left:20px;list-style:none}table tr td,table tr th{border:none!important}.tab tr:last-child>td{border:1px solid #000!important}.divPrint,.divPrint table{font-size:14pt}.subAgenda td{font-size:11pt!important}b{font-weight:700!important}</style>");
                            strBody.Append(strMenu.ToString());
                            strBody.Append("</div></body></html>");

                            //Force this content to be downloaded 
                            //as a Word document with the name of your choice
                            Response.AppendHeader("Content-Type", "application/msword");
                            Response.AppendHeader("Content-disposition", "attachment; filename=myword.doc");

                            Response.Write(strBody.ToString());
                            Response.End();
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                            btnPrint.Visible = false;
                            btnExport.Visible = false;
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

//        protected void btnExport_Click(object sender, EventArgs e)
//        {


//            try
//            {
               
//                string strMeetingId = ddlMeeting.SelectedValue;
//                if (strMeetingId != "0")
//                {
//                    Guid meetingId;
//                    if (Guid.TryParse(strMeetingId, out meetingId))
//                    {
//                        ViewState["MeetingId"] = meetingId;

//                        StringBuilder strMenu = new StringBuilder("");
//                        StringBuilder strAgendaNames = new StringBuilder();
//                        StringBuilder strAllAgenda = new StringBuilder();
//                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

//                        if (objAgentList.Count > 0)
//                        {

//                            hdnMeeting.Value = objAgentList[0].MeetingVenue + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " " + objAgentList[0].MeetingTime;
//                            hdnMeetingNumber.Value = objAgentList[0].MeetingNumber;
//                            hdnMeetingDate.Value = Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd/MM/yyyy");

//                            //get only parent agenda
//                            var objParentAgenda = from agend in objAgentList
//                                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
//                                                  select agend;



//                            //get only sub agenda
//                            var subAgendaList = from agend in objAgentList
//                                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
//                                                select agend;

//                            //get agenda name/note and agenda id
//                            var agendaName = (from agend in objParentAgenda

//                                              select (new
//                                              {
//                                                  AgendaName = agend.AgendaName.Replace(Environment.NewLine, "<BR/>"),
//                                                  AgendaId = agend.AgendaId,
//                                                  UplaodedAgenda = agend.UploadedAgendaNote,
//                                                  AgendaNote = agend.AgendaNote,
//                                                  DeletedAgenda = agend.DeletedAgenda,
//                                                  Classifications = agend.Classification,
//                                                  SerialNumber = agend.SerialNumber,
//                                                  Presenter = agend.Presenter,
//                                                  SerialNumberType = agend.SerialNumberType,
//                                                  SerialTitle = agend.SerialTitle,
//                                                  SerialText = agend.SerialText
//                                              })).Distinct().ToList();

//                            if (agendaName.Count() > 0)
//                            {
//                                string ClassificationOld = "";
//                                string ClassificationNew = "";

//                                strMenu.Append(@" <br /> <div class='divPrint' style='margin:15px;'>
//        <div  style='font-size: 11px;'   align='right'><b><div style='font-size: 11pt;'>" + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" Meeting No. " + objAgentList[0].MeetingNumber + @" dated " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("dd MMM yyyy") + @"</div></b></div>
//         <br /> 
//        <div  align='right' ><b><u style='font-size: 14pt;'>Private & Confidential</u></b></div>
//         <br /> 
//        <div style='width:100%;' align='center'>
//            <h3><b><div style='font-size: 24pt;font-weight: normal;text-align: center;text-transform: uppercase;' >" + Session["EntityName"].ToString() + @" </div> </b></h3>
//           <div style='font-size: 14pt;'> Agenda for the meeting of the " + MM.Core.Encryptor.DecryptString(objAgentList[0].ForumName) + @" to be held at " + objAgentList[0].MeetingVenue + @" on <u>" + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + @" at " + objAgentList[0].MeetingTime + @"
//        </u></div></div>
//         <br /> 
//         <div>
//            <b>
//                The Agenda items are arranged as per the following format:</b>  <br />  ");

//                                for (int i = 0; i <= agendaName.Count() - 1; i++)
//                                {
//                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ".      " + agendaName[i].AgendaName + "</b>  <br /> ");
//                                }

//                                strMenu.Append(@" </div>  <br />  <div style='width:100%;'>");

//                                for (int i = 0; i <= agendaName.Count() - 1; i++)
//                                {
//                                    Guid agendaId = agendaName[i].AgendaId;
//                                    ClassificationNew = agendaName[i].Classifications;
//                                    //get sub agenda for parent agenda 
//                                    var subAgendaName = (from subAgenda in subAgendaList
//                                                         where (subAgenda.ParentAgendaId == agendaId)
//                                                         select (new
//                                                         {
//                                                             AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
//                                                             AgendaId = subAgenda.AgendaId,
//                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
//                                                             AgendaNote = subAgenda.AgendaNote,
//                                                             DeletedAgenda = subAgenda.DeletedAgenda,
//                                                             Presenter = subAgenda.Presenter,
//                                                             SerialNumber = subAgenda.SerialNumber,
//                                                             SerialNumberType = subAgenda.SerialNumberType,
//                                                             SerialTitle = subAgenda.SerialTitle,
//                                                             SerialText = subAgenda.SerialText
//                                                         })).ToList();
//                                    if (ClassificationOld != ClassificationNew)
//                                    {
//                                        ClassificationOld = ClassificationNew;
//                                        if (agendaName[i].Classifications != "")
//                                        {
//                                            // strMenu.Append(" < h2><b>" + agendaName[i].Classifications + "</b></h2>");
//                                        }

//                                    }

//                                    string Presenter = "";

//                                    if (agendaName[i].Presenter.Trim().Length > 0)
//                                    {
//                                        Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
//                                    }

//                                    string SerialNumber = "";
//                                    if (agendaName[i].SerialNumber.Length > 0)
//                                    {
//                                        SerialNumber = agendaName[i].SerialNumber + " : ";
//                                    }




//                                    strMenu.Append("<b>" + agendaName[i].SerialNumber + ". <u>" + agendaName[i].AgendaName + "</u></b><br/>");
//                                    //strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");


//                                    strMenu.Append(" <br /> ");

//                                    //attach sub agenda to parent agenda list element
//                                    if (subAgendaName.Count() > 0)
//                                    {
//                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
//                                        // strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
//                                        strMenu.Append("<table>");
//                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
//                                        {
//                                            Presenter = "";
//                                            string serialNo = "";

//                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
//                                            {
//                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
//                                            }

//                                            if (subAgendaName[j].SerialNumber.Trim().Length > 0)
//                                            {
//                                                if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
//                                                {
//                                                    if (subAgendaName[j].SerialNumberType != "Other")
//                                                    {
//                                                        objSerial[subAgendaName[j].SerialNumber] += 1;
//                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
//                                                    }
//                                                    else
//                                                    {
//                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
//                                                    }
//                                                }
//                                                else
//                                                {
//                                                    if (subAgendaName[j].SerialNumberType != "Other")
//                                                    {
//                                                        objSerial.Add(subAgendaName[j].SerialNumber, 1);
//                                                        serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
//                                                    }
//                                                    else
//                                                    {
//                                                        serialNo = subAgendaName[j].SerialNumber + " : ";
//                                                    }
//                                                }
//                                            }


//                                            strMenu.Append(@"<tr><td style='width:12%; vertical-align: top;'>" + CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + "</td><td style='width:3%;'></td><td style='width:87%;'> " + subAgendaName[j].AgendaName);
//                                            //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + CreateSubAgenda.GetTitles(subAgendaName[j].SerialText, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText) + subAgendaName[j].AgendaName + Presenter);// + "</li>");


//                                            Guid subAgendaId = subAgendaName[j].AgendaId;
//                                            //Get sub sub agenda
//                                            var subSubAgendaName = (from subAgenda in subAgendaList
//                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
//                                                                    select (new
//                                                                    {
//                                                                        AgendaName = subAgenda.AgendaName.Replace(Environment.NewLine, "<BR/>"),
//                                                                        AgendaId = subAgenda.AgendaId,
//                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote,
//                                                                        AgendaNote = subAgenda.AgendaNote,
//                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
//                                                                        Presenter = subAgenda.Presenter,
//                                                                        SerialNumber = subAgenda.SerialNumber,
//                                                                        SerialNumberType = subAgenda.SerialNumberType,
//                                                                        SerialTitle = subAgenda.SerialTitle,
//                                                                        SerialText = subAgenda.SerialText
//                                                                    })).ToList();
//                                            if (subSubAgendaName.Count() > 0)
//                                            {
//                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
//                                                //attach sub sub agenda to parent agenda list element
//                                                //   strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
//                                                strMenu.Append(" <table class='subAgenda' cellspacing='0' >");
//                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
//                                                {
//                                                    Presenter = "";

//                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
//                                                    {
//                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
//                                                    }

//                                                    string serialNoSub = "";

//                                                    //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialText, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + subSubAgendaName[y].AgendaName + Presenter);
//                                                    strMenu.Append("<tr> <td style='width:8%;padding:0;margin:0;vertical-align:top;'>  " + CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText) + " </td> <td style='width:3%;'></td>  <td style='width:60%;vertical-align: top;'>" + subSubAgendaName[y].AgendaName);

//                                                    //   strMenu.Append(" </td></tr>");
//                                                }
//                                                // strMenu.Append("</ol>");
//                                                strMenu.Append("</table>");

//                                            }
//                                            else
//                                            {
//                                                strMenu.Append(" </td>  </tr>");
//                                            }
//                                            //strMenu.Append("</li>");
//                                           // strMenu.Append(@"</td></tr>");
//                                        }
//                                        //strMenu.Append("</ol></div></li>");
//                                        strMenu.Append("</table> <br />  <br /> ");

//                                    }
//                                    else
//                                    {
//                                        // strMenu.Append("</li>");
//                                    }

//                                }
//                                //     strMenu.Append("</ol></div>");
//                                strMenu.Append(@" </div> <div> <br /> Any other item with the permission of the Chair. <br />  <br /> </div>
//        <table style='width:100%'>
//            <tr>
//                <td style='width:50%;vertical-align:top;text-align:left;line-height: 1;'> 
//<div>Central Board Secretariat</div><div>Corporate Centre </div><div style='margin-top: 12px;'><b>  MUMBAI, (date)</b> </div></td>
//                <td style='width:50%;vertical-align:top;text-align:right;' > <b>     General Manager & <br />  Secretary, Central Board</b></td>
//            </tr>
//        </table></div>");

//                            }
//                            //  lblList.Text = strMenu.ToString();

//                            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
//                            Microsoft.Office.Interop.Word.Document wordDoc = new Microsoft.Office.Interop.Word.Document();
//                            Object oMissing = System.Reflection.Missing.Value;
//                            wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
//                            word.Visible = false;
//                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
//                            string filePath = Server.MapPath(savePath) + Guid.NewGuid() + ".html";
//                            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
//                            StreamWriter writer = new StreamWriter(fs);
//                            writer.Write(strMenu.ToString());
//                            writer.Close();
//                            fs.Close();
//                            //    if(File.Exists())
//                            Object filepath = filePath;
//                            Object confirmconversion = System.Reflection.Missing.Value;
//                            Object readOnly = false;

//                            string fileDocSave = Server.MapPath(savePath) + Guid.NewGuid() + ".doc";
//                            Object saveto = fileDocSave;
//                            Object oallowsubstitution = System.Reflection.Missing.Value;

//                            wordDoc = word.Documents.Open(ref filepath, ref confirmconversion, ref readOnly, ref oMissing,
//                                                          ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                          ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                                          ref oMissing, ref oMissing, ref oMissing, ref oMissing);
//                            object fileFormat = WdSaveFormat.wdFormatDocument97;
//                            wordDoc.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
//                                           ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
//                                           ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing,
//                                           ref oMissing);
//                            wordDoc.Close();
//                            word.Quit();
//                            Response.AppendHeader("Content-Type", "application/msword");
//                            Response.AppendHeader("Content-Disposition",
//                                               "attachment; filename=Agenda.doc;");
//                            Response.WriteFile(fileDocSave);
//                            Response.Flush();
                         
//                            //    response.Flush();


//                            if (File.Exists(fileDocSave))
//                            {
//                                File.Delete(fileDocSave);
//                            }

//                            if (File.Exists(filePath))
//                            {
//                                File.Delete(filePath);
//                            }
//                            Response.End();
//                        }


//                    }


//                }
//            }
//            catch (Exception ex)
//            {
//                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
//                Error.Visible = true;

//                LogError objEr = new LogError();
//                objEr.HandleException(ex);

//            }
//        }
    }

}