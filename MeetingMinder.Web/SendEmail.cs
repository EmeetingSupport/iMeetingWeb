using MM.Core;
using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Xml;

namespace MeetingMinder.Web
{
    public class SendEmail
    {
        /// <summary>
        /// Send Notification email to maker
        /// </summary>        
        /// <param name="UserId">Guid specifying UserId</param>
        public void SendNotifyEmail(string ApprovalType, Guid UserId, out bool IsSuccess, out string Message)
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
                        Message = "Email address of maker not exists. Email sending failed. ";
                        IsSuccess = false;
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
                    if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(".") + "/img/Uploads/Email.xml"))
                    {
                        Message = "Email sending failed. Configuration setting missing";
                        IsSuccess = false;
                        return;
                    }
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
                    string EmailSubject = "Approval Notification";

                    //string strlink = @"http:///Login.aspx";
                    string EmailBody = @"<p>
	Dear " + objUser.FirstName + @",</p>
<p>
	&nbsp;</p>
<p>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
You have sent " + ApprovalType + @" for approval by " + System.Web.HttpContext.Current.Session["FName"].ToString() + " " + System.Web.HttpContext.Current.Session["LName"].ToString() + @". Please login to imeeting app to view details.</p>
<p>
	&nbsp;</p>
<p>
	Regards<br />
	iMeeting Admin Team</p>";


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
                    IsSuccess = true;
                    Message = "Email sent to checker.";
                }
                else
                {
                    IsSuccess = false;
                    Message = "Email sending failed.";
                }

            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;
                IsSuccess = false;
                Message = "Email sending failed. Try again after some time";
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public void SendUserMail(Guid userid)
        {
            try
            {
                UserDomain objUser = UserDataProvider.Instance.Get(userid);
                if (objUser != null)
                {
                    //get created user details
                    UserDomain objCreatedUser = UserDataProvider.Instance.Get(Guid.Parse(Convert.ToString(HttpContext.Current.Session["UserId"])));
                    string url = string.Empty;
                    string fullname = string.Empty;
                    string contact = string.Empty;
                    if (objCreatedUser != null)
                    {
                        fullname = objCreatedUser.Suffix + " " + objCreatedUser.FirstName + " " + objCreatedUser.LastName;
                        contact = objCreatedUser.Mobile;

                        string mob = objUser.Mobile.Substring(0, 2);
                        if (mob == "91")
                        {
                            url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NationalIPA"]);
                        }
                        else
                        {
                            url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NepalIPA"]);
                        }
                    }

                    string department = string.Empty;
                    EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(Convert.ToString(HttpContext.Current.Session["EntityId"])));
                    if (objEntity != null)
                    {
                        department = objEntity.EntityName;
                    }

                    //Email user name and password

                    string ToEmail = objUser.EmailID1;
                    string uname = objUser.UserName;
                    string passwd = objUser.Password;
                    string suffix = objUser.Suffix;
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
                        Password = Encryptor.DecryptString(servernodes.SelectSingleNode("password").InnerText);
                        SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                        SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                        SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                    }
                    string EmailSubject = "User Created";

                    //string strlink = @"http:///Login.aspx";
                    string EmailBody = string.Empty;
                    if (objUser.IsEnabledOnIpad)
                    {
                        EmailBody = @"<p><h3>Welcome " + suffix + " " + objUser.FirstName + " " + objUser.LastName + ".</h3> <br>Your account to access iMeeting application has been created successfully. Please login using below mentioned credentials.</p><p> Username: " + uname + @"</p> <p> Password: " + passwd + @"</p><p><a href=" + url + @"><b>Click here</b></a> to download the iPad application.</p><p>For any queries, please contact <b>" + fullname + @"</b>, Contact no. <b>" + contact + @"</b> and Department <b>" + department + @".</b></p>
<p>
    Regards <br/>
    iMeeting Admin Team</p> ";
                    }
                    else
                    {
                        EmailBody = @"<p><h3>Welcome " + suffix + " " + objUser.FirstName + " " + objUser.LastName + ".</h3> <br>Your account to access iMeeting application has been created successfully. Please login using below mentioned credentials.</p><p> Username: " + uname + @"</p> <p> Password: " + passwd + @"</p><p>For any queries, please contact <b>" + fullname + @"</b>, Contact no. <b>" + contact + @"</b> and Department <b>" + department + @".</b></p>
<p>
    Regards <br/>
    iMeeting Admin Team</p> ";
                    }



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
                    //IsSuccess = true;
                    //Message = "Email sent to checker.";
                }
                else
                {
                    //    IsSuccess = false;
                    //  Message = "Email sending failed.";
                }

            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;
                //IsSuccess = false;
                // Message = "Email sending failed. Try again after some time";
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
