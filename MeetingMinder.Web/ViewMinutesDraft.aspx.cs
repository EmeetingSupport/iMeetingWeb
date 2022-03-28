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
using System.Web.Services;


namespace MeetingMinder.Web
{
    public partial class ViewMinutesDraft : System.Web.UI.Page
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
                #endregion

                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

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
             //   btnPrint.Visible = false;
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;
                        hdnMeeting.Value = strMeetingId;
                        StringBuilder strMenu = new StringBuilder("");
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendaForMinutes(meetingId);

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
                                                  UplaodedAgenda = agend.UploadedAgendaNote,
                                                  Minute = agend.Minutes

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
                                                             UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                             Minute = subAgenda.Minutes
                                                         })).ToList();
                                    string strOnclick = "ShowPopUp(" + agendaName[i].AgendaName + ", " + agendaId + ", " + agendaName[i].Minute + ")";
                                    if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + agendaName[i].AgendaName + "  </h3><br><span>" + agendaName[i].Minute + "</span>");
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + agendaName[i].AgendaName + " </h3> <br><span>" + agendaName[i].Minute + "</span>");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + " <br><span>" + subAgendaName[j].Minute + "</span>");
                                            }
                                            else
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + ">" + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + "<br><span>" + subAgendaName[j].Minute + "</span> ");// + "</li>");
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
                                                                        Minute = subAgenda.Minutes
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                //attach sub sub agenda to parent agenda list element
                                                strMenu.Append("<ol class=inddrag  style='list-style: upper-alpha outside none;margin-left:50px;'>");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + subSubAgendaName[y].AgendaName + "<br><span>" + subSubAgendaName[y].Minute + "</span>");
                                                    }
                                                    else
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + subSubAgendaName[y].AgendaName + "<br><span>" + subSubAgendaName[y].Minute + "</span>");
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
                        //    btnPrint.Visible = true;

                            //  lblList.Text = strMenuList;
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";
                          //  btnPrint.Visible = false;
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

        [WebMethod]
        public static string AddMinutes(string Meeting, string Agenda, string Minutes)
        {
            string strResponse = "";
            try
            {
                MinutesDomain objMintes = new MinutesDomain();
                objMintes.AgendaId = Guid.Parse(Agenda);
                objMintes.MeetingId = Guid.Parse(Meeting);
                objMintes.Minutes = Minutes;
                objMintes.CreatedBy = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                objMintes.UpdatedBy = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                objMintes = MinutesDataProvider.Instance.MinuteOperation(objMintes);
                if (objMintes.MinuteId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    strResponse = "Minutes added successfully";
                }
                else
                {
                    strResponse = "Minutes addition failed ";
                }
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;
                strResponse = "Try again after some time";
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
            return strResponse;
        }

    }
}