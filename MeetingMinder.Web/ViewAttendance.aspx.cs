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

namespace MeetingMinder.Web
{
    public partial class ViewAttendance : System.Web.UI.Page
    {
        /// <summary>
        /// page laod event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Bind meeting drop down
                // BindMeetings();

                //BindEntity();
                Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                BindForum(EntityId.ToString());
                ddlEntity.SelectedValue = EntityId.ToString();

              //  ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId));
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


        /// Bind Entity list to drop down
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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p=>p.ForumName).ToList();
                    // IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
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
                        //if (dtMeeting < dtToday)
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

        /// <summary>
        /// Bind Meetings list to drop down
        /// </summary>
        /// <param name="strForumId">string specifying strForumId</param>
        /// <summary>
        /// Bind meeting drop down
        /// </summary>
        private void BindMeetings()
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                string UserId = Session["UserId"].ToString();
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByUserAccess(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                // IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetApprovedMeetingByUser(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                //MeetingDataProvider.Instance.GetMeetingByFroumID(forumId);

                DateTime dtToday = DateTime.Now.Date;

                foreach (MeetingDomain item in objMeeting)
                {
                    DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                    ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }

        }

        /// <summary>
        /// Bind Attendance By Meeting 
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        private void BindAttendance(Guid MeetingId)
        {
            try
            {
                IList<AttendanceDomain> objAttendance = AttendanceDataprovider.Instance.GetAttendanceByMeetingId(MeetingId);
                grdAttendance.DataSource = objAttendance;
                grdAttendance.DataBind();
                if (grdAttendance.Rows.Count > 0)
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    BindAttendance(Guid.Parse(strMeetingId));
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            string strMeetingId = ddlMeeting.SelectedValue;
            if (strMeetingId != "0")
            {
                IList<AttendanceDomain> objAttendance = AttendanceDataprovider.Instance.GetAttendanceByMeetingId(Guid.Parse(strMeetingId));

                IList<UserInfo> objUserinfo = new List<UserInfo>();
                UserInfo objUsersDetails ;
                foreach (AttendanceDomain objUser in objAttendance)
                {
                    objUsersDetails = new UserInfo();
                    objUsersDetails.Name = objUser.Suffix + " " + objUser.FirstName + " " + objUser.LastName;
                    objUsersDetails.Attendance = objUser.Attending;
                    objUsersDetails.Meeting = ddlMeeting.SelectedItem.Text;
                    objUsersDetails.Reason = objUser.Reason;

                    objUserinfo.Add(objUsersDetails);
                }

                if (objUserinfo.Count > 0)
                {
                    ExportToExcel(objUserinfo, "Attendance");
                }
            }
        }


        public void ExportToExcel(IList<UserInfo> ds, string filename)
        {
            HttpResponse response = HttpContext.Current.Response;

            // first let's clean up the response.object
            response.Clear();
            response.Charset = "";

            // set the response mime type for excel
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + ".xls\"");

            // create a string writer
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // instantiate a datagrid
                    GridView gdvContact = new GridView();
                    gdvContact.DataSource = ds;
                    gdvContact.DataBind();
                    gdvContact.HeaderRow.Font.Bold = true;


                    gdvContact.RenderControl(htw);
                    response.Write(sw.ToString());
                    response.End();
                }
            }
        }
    }

    public class UserInfo
    {
        public string Name { get; set; }

        public string Reason { get; set; }

        public string Attendance { get; set; }
        public string Meeting { get; set; }
    }
}