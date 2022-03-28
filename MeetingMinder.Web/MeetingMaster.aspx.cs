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

namespace MeetingMinder.Web
{
    public partial class MeetingMaster : System.Web.UI.Page
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
                //bind meeting list
                BindMeeting();

                //Bind User to drop down
                BindUsers();
            }
        }

        /// <summary>
        /// Bind User List to drop down
        /// </summary>
        private void BindUsers()
        {
            try
            {
                ddlUser.DataSource = UserDataProvider.Instance.GetAllChecker();
                ddlUser.DataBind();
                ddlUser.DataValueField = "UserId";
                ddlUser.DataTextField = "UserName";
                ddlUser.DataBind();

                //Remove logged in users id from drop down
                //string UserId = Convert.ToString(Session["UserId"]);
                //ListItem UserIdToRemove = ddlUser.Items.FindByValue(UserId);
                //ddlUser.Items.Remove(UserIdToRemove);

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
        /// Bind Meeintg List
        /// </summary>
        private void BindMeeting()
        {
            try
            {
                //check for forum id
                if (Request.QueryString["id"] != null)
                {
                    string strForumId = Convert.ToString(Request.QueryString["id"]);
                    ForumDomain objForum = ForumDataProvider.Instance.Get(Guid.Parse(strForumId));
                    string EntityName = objForum.EntityName;
                    string ForumName = objForum.ForumName;
                    string EntityId = objForum.EntityId.ToString();

                    ViewState["entityId"] = EntityId;

                    Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");


                    ltl_bredcrumbs.Text = "";

                    Guid forumId;
                    if (Guid.TryParse(strForumId, out forumId))
                    {
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId);

                        ViewState["forumId"] = forumId;
                        grdMeeting.DataSource = objMeeting;

                        grdMeeting.DataBind();
                        //if (objMeeting.Count > 0)
                        //{
                        //    EntityName = objMeeting[0].EntityName;
                        //    EntityId = objMeeting[0].EntityId.ToString();
                        //}
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
                        Error.Visible = true;
                    }


                    ltl_bredcrumbs.Text += "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home<a>&nbsp;/&nbsp;<a href='EntityMaster.aspx'>" + EntityName + "<a>&nbsp;/&nbsp;<a href=ForumMaster.aspx?id=" + EntityId + ">" + ForumName + "<a>&nbsp;";

                }
                else
                {

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
        /// Delelte Seleted Meeting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lbRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                //check Delete permission
                if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                {

                    //StringBuilder strQuery = new StringBuilder("");
                    StringBuilder strMeetingIds = new StringBuilder(",");
                    int count = 0;
                    // get all checked meeting
                    for (int i = 0; i < grdMeeting.Rows.Count; i++)
                    {


                        CheckBox chkSelect = (CheckBox)grdMeeting.Rows[i].FindControl("chkSubAdmin");
                        if (chkSelect.Checked)
                        {
                            count++;
                            string MeetingID = Convert.ToString(grdMeeting.DataKeys[i].Value.ToString());
                            // strQuery.Append("	DELETE FROM [Meeting]   WHERE  MeetingId = '" + MeetingID + "'  ");
                            strMeetingIds.Append(MeetingID + ",");
                        }
                    }
                    if (count > 0)
                    {
                        bool bStatus = MeetingDataProvider.Instance.DeleteSelectedMeeting(strMeetingIds.ToString());
                        if (!bStatus)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Meeting Deleted Successfully";
                            Info.Visible = true;
                            BindMeeting();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please Select atleast one checkbox";
                        Error.Visible = true;
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry Access denied ";
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
        /// Insert or edit values 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                if (txtMeetingDate.Text != "" && txtMeetingVenue.Text != "")
                {
                    MeetingDomain objMeeting = new MeetingDomain();
                    objMeeting.MeetingVenue = txtMeetingVenue.Text;
                    DateTime dtMeeting = Convert.ToDateTime(txtMeetingDate.Text);
                    DateTime dtToday = DateTime.Now;
                    if (dtMeeting <= dtToday)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Meeting date is past date";
                        Error.Visible = true;
                        return;
                    }
                    objMeeting.MeetingDate = dtMeeting.ToString();
                    objMeeting.MeetingTime = txtTime.Text;
                    Guid CheckerId;
                    if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                    {
                        objMeeting.MeetingChecker = CheckerId;
                    }

                    objMeeting.ForumId = Guid.Parse(ViewState["forumId"].ToString());


                    objMeeting.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                    objMeeting.UpdatedBy = Guid.Parse(Session["UserId"].ToString());


                    if (objMeeting.CreatedBy == objMeeting.MeetingChecker)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                        Error.Visible = true;
                        return;
                    }

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {

                        //Update Meeting 
                        if (hdnMeetingId.Value != "" && hdnMeetingId.Value != null)
                        {


                            objMeeting.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                            objMeeting.MeetingId = Guid.Parse(hdnMeetingId.Value);

                            //Guid CreatedBy = Guid.Parse(ViewState["CreatedBy"].ToString());

                            //if (CreatedBy == objMeeting.MeetingChecker)
                            //{
                            //    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                            //    Error.Visible = true;
                            //    return;
                            //}

                            //MeetingDataProvider.Instance.Update(objMeeting);
                            ((Label)Info.FindControl("lblName")).Text = " Meeting updated successfully";
                        }
                        //Insert Meeting
                        else
                        {
                            UserAcess objUser = new UserAcess();
                            //check add permission
                            if (objUser.IsAdd(Guid.Parse(ViewState["entityId"].ToString())))
                            {
                                // MeetingDataProvider.Instance.Insert(objMeeting);
                                ((Label)Info.FindControl("lblName")).Text = " Meeting inserted successfully";
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Sorry Access denied ";
                                Error.Visible = true;
                            }
                        }

                        Info.Visible = true;
                        BindMeeting();
                        ClearData();
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
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
        /// Insertion or Updation cancel
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Clear form data
        /// </summary>
        private void ClearData()
        {
            try
            {
                txtMeetingDate.Text = "";
                txtMeetingVenue.Text = "";
                hdnMeetingId.Value = "";
                ddlUser.SelectedValue = "0";
                txtTime.Text = "";
                btnInsert.Text = "Save";
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
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMeeting.PageIndex = e.NewPageIndex;
                BindMeeting();
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
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeetingRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                //delete meeting
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    UserAcess objUser = new UserAcess();
                    //check Delete permission
                    if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                    {
                        string MeetingId = Convert.ToString(e.CommandArgument.ToString());
                        bool bStatus = MeetingDataProvider.Instance.Delete(Guid.Parse(MeetingId), "");
                        if (bStatus == false)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Meeting Deleted Successfully";
                            Info.Visible = true;
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                        BindMeeting();

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry Access denied ";
                        Error.Visible = true;
                    }
                }

                // view meeting
                if (e.CommandName.ToLower().Equals("view"))
                {

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
        /// Row deleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                //check edit permission
                if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                {
                    string strMeetingId = grdMeeting.DataKeys[e.NewEditIndex].Value.ToString();

                    MeetingDomain objMeetingDomain = MeetingDataProvider.Instance.Get(Guid.Parse(strMeetingId));
                    //convert date to dd/mm/yyyy formate
                    txtMeetingDate.Text = Convert.ToDateTime(objMeetingDomain.MeetingDate).ToString("MM/dd/yyyy");
                    txtMeetingVenue.Text = objMeetingDomain.MeetingVenue;
                    hdnMeetingId.Value = Convert.ToString(objMeetingDomain.MeetingId);
                    txtTime.Text = Convert.ToString(objMeetingDomain.MeetingTime);
                    ddlUser.SelectedValue = Convert.ToString(objMeetingDomain.MeetingChecker);
                    ViewState["CreatedBy"] = objMeetingDomain.CreatedBy;
                    btnInsert.Text = "Update";
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry Access denied ";
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
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid meetingId = Guid.Parse(ViewState["forumId"].ToString());
                DataTable dt = (DataTable)MeetingDataProvider.Instance.GetMeetingByFroumID(meetingId).AsDataTable();
                DataView dv = new DataView(dt);
                if (Convert.ToString(ViewState["sortDirection"]) == "asc")
                {
                    ViewState["sortDirection"] = "dsc";
                    dv.Sort = e.SortExpression + " DESC";
                }
                else
                {
                    ViewState["sortDirection"] = "asc";
                    dv.Sort = e.SortExpression + " ASC";
                }

                grdMeeting.DataSource = dv;
                grdMeeting.DataBind();
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