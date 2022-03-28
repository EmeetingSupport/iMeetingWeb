using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeetingMinder.Web
{
    public partial class UserAttendance : System.Web.UI.Page
    {
        public string strEntity = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                BindEntity();

                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    strEntity = Session["EntityName"].ToString();
                    //rcap

                    //rlife
                    //if (EntityId.ToString().Equals("ac0b10c7-46d9-45e0-b6e7-113aaaa504d9"))
                    //{
                    //    strName = " Puja Mehta ";
                    //    strPhone = " 9320032956 ";
                    //}

                    //rgeneral

                    //strName = " Secretarial Team ";
                    //strPhone = " Email or Phone";


                    BindForum(EntityId.ToString());

                    ddlEntity.SelectedValue = EntityId.ToString();
                    ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList();
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
        /// drop down list Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlEntity.SelectedValue;
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
        /// Bind Forum List to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                Guid entityId;
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();

                    //ForumDataProvider.Instance.GetForumByEntityId(entityId);
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
                ddlMeeting.Items.Clear();

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                if (strForumId != null && strForumId != "0")
                {
                    Guid forumId = Guid.Parse(Convert.ToString(strForumId));
                    //IList<UserDomain> objUserDom = UserDataProvider.Instance.GetAllUserForEmailNotification(forumId);
                    //grdReport.DataSource = objUserDom;
                    //grdReport.DataBind();


                    IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();

                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    DateTime dtToday = DateTime.Now;

                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate + " " + item.MeetingTime);
                        if (dtMeeting <= dtToday.AddDays(1) && (dtToday - dtMeeting).Days <= 1)
                        {
                            //  ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, dtMeeting.ToString("D") + '`' + item.MeetingVenue + '`' + item.MeetingTime));
                            ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                        }

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

        protected void lnkEmailConfig_Click(object sender, EventArgs e)
        {

        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    IList<UserDomain> objUser = UserDataProvider.Instance.GetAllUserForAttendace(Guid.Parse(strMeetingId)).Where(p => p.IsEnabledOnIpad).OrderBy(p => p.FirstName).ToList();
                    grdReport.DataSource = objUser;
                    grdReport.DataBind();

                    if (objUser.Count > 0)
                    {
                        btnSubmit.Visible = true;
                    }
                    else
                    {
                        btnSubmit.Visible = false;
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid MeetingId = Guid.Parse(ddlMeeting.SelectedValue);

                UserAttendanceDomain objUserAttendanceDomain = new UserAttendanceDomain();
                objUserAttendanceDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objUserAttendanceDomain.MeetingId = MeetingId;

                for (int i = 0; i < grdReport.Rows.Count; i++)
                {
                    RadioButtonList rbLst = (RadioButtonList)grdReport.Rows[i].FindControl("rbAttendance");
                    HiddenField hdfUserId = (HiddenField)grdReport.Rows[i].FindControl("hdfUserId");


                    objUserAttendanceDomain.Attendance = Convert.ToBoolean(rbLst.SelectedValue);
                    objUserAttendanceDomain.MeetingId = MeetingId;
                    objUserAttendanceDomain.UserId = Guid.Parse(hdfUserId.Value);


                    objUserAttendanceDomain = UserAttendanceDataprovider.Instance.InsertUserAttendance(objUserAttendanceDomain);
                }

                if (grdReport.Rows.Count > 0)
                {
                    Info.Visible = true;
                    ((Label)Info.FindControl("lblName")).Text = "Attendance saved successfully";
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

        protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RadioButtonList rbl = e.Row.FindControl("rbAttendance") as RadioButtonList;
                if (rbl != null)
                {
                    rbl.SelectedValue = e.Row.DataItem.ToString(); //((YOURDATAITEM)(e.Row.DataItem).YourProperty.ToString();
                }
            }
        }
    }
}