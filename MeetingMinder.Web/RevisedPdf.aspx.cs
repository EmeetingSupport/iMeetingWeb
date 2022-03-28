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
    public partial class RevisedPdf : System.Web.UI.Page
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
                //  ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                #endregion



                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

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
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting > dtToday)
                        //{
                        ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));

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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId);
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
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
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
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        IList<RevisedPdfDomain> objPdf = RevisedPdfDataProvider.Instance.Get(meetingId);

                        if (objAgentList.Count > 0)
                        {
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
                                                  UplaodedAgenda = agend.UploadedAgendaNote

                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                strMenu.Append("<div id='accordion'><ol id='sort'>");
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
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote
                                                         })).ToList();

                                    List<RevisedPdfDomain> objRev = (from RevAgenda in objPdf where (RevAgenda.AgendaId == agendaId) select RevAgenda).ToList<RevisedPdfDomain>();
                                    StringBuilder sbRevPdf = new StringBuilder("");
                                    if (objRev.Count() > 0)
                                    {
                                        sbRevPdf = new StringBuilder("");
                                        //sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Revised Agenda</th><th>Initial Pdf</th><th>Revised Pdf</th><th>Time</th></tr></thead>");
                                        sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Deleted Pdf</th><th>Time</th></tr></thead>");

                                        for (int a = 0; a <= objRev.Count() - 1; a++)
                                        {
                                            //sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td>" + objRev[a].AgendaNew + "</td><td> <a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_Old + " Target='_blank'> " + objRev[a].PdfOldName + "</a> </td><td><a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_New + " Target='_blank'> " + objRev[a].Pdf_NewName + "</a></td><td>" + objRev[a].CreateOn + "</td></tr>");
                                            //-Commentd for adding new link-//sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_New + " Target='_blank'> " + objRev[a].Pdf_NewName + "</a></td><td>" + objRev[a].CreateOn + "</td></tr>");
                                            sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  href='meeting_agenda-" + HttpUtility.UrlEncode(objRev[a].Pdf_New.Trim().Substring(0, objRev[a].Pdf_New.Trim().Length - 4)) + ".aspx' Target='_blank'> " + objRev[a].Pdf_NewName + " </a></td><td>" + objRev[a].CreateOn + "</td></tr>");                                             
                                        }
                                        sbRevPdf.Append("</table></div>");
                                    }
                                    else
                                    {
                                        sbRevPdf = new StringBuilder("");
                                    }
                                    if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3> " + agendaName[i].AgendaName + " </h3> " + sbRevPdf.ToString() + "");
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + agendaName[i].AgendaName + "</h3> " + sbRevPdf.ToString());
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {

                                            objRev = (from RevAgenda in objPdf where (RevAgenda.AgendaId == subAgendaName[j].AgendaId) select RevAgenda).ToList<RevisedPdfDomain>();

                                            if (objRev.Count() > 0)
                                            {
                                                sbRevPdf = new StringBuilder("");
                                                //sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Revised Agenda</th><th>Initial Pdf</th><th>Revised Pdf</th><th>Time</th></tr></thead>");
                                                sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Deleted Pdf</th><th>Time</th></tr></thead>");
                                                for (int a = 0; a <= objRev.Count() - 1; a++)
                                                {
                                                    //sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td>" + objRev[a].AgendaNew + "</td><td> <a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_Old + " Target='_blank'> " + objRev[a].PdfOldName + "</a> </td><td><a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_New + " Target='_blank'> " + objRev[a].Pdf_NewName + "</a></td><td>" + objRev[a].CreateOn + "</td></tr>");
                                                   //-comented for new adding new link to downlaod-// sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_New + " Target='_blank'> " + objRev[a].Pdf_NewName + "</a></td><td>" + objRev[a].CreateOn + "</td></tr>");
                                                    sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  href='meeting_agenda-" + HttpUtility.UrlEncode(objRev[a].Pdf_New.Trim().Substring(0, objRev[a].Pdf_New.Trim().Length - 4)) + ".aspx'  Target='_blank'>  " + objRev[a].Pdf_NewName + "  </a></td><td>" + objRev[a].CreateOn + "</td></tr>");

                                                }
                                                sbRevPdf.Append("</table></div>");
                                            }
                                            else
                                            {
                                                sbRevPdf = new StringBuilder("");
                                            }

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + sbRevPdf.ToString() + "");
                                            }
                                            else
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + ">" + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + sbRevPdf.ToString());// + "</li>");
                                            }
                                            Guid subAgendaId = subAgendaName[j].AgendaId;
                                            //Get sub sub agenda
                                            var subSubAgendaName = (from subAgenda in subAgendaList
                                                                    where (subAgenda.ParentAgendaId == subAgendaId)
                                                                    select (new
                                                                    {
                                                                        AgendaName = subAgenda.AgendaName,
                                                                        AgendaId = subAgenda.AgendaId,
                                                                        UplaodedAgenda = subAgenda.UploadedAgendaNote
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {


                                                //attach sub sub agenda to parent agenda list element
                                                strMenu.Append("<ol class=inddrag  style='list-style: upper-alpha outside none;margin-left:50px;'>");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    objRev = (from RevAgenda in objPdf where (RevAgenda.AgendaId == subSubAgendaName[y].AgendaId) select RevAgenda).ToList<RevisedPdfDomain>();

                                                    if (objRev.Count() > 0)
                                                    {
                                                        sbRevPdf = new StringBuilder("");
                                                        //sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Revised Agenda</th><th>Initial Pdf</th><th>Revised Pdf</th><th>Time</th></tr></thead>");
                                                        sbRevPdf.Append("<div><table class='datatable'><thead><tr><th>Initial Agenda</th><th>Deleted Pdf</th><th>Time</th></tr></thead>");

                                                        for (int a = 0; a <= objRev.Count() - 1; a++)
                                                        {
                                                            //-commented for new link adding-//sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  href=ViewAgenda.aspx?file=" + objRev[a].Pdf_New + " Target='_blank'> " + objRev[a].Pdf_NewName + "</a></td><td>" + objRev[a].CreateOn + "</td></tr>");
                                                            sbRevPdf.Append("<tr><td>" + objRev[a].AgendaOld + "</td><td><a  <a  href='meeting_agenda-" + HttpUtility.UrlEncode(objRev[a].Pdf_New.Trim().Substring(0, objRev[a].Pdf_New.Trim().Length - 4)) + ".aspx'  Target='_blank'>  " + objRev[a].Pdf_NewName + " </a></td><td>" + objRev[a].CreateOn + "</td></tr>");                                                            
                                                        }
                                                        sbRevPdf.Append("</table></div>");
                                                    }
                                                    else
                                                    {
                                                        sbRevPdf = new StringBuilder("");
                                                    }
                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + subSubAgendaName[y].AgendaName + sbRevPdf.ToString() + " ");
                                                    }
                                                    else
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + subSubAgendaName[y].AgendaName + sbRevPdf.ToString());
                                                    }
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
                            lblList.Visible = true;
                            //  lblList.Text = strMenuList;
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";

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

    }
}