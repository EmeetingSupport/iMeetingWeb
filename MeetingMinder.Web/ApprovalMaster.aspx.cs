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
using System.Configuration;
using System.Net.Mail;
using System.Xml;
using System.Security.Cryptography;
using System.Net;

namespace MeetingMinder.Web
{
    public partial class ApprovalMaster : System.Web.UI.Page
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
            // Page.MaintainScrollPositionOnPostBack = true;
            if (Request.QueryString["fileMin"] != null)
            {
                try
                {
                    string fileName = Convert.ToString(Request.QueryString["fileMin"]);
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

            if (Request.QueryString["fileEntity"] != null)
            {
                try
                {
                    string fileName = Convert.ToString(Request.QueryString["fileEntity"]);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);

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
            if (Request.QueryString["fileForum"] != null)
            {
                try
                {
                    string fileName = Convert.ToString(Request.QueryString["fileForum"]);
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
                try
                {
                    Guid UserId = Guid.Parse(Session["UserId"].ToString());
                    //  BindEntity(UserId);
                    BindForum(UserId);
                    BindMeeting(UserId);
                    BindUser(UserId);
                    BindNotice(UserId);
                    BindAgendaMeetings(UserId);
                    BindMinutes(UserId);

                    //grdApprove.Visible = true;
                    grdForum.Visible = true;
                    grdMeeting.Visible = true;
                    grdUser.Visible = true;
                    grdNotice.Visible = true;
                    grdAgenda.Visible = true;
                    grdMinutes.Visible = true;

                    grdForumPending.Visible = false;
                    grdMeetingPending.Visible = false;
                    grdUserPending.Visible = false;
                    grdNoticePending.Visible = false;
                    grdAgendaPending.Visible = false;
                    grdMinutesPending.Visible = false;
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

        /// <summary>
        /// Event That Fired on Selected IndexChange 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        //protected void ddlMaster_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //      int SelectedVal = Convert.ToInt16(ddlMaster.SelectedValue);
        //        string SelectedVal = ddlMaster.SelectedValue;
        //        Guid UserId = Guid.Parse(Session["UserId"].ToString());
        //        if (SelectedVal != "0")
        //        {
        //            BindData(SelectedVal, UserId);
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

        /// <summary>
        /// Bind Data to grid
        /// </summary>
        /// <param name="SelectedVal">string specifying SelectedValue</param>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindData(string SelectedVal, Guid UserId)
        {
            try
            {
                //switch (SelectedVal)
                //{
                //case "Entity":
                //BindEntity(UserId);
                //break;

                //case "Forum":
                //BindForum(UserId);
                //break;

                //case "Meeting":
                //BindMeeting(UserId);
                //break;

                //case "User":
                //BindUser(UserId);
                //break;

                //case "Notice":
                //BindNotice(UserId);
                //break;

                //case "Agenda":
                //BindAgendaMeetings(UserId);
                //break;

                //case "Minutes":
                //BindMinutes(UserId);
                //break;
                //}


                BindForum(UserId);
                BindMeeting(UserId);
                BindUser(UserId);
                BindNotice(UserId);
                BindAgendaMeetings(UserId);
                BindMinutes(UserId);

                grdForum.Visible = true;
                grdMeeting.Visible = true;
                grdUser.Visible = true;
                grdNotice.Visible = true;
                grdAgenda.Visible = true;
                grdMinutes.Visible = true;
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
        /// Bind forums list to approve
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindForum(Guid UserId)
        {
            try
            {
                grdForum.Visible = true;
                grdForum.DataSource = ApprovalDataProvider.Instance.GetUnApprovedForums(UserId).OrderBy(p => p.ForumName).ToList(); ;
                grdForum.DataBind();

                grdApprove.Visible = false;
                grdMeeting.Visible = false;
                grdUser.Visible = false;
                grdNotice.Visible = false;
                grdAgenda.Visible = false;
                grdMinutes.Visible = false;
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
        /// Bind Meeting List to approve
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindMeeting(Guid UserId)
        {
            try
            {
                grdMeeting.Visible = true;

                IList<ApprovalDomain> objMeetingBind = new List<ApprovalDomain>();
                IList<ApprovalDomain> objMeeting = ApprovalDataProvider.Instance.GetUnApprovedMeetings(UserId).OrderBy(p => p.MeetingDate).ToList(); ;
                DateTime dtToday = DateTime.Now.Date;

                foreach (ApprovalDomain item in objMeeting)
                {
                    DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate + " " + item.MeetingTime);
                    if (dtMeeting >= dtToday)
                    {
                        objMeetingBind.Add(item);
                    }
                }

                grdMeeting.DataSource = objMeetingBind;
                grdMeeting.DataBind();

                grdApprove.Visible = false;
                grdForum.Visible = false;
                grdUser.Visible = false;
                grdNotice.Visible = false;
                grdAgenda.Visible = false;
                grdMinutes.Visible = false;
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
        /// Bind Users List to approve
        /// <param name="UserId">Guid specifying UserId</param>
        /// </summary>
        private void BindUser(Guid UserId)
        {
            try
            {
                grdUser.Visible = true;
                grdUser.DataSource = ApprovalDataProvider.Instance.GetUnApprovedUser(UserId).OrderBy(p => p.FirstName).ToList(); ;
                grdUser.DataBind();

                grdApprove.Visible = false;
                grdForum.Visible = false;
                grdMeeting.Visible = false;
                grdNotice.Visible = false;
                grdAgenda.Visible = false;
                grdMinutes.Visible = false;
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
        /// Bind notice list to grid
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindNotice(Guid UserId)
        {
            try
            {
                grdNotice.Visible = true;

                IList<NoticeDomain> objNoticeDomain = new List<NoticeDomain>();
                IList<NoticeDomain> objNotice = NoticeDataProvider.Instance.GetUnapprovedNotice(UserId).OrderBy(p => p.MeetingDate).ToList(); ;
                DateTime dtToday = DateTime.Now.Date;

                foreach (NoticeDomain item in objNotice)
                {
                    DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                    if (dtMeeting >= dtToday)
                    {
                        objNoticeDomain.Add(item);
                    }
                }

                //  grdNotice.DataSource = NoticeDataProvider.Instance.GetUnapprovedNotice(UserId);
                grdNotice.DataSource = objNoticeDomain;
                grdNotice.DataBind();

                grdApprove.Visible = false;
                grdForum.Visible = false;
                grdMeeting.Visible = false;
                grdUser.Visible = false;
                grdMinutes.Visible = false;
                grdAgenda.Visible = false;
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
        /// Bind agenda meetings by UserId
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindAgendaMeetings(Guid UserId)
        {
            try
            {
                grdAgenda.Visible = true;
                grdAgenda.DataSource = ApprovalDataProvider.Instance.GetUnApprovedMeetingForAgenda(UserId).OrderBy(p => p.ForumName).ToList();
                grdAgenda.DataBind();

                grdForum.Visible = false;
                grdMeeting.Visible = false;
                grdUser.Visible = false;
                grdNotice.Visible = false;
                grdApprove.Visible = false;
                grdMinutes.Visible = false;
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
        /// Bind entity list to approve
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindEntity(Guid UserId)
        {
            try
            {
                grdApprove.Visible = true;
                grdApprove.DataSource = ApprovalDataProvider.Instance.GetUnApprovedEntity(UserId).OrderBy(p => p.EntityName).ToList(); ;
                grdApprove.DataBind();

                grdForum.Visible = false;
                grdMeeting.Visible = false;
                grdUser.Visible = false;
                grdNotice.Visible = false;
                grdAgenda.Visible = false;
                grdMinutes.Visible = false;
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
        /// Bind unapproved upload minutes
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        private void BindMinutes(Guid UserId)
        {
            try
            {
                grdMinutes.Visible = true;
                grdMinutes.DataSource = UploadMintesDataProvider.Instance.GetUnapprovedMinutes(UserId).OrderBy(p => p.MeetingDate).ToList(); ;
                grdMinutes.DataBind();

                grdNotice.Visible = false;
                grdApprove.Visible = false;
                grdForum.Visible = false;
                grdMeeting.Visible = false;
                grdUser.Visible = false;
                grdAgenda.Visible = false;
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
        protected void grdForumRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //approve Forum item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");


                    string str_Ipad = chApprove.Checked ? " and Enabled On Ipad App" : "";

                    string strForumId = Convert.ToString(e.CommandArgument);
                    SaveAction("Approved", "Forum", Guid.Parse(UserId), Guid.Parse(strForumId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "Forum approved successfully" + str_Ipad;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Forum Approval", "Success", Convert.ToString(strForumId), "Forum approved successfully");

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Forum", "Approved", Guid.Parse(hdnUserId.Value));
                    }
                    BindData("Forum", Guid.Parse(UserId));
                    Info.Visible = true;
                }

                //reject Forum  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strForumId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Forum", Guid.Parse(UserId), Guid.Parse(strForumId), false);

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Forum", "Declined", Guid.Parse(hdnUserId.Value));
                    }

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Forum Approval", "Declined", Convert.ToString(strForumId), "Forum declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Forum declined successfully";
                    BindData("Forum", Guid.Parse(UserId));
                    Info.Visible = true;
                }


                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strEntityId = Convert.ToString(e.CommandArgument);
                    ForumDomain objForum = ForumDataProvider.Instance.Get(Guid.Parse(strEntityId));
                    string strLink = string.IsNullOrEmpty(objForum.MembersInfo) ? "<label  style='color:red'>No File uplaoded</label>" : "<a  href='ApprovalMaster.aspx?fileForum=" + objForum.MembersInfo + @"' target='_blank'  style='color:blue'>View</a>";

                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
                       <tr><td>
                                    <label>
                                     Forum Full Name:
                                    </label>
                                </td>
                          <td>
                  " + objForum.ForumName + @"
                                </td>

                                        </tr>
                                <tr><td>
                                    <label>
                                     Forum Short Name  :
                                    </label>
                                </td>

                                <td>
                        " + objForum.ForumShortName + @"
                                </td>
</tr>
<tr>
<td><label>
                                     Entity  Name  :
                                    </label></td>
<td>" + objForum.EntityName + @"</td>
</tr>


<tr>
                                   <td>
                                    <label>
                                    Enable :
                                    </label>
                                </td>
                                <td>
                              " + objForum.IsEnable + @"
                                  
                                </td></tr>

                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objForum.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr></table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Forum : " + objForum.ForumName + @"' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog( {   maxWidth:700,
                     width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
        /// Approve or reject 
        /// </summary>
        /// <param name="Action">string specifying Action</param>
        /// <param name="Transaction">string specifying Transaction</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="Id">Guid specifying Id</param>
        /// <param name="EnableOnIpad">bool specifying EnabledOnIpad</param>
        private void SaveAction(string Action, string Transaction, Guid UserId, Guid Id, bool EnableOnIpad)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (Action != "Approved")
                {
                    if (txtDecline.Text != "")
                    {
                        objUser.TextHtmlEncode(ref txtDecline);
                        if (!objUser.isValidChar(txtDecline.Text))
                        {
                            txtDecline.Text = "";
                            ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                            Error.Visible = true;
                            return;
                        }


                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter decline reason";
                        Error.Visible = true;
                        return;
                    }
                }
                string DeclineReson = txtDecline.Text.Trim();
                ApprovalDataProvider.Instance.UpdateStaus(UserId, Id, Action, Transaction, EnableOnIpad, DeclineReson);
                string scrip = "";
                if (Transaction != "User" && Action == "Approved")
                {
                    string url = Request.Url.ToString();
                    string[] webUrl = url.Split('/');
                    int count = 0;
                    if (webUrl.Count() == 4)
                    {
                        count = 1;
                        scrip = "var r=confirm('Do you want to send notification?'); if(r) {  var url = window.location.pathname; var pathArray = url.split('/'); var host = pathArray[1]; var newHost = '/IpadNotification.aspx';  window.location = " + webUrl[count] + " newHost; }  ";

                    }
                    else
                    {
                        count = webUrl.Count() - 2;
                        scrip = "var r=confirm('Do you want to send notification?'); if(r) {  var url = window.location.pathname; var pathArray = url.split('/'); var host = pathArray[1]; var newHost = '/IpadNotification.aspx';  window.location = '/" + webUrl[count] + "/IpadNotification.aspx'  }  ";

                    }


                    ClientScript.RegisterStartupScript(GetType(), "alert", scrip, true);
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
        protected void grdForum_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)ApprovalDataProvider.Instance.GetUnApprovedForums(UserId).AsDataTable();
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

                grdForum.DataSource = dv;
                grdForum.DataBind();
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
        protected void grdApproveRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //approve Entity item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");
                    string str_Ipad = chApprove.Checked ? " and Enabled On Ipad App" : "";
                    string strEntityId = Convert.ToString(e.CommandArgument);
                    SaveAction("Approved", "Entity", Guid.Parse(UserId), Guid.Parse(strEntityId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "Entity approved successfully" + str_Ipad;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Entity Approval", "Success", Convert.ToString(strEntityId), "Entity approved successfully");

                    BindData("Entity", Guid.Parse(UserId));
                    Info.Visible = true;
                }

                //reject Entity  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string strEntityId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Entity", Guid.Parse(UserId), Guid.Parse(strEntityId), false);

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Agenda", "Declined", Guid.Parse(hdnUserId.Value));
                    }

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Entity Approval", "Declined", Convert.ToString(strEntityId), "Entity declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Entity declined successfully";
                    BindData("Entity", Guid.Parse(UserId));
                    Info.Visible = true;
                }

                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strEntityId = Convert.ToString(e.CommandArgument);
                    EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(strEntityId));

                    string strLink = string.IsNullOrEmpty(objEntity.EntityMeeting) ? "<label  style='color:red'>No file uplaoded</label>" : "<a  href='ApprovalMaster.aspx?fileEntity=" + objEntity.EntityMeeting + @"' target='_blank'  style='color:blue'>View</a>";


                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
                       <tr><td>
                                    <label>
                                    Entity Full Name:
                                    </label>
                                </td>
                          <td>
                  " + objEntity.EntityName + @"
                                </td>

                                        </tr>
                                <tr><td>
                                    <label>
                                    Entity Short Name  :
                                    </label>
                                </td>

                                <td>
                        " + objEntity.EntityShortName + @"
                                </td>
<tr>
                                   <td>
                                    <label>
                                    Enable :
                                    </label>
                                </td>
                                <td>
                              " + objEntity.IsEnable + @"
                                  
                                </td></tr>

                                   <tr><td>
                                    <label>
                                    Entity Logo :
                                    </label>
                                </td>
                                <td>
<img src='img/Uploads/EntityLogo/" + objEntity.EntityLogo + @"'  height='50px' width='50px' alt='logo' ></img>
</td></tr> <tr><td>
                                    <label>
                                    Yearly Meetings  :
                                    </label>
                                </td>
                                <td>
" + strLink + @"
</td></tr>


                                  <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objEntity.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr>
</table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Entity : " + objEntity.EntityName + @"' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
        protected void grdApprove_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)ApprovalDataProvider.Instance.GetUnApprovedEntity(UserId).AsDataTable();
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

                grdApprove.DataSource = dv;
                grdApprove.DataBind();
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
        protected void grdMeetingCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //Meeting Entity item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");



                    string str_Ipad = chApprove.Checked ? " and enabled on ipad app" : "";
                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    SaveAction("Approved", "Meeting", Guid.Parse(UserId), Guid.Parse(strMeetingId), chApprove.Checked);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Meeting Approval", "Success", Convert.ToString(strMeetingId), "Meeting approved successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Meeting approved successfully" + str_Ipad;
                    BindData("Meeting", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Meeting", "Approved", Guid.Parse(hdnUserId.Value));
                    }
                }

                //Meeting Entity  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Meeting", Guid.Parse(UserId), Guid.Parse(strMeetingId), false);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Meeting Approval", "Declined", Convert.ToString(strMeetingId), "Meeting declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Meeting declined successfully";
                    BindData("Meeting", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Meeting", "Declined", Guid.Parse(hdnUserId.Value));
                    }
                }

                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    string[] ids = strMeetingId.Split(',');
                    MeetingDomain objMeeting = MeetingDataProvider.Instance.Get(Guid.Parse(ids[0]));


                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
<tr>
<td><label>
                                     Entity  Name  :
                                    </label></td>
<td>" + ids[1] + @"</td>
</tr>
                       <tr><td>
                                    <label>
                                     Forum Full Name:
                                    </label>
                                </td>
                          <td>
                  " + ids[2] + @"
                                </td>

                                        </tr>

<tr>
                                   <td>
                                    <label>
                                    Meeting Date :
                                    </label>
                                </td>
                                <td>
                              " + Convert.ToDateTime(objMeeting.MeetingDate).ToString("dd/MM/yyyy") + @"
                                  
                                </td></tr>
                            <tr><td>
                                    <label>
                                    Meeting Time  :
                                    </label>
                                </td>

                                <td>
                        " + objMeeting.MeetingTime + @"
                                </td>
</tr>
                            <tr><td>
                                    <label>
                                     Meeting Venue  :
                                    </label>
                                </td>

                                <td>
                        " + objMeeting.MeetingVenue + @"
                                </td>
</tr>
                            <tr><td>
                                    <label>
                                     Meeting Number  :
                                    </label>
                                </td>

                                <td>
                        " + objMeeting.MeetingNumber + @"
                                </td>
</tr>
                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objMeeting.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr></table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Meeting : " + objMeeting.MeetingVenue + @"' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({   maxWidth:700,
                     width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)ApprovalDataProvider.Instance.GetUnApprovedMeetings(UserId).AsDataTable();
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

        /// <summary>
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdApprove_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdApprove.PageIndex = e.NewPageIndex;
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("Entity", Guid.Parse(UserId));
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
        protected void grdForum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdForum.PageIndex = e.NewPageIndex;
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("Forum", Guid.Parse(UserId));
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
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("Meeting", Guid.Parse(UserId));
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
        /// gridview rowcommand event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //Meeting Entity item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");

                    string strApprUserId = Convert.ToString(e.CommandArgument);
                    SaveAction("Approved", "User", Guid.Parse(UserId), Guid.Parse(strApprUserId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "User approved successfully";

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "User Approval", "Success", Convert.ToString(strApprUserId), "User approved successfully");

                    if (chApprove.Checked)
                    {
                        SendEmail(Guid.Parse(strApprUserId));
                    }
                    BindData("User", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("User", "Approved", Guid.Parse(hdnUserId.Value));
                    }
                }

                //Meeting Entity  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "User", Guid.Parse(UserId), Guid.Parse(strMeetingId), false);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "User Approval", "Declined", Convert.ToString(strMeetingId), "User declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "User declined successfully";
                    BindData("User", Guid.Parse(UserId));
                    Info.Visible = true;


                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("User", "Declined", Guid.Parse(hdnUserId.Value));
                    }
                }

                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strUserId = Convert.ToString(e.CommandArgument);

                    UserDomain objUser = UserDataProvider.Instance.Get(Guid.Parse(strUserId));
                    IList<UserEntityDomain> objEntity = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(strUserId));
                    string strEntity = "";
                    foreach (UserEntityDomain entity in objEntity)
                    {
                        strEntity += entity.EntityName + "<br/>";
                    }
                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
<tr>
<td><label>
                                    Photograh  :
                                    </label></td>
<td> <img src='img/Uploads/ProfilePic/" + objUser.Photograph + @"' width='50px' height='50px' alt='Photo' /></td>
</tr>
     
<tr>
<td><label>
                                     First  Name  :
                                    </label></td>
<td>" + objUser.FirstName + @"</td>
</tr>
                       <tr><td>
                                    <label>
                                     Last Name:
                                    </label>
                                </td>
                          <td>
                  " + objUser.LastName + @"
                                </td>

                                        </tr>

<tr>
                                   <td>
                                    <label>
                                   Designation :
                                    </label>
                                </td>
                                <td>
                              " + objUser.Designation + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Pan No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.PANNo + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Residential Address :
                                    </label>
                                </td>
                                <td>
                              " + objUser.ResidentialAddress + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Residential Phone No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.ResidencePhone + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                 Mobile No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.Mobile + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   DIN No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.DINNumber + @"
                                  
                                </td></tr>
                            <tr><td>
                                    <label>
                                  Office Address  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.OfficeAddress + @"
                                </td>
</tr>
                            <tr><td>
                                    <label>
                                   Office Phone No.  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.OfficePhone + @"
                                </td>
</tr>
                       <tr><td>
                                    <label>
                                    Email Id 1  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.EmailID1 + @"
                                </td>
</tr>
                       <tr><td>
                                    <label>
                                    Email Id 2 :
                                    </label>
                                </td>

                                <td>
                        " + objUser.EmailID2 + @"
                                </td>
</tr>
                           <tr><td>
                                    <label>
                                   Secretary Name  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryName + @"
                                </td>
</tr>

  <tr><td>
                                    <label>
                                   Secretary Residental Phone No.:
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryResidentalPhone + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                Secretary Office Phone  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryOfficePhone + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Mobile No.  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryMobile + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Email Id 1  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryEmailID1 + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Email Id 2  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryEmailID2 + @"
                                </td>
</tr>

<tr><td>
                                    <label>
                                   User Enabled on Ipad :
                                    </label>
                                </td>

                                <td>
                        " + objUser.IsEnabledOnIpad + @"
                                </td>
</tr>

<tr><td>
                                    <label>
                                   User Enabled on Web  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.IsEnabledOnWebApp + @"
                                </td>
</tr>
 <tr><td>
                                    <label>
                                    Entity :
                                    </label>
                                </td>
                                <td>
" + strEntity + @"
</td></tr>
                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objUser.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr></table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','User : " + objUser.UserName + @"' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
        /// Grid view sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)ApprovalDataProvider.Instance.GetUnApprovedUser(UserId).AsDataTable();
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

                grdUser.DataSource = dv;
                grdUser.DataBind();
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
        /// grid view page index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUser.PageIndex = e.NewPageIndex;
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("User", Guid.Parse(UserId));
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
        /// Send email to username and password
        /// </summary>        
        /// <param name="UserId">Guid specifying UserId</param>
        public void SendEmail(Guid UserId)
        {
            try
            {
                UserDomain objUser = UserDataProvider.Instance.Get(UserId);
                if (objUser != null)
                {
                    //Email user name and password

                    string ToEmail = objUser.EmailID1;
                    if (ToEmail.Length == 0)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Email address of maker not exists. Email sending failed.";
                        Error.Visible = true;
                        return;
                    }
                    string UserName = objUser.UserName;
                    string Pass = objUser.Password;

                    //string EmailFrom = ConfigurationManager.AppSettings["Email"];
                    //string Password = ConfigurationManager.AppSettings["EPassword"];
                    string EmailSubject = "Your Username and Password";

                    MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                    SmtpClient SMTPServer = new SmtpClient();

                    SMTPServer = new SmtpClient();
                    string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                    string Password = "";// = ConfigurationManager.AppSettings["EPassword"];

                    XmlDocument xml = new XmlDocument();
                    if (!System.IO.File.Exists(Server.MapPath(".") + "/img/Uploads/Email.xml"))
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Email sending failed. Configuration setting missing";
                        Error.Visible = true;
                        return;
                    }

                    xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");
                    XmlNodeList serverlist = xml.SelectNodes("//email");
                    foreach (XmlNode servernodes in serverlist)
                    {
                        EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                        Password = servernodes.SelectSingleNode("password").InnerText;
                        SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                        SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                        SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                    }

                    //string strlink = @"http:///Login.aspx";
                    string EmailBody = "<p>";
                    EmailBody += "<table style=\"border-style:none;\"><tr style=\" border-style:none;\"><td>";

                    //<td><img src=" + ("http:// /images/logo.png") + "></img></td>
                    EmailBody += "<font color=\"Teal\"></b> IMEETING USERNAME AND PASSWORD</b></font></td></tr></table><BR />";
                    EmailBody += "Dear User <BR /><BR />";
                    EmailBody += "Following are your Login Details :-<BR />";

                    EmailBody += "<Table width=\"700px\" style=\"border-color:Teal; height:100px; width:50%; border-style:double;\">";
                    EmailBody += "<tr style=\" border-style:none;\"><td>";

                    EmailBody += "Login Email</td><td> :</td><td>" + UserName + "</td></tr>";
                    EmailBody += "<tr style=\" border-style:none;\"><td>Password</td><td> :</td><td>" + Pass + "</td></tr>";
                    EmailBody += "</table><br /><br /><br />";
                    EmailBody += "&nbsp; Regards <br /> iMeeting  Admin Team";
                    EmailBody += "</p>";


                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                    SMTPServer.UseDefaultCredentials = false;
                    SMTPServer.Credentials = SMTPUserInfo;

                    //  SMTPServer.EnableSsl = true;
                    MailAddress fromAddress = new MailAddress(EmailFrom, "iMeeting Admin");
                    objmail.From = fromAddress;
                    objmail.To.Add(ToEmail);
                    objmail.Body = EmailBody;
                    objmail.Subject = EmailSubject;

                    objmail.IsBodyHtml = true;

                    //SMTPServer.Host = ConfigurationManager.AppSettings["SmtpClient"];
                    //SMTPServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                    //SMTPServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    objmail.Headers.Add("NAME", "Admin");

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                    SMTPServer.Send(objmail);
                    objmail.To.Clear();

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
        /// Send Notification email to maker
        /// </summary>        
        /// <param name="UserId">Guid specifying UserId</param>
        public void SendNotifyEmail(string ApprovalType, string ApprovalStatus, Guid UserId)
        {
            try
            {
                UserDomain objUser = UserDataProvider.Instance.Get(UserId);
                if (objUser != null)
                {
                    //Email user name and password

                    string ToEmail = objUser.EmailID1;
                    if (ToEmail.Length == 0)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Email address of maker not exists. Email sending failed. ";
                        Error.Visible = true;
                        return;
                    }


                    //string EmailFrom = ConfigurationManager.AppSettings["Email"];
                    //string Password = ConfigurationManager.AppSettings["EPassword"];

                    MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                    SmtpClient SMTPServer = new SmtpClient();

                    SMTPServer = new SmtpClient();
                    string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                    string Password = "";// = ConfigurationManager.AppSettings["EPassword"];
                    //SMTPServer.Host = ConfigurationManager.AppSettings["SmtpClient"];
                    //SMTPServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                    //SMTPServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    XmlDocument xml = new XmlDocument();
                    if (!System.IO.File.Exists(Server.MapPath(".") + "/img/Uploads/Email.xml"))
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Email sending failed. Configuration setting missing";
                        Error.Visible = true;
                        return;
                    }
                    xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");
                    XmlNodeList serverlist = xml.SelectNodes("//email");
                    foreach (XmlNode servernodes in serverlist)
                    {
                        EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                        Password = servernodes.SelectSingleNode("password").InnerText;
                        SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                        SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                        SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                    }
                    string EmailSubject = "Approval Notification";

                    //string strlink = @"http:///Login.aspx";
                    string EmailBody = @"<p>
	Dear " + objUser.FirstName + @",</p>
<p>
	&nbsp;</p>
<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Your " + ApprovalType + @" is " + ApprovalStatus + @" by checker. Please login to imeeting app to view details.</p>
<p>
	&nbsp;</p>
<p>
	Regards<br />
	iMeeting Admin Team</p>";
                    //EmailBody += "<table style=\"border-style:none;\"><tr style=\" border-style:none;\"><td>";

                    ////<td><img src=" + ("http:// /images/logo.png") + "></img></td>
                    //EmailBody += "<font color=\"Teal\"></b>Approval Notification</b></font></td></tr></table><BR />";
                    //EmailBody += "Dear User <BR /><BR />";
                    //EmailBody += "Following are your Login Details :-<BR />";

                    //EmailBody += "<Table width=\"700px\" style=\"border-color:Teal; height:100px; width:50%; border-style:double;\">";
                    //EmailBody += "<tr style=\" border-style:none;\"><td>";

                    //EmailBody += "Login Email</td><td> :</td><td>" + UserName + "</td></tr>";
                    //EmailBody += "<tr style=\" border-style:none;\"><td>Password</td><td> :</td><td>" + Pass + "</td></tr>";
                    //EmailBody += "</table><br /><br /><br />";
                    //EmailBody += "&nbsp; Regards <br /> eMeeting  Admin Team";
                    EmailBody += "</p>";



                    System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                    SMTPServer.UseDefaultCredentials = false;
                    SMTPServer.Credentials = SMTPUserInfo;

                    //SMTPServer.EnableSsl = true;

                    MailAddress fromAddress = new MailAddress(EmailFrom, "iMeeting Admin");
                    objmail.From = fromAddress;
                    objmail.To.Add(ToEmail);
                    objmail.Body = EmailBody;
                    objmail.Subject = EmailSubject;

                    objmail.IsBodyHtml = true;

                    //SMTPServer.Host = ConfigurationManager.AppSettings["SmtpClient"];
                    //SMTPServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                    //SMTPServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                    objmail.Headers.Add("NAME", "Admin");

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                    SMTPServer.Send(objmail);
                    objmail.To.Clear();

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
        /// Gridview rowcommand event 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdNoticeRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //approve Forum item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");


                    string str_Ipad = chApprove.Checked ? " and Enabled On Ipad App" : "";
                    string strForumId = Convert.ToString(e.CommandArgument);
                    SaveAction("Approved", "Notice", Guid.Parse(UserId), Guid.Parse(strForumId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "Notice approved successfully" + str_Ipad;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Notice Approval", "Success", Convert.ToString(strForumId), "Notice approved successfully");

                    BindData("Notice", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Notice", "Approved", Guid.Parse(hdnUserId.Value));
                    }
                }

                //reject Forum  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string strForumId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Notice", Guid.Parse(UserId), Guid.Parse(strForumId), false);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Notice Approval", "Declined", Convert.ToString(strForumId), "Notice declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Notice declined successfully";
                    BindData("Notice", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Notice", "Declined", Guid.Parse(hdnUserId.Value));
                    }
                }


                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strNoticeId = Convert.ToString(e.CommandArgument);
                    string[] ids = strNoticeId.Split(',');
                    NoticeDomain objNotice = NoticeDataProvider.Instance.Get(Guid.Parse(ids[0]));

                    MeetingDomain objMeeting = MeetingDataProvider.Instance.GetMeetingWithEntity(objNotice.MeetingId);

                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
                       <tr><td>
                                    <label>
                                    Meeting Venue:
                                    </label>
                                </td>
                          <td>
                  " + ids[3] + @"
                                </td>

                                        </tr>
<tr>
<td><label>
                                     Meeting Date  :
                                    </label></td>
<td>" + ids[1] + @"</td>
</tr>
                                <tr><td>
                                    <label>
                                   Meeting Time  :
                                    </label>
                                </td>

                                <td>
                        " + ids[2] + @"
                                </td>
</tr>

<tr>
                                   <td>
                                    <label>
                                    Notice :
                                    </label>
                                </td>
                                <td>
                              " + objNotice.NoticeMessage.Replace("{Meeting_Number}", objMeeting.MeetingNumber).Replace("{Committee}", objMeeting.ForumName).Replace("{Entity}", objMeeting.EntityName).Replace("{Date}", Convert.ToDateTime(objMeeting.MeetingDate).ToString("D")).Replace("{Time}", objMeeting.MeetingTime).Replace("{Venue}", objMeeting.MeetingVenue) + @"
                                  
                                </td></tr>

                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objNotice.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr></table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Notice : Meeting' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdNotice_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)NoticeDataProvider.Instance.GetUnapprovedNotice(UserId).AsDataTable();
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

                grdNotice.DataSource = dv;
                grdNotice.DataBind();
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
        /// Gridview pageindex change event.
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdNotice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                grdNotice.PageIndex = e.NewPageIndex;
                BindNotice(UserId);
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
        /// Gridview rowcommand 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgendaCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //approve Forum item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    string strForumId = Convert.ToString(e.CommandArgument);
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");


                    string str_Ipad = chApprove.Checked ? " and Enabled On Ipad App" : "";
                    SaveAction("Approved", "Agenda", Guid.Parse(UserId), Guid.Parse(strForumId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "Agenda approved successfully" + str_Ipad;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Agenda Approval", "Success", Convert.ToString(strForumId), "Agenda approved successfully");

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Agenda", "Approved", Guid.Parse(hdnUserId.Value));
                    }

                    BindData("Agenda", Guid.Parse(UserId));
                    Info.Visible = true;
                }

                //reject Forum  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    string strForumId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Agenda", Guid.Parse(UserId), Guid.Parse(strForumId), false);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Agenda Approval", "Declined", Convert.ToString(strForumId), "Agenda declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Agenda declined successfully";
                    BindData("Agenda", Guid.Parse(UserId));
                    Info.Visible = true;
                }


                if (e.CommandName.ToLower().Equals("view"))
                {

                    string strMeetingId = Convert.ToString(e.CommandArgument);

                    Guid meetingId;
                    if (Guid.TryParse(strMeetingId, out meetingId))
                    {
                        ViewState["MeetingId"] = meetingId;

                        StringBuilder strMenu = new StringBuilder("");

                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        HiddenField hdnChekerUserId = (HiddenField)row.Cells[0].FindControl("hdnChekerUserId");

                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetAgendabyMeetingId(meetingId, Guid.Parse(hdnChekerUserId.Value));

                        // if (objAgentList.Count > 0)
                        //{
                        //    //get only parent agenda
                        //    var objParentAgenda = from agend in objAgentList
                        //                          where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        //                          select agend;



                        //    //get only sub agenda
                        //    var subAgendaList = from agend in objAgentList
                        //                        where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        //                        select agend;

                        //    //get agenda name/note and agenda id
                        //    var agendaName = (from agend in objParentAgenda

                        //                      select (new
                        //                      {
                        //                          AgendaName = agend.AgendaName,
                        //                          AgendaId = agend.AgendaId,
                        //                          UplaodedAgenda = agend.UploadedAgendaNote

                        //                      })).Distinct().ToList();

                        //    if (agendaName.Count() > 0)
                        //    {
                        //        strMenu.Append("<div id='accordion'><ol id='sort'>");
                        //        for (int i = 0; i <= agendaName.Count() - 1; i++)
                        //        {
                        //            Guid agendaId = agendaName[i].AgendaId;
                        //            if (agendaName[i].UplaodedAgenda.Length > 0)
                        //            {
                        //                strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");
                        //            }
                        //            else
                        //            {
                        //                strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + agendaName[i].AgendaName + "</h3>");
                        //            }

                        //            //get sub agenda for parent agenda 
                        //            var subAgendaName = (from subAgenda in subAgendaList
                        //                                 where (subAgenda.ParentAgendaId == agendaId)
                        //                                 select (new
                        //                                 {
                        //                                     AgendaName = subAgenda.AgendaName,
                        //                                     AgendaId = subAgenda.AgendaId,
                        //                                     UplaodedAgenda = subAgenda.UploadedAgendaNote
                        //                                 })).ToList();

                        //            //attach sub agenda to parent agenda list element
                        //            if (subAgendaName.Count() > 0)
                        //            {
                        //                strMenu.Append("<div><ol type=a class=ddrag>");
                        //                for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                        //                {
                        //                    if (subAgendaName[j].UplaodedAgenda.Length > 0)
                        //                    {
                        //                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><a href=ViewAgenda.aspx?file=" + subAgendaName[j].UplaodedAgenda + "  Target='_blank'> " + subAgendaName[j].AgendaName + "</a>");
                        //                    }
                        //                    else
                        //                    {
                        //                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + ">" + subAgendaName[j].AgendaName);// + "</li>");
                        //                    }
                        //                    Guid subAgendaId = subAgendaName[j].AgendaId;
                        //                    //Get sub sub agenda
                        //                    var subSubAgendaName = (from subAgenda in subAgendaList
                        //                                            where (subAgenda.ParentAgendaId == subAgendaId)
                        //                                            select (new
                        //                                            {
                        //                                                AgendaName = subAgenda.AgendaName,
                        //                                                AgendaId = subAgenda.AgendaId,
                        //                                                UplaodedAgenda = subAgenda.UploadedAgendaNote
                        //                                            })).ToList();
                        //                    if (subSubAgendaName.Count() > 0)
                        //                    {
                        //                        //attach sub sub agenda to parent agenda list element
                        //                        strMenu.Append("<ol class=inddrag type=i>");
                        //                        for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                        //                        {
                        //                            if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                        //                            {
                        //                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><a href=ViewAgenda.aspx?file=" + subSubAgendaName[y].UplaodedAgenda + "  Target='_blank'> " + subSubAgendaName[y].AgendaName + "</a>");
                        //                            }
                        //                            else
                        //                            {
                        //                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + subSubAgendaName[y].AgendaName);
                        //                            }
                        //                        }
                        //                        strMenu.Append("</ol>");
                        //                    }
                        //                    strMenu.Append("</li>");
                        //                }
                        //                strMenu.Append("</ol></div></li>");
                        //            }

                        //        }
                        //        strMenu.Append("</ol></div>");
                        //    }
                        //}

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
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,

                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            Dictionary<string, int> objSerial = new Dictionary<string, int>();
                            Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";
                                strMenu.Append("<div id='accordion'><ol class='ddrag'>");
                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
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
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             Presenter = subAgenda.Presenter,
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

                                    if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                                    {
                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");

                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");

                                        for (int ii = 0; ii <= agendaNames.Count() - 1; ii++)
                                        {
                                            if (!strDeletedAgenda.Contains(agendaNames[ii].Trim()))
                                            {
                                                string pdfName = string.IsNullOrEmpty(agendaPdfs[ii].Trim()) ? "Agenda.pdf" : agendaPdfs[ii].Trim();
                                                strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ii].Trim().Substring(0, agendaNames[ii].Trim().Length - 4)) + ".aspx  Target='_blank' >" + pdfName + " </a> <br/> ");
                                            }

                                        }


                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        
                                        strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            Presenter = "";
                                            string serialNo = "";

                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                            }

                                            serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                            if (serialNo.Trim().Length > 0)
                                            {
                                                serialNo = serialNo + " : ";
                                            }
                                            //if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                            //{
                                            //    if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                            //    {
                                            //        if (subAgendaName[j].SerialNumberType != "Other")
                                            //        {
                                            //            objSerial[subAgendaName[j].SerialNumber] += 1;
                                            //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        if (subAgendaName[j].SerialNumberType != "Other")
                                            //        {
                                            //            objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                            //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //}

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                                string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                                string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><a href=ViewAgenda.aspx?file=" + subAgendaName[j].UplaodedAgenda + "  Target='_blank'> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + "</a>");                                      

                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + " " + serialNo + subAgendaName[j].AgendaName + Presenter + "");
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
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + serialNo + subAgendaName[j].AgendaName + Presenter);// + "</li>");
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
                                                                        Presenter =subAgenda.Presenter,
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
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";
                                                    serialNoSub = CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText);
                                                    if (serialNoSub.Trim().Length > 0)
                                                    {
                                                        serialNoSub = serialNoSub + " : ";
                                                    }
                                                    //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                                    //{
                                                    //    if (objSerial.ContainsKey(subSubAgendaName[y].SerialNumber))//objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                                    //    {
                                                    //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                                    //        {
                                                    //            //objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";

                                                    //            objSerial[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
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
                                                    //            objSerial.Add(subSubAgendaName[y].SerialNumber, 1);
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";

                                                    //            //objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                                    //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
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

                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + serialNoSub + subSubAgendaName[y].AgendaName + Presenter + "");

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
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter);
                                                    }
                                                }
                                                strMenu.Append("</ol>");
                                            }
                                            else
                                            {
                                                strMenu.Append("</li>");
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

                            //  lblList.Text = strMenuList;
                        }
                        lblDetails.Text = strMenu.ToString();
                    }


                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Agenda : Meeting' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);
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
        /// Gridview page sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)ApprovalDataProvider.Instance.GetUnApprovedMeetingForAgenda(UserId).AsDataTable();
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

                grdAgenda.DataSource = dv;
                grdAgenda.DataBind();
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
        /// Gridview pageindex change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAgenda.PageIndex = e.NewPageIndex;
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("Agenda", Guid.Parse(UserId));
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
        protected void grdMinutesRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //Meeting Entity item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    string strMeetingId = Convert.ToString(e.CommandArgument);

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    CheckBox chApprove = (CheckBox)row.Cells[0].FindControl("chkEnable");

                    string str_Ipad = chApprove.Checked ? " and enabled on ipad app" : "";
                    SaveAction("Approved", "Minutes", Guid.Parse(UserId), Guid.Parse(strMeetingId), chApprove.Checked);
                    ((Label)Info.FindControl("lblName")).Text = "Upload minutes approved successfully" + str_Ipad;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Upload Minutes Approval", "Success", Convert.ToString(strMeetingId), "Upload minutes approved successfully");


                    BindData("Minutes", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Minutes", "Approved", Guid.Parse(hdnUserId.Value));
                    }
                }

                //Meeting Entity  item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    SaveAction("Declined", "Minutes", Guid.Parse(UserId), Guid.Parse(strMeetingId), false);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Upload Minutes Approval", "Declined", Convert.ToString(strMeetingId), "Upload minutes declined successfully");

                    ((Label)Info.FindControl("lblName")).Text = "Upload minutes declined successfully";
                    BindData("Minutes", Guid.Parse(UserId));
                    Info.Visible = true;

                    CheckBox chkNotify = (CheckBox)row.Cells[0].FindControl("chkNotify");
                    if (chkNotify.Checked)
                    {
                        HiddenField hdnUserId = (HiddenField)row.Cells[0].FindControl("hdnMaker");
                        SendNotifyEmail("Minutes", "Declined", Guid.Parse(hdnUserId.Value));
                    }
                }

                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strMinutes = Convert.ToString(e.CommandArgument);
                    string[] ids = strMinutes.Split(',');
                    UploadMinutesDomain objUploadMinutes = UploadMintesDataProvider.Instance.Get(Guid.Parse(ids[0]));

                    ViewState["EncryptionKey"] = objUploadMinutes.EncryptionKey;
                    ViewState["UploadMom"] = objUploadMinutes.UploadFile;

                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
<tr>
<td><label>
                                     Meeting Time  :
                                    </label></td>
<td>" + ids[2] + @"</td>
</tr>
                       <tr><td>
                                    <label>
                                    Meeting Date:
                                    </label>
                                </td>
                          <td>
                  " + ids[1] + @"
                                </td>

                                        </tr>


                            <tr><td>
                                    <label>
                                     Meeting Venue  :
                                    </label>
                                </td>

                                <td>
                        " + ids[3] + @"
                                </td>
</tr>
                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objUploadMinutes.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr>

                                   <tr><td>
                                    <label>
                                    Upload Minutes  :
                                    </label>
                                </td>
                                <td>
 <a  href='javascript:void(0)'  onclick='viewMom();'  style='color:blue'>View</a>
</td></tr>

</table>
</div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','Upload Minutes' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog(
{
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

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
        /// Gridview Sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMinutes_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                DataTable dt = (DataTable)UploadMintesDataProvider.Instance.GetUnapprovedMinutes(UserId).AsDataTable();
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

                grdMinutes.DataSource = dv;
                grdMinutes.DataBind();
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
        /// Gridview PageIndexChanging event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMinutes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMinutes.PageIndex = e.NewPageIndex;
                string UserId = Convert.ToString(Session["UserId"]);
                BindData("Minutes", Guid.Parse(UserId));
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void rdbCheckers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string rdbChe = rdbCheckers.SelectedValue;
                if (rdbChe.ToLower().Equals("history"))
                {
                    DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();
                    grdForumPending.DataSource = dsAllPending.Tables[1];
                    grdForumPending.DataBind();

                    grdMeetingPending.DataSource = dsAllPending.Tables[2];
                    grdMeetingPending.DataBind();

                    grdNoticePending.DataSource = dsAllPending.Tables[3];
                    grdNoticePending.DataBind();

                    grdUserPending.DataSource = dsAllPending.Tables[0];
                    grdUserPending.DataBind();

                    grdAgendaPending.DataSource = dsAllPending.Tables[4];
                    grdAgendaPending.DataBind();

                    grdMinutesPending.DataSource = dsAllPending.Tables[5];
                    grdMinutesPending.DataBind();

                    grdForum.Visible = false;
                    grdMeeting.Visible = false;
                    grdUser.Visible = false;
                    grdNotice.Visible = false;
                    grdAgenda.Visible = false;
                    grdMinutes.Visible = false;

                    grdForumPending.Visible = true;
                    grdMeetingPending.Visible = true;
                    grdUserPending.Visible = true;
                    grdNoticePending.Visible = true;
                    grdAgendaPending.Visible = true;
                    grdMinutesPending.Visible = true;
                }

                if (rdbChe.ToLower().Equals("dashbord"))
                {
                    Guid UserId = Guid.Parse(Session["UserId"].ToString());
                    //  BindEntity(UserId);
                    BindForum(UserId);
                    BindMeeting(UserId);
                    BindUser(UserId);
                    BindNotice(UserId);
                    BindAgendaMeetings(UserId);
                    BindMinutes(UserId);

                    //grdApprove.Visible = true;
                    grdForum.Visible = true;
                    grdMeeting.Visible = true;
                    grdUser.Visible = true;
                    grdNotice.Visible = true;
                    grdAgenda.Visible = true;
                    grdMinutes.Visible = true;

                    grdForumPending.Visible = false;
                    grdMeetingPending.Visible = false;
                    grdUserPending.Visible = false;
                    grdNoticePending.Visible = false;
                    grdAgendaPending.Visible = false;
                    grdMinutesPending.Visible = false;
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

        protected void grdForumPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdForumPending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();
                grdForumPending.DataSource = dsAllPending.Tables[1];
                grdForumPending.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdMeetingPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMeetingPending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();

                grdMeetingPending.DataSource = dsAllPending.Tables[2];
                grdMeetingPending.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdUserPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUserPending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();

                grdUserPending.DataSource = dsAllPending.Tables[0];
                grdUserPending.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdNoticePending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdNoticePending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();

                grdNoticePending.DataSource = dsAllPending.Tables[3];
                grdNoticePending.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdAgendaPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAgendaPending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();

                grdAgendaPending.DataSource = dsAllPending.Tables[4];
                grdAgendaPending.DataBind();


            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdMinutesPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMinutesPending.PageIndex = e.NewPageIndex;
                DataSet dsAllPending = CheckerDataProvider.Instance.GetPendingDetails();

                grdMinutesPending.DataSource = dsAllPending.Tables[5];
                grdMinutesPending.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void lnkViewMom_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["UploadMom"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                string FilePath = Server.MapPath(savePath + fileName);
                byte[] decryptedBytes = null;
                byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged rijndael = new RijndaelManaged())
                    {
                        byte[] ba = Encoding.Default.GetBytes(ViewState["EncryptionKey"].ToString());
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
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }
    }
}