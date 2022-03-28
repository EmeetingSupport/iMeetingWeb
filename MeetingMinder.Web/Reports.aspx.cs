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
using System.IO;
using System.Net;
using System.Security.Cryptography;
using iTextSharp.text.pdf;
using System.Data.SqlClient;

namespace MeetingMinder.Web
{
    public partial class Reports : System.Web.UI.Page
    {
        /// <summary>
        /// page laod event
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
                #region
                ///Comment for hide entity name
                BindEntity();
                BindSerialNumber();
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



                ddlMember.Items.Insert(0, new ListItem("Select Forum/Members ", "0"));

                ddlAgenda.Items.Insert(0, new ListItem("Select", "0"));

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
            }
        }

        /// <summary>
        /// Bind User
        /// </summary>
        private void BindUser(Guid MeetingId)
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName+' ('+Username+')'";

                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllUserForAttendace(MeetingId);
                if (objUser.Count > 0)
                {
                    //Bind drop down list
                    //ddlUser.DataSource = objUser;
                    //ddlUser.DataBind();
                    //ddlUser.DataTextField = "UserName";
                    //ddlUser.DataValueField = "UserId";
                    //ddlUser.DataBind();
                    //ddlUser.Items.Insert(0, new ListItem("Select User", "0"));

                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                    ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
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

        private void BindAttendanceForReport(int pageIndex)
        {
            try
            {
                Guid ForumId = Guid.Empty;
                Guid MeetingId = Guid.Empty;
                Guid UserId = Guid.Empty;

                DateTime StartDate = Convert.ToDateTime("01/01/1978");
                DateTime EndDate = Convert.ToDateTime("01/01/1978");



                if (ddlForum.SelectedValue.ToString() != "" && ddlForum.SelectedValue.ToString() != "0")
                {
                    ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                }
                if (ddlMeeting.SelectedValue.ToString() != "" && ddlMeeting.SelectedValue.ToString() != "0")
                {
                    MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                }
                if (ddlUser.SelectedValue.ToString() != "" && ddlUser.SelectedValue.ToString() != "0")
                {
                    UserId = Guid.Parse(ddlUser.SelectedValue.ToString());
                }

                if (txtFrom.Text != "" && txtTo.Text != "")
                {
                    StartDate = Convert.ToDateTime(txtFrom.Text);
                    EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                }

                DataSet ds = UserAttendanceDataprovider.Instance.GetAttendanceReport(ForumId, MeetingId, UserId, StartDate, EndDate, pageIndex, grdAttendanceReport.PageSize);

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                    if (totalCount > grdAttendanceReport.PageSize)
                    {
                        PopulatePager(totalCount, pageIndex);
                    }
                    else
                    {
                        //clear page data
                        rptAttendance.DataSource = null;
                        rptAttendance.DataBind();
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    grdAttendanceReport.DataSource = ds;
                    grdAttendanceReport.DataBind();
                    btnExport.Visible = true;

                }
                else
                {

                    grdAttendanceReport.DataSource = null;
                    grdAttendanceReport.DataBind();
                    btnExport.Visible = false;
                    btnExport.OnClientClick = "";
                }
                trAttendanceReport.Visible = true;

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
        /// Bind serial No
        /// </summary>
        private void BindSerialNumber()
        {
            try
            {
                IList<SerialNumberDomain> objSerialNumber = SerialNumberDataProvider.Instance.Get().OrderBy(p => p.SerialNumber).ToList();
                ddlSerialNumber.DataSource = objSerialNumber;
                ddlSerialNumber.DataBind();
                ddlSerialNumber.DataTextField = "SerialNumber";
                ddlSerialNumber.DataBind();
                ddlSerialNumber.Items.Insert(0, new ListItem("Select ", "0"));

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
        /// Bind all agenda items
        /// </summary>
        private void BindAllAgenda(Guid ForumId)
        {
            try
            {
                IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAllAgendaTitles(ForumId);//.OrderBy(p => p.AgendaName).ToList(); ;
                ddlAgenda.DataSource = objAgenda;
                ddlAgenda.DataBind();
                ddlAgenda.DataTextField = "AgendaName";
                ddlAgenda.DataValueField = "AgendaId";
                ddlAgenda.DataBind();
                ddlAgenda.Items.Insert(0, new ListItem("Select ", "0"));
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
        /// drop down list Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlEntity.SelectedValue;
                trUsers.Visible = false;
                btnExport.Visible = false;
                if (strEntityId != "0")
                {
                    lblView.Visible = false;
                    lnkView.Visible = false;
                    BindForum(strEntityId);
                    ddlMember.Items.Clear();
                    ddlMember.Items.Insert(0, new ListItem("Select Forum/Members ", "0"));
                }
                else
                {
                    ddlForum.Items.Clear();
                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
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

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MembesInfo"]);

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
                if (strForumId != null && strForumId != "0")
                {
                    trUsers.Visible = true;
                    Guid forumId = Guid.Parse(Convert.ToString(strForumId));
                    IList<UserDomain> objUserDom = UserDataProvider.Instance.GetAllUserForReport(forumId);
                    grdReport.DataSource = objUserDom;
                    grdReport.DataBind();

                    if (objUserDom.Count > 0)
                    {
                        btnExport.Visible = true;
                    }
                    else
                    {
                        btnExport.Visible = false;
                    }

                    BindAllAgenda(Guid.Parse(strForumId));
                    BindMeeting(strForumId);
                }
                else
                {
                    trUsers.Visible = false;
                    btnExport.Visible = false;
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
        protected void ddlForum_SelectedIndexChanged_old(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    ViewState["Member"] = null;
                    ForumDomain objForum = ForumDataProvider.Instance.Get(Guid.Parse(ddlForum.SelectedValue));
                    if (objForum != null)
                    {
                        if (objForum.MembersInfo != null)
                        {
                            if (objForum.MembersInfo.Length > 0)
                            {
                                ddlMember.Items.Clear();
                                ddlMember.Items.Insert(0, new ListItem("Select Forum/Members ", "0"));
                                ViewState["Member"] = objForum.MembersInfo;
                                //lnkView.Visible = true;
                                //lblView.Visible = false;
                                lnkView.Visible = false;
                                lblView.Visible = false;
                                ddlMember.Items.Insert(1, new ListItem("View Details", objForum.MembersInfo));

                            }
                            else
                            {
                                ddlMember.Items.Clear();
                                ddlMember.Items.Insert(0, new ListItem("Select Forum/Members ", "0"));
                                lblView.Text = "No file uploaded";
                                lblView.ForeColor = System.Drawing.Color.Red;
                                // lblView.Visible = true;
                                lblView.Visible = false;
                                lnkView.Visible = false;
                            }
                        }
                        else
                        {
                            lblView.Text = "No file uploaded";
                            lblView.ForeColor = System.Drawing.Color.Red;
                            lblView.Visible = false;
                            // lblView.Visible = true;
                            lnkView.Visible = false;
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

        /// <summary>
        /// drop down list selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string fileView = ddlMember.SelectedValue;
                if (fileView.Length > 0 && fileView != "0")
                {
                    lnkView.Visible = true;
                    lblView.Visible = false;
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
        /// Button export to excel click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnExport_Click(object sender, EventArgs e)
        {

            try
            {
                if (ddlReport.SelectedValue == "Forum")
                {
                    Guid forumId = Guid.Parse(Convert.ToString(ddlForum.SelectedValue));
                    IList<UserDomain> objUserList = UserDataProvider.Instance.GetAllUserForReport(forumId).OrderBy(p => p.FirstName.Trim()).OrderBy(p => p.LastName.Trim()).ToList();
                    if (objUserList.Count > 0)
                    {
                        IList<UserInfo> objUserinfo = new List<UserInfo>();
                        UserInfo objUsersDetails;
                        foreach (UserDomain objUser in objUserList)
                        {
                            objUsersDetails = new UserInfo();
                            objUsersDetails.Name = objUser.Suffix + " " + objUser.FirstName + " " + objUser.LastName;
                            objUsersDetails.Designation = WebUtility.HtmlDecode(objUser.Designation);
                            objUserinfo.Add(objUsersDetails);
                        }
                        ExportToExcel(objUserinfo, "Member Info");
                    }
                }
                else if (ddlReport.SelectedValue == "Meeting")
                {
                    IList<MeetingDomain> objMeetingDomain = MeetingDataProvider.Instance.GetMeetingReportByYear(Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1));
                    objMeetingDomain = objMeetingDomain.Where(p => Convert.ToDateTime(p.MeetingDate) >= Convert.ToDateTime(txtFrom.Text) && Convert.ToDateTime(p.MeetingDate) <= Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1)).OrderByDescending(p => Convert.ToDateTime(p.MeetingDate + " " + p.MeetingTime)).ToList();
                    ExportToExcelMeetingReport(objMeetingDomain, "Meeting Info");
                                        
                }
                else if (ddlReport.SelectedValue == "Agenda")
                {
                    Guid ForumId = Guid.Empty;
                    Guid UserId = Guid.Empty;
                    string AgendaName = "";
                    DateTime StartDate = Convert.ToDateTime("01/01/1978");
                    DateTime EndDate = Convert.ToDateTime("01/01/1978");
                    string SerialNumber = "";
                    int pageIndex = 0;
                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                    }

                    if (ddlAgenda.SelectedValue.ToString() != "0")
                    {
                        AgendaName = ddlAgenda.SelectedItem.ToString();
                    }

                    if (ddlSerialNumber.SelectedValue.ToString() != "0")
                    {
                        SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    //DataSet ds = AgendaDataProvider.Instance.GetAgendaForReport(ForumId, AgendaName, SerialNumber, StartDate, EndDate, pageIndex, grdAgendaReport.PageSize);
                    //ExportToExcelAgendaReport(ds, "Agenda Info");
                }

                else if (ddlReport.SelectedValue == "Minutes")
                {
                    Guid ForumId = Guid.Empty;
                    Guid MeetingId = Guid.Empty;
                    Guid UserId = Guid.Empty;
                    //string AgendaName = "";
                    DateTime StartDate = Convert.ToDateTime("01/01/1978");
                    DateTime EndDate = Convert.ToDateTime("01/01/1978");
                    //string SerialNumber = "";

                    UserId = Guid.Parse(Session["UserId"].ToString());
                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                    }

                    if (ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    int pageIndex = 0;
                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }
                    DataSet ds = UploadMintesDataProvider.Instance.GetUploadMinutesByUserAcessForReport(UserId, ForumId, MeetingId, StartDate, EndDate, pageIndex, grdMinutesReports.PageSize);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            IList<MinutesList> objMinutesList = new List<MinutesList>();
                            MinutesList objMinutes;
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                objMinutes = new MinutesList();
                                objMinutes.ForumName = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["ForumName"].ToString());
                                objMinutes.MeetingNumber = ds.Tables[0].Rows[i]["MeetingNumber"].ToString();
                                objMinutes.Meeting = Convert.ToDateTime(MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["MeetingDate"].ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["MeetingTime"].ToString()) + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["MeetingVenue"].ToString());
                                //Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())
                                objMinutesList.Add(objMinutes);
                            }
                            ExportToExcelMinutes(objMinutesList, "Minutes Info");
                        }
                    }
                }
                else if (ddlReport.SelectedValue == "Attendance")
                {
                    Guid ForumId = Guid.Empty;
                    Guid MeetingId = Guid.Empty;
                    Guid UserId = Guid.Empty;

                    DateTime StartDate = Convert.ToDateTime("01/01/1978");
                    DateTime EndDate = Convert.ToDateTime("01/01/1978");



                    if (ddlForum.SelectedValue.ToString() != "" && ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                    }
                    if (ddlMeeting.SelectedValue.ToString() != "" && ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                    }
                    if (ddlUser.SelectedValue.ToString() != "" && ddlUser.SelectedValue.ToString() != "0")
                    {
                        UserId = Guid.Parse(ddlUser.SelectedValue.ToString());
                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    int pageIndex = 0;
                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    DataSet ds = UserAttendanceDataprovider.Instance.GetAttendanceReport(ForumId, MeetingId, UserId, StartDate, EndDate, pageIndex, grdAttendanceReport.PageSize);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            IList<AttendanceList> objAttendanceList = new List<AttendanceList>();
                            AttendanceList objAttendance;
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                objAttendance = new AttendanceList();
                                objAttendance.Name = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["Suffix"].ToString()) + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["FirstName"].ToString()) + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["LastName"].ToString());
                                objAttendance.Attendance = Null.SetNullBoolean(ds.Tables[0].Rows[i]["Attendance"].ToString()) ? "Present" : "Absent";
                                objAttendance.Meeting = Convert.ToDateTime(MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["MeetingDate"].ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["MeetingTime"].ToString()) + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["MeetingVenue"].ToString());
                                //Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())
                                objAttendanceList.Add(objAttendance);
                            }
                            ExportToExcelAttendace(objAttendanceList, "Attendance");
                        }
                    }
                }

                else if (ddlReport.SelectedValue == "Proceeding")
                {
                    Guid ForumId = Guid.Empty;
                    Guid MeetingId = Guid.Empty;
                    DateTime StartDate = Convert.ToDateTime("01/01/1978");
                    DateTime EndDate = Convert.ToDateTime("01/01/1978");

                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                    }
                    if (ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                    }
                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }
                    int pageIndex = 0;
                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }
                    DataSet ds = ProcedingDataProvider.Instance.GetProcedingsForReport(ForumId, MeetingId, StartDate, EndDate, pageIndex, grdProceedingReport.PageSize);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            IList<MinutesList> objMinutesList = new List<MinutesList>();
                            MinutesList objMinutes;
                            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                            {
                                objMinutes = new MinutesList();
                                objMinutes.ForumName = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["ForumName"].ToString());
                                objMinutes.Meeting = Convert.ToDateTime(MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[i]["MeetingDate"].ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["MeetingTime"].ToString()) + " " + MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["MeetingVenue"].ToString());
                                //Convert.ToDateTime(MM.Core.Encryptor.DecryptString(Eval("MeetingDate").ToString())).ToString("D") + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingTime").ToString()) + " " + MM.Core.Encryptor.DecryptString(Eval("MeetingVenue").ToString())		
                                objMinutesList.Add(objMinutes);
                            }
                            ExportToExcelMinutes(objMinutesList, "Minutes Info");
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

        private void ExportToExcelMinutes(IList<MinutesList> ds, string filename)
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
        private void ExportToExcelAttendace(IList<AttendanceList> ds, string filename)
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

        public class UserInfo
        {
            public string Name { get; set; }
            public string Designation { get; set; }
        }

        public class MinutesList
        {
            public string ForumName { get; set; }

            public string MeetingNumber { get; set; }
            public string Meeting { get; set; }

        }
        public class AttendanceList
        {
            public string Name { get; set; }
            public string Meeting { get; set; }
            public string Attendance { get; set; }

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

        public void ExportToExcelMeetingReport(IList<MeetingDomain> ds, string filename)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;

                // first let's clean up the response.object
                response.Clear();
                response.Charset = "";

                var MeetingDomain = (from meeting in ds
                                     select new
                                     {
                                         ForumName = meeting.ForumName,
                                         MeetingNumber = meeting.MeetingNumber,
                                         MeetingDate = Convert.ToDateTime(meeting.MeetingDate).ToString("D"),
                                         Time = meeting.MeetingTime,
                                         Venue = meeting.MeetingVenue
                                     }).ToList();


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
                        gdvContact.DataSource = MeetingDomain;
                        gdvContact.DataBind();
                        gdvContact.HeaderRow.Font.Bold = true;


                        gdvContact.RenderControl(htw);
                        response.Write(sw.ToString());
                        response.End();
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

        public void ExportToExcelAgendaReport(DataSet ds, string filename)
        {
            try
            {
                HttpResponse response = HttpContext.Current.Response;

                // first let's clean up the response.object
                response.Clear();
                response.Charset = "";

                DataTable agendas = ds.Tables[0];

                var query = agendas.AsEnumerable().
                    Select(agenda => new
                    {
                        ForumName = MM.Core.Encryptor.DecryptString(agenda.Field<string>("ForumName").ToString()),
                        Meeting = Convert.ToDateTime(MM.Core.Encryptor.DecryptString(agenda.Field<string>("MeetingDate"))).ToString("D") + " " + MM.Core.Encryptor.DecryptString(agenda.Field<string>("MeetingTime")) + " " + MM.Core.Encryptor.DecryptString(agenda.Field<string>("MeetingVenue")),
                        Agenda = Agenda(agenda.Field<Guid>("MeetingId").ToString())
                    }).ToList();

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
                        gdvContact.DataSource = Agenda("d5646132-7274-4cd5-8fbc-c252ebef484e");
                        gdvContact.DataBind();
                        gdvContact.HeaderRow.Font.Bold = true;

                        gdvContact.RenderControl(htw);
                        response.Write(sw.ToString());
                        response.End();
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



        private void BindAgendaForReport(int pageIndex)
        {
            try
            {
                Guid ForumId = Guid.Empty;
                Guid UserId = Guid.Empty;
                string AgendaName = "";
                DateTime StartDate = Convert.ToDateTime("01/01/1978");
                DateTime EndDate = Convert.ToDateTime("01/01/1978");
                string SerialNumber = "";


                if (ddlForum.SelectedValue.ToString() != "0")
                {
                    ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                }

                //if (ddlAgenda.SelectedValue.ToString() != "0")
                //{
                //    AgendaName = ddlAgenda.SelectedItem.ToString();
                //}

                AgendaName = txtAgendaTitle.Text.Trim();

                if (ddlSerialNumber.SelectedValue.ToString() != "0")
                {
                    SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                }

                if (txtFrom.Text != "" && txtTo.Text != "")
                {
                    StartDate = Convert.ToDateTime(txtFrom.Text);
                    EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                }

                DataSet ds = AgendaDataProvider.Instance.GetAgendaForReport(ForumId, AgendaName, SerialNumber, StartDate, EndDate, pageIndex, grdAgendaReport.PageSize);

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                    if (totalCount > grdAgendaReport.PageSize)
                    {
                        PopulatePager(totalCount, pageIndex);
                    }
                    else
                    {
                        //clear page data
                        rptPagerAgenda.DataSource = null;
                        rptPagerAgenda.DataBind();
                    }
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    grdAgendaReport.DataSource = ds;
                    grdAgendaReport.DataBind();
                    btnExport.Visible = true;

                }
                else
                {

                    grdAgendaReport.DataSource = null;
                    grdAgendaReport.DataBind();
                    btnExport.Visible = false;
                    btnExport.OnClientClick = "";
                }
                trAgendaReport.Visible = true;

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


        private void BindMinutesForReport(int pageIndex)
        {
            try
            {
                Guid ForumId = Guid.Empty;
                Guid MeetingId = Guid.Empty;
                Guid UserId = Guid.Empty;
                //string AgendaName = "";
                DateTime StartDate = Convert.ToDateTime("01/01/1978");
                DateTime EndDate = Convert.ToDateTime("01/01/1978");
                //string SerialNumber = "";

                UserId = Guid.Parse(Session["UserId"].ToString());
                if (ddlForum.SelectedValue.ToString() != "0")
                {
                    ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                }

                if (ddlMeeting.SelectedValue.ToString() != "0")
                {
                    MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                }

                if (txtFrom.Text != "" && txtTo.Text != "")
                {
                    StartDate = Convert.ToDateTime(txtFrom.Text);
                    EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                }

                DataSet ds = UploadMintesDataProvider.Instance.GetUploadMinutesByUserAcessForReport(UserId, ForumId, MeetingId, StartDate, EndDate, pageIndex, grdMinutesReports.PageSize);

                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                    if (totalCount > grdMinutesReports.PageSize)
                    {
                        PopulatePager(totalCount, pageIndex);
                    }
                    else
                    {
                        //clear page data
                        rptMinute.DataSource = null;
                        rptMinute.DataBind();
                    }
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    grdMinutesReports.DataSource = ds;
                    grdMinutesReports.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    grdMinutesReports.DataSource = null;
                    grdMinutesReports.DataBind();
                    btnExport.Visible = false;
                }

                trMinutesReport.Visible = true;
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        private void BindProceedingForReport(int pageIndex)
        {
            try
            {
                Guid ForumId = Guid.Empty;
                Guid MeetingId = Guid.Empty;
                DateTime StartDate = Convert.ToDateTime("01/01/1978");
                DateTime EndDate = Convert.ToDateTime("01/01/1978");

                if (ddlForum.SelectedValue.ToString() != "0")
                {
                    ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                }
                if (ddlMeeting.SelectedValue.ToString() != "0")
                {
                    MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                }
                if (txtFrom.Text != "" && txtTo.Text != "")
                {
                    StartDate = Convert.ToDateTime(txtFrom.Text);
                    EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                }
                DataSet ds = ProcedingDataProvider.Instance.GetProcedingsForReport(ForumId, MeetingId, StartDate, EndDate, pageIndex, grdProceedingReport.PageSize);


                if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                    if (totalCount > grdAgendaReport.PageSize)
                    {
                        PopulatePager(totalCount, pageIndex);
                    }
                    else
                    {
                        //clear page data
                        rptProceeding.DataSource = null;
                        rptProceeding.DataBind();
                    }
                }


                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    grdProceedingReport.DataSource = ds;
                    grdProceedingReport.DataBind();
                    btnExport.Visible = true;
                }
                else
                {
                    grdProceedingReport.DataSource = null;
                    grdProceedingReport.DataBind();
                    btnExport.Visible = false;
                }
                trProceedingReport.Visible = true;
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }



        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / grdProceedingReport.PageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                if (currentPage == 1 && pageCount <= 10)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage == 1 && pageCount > 10)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage != 1 && pageCount <= 10)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage != 1 && pageCount > 10 && currentPage != pageCount)
                {
                    int z = currentPage - 5;
                    int x, y;
                    if (z < 1)
                    {
                        z = 1;
                        for (int i = z; i <= 10; i++)
                        {
                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                        }
                    }
                    else if (z >= 1)
                    {
                        if (pageCount < currentPage + 4)
                        {
                            y = (z - ((currentPage + 4) - pageCount));
                            x = pageCount;
                        }
                        else
                        {
                            x = currentPage + 4;
                            y = z;
                        }
                        for (int i = y; i <= x; i++)
                        {
                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                        }
                    }
                }
                else if (currentPage == pageCount)
                {
                    for (int i = pageCount - 9; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }

                //for (int i = 1; i <= pageCount; i++)
                //{
                //    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                //}

            }

            if (ddlReport.SelectedValue == "Agenda")
            {
                rptPagerAgenda.DataSource = pages;
                rptPagerAgenda.DataBind();
            }
            if (ddlReport.SelectedValue == "Minutes")
            {
                rptMinute.DataSource = pages;
                rptMinute.DataBind();
            }
            if (ddlReport.SelectedValue == "Proceeding")
            {
                rptProceeding.DataSource = pages;
                rptProceeding.DataBind();
            }

            if (ddlReport.SelectedValue == "Attendance")
            {
                rptAttendance.DataSource = pages;
                rptProceeding.DataBind();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlReport.SelectedValue == "Meeting")
                {
                    IList<MeetingDomain> objMeetingDomain = MeetingDataProvider.Instance.GetMeetingReportByYear(Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1));
                    objMeetingDomain = objMeetingDomain.Where(p => Convert.ToDateTime(p.MeetingDate) >= Convert.ToDateTime(txtFrom.Text) && Convert.ToDateTime(p.MeetingDate) <= Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1)).OrderByDescending(p => Convert.ToDateTime(p.MeetingDate + " " + p.MeetingTime)).ToList();
                    if (objMeetingDomain.Count > 0)
                    {
                        btnExport.Visible = true;
                    }
                    else
                    {
                        btnExport.Visible = false;
                    }
                    grdMeetingReport.DataSource = objMeetingDomain;
                    grdMeetingReport.DataBind();
                    trMeetingReport.Attributes.Add("style", "display:table-row");
                    btnExport.Attributes.Add("style", "display:block");
                    trMeetingReport.Visible = true;

                    //Meeting MIS History
                    IList<MeetingDomain> objMeetingHistory = MeetingDataProvider.Instance.GetMeetingShadowReportByYear(Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1));
                    objMeetingHistory = objMeetingHistory.Where(p => Convert.ToDateTime(p.MeetingDate) >= Convert.ToDateTime(txtFrom.Text) && Convert.ToDateTime(p.MeetingDate) <= Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1)).OrderByDescending(p => Convert.ToDateTime(p.UpdatedOn)).ToList();

                    grdMeetingHistory.DataSource = objMeetingHistory;
                    grdMeetingHistory.DataBind();

                    if (chkMeetingHistory.Checked)
                    {
                        trMeetingMISDetails.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                        trMeetingMISDetails.Attributes.Add("style", "display:none");
                    }
                }
                else if (ddlReport.SelectedValue == "Agenda")
                {
                    BindAgendaForReport(1);
                }
                else if (ddlReport.SelectedValue == "Minutes")
                {
                    BindMinutesForReport(1);
                }
                else if (ddlReport.SelectedValue == "Proceeding")
                {
                    BindProceedingForReport(1);
                }
                else if (ddlReport.SelectedValue == "Attendance")
                {
                    BindAttendanceForReport(1);

                }
                rfvFrom.Enabled = false;
                rfvTo.Enabled = false;
                rfvForum.Enabled = false;
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }

        }
        protected void Page_Changed_Attendance(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            // ViewState["PageIndex_Proceeding"] = pageIndex;
            BindAttendanceForReport(pageIndex);

        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Guid MeetingId;
                if (ddlMeeting.SelectedValue != "0")
                {
                    MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
                    BindUser(MeetingId);
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

        protected void grdAttendanceReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAttendanceReport.PageIndex = e.NewPageIndex;
            BindAttendanceForReport(e.NewPageIndex);
        }
        protected void grdMinutesReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("view"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblEncrptionkey = (Label)row.Cells[0].FindControl("lblEncrptionkey");


                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                    string fileName = Convert.ToString(e.CommandArgument);

                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    byte[] decryptedBytes = null;
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (RijndaelManaged rijndael = new RijndaelManaged())
                        {
                            byte[] ba = Encoding.Default.GetBytes(MM.Core.Encryptor.DecryptString(lblEncrptionkey.Text));
                            var hexString = BitConverter.ToString(ba);
                            hexString = hexString.Replace("-", "");

                            byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                            var hexStrings = BitConverter.ToString(bsa);
                            hexStrings = hexStrings.Replace("-", "");

                            rijndael.Mode = CipherMode.CBC;
                            rijndael.Padding = PaddingMode.PKCS7;
                            rijndael.KeySize = 256;
                            rijndael.BlockSize = 128;
                            rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                            rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                            using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                        //Set the appropriate ContentType.
                        Response.ContentType = "Application/pdf";
                        //Get the physical path to the file.
                        // string FilePath = Server.MapPath(savePath + fileName);

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Minutes.pdf");
                        // Response.WriteFile(FilePath);
                        Response.BinaryWrite(ms.ToArray());
                        Response.Flush();
                    }


                    //Write the file directly to the HTTP content output stream.
                    //Response.WriteFile(FilePath);
                    //Response.End();
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

        protected void grdProceedingReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("view"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblEncrptionkey = (Label)row.Cells[0].FindControl("lblEncrptionkey");


                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                    string fileName = Convert.ToString(e.CommandArgument);

                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    byte[] decryptedBytes = null;
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (RijndaelManaged rijndael = new RijndaelManaged())
                        {
                            byte[] ba = Encoding.Default.GetBytes(MM.Core.Encryptor.DecryptString(lblEncrptionkey.Text));
                            var hexString = BitConverter.ToString(ba);
                            hexString = hexString.Replace("-", "");

                            byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                            var hexStrings = BitConverter.ToString(bsa);
                            hexStrings = hexStrings.Replace("-", "");

                            rijndael.Mode = CipherMode.CBC;
                            rijndael.Padding = PaddingMode.PKCS7;
                            rijndael.KeySize = 256;
                            rijndael.BlockSize = 128;
                            rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                            rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                            using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                        //Set the appropriate ContentType.
                        Response.ContentType = "Application/pdf";
                        //Get the physical path to the file.
                        // string FilePath = Server.MapPath(savePath + fileName);

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Proceding.pdf");
                        // Response.WriteFile(FilePath);
                        Response.BinaryWrite(ms.ToArray());
                        Response.Flush();
                    }


                    //Write the file directly to the HTTP content output stream.
                    //Response.WriteFile(FilePath);
                    //Response.End();
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


        protected void PageSize_Changed(object sender, EventArgs e)
        {
            BindAgendaForReport(1);
        }


        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            ViewState["PageIndex"] = pageIndex;
            BindAgendaForReport(pageIndex);

        }



        protected void Page_Changed_Proceeding(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            // ViewState["PageIndex_Proceeding"] = pageIndex;
            BindProceedingForReport(pageIndex);

        }

        protected void Page_Changed_Minute(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            // ViewState["PageIndex_Minute"] = pageIndex;
            BindMinutesForReport(pageIndex);

        }


        protected void grdAgendaReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAgendaReport.PageIndex = e.NewPageIndex;
            BindAgendaForReport(e.NewPageIndex);
        }

        public string Agenda(object MeetingId)
        {

            try
            {

                string strMeetingId = Convert.ToString(MeetingId);
                if (strMeetingId != "0")
                {
                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        StringBuilder strMenu = new StringBuilder("");
                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                        Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
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
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  SerialNumber = agend.SerialNumber,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                strMenu.Append("<div id='accordion'><ol id='sort'>");
                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
                                    Guid agendaId = agendaName[i].AgendaId;

                                    string SerialNumber = "";
                                    if (agendaName[i].SerialNumber.Length > 0)
                                    {
                                        SerialNumber = agendaName[i].SerialNumber + " : ";
                                    }
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
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText
                                                         })).ToList();


                                    if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                                    {
                                        if (subAgendaName.Count() > 0)
                                            strMenu.Append("<li id=" + agendaId + " class='group' ><h3>   " + SerialNumber + " " + agendaName[i].AgendaName + "</h3>");

                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        for (int ii = 0; ii <= agendaNames.Count() - 1; ii++)
                                        {
                                            if (!strDeletedAgenda.Contains(agendaNames[ii].Trim()))
                                            {
                                                string pdfName = string.IsNullOrEmpty(agendaPdfs[ii].Trim()) ? "Agenda.pdf" : agendaPdfs[ii].Trim();
                                                if (subAgendaName.Count() > 0)
                                                    strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ii].Trim().Substring(0, agendaNames[ii].Trim().Length - 4)) + ".aspx  Target='_blank' >" + pdfName + " </a> <br/> ");
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (subAgendaName.Count() > 0)
                                            strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + "</h3>");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {

                                        strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            string serialNo = "";
                                            serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                            if (serialNo.Trim().Length > 0)
                                            {
                                                serialNo = serialNo + " : ";
                                            }


                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                                string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                                string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName + "");
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span>  " + serialNo + subAgendaName[j].AgendaName + "");
                                                for (int ll = 0; ll <= agendaNames.Count() - 1; ll++)
                                                {
                                                    if (!strDeletedAgenda.Contains(agendaNames[ll].Trim()))
                                                    {
                                                        string pdfName = string.IsNullOrEmpty(agendaPdfs[ll].Trim()) ? "Agenda.pdf" : agendaPdfs[ll].Trim();
                                                        strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ll].Trim().Substring(0, agendaNames[ll].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                        // strMenu.Append("<br/> <a  href=ViewAgenda.aspx?file=" + agendaNames[ll] + " Target='_blank' >" + pdfName + " </a> ");
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName);// + "</li>");
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span>   " + serialNo + subAgendaName[j].AgendaName);// + "</li>");
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


                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');

                                                        // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' />" + subSubAgendaName[y].AgendaName + " ");
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + serialNoSub + subSubAgendaName[y].AgendaName + " ");

                                                        for (int pp = 0; pp <= agendaNames.Count() - 1; pp++)
                                                        {
                                                            if (!strDeletedAgenda.Contains(agendaNames[pp].Trim()))
                                                            {
                                                                string pdfName = string.IsNullOrEmpty(agendaPdfs[pp].Trim()) ? "Agenda.pdf" : agendaPdfs[pp].Trim();
                                                                strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[pp].Trim().Substring(0, agendaNames[pp].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                                // strMenu.Append("<br/> <a  href=ViewAgenda.aspx?file=" + agendaNames[pp] + " Target='_blank' >" + pdfName + " </a> ");
                                                            }

                                                        }
                                                    }
                                                    else
                                                    {
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subSubAgendaName[y].AgendaName);
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">  " + serialNoSub + subSubAgendaName[y].AgendaName);
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
                            return strMenu.ToString();


                        }
                        else
                        {
                            return "No agenda uploaded or approved";

                        }
                    }

                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid Agenda search";
                        Error.Visible = true;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
                return "";
            }

        }

        protected void grdProceedingReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProceedingReport.PageIndex = e.NewPageIndex;
            BindProceedingForReport(e.NewPageIndex);
        }

        protected void grdMinutesReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMinutesReports.PageIndex = e.NewPageIndex;
            BindMinutesForReport(e.NewPageIndex);
        }

        protected void grdAgendaReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("download"))
                {
                    IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(Guid.Parse(e.CommandArgument.ToString()));
                    DataTable dtAgenda = new DataTable();
                    List<string> AgendaFileList = new List<string>();
                    dtAgenda.Columns.Add(new DataColumn("AgendaPDFName"));

                    for (int j = 0; j <= objAgentList.Count - 1; j++)
                    {
                        string Uploaded = objAgentList[j].UploadedAgendaNote;
                        string Deleted = objAgentList[j].DeletedAgenda;

                        string[] arrAgendaFile = Uploaded.Replace(" ", "").Split(',');


                        string[] arrDeletedpdf = Deleted.Replace(" ", "").Split(',');


                        string[] agendaNames = Uploaded.Split(',');

                        if (!string.IsNullOrEmpty(Uploaded))
                        {
                            bool status = false;
                            for (int i = 0; i <= agendaNames.Count() - 1; i++)
                            {

                                if (!arrDeletedpdf.Contains(agendaNames[i].Trim()) && !status)
                                {
                                    DataRow row = dtAgenda.NewRow();
                                    row["AgendaPDFName"] = agendaNames[i].Trim();
                                    dtAgenda.Rows.Add(row);
                                    AgendaFileList.Add(agendaNames[i].Trim());
                                }
                            }
                        }
                    }

                    if (dtAgenda.Rows.Count > 0)
                    {
                        DataSet ds = new DataSet();//= AgendaDataProvider.Instance.GetAgendKeysByPDFName(dtAgenda);
                        using (SqlConnection sqlcon = new SqlConnection(MM.Core.Config.GetConnectionString("ConnectionString")))
                        {
                            try
                            {
                                sqlcon.Open();

                                using (SqlCommand cmd = new SqlCommand("[GetAgendaKeys]"))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = sqlcon;
                                    cmd.Parameters.AddWithValue("@AgendaPdfs", dtAgenda);

                                    SqlDataAdapter da = new SqlDataAdapter();
                                    da.SelectCommand = cmd;
                                    da.Fill(ds);
                                    sqlcon.Close();

                                    sqlcon.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                Error.Visible = true;

                                LogError objEr = new LogError();
                                objEr.HandleException(ex);
                            }
                            finally
                            {
                                sqlcon.Close();

                            }
                        }

                        List<string> arrAgendaFiles = objAgentList.Where(p => p.UploadedAgendaNote.Length > 0).Select(p => p.UploadedAgendaNote).ToList();
                        if (ds != null && ds.Tables[0].Rows.Count > 0 && AgendaFileList.Count > 0)
                        {

                            bool status = MergePdf(AgendaFileList, AgendaFileList[0].Trim(), ds.Tables[0]);
                            //if (!status)
                            //{
                            //    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            //    Error.Visible = true;
                            //}
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


        public bool MergePdf(List<string> files, string strFileName, DataTable dtAgenda)
        {
            try
            {

                if (files.Count == 0)
                {
                    return true;
                }

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                iTextSharp.text.Document document = new iTextSharp.text.Document();

                string File_Name = Guid.NewGuid().ToString() + ".pdf";//Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf");
                strFileName = File_Name;

                var query = dtAgenda.AsEnumerable().
                    Select(agenda => new
                    {
                        EncryptionKey = MM.Core.Encryptor.DecryptString(agenda.Field<string>("EncryptionKey").ToString()),
                        AgendaFile = agenda.Field<string>("AgendaName").ToString()

                    }).ToList();


                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Create));

                if (writer == null)
                {
                    return false;
                }

                // step 3: we open the document
                document.Open();

                //string AgendaEncrKey = "";
                //AgendaEncrKey = Session["EncryptionKey"].ToString();


                bool pdfCreate = false;
                foreach (string fileName in files)
                {
                    string AgendaEncrKey = query.Where(p => p.AgendaFile.Trim() == fileName).Select(p => p.EncryptionKey).FirstOrDefault();

                    string AgendaKey = "";
                    string fileNames = fileName.Split(',')[0];

                    if (fileName.Length <= 0 && !fileName.EndsWith(".pdf") || AgendaEncrKey.Length == 0)
                    {
                        break;
                    }
                    PdfReader reader = null;

                    string PdfPassword = EncryptionHelper.GetPassword(fileNames, AgendaKey);
                    if (PdfPassword != "")
                    {
                        byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(PdfPassword);


                        byte[] decryptedBytes = null;
                        fileNames = fileName.Split(',')[0].Trim().Substring(0, fileName.Length - 4) + "_merge.pdf";
                        byte[] bytesToBeEncrypted = File.ReadAllBytes(Server.MapPath(savePath + "/" + fileNames));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (RijndaelManaged rijndael = new RijndaelManaged())
                            {
                                byte[] ba = Encoding.Default.GetBytes(AgendaEncrKey);
                                var hexString = BitConverter.ToString(ba);
                                hexString = hexString.Replace("-", "");

                                byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                                var hexStrings = BitConverter.ToString(bsa);
                                hexStrings = hexStrings.Replace("-", "");

                                rijndael.Mode = CipherMode.CBC;
                                rijndael.Padding = PaddingMode.PKCS7;
                                rijndael.KeySize = 256;
                                rijndael.BlockSize = 128;
                                rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                                rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;


                                using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                                {
                                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                    cs.Close();
                                }
                                decryptedBytes = ms.ToArray();
                            }
                        }
                        File.WriteAllBytes(Server.MapPath(savePath + "/" + "oo" + fileNames), decryptedBytes);
                        reader = new PdfReader(Server.MapPath(savePath + "/" + "oo" + fileNames), password);
                    }
                    else
                    {
                        reader = new PdfReader(Server.MapPath(savePath + "/" + fileNames));
                    }
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                        pdfCreate = true;
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        //  writer.CopyAcroForm(reader);
                        writer.AddDocument(reader);
                    }

                    reader.Close();
                    if (File.Exists(Server.MapPath(savePath + "/" + "oo" + fileNames)))
                    {
                        File.Delete(Server.MapPath(savePath + "/" + "oo" + fileNames));
                    }
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
                PdfReader readers = new PdfReader(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"));
                using (MemoryStream memoryStream = new MemoryStream())
                {


                    PdfStamper stamper = new PdfStamper(readers, memoryStream);

                    stamper.Close();
                    readers.Close();

                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    // string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "Agenda.pdf");
                    // Response.WriteFile(FilePath);
                    Response.BinaryWrite(memoryStream.ToArray());
                    Response.Flush();


                    if (File.Exists((Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"))))
                    {
                        File.Delete(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"));
                    }


                }
                return true;
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Uploaded Pdf Error: " + ex.Message;//"Try again after some time";
                //Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);

                return false;
            }
        }

        //protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(ddlReport.SelectedValue!="0")
        //        {
        //            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
        //            BindForum(EntityId.ToString());
        //            txtFrom.Text = "";
        //            txtTo.Text = "";
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
                
    }
}