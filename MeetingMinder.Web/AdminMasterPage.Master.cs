using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Domain;
using MM.Data;
using System.IO;
using MM.Core;
using System.Web.Security;
using System.Configuration;
using System.Web.Helpers;

namespace MeetingMinder.Web
{
    public partial class AdminMasterPage : System.Web.UI.MasterPage
    {

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(Application["Counter"].ToString().Equals("2"))
            //{
            //    Application["Counter"] = 3;
            //    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            //    Response.Redirect("default.aspx");            
            //}
            try
            {
                if (IsPostBack)
                {
                    //AntiForgery
                    //AntiForgery.Validate();
                }

                string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
                if (AuthenticationType.ToLower().Equals("ad"))
                {
                    ulChangePassword.Visible = false;
                }
                else
                {
                    ulChangePassword.Visible = true;
                }


                AntiforgeryChecker.Check(this, AntiCsrftoken);
                string s = Response.StatusDescription;


                //check user user session
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    if (!AddLoginList(Session["UserId"].ToString()))
                    {
                        Response.Redirect("Login.aspx?PageName=Defaults");
                    }
                    //if (HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Value != "")
                    {
                        //if (HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path == "/")
                        //{
                        //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Value = Session.SessionID;
                        //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = "/emeeting";
                        //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Secure = true;
                        //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Domain = "/jmjgreen";
                        //}
                    }
                    //if (Request.UrlReferrer != null)
                    //{
                    //    string prvPage = Request.UrlReferrer.ToString();
                    //    if (HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path == "/" && !prvPage.ToString().ToLower().EndsWith("login.aspx"))
                    //    {
                    //        HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Value = Session.SessionID;
                    //        HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath; //"/emeeting/";
                    //        HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Secure = true;
                    //        string domain = Request.Url.GetBaseDomain();
                    //        domain = domain.ToLower().Replace("www.", "");
                    //        if (domain != Request.Url.DnsSafeHost)
                    //            HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Domain = domain;
                    //        //domain = domain.ToLower().Replace("www.", "");
                    //       // HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Domain = domain;//"/jmjgreen";

                    //    }
                    //}
                    //if (Response.Cookies["Anti-CSRFToken"] != null)
                    //{
                    //    if (HttpContext.Current.Response.Cookies["Anti-CSRFToken"].Path == "/")
                    //    {
                    //         HttpContext.Current.Response.Cookies["Anti-CSRFToken"].Value = Session.SessionID;
                    //        HttpContext.Current.Response.Cookies["Anti-CSRFToken"].Path = Request.ApplicationPath;
                    //        HttpContext.Current.Response.Cookies["Anti-CSRFToken"].Secure = true;
                    //        HttpContext.Current.Response.Cookies["Anti-CSRFToken"].Domain = Request.Url.Host.Replace("www.", "");
                    //    }

                    //}
                    //Check user has changed password
                    if (Session["IsAuthorized"] != null)
                    {
                        bool IsAuthorized = (bool)Session["IsAuthorized"];
                        if (!IsAuthorized)
                        {
                            Response.Redirect("AnswerQuestion.aspx");
                        }
                    }

                    bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);
                    bool EntityCount = Convert.ToBoolean(Session["EntityCount"]); ;

                    if (EntityCount)
                    {
                        lnkSwitch.Visible = true;
                    }
                    else
                    {
                        lnkSwitch.Visible = false;
                    }

                    if (IsSuperAdmin)
                    {
                        trAllUserMaster.Visible = true;
                        trUpcomingMeeting.Visible = true;
                    }

                    lblLoginName.Text = Convert.ToString(Session["FName"]);
                    //check session roll existance
                    if (Session["Roll"] == null)
                    {
                        RollDomain objRoll = RollDataProvider.Instance.GetRollByUserId(Guid.Parse(Session["UserId"].ToString()));

                        Session["Roll"] = objRoll;
                        BindRolls();

                    }
                    else
                    {
                        BindRolls();
                    }

                    if (Session["UserId"] != null && Session["EntityId"] == null)
                    {
                        Response.Redirect("Redirect.aspx");
                    }

                    if (Session["EntityId"] == null)
                    {
                        BindEntity();
                    }
                    else
                    {
                        lblEntityName.Text = Session["EntityName"].ToString();
                        if (Session["EntityName"].ToString().Length > 19)
                        {
                            lblLoginName.Text = Convert.ToString(Session["FName"]) + "(" + Session["EntityName"].ToString().Substring(0, 19).TrimEnd() + "...)";
                        }
                        else
                        {
                            lblLoginName.Text = Convert.ToString(Session["FName"]) + "(" + Session["EntityName"].ToString() + ")";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("XSRF Attack Detected!"))
                {
                    //UserControl Error = (UserControl)ContentPlaceHolder1.FindControl("Error");
                    //if (Error != null)
                    //{
                    //    ((Label)Error.FindControl("lblError")).Text = " Something went wrong";
                    //    Error.Visible = true;

                    //    Response.Flush();
                    //    return;
                    //}

                    Response.Redirect("error.aspx?ErrorCode=0");
                }

                //  throw;
            }
        }

        /// <summary>
        /// Bind 1st entity from assigned list
        /// </summary>
        private void BindEntity()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"].ToString());
                IList<UserEntityDomain> objEntity = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)); //EntityDataProvider.Instance.Get();
                if (objEntity != null && objEntity.Count > 0)
                {
                    Session["EntityId"] = objEntity[0].EntityId;
                    Session["EntityName"] = objEntity[0].EntityName;
                    Session["EncryptionKey"] = objEntity[0].EncryptionKey;
                    lblEntityName.Text = objEntity[0].EntityName;
                }
                else
                {
                    Session["EntityId"] = null;
                }
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Bind links according to rolls
        /// </summary>
        private void BindRolls()
        {
            try
            {
                RollDomain objRoll = (RollDomain)Session["Roll"];
                string strPath = Path.GetFileName(Request.PhysicalPath);
                if (objRoll != null)
                {
                    trAccessRights.Visible = objRoll.AccessRightMaster;

                    // trApprovalMaster.Visible = objRoll.ApprovalMaster;
                    trUserMaster.Visible = objRoll.UserMaster;
                    trEntityMaster.Visible = objRoll.EntityMaster;
                    trRollMaster.Visible = objRoll.RollMaster;
                    if (!objRoll.AccessRightMaster && !objRoll.ApprovalMaster && !objRoll.UserMaster && !objRoll.EntityMaster && !objRoll.RollMaster)
                    {

                        searchlink.Visible = false;
                    }

                    lnkReports.Visible = objRoll.Report;
                    if (!objRoll.GlobalSetting && !objRoll.IpadNotification && !objRoll.MasterAgenda && !objRoll.EmailNotification && !objRoll.Meeting && !objRoll.Agenda && !objRoll.PhotoGallery && !objRoll.BoardofDirectore && !objRoll.UploadMinutes && !objRoll.MeetingSchedule && !objRoll.AboutUs && !objRoll.RevisedPdf && !objRoll.DeviceManager && !objRoll.EmailHistory && !objRoll.PublishHistory && !objRoll.HeaderMaster && !objRoll.SectionMaster && !objRoll.SerialNoMaster && !objRoll.TabSetting && !objRoll.ProceedingMaster && !objRoll.MeetingRetention)
                    {
                        lnkTransactions.Visible = false;
                    }
                    // lnkTransactions.Visible = objRoll.Transaction;

                    trAgenda.Visible = objRoll.Agenda;
                    trMeeting.Visible = objRoll.Meeting;
                    trGlobalSetting.Visible = objRoll.GlobalSetting;
                    trIpadNoti.Visible = objRoll.IpadNotification;
                    trEmailNoti.Visible = objRoll.EmailNotification;
                    trPhoto.Visible = objRoll.PhotoGallery;
                    trUploadMin.Visible = objRoll.UploadMinutes;
                    TrBod.Visible = objRoll.BoardofDirectore;
                    trMasterAgenda.Visible = objRoll.MasterAgenda;

                    trviewAgenda.Visible = objRoll.AgendaView;
                    trViewMinutes.Visible = objRoll.MinutesView;
                    trViewNotice.Visible = objRoll.NoticeView;
                    trMeetingView.Visible = objRoll.MeetingView;

                    trAgendaAccess.Visible = objRoll.RevisedPdf;

                    trAboutUs.Visible = objRoll.AboutUs;
                    trMeetingSchedule.Visible = objRoll.MeetingSchedule;
                    trDeviceManger.Visible = objRoll.DeviceManager;

                    trEmailHistory.Visible = objRoll.EmailHistory;
                    trPublishHistroy.Visible = objRoll.PublishHistory;

                    trSectionMaster.Visible = objRoll.SectionMaster;

                    trHeaderMaster.Visible = objRoll.HeaderMaster;

                    trAgendaMarker.Visible = objRoll.AgendaMarker;

                    trAttendance.Visible = objRoll.AttendanceView;
                    trViewProceeding.Visible = objRoll.ProceedingView;

                    trSerialNo.Visible = objRoll.SerialNoMaster;
                    trProceedingMaster.Visible = objRoll.ProceedingMaster;
                    trTabSetting.Visible = objRoll.TabSetting;

                    trMeetingRetention.Visible = objRoll.MeetingRetention;

                    bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                    if (!objRoll.MeetingView && !objRoll.MinutesView && !objRoll.NoticeView && !objRoll.AgendaView && !objRoll.AgendaMarker && !objRoll.AttendanceView && !objRoll.ProceedingView && !IsSuperAdmin)
                    {
                        lnkView.Visible = false;
                    }

                    //lnkView.Visible = objRoll.View;

                    // lnkApprovals.Visible = objRoll.Approval;
                    lnkApprovals.Visible = objRoll.Approval;

                    if (!objRoll.AccessRightMaster && strPath.ToLower().Equals("accessrightadmin.aspx") || !objRoll.AccessRightMaster && strPath.ToLower().Equals("forumaccess.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.AttendanceView && strPath.ToLower().Equals("viewattendance.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }
                    if (!objRoll.ProceedingView && strPath.ToLower().Equals("viewproceeding.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }


                    if (!objRoll.ProceedingMaster && strPath.ToLower().Equals("proceedingmaster.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.TabSetting && strPath.ToLower().Equals("globaltab.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.SerialNoMaster && strPath.ToLower().Equals("serialnumbermaster.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.AboutUs && strPath.ToLower().Equals("aboutus.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.MeetingSchedule && strPath.ToLower().Equals("yearlymeeting.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.UserMaster && strPath.ToLower().Equals("usermaster.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    //if (!objRoll.EntityMaster && strPath.ToLower().Equals("entitymaster.aspx"))
                    //{
                    //    Response.Redirect("Default.aspx");
                    //}

                    if (!objRoll.EntityMaster && strPath.ToLower().Equals("departmentmaster.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.RollMaster && strPath.ToLower().Equals("rolemaster.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    //if (!objRoll.RollMaster && strPath.ToLower().Equals("rollmaster.aspx"))
                    //{
                    //    Response.Redirect("Default.aspx");
                    //}

                    if (!objRoll.Report && strPath.ToLower().Equals("reports.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.Agenda && (strPath.ToLower().Equals("createagenda.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.BoardofDirectore && (strPath.ToLower().Equals("keyinformation.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.EmailNotification && (strPath.ToLower().Equals("emailnotifications.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.GlobalSetting && (strPath.ToLower().Equals("globalsetting.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.IpadNotification && (strPath.ToLower().Equals("ipadnotification.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.MasterAgenda && (strPath.ToLower().Equals("masteragenda.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.Meeting && (strPath.ToLower().Equals("createmeeting.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.PhotoGallery && (strPath.ToLower().Equals("photogallery.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.UploadMinutes && (strPath.ToLower().Equals("uploadminutes.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.MeetingView && strPath.ToLower().Equals("meetingschedule.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.MinutesView && strPath.ToLower().Equals("viewminutes.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.NoticeView && strPath.ToLower().Equals("viewnotice.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.AgendaView && strPath.ToLower().Equals("viewagenda.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.RevisedPdf && strPath.ToLower().Equals("agendaaccess.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }
                    if (!objRoll.DeviceManager && strPath.ToLower().Equals("userdevices.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.EmailHistory && strPath.ToLower().Equals("emailhistory.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.PublishHistory && strPath.ToLower().Equals("publishhistroy.aspx"))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.SectionMaster && (strPath.ToLower().Equals("sectionmaster.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.HeaderMaster && (strPath.ToLower().Equals("headermaster.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.AgendaMarker && (strPath.ToLower().Equals("agendamarker.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    if (!objRoll.MeetingRetention && (strPath.ToLower().Equals("meetingretention.aspx")))
                    {
                        Response.Redirect("Default.aspx");
                    }

                    //if (!objRoll.Transaction && (strPath.ToLower().Equals("createmeeting.aspx") || strPath.ToLower().Equals("NoticeMaster.aspx") || strPath.ToLower().Equals("createagenda.aspx") || strPath.ToLower().Equals("uploadminutes.aspx")))
                    //{
                    //    Response.Redirect("Default.aspx");
                    //}

                    //if (!objRoll.View && (strPath.ToLower().Equals("meetingschedule.aspx") || strPath.ToLower().Equals("viewnotice.aspx") || strPath.ToLower().Equals("viewagenda.aspx") || strPath.ToLower().Equals("viewminutes.aspx") || strPath.ToLower().Equals("viewattendance.aspx")))
                    //{
                    //    Response.Redirect("Default.aspx");
                    //}

                    //if (!objRoll.ApprovalMaster && strPath.ToLower().Equals("reports.aspx"))
                    //{
                    //    Response.Redirect("Default.aspx");
                    //}
                }
                else
                {
                    searchlink.Visible = false;
                    lnkReports.Visible = false;
                    lnkTransactions.Visible = false;
                    lnkView.Visible = false;

                    lnkApprovals.Visible = false;

                    trAccessRights.Visible = false;
                    // trApprovalMaster.Visible = objRoll.ApprovalMaster;
                    trUserMaster.Visible = false;
                    trEntityMaster.Visible = false;
                    trRollMaster.Visible = false;

                    if (!(strPath.ToLower().Equals("default.aspx") || strPath.ToLower().Equals("answerquestion.aspx") || strPath.ToLower().Equals("changepassword.aspx")))
                    {
                        Response.Redirect("default.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void lnkSwitch_Click(object sender, EventArgs e)
        {
            Response.Redirect("Redirect.aspx", false);
        }

        public bool AddLoginList(string userId)
        {
            System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
         as System.Collections.Generic.Dictionary<string, string>;
            if (d != null)
            {
                if (d.Keys.Contains(userId) && d.Values.Contains(Session.SessionID))
                {
                    // User is already logged in!!!
                    return true;
                }
            }

            return false;
        }
    }


    public static class AntiforgeryChecker
    {
        public static void Check(MasterPage page, HiddenField antiforgery)
        {
            string previousUrl = "";
            if (HttpContext.Current.Request.Url != null)
            {
                previousUrl = HttpContext.Current.Request.Url.ToString();
            }
            if (previousUrl.ToLower().IndexOf("noticemaster.aspx") == -1)
            {
                if (!page.IsPostBack)
                {
                    Guid antiforgeryToken = Guid.NewGuid();
                    page.Session["AntiforgeryToken"] = antiforgeryToken;
                    antiforgery.Value = antiforgeryToken.ToString();
                }
                else
                {
                    try
                    {
                        Guid stored = (Guid)page.Session["AntiforgeryToken"];
                        Guid sent = new Guid(antiforgery.Value);
                        if (sent != stored)
                        {
                            throw new System.Security.SecurityException("XSRF Attack Detected!");
                        }
                    }
                    catch { throw new System.Security.SecurityException("XSRF Attack Detected!"); }
                }
            }
        }
    }

}