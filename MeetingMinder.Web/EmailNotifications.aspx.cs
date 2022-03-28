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
using System.Net.Mail;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace MeetingMinder.Web
{
    public partial class EmailNotifications : System.Web.UI.Page
    {
        public string strName = "";
        public string strPhone = "";
        public string strEntity = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            lblAlert.Text = "";
            if (Request.UrlReferrer != null)
            {
                string prvPage = Request.UrlReferrer.ToString();

                if (Session["EAMessage"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["EAMessage"].ToString();
                    Info.Visible = true;
                    Session["EAMessage"] = null;
                }
            }


            Page.MaintainScrollPositionOnPostBack = true;
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
                    strEntity = Session["EntityName"].ToString();
                    //rcap

                    //rlife
                    //if (EntityId.ToString().Equals("ac0b10c7-46d9-45e0-b6e7-113aaaa504d9"))
                    //{
                    //    strName = " Puja Mehta ";
                    //    strPhone = " 9320032956 ";
                    //}

                    //rgeneral

                    strName = " Secretarial Team ";
                    strPhone = " Email or Phone";


                    BindForum(EntityId.ToString());

                    ddlEntity.SelectedValue = EntityId.ToString();
                    ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }

                #endregion

                // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                //SendEmaisl(Guid.Parse("b9cf1f64-e0d3-478a-83eb-03119901fa4a"));
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    strEntity = Session["EntityName"].ToString();
                    //rcap
                    //if (EntityId.ToString().Equals("50c3252d-e067-448e-86e7-ed5fb4a43d8f"))
                    //{
                    //    strName = "Mr. Gajendra Thakur ";
                    //    strPhone = " 9321363257 ";
                    //}
                    //else
                    //    //rlife
                    //    //if (EntityId.ToString().Equals("ac0b10c7-46d9-45e0-b6e7-113aaaa504d9"))
                    //    //{
                    //    //    strName = " Puja Mehta ";
                    //    //    strPhone = " 9320032956 ";
                    //    //}

                    //    //rgeneral
                    //    if (EntityId.ToString().Equals("50a862b1-41b3-4751-b91d-ce4653111d91"))
                    //    {
                    //        strName = "Mr. Mohan Khandekar ";
                    //        strPhone = " 9324032570 ";
                    //    }
                    //    else
                    //    {
                    strName = " Secretarial Team ";
                    strPhone = " Email or Phone";

                    // }
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
                    IList<UserDomain> objUserDom = UserDataProvider.Instance.GetAllUserForEmailNotification(forumId);
                    grdReport.DataSource = objUserDom;
                    grdReport.DataBind();


                    IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();

                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    DateTime dtToday = DateTime.Now;

                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate + " " + item.MeetingTime);
                        //if (dtMeeting >= dtToday)
                        //{
                        //ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, dtMeeting.ToString("D") + '`' + item.MeetingVenue + '`' + item.MeetingTime));
                        ddlMeeting.Items.Add(new ListItem(WebUtility.HtmlDecode(item.MeetingNumber) + ' ' + WebUtility.HtmlDecode(item.ForumName) + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), dtMeeting.ToString("D") + '`' + WebUtility.HtmlDecode(item.MeetingVenue) + '`' + item.MeetingTime + '`' + item.MeetingId));

                        //}
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
        /// Button send click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                UserAcess objUser = new UserAcess();
                if (txtcc.Text != "")
                {
                    if (!objUser.isValidChar(txtcc.Text))
                    {
                        txtcc.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtcc.Text))
                    {
                        txtcc.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtcc.Text))
                    {
                        txtcc.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                //if (txtEmailBo.Text != "")
                //{
                //    if (!objUser.isValidChar(txtEmailBo.Text))
                //    {
                //        txtEmailBo.Text = "";
                //        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                //        Error.Visible = true;
                //        return;
                //    }
                //}
                //else
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Please enter email body";
                //    Error.Visible = true;
                //    return;
                //}

                if (txtSubject.Text != "")
                {
                    if (!objUser.isValidChar(txtSubject.Text))
                    {
                        txtSubject.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtSubject.Text))
                    {
                        txtSubject.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid email subject.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSubject.Text))
                    {
                        txtSubject.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter email subject";
                    Error.Visible = true;
                    return;
                }


                if (!File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") + "/img/Uploads/Email.xml"))
                {
                    ((Label)Error.FindControl("lblError")).Text = "Email configuration setting missing";
                    Error.Visible = true;
                    return;
                }
                //  IList<string> receiverList = new List<string>();
                Dictionary<string, string> receiverList = new Dictionary<string, string>();
                //get all checked entity
                for (int i = 0; i < grdReport.Rows.Count; i++)
                {
                    CheckBox chkEmail1 = (CheckBox)grdReport.Rows[i].FindControl("chkEmailId");
                    if (chkEmail1.Checked)
                    {
                        count++;
                        Label Email1 = (Label)grdReport.Rows[i].FindControl("lblUserEmail");
                        Label Name = (Label)grdReport.Rows[i].FindControl("lblName");
                        if (!receiverList.ContainsKey(Email1.Text))
                        {
                            receiverList.Add(Email1.Text, Name.Text);
                        }
                    }

                    CheckBox chkEmail2 = (CheckBox)grdReport.Rows[i].FindControl("chkEmailId2");
                    if (chkEmail2.Checked)
                    {
                        count++;
                        Label Email2 = (Label)grdReport.Rows[i].FindControl("lblUserEmail1");
                        Label Names = (Label)grdReport.Rows[i].FindControl("lblName");
                        if (!receiverList.ContainsKey(Email2.Text))
                        {
                            receiverList.Add(Email2.Text, Names.Text);
                        }
                    }

                    CheckBox chkSecEmail1 = (CheckBox)grdReport.Rows[i].FindControl("chkSecEmailId");
                    if (chkSecEmail1.Checked)
                    {
                        count++;
                        Label SecEmail1 = (Label)grdReport.Rows[i].FindControl("lblSecEmailId1");
                        Label SecNames = (Label)grdReport.Rows[i].FindControl("lblSecName");
                        if (!receiverList.ContainsKey(SecEmail1.Text))
                        {
                            receiverList.Add(SecEmail1.Text, SecNames.Text);
                        }
                    }

                    CheckBox chkSecEmail2 = (CheckBox)grdReport.Rows[i].FindControl("chkSecEmailId2");
                    if (chkSecEmail1.Checked)
                    {
                        count++;
                        Label SecEmail2 = (Label)grdReport.Rows[i].FindControl("lblSecEmailId2");
                        Label SecNamess = (Label)grdReport.Rows[i].FindControl("lblSecName");
                        if (!receiverList.ContainsKey(SecEmail2.Text))
                        {
                            receiverList.Add(SecEmail2.Text, SecNamess.Text);
                        }
                    }
                }

                //get created user details
                string fullname = string.Empty;
                string contact = string.Empty;
                UserDomain objCreatedUser = UserDataProvider.Instance.Get(Guid.Parse(Convert.ToString(HttpContext.Current.Session["UserId"])));
                fullname = objCreatedUser.Suffix + " " + objCreatedUser.FirstName + " " + objCreatedUser.LastName;
                contact = objCreatedUser.Mobile;

                string department = string.Empty;
                EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(Convert.ToString(HttpContext.Current.Session["EntityId"])));
                if (objEntity != null)
                {
                    department = objEntity.EntityName;
                }


                System.Text.StringBuilder receriverHistory = new System.Text.StringBuilder("");
                foreach (KeyValuePair<string, string> strEmail in receiverList)
                {
                    SmtpClient SMTPServer = new SmtpClient();
                    string Password = "";
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");

                    string EmailFrom = ConfigurationManager.AppSettings["Email"];
                    string ToEmail = strEmail.Key;//objUser.EmailID1;
                    XmlNodeList serverlist = xml.SelectNodes("//email");
                    foreach (XmlNode servernodes in serverlist)
                    {
                        EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                        Password = Encryptor.DecryptString(servernodes.SelectSingleNode("password").InnerText);
                        //Password = servernodes.SelectSingleNode("password").InnerText;
                        SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                        SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                        SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                    }
                    if (ToEmail != "" && ToEmail != null)
                    {
                        if (receriverHistory.Length == 0)
                        {
                            receriverHistory.Append(strEmail.Value + "&lt;" + strEmail.Key + "&gt;");
                        }
                        else
                        {
                            receriverHistory.Append("," + strEmail.Value + "&lt;" + strEmail.Key + "&gt;");
                        }

                        MailMessage objmail = new MailMessage();
                        MailAddress fromAddress = new MailAddress(EmailFrom, "iMeetings App Admin");
                        objmail.From = fromAddress;

                        objmail.To.Add(ToEmail.Trim());
                        string body = txtEmailBo.Text;




                        body = body.Replace("{Name}", strEmail.Value).Replace("[FullName]", fullname).Replace("[ContactNo]", contact).Replace("[DepartmentName]", department);
                        objmail.Body = body;
                        objmail.Subject = txtSubject.Text;
                        if (txtcc.Text.Length > 0)
                        {
                            objmail.CC.Add(txtcc.Text);
                        }
                        objmail.IsBodyHtml = true;

                        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                        SMTPServer.UseDefaultCredentials = false;
                        SMTPServer.Credentials = SMTPUserInfo;
                        //SMTPServer.EnableSsl = true;


                        objmail.Headers.Add("NAME", "Admin");


                        objmail.IsBodyHtml = true;

                        //                    ServicePointManager.ServerCertificateValidationCallback =
                        //delegate(object s, X509Certificate certificate,
                        //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        //{ return true; };

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                        SMTPServer.Send(objmail);
                        objmail.To.Clear();

                        //SendEmail(objmail);
                    }
                    Guid MeetingId = Guid.Empty;
                    string MeetingVal = ddlMeeting.SelectedValue;
                    if (MeetingVal != "0")
                    {
                        string[] meetingParams = MeetingVal.Split('`');
                        if (meetingParams.Count() >= 3)
                        {
                            Guid.TryParse(meetingParams[3], out MeetingId);
                        }
                    }

                    EmailHistroyDomain objEmailHistroy = new EmailHistroyDomain();
                    objEmailHistroy.MeetingId = MeetingId;//Guid.Parse(ddlMeeting.SelectedValue);
                    objEmailHistroy.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                    objEmailHistroy.Body = txtEmailBo.Text;
                    objEmailHistroy.CC = txtcc.Text;
                    objEmailHistroy.Receiver = receriverHistory.ToString();
                    objEmailHistroy.Sender = EmailFrom;
                    objEmailHistroy.Subject = txtSubject.Text;
                    objEmailHistroy = MeetingDataProvider.Instance.InsertEmailHistory(objEmailHistroy);

                }
                ((Label)Info.FindControl("lblName")).Text = "Email sent successfully";

                //web logs
                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Email Notification", "Success", "", "Email sent successfully");

                Response.StatusCode = 302;
                Response.AddHeader("Location", "EmailNotifications.aspx");
                Session["EAMessage"] = ((Label)Info.FindControl("lblName")).Text;

                Info.Visible = true;
                //   ddlEntity.SelectedIndex = -1;
                ddlForum.SelectedIndex = -1;
                ddlTemplates.SelectedIndex = -1;
                txtcc.Text = "";
                txtEmailBo.Text = "";
                txtSubject.Text = "";
                grdReport.DataSource = null;
                grdReport.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public static void SendEmail(System.Net.Mail.MailMessage m)
        {
            SendEmail(m, true);
        }



        public static void SendEmail(System.Net.Mail.MailMessage m, Boolean Async)
        {
            System.Net.Mail.SmtpClient smtpClient = null;
            smtpClient = new SmtpClient();

            // SmtpClient smtpClient = new SmtpClient();        

            //string EmailFrom = ConfigurationManager.AppSettings["Email"];
            //string Password = ConfigurationManager.AppSettings["EPassword"];


            XmlDocument xml = new XmlDocument();

            xml.Load(System.Web.Hosting.HostingEnvironment.MapPath("~") + "/img/Uploads/Email.xml");

            string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
            string Password = "";// = ConfigurationManager.AppSettings["EPassword"];

            string port = "0"; //= ConfigurationManager.AppSettings["port"];
            string strSmtpClient = ""; // = ConfigurationManager.AppSettings["SmtpClient"];

            //smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

            XmlNodeList serverlist = xml.SelectNodes("//email");
            foreach (XmlNode servernodes in serverlist)
            {
                EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                Password = servernodes.SelectSingleNode("password").InnerText;
                port = servernodes.SelectSingleNode("port").InnerText;
                strSmtpClient = servernodes.SelectSingleNode("smtpclient").InnerText;
                smtpClient.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
            }

            //   smtpClient = new SmtpClient();
            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = SMTPUserInfo;
            smtpClient.EnableSsl = true;

            smtpClient.Host = strSmtpClient;
            smtpClient.Port = Convert.ToInt16(port);
            smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            if (Async)
            {
                SendEmailDelegate sd = new SendEmailDelegate(smtpClient.Send);
                AsyncCallback cb = new AsyncCallback(SendEmailResponse);
                sd.BeginInvoke(m, cb, sd);
            }
            else
            {
                smtpClient.Send(m);
            }
        }

        private delegate void SendEmailDelegate(System.Net.Mail.MailMessage m);
        private static void SendEmailResponse(IAsyncResult ar)
        {
            SendEmailDelegate sd = (SendEmailDelegate)(ar.AsyncState);

            sd.EndInvoke(ar);
        }


        /// <summary>
        /// Eamil Config Save 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEmailSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (txtPassword.Text != "")
                {
                    if (!objUser.isValidChar(txtPassword.Text))
                    {
                        txtPassword.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtPort.Text != "")
                {
                    if (!objUser.isValidChar(txtPort.Text))
                    {
                        txtPort.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter port";
                    Error.Visible = true;
                    return;
                }

                if (txtSenderEmail.Text != "")
                {
                    if (!objUser.isValidChar(txtSenderEmail.Text))
                    {
                        txtSenderEmail.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter sender email";
                    Error.Visible = true;
                    return;
                }

                if (txtSmtpClient.Text != "")
                {
                    if (!objUser.isValidChar(txtSmtpClient.Text))
                    {
                        txtSmtpClient.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter smtp client";
                    Error.Visible = true;
                    return;
                }

                XmlDocument xml = new XmlDocument();
                if (System.IO.File.Exists(Server.MapPath(".") + "/img/Uploads/Email.xml"))
                {
                    xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");

                    XmlNodeList serverlist = xml.SelectNodes("//email");
                    foreach (XmlNode servernodes in serverlist)
                    {
                        servernodes.SelectSingleNode("senderemail").InnerText = txtSenderEmail.Text.Trim();
                        if (txtPassword.Text == " ***** ")
                        {
                            servernodes.SelectSingleNode("password").InnerText = Convert.ToString(ViewState["EmailPassword"]);
                        }
                        else
                        {
                            servernodes.SelectSingleNode("password").InnerText = txtPassword.Text;
                        }
                        //servernodes.SelectSingleNode("password").InnerText = //txtPassword.Text.Trim();
                        servernodes.SelectSingleNode("port").InnerText = Convert.ToInt64(txtPort.Text.Trim()).ToString();
                        servernodes.SelectSingleNode("smtpclient").InnerText = txtSmtpClient.Text.Trim();
                        servernodes.SelectSingleNode("ssl").InnerText = chkSSL.Checked.ToString();
                    }
                    xml.Save(Server.MapPath(".") + "/img/Uploads/Email.xml");
                    GetConfig();
                    lblAlert.Text = " <script> alert('Configuration saved successfully'); ShowPopUp(); </script>";

                }
                else
                {
                    string XmlFile = @"<?xml version='1.0'?>" +
"<email><senderemail>" + txtSenderEmail.Text.Trim() +
  "</senderemail>" +
  "<password>" + txtPassword.Text.Trim() +
      "</password> <port>" + Convert.ToInt64(txtPort.Text.Trim()).ToString() +
 "</port> <smtpclient>" + txtSmtpClient.Text.Trim() +
 "</smtpclient> <ssl>" + chkSSL.Checked.ToString() +
     "</ssl></email>";

                    FileStream fs = new FileStream(Server.MapPath(".") + "/img/Uploads/Email.xml", FileMode.Append, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.Write(XmlFile);
                    writer.Close();
                    fs.Close();
                    GetConfig();
                    lblAlert.Text = " <script> alert('Configuration saved successfully'); ShowPopUp(); </script>";
                }

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                lblAlert.Text = " <script> alert('Configuration saving failed, Try again after some time ); ShowPopUp(); </script>";
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public void GetConfig()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                if (System.IO.File.Exists(Server.MapPath(".") + "/img/Uploads/Email.xml"))
                {
                    xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");

                    XmlNodeList serverlist = xml.SelectNodes("//email");
                    foreach (XmlNode servernodes in serverlist)
                    {
                        txtSenderEmail.Text = servernodes.SelectSingleNode("senderemail").InnerText;
                        //  txtPassword.Text = servernodes.SelectSingleNode("password").InnerText;

                        ViewState["EmailPassword"] = servernodes.SelectSingleNode("password").InnerText;

                        if (servernodes.SelectSingleNode("password").InnerText != "")
                        {
                            txtPassword.Attributes.Add("value", " ***** ");
                        }
                        else
                        {
                            txtPassword.Attributes.Add("value", "");
                        }
                        //txtPassword.Attributes.Add("value", servernodes.SelectSingleNode("password").InnerText);
                        txtPort.Text = servernodes.SelectSingleNode("port").InnerText;
                        txtSmtpClient.Text = servernodes.SelectSingleNode("smtpclient").InnerText;
                        chkSSL.Checked = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                    }

                    //  xml.Save(Server.MapPath(".") + "/Email.xml");              
                }
                else
                {
                    txtSenderEmail.Text = "";
                    txtPassword.Text = "";
                    txtPort.Text = "";
                    txtSmtpClient.Text = "";
                    chkSSL.Checked = false;
                }
                hdnPopUp.Value = "1";
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
        /// link button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lnkEmailConfig_Click(object sender, EventArgs e)
        //{
        //    GetConfig();
        //}

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }
    }
}