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
using System.Text;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Cryptography;
using System.Web.SessionState;
using System.DirectoryServices;
using System.Net.Mail;
using System.Xml;

namespace MeetingMinder.Web
{
    public partial class iPadNepalUserVerifyOTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["MobileNo"] != null && Session["OTPId"] != null)
                {
                    string MobNo = Session["MobileNo"].ToString();
                    lblloginMsg.Text = "One time password (OTP) has been sent to your mobile ******" + MobNo.Substring(MobNo.Length - 4);
                }
                else if (Session["EmailID1"] != null && Session["OTPId"] != null)
                {
                    string email = Session["EmailID1"].ToString();
                    lblloginMsg.Text = "One time password (OTP) has been sent to your email " + email.Substring(0, 1) + "******" + email.Substring(email.Length - 11);
                }
                else
                {
                    Response.Redirect("iPadNepalUserLogin.aspx");
                }
            }
        }

        private void UpdateCaptchaText()
        {
            //txtCaptchaText.Text = string.Empty;
            //lblStatus.Visible = false;
            //Store the captcha text in session to validate
            Session["Captcha"] = Guid.NewGuid().ToString().Substring(0, 6);
        }

        protected void btnReGenerate_Click(object sender, EventArgs e)
        {
            UpdateCaptchaText();
        }


        protected void ValidateCaptcha2(object sender, ServerValidateEventArgs e)
        {
            CaptchaControl2.ValidateCaptcha(txtCaptcha2.Text.Trim());
            e.IsValid = CaptchaControl2.UserValidated;
            if (e.IsValid)
            {
                txtOtp.Text = "";
                txtCaptcha2.Text = "";
                hdnPassword.Value = "";
                lblmsg.Text = "Invalid Captcha.";
                //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Valid Captcha!');", true);
            }
        }

        protected void btnOTP_Click(object sender, EventArgs e)
        {
            try
            {
                //string OTP = Session["OTPPassword"].ToString();

                //CaptchaControl2.ValidateCaptcha(txtCaptcha2.Text.Trim());
                //if (!CaptchaControl2.UserValidated)
                //{

                //    lblmsg.Visible = true;
                //    txtOtp.Text = "";
                //    txtCaptcha2.Text = "";

                //    lblmsg.Text = "Invalid Captcha.";

                //    string sessioni = Guid.NewGuid().ToString();
                //    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                //    HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                //    return;
                //}

                //if (Session["OTPId"] != null)
                //{
                //    Response.Redirect("iPadUserLogin.aspx");
                //}

                Guid OTPId = Null.SetNullGuid(Session["OTPId"].ToString());
                Guid UserId = Null.SetNullGuid(Session["UserIdDownload"].ToString());
                string OTP = txtOtp.Text.Trim();
                int TimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WebOTPTimeOut"]);

                bool Verify = UserDataProvider.Instance.VerifyOTP(UserId, TimeOut, OTP, OTPId);

                if (Verify)
                {
                    txtOtp.Text = "";
                    txtCaptcha2.Text = "";                    

                    string RedirectUrl = string.Empty;
                    //if (Session["UserType"] != null)
                    //{
                    //    RedirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebIPAUrl"]);
                    //}
                    //else
                    //{
                    //    RedirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WebIPAUrl"]);
                    //}

                    RedirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebIPAUrl"]);
                    
                    Session.Abandon();
                    Session.Clear();

                    HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
                    myCookie.Value = Guid.NewGuid().ToString();
                    myCookie.Path = Request.ApplicationPath;
                    Response.Cookies.Add(myCookie);

                    Response.Redirect(RedirectUrl);

                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Invalid OTP, Please try again.";
                    txtCaptcha2.Text = "";

                    lblMessage.Text = "";
                    txtOtp.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Invalid OTP, Please try again.";

                LogError objEr = new LogError();
                objEr.HandleException(ex);

                //throw;
            }
        }

        protected void btnResendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                lblmsg.Visible = false;
                lblMessage.Text = "";
                txtOtp.Text = "";

                UserDomain list = null;
                list = UserDataProvider.Instance.ResendOTP(Guid.Parse(Session["UserIdDownload"].ToString()));

                if (list.IsEnabledOnIpad)
                {
                    Session["UserIdDownload"] = list.UserId;
                    Session["OTPId"] = list.OTPId;

                    string mob = list.Mobile.Substring(0, 2);
                    if (mob == "91")
                    {
                        Session["UserType"] = null;
                        Session["MobileNo"] = list.Mobile;
                        Session.Timeout = list.SessionTimeout;
                        //SendOTP(list.Mobile, list.OTP);

                        string url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostOTPUrl"]);
                        string username = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostUsername"]);
                        string password = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostPassword"]));

                        string contentType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostContentType"]);
                        string message = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostMessage"]);

                        string authInfo = username + ":" + password;
                        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                        message = message.Replace("{Date}", DateTime.Now.ToString("dd MMM hh:mm")).Replace("{OTP}", list.OTP).Replace("{TimeOut}", "2");

                        string data = string.Empty;
                        data = contentType + "&" + "mobile=" + list.Mobile + "&" + message;

                        string response = string.Empty;
                        response = UserDataProvider.PostSMS(url, data, authInfo);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(list.UserId), "Resend OTP", "", "", Encryptor.EncryptString(response));
                    }
                    else
                    {
                        Session["UserType"] = "International";
                        Session["MobileNo"] = list.Mobile;
                        Session.Timeout = list.SessionTimeout;
                        //SendInternationalOTP(list.Mobile, list.OTP);

                        string url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntOTPUrl"]);
                        string username = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntUsername"]);
                        string password = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntPassword"]));

                        string contentType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntContentType"]);
                        string message = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntMessage"]);

                        string authInfo = username + ":" + password;
                        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                        message = message.Replace("{Date}", DateTime.Now.ToString("dd MMM hh:mm")).Replace("{OTP}", list.OTP).Replace("{TimeOut}", "2");

                        string data = string.Empty;
                        data = contentType + "&" + "mobile=" + list.Mobile + "&" + message;

                        string response = string.Empty;
                        response = UserDataProvider.PostSMS(url, data, authInfo);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(list.UserId), "Resend Internation OTP", "", "", Encryptor.EncryptString(response));
                    }

                    //if (Session["EmailID1"] != null)
                    //{
                    //    Session["EmailID1"] = list.EmailID1;
                    //    Session.Timeout = list.SessionTimeout;
                    //    SendOTPMail(list.OTP, list.EmailID1);
                    //}
                    //else
                    //{
                    //    Session["MobileNo"] = list.Mobile;
                    //    Session.Timeout = list.SessionTimeout;
                    //    SendOTP(list.Mobile, list.OTP);
                    //}

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "OTP Resent Successfully.";
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Session Expired, Please log in again.";

                LogError objEr = new LogError();
                objEr.HandleException(ex);

                btnBackTologin.Visible = true;
                btnOTP.Visible = false;
                //throw;
            }
        }

        protected void SendOTP(string MobileNo, string OTPUser)
        {
            string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WebOTPSend"]);

            string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WebOTPUrl"]);
            string customerType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Websender"]);

            //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());


            if (OTPSend.ToLower().Equals("true"))
            {
                if (MobileNo == null || MobileNo.Length == 0)
                {
                    lblMessage.Text = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                    return;
                }

                if (!(MobileNo.Length > 9))
                {
                    lblMessage.Text = "Invalid Mobile Number, Please Contact Administrator";
                    return;
                }
                // objResponse.OTPSend = Convert.ToBoolean(OTPSend);

                try
                {

                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 |
                    (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    string dcode = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Webdcode"]);
                    string subuid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Websubuid"]);
                    int OTPTimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WebOTPTimeOut"]);

                    string pwd = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Webpwd"]));

                    string OTPKey = "";
                    string OTPVersion = "";
                    OTPUrl = OTPUrl.Replace("{dcode}", dcode).Replace("{subuid}", subuid).Replace("{pwd}", pwd).Replace("{sender}", customerType)
                           .Replace("{pno}", MobileNo).Replace("{TimeOut}", OTPTimeOut.ToString()).Replace("{Date}", DateTime.Now.ToString("dd MMM hh:mm")).Replace("{OTP}", OTPUser);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(OTPUrl);
                    webRequest.Method = "GET";

                    using (WebResponse response = webRequest.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            string soapResult = rd.ReadToEnd();
                            if (soapResult.Length > 8)
                            {
                                //objResponse.OTPKey = objUser.OTPId.ToString();
                            }
                            Response.Write(soapResult);

                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "OTP Error";//e.Message;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
        }

        protected void btnBackTologin_Click(object sender, EventArgs e)
        {
            Response.Redirect("iPadNepalUserLogin.aspx");
        }

        public bool SendOTPMail(string OTP, string ToEmail)
        {
            try
            {
                //UserDomain objUser = UserDataProvider.Instance.Get(userid);
                //if (objUser != null)
                //{
                //Email user name and password

                //string ToEmail = objUser.EmailID1;
                //string uname = objUser.UserName;
                //string passwd = objUser.Password;
                //if (ToEmail.Length == 0)
                //{
                //    Message = "Email address of maker not exists. Email sending failed. ";
                //    IsSuccess = false;
                //    return;
                //}


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
                //if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(".") + "/img/Uploads/Email.xml"))
                //{
                //    Message = "Email sending failed. Configuration setting missing";
                //    IsSuccess = false;
                //    return;
                //}
                xml.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/img/Uploads/Email.xml");
                XmlNodeList serverlist = xml.SelectNodes("//email");
                foreach (XmlNode servernodes in serverlist)
                {
                    EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                    Password = servernodes.SelectSingleNode("password").InnerText;
                    SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                    SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                    SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                }
                string EmailSubject = "One Time Password(OTP) for iMeeting application";
                int TimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WebOTPTimeOut"]);

                //string strlink = @"http:///Login.aspx";
                string EmailBody = @"<p><h4>Dear User,</h4> <br>Your One-Time Password (OTP) is " + OTP + @".<br> This OTP used for login to your iMeeting application. Valid for " + TimeOut + @" mins.</p><br><p>Please do not share with anyone.</p>
<p>
	Regards<br />
	iMeeting Admin Team</p>";


                EmailBody += "</p>";



                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                SMTPServer.UseDefaultCredentials = false;
                SMTPServer.Credentials = SMTPUserInfo;

                //SMTPServer.EnableSsl = true;

                MailAddress fromAddress = new MailAddress(EmailFrom, "iMeeting");
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
                //IsSuccess = true;
                //Message = "Email sent to checker.";
                //}
                //else
                //{
                //    //    IsSuccess = false;
                //    //  Message = "Email sending failed.";
                //}

                return true;
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;
                //IsSuccess = false;
                // Message = "Email sending failed. Try again after some time";
                LogError objEr = new LogError();
                objEr.HandleException(ex);

                return false;
            }
        }

        protected void SendInternationalOTP(string MobileNo, string OTPUser)
        {
            string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPSend"]);

            string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPUrl"]);
            string customerType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebsender"]);

            //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());


            if (OTPSend.ToLower().Equals("true"))
            {
                if (MobileNo == null || MobileNo.Length == 0)
                {
                    lblMessage.Text = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                    return;
                }

                if (!(MobileNo.Length > 9))
                {
                    lblMessage.Text = "Invalid Mobile Number, Please Contact Administrator";
                    return;
                }
                // objResponse.OTPSend = Convert.ToBoolean(OTPSend);

                try
                {

                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 |
                    (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    string dcode = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebdcode"]);
                    string subuid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebsubuid"]);
                    int OTPTimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPTimeOut"]);

                    string pwd = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebpwd"]));

                    OTPUrl = OTPUrl.Replace("{dcode}", dcode).Replace("{subuid}", subuid).Replace("{pwd}", pwd).Replace("{sender}", customerType)
                           .Replace("{pno}", MobileNo).Replace("{TimeOut}", OTPTimeOut.ToString()).Replace("{Date}", DateTime.Now.ToString("dd MMM hh:mm")).Replace("{OTP}", OTPUser);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(OTPUrl);
                    webRequest.Method = "GET";

                    using (WebResponse response = webRequest.GetResponse())
                    {
                        using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                        {
                            string soapResult = rd.ReadToEnd();
                            if (soapResult.Length > 8)
                            {
                                //objResponse.OTPKey = objUser.OTPId.ToString();
                            }
                            Response.Write(soapResult);

                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    lblMessage.Text = "OTP Error";//e.Message;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }

    }
}