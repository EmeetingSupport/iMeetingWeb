using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Domain;
using MM.Data;
using System.IO;
using System.Data;
using MM.Core;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Xml;
using System.Net;

namespace MeetingMinder.Web
{
    public partial class Forgot_Password : System.Web.UI.Page
    {
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblmsg.Visible = false;
            lblmsg.Text = "";
            string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);
            if (AuthenticationType.ToLower().Equals("ad"))
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                divQuestion.Visible = false;
                //try
                //{
                //    //divQuestion.Visible = false;  
                //    IList<SecurityQuestionDomain> objSecurity = SecurityQuestionDataProvider.Instance.Get();
                //    ddlSecurityQuestion.DataSource = objSecurity;
                //    ddlSecurityQuestion.DataBind();
                //    ddlSecurityQuestion.DataTextField = "SecurityQuestion";
                //    ddlSecurityQuestion.DataValueField = "SecurityQuestionId";
                //    ddlSecurityQuestion.DataBind();
                //    ddlSecurityQuestion.Items.Insert(0, new ListItem("Select Question", "0"));

                //}
                //catch (Exception ex)
                //{

                //}

            }
        }

        /// <summary>
        /// Button submit click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSecurityAnswer != null && txtSecurityAnswer.Text != "")
                {
                    CaptchaSecurityAnswer.ValidateCaptcha(txtSecurityAnswerCaptcha.Text.Trim());
                    if (!CaptchaSecurityAnswer.UserValidated)
                    {

                        lblmsg.Visible = true;
                        txtSecurityAnswer.Text = "";
                        txtSecurityAnswerCaptcha.Text = "";

                        lblmsg.Text = "Invalid Captcha.";
                        return;
                    }
                    txtCaptchaText.Text = "";
                    //  string QuestionId = ddlSecurityQuestion.SelectedValue;
                    string Answer = txtSecurityAnswer.Text;
                    //DataTable objUser = (DataTable)Session["objUser"];// = UserDataProvider.Instance.GetPassword(txtUserName.Text, Guid.Parse(QuestionId), Answer);
                    DataTable objUser = UserDataProvider.Instance.GetPasswordByUsername(txtUserName.Text);
                    string dbAns = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["Answer"]));

                    string Name = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["FirstName"]));
                    if (dbAns.ToLower().Equals(Answer.ToLower()))
                    {

                        //Email user name and password

                        string ToEmail = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["EmailID1"]));

                        if (string.IsNullOrEmpty(ToEmail))
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Your email address does not exist, Please contact administrator.";
                            return;
                        }

                        if (!System.IO.File.Exists(Server.MapPath(".") + "/img/Uploads/Email.xml"))
                        {
                            lblmsg.Text = "Email sending failed. Configuration setting missing";
                            lblmsg.Visible = true;
                            return;
                        }
                        //code email link:
                        Guid g = Guid.NewGuid();
                        DateTime d = DateTime.Now;
                        UserDomain objUserData = new UserDomain();
                        objUserData.UserId = Guid.Parse(objUser.Rows[0]["UserId"].ToString());
                        objUserData.EmailID1 = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["EmailID1"]));
                        objUserData.ActivationCode = Guid.NewGuid();
                        UserDataProvider.Instance.InsertPasswordResetLink(objUserData);

                        // string url = "http://localhost:55698/ResetPassword.aspx?ActivationToken=" + objUserData.ActivationCode.ToString();
                        string url = Convert.ToString(ConfigurationManager.AppSettings["Current_Url"]) + "/ResetPassword.aspx?ActivationToken=" + objUserData.ActivationCode.ToString();
                        MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                        SmtpClient SMTPServer = new SmtpClient();

                        string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                        string Password = "";// = ConfigurationManager.AppSettings["EPassword"]
                        XmlDocument xml = new XmlDocument();
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

                        string EmailSubject = "Reset Your Password";

                        //string strlink = @"http:///Login.aspx";
                        //string EmailBody = "<p>";
                        //EmailBody += "<table style=\"border-style:none;\"><tr style=\" border-style:none;\"><td>";

                        ////<td><img src=" + ("http:// /images/logo.png") + "></img></td>
                        //EmailBody += "<font color=\"Teal\"></b> EMEETING PASSWORD RECOVERY</b></font></td></tr></table><BR />";
                        //EmailBody += "Dear User <BR /><BR />";
                        //EmailBody += "Following are your Login Details :-<BR />";

                        //EmailBody += "<Table width=\"700px\" style=\"border-color:Teal; height:100px; width:50%; border-style:double;\">";
                        //EmailBody += "<tr style=\" border-style:none;\"><td>";

                        //EmailBody += "Login Email</td><td> :</td><td>" + txtUserName.Text + "</td></tr>";
                        //EmailBody += "<tr style=\" border-style:none;\"><td>Password</td><td> :</td><td>" + Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["Password"]))
                        //+ "</td></tr>";
                        //EmailBody += "</table><br /><br /><br />";
                        //EmailBody += "&nbsp; Regards <br /> eMeeting Admin Team";
                        //EmailBody += "</p>";
                        //EmailBody += "<p> To Change your Password Please  <a href='"+url.Trim()+"'>click here</a></p>";


                        string EmailBody = @" <div style='margin:20px;'>
      Hi " + Name + @", 
        <div  style='margin:20px;'>We got a request to reset your iMeeting password.</div>
        <div  style='margin:20px 20px 20px 160px;'><a href='" + url + @"' style=' -webkit-border-radius: 4px;
    -moz-border-radius: 4px;
    border-radius: 4px;
    border: solid 1px #20538D;
    text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.4);
    -webkit-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    -moz-box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.4), 0 1px 1px rgba(0, 0, 0, 0.2);
    background: #4479BA;
    color: #FFF;
    padding: 8px 12px;
    text-decoration: none;'>Reset Password</a></div>
        <div  style='margin:20px;'>
            If you ignore this message, your password won't be changed.
        </div>
        <div style='margin:20px;'>
            If you didn't request a password reset, contact the Secretarial Team on Email or Phone. 
        </div>
    </div>&nbsp; Regards <br /> iMeeting Admin Team";
                        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                        SMTPServer.UseDefaultCredentials = false;
                        SMTPServer.Credentials = SMTPUserInfo;
                        MailAddress fromAddress = new MailAddress(EmailFrom, "iMeeting Admin");
                        objmail.Headers.Add("NAME", "Admin");
                        objmail.From = fromAddress;
                        objmail.To.Add(ToEmail);
                        objmail.Body = EmailBody;
                        objmail.Subject = EmailSubject;

                        objmail.IsBodyHtml = true;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;


                        SMTPServer.Send(objmail);
                        objmail.To.Clear();
                        lblmsg.Visible = true;
                        Session["objUser"] = null;
                        lblmsg.Text = " Password Reset Email Has Been Sent To Your Account";
                        ltlMessage.Text = "<script> if(confirm(' Password Reset Email Has Been Sent To Your Account ')){location = 'Login.aspx';} else   { location = 'Login.aspx';  } </script>";
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Wrong question or answer! Can not process your request.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Error occured try again after some time.";

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }
        protected void ValidateCaptcha(object sender, ServerValidateEventArgs e)
        {
            CaptchaForgotPassword.ValidateCaptcha(txtCaptchaText.Text.Trim());
            e.IsValid = CaptchaForgotPassword.UserValidated;
            if (e.IsValid)
            {
                txtUserName.Text = "";
                txtCaptchaText.Text = "";
                lblmsg.Text = "Invalid Captcha.";
            }
        }
        /// <summary>
        /// Button next click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName != null && txtUserName.Text != "")
                {
                    CaptchaForgotPassword.ValidateCaptcha(txtCaptchaText.Text.Trim());
                    if (!CaptchaForgotPassword.UserValidated)
                    {

                        lblmsg.Visible = true;
                        txtUserName.Text = "";
                        txtCaptchaText.Text = "";

                        lblmsg.Text = "Invalid Captcha.";
                        return;
                    }
                    txtCaptchaText.Text = "";
                    DataTable objUser = UserDataProvider.Instance.GetPasswordByUsername(txtUserName.Text);
                    if (objUser.Rows.Count > 0)
                    {
                        Guid securityId;
                        if (Guid.TryParse(Convert.ToString(objUser.Rows[0]["SecurityQuestionId"]), out securityId))
                        {
                            IList<SecurityQuestionDomain> objSecurity = SecurityQuestionDataProvider.Instance.Get();
                            var strQuest = (from p in objSecurity
                                            where p.SecurityQuestionId == securityId
                                            select p.SecurityQuestion).ToList();
                            if (strQuest.Count() > 0)
                            {
                                lblQuestion.Text = strQuest[0];

                                Session["objUser"] = objUser;

                                divName.Visible = false;
                                divQuestion.Visible = true;
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblQuestion.Text = "";
                                lblmsg.Text = "Wrong user name.";
                            }
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblQuestion.Text = "";
                            lblmsg.Text = " Security question not found";
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblQuestion.Text = "";
                        lblmsg.Text = "Wrong user name.";
                    }
                }
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnSubmit1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName != null && txtUserName.Text != "")
                {
                    CaptchaForgotPassword.ValidateCaptcha(txtCaptchaText.Text.Trim());
                    if (!CaptchaForgotPassword.UserValidated)
                    {

                        lblmsg.Visible = true;
                        txtUserName.Text = "";
                        txtCaptchaText.Text = "";

                        lblmsg.Text = "Invalid Captcha.";
                        return;
                    }
                    txtCaptchaText.Text = "";
                    DataTable objUser = UserDataProvider.Instance.GetPasswordByUsername(txtUserName.Text);
                    if (objUser.Rows.Count > 0)
                    {
                        UserDomain objUserDomain = new UserDomain();
                        objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["Moblie"]));
                        objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["FirstName"]));
                        objUserDomain.LastName = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["LastName"]));
                        objUserDomain.Password = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["Password"]));
                        objUserDomain.EmailID1 = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["EmailID1"]));
                        objUserDomain.Suffix = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["Suffix"]));
                        objUserDomain.UserName = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["UserName"]));

                        if (objUserDomain.Mobile == null || objUserDomain.Mobile.Length == 0)
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                            return;
                        }

                        if (!(objUserDomain.Mobile.Length > 9))
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Invalid Mobile Number, Please Contact Administrator";
                            return;
                        }

                        bool status;
                        string mob = objUserDomain.Mobile.Substring(0, 2);
                        if (mob == "91")
                        {
                            string url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostOTPUrl"]);
                            string username = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostUsername"]);
                            string password = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostPassword"]));

                            string contentType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostContentType"]);
                            string message = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostForgotPass"]);

                            string authInfo = username + ":" + password;
                            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                            message = message.Replace("{FirstName}", objUserDomain.FirstName).Replace("{Password}", objUserDomain.Password);

                            string data = string.Empty;
                            data = contentType + "&" + "mobile=" + objUserDomain.Mobile + "&" + message;

                            //string response = string.Empty;
                            //response = UserDataProvider.PostSMS(url, data, authInfo);

                            bool response = PostSMS(url, data, authInfo);

                            response = SendEmail(objUserDomain.Suffix + " " + objUserDomain.FirstName + " " + objUserDomain.LastName, objUserDomain.UserName, objUserDomain.Password, objUserDomain.EmailID1);

                            if (response)
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Password has been sent to your registered mobile number and email address";
                                //lblmsg.Text = "Your password has been sent to mobile number ending with ******" + objUserDomain.Mobile.Substring(objUserDomain.Mobile.Length - 4);

                                var encrypt = Encryptor.EncryptString(data);
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(txtUserName.Text, "Forgot Password", "Success", "", "Password sent :- " + encrypt + "");

                                return;
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Something went wrong. please try again after sometime.";

                                return;
                            }

                            status = true;
                            //status = SendOTP(objUserDomain.FirstName, objUserDomain.Mobile, objUserDomain.Password);
                        }
                        else
                        {

                            string url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntOTPUrl"]);
                            string username = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntUsername"]);
                            string password = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntPassword"]));

                            string contentType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntContentType"]);
                            string message = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PostIntForgotPass"]);

                            string authInfo = username + ":" + password;
                            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                            message = message.Replace("{FirstName}", objUserDomain.FirstName).Replace("{Password}", objUserDomain.Password);

                            string data = string.Empty;
                            data = contentType + "&" + "mobile=" + objUserDomain.Mobile + "&" + message;

                            //string response = string.Empty;
                            //response = UserDataProvider.PostSMS(url, data, authInfo);

                            bool response = PostSMS(url, data, authInfo);

                            response = SendEmail(objUserDomain.Suffix + " " + objUserDomain.FirstName + " " + objUserDomain.LastName, objUserDomain.UserName, objUserDomain.Password, objUserDomain.EmailID1);

                            if (response)
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Password has been sent to your registered mobile number and email address";

                                //lblmsg.Text = "Your password has been sent to mobile number ending with ******" + objUserDomain.Mobile.Substring(objUserDomain.Mobile.Length - 4);

                                var encrypt = Encryptor.EncryptString(data);
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(txtUserName.Text, "Forgot Password", "Success", "", "Password sent :- " + encrypt + "");

                                return;
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Something went wrong. please try again after sometime.";

                                return;
                            }

                            status = true;
                            //status = SendInternationalOTP(objUserDomain.FirstName, objUserDomain.Mobile, objUserDomain.Password);

                        }

                        if (status)
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Your password has been sent to mobile number ending with ******" + objUserDomain.Mobile.Substring(objUserDomain.Mobile.Length - 4);
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "Something went wrong. please try again after sometime.";
                        }

                        txtUserName.Text = "";
                        txtCaptchaText.Text = "";
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblQuestion.Text = "";
                        lblmsg.Text = "Invalid username.";
                        txtUserName.Text = "";
                        txtCaptchaText.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Something went wrong. please try again after sometime.";

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        bool SendOTP(string FirstName, string MobileNo, string Password)
        {
            string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WebOTPSend"]);

            string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NationalForgotPass"]);
            string customerType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Websender"]);
            bool result = false;

            if (OTPSend.ToLower().Equals("true"))
            {
                try
                {

                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 |
                    (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    string dcode = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Webdcode"]);
                    string subuid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Websubuid"]);

                    string pwd = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Webpwd"]));

                    OTPUrl = OTPUrl.Replace("{dcode}", dcode).Replace("{subuid}", subuid).Replace("{pwd}", pwd).Replace("{sender}", customerType)
                           .Replace("{pno}", MobileNo).Replace("{FirstName}", FirstName).Replace("{Password}", Password);

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
                            result = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    lblmsg.Text = "OTP Error";//e.Message;
                    result = false;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
            return result;
        }

        bool SendInternationalOTP(string FirstName, string MobileNo, string Password)
        {
            string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPSend"]);

            string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["InternationalForgotPass"]);
            string customerType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebsender"]);
            bool result = false;

            if (OTPSend.ToLower().Equals("true"))
            {
                try
                {
                    ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 |
                    (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    string dcode = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebdcode"]);
                    string subuid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebsubuid"]);

                    string pwd = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebpwd"]));

                    OTPUrl = OTPUrl.Replace("{dcode}", dcode).Replace("{subuid}", subuid).Replace("{pwd}", pwd).Replace("{sender}", customerType)
                           .Replace("{pno}", MobileNo).Replace("{FirstName}", FirstName).Replace("{Password}", Password);

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
                            result = true;

                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    lblmsg.Text = "OTP Error";//e.Message;
                    result = false;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
            return result;
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }

        public static bool PostSMS(string URL, string data, string authInfo)
        {
            bool status = false;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);

                httpRequest.Method = "POST";

                httpRequest.Accept = "application/x-www-form-urlencoded";

                httpRequest.ContentType = "application/x-www-form-urlencoded";

                byte[] bytedata = Encoding.UTF8.GetBytes(data);

                Stream requestStream = httpRequest.GetRequestStream();

                requestStream.Write(bytedata, 0, bytedata.Length);

                requestStream.Close();

                //httpRequest.Headers.Add("Authorization", authInfo);
                httpRequest.Headers.Add("Authorization", "Basic " + authInfo);

                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                string resulXmlFromWebService = string.Empty;

                using (StreamReader srd = new StreamReader(httpResponse.GetResponseStream()))
                {
                    resulXmlFromWebService = srd.ReadToEnd();
                }

                status = true;
            }
            catch (Exception ex)
            {
                status = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }

            return status;
        }

        public bool SendEmail(string name, string userName, string password, string email)
        {
            bool blnRetVal = false;
            try
            {
                System.Text.StringBuilder strEmailBuilder = new System.Text.StringBuilder();

                strEmailBuilder.AppendFormat("<table border='0' cellspacing='0' cellpadding='10' width='500' align='center' style='border: 1px solid #ccc; font-family: Arial, Helvetica, sans-serif; font-size: 14px; border-collapse: collapse; line-height: 20px;'>");
                strEmailBuilder.AppendFormat("<tr><td colspan='2'>Dear " + name + ",<br /></td></tr>");
                strEmailBuilder.AppendFormat("<tr bgcolor='#f4f4f4'><td width='130' colspan='2'>Your login credentials are as follows -</td></tr>");
                strEmailBuilder.AppendFormat("<tr><td>Username:</td><td><strong>" + userName + " </strong></td></tr>");
                strEmailBuilder.AppendFormat("<tr><td>Password:</td><td><strong>" + password + " </strong></td></tr>");
                strEmailBuilder.AppendFormat("<tr bgcolor='#f4f4f4'><td colspan='2'><p>Kindly use the same for log in to the application.</p></td></tr>");
                strEmailBuilder.AppendFormat("<tr><td colspan='2'>Regards,<br /><strong>iMeeting Support Team</strong></td></tr>");
                strEmailBuilder.AppendFormat("<tr><td align='center' colspan='2' height='1'></td></tr>");
                strEmailBuilder.AppendFormat("</table>");

                MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                SmtpClient SMTPServer = new SmtpClient();

                string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                string Password = "";// = ConfigurationManager.AppSettings["EPassword"]
                XmlDocument xml = new XmlDocument();
                xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");
                XmlNodeList serverlist = xml.SelectNodes("//email");
                foreach (XmlNode servernodes in serverlist)
                {
                    EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                    Password = Encryptor.DecryptString(servernodes.SelectSingleNode("password").InnerText);
                    SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                    SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                    SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                }

                string EmailSubject = "Forgot Password";

                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                SMTPServer.UseDefaultCredentials = false;
                SMTPServer.Credentials = SMTPUserInfo;
                MailAddress fromAddress = new MailAddress(EmailFrom, "iMeeting Support");
                //objmail.Headers.Add("NAME", "Admin");
                objmail.From = fromAddress;
                objmail.To.Add(email);
                objmail.Body = strEmailBuilder.ToString();
                objmail.Subject = EmailSubject;

                objmail.IsBodyHtml = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                SMTPServer.Send(objmail);
                objmail.To.Clear();

                blnRetVal = true;
            }
            catch (Exception ex)
            {
                blnRetVal = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
            return blnRetVal;
        }
    }
}