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
using System.Globalization;

namespace MeetingMinder.Web
{
    public partial class MeetingRetention : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            //Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                #region
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion
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
                if (strForumId != "0")
                {
                    grdMeeting.EditIndex = -1;
                    BindMeeting(ddlForum.SelectedValue);
                }
                else
                {
                    grdMeeting.DataSource = null;
                    grdMeeting.DataBind();
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
        /// bind meeting by forum id
        /// </summary>
        /// <param name="strForumId"></param>
        private void BindMeeting(string strForumId)
        {
            try
            {
                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderByDescending(p => DateTime.Parse(p.MeetingDate)).Take(5).ToList();

                    IList<MeetingDomain> listMeeting = new List<MeetingDomain>();
                    foreach (MeetingDomain item in objMeeting)
                    {
                        MeetingDomain objMeetingDomain = new MeetingDomain();
                        string meetDt = item.MeetingDate;
                        string[] strArry = meetDt.Split('/');
                        meetDt = strArry[1] + "/" + strArry[0] + "/" + strArry[2];
                        DateTime dtMeeting = Convert.ToDateTime(meetDt + " " + item.MeetingTime, new CultureInfo("hi-IN")).AddDays(4);
                        DateTime dtToday = DateTime.Now;
                        objMeetingDomain.PastMeetingExpiry = 1;
                        if (dtMeeting < dtToday)
                        {
                            objMeetingDomain.PastMeetingExpiry = 0;
                        }

                        objMeetingDomain.MeetingId = item.MeetingId;
                        objMeetingDomain.MeetingTime = item.MeetingTime;
                        objMeetingDomain.MeetingDate = item.MeetingDate;
                        objMeetingDomain.ForumName = item.ForumName;
                        objMeetingDomain.MeetingNumber = item.MeetingNumber;
                        objMeetingDomain.PastMeeting = item.PastMeeting;
                        objMeetingDomain.MeetingVenue = item.MeetingVenue;
                        listMeeting.Add(objMeetingDomain);
                    }

                    grdMeeting.DataSource = listMeeting;
                    grdMeeting.DataBind();

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
        /// Gridview pageindexchange event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMeeting.PageIndex = e.NewPageIndex;
                BindMeeting(ddlForum.SelectedValue);
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
        /// Gridview row command event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// Gridview rowediting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdMeeting.EditIndex = e.NewEditIndex;
                BindMeeting(ddlForum.SelectedValue);
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
        /// Gridview row update event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TextBox txtVal = (TextBox)grdMeeting.Rows[e.RowIndex].FindControl("txtVal");
                TextBox txtMeetingDate = (TextBox)grdMeeting.Rows[e.RowIndex].FindControl("txtMeetingDate");
                TextBox txtTime = (TextBox)grdMeeting.Rows[e.RowIndex].FindControl("txtTime");

                UserAcess objUser = new UserAcess();

                if (txtVal.Text != "")
                {
                    if (!objUser.CSVValidation(txtVal.Text))
                    {
                        txtVal.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtVal.Text))
                    {
                        txtVal.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter value";
                    Error.Visible = true;
                    return;
                }

                if (txtMeetingDate.Text != "")
                {
                    if (!objUser.CSVValidation(txtMeetingDate.Text))
                    {
                        txtMeetingDate.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtMeetingDate.Text))
                    {
                        txtMeetingDate.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter value";
                    Error.Visible = true;
                    return;
                }

                if (txtTime.Text != "")
                {
                    if (!objUser.CSVValidation(txtTime.Text))
                    {
                        txtTime.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtTime.Text))
                    {
                        txtTime.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter value";
                    Error.Visible = true;
                    return;
                }
                Guid id = Guid.Parse(grdMeeting.DataKeys[e.RowIndex].Value.ToString());
                //Label lblKey = (Label)grdMeeting.Rows[e.RowIndex].FindControl("lblKey");
                MeetingRetentionDomain objMeeting = new MeetingRetentionDomain();
                objMeeting.Id = id;
                objMeeting.Value = txtVal.Text.Trim();
                objMeeting.UpdatedBy = Guid.Parse(Session["UserId"].ToString());


                MeetingDomain objMeetingDetails = MeetingDataProvider.Instance.Get(objMeeting.Id);

                objMeeting.MeetingDate = txtMeetingDate.Text.Trim();
                objMeeting.MeetingTime = txtTime.Text.Trim();

                string dtMeet = objMeeting.MeetingDate.Replace('.', '/');
                string dtMeetTime = objMeeting.MeetingTime.Replace('.', ':');

                DateTime dtToday = DateTime.Now;
                DateTime dtMeeting = Convert.ToDateTime(dtMeet + " " + dtMeetTime, new CultureInfo("hi-IN")).AddDays(4);
                if (dtMeeting < dtToday)
                {
                    ((Label)Error.FindControl("lblError")).Text = " Meeting days can not be editable.";
                    Error.Visible = true;
                    return;
                }

                DateTime dtMeeting1 = Convert.ToDateTime(dtMeet + " " + dtMeetTime, new CultureInfo("hi-IN"));
                if (objMeetingDetails.MeetingDate != dtMeeting1.ToString("MM/dd/yyyy") || objMeetingDetails.MeetingTime != dtMeetTime.ToString())
                {
                    if (dtMeeting1 < dtToday)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Meeting date and time can not be earlier than current time.";
                        Error.Visible = true;
                        return;
                    }
                }


                //objMeeting.MeetingTime = dtMeetTime;

                if (txtVal.Text.Length == 0)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Meeting retention updatation failed. Please enter value in text box";
                    Error.Visible = true;
                }

                objMeeting.MeetingDate = dtMeeting1.ToString("MM/dd/yyyy");
                objMeeting.MeetingTime = dtMeetTime;

                bool status = MeetingDataProvider.Instance.UpdateMeetingRetention(objMeeting);
                if (status)
                {
                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objMeeting);
                    var encrypt = Encryptor.EncryptString(json);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Meeting Retention", "Success", Convert.ToString(objMeeting.Id), "Meeting retention updated successfully! :- " + encrypt + "");

                    ((Label)Info.FindControl("lblName")).Text = "Meeting retention updated successfully!";
                    Info.Visible = true;
                    grdMeeting.EditIndex = -1;
                    BindMeeting(ddlForum.SelectedValue);
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Meeting Retention", "Failed", Convert.ToString(objMeeting.Id), "Meeting retention updatation failed");

                    ((Label)Error.FindControl("lblError")).Text = "Meeting retention updatation failed";
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
        /// Gridview row cancelling event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdMeeting.EditIndex = -1;
                BindMeeting(ddlForum.SelectedValue);
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
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(Guid.Parse(ddlForum.SelectedValue)).OrderByDescending(p => DateTime.Parse(p.MeetingDate)).Take(5).ToList();

                IList<MeetingDomain> listMeeting = new List<MeetingDomain>();
                foreach (MeetingDomain item in objMeeting)
                {
                    MeetingDomain objMeetingDomain = new MeetingDomain();
                    string meetDt = item.MeetingDate;
                    string[] strArry = meetDt.Split('/');
                    meetDt = strArry[1] + "/" + strArry[0] + "/" + strArry[2];
                    DateTime dtMeeting = Convert.ToDateTime(meetDt + " " + item.MeetingTime, new CultureInfo("hi-IN")).AddDays(2);
                    DateTime dtToday = DateTime.Now;
                    objMeetingDomain.PastMeetingExpiry = 1;
                    if (dtMeeting < dtToday)
                    {
                        objMeetingDomain.PastMeetingExpiry = 0;
                    }

                    objMeetingDomain.MeetingId = item.MeetingId;
                    objMeetingDomain.MeetingTime = item.MeetingTime;
                    objMeetingDomain.MeetingDate = item.MeetingDate;
                    objMeetingDomain.ForumName = item.ForumName;
                    objMeetingDomain.MeetingNumber = item.MeetingNumber;
                    objMeetingDomain.PastMeeting = item.PastMeeting;
                    listMeeting.Add(objMeetingDomain);
                }

                DataTable dt = (DataTable)listMeeting.AsDataTable();
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