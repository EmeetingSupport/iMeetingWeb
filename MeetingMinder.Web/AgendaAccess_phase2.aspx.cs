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
    public partial class AgendaAccess_phase2 : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                //Bind Entity List
                #region
                ///Comment for hide entity name
                BindEntity();

                BindUsers();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    //  BindForum(EntityId.ToString());
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

            }
        }

        /// <summary>
        /// Bind Users 
        /// </summary>
        private void BindUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";

                IList<UserDomain> objUserList = UserDataProvider.Instance.Get().OrderBy(p => p.FirstName).ToList();
                if (objUserList.Count > 0)
                {
                    DataTable dt = objUserList.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }
                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));

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
                        if (dtMeeting.AddDays(1).Date >= dtToday.Date)
                        {
                         //   ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                            ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName +' '+ dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));
                        }
                    }

                    if (ddlMeeting.Items.Count == 1)
                    {
                        ddlMeeting.Items.Clear();
                        ddlMeeting.Items.Insert(0, new ListItem("No upcoming meeting", "0"));
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

        /// <summary>
        /// drop down list  Selected Index Change event
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
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblList.Text = "";
                lblList.Visible = true;

                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0" && ddlUser.SelectedValue != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingIdForAccess(meetingId);

                        IList<UserAgendaDomain> objUserAgenda = UserAgendaDataProvider.Instance.GetUserAgendaByMeeting(Guid.Parse(ddlMeeting.SelectedValue), Guid.Parse(ddlUser.SelectedValue));

                        List<Guid> objUsersAgendaIds = new List<Guid>();
                        if (objUserAgenda.Count > 0)
                        {
                            objUsersAgendaIds = (from ids in objUserAgenda select ids.AgendaId).ToList<Guid>();
                            //ViewState["AgendaIds"] = objUsersAgendaIds;

                            IEnumerable<string> strVals = (from ids in objUserAgenda select ids.AgendaId.ToString());
                            hdnAgenda.Value = string.Join(",", strVals.ToArray());
                            ViewState["AgendaIds"] = strVals.ToList();
                            //  strVals.ToArray();
                            //hdnAgenda.Value = objUsersAgendaIds.ToString().ToArray();
                        }

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

                                    string AgendaAccess = "";//" checked='checked' ";
                                    if (objUsersAgendaIds.Contains(agendaId))
                                    {
                                        AgendaAccess = " checked='checked' ";
                                    }
                                    if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' > <input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + agendaName[i].AgendaName + "");
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + agendaName[i].AgendaName + "");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        strMenu.Append(" <ol  style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            AgendaAccess = "";//" checked='checked' ";
                                            if (objUsersAgendaIds.Contains(subAgendaName[j].AgendaId))
                                            {
                                                AgendaAccess = " checked='checked' ";
                                            }

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName + "");
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input  onclick=' onclick='SaveAccessSubAgenda(this)' ;'  type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + "   />" + subAgendaName[j].AgendaName + "");
                                            }
                                            else
                                            {
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName);// + "</li>");
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input   onclick='SaveAccessSubAgenda(this)'   type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + "   /> " + subAgendaName[j].AgendaName);// + "</li>");
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
                                                    AgendaAccess = "";//" checked='checked' ";
                                                    if (objUsersAgendaIds.Contains(subSubAgendaName[y].AgendaId))
                                                    {
                                                        AgendaAccess = " checked='checked' ";
                                                    }

                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' />" + subSubAgendaName[y].AgendaName + " ");
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input onclick='SaveAccessSubSubbAgenda(this)' type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + "    />" + subSubAgendaName[y].AgendaName + " ");
                                                    }
                                                    else
                                                    {
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subSubAgendaName[y].AgendaName);
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> <input onclick='SaveAccessSubSubbAgenda(this)'  type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + "  />" + subSubAgendaName[y].AgendaName);
                                                    }
                                                }
                                                strMenu.Append("</ol>");
                                            }
                                            strMenu.Append("</li>");
                                        }
                                        strMenu.Append("</ol></li>");
                                    }

                                }
                                strMenu.Append("</ol></div>");
                            }
                            lblList.Text = strMenu.ToString();


                            //  lblList.Text = strMenuList;
                        }
                        else
                        {
                            lblList.Text = "No agenda uploaded or approved";

                        }
                    }

                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid agenda search";
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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlMeeting.SelectedValue != "0")
                //{
                //    Guid entityId;
                //    if (Guid.TryParse(ddlMeeting.SelectedValue, out entityId))
                //    {
                //         ddlMeeting_SelectedIndexChanged(sender, e);

                //    }
                //}
                if (ddlUser.SelectedValue != "0")
                {
                    ddlForum.Items.Clear();

                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                    ddlMeeting.Items.Clear();
                    ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                    Guid UserId = Guid.Parse(ddlUser.SelectedValue);
                    Guid entityId;
                    if (Guid.TryParse(ddlEntity.SelectedValue, out entityId))
                    {
                        IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();
                        //IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);                    
                        ddlForum.DataSource = objForum;
                        ddlForum.DataBind();
                        ddlForum.DataTextField = "ForumName";
                        ddlForum.DataValueField = "ForumId";
                        ddlForum.DataBind();
                        ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                        if (ddlForum.Items.Count == 1)
                        {
                            ddlForum.Items.Insert(0, new ListItem("No forum assigned", "0"));
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (ddlUser.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please select user";
                    Error.Visible = true;
                    return;
                }

                if (ddlMeeting.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please select Meeting";
                    Error.Visible = true;
                    return;
                }
                UserAgendaDomain objUserAgenda = new UserAgendaDomain();
                string AgendaIds = hdnAgenda.Value;

                //if (AgendaIds == "")
                //{
                //    ((Label)Error.FindControl("lblError")).Text = "Please select at least one checkbox";
                //    Error.Visible = true;
                //    return;
                //}


                objUserAgenda.UserId = Guid.Parse(ddlUser.SelectedValue);

                objUserAgenda.UpdateBy = Guid.Parse(Session["UserId"].ToString());
                objUserAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objUserAgenda.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);

                StringBuilder sbAdd = new StringBuilder(",");
                StringBuilder sbDelete = new StringBuilder(",");

                if (ViewState["AgendaIds"] != null)
                {
                    if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                        return;
                    }
                    // List<Guid> objAgendaIds = (List<Guid>)ViewState["AgendaIds"];
                    List<string> objAgendaIds = (List<string>)ViewState["AgendaIds"];
                    List<string> objRemovedAgendaIds = new List<string>();
                    // objRemovedAgendaIds = objAgendaIds; 
                    List<string> objInsertIds = hdnAgenda.Value.Split(',').ToList<string>();
                    List<string> objDeleteList = objAgendaIds.Except(objInsertIds).ToList();
                    List<string> objAddList = objInsertIds.Except(objAgendaIds).ToList();

                    foreach (string objId in objAddList)
                    {
                        sbAdd.Append(objId.ToString() + ",");
                    }

                    foreach (string objId in objDeleteList)
                    {
                        sbDelete.Append(objId.ToString() + ",");
                    }

                    //if (objAgendaIds.Count > 0)
                    //{
                    //    // string[] newAgendaIds = hdnAgenda.Value;
                    //    // foreach (Guid objId in objAgendaIds)
                    //    for (int i = 0; i <= objAgendaIds.Count - 1; i++)
                    //    {
                    //        //if (AgendaIds.Contains(objId.ToString()))
                    //        //{
                    //        //    objRemovedAgendaIds.Remove(objId);
                    //        //}
                    //        //else
                    //        //{
                    //        //    sbAdd.Append(objId.ToString() + ",");
                    //        //}

                    //        if (AgendaIds.Contains(objAgendaIds[i].ToString()))
                    //        {
                    //            objRemovedAgendaIds.Add(objAgendaIds[i]);
                    //        }
                    //        else
                    //        {
                    //            sbAdd.Append(objAgendaIds[i].ToString() + ",");
                    //        }
                    //    }

                    //    //foreach (Guid objId in objAgendaIds)
                    //    //{
                    //    //    if (objAgendaIds.Contains(objId) && !objRemovedAgendaIds.Contains(objId))
                    //    //    {
                    //    //        // objAgendaIds.Remove(objAgendaIds[i]);

                    //    //        sbDelete.Append(objId.ToString() + ",");
                    //    //    }
                    //    //}

                    //    foreach (string objId in objAgendaIds)
                    //    {
                    //        if (objAgendaIds.Contains(objId) && !objRemovedAgendaIds.Contains(objId))
                    //        {
                    //            // objAgendaIds.Remove(objAgendaIds[i]);

                    //            sbDelete.Append(objId.ToString() + ",");
                    //        }
                    //    }
                    //}

                    bool status = UserAgendaDataProvider.Instance.Update(objUserAgenda, sbAdd.ToString(), sbDelete.ToString());
                    ((Label)Info.FindControl("lblName")).Text = "Agenda access updated sucessfully ";
                    Info.Visible = true;
                    ddlMeeting_SelectedIndexChanged(sender, e);
                    //if (status)
                    //{
                    //    ((Label)Info.FindControl("lblName")).Text = "Agenda access updated sucessfully ";
                    //    Info.Visible = true;
                    //}
                }

                else
                {
                    if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                        return;
                    }
                    bool status = false;
                    if (AgendaIds != "")
                    {
                        status = UserAgendaDataProvider.Instance.Insert(objUserAgenda, "," + AgendaIds + ",");
                        ((Label)Info.FindControl("lblName")).Text = "Agenda access inserted sucessfully ";
                        Info.Visible = true;
                        ddlMeeting_SelectedIndexChanged(sender, e);
                    }

                    //if (status)
                    //{
                    //((Label)Info.FindControl("lblName")).Text = "Agenda access inserted sucessfully ";
                    //Info.Visible = true;
                    //}

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
    }
}