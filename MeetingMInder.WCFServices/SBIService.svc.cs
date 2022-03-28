using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MM.Domain;
using MM.Data;
using MM.Services;
using MeetingMInder.WCFServices.Request;
using MeetingMInder.WCFServices.Response;
using MM.Core;
using System.Web.Configuration;
using System.Web;
using System.ServiceModel.Activation;
using System.Net;

using MeetingMInder.WCFServices.MessageInterceptor;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Data;

namespace MeetingMInder.WCFServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [MessageInspectorBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SBIService : ISBIService
    {
        // private readonly HttpContext context;

        // public Service1()
        //{
        //    context = HttpContext.Current;
        //}
        public static List<UserManager> objUserManager = new List<UserManager>();

        public string GetData()
        {
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\Uploads\\Email.xml";
            //System.Web.Hosting.HostingEnvironment.MapPath("/img/Uploads/Email.xml");
            return string.Format("Just Test ");
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        /// <summary>
        /// Get All Entity
        /// </summary>
        /// <returns></returns>
        public List<EntityDomain> GetEntity()
        {
            List<EntityDomain> objResponse = new List<EntityDomain>();
            EntityDomain objSpcl;
            EntityServices objService = new EntityServices();


            IList<EntityDomain> objSp = objService.GetAllEntityServices();
            //create List of entity
            foreach (EntityDomain entity in objSp)
            {
                objSpcl = new EntityDomain();
                objSpcl.EntityId = entity.EntityId;
                objSpcl.EntityName = entity.EntityName;


                objResponse.Add(objSpcl);

            }

            return objResponse;
        }

        /// <summary>
        ///  Code for Login Request
        /// </summary>
        /// <param name="objLogin">Class object specifying objLogin</param>
        /// <returns></returns> 
        public LoginResponse Login(LoginRequest objLogin)
        {

            SecurityQuestionService objQuestionService = new SecurityQuestionService();
            LoginResponse objResponse = new LoginResponse();
            UserServices objUserServices = new UserServices();
            EntityServices objEntityService = new EntityServices();
            EntityDetails objEntityDetails = null;
            List<EntityDetails> objEntity = new List<EntityDetails>();

            UserDomain objUser;
            string password = "";
            string RequestIdCurr = "";

            try
            {
                password = MM.Core.AesEncryption.AES_Decrypt256(objLogin.Password);
                if (password.IndexOf("!@#$") > -1)
                {
                    int ind = password.IndexOf("!@#$") + 4;
                    RequestIdCurr = password.Substring(0, ind - 4);
                    password = password.Substring(ind, password.Length - ind);
                }
            }
            catch (Exception e)
            {
                LogError objLog = new LogError();
                string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                string sPath = strddd + "img\\Uploads\\Logs\\";
                string file = "Log-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                bool exists = System.IO.Directory.Exists(sPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(sPath);
                StringBuilder sb = new StringBuilder("Error" + Environment.NewLine);

                sb.Append("Message: " + e.Message + Environment.NewLine);
                sb.Append("Source: " + e.Source + Environment.NewLine);
                sb.Append("TargetSite: " + e.TargetSite + Environment.NewLine);
                sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
                sb.Append("InnerException: " + e.InnerException);

                sb.Append(Environment.NewLine + Environment.NewLine);

                FileStream fs = new FileStream(sPath + file, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(sb.ToString());
                writer.Close();
                fs.Close();

                objResponse.Status = false;
                objResponse.Entity = null;

                return objResponse;
            }
            if (objLogin.RequestId != RequestIdCurr)
            {
                objResponse.Status = false;
                objResponse.Entity = null;

                return objResponse;
            }

            string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
            if (AuthenticationType.ToLower().Equals("ad"))
            {
                //Ad Auth
                string AdDomain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AdDomain"]);
                bool IsValid = UserAuthenticate.AuthUser(AdDomain, objLogin.UserName, password);

                if (IsValid)
                {
                    objUser = objUserServices.UserLoginIpad(objLogin.UserName.ToLower(), objLogin.UdId, objLogin.DeviceToken);
                }
                else
                {
                    objResponse.Status = false;
                    objResponse.Entity = null;

                    return objResponse;
                }
            }
            else
            {
                objUser = objUserServices.UserLoginIpad(objLogin.UserName.ToLower(), password, objLogin.Password, objLogin.DeviceToken, objLogin.UserId ?? Guid.Empty);
            }
            string token = GenerateToken();
            if (objUser != null)
            {
                if (objUser.IsEnabledOnIpad)
                {
                    string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OTPSend"]);

                    string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["OTPUrl"]);
                    string customerType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["sender"]);

                    objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());


                    if (OTPSend.ToLower().Equals("true"))
                    {
                        if (objUser.Mobile == null || objUser.Mobile.Length == 0)
                        {
                            objResponse.OTPError = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                            return objResponse;
                        }

                        if (!(objUser.Mobile.Length > 9))
                        {
                            objResponse.OTPError = "Invalid Mobile Number, Please Contact Administrator";
                            return objResponse;
                        }
                        // objResponse.OTPSend = Convert.ToBoolean(OTPSend);

                        try
                        {

                            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                            string dcode = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["dcode"]);
                            string subuid = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["subuid"]);
                            int OTPTimeOut = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["OTPTimeOut"]);

                            string pwd = Encryptor.DecryptString(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["pwd"]));

                            objResponse.OTPKey = "";
                            objResponse.OTPVersion = "";
                            OTPUrl = OTPUrl.Replace("{dcode}", dcode).Replace("{subuid}", subuid).Replace("{pwd}", pwd).Replace("{sender}", customerType)
                                   .Replace("{pno}", objUser.Mobile).Replace("{TimeOut}", OTPTimeOut.ToString()).Replace("{Date}", DateTime.Now.ToString("dd MMM hh:mm")).Replace("{OTP}", objUser.OTP);

                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(OTPUrl);
                            webRequest.Method = "GET";

                            using (WebResponse response = webRequest.GetResponse())
                            {
                                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                                {
                                    string soapResult = rd.ReadToEnd();
                                    if (soapResult.Length > 8)
                                    {
                                        objResponse.OTPKey = objUser.OTPId.ToString();
                                    }
                                    // Response.Write(soapResult);

                                }
                            }
                        }

                        catch (Exception e)
                        {
                            string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                            string sPath = strddd + "img\\Uploads\\error_Wcf.txt";

                            StringBuilder sb = new StringBuilder();
                            sb.Append("Time of Error: " + DateTime.Now.ToString("g") + Environment.NewLine);
                            HttpContext context = HttpContext.Current;
                            if (context != null)
                            {
                                sb.Append("error: " + e.Message + Environment.NewLine);
                                sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
                                sb.Append("URL: " + context.Request.Url + Environment.NewLine);
                                sb.Append("Form: " + context.Request.Form.ToString() + Environment.NewLine);
                                sb.Append("QueryString: " + context.Request.QueryString.ToString() + Environment.NewLine);
                                sb.Append("Server Name: " + context.Request.ServerVariables["SERVER_NAME"] + Environment.NewLine);
                                sb.Append("User Agent: " + context.Request.UserAgent + Environment.NewLine);
                                sb.Append("User IP: " + context.Request.UserHostAddress + Environment.NewLine);
                                sb.Append("User Host Name: " + context.Request.UserHostName + Environment.NewLine);
                                //sb.Append("User is Authenticated: " + context.User.Identity.IsAuthenticated.ToString() + Environment.NewLine);
                                //sb.Append("User Name: " + context.User.Identity.Name + Environment.NewLine);
                            }



                            sb.Append("\n\n");

                            FileStream fs = new FileStream(sPath, FileMode.Append, FileAccess.Write);
                            StreamWriter writer = new StreamWriter(fs);
                            writer.Write(sb.ToString());
                            writer.Close();
                            fs.Close();
                            //objResponse.OTPKey = "123456";
                            //objResponse.OTPVersion = "1";
                            objResponse.OTPError += "OTP Error";//e.Message;
                        }
                    }
                    else
                    {
                        objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
                    }
                    Guid UserId = objUser.UserId;

                    IList<EntityDomain> objEntityList = objEntityService.GetAllEntityByUsderId(UserId);


                    //Get request url



                    //Get request url

                    //string url = System.ServiceModel.OperationContext.Current.RequestContext.RequestMessage.Headers.To.ToString();
                    //string[] webUrl = url.Split('/');

                    foreach (EntityDomain entity in objEntityList)
                    {
                        objEntityDetails = new EntityDetails();

                        objEntityDetails.CreatedBy = entity.CreatedBy;
                        objEntityDetails.CreatedOn = entity.CreatedOn;
                        objEntityDetails.EntityChecker = entity.EntityChecker;
                        objEntityDetails.EntityId = entity.EntityId;
                        //objEntityDetails.EntityLogo = ("http://" + webUrl[2] + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + entity.EntityLogo).ToBase64Encryption();
                        objEntityDetails.EntityLogo = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + entity.EntityLogo).ToBase64Encryption();
                        objEntityDetails.EntityName = entity.EntityName;
                        objEntityDetails.EntityShortName = entity.EntityShortName;
                        objEntityDetails.IsEnable = entity.IsEnable;
                        objEntityDetails.UpdatedBy = entity.UpdatedBy;
                        objEntityDetails.UpdatedOn = entity.UpdatedOn;
                        //objEntityDetails.EntityMeeting = string.IsNullOrEmpty(entity.EntityMeeting) ? "" : ("http://" + webUrl[2] + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] +entity.EntityMeeting).ToBase64Encryption();
                        objEntityDetails.EntityMeeting = string.IsNullOrEmpty(entity.EntityMeeting) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + entity.EntityMeeting).ToBase64Encryption();

                        objEntity.Add(objEntityDetails);
                    }

                    objResponse.Status = true;
                    objResponse.Entity = objEntity;
                    objResponse.UserId = UserId;
                    objResponse.IsAuthorized = objUser.IsAuthorized;
                    objResponse.SecurityQuestionId = objUser.SecurityQuestionId;
                    //objResponse.OTPKey = objUser.OTPId.ToString();
                    // objResponse.Answer = objUser.Answer;
                    // objResponse.LostDevice = objUser.IsDeviceLost;
                    objResponse.DisplayName = objUser.Suffix + " " + objUser.FirstName + " " + objUser.LastName;
                    IList<SecurityQuestionDomain> objSecurity = objQuestionService.GetAllQuestion();
                    var strQuest = (from p in objSecurity
                                    where p.SecurityQuestionId == objResponse.SecurityQuestionId
                                    select p.SecurityQuestion).ToList();
                    if (strQuest.Count() > 0)
                    {
                        objResponse.SecurityQuestion = strQuest[0];
                    }

                    objResponse.IsLocked = objUser.IsLocked;

                    objResponse.UserExists = true;

                    //  HttpContext.Current.Session["USERLOGGEDIN"] = objUser.UserId;
                    // objResponse.EncryptionKey = objUser.FoodPreference;
                    //if (objUser.IsAuthorized)
                    //{
                    //    objResponse.Token = objUser.Token;
                    //}
                    //else
                    //{
                    //    objResponse.Token = "";
                    //}
                    //objResponse.Token


                    bool IsLost = IsLostDevice(objLogin.UserId ?? Guid.Empty);
                    if (IsLost)
                    {
                        WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
                    }

                    #region code comment on 10 Oct 2015 for removing token validation
                    //ExpireItems objExpire = new ExpireItems();
                    //UserManager objUserSession = new UserManager();
                    //objUserSession.StartTime = DateTime.Now;
                    //objUserSession.UserId = objUser.UserId.ToString();
                    //objUserSession.AcceeToken = token;
                    //objExpire.timer = new System.Timers.Timer(TimeSpan.FromMinutes(objUser.SessionTimeout).TotalMilliseconds);

                    //objExpire.timer.Elapsed += new System.Timers.ElapsedEventHandler(objExpire.Elapsed_Event);
                    //objExpire.AcceeToken = token;

                    //objExpire.timer.Start();
                    //objUserSession.timer = objExpire.timer;
                    //objUserSession.SessionTimeOut = objUser.SessionTimeout;
                    //   objUserManager.Add(objUserSession);
                    #endregion

                    objResponse.RequestId = AesEncryption.EncryptString256(objLogin.UserName + "!@#$" + objLogin.RequestId + "!@#$" + password);

                    WebOperationContext.Current.OutgoingResponse.Headers.Add("AccessToken", token);
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Property", MM.Core.AesEncryption.EncryptString256(objUser.EncryptionKey));




                    // UserManager ob = new UserManager(token, objUser.SessionTimeout, objUserManager);
                }
                else
                {
                    objResponse.Status = false;
                    objResponse.Entity = null;
                }
            }
            else
            {
                objResponse.Status = false;
                objResponse.Entity = null;

                //check user exists or not
                DataTable dt = new DataTable();
                dt = objUserServices.iPadUserExists(objLogin.UserName);
                if (dt.Rows.Count > 0)
                {
                    objResponse.UserExists = true;
                    objResponse.IsLocked = Null.SetNullBoolean(dt.Rows[0]["IsLocked"]);
                }
                else
                {
                    objResponse.UserExists = false;
                    objResponse.IsLocked = false;
                }
            }

            return objResponse;
        }

        /// <summary>
        /// code for Reset password request
        /// </summary>
        /// <param name="objResetPass">Class object specifying objResetPass</param>
        /// <returns></returns>
        public ResetPasswordResponse ResetPassword(ResetPasswordRequest objResetPass)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
            bool IsLost = IsLostDevice(objResetPass.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }
            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objResetPass.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }
            ResetPasswordResponse objPasswordReset = new ResetPasswordResponse();

            SecurityQuestionService objService = new SecurityQuestionService();

            UserServices objUserService = new UserServices();

            SecurityQuestionDomain objSecurityQuestion = objService.GetSecurityQuestion(objResetPass.UserId);

            if (objSecurityQuestion != null)
            {

                string strDbAnswer = Convert.ToString(objSecurityQuestion.Answer);
                Guid strQuestionId = objSecurityQuestion.SecurityQuestionId;

                if (objResetPass.SecurityQuestionId.Equals(strQuestionId))
                {
                    //check Answer  is same as in database answer
                    if (strDbAnswer.ToLower().Equals(objResetPass.Answer.ToLower()))
                    {
                        bool isChange = objUserService.ChangePassword(objResetPass.UserId, objResetPass.OldPassword, objResetPass.NewPassword);
                        if (isChange)
                        {
                            objPasswordReset.IsPasswordChanged = true;
                        }
                    }
                    else
                    {
                        objPasswordReset.IsPasswordChanged = false;
                    }
                }
                else
                {
                    objPasswordReset.IsPasswordChanged = false;
                }
            }
            else
            {
                objPasswordReset.IsPasswordChanged = false;
            }

            return objPasswordReset;
        }

        /// <summary>
        /// code for forget password request
        /// </summary>
        /// <param name="objForgetPass">Class object specifying objForgetPass</param>
        /// <returns></returns>
        public ForgetPasswordResponse ForgetPassword(ForgetPasswordRequest objForgetPass)
        {
            UserServices objUserService = new UserServices();

            ForgetPasswordResponse objResponse = new ForgetPasswordResponse();

            string objPassword = objUserService.ForgetPassword(Encryptor.EncryptString(objForgetPass.UserName), objForgetPass.SecurityQuestionId, objForgetPass.Answer);
            bool IsLost = IsLostDevice(objForgetPass.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }
            objResponse.Password = objPassword;
            return objResponse;
        }

        /// <summary>
        /// code for Change password request comment on 16 March 19
        /// </summary>
        /// <param name="objChangePassword">Class object specifying objResetPass</param>
        /// <returns></returns>
        //public ChangePasswordResponse ChangePassword(ChangePasswordRequest objChangePassword)
        //{
        //    ChangePasswordResponse objChangePasswordResp = new ChangePasswordResponse();

        //    UserServices objUserService = new UserServices();

        //    string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

        //    System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(objChangePassword.NewPassword, passwordRegex);

        //    if (!match.Success)
        //    {
        //        objChangePasswordResp.Status = "failed";
        //        return objChangePasswordResp;
        //    }


        //    bool isChange = objUserService.ChangePassword(objChangePassword.UserId, objChangePassword.OldPassword, objChangePassword.NewPassword);
        //    if (isChange)
        //    {
        //        objChangePasswordResp.Status = "Success";
        //    }
        //    else
        //    {
        //        objChangePasswordResp.Status = "failed";
        //    }
        //    bool IsLost = IsLostDevice(objChangePassword.UserId);
        //    if (IsLost)
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
        //    }
        //    // objChangePasswordResp.LostDevice = IsLostDevice(objChangePassword.UserId);
        //    return objChangePasswordResp;
        //}

        /// <summary>
        /// encrypted password
        /// </summary>
        /// <param name="objChangePassword"></param>
        /// <returns></returns>
        public ChangePasswordResponse ChangePassword(ChangePasswordRequest objChangePassword)
        {
            ChangePasswordResponse objChangePasswordResp = new ChangePasswordResponse();
            string OldPassword = string.Empty;
            string NewPassword = string.Empty;
            string password = string.Empty;
            string RequestIdCurr = string.Empty;

            UserServices objUserService = new UserServices();

            try
            {
                password = MM.Core.AesEncryption.AES_Decrypt256(objChangePassword.Password);
                if (password.IndexOf("!@#$") > -1)
                {
                    int ind = password.IndexOf("!@#$") + 4;
                    RequestIdCurr = password.Substring(0, ind - 4);
                    password = password.Substring(ind, password.Length - ind);

                    int ind1 = password.IndexOf("!@#$") + 4;
                    OldPassword = password.Substring(0, ind1 - 4);
                    NewPassword = password.Substring(ind1, password.Length - ind1);
                }
            }
            catch (Exception e)
            {
                LogError objLog = new LogError();
                string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

                string sPath = strddd + "img\\Uploads\\Logs\\";
                string file = "Log-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                bool exists = System.IO.Directory.Exists(sPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(sPath);
                StringBuilder sb = new StringBuilder("Error" + Environment.NewLine);

                sb.Append("Message: " + e.Message + Environment.NewLine);
                sb.Append("Source: " + e.Source + Environment.NewLine);
                sb.Append("TargetSite: " + e.TargetSite + Environment.NewLine);
                sb.Append("StackTrace: " + e.StackTrace + Environment.NewLine);
                sb.Append("InnerException: " + e.InnerException);

                sb.Append(Environment.NewLine + Environment.NewLine);

                FileStream fs = new FileStream(sPath + file, FileMode.Append, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(sb.ToString());
                writer.Close();
                fs.Close();

                objChangePasswordResp.Status = "failed";
                return objChangePasswordResp;
            }
            if (objChangePassword.RequestId != RequestIdCurr)
            {
                objChangePasswordResp.Status = "failed";
                return objChangePasswordResp;
            }

            string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(NewPassword, passwordRegex);

            if (!match.Success)
            {
                objChangePasswordResp.Status = "failed";
                return objChangePasswordResp;
            }

            if (OldPassword == NewPassword)
            {
                objChangePasswordResp.Status = "failed";
                objChangePasswordResp.Message = "New Password should not be same as old password";
                return objChangePasswordResp;
            }

            System.Data.DataTable dt = new System.Data.DataTable();
            dt = UserDataProvider.Instance.ChangePasswordWebNew(objChangePassword.UserId, NewPassword);
            string[] data = dt.AsEnumerable().Where(s => s.Field<string>("Password") == Encryptor.EncryptString(NewPassword)).Select(s => s.Field<string>("Password")).ToArray();
            int arrcount = data.Length;
            if (arrcount == 0)
            {

                bool isChange = objUserService.ChangePassword(objChangePassword.UserId, OldPassword, NewPassword);
                if (isChange)
                {
                    objChangePasswordResp.Status = "Success";
                }
                else
                {
                    objChangePasswordResp.Status = "failed";
                    objChangePasswordResp.Message = "Please check your old password";
                    return objChangePasswordResp;
                }
            }
            else
            {
                objChangePasswordResp.Status = "failed";
                objChangePasswordResp.Message = "Previous 3 password is not allow!";
                return objChangePasswordResp;
            }

            bool IsLost = IsLostDevice(objChangePassword.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }
            // objChangePasswordResp.LostDevice = IsLostDevice(objChangePassword.UserId);
            return objChangePasswordResp;
        }

        public EmailResponse SendEmail(EmailRequest objRequest)
        {
            EmailResponse objResponse = new EmailResponse();
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];

            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }
            string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "\\img\\Uploads\\Email.xml";
            //System.Web.Hosting.HostingEnvironment.MapPath("/img/Uploads/Email.xml");
            if (!System.IO.File.Exists(xmlPath))// System.Web.HttpContext.Current.Server.MapPath("/img/Uploads/Email.xml")));//
            {
                //System.Web.HttpContext.Current.Server.MapPath("img/Uploads/Email.xml");
                objResponse.Error = " Email sending failed. Configuration setting missing, please contact administrator.";
                objResponse.Status = "Email sending failed.";
                return objResponse;
            }
            try
            {
                MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                SmtpClient SMTPServer = new SmtpClient();

                string EmailFrom = "";
                string Password = "";
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlPath);
                XmlNodeList serverlist = xml.SelectNodes("//email");
                foreach (XmlNode servernodes in serverlist)
                {
                    EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                    Password = servernodes.SelectSingleNode("password").InnerText;
                    SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                    SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                    SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                }

                string EmailSubject = objRequest.Subject;


                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
                SMTPServer.UseDefaultCredentials = false;
                SMTPServer.Credentials = SMTPUserInfo;
                //SMTPServer.EnableSsl = true;

                MailAddress fromAddress = new MailAddress(EmailFrom, "eMeeting Admin");
                objmail.Headers.Add("NAME", "Admin");
                objmail.From = fromAddress;
                objmail.To.Add(objRequest.To);
                objmail.Body = objRequest.Body;
                objmail.Subject = EmailSubject;
                if (objRequest.CC.Length > 0)
                {
                    objmail.CC.Add(objRequest.CC);
                }

                if (objRequest.BCC.Length > 0)
                {
                    objmail.Bcc.Add(objRequest.BCC);
                }

                if (objRequest.ReplyTo.Length > 0)
                {
                    objmail.ReplyToList.Add(objRequest.ReplyTo);
                }
                objmail.IsBodyHtml = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                SMTPServer.Send(objmail);
                objmail.To.Clear();
                objResponse.Status = "Email sent successfully.";
                return objResponse;
            }
            catch (Exception Ex)
            {
                objResponse.Error = " Email sending failed. " + Ex.Message;
                objResponse.Status = "Email sending failed.";
                return objResponse;
            }
        }

        #region "Get security question"
        /// <summary>
        /// Get all squcurity question and ids
        /// </summary>
        /// <returns></returns>
        //public List<SecurityQuestionsResponse> GetSecurityQuestion()
        //{
        //    string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];

        //    if (token != null && token.Length > 0)
        //    {
        //        if (!ValidateToken(token))
        //        {
        //            WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
        //        return null;
        //    }
        //    SecurityQuestionService objQuestionService = new SecurityQuestionService();
        //    SecurityQuestionsResponse objSecurityQuestions = null;
        //    List<SecurityQuestionsResponse> objResponse = new List<SecurityQuestionsResponse>();
        //    IList<SecurityQuestionDomain> objQuestion = objQuestionService.GetAllQuestion();

        //    foreach (SecurityQuestionDomain quest in objQuestion)
        //    {
        //        objSecurityQuestions = new SecurityQuestionsResponse();
        //        objSecurityQuestions.SecurityQuestion = quest.SecurityQuestion;
        //        objSecurityQuestions.SecurityQuestionId = quest.SecurityQuestionId;
        //        objResponse.Add(objSecurityQuestions);
        //    }
        //    return objResponse;
        //}
        #endregion

        /// <summary>
        /// Get all data for wcf services
        /// </summary>
        /// <param name="objRequest">Class object specifying objRequest</param>
        /// <returns></returns>
        public GetAuthenticateDataResponse GetAuthenticateData(GetAuthenticateDataRequest objRequest)
        {
            //string strddd = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //LogError objEr = new LogError();
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
            //  objEr.WriteToFile("  token str" + token);

            GetAuthenticateDataResponse objResponse = new GetAuthenticateDataResponse();
            bool IsLost = IsLostDevice(objRequest.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }

            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    //   objEr.WriteToFile("  token invalid ");
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                //  objEr.WriteToFile("  token valid ");
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }
            //  objEr.WriteToFile("  before response ");

            UserEntityServices objUserEntity = new UserEntityServices();
            List<EntityDetails> objEntityList = new List<EntityDetails>();
            List<ForumDetails> objForumList = new List<ForumDetails>();
            List<MeetingDetails> objMeetingList = new List<MeetingDetails>();
            List<AgendaDetails> objAgendaList = new List<AgendaDetails>();
            List<NoticeDetails> objNoticeList = new List<NoticeDetails>();
            List<MinutesDetails> objMinList = new List<MinutesDetails>();
            string ResponseTime = "";
            System.Data.DataSet ds = null;

            //first request
            if (objRequest.StartTime.ToLower().Equals("0") || objRequest.UpdatedTimeStamp.ToLower().Equals("1"))
            {
                ds = objUserEntity.GetDataForWcfDataByEntity(objRequest.UserId, objRequest.EntityId, objRequest.UdId);
            }
            else
            {
                Int64 interval = Convert.ToInt64(objRequest.StartTime);
                DateTime dt = DateTime.Now.ToDateTime(interval);
                ds = objUserEntity.GetDataForWcfWithEntity(objRequest.UserId, dt, System.DateTime.Now, objRequest.EntityId, objRequest.UdId);
            }

            if (ds.Tables.Count > 0)
            {
                EntityDetails objEntity = null;
                //Get request url
                //string url = System.ServiceModel.OperationContext.Current.RequestContext.RequestMessage.Headers.To.ToString();
                //string[] webUrl = url.Split('/');

                //var url = HttpContext.Current.Request.Url;
                //var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

                //Entity details
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    objEntity = new EntityDetails();
                    objEntity.EntityId = Guid.Parse(ds.Tables[0].Rows[i]["EntityId"].ToString());
                    objEntity.EntityLogo = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityLogo"].ToString()))).ToBase64Encryption();

                    //objEntity.EntityLogo = ("http://" + webUrl[2]+ "/" +(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityLogo"].ToString()))).ToBase64Encryption();
                    //"img/Uploads/EntityLogo/" + Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityLogo"].ToString());
                    objEntity.EntityName = Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityName"].ToString());
                    objEntity.EntityShortName = Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityShortName"].ToString());
                    objEntity.IsEnable = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsEnable"].ToString());
                    objEntity.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString());
                    objEntity.UpdatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdatedOn"].ToString());
                    objEntity.CreatedBy = Guid.Parse(ds.Tables[0].Rows[i]["CreatedBy"].ToString());
                    objEntity.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[i]["UpdatedBy"].ToString());
                    objEntity.EntityChecker = Guid.Parse(ds.Tables[0].Rows[i]["EntityChecker"].ToString());
                    objEntity.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());

                    objEntity.EntityMeeting = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EntityMeeting"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] + Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityMeeting"].ToString())).ToBase64Encryption();
                    //objEntity.EntityMeeting = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EntityMeeting"].ToString()) ? "" : ("http://" + webUrl[2] + "/" + System.Configuration.ConfigurationManager.AppSettings["EntityLogo"] +  Encryptor.DecryptString(ds.Tables[0].Rows[i]["EntityMeeting"].ToString())).ToBase64Encryption();
                    //objEntity.ApprovedBy = Guid.Parse(ds.Tables[0].Rows[i]["ApprovedBy"].ToString());
                    objEntityList.Add(objEntity);
                }
                ForumDetails objForum = null;
                //Forum details
                for (int j = 0; j <= ds.Tables[1].Rows.Count - 1; j++)
                {
                    objForum = new ForumDetails();
                    objForum.ForumId = Guid.Parse(ds.Tables[1].Rows[j]["ForumId"].ToString());
                    objForum.ForumShortName = Encryptor.DecryptString(ds.Tables[1].Rows[j]["ForumShortName"].ToString());
                    objForum.EntityName = Encryptor.DecryptString(ds.Tables[1].Rows[j]["EntityName"].ToString());
                    objForum.EntityId = Guid.Parse(ds.Tables[1].Rows[j]["EntityId"].ToString());
                    objForum.ForumChecker = Guid.Parse(ds.Tables[1].Rows[j]["ForumChecker"].ToString());
                    objForum.IsEnable = Convert.ToBoolean(ds.Tables[1].Rows[j]["IsEnable"].ToString());
                    //objForum.MembersInfo = string.IsNullOrEmpty(ds.Tables[1].Rows[j]["MembersInfo"].ToString()) ? "" : ("http://" + webUrl[2] + "/" + System.Configuration.ConfigurationManager.AppSettings["MembesInfo"] + ds.Tables[1].Rows[j]["MembersInfo"].ToString()).ToBase64Encryption();
                    objForum.MembersInfo = string.IsNullOrEmpty(ds.Tables[1].Rows[j]["MembersInfo"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["MembesInfo"] + ds.Tables[1].Rows[j]["MembersInfo"].ToString()).ToBase64Encryption();

                    // objForum.MembersInfo = string.IsNullOrEmpty(ds.Tables[1].Rows[j]["MembersInfo"].ToString()) ? "" : ("http://"+  VirtualPathUtility.ToAbsolute("~/img/Uploads/Forum/"+ds.Tables[1].Rows[j]["MembersInfo"].ToString())).ToBase64Encryption();
                    //"img/Uploads/Forum/"+ds.Tables[1].Rows[j]["MembersInfo"].ToString();
                    objForum.UpdatedOn = Convert.ToDateTime(ds.Tables[1].Rows[j]["UpdatedOn"].ToString());
                    objForum.CreatedOn = Convert.ToDateTime(ds.Tables[1].Rows[j]["CreatedOn"].ToString());
                    objForum.CreatedBy = Guid.Parse(ds.Tables[1].Rows[j]["CreatedBy"].ToString());
                    objForum.UpdatedBy = Guid.Parse(ds.Tables[1].Rows[j]["UpdatedBy"].ToString());
                    objForum.ForumName = Encryptor.DecryptString(ds.Tables[1].Rows[j]["ForumName"].ToString());

                    if (ds.Tables[1].Rows[j]["IsActive"].ToString() != null && (ds.Tables[1].Rows[j]["IsActive"].ToString() == "1" || ds.Tables[1].Rows[j]["IsActive"].ToString().ToLower().Equals("true")))
                    {
                        objForum.IsActive = true;
                    }
                    else
                    {
                        objForum.IsActive = false;
                    }

                    // objForum.IsActive = Convert.ToBoolean(ds.Tables[1].Rows[j]["IsActive"].ToString());
                    objForum.ForumOrder = Null.SetNullInteger(ds.Tables[1].Rows[j]["ForumOrder"]);
                    objForumList.Add(objForum);
                }

                MeetingDetails objMeeting = null;
                //Meeting details
                for (int k = 0; k <= ds.Tables[2].Rows.Count - 1; k++)
                {
                    objMeeting = new MeetingDetails();
                    objMeeting.MeetingId = Guid.Parse(ds.Tables[2].Rows[k]["MeetingId"].ToString());
                    objMeeting.MeetingTime = Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingTime"].ToString());
                    objMeeting.MeetingVenue = Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingVenue"].ToString());
                    objMeeting.UpdatedBy = Guid.Parse(ds.Tables[2].Rows[k]["UpdatedBy"].ToString());
                    objMeeting.CreatedBy = Guid.Parse(ds.Tables[2].Rows[k]["CreatedBy"].ToString());
                    objMeeting.CreatedOn = Convert.ToDateTime(ds.Tables[2].Rows[k]["CreatedOn"].ToString());
                    objMeeting.UpdatedOn = Convert.ToDateTime(ds.Tables[2].Rows[k]["UpdatedOn"].ToString());
                    objMeeting.ForumName = Encryptor.DecryptString(ds.Tables[2].Rows[k]["ForumName"].ToString());
                    objMeeting.EntityName = Encryptor.DecryptString(ds.Tables[2].Rows[k]["EntityName"].ToString());
                    objMeeting.MeetingChecker = Guid.Parse(ds.Tables[2].Rows[k]["MeetingChecker"].ToString());
                    objMeeting.EntityId = Guid.Parse(ds.Tables[2].Rows[k]["EntityId"].ToString());
                    objMeeting.ForumId = Guid.Parse(ds.Tables[2].Rows[k]["ForumId"].ToString());
                    objMeeting.MeetingDate = Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingDate"].ToString())).ToString("D");
                    //   objMeeting.IsActive = Convert.ToBoolean(ds.Tables[2].Rows[k]["IsActive"].ToString());
                    if (ds.Tables[2].Rows[k]["IsActive"].ToString() != null && (ds.Tables[2].Rows[k]["IsActive"].ToString() == "1" || ds.Tables[2].Rows[k]["IsActive"].ToString().ToLower().Equals("true")))
                    {
                        objMeeting.IsActive = true;
                    }
                    else
                    {
                        objMeeting.IsActive = false;
                    }

                    // Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingTime"].ToString()) +Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingDate"].ToString()));
                    // (Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingDate"].ToString())) + " 1:00:00 PM";
                    string meetting = (Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingDate"].ToString())) + " " + Encryptor.DecryptString(ds.Tables[2].Rows[k]["MeetingTime"].ToString());
                    DateTime dtMeeting = Convert.ToDateTime(meetting);
                    objMeeting.MeetingTimeStamp = dtMeeting.ToUnixTimestamp().ToString();
                    objMeeting.PublishVersion = ds.Tables[2].Rows[k]["PublishVersion"].ToString();

                    objMeeting.MeetingNumber = ds.Tables[2].Rows[k]["MeetingNumber"].ToString();

                    objMeeting.MeetingHeader = ds.Tables[2].Rows[k]["Header"].ToString();
                    objMeeting.MeetingType = ds.Tables[2].Rows[k]["MeetingType"].ToString();

                    string Convenor = ds.Tables[2].Rows[k]["Convenor"].ToString();
                    if (Convenor.ToLower().Contains(objRequest.UserId.ToString().ToLower()))
                    {
                        objMeeting.Convenor = objRequest.UserId.ToString();
                    }
                    else
                    {
                        objMeeting.Convenor = "00000000-0000-0000-0000-000000000000";//ds.Tables[2].Rows[k]["Convenor"].ToString();
                    }

                    //Past Meeting number 
                    string pastMeeting = ds.Tables[2].Rows[k]["PastMeeting"].ToString();
                    if (pastMeeting != string.Empty && pastMeeting != "0")
                    {
                        objMeeting.PastMeeting = ds.Tables[2].Rows[k]["PastMeeting"].ToString();
                    }
                    else
                    {
                        objMeeting.PastMeeting = Convert.ToString(ds.Tables[9].Rows[0]["Value"]);
                    }



                    objMeetingList.Add(objMeeting);
                }

                NoticeDetails objNotice = null;
                //Notice details
                for (int l = 0; l <= ds.Tables[3].Rows.Count - 1; l++)
                {
                    objNotice = new NoticeDetails();
                    objNotice.NoticeId = Guid.Parse(ds.Tables[3].Rows[l]["NoticeId"].ToString());
                    objNotice.EntityId = Guid.Parse(ds.Tables[3].Rows[l]["EntityId"].ToString());
                    objNotice.ForumId = Guid.Parse(ds.Tables[3].Rows[l]["ForumId"].ToString());
                    objNotice.IsApproved = ds.Tables[3].Rows[l]["IsApproved"].ToString();
                    objNotice.MeetingDate = Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingDate"].ToString());
                    objNotice.MeetingTime = Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingTime"].ToString());
                    objNotice.MeetingVenue = Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingVenue"].ToString());
                    objNotice.NoticeMessage = ds.Tables[3].Rows[l]["NoticeMessage"].ToString().Replace("{Meeting_Number}", ds.Tables[3].Rows[l]["MeetingNumber"].ToString()).Replace("{Committee}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["ForumName"].ToString())).Replace("{Entity}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["EntityName"].ToString())).Replace("{Date}", Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingDate"].ToString())).ToString("D")).Replace("{Time}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingTime"].ToString())).Replace("{Venue}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingVenue"].ToString())); ;
                    //if (Encryptor.DecryptString(ds.Tables[3].Rows[l]["GeneralManager"].ToString()) != "")
                    //{
                    //    objNotice.NoticeMessage = ds.Tables[3].Rows[l]["NoticeMessage"].ToString().Replace("{Meeting_Number}", ds.Tables[3].Rows[l]["MeetingNumber"].ToString()).Replace("{Committee}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["ForumName"].ToString())).Replace("{Entity}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["EntityName"].ToString())).Replace("{Date}", Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingDate"].ToString())).ToString("D")).Replace("{Time}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingTime"].ToString())).Replace("{Venue}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingVenue"].ToString())).Replace("{General Manager}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["GeneralManager"].ToString()));
                    //}
                    //else
                    //{
                    //    objNotice.NoticeMessage = ds.Tables[3].Rows[l]["NoticeMessage"].ToString().Replace("{Meeting_Number}", ds.Tables[3].Rows[l]["MeetingNumber"].ToString()).Replace("{Committee}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["ForumName"].ToString())).Replace("{Entity}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["EntityName"].ToString())).Replace("{Date}", Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingDate"].ToString())).ToString("D")).Replace("{Time}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingTime"].ToString())).Replace("{Venue}", Encryptor.DecryptString(ds.Tables[3].Rows[l]["MeetingVenue"].ToString()));
                    //}
                    objNotice.UpdatedBy = Guid.Parse(ds.Tables[3].Rows[l]["UpdatedBy"].ToString());
                    objNotice.CreatedBy = Guid.Parse(ds.Tables[3].Rows[l]["CreatedBy"].ToString());
                    objNotice.CreatedOn = Convert.ToDateTime(ds.Tables[3].Rows[l]["CreatedOn"].ToString()).ToString("MMMM dd, yyyy");
                    objNotice.UpdatedOn = Convert.ToDateTime(ds.Tables[3].Rows[l]["UpdatedOn"].ToString()).ToString("MMMM dd, yyyy");
                    objNotice.NoticeChecker = Guid.Parse(ds.Tables[3].Rows[l]["NoticeChecker"].ToString());
                    objNotice.MeetingId = Guid.Parse(ds.Tables[3].Rows[l]["MeetingId"].ToString());
                    // objNotice.IsActive = Convert.ToBoolean(ds.Tables[3].Rows[l]["IsActive"].ToString());

                    if (ds.Tables[3].Rows[l]["IsActive"].ToString() != null && (ds.Tables[3].Rows[l]["IsActive"].ToString() == "1" || ds.Tables[3].Rows[l]["IsActive"].ToString().ToLower().Equals("true")))
                    {
                        objNotice.IsActive = true;
                    }
                    else
                    {
                        objNotice.IsActive = false;
                    }

                    objNotice.IsSigned = Convert.ToBoolean(ds.Tables[3].Rows[l]["IsSigned"].ToString());
                    objNotice.DisplayName = Encryptor.DecryptString(ds.Tables[3].Rows[l]["Suffix"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[3].Rows[l]["FirstName"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[3].Rows[l]["LastName"].ToString());
                    objNoticeList.Add(objNotice);
                }

                AgendaDetails objAgenda = null;
                //Agenda details  
                for (int m = 0; m <= ds.Tables[4].Rows.Count - 1; m++)
                {
                    objAgenda = new AgendaDetails();
                    objAgenda.AgendaId = Guid.Parse(ds.Tables[4].Rows[m]["AgendaId"].ToString());
                    objAgenda.AgendaName = ds.Tables[4].Rows[m]["AgendaName"].ToString().Replace("`", "&#x20B9");

                    string AgendaPdf = ds.Tables[4].Rows[m]["AgendaNote"].ToString();
                    //--//
                    objAgenda.AgendaNote = ds.Tables[4].Rows[m]["AgendaNote"].ToString();

                    objAgenda.AgendaOrder = Convert.ToInt64(ds.Tables[4].Rows[m]["AgendaOrder"].ToString());
                    objAgenda.CreatedBy = Guid.Parse(ds.Tables[4].Rows[m]["CreatedBy"].ToString());
                    objAgenda.UpdatedBy = Guid.Parse(ds.Tables[4].Rows[m]["UpdatedBy"].ToString());
                    objAgenda.UpdateOn = Convert.ToDateTime(ds.Tables[4].Rows[m]["UpdateOn"].ToString());
                    objAgenda.CreatedOn = Convert.ToDateTime(ds.Tables[4].Rows[m]["CreatedOn"].ToString());
                    objAgenda.EntityId = Guid.Parse(ds.Tables[4].Rows[m]["EntityId"].ToString());
                    objAgenda.EntityName = Encryptor.DecryptString(ds.Tables[4].Rows[m]["EntityName"].ToString());
                    objAgenda.ForumId = Guid.Parse(ds.Tables[4].Rows[m]["ForumId"].ToString());
                    objAgenda.MeetingId = Guid.Parse(ds.Tables[4].Rows[m]["MeetingId"].ToString());
                    objAgenda.ParentAgendaId = Guid.Parse(ds.Tables[4].Rows[m]["ParentAgendaId"].ToString());
                    objAgenda.MeetingVenue = Encryptor.DecryptString(ds.Tables[4].Rows[m]["MeetingVenue"].ToString());
                    objAgenda.Classification = ds.Tables[4].Rows[m]["Classification"].ToString();

                    objAgenda.SerialKeyType = ds.Tables[4].Rows[m]["SerialNumberType"].ToString();

                    objAgenda.Presenter = ds.Tables[4].Rows[m]["Presenter"].ToString();
                    if (objAgenda.ParentAgendaId == Guid.Empty)
                    {
                        objAgenda.SerialNumber = ds.Tables[4].Rows[m]["SerialNumber"].ToString();
                        objAgenda.SerialKey = "";
                    }
                    else
                    {
                        objAgenda.SerialKey = ds.Tables[4].Rows[m]["SerialNumber"].ToString();
                        objAgenda.SerialNumber = "";
                    }

                    objAgenda.AgendaSerialNo = GetTitles(ds.Tables[4].Rows[m]["SerialTitle"].ToString(), ds.Tables[4].Rows[m]["SerialNumber"].ToString(), ds.Tables[4].Rows[m]["SerialText"].ToString());

                    if (ds.Tables[4].Rows[m]["FileUploadOn"].ToString() == "")
                    {
                        objAgenda.FileUploadOn = MM.Core.Null.SetNullDateTime(null).ToUnixTimestamp().ToString();
                    }
                    else
                    {
                        objAgenda.FileUploadOn = MM.Core.Null.SetNullDateTime(ds.Tables[4].Rows[m]["FileUploadOn"]).ToUnixTimestamp().ToString();
                    }
                    //objAgenda.EncryptionKey = Encryptor.DecryptString(ds.Tables[4].Rows[m]["EncryptionKey"].ToString());
                    string AgendaFile = ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString();

                    //--//
                    //-- //objAgenda.UploadedAgendaNote = string.IsNullOrEmpty(ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["Agenda"] + ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString())).ToBase64Encryption();
                    //--//
                    objAgenda.UploadedAgendaNote = string.IsNullOrEmpty(ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/emeetings_igendapdf-" + (HttpUtility.UrlEncode(ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString().Substring(0, ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString().Length - 4)) + ".pdf")).ToBase64Encryption();


                    //   objAgenda.UploadedAgendaNote = string.IsNullOrEmpty(ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString()) ? "" : ("http://" + webUrl[2]+ "/" + (System.Configuration.ConfigurationManager.AppSettings["Agenda"] + ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString())).ToBase64Encryption();
                    //"img/Uploads/Agenda/" + ds.Tables[4].Rows[m]["UploadedAgendaNote"].ToString();
                    objAgenda.ForumName = Encryptor.DecryptString(ds.Tables[4].Rows[m]["ForumName"].ToString());
                    objAgenda.AgendaChecker = Guid.Parse(ds.Tables[4].Rows[m]["AgendaChecker"].ToString());
                    //  objAgenda.IsActive = Convert.ToBoolean(ds.Tables[4].Rows[m]["IsActive"].ToString());


                    if (ds.Tables[4].Rows[m]["IsActive"].ToString() != null && (ds.Tables[4].Rows[m]["IsActive"].ToString() == "1" || ds.Tables[4].Rows[m]["IsActive"].ToString().ToLower().Equals("true")))
                    {
                        objAgenda.IsActive = true;
                    }
                    else
                    {
                        objAgenda.IsActive = false;
                    }

                    string DelPdfs = ds.Tables[4].Rows[m]["DeletedPDfs"].ToString();

                    List<Pdfs> upPdfs = new List<Pdfs>();
                    if (!string.IsNullOrEmpty(AgendaFile))
                    {
                        string[] arrAgendaFiles = AgendaFile.Replace(" ", "").Split(',');
                        string[] arrAgendaPdfs = AgendaPdf.Replace(" ", "").Split(',');

                        string[] arrDeletedpdf = DelPdfs.Replace(" ", "").Split(',');
                        bool SendPdfFile = false;
                        foreach (string str in arrAgendaFiles)

                            if (!arrDeletedpdf.Contains(str))
                            {
                                SendPdfFile = true;
                            }



                        if (arrAgendaFiles.Count() > 0 && SendPdfFile)
                        {
                            if (arrAgendaFiles.Count() > 1)
                            {
                                //-// objAgenda.UploadedAgendaNote = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["Agenda"] + arrAgendaFiles[0].Trim().Split('.')[0] + "_merge.pdf")).ToBase64Encryption();
                                objAgenda.UploadedAgendaNote = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/emeetings_igendapdf-" + (HttpUtility.UrlEncode(arrAgendaFiles[0].Trim().Substring(0, arrAgendaFiles[0].Trim().Length - 4) + "_merge.pdf"))).ToBase64Encryption();
                            }
                            //for (int pdfUp = 0; pdfUp <= arrAgendaFiles.Count() - 1; pdfUp++)
                            //{
                            //    Pdfs objPdf = new Pdfs();
                            //    objPdf.FileUrl = string.IsNullOrEmpty(arrAgendaFiles[pdfUp]) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["Agenda"] + arrAgendaFiles[pdfUp].Trim())).ToBase64Encryption();
                            //    objPdf.PdfName = arrAgendaPdfs[pdfUp].Trim();
                            //    objPdf.IsActive = true;
                            //    if (arrDeletedpdf.Contains(arrAgendaFiles[pdfUp].Trim()))
                            //    {
                            //        objPdf.IsActive = false;
                            //    }
                            //    upPdfs.Add(objPdf);
                            //}
                        }
                        else
                        {
                            objAgenda.UploadedAgendaNote = "";
                        }
                    }
                    else
                    {
                        objAgenda.UploadedAgendaNote = "";
                    }
                    //objAgenda.UploadedFiles = upPdfs;
                    //objAgenda.UploadedAgendaNote = upPdfs;
                    objAgendaList.Add(objAgenda);
                }

                MinutesDetails objMin = null;
                //Minutes details
                for (int s = 0; s <= ds.Tables[5].Rows.Count - 1; s++)
                {
                    objMin = new MinutesDetails();
                    objMin.UploadMinuteId = Guid.Parse(ds.Tables[5].Rows[s]["UploadMinuteId"].ToString());
                    //objMin.UploadFile = string.IsNullOrEmpty(ds.Tables[5].Rows[s]["UploadFile"].ToString()) ? "" :
                    //      ("http://" +webUrl[2] +"/"+(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"] + ds.Tables[5].Rows[s]["UploadFile"].ToString())).ToBase64Encryption();

                    objMin.UploadFile = string.IsNullOrEmpty(ds.Tables[5].Rows[s]["UploadFile"].ToString()) ? "" :
                         (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"] + ds.Tables[5].Rows[s]["UploadFile"].ToString())).ToBase64Encryption();

                    //"img/Uploads/UploadMinutes/" + ds.Tables[5].Rows[s]["UploadFile"].ToString();
                    objMin.UpdatedOn = Convert.ToDateTime(ds.Tables[5].Rows[s]["UpdatedOn"].ToString());
                    objMin.CreatedOn = Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[5].Rows[s]["MeetingDate"].ToString()));//Convert.ToDateTime(ds.Tables[5].Rows[s]["CreatedOn"].ToString());
                    objMin.CreatedBy = Guid.Parse(ds.Tables[5].Rows[s]["UploadMinuteId"].ToString());
                    objMin.UpdatedBy = Guid.Parse(ds.Tables[5].Rows[s]["UploadMinuteId"].ToString());

                    objMin.MeetingDate = Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[5].Rows[s]["MeetingDate"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[5].Rows[s]["MeetingTime"].ToString())).ToString("D");
                    objMin.MeetingTime = Encryptor.DecryptString(ds.Tables[5].Rows[s]["MeetingTime"].ToString());
                    objMin.MeetingVenue = Encryptor.DecryptString(ds.Tables[5].Rows[s]["MeetingVenue"].ToString());
                    //objMin.IsActive = Convert.ToBoolean(ds.Tables[5].Rows[s]["IsActive"].ToString());

                    if (ds.Tables[5].Rows[s]["IsActive"].ToString() != null && (ds.Tables[5].Rows[s]["IsActive"].ToString() == "1" || ds.Tables[5].Rows[s]["IsActive"].ToString().ToLower().Equals("true")))
                    {
                        objMin.IsActive = true;
                    }
                    else
                    {
                        objMin.IsActive = false;
                    }

                    objMin.ForumId = Guid.Parse(ds.Tables[5].Rows[s]["ForumId"].ToString());
                    objMin.MeetingId = Guid.Parse(ds.Tables[5].Rows[s]["MeetingId"].ToString());
                    objMin.ForumName = Encryptor.DecryptString(ds.Tables[5].Rows[s]["ForumName"].ToString());
                    objMinList.Add(objMin);
                }

                //Response time used for next request
                if (ds.Tables[6].Rows.Count > 0)
                {
                    DateTime timestamp = Convert.ToDateTime(ds.Tables[6].Rows[0]["ResponseTime"].ToString());
                    ResponseTime = timestamp.ToUnixTimestamp().ToString();
                }

                //Meeting number 
                if (ds.Tables[7].Rows.Count > 0)
                {
                    objResponse.MeetingNumber = Convert.ToString(ds.Tables[7].Rows[0]["Value"]);
                }

                //Upload minutes number 
                if (ds.Tables[8].Rows.Count > 0)
                {
                    objResponse.UploadMinuteNumber = Convert.ToString(ds.Tables[8].Rows[0]["Value"]);
                }

                //Past Meeting number 
                if (ds.Tables[9].Rows.Count > 0)
                {
                    objResponse.PastMeeting = Convert.ToString(ds.Tables[9].Rows[0]["Value"]);
                }

                //Past Meeting number 
                if (ds.Tables[10].Rows.Count > 0)
                {
                    objResponse.SessionTimeOut = Convert.ToString(ds.Tables[10].Rows[0]["Value"]);
                }

                //Past Meeting number 
                if (ds.Tables[11].Rows.Count > 0)
                {
                    objResponse.OfflineDays = Convert.ToString(ds.Tables[11].Rows[0]["Value"]);
                }

                //Local Notification count
                if (ds.Tables[12].Rows.Count > 0)
                {
                    objResponse.LocalNotificationDays = Convert.ToString(ds.Tables[12].Rows[0]["Value"]);
                }
                //Proceedings count
                if (ds.Tables[13].Rows.Count > 0)
                {
                    objResponse.ProceedingsCount = Convert.ToString(ds.Tables[13].Rows[0]["Value"]);
                }

                //time stamp
                if (ds.Tables[14].Rows.Count > 0)
                {
                    if (ds.Tables[14].Rows[0]["updateTimeStamp"].ToString() != "")
                    {
                        objResponse.IsUpdatedTimeStamp = true;
                    }
                    else
                    {
                        objResponse.IsUpdatedTimeStamp = false;
                    }
                }

                //About us 
                if (ds.Tables[15].Rows.Count > 0)
                {
                    // objResponse.AboutUsImage = Convert.ToString(ds.Tables[10].Rows[0]["Image"]);
                    //      objResponse.AboutUs = ("http://" + webUrl[2] + "/" + System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + Convert.ToString(ds.Tables[10].Rows[0]["Image"])).ToBase64Encryption();
                    objResponse.AboutUs = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + Convert.ToString(ds.Tables[15].Rows[0]["Image"])).ToBase64Encryption();

                }
                List<Proceeding> objProceedingDomainList = new List<Proceeding>();
                //Proceedings
                if (ds.Tables.Count > 17)
                {
                    if (ds.Tables[17].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds.Tables[17].Rows.Count - 1; i++)
                        {
                            Proceeding objProceeding = new Proceeding();
                            objProceeding.MeetingId = Guid.Parse(ds.Tables[17].Rows[i]["MeetingId"].ToString());
                            objProceeding.ForumId = Guid.Parse(ds.Tables[17].Rows[i]["ForumId"].ToString());
                            objProceeding.ProcedingName =
                                 string.IsNullOrEmpty(ds.Tables[17].Rows[i]["ProcedingName"].ToString()) ? "" :
                             (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"] + ds.Tables[17].Rows[i]["ProcedingName"].ToString())).ToBase64Encryption();

                            //ds.Tables[16].Rows[i]["ProcedingName"].ToString();
                            objProceeding.CreatedBy = Guid.Parse(ds.Tables[17].Rows[i]["CreatedBy"].ToString());
                            objProceeding.CreatedOn = Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[17].Rows[i]["MeetingDate"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[17].Rows[i]["MeetingTime"].ToString()));//Convert.ToDateTime(ds.Tables[16].Rows[i]["CreatedOn"].ToString());
                            objProceeding.UpdatedBy = Guid.Parse(ds.Tables[17].Rows[i]["UpdatedBy"].ToString());
                            objProceeding.UpdatedOn = Convert.ToDateTime(ds.Tables[17].Rows[i]["UpdatedOn"].ToString());
                            // objProceeding.IsActive = Convert.ToBoolean(ds.Tables[16].Rows[i]["IsActive"].ToString());


                            if (ds.Tables[17].Rows[i]["IsActive"].ToString() != null && (ds.Tables[17].Rows[i]["IsActive"].ToString() == "1" || ds.Tables[17].Rows[i]["IsActive"].ToString().ToLower().Equals("true")))
                            {
                                objProceeding.IsActive = true;
                            }
                            else
                            {
                                objProceeding.IsActive = false;
                            }

                            // objProceeding.EncryptionKey = ds.Tables[16].Rows[i]["EncryptionKey"].ToString();
                            objProceeding.ForumName = Encryptor.DecryptString(ds.Tables[17].Rows[i]["ForumName"].ToString());

                            objProceeding.MeetingDate = Convert.ToDateTime(Encryptor.DecryptString(ds.Tables[17].Rows[i]["MeetingDate"].ToString())).ToString("D");
                            objProceeding.MeetingTime = Encryptor.DecryptString(ds.Tables[17].Rows[i]["MeetingTime"].ToString());
                            objProceeding.MeetingVenue = Encryptor.DecryptString(ds.Tables[17].Rows[i]["MeetingVenue"].ToString());

                            objProceedingDomainList.Add(objProceeding);
                        }
                    }
                }






                objResponse.ProceedingList = objProceedingDomainList;
                objResponse.Entity = objEntityList;
                objResponse.Forum = objForumList;
                objResponse.Meeting = objMeetingList;
                objResponse.Notice = objNoticeList;
                objResponse.Agenda = objAgendaList;
                objResponse.UploadedMinutes = objMinList;
                objResponse.ResponseTime = ResponseTime;

            }
            objResponse.RequestId = AesEncryption.EncryptString256(objRequest.UserId + "!@#$" + objRequest.RequestId);
            return objResponse;
        }


        //public ProceedingResponse Proceeding(KeyInformationRequest objRequest)
        //{
        //    string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
        //    ProceedingResponse objResponse = new ProceedingResponse();

        //    bool IsLost = IsLostDevice(objRequest.UserId);
        //    if (IsLost)
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
        //    }

        //    if (token != null && token.Length > 0)
        //    {
        //        if (!ValidateToken(token, objRequest.UserId.ToString()))
        //        {
        //            WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
        //        return null;
        //    }

        //    ProceedingService objProceedingService = new ProceedingService();
        //    List<Proceeding> objProceedingDomainList = new List<Proceeding>();



        //    System.Data.DataSet ds;
        //    //first request
        //    if (objRequest.StartTime.ToLower().Equals("0"))
        //    {
        //        ds = objProceedingService.GetAllProceedingInfoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
        //        // objPhotoGallery = objService.GetAllPhotoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
        //    }
        //    else
        //    {
        //        Int64 interval = Convert.ToInt64(objRequest.StartTime);
        //        DateTime dt = DateTime.Now.ToDateTime(interval);
        //        ds = objProceedingService.GetAllProceedingInfoWcf(dt, DateTime.Now, objRequest.EntityId);
        //        //  objPhotoGallery = objService.GetAllPhotoWcf(dt, DateTime.Now, objRequest.EntityId);
        //    }
        //    DateTime timestamp = DateTime.Now;
        //    if (ds.Tables.Count > 0)
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //            {
        //                Proceeding objProceeding = new Proceeding();
        //                objProceeding.MeetingId = Guid.Parse(ds.Tables[0].Rows[i]["MeetingId"].ToString());
        //                objProceeding.ForumId = Guid.Parse(ds.Tables[0].Rows[i]["ForumId"].ToString());
        //                objProceeding.ProcedingName =
        //                     string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ProcedingName"].ToString()) ? "" :
        //                 (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"] + ds.Tables[0].Rows[i]["ProcedingName"].ToString())).ToBase64Encryption();

        //                //ds.Tables[0].Rows[i]["ProcedingName"].ToString();
        //                objProceeding.CreatedBy = Guid.Parse(ds.Tables[0].Rows[i]["CreatedBy"].ToString());
        //                objProceeding.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString());
        //                objProceeding.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[i]["UpdatedBy"].ToString());
        //                objProceeding.UpdatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdatedOn"].ToString());
        //                objProceeding.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
        //                // objProceeding.EncryptionKey = ds.Tables[0].Rows[i]["EncryptionKey"].ToString();


        //                objProceedingDomainList.Add(objProceeding);
        //            }
        //        }

        //    }


        //    objResponse.ProceedingList = objProceedingDomainList;

        //    objResponse.ResponseTime = timestamp.ToUnixTimestamp().ToString();

        //    return objResponse;


        //}


        /// <summary>
        /// code for forget password request commented on 16 March 19
        /// </summary>
        /// <param name="objForgetPass">Class object specifying objForgetPass</param>
        /// <returns></returns>
        public ForgotPasswordResponse ForgotPasswordOld(ForgotPasswordRequest objRequest)
        {
            ForgotPasswordResponse objResponse = new ForgotPasswordResponse();
            if (objRequest.UserName.Length > 0)
            {
                System.Data.DataTable objUser = UserDataProvider.Instance.GetPasswordByUsername_Ipad(objRequest.UserName, objRequest.MobileNo);

                if (objUser.Rows.Count == 0)
                {
                    objResponse.Status = "Your Username or Mobile number does not match, please contact administrator.";
                    return objResponse;
                }

                string ToEmail = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["EmailID1"]));

                if (string.IsNullOrEmpty(ToEmail))
                {
                    objResponse.Status = "Your email address does not exist, please contact administrator.";
                    return objResponse;
                }
                string xmlPath = System.Web.Hosting.HostingEnvironment.MapPath("/img/Uploads/Email.xml");
                if (!System.IO.File.Exists(xmlPath))// System.Web.HttpContext.Current.Server.MapPath("/img/Uploads/Email.xml")));//
                {
                    //System.Web.HttpContext.Current.Server.MapPath("img/Uploads/Email.xml");
                    objResponse.Status = " Email sending failed. Configuration setting missing, please contact administrator.";
                    return objResponse;
                }

                string Name = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["FirstName"]));

                Guid g = Guid.NewGuid();
                DateTime d = DateTime.Now;
                UserDomain objUserData = new UserDomain();
                objUserData.UserId = Guid.Parse(objUser.Rows[0]["UserId"].ToString());
                objUserData.EmailID1 = Encryptor.DecryptString(Convert.ToString(objUser.Rows[0]["EmailID1"]));
                objUserData.ActivationCode = Guid.NewGuid();
                UserDataProvider.Instance.InsertPasswordResetLink(objUserData);

                string url = Convert.ToString(ConfigurationManager.AppSettings["Current_Url"]) + "/ResetPassword.aspx?ActivationToken=" + objUserData.ActivationCode.ToString();


                MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                SmtpClient SMTPServer = new SmtpClient();

                string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                string Password = "";// = ConfigurationManager.AppSettings["EPassword"]
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlPath);//(AppDomain.CurrentDomain.BaseDirectory+ "/img/Uploads/Email.xml");
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
                MailAddress fromAddress = new MailAddress(EmailFrom, "eMeeting Admin");
                objmail.Headers.Add("NAME", "Admin");
                objmail.From = fromAddress;
                objmail.To.Add(ToEmail);
                objmail.Body = EmailBody;
                objmail.Subject = EmailSubject;

                objmail.IsBodyHtml = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                SMTPServer.Send(objmail);
                objmail.To.Clear();
                objResponse.Status = "Password reset email has been sent to your account";
                return objResponse;
            }
            else
            {
                objResponse.Status = "Please enter username";
            }
            return objResponse;
        }

        /// <summary>
        /// forgot password on mobile number
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest objRequest)
        {
            ForgotPasswordResponse objResponse = new ForgotPasswordResponse();
            if (objRequest.UserName.Length > 0)
            {

                System.Data.DataTable objUser = UserDataProvider.Instance.GetPasswordByUsernameAndMobile(objRequest.UserName, objRequest.MobileNo);
                if (objUser.Rows.Count > 0)
                {
                    UserDomain objUserDomain = new UserDomain();
                    objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["Moblie"]));
                    objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["FirstName"]));
                    objUserDomain.Password = Encryptor.DecryptString(Null.SetNullString(objUser.Rows[0]["Password"]));

                    if (objUserDomain.Mobile == null || objUserDomain.Mobile.Length == 0)
                    {
                        objResponse.Status = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                        return objResponse;
                    }

                    if (!(objUserDomain.Mobile.Length > 9))
                    {
                        objResponse.Status = "Invalid Mobile Number, Please Contact Administrator";
                        return objResponse;
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

                        string response = string.Empty;
                        response = UserDataProvider.PostSMS(url, data, authInfo);

                        status = true;
                        //status = SendDomesticPassword(objUserDomain.FirstName, objUserDomain.Mobile, objUserDomain.Password);
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

                        string response = string.Empty;
                        response = UserDataProvider.PostSMS(url, data, authInfo);

                        status = true;

                        //status = SendInternationalPassword(objUserDomain.FirstName, objUserDomain.Mobile, objUserDomain.Password);
                    }

                    if (status)
                    {
                        objResponse.Status = "Your password has been sent to mobile number ending with ******" + objUserDomain.Mobile.Substring(objUserDomain.Mobile.Length - 4);
                        return objResponse;
                    }
                    else
                    {

                        objResponse.Status = "Something went wrong. please try again after sometime.";
                        return objResponse;
                    }
                }
                else
                {
                    objResponse.Status = "Invalid username and mobile no.";
                    return objResponse;
                }
            }
            else
            {
                objResponse.Status = "Please enter username and mobile no.";
            }
            return objResponse;
        }

        /// <summary>
        /// Get user passowrd 
        /// </summary>
        /// <param name="objRequest">Class object specifying objRequest</param>
        /// <returns></returns>
        public ForgotPasswordResponse_old ForgotPassword_old(ForgotPasswordRequest objRequest)
        {
            ForgotPasswordResponse_old objResponse = new ForgotPasswordResponse_old();
            UserServices objUser = new UserServices();
            SecurityQuestionService objQuestionService = new SecurityQuestionService();
            System.Data.DataTable dtUserDetails = objUser.GetPasswordByUserName(objRequest.UserName);
            if (dtUserDetails.Rows.Count > 0)
            {
                objResponse.SecurityQuestionId = Guid.Parse(dtUserDetails.Rows[0]["SecurityQuestionId"].ToString());

                objResponse.Answer = Encryptor.DecryptString(dtUserDetails.Rows[0]["Answer"].ToString());
                objResponse.Password = Encryptor.DecryptString(dtUserDetails.Rows[0]["Password"].ToString());
                IList<SecurityQuestionDomain> objSecurity = objQuestionService.GetAllQuestion();
                var strQuest = (from p in objSecurity
                                where p.SecurityQuestionId == objResponse.SecurityQuestionId
                                select p.SecurityQuestion).ToList();
                if (strQuest.Count() > 0)
                {
                    objResponse.SecurityQuestion = strQuest[0];
                }
            }
            return objResponse;
        }

        /// <summary>
        /// Get Board of directore list
        /// </summary>
        /// <returns></returns>
        public KeyInformationResponse KeyInformation(KeyInformationRequest objRequest)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
            KeyInformationResponse objResponse = new KeyInformationResponse();
            bool IsLost = IsLostDevice(objRequest.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }

            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }

            KeyInformationServices objKeyInfoService = new KeyInformationServices();
            List<BoardOfDirectors> objDirector = new List<BoardOfDirectors>();

            PhotoGalleryService objService = new PhotoGalleryService();
            List<PhotoGallery> objPhoto = new List<PhotoGallery>();

            List<Tabs> objGlobalTabList = new List<Tabs>();


            System.Data.DataSet ds;
            //first request
            if (objRequest.StartTime.ToLower().Equals("0"))
            {
                ds = objKeyInfoService.GetAllKeyInfoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
                // objPhotoGallery = objService.GetAllPhotoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
            }
            else
            {
                Int64 interval = Convert.ToInt64(objRequest.StartTime);
                DateTime dt = DateTime.Now.ToDateTime(interval);
                ds = objKeyInfoService.GetAllKeyInfoWcf(dt, DateTime.Now, objRequest.EntityId);
                //  objPhotoGallery = objService.GetAllPhotoWcf(dt, DateTime.Now, objRequest.EntityId);
            }
            DateTime timestamp = DateTime.Now;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        BoardOfDirectors objBOD = new BoardOfDirectors();
                        objBOD.Description = ds.Tables[0].Rows[i]["Description"].ToString();
                        objBOD.Designation = ds.Tables[0].Rows[i]["Designation"].ToString();
                        objBOD.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        objBOD.DirectorId = Guid.Parse(ds.Tables[0].Rows[i]["KeyInformationId"].ToString());
                        objBOD.CreatedBy = Guid.Parse(ds.Tables[0].Rows[i]["CreatedBy"].ToString());
                        objBOD.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString());
                        objBOD.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[i]["UpdatedBy"].ToString());
                        objBOD.UpdatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdateOn"].ToString());
                        objBOD.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        //objBOD.Photograph = string.IsNullOrEmpty(keyInfo.Photograph) ? "" :
                        //          ("http://" + webUrl[2] + "/" + (System.Configuration.ConfigurationManager.AppSettings["KeyInfo"] +keyInfo.Photograph)).ToBase64Encryption();

                        objBOD.Photograph = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Photograph"].ToString()) ? "" :
                          (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["KeyInfo"] + ds.Tables[0].Rows[i]["Photograph"].ToString())).ToBase64Encryption();
                        objBOD.DirectorOrder = Convert.ToInt32(Null.SetNullInteger(ds.Tables[0].Rows[i]["KeyInformationOrder"]));

                        objDirector.Add(objBOD);
                    }
                }


                //string url = System.ServiceModel.OperationContext.Current.RequestContext.RequestMessage.Headers.To.ToString();
                //string[] webUrl = url.Split('/');
                //foreach (KeyInformationDomain keyInfo in objKeyInfoDomain)
                //{

                //}

                for (int j = 0; j <= ds.Tables[1].Rows.Count - 1; j++)
                {
                    PhotoGallery objPhotoGal = new PhotoGallery();
                    objPhotoGal.PhotoGalleryId = Guid.Parse(ds.Tables[1].Rows[j]["PhotoGalleryId"].ToString());
                    //objPhotoGal.Photograph = string.IsNullOrEmpty(objPhotoDomain.Photograph) ? "" :
                    //          ("http://" + webUrl[2] + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + objPhotoDomain.Photograph)).ToBase64Encryption();

                    objPhotoGal.Photograph = string.IsNullOrEmpty(ds.Tables[1].Rows[j]["Photograph"].ToString()) ? "" :
                             (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + ds.Tables[1].Rows[j]["Photograph"].ToString())).ToBase64Encryption();
                    objPhotoGal.CreatedBy = Guid.Parse(ds.Tables[1].Rows[j]["CreatedBy"].ToString());
                    objPhotoGal.UpdatedBy = Guid.Parse(ds.Tables[1].Rows[j]["UpdatedBy"].ToString());
                    objPhotoGal.CreatedOn = Convert.ToDateTime(ds.Tables[1].Rows[j]["CreatedOn"].ToString());
                    objPhotoGal.UpdatedOn = Convert.ToDateTime(ds.Tables[1].Rows[j]["UpdateOn"].ToString());
                    objPhotoGal.Description = ds.Tables[1].Rows[j]["Description"].ToString();
                    objPhotoGal.IsActive = Convert.ToBoolean(ds.Tables[1].Rows[j]["IsActive"].ToString());
                    objPhoto.Add(objPhotoGal);
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds.Tables[2].Rows.Count - 1; i++)
                    {
                        Tabs objGlobalTab = new Tabs();
                        objGlobalTab.GlobalTabId = Convert.ToInt16(ds.Tables[2].Rows[i]["GlobalTabId"].ToString());
                        objGlobalTab.Title = ds.Tables[2].Rows[i]["Title"].ToString().Replace("'", "&#39;");
                        //  objGlobalTab.UploadedPDF = ds.Tables[2].Rows[i]["UploadedPDF"].ToString();

                        string UploadedPDF = ds.Tables[2].Rows[i]["UploadedPDF"].ToString();
                        if (!string.IsNullOrEmpty(UploadedPDF))
                        {
                            string[] arrUploadedPDFs = UploadedPDF.Replace(" ", "").Split(',');

                            objGlobalTab.UploadedPDF = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" +
                                (System.Configuration.ConfigurationManager.AppSettings["KeyInfo"] + (arrUploadedPDFs[0].Trim().Substring(0, arrUploadedPDFs[0].Trim().Length - 4) + "_merge.pdf"))).ToBase64Encryption();
                        }
                        objGlobalTab.CreatedBy = Guid.Parse(ds.Tables[2].Rows[i]["CreatedBy"].ToString());
                        objGlobalTab.CreatedOn = Convert.ToDateTime(ds.Tables[2].Rows[i]["CreatedOn"].ToString());
                        objGlobalTab.UpdatedBy = Guid.Parse(ds.Tables[2].Rows[i]["UpdatedBy"].ToString());
                        objGlobalTab.UpdatedOn = Convert.ToDateTime(ds.Tables[2].Rows[i]["UpdatedOn"].ToString());
                        objGlobalTab.IsActive = Convert.ToBoolean(ds.Tables[2].Rows[i]["IsActive"].ToString());


                        objGlobalTabList.Add(objGlobalTab);
                    }
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    timestamp = Convert.ToDateTime(ds.Tables[3].Rows[0]["ResponseTime"].ToString());
                }
            }
            //foreach (PhotoGalleryDomain objPhotoDomain in objPhotoGallery)
            //{
            //    PhotoGallery objPhotoGal = new PhotoGallery();
            //    objPhotoGal.PhotoGalleryId = objPhotoDomain.PhotoGalleryId;
            //    //objPhotoGal.Photograph = string.IsNullOrEmpty(objPhotoDomain.Photograph) ? "" :
            //    //          ("http://" + webUrl[2] + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + objPhotoDomain.Photograph)).ToBase64Encryption();

            //    objPhotoGal.Photograph = string.IsNullOrEmpty(objPhotoDomain.Photograph) ? "" :
            //             (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + objPhotoDomain.Photograph)).ToBase64Encryption();
            //    objPhotoGal.CreatedBy = objPhotoDomain.CreatedBy;
            //    objPhotoGal.UpdatedBy = objPhotoDomain.UpdatedBy;
            //    objPhotoGal.CreatedOn = objPhotoDomain.CreatedOn;
            //    objPhotoGal.UpdatedOn = objPhotoDomain.UpdatedOn;
            //    objPhotoGal.Description = objPhotoDomain.Description;
            //    objPhotoGal.IsActive = objPhotoDomain.IsActive;
            //    objPhoto.Add(objPhotoGal);
            //}
            objResponse.GlobalTabList = objGlobalTabList;
            objResponse.PhotoGallery = objPhoto;
            objResponse.BoardOfDirector = objDirector;



            objResponse.ResponseTime = timestamp.ToUnixTimestamp().ToString();

            return objResponse;
        }

        /// <summary>
        /// Get photo Gallery details
        /// </summary>
        /// <param name="objRequest">Class object specifying objRequest</param>
        /// <returns></returns>
        public PhotoGalleryResponse PhotoGallery(PhotoGalleryRequest objRequest)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];

            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }
            PhotoGalleryResponse objResponse = new PhotoGalleryResponse();
            PhotoGalleryService objService = new PhotoGalleryService();
            List<PhotoGallery> objPhoto = new List<PhotoGallery>();
            IList<PhotoGalleryDomain> objPhotoGallery = null;

            //first request
            if (objRequest.StartTime.ToLower().Equals("0"))
            {
                objPhotoGallery = objService.GetAllPhotoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
            }
            else
            {
                Int64 interval = Convert.ToInt64(objRequest.StartTime);
                DateTime dt = DateTime.Now.ToDateTime(interval);
                objPhotoGallery = objService.GetAllPhotoWcf(dt, DateTime.Now, objRequest.EntityId);
            }

            //string url = System.ServiceModel.OperationContext.Current.RequestContext.RequestMessage.Headers.To.ToString();
            //string[] webUrl = url.Split('/');

            foreach (PhotoGalleryDomain objPhotoDomain in objPhotoGallery)
            {
                PhotoGallery objPhotoGal = new PhotoGallery();
                objPhotoGal.PhotoGalleryId = objPhotoDomain.PhotoGalleryId;
                //objPhotoGal.Photograph = string.IsNullOrEmpty(objPhotoDomain.Photograph) ? "" :
                //          ("http://" + webUrl[2] + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + objPhotoDomain.Photograph)).ToBase64Encryption();

                objPhotoGal.Photograph = string.IsNullOrEmpty(objPhotoDomain.Photograph) ? "" :
                        (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + (System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"] + objPhotoDomain.Photograph)).ToBase64Encryption();
                objPhotoGal.CreatedBy = objPhotoDomain.CreatedBy;
                objPhotoGal.UpdatedBy = objPhotoDomain.UpdatedBy;
                objPhotoGal.CreatedOn = objPhotoDomain.CreatedOn;
                objPhotoGal.UpdatedOn = objPhotoDomain.UpdatedOn;
                objPhotoGal.Description = objPhotoDomain.Description;
                objPhotoGal.IsActive = objPhotoDomain.IsActive;
                objPhoto.Add(objPhotoGal);
            }
            objResponse.PhotoGallery = objPhoto;
            DateTime timestamp = DateTime.Now;
            objResponse.ResponseTime = timestamp.ToUnixTimestamp().ToString();

            return objResponse;
        }

        /// <summary>
        /// Draft Mom request
        /// </summary>
        /// <param name="objRequest">Class object specifying objRequest </param>
        /// <returns></returns>
        public DraftMOMResponse DraftMOM(DraftMOMRequest objRequest)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];

            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }
            DraftMOMResponse objDraftMOM = new DraftMOMResponse();
            DraftMOMService objService = new DraftMOMService();

            DateTime startTime = Convert.ToDateTime("09/09/1929 12:00:00 AM");
            if (Convert.ToInt32(objRequest.StartTime) > 1)
            {
                Int64 interval = Convert.ToInt64(objRequest.StartTime);
                DateTime dt = DateTime.Now.ToDateTime(interval);
                startTime = dt;
            }

            System.Data.DataSet ds = objService.GetDraftMOMWcf(objRequest.UserId, startTime);

            if (ds.Tables.Count > 0)
            {
                List<DraftMOMDetails> objDraftList = new List<DraftMOMDetails>();
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    DraftMOMDetails objMOM = new DraftMOMDetails();
                    objMOM.CreatedBy = Guid.Parse(ds.Tables[0].Rows[i]["CreatedBy"].ToString());
                    objMOM.CreatedByName = Encryptor.DecryptString(ds.Tables[0].Rows[i]["CreatorFName"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[0].Rows[i]["CreatorLName"].ToString());
                    objMOM.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString());
                    objMOM.CreatedOnString = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString()).ToString("f");
                    objMOM.DraftMinutesId = Guid.Parse(ds.Tables[0].Rows[i]["DraftMinutesId"].ToString());
                    objMOM.DraftMOMName = ds.Tables[0].Rows[i]["DraftMOMName"].ToString();

                    objMOM.DraftMinute = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["DraftMinute"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/" + System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"] + ds.Tables[0].Rows[i]["DraftMinute"].ToString()).ToBase64Encryption();
                    objMOM.UpdateOnOnString = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdatedOn"].ToString()).ToString("f");
                    objMOM.UpdateOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdatedOn"].ToString());
                    objMOM.UpdatedByName = Encryptor.DecryptString(ds.Tables[0].Rows[i]["UpdateFName"].ToString()) + " " + Encryptor.DecryptString(ds.Tables[0].Rows[i]["UpdateLName"].ToString());
                    objMOM.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[i]["UpdateBy"].ToString());
                    objMOM.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                    objDraftList.Add(objMOM);
                }
                objDraftMOM.DraftMOM = objDraftList;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                DateTime timestamp = Convert.ToDateTime(ds.Tables[1].Rows[0]["ResponseTime"].ToString());
                objDraftMOM.ResponseTime = timestamp.ToUnixTimestamp().ToString();
            }
            return objDraftMOM;
        }

        /// <summary>
        /// Get User delete status by Userid
        /// </summary>
        /// <param name="objRequest">Class object specifying objRequest</param>
        /// <returns></returns>
        public UserStatusResponse UserStatus(UserStatusRequest objRequest)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
            UserStatusResponse objResponse = new UserStatusResponse();
            bool IsLost = IsLostDevice(objRequest.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }
            if (token != null && token.Length > 0)
            {
                if (!ValidateToken(token, objRequest.UserId.ToString()))
                {
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                    return null;
                }
            }
            else
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
                return null;
            }

            UserEntityServices objEntityService = new UserEntityServices();
            System.Data.DataSet dsUserDetails = objEntityService.GetUserStatusById(objRequest.UserId);
            if (dsUserDetails != null && dsUserDetails.Tables.Count > 0 && dsUserDetails.Tables[0].Rows.Count > 0)
            {
                bool IsActive = Convert.ToBoolean(dsUserDetails.Tables[0].Rows[0]["IsActive"].ToString());
                bool IsEnableOnIpad = Convert.ToBoolean(dsUserDetails.Tables[0].Rows[0]["EnabledOnIpad"].ToString());

                if (!IsActive || !IsEnableOnIpad)
                {
                    objResponse.IsActive = false;
                }
                else
                {
                    objResponse.IsActive = true;
                }
            }

            return objResponse;
        }

        /// <summary>
        /// Is user lost device
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool IsLostDevice(Guid UserId)
        {
            UserServices objUser = new UserServices();
            int i = objUser.IsUserLostDevice(UserId);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Log out service
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public ChangePasswordResponse LogOut(ChangePasswordRequest objRequest)
        {
            string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
            UserManager objUserman = objUserManager.Find(p => p.AcceeToken == token && p.UserId == objRequest.UserId.ToString());

            bool IsLost = IsLostDevice(objRequest.UserId);
            if (IsLost)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
            }
            // Use reflection to get the Elapsed event.
            //System.Reflection.EventInfo[] eInfo = objUserman.timer.GetType().GetEvents();
            //foreach (System.Reflection.EventInfo ob in eInfo)
            //{
            //    Type handlerType = ob.EventHandlerType;
            //    System.Reflection.MethodInfo invokeMethod = handlerType.GetMethod("Invoke");
            //    System.Reflection.ParameterInfo[] parms = invokeMethod.GetParameters();
            //    Type[] parmTypes = new Type[parms.Length];
            //    for (int i = 0; i < parms.Length; i++)
            //    {
            //        parmTypes[i] = parms[i].ParameterType;
            //    }
            //}

            ChangePasswordResponse objChangePasswordResp = new ChangePasswordResponse();
            if (objUserman != null)
            {
                //ExpireItems objExpire = new ExpireItems();
                objUserman.timer.Stop();

                //.Elapsed -= new System.Timers.ElapsedEventHandler(objExpire.Elapsed_Event);
                //objExpire.AcceeToken = token;

                objUserManager.Remove(objUserman);
                objChangePasswordResp.Status = "Success";
            }
            else
            {
                objChangePasswordResp.Status = "failed";
            }
            //objChangePasswordResp.LostDevice = IsLostDevice(objRequest.UserId);
            return objChangePasswordResp;
        }

        /// <summary>
        /// Clean data request
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public ChangePasswordResponse DataClean(DataCleanRequest objRequest)
        {
            ChangePasswordResponse objChangePasswordResp = new ChangePasswordResponse();
            UserEntityServices objService = new UserEntityServices();
            bool status = objService.DataClean(objRequest.DeviceToken);
            if (status)
            {
                objChangePasswordResp.Status = "Success";
            }
            else
            {
                objChangePasswordResp.Status = "failed";
            }
            return objChangePasswordResp;
        }

        public string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            time = time.Concat(key).ToArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        public bool ValidateToken(string token, string UserId)
        {
            #region "comment on 10 Oct 2015 to remove token validation"
            //// LogError objEr = new LogError();
            //// objEr.WriteToFile("  validation method " + token);
            //UserManager objUserman = objUserManager.Find(p => p.AcceeToken == token && p.UserId == UserId);
            ////  objEr.WriteToFile("  objUserman ");
            ////      byte[] data = Convert.FromBase64String(token);
            ////  objEr.WriteToFile("  base 64 data ");
            ////   DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            ////   objEr.WriteToFile("  base 64 comp ");
            //if (objUserman == null)//when < DateTime.UtcNow.AddHours(-1) || 
            //{
            //    //    objEr.WriteToFile(" not  objUserman found");
            //    return false;
            //}
            //else
            //{
            //    //objEr.WriteToFile("  objUserman found");
            //    objUserman.timer.Stop();
            //    // objEr.WriteToFile("  timer stop");
            //    objUserman.timer.Start();
            //    // objEr.WriteToFile("  start");
            //    return true;
            //    //objUserManager.Remove(objUserman);
            //    //objUserManager.Add(objUserman);
            //    // UserManager objusermans = new UserManager(objUserman.AcceeToken, objUserman.SessionTimeOut, objUserManager);
            //}
            #endregion

            return true;

        }

        private static bool ValidateRemoteCertificate(
    object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors policyErrors
    )
        {
            return true;
        }
        public VerifyOTPResponse VerifyOTP(VerifyOTPRequest objRequest)
        {

            try
            {
                VerifyOTPResponse objResponse = new VerifyOTPResponse();
                objResponse.IsActive = AesEncryption.EncryptString256(objRequest.RequestId + "!@#$false");// Convert.ToBoolean(OTPSend)
                objResponse.RequestId = AesEncryption.EncryptString256(objRequest.UserId + "!@#$" + objRequest.RequestId);

                UserServices objUser = new UserServices();
                int OTPTimeOut = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["OTPTimeOut"]);
                bool IsValid = objUser.VerifyOTP(objRequest.UserId, OTPTimeOut, objRequest.OTPValue, objRequest.OTPKey);

                objResponse.IsActive = AesEncryption.EncryptString256(objRequest.RequestId + "!@#$" + Convert.ToString(IsValid).ToLower());

                return objResponse;
            }
            catch (Exception e)
            {
                VerifyOTPResponse objResponse = new VerifyOTPResponse();

                objResponse.OTPError += "Otp Error";//e.Message;
                objResponse.RequestId = AesEncryption.EncryptString256(objRequest.UserId + "!@#$" + objRequest.RequestId);

                return objResponse;
            }


        }

        public AgendaAckResponse_New AgendaAcknowledgement(AgendaAckRequest objRequest)
        {
            AgendaAckResponse_New objResponse = new AgendaAckResponse_New();
            List<AgendaAckResponse> objAck = new List<AgendaAckResponse>();
            MM.Services.AgendaServices objService = new AgendaServices();
            DateTime? AgendaReadOn = null;
            DateTime? NoticeReadOn = null;

            foreach (Ack acks in objRequest.AgendaAck)
            {
                AgendaAckResponse objAgendaAckResponse = new AgendaAckResponse();
                if (acks.AgendaReadOn != null)
                {
                    AgendaReadOn = Convert.ToDateTime(acks.AgendaReadOn);
                }

                if (acks.NoticeReadOn != null)
                {
                    NoticeReadOn = Convert.ToDateTime(acks.NoticeReadOn);
                }

                string strAccessToken;
                if (WebOperationContext.Current.IncomingRequest.Headers["AccessToken"] != null)
                {
                    strAccessToken = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"].ToString();
                }
                else
                {
                    objAgendaAckResponse.Status = "failed";
                    return objResponse;
                }

                Int32 status = objService.InsertAgendaAck(acks.MeetingId, acks.UserId, AgendaReadOn ?? Convert.ToDateTime("01/01/1978"), NoticeReadOn ?? Convert.ToDateTime("01/01/1978"), acks.IsNoticeRead, acks.IsAgendaRead, acks.PublishVersion, strAccessToken);

                if (status > 0)
                {
                    objAgendaAckResponse.Status = "Success";
                }
                else
                {
                    objAgendaAckResponse.Status = "failed";
                }

                objAgendaAckResponse.MeetingId = acks.MeetingId;
                objAgendaAckResponse.UserId = acks.UserId;
                objAgendaAckResponse.AgendaReadOn = acks.AgendaReadOn;
                objAgendaAckResponse.NoticeReadOn = acks.NoticeReadOn;
                objAgendaAckResponse.IsAgendaRead = acks.IsAgendaRead;
                objAgendaAckResponse.IsNoticeRead = acks.IsNoticeRead;

                objAck.Add(objAgendaAckResponse);
            }
            //if (objRequest.AgendaReadOn != null)
            //{
            //    AgendaReadOn = Convert.ToDateTime(objRequest.AgendaReadOn);
            //}

            //if (objRequest.NoticeReadOn != null)
            //{
            //    NoticeReadOn = Convert.ToDateTime(objRequest.NoticeReadOn);
            //}

            //Int32 status = objService.InsertAgendaAck(objRequest.MeetingId, objRequest.UserId, AgendaReadOn ?? null, NoticeReadOn ?? null, objRequest.IsAgendaRead, objRequest.IsNoticeRead);
            //if (status > 0)
            //{
            //    objAgendaAckResponse.Status = "Success";
            //}
            //else
            //{
            //    objAgendaAckResponse.Status = "failed";
            //}
            //return objAgendaAckResponse;

            objResponse.Response = objAck;
            return objResponse;
        }
        //public GlobalTabResponse Tabs(KeyInformationRequest objRequest)
        //{
        //    string token = WebOperationContext.Current.IncomingRequest.Headers["AccessToken"];
        //    GlobalTabResponse objResponse = new GlobalTabResponse();

        //    bool IsLost = IsLostDevice(objRequest.UserId);
        //    if (IsLost)
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("LostDevice", "true");
        //    }

        //    if (token != null && token.Length > 0)
        //    {
        //        if (!ValidateToken(token, objRequest.UserId.ToString()))
        //        {
        //            WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        WebOperationContext.Current.OutgoingResponse.Headers.Add("TokenExpired", "true");
        //        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Forbidden;
        //        return null;
        //    }

        //    GlobalTabService objGlobalTabService = new GlobalTabService();
        //    List<GlobalTabDomain> objGlobalTabList = new List<GlobalTabDomain>();



        //    System.Data.DataSet ds;
        //    //first request
        //    if (objRequest.StartTime.ToLower().Equals("0"))
        //    {
        //        ds = objGlobalTabService.GetAllGlobalTabInfoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
        //        // objPhotoGallery = objService.GetAllPhotoWcf(Convert.ToDateTime("11/30/1800 12:00:00 AM"), DateTime.Now, objRequest.EntityId);
        //    }
        //    else
        //    {
        //        Int64 interval = Convert.ToInt64(objRequest.StartTime);
        //        DateTime dt = DateTime.Now.ToDateTime(interval);
        //        ds = objGlobalTabService.GetAllGlobalTabInfoWcf(dt, DateTime.Now, objRequest.EntityId);
        //        //  objPhotoGallery = objService.GetAllPhotoWcf(dt, DateTime.Now, objRequest.EntityId);
        //    }
        //    DateTime timestamp = DateTime.Now;
        //    if (ds.Tables.Count > 0)
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //            {
        //                GlobalTabDomain objGlobalTab = new GlobalTabDomain();
        //                objGlobalTab.Title = ds.Tables[0].Rows[i]["Title"].ToString();
        //                //  objGlobalTab.UploadedPDF = ds.Tables[0].Rows[i]["UploadedPDF"].ToString();

        //                string UploadedPDF = ds.Tables[0].Rows[i]["UploadedPDF"].ToString();
        //                if (!string.IsNullOrEmpty(UploadedPDF))
        //                {
        //                    string[] arrUploadedPDFs = UploadedPDF.Replace(" ", "").Split(',');

        //                    objGlobalTab.UploadedPDF = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) +
        //                        (System.Configuration.ConfigurationManager.AppSettings["KeyInfo"] + (arrUploadedPDFs[0].Trim().Substring(0, arrUploadedPDFs[0].Trim().Length - 4) + "_merge.pdf"))).ToBase64Encryption();
        //                }
        //                objGlobalTab.CreatedBy = Guid.Parse(ds.Tables[0].Rows[i]["CreatedBy"].ToString());
        //                objGlobalTab.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedOn"].ToString());
        //                objGlobalTab.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[i]["UpdatedBy"].ToString());
        //                objGlobalTab.UpdatedOn = Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdatedOn"].ToString());
        //                objGlobalTab.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());


        //                objGlobalTabList.Add(objGlobalTab);
        //            }
        //        }

        //    }


        //    objResponse.GlobalTabList = objGlobalTabList;




        //    objResponse.ResponseTime = timestamp.ToUnixTimestamp().ToString();

        //    return objResponse;


        //}

        public AgendaMarkerResponse AgendaMarked(AgendaMarkerRequest objRequest)
        {
            AgendaMarkerResponse objResponse = new AgendaMarkerResponse();
            List<string> Agenda = new List<string>();
            MM.Services.AgendaServices objService = new AgendaServices();
            try
            {

                System.Data.DataSet ds = objService.GetAgendaReadUnread(objRequest.MeetingId, objRequest.UserId);
                int totalCount = 0;
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objResponse.Status = "1";
                    objResponse.Error = "";
                    objResponse.MeetingId = objRequest.MeetingId;
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    objResponse.Total = Convert.ToString(ds.Tables[0].Rows[0]["TotalCount"]);

                    //}
                    //else
                    //{
                    //    objResponse.Total = "0";
                    //}

                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    //    objResponse.Read = Convert.ToString(ds.Tables[1].Rows[0]["ReadCount"]);
                    //}
                    //else
                    //{
                    //    objResponse.Read = "0";
                    //}

                    objResponse.Read = ds.Tables[0].Rows[0]["ReadCount"].ToString();

                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        for (int j = 0; j <= ds.Tables[2].Rows.Count - 1; j++)
                        {
                            Agenda.Add(ds.Tables[2].Rows[j]["AgendaId"].ToString());

                        }
                    }

                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        for (int j = 0; j <= ds.Tables[1].Rows.Count - 1; j++)
                        {
                            string Uploaded = ds.Tables[1].Rows[j]["UploadedAgendaNote"].ToString();
                            string Deleted = ds.Tables[1].Rows[j]["DeletedPdfs"].ToString();

                            string[] arrAgendaFiles = Uploaded.Replace(" ", "").Split(',');


                            string[] arrDeletedpdf = Deleted.Replace(" ", "").Split(',');

                            //foreach (string str in arrAgendaFiles)

                            //    if (!arrDeletedpdf.Contains(str))
                            //    {
                            //        totalCount++;
                            //    }
                            string[] agendaNames = Uploaded.Split(',');

                            if (!string.IsNullOrEmpty(Uploaded))
                            {
                                bool status = false;
                                for (int i = 0; i <= agendaNames.Count() - 1; i++)
                                {

                                    if (!arrDeletedpdf.Contains(agendaNames[i].Trim()) && !status)
                                    {
                                        status = true;
                                        totalCount++;
                                    }
                                }
                            }


                        }

                        objResponse.Total = totalCount.ToString();//ds.Tables[1].Rows[0]["TotalCount"].ToString();
                                                                  // objResponse.Total = ds.Tables[1].Rows[0]["TotalCount"].ToString();
                    }
                    else
                    {
                        objResponse.Total = "0";
                    }

                }
                else
                {
                    objResponse.Total = "0";
                    objResponse.Read = "0";
                }
            }
            catch (Exception ex)
            {
                objResponse.Status = "0";
                objResponse.Error = ex.Message;
            }
            objResponse.Agenda = Agenda;
            return objResponse;
        }

        public CheckUpdateResponse CheckUpdate(CheckUpdateRequest objRequest)
        {
            CheckUpdateResponse objResponse = new CheckUpdateResponse();
            MM.Services.AgendaServices objService = new AgendaServices();
            IList<IPADomain> objIpa = objService.GetIPADomainByEntity(objRequest.EntityId);
            if (objIpa.Count() > 0)
            {
                objResponse.Link = objIpa[0].IPA;
                objResponse.Version = objIpa[0].IPAName;
            }
            else
            {
                objResponse.Link = "";
                objResponse.Version = "";
            }
            return objResponse;
        }


        public AttendanceResponse Attendance(AttendanceRequest objRequest)
        {
            AttendanceResponse objResposne = new AttendanceResponse();
            MM.Services.AttendanceService objService = new AttendanceService();
            MM.Domain.AttendanceDomain objAttendance = objService.InsertOrUpdateAttendance(objRequest.UserId, objRequest.MeetingId, objRequest.Reason, objRequest.Attending);
            if (objAttendance.AttendanceId != Guid.Empty)
            {
                objResposne.Status = "1";
            }
            else
            {
                objResposne.Status = "0";
            }
            return objResposne;
        }
        public AgendaCounterResponse AgendaCounter(AgendaCounterServiceRequest objRequest)
        {
            AgendaCounterResponse objResponse = new AgendaCounterResponse();
            MM.Services.AgendaServices objService = new AgendaServices();
            System.Data.DataSet ds = objService.GetAgendaCounter(objRequest.AgendaId, objRequest.UserId, objRequest.IsRead);
            int totalCount = 0;
            List<string> Agenda = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                objResponse.Read = ds.Tables[0].Rows[0]["ReadCount"].ToString();

                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    for (int j = 0; j <= ds.Tables[2].Rows.Count - 1; j++)
                    {
                        Agenda.Add(ds.Tables[2].Rows[j]["AgendaId"].ToString());

                    }
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    for (int j = 0; j <= ds.Tables[1].Rows.Count - 1; j++)
                    {
                        string Uploaded = ds.Tables[1].Rows[j]["UploadedAgendaNote"].ToString();
                        string Deleted = ds.Tables[1].Rows[j]["DeletedPdfs"].ToString();

                        string[] arrAgendaFiles = Uploaded.Replace(" ", "").Split(',');


                        string[] arrDeletedpdf = Deleted.Replace(" ", "").Split(',');
                        //bool status = false;
                        //foreach (string str in arrAgendaFiles)

                        //    if (!arrDeletedpdf.Contains(str) && !status)
                        //    {
                        //        totalCount++;
                        //        status = true;
                        //    }


                        //string[] UploadedArr = Uploaded.Split(',');

                        string[] agendaNames = Uploaded.Split(',');

                        if (!string.IsNullOrEmpty(Uploaded))
                        {
                            bool status = false;
                            for (int i = 0; i <= agendaNames.Count() - 1; i++)
                            {

                                if (!arrDeletedpdf.Contains(agendaNames[i].Trim()) && !status)
                                {
                                    status = true;
                                    totalCount++;
                                }
                            }
                        }
                    }

                    objResponse.Total = totalCount.ToString();//ds.Tables[1].Rows[0]["TotalCount"].ToString();
                    objResponse.Status = "1";
                }
                else
                {
                    objResponse.Total = "0";
                    objResponse.Status = "0";
                }
            }
            objResponse.Agenda = Agenda;
            return objResponse;
        }

        public UserLockResponse UserLock(UserLockRequest objRequest)
        {
            UserLockResponse objResponse = new UserLockResponse();
            UserServices objUserServices = new UserServices();

            if (objRequest.UserName != null)
            {
                bool res = objUserServices.iPadUserLock(objRequest.UserName);
                if (res)
                {
                    objResponse.Status = "Success";
                }
                else
                {
                    objResponse.Status = "Failed";
                }
            }
            else
            {
                objResponse.Status = "Failed";
            }

            return objResponse;
        }

        public string GetTitles(string SerialAlhapa, string SerialTitle, string SerialNumber)
        {
            string Serial = "";
            //if (SerialAlhapa.Trim().Length > 0 && SerialTitle.Trim().Length > 0 && SerialNumber.Trim().Length > 0)
            //{
            //    return SerialAlhapa + "-" + SerialTitle + "-" + SerialNumber;
            //}
            //else if (SerialAlhapa.Trim().Length == 0 && SerialTitle.Trim().Length == 0 && SerialNumber.Trim().Length == 0)
            //{
            //    return "";
            //}

            if (SerialAlhapa.Trim().Length != 0)
            {
                Serial = SerialAlhapa;
            }

            if (SerialTitle.Trim().Length != 0)
            {
                if (Serial.Trim().Length == 0)
                {
                    Serial = SerialTitle;
                }
                else
                {
                    Serial = Serial + "-" + SerialTitle;
                }
            }

            if (SerialNumber.Trim().Length != 0)
            {

                if (Serial.Trim().Length == 0)
                {
                    Serial = SerialNumber;
                }
                else
                {
                    Serial = Serial + "-" + SerialNumber;
                }
            }
            return Serial;
        }
        //public class CustomValidator : System.IdentityModel.Selectors.UserNamePasswordValidator
        //{
        //    public override void Validate(string userName, string password)
        //    {
        //        if (userName != @"hello")
        //        {
        //            throw new FaultException(@"User name must be 'hello'.");
        //        }
        //    }
        //}

        public bool SendOTPMail(string OTP, string ToEmail)
        {
            try
            {

                MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
                SmtpClient SMTPServer = new SmtpClient();

                SMTPServer = new SmtpClient();
                string EmailFrom = "";// = ConfigurationManager.AppSettings["Email"];
                string Password = "";// = ConfigurationManager.AppSettings["EPassword"];
                                     //SMTPServer.Host = ConfigurationManager.AppSettings["SmtpClient"];
                                     //SMTPServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                                     //SMTPServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                XmlDocument xml = new XmlDocument();

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
                int TimeOut = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OTPTimeOut"]);

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

                objmail.Headers.Add("NAME", "Admin");

                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                SMTPServer.Send(objmail);
                objmail.To.Clear();

                return true;
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);

                return false;
            }
        }

        bool SendDomesticPassword(string FirstName, string MobileNo, string Password)
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
                            // Response.Write(soapResult);
                            result = true;
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    //lblmsg.Text = "OTP Error";//e.Message;
                    result = false;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
            return result;
        }

        bool SendInternationalPassword(string FirstName, string MobileNo, string Password)
        {
            string OTPSend = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPSend"]);

            string OTPUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IntWebOTPUrl"]);
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
                            //Response.Write(soapResult);
                            result = true;

                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError objEr = new LogError();
                    objEr.HandleException(ex);

                    //lblmsg.Text = "OTP Error";//e.Message;
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

        /// <summary>
        /// Forum list by UserId and EntityId
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public List<ForumResponse> Forum(ForumRequest objRequest)
        {
            try
            {
                List<ForumResponse> objResponse = new List<ForumResponse>();

                IList<ForumDomain> objForumData = ForumDataProvider.Instance.GetForumsByUserAccess(objRequest.UserId, objRequest.EntityId);

                ForumResponse objForumRes;

                foreach (var obj in objForumData)
                {
                    objForumRes = new ForumResponse();

                    objForumRes.ForumId = obj.ForumId;
                    objForumRes.ForumName = obj.ForumName;
                    objForumRes.ForumShortName = obj.ForumShortName;
                    objForumRes.EntityId = obj.EntityId;
                    objForumRes.EntityName = obj.EntityName;
                    objForumRes.CreatedOn = obj.CreatedOn;
                    objForumRes.UpdatedOn = obj.UpdatedOn;

                    objResponse.Add(objForumRes);
                }

                return objResponse;
            }
            catch (Exception ex)
            {
                LogError objError = new LogError();
                objError.HandleException(ex);
                return null;
            }


        }

        /// <summary>
        /// Meeting list by forumId
        /// </summary>
        /// <param name="objRequest"></param>
        /// <returns></returns>
        public List<MeetingResponse> Meeting(MeetingRequest objRequest)
        {
            try
            {
                List<MeetingResponse> objResponse = new List<MeetingResponse>();

                IList<MeetingDomain> objMeetingData = MeetingDataProvider.Instance.GetPublishedMeetingAndAgendaByForumId(objRequest.ForumId).OrderByDescending(p => DateTime.Parse(p.MeetingDate)).ToList();

                MeetingResponse objMeetingRes;
                DateTime dtToday = DateTime.Today;

                foreach (var obj in objMeetingData)
                {
                    DateTime dtMeeting = Convert.ToDateTime(obj.MeetingDate + " " + obj.MeetingTime);
                    if (dtMeeting <= dtToday)
                    {
                        objMeetingRes = new MeetingResponse();

                        objMeetingRes.MeetingId = obj.MeetingId;
                        objMeetingRes.MeetingDate = obj.MeetingDate;
                        objMeetingRes.MeetingVenue = obj.MeetingVenue;
                        objMeetingRes.MeetingTime = obj.MeetingTime;
                        objMeetingRes.MeetingNumber = obj.MeetingNumber;
                        objMeetingRes.CreatedOn = obj.CreatedOn;
                        objMeetingRes.UpdatedOn = obj.UpdatedOn;
                        objMeetingRes.ForumId = obj.ForumId;
                        objMeetingRes.EntityId = obj.EntityId;

                        objResponse.Add(objMeetingRes);
                    }

                }

                return objResponse;
            }
            catch (Exception ex)
            {
                LogError objError = new LogError();
                objError.HandleException(ex);
                return null;
            }


        }

        public List<AgendaResponse> Agenda(AgendaRequest objRequest)
        {
            List<AgendaResponse> objResponse = new List<AgendaResponse>();

            UserEntityServices objUserEntity = new UserEntityServices();

            DataSet ds = UserEntityDataProvider.Instance.GetPublishedAgendaByMeetingId(objRequest.MeetingId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AgendaResponse objAgenda = null;
                    //Agenda details  
                    for (int m = 0; m <= ds.Tables[0].Rows.Count - 1; m++)
                    {
                        objAgenda = new AgendaResponse();
                        objAgenda.AgendaId = Guid.Parse(ds.Tables[0].Rows[m]["AgendaId"].ToString());
                        objAgenda.AgendaName = ds.Tables[0].Rows[m]["AgendaName"].ToString().Replace("`", "&#x20B9");

                        string AgendaPdf = ds.Tables[0].Rows[m]["AgendaNote"].ToString();
                        //--//
                        objAgenda.AgendaNote = ds.Tables[0].Rows[m]["AgendaNote"].ToString();

                        objAgenda.AgendaOrder = Convert.ToInt64(ds.Tables[0].Rows[m]["AgendaOrder"].ToString());
                        objAgenda.CreatedBy = Guid.Parse(ds.Tables[0].Rows[m]["CreatedBy"].ToString());
                        objAgenda.UpdatedBy = Guid.Parse(ds.Tables[0].Rows[m]["UpdatedBy"].ToString());
                        objAgenda.UpdateOn = Convert.ToDateTime(ds.Tables[0].Rows[m]["UpdateOn"].ToString());
                        objAgenda.CreatedOn = Convert.ToDateTime(ds.Tables[0].Rows[m]["CreatedOn"].ToString());
                        objAgenda.EntityId = Guid.Parse(ds.Tables[0].Rows[m]["EntityId"].ToString());
                        objAgenda.EntityName = Encryptor.DecryptString(ds.Tables[0].Rows[m]["EntityName"].ToString());
                        objAgenda.ForumId = Guid.Parse(ds.Tables[0].Rows[m]["ForumId"].ToString());
                        objAgenda.MeetingId = Guid.Parse(ds.Tables[0].Rows[m]["MeetingId"].ToString());
                        objAgenda.ParentAgendaId = Guid.Parse(ds.Tables[0].Rows[m]["ParentAgendaId"].ToString());
                        objAgenda.MeetingVenue = Encryptor.DecryptString(ds.Tables[0].Rows[m]["MeetingVenue"].ToString());
                        objAgenda.Classification = ds.Tables[0].Rows[m]["Classification"].ToString();

                        objAgenda.SerialKeyType = ds.Tables[0].Rows[m]["SerialNumberType"].ToString();

                        objAgenda.Presenter = ds.Tables[0].Rows[m]["Presenter"].ToString();
                        if (objAgenda.ParentAgendaId == Guid.Empty)
                        {
                            objAgenda.SerialNumber = ds.Tables[0].Rows[m]["SerialNumber"].ToString();
                            objAgenda.SerialKey = "";
                        }
                        else
                        {
                            objAgenda.SerialKey = ds.Tables[0].Rows[m]["SerialNumber"].ToString();
                            objAgenda.SerialNumber = "";
                        }

                        objAgenda.AgendaSerialNo = GetTitles(ds.Tables[0].Rows[m]["SerialTitle"].ToString(), ds.Tables[0].Rows[m]["SerialNumber"].ToString(), ds.Tables[0].Rows[m]["SerialText"].ToString());

                        if (ds.Tables[0].Rows[m]["FileUploadOn"].ToString() == "")
                        {
                            objAgenda.FileUploadOn = MM.Core.Null.SetNullDateTime(null).ToUnixTimestamp().ToString();
                        }
                        else
                        {
                            objAgenda.FileUploadOn = MM.Core.Null.SetNullDateTime(ds.Tables[0].Rows[m]["FileUploadOn"]).ToUnixTimestamp().ToString();
                        }
                        string AgendaFile = ds.Tables[0].Rows[m]["UploadedAgendaNote"].ToString();

                        objAgenda.UploadedAgendaNote = string.IsNullOrEmpty(ds.Tables[0].Rows[m]["UploadedAgendaNote"].ToString()) ? "" : (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/emeetings_igendapdf-" + (HttpUtility.UrlEncode(ds.Tables[0].Rows[m]["UploadedAgendaNote"].ToString().Substring(0, ds.Tables[0].Rows[m]["UploadedAgendaNote"].ToString().Length - 4)) + ".pdf")).ToBase64Encryption();

                        objAgenda.ForumName = Encryptor.DecryptString(ds.Tables[0].Rows[m]["ForumName"].ToString());
                        objAgenda.AgendaChecker = Guid.Parse(ds.Tables[0].Rows[m]["AgendaChecker"].ToString());


                        if (ds.Tables[0].Rows[m]["IsActive"].ToString() != null && (ds.Tables[0].Rows[m]["IsActive"].ToString() == "1" || ds.Tables[0].Rows[m]["IsActive"].ToString().ToLower().Equals("true")))
                        {
                            objAgenda.IsActive = true;
                        }
                        else
                        {
                            objAgenda.IsActive = false;
                        }

                        string DelPdfs = ds.Tables[0].Rows[m]["DeletedPDfs"].ToString();

                        List<Pdfs> upPdfs = new List<Pdfs>();
                        if (!string.IsNullOrEmpty(AgendaFile))
                        {
                            string[] arrAgendaFiles = AgendaFile.Replace(" ", "").Split(',');
                            string[] arrAgendaPdfs = AgendaPdf.Replace(" ", "").Split(',');

                            string[] arrDeletedpdf = DelPdfs.Replace(" ", "").Split(',');
                            bool SendPdfFile = false;
                            foreach (string str in arrAgendaFiles)

                                if (!arrDeletedpdf.Contains(str))
                                {
                                    SendPdfFile = true;
                                }

                            if (arrAgendaFiles.Count() > 0 && SendPdfFile)
                            {
                                if (arrAgendaFiles.Count() > 1)
                                {
                                    objAgenda.UploadedAgendaNote = (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Current_Url"]) + "/emeetings_igendapdf-" + (HttpUtility.UrlEncode(arrAgendaFiles[0].Trim().Substring(0, arrAgendaFiles[0].Trim().Length - 4) + "_merge.pdf"))).ToBase64Encryption();
                                }
                            }
                            else
                            {
                                objAgenda.UploadedAgendaNote = "";
                            }
                        }
                        else
                        {
                            objAgenda.UploadedAgendaNote = "";
                        }
                        objResponse.Add(objAgenda);
                    }
                }
            }

            return objResponse;
        }

        public ServerResponse GetAppServerStatus()
        {
            ServerResponse objResposne = new ServerResponse();
            try
            {
                objResposne.Message = "Success";
                objResposne.Status = true;
            }
            catch (Exception ex)
            {
                objResposne.Message = ex.Message;
                objResposne.Status = false;
            }
            return objResposne;
        }

        public ServerResponse GetDbServerStatus()
        {
            EntityServices objService = new EntityServices();
            ServerResponse objResposne = new ServerResponse();
            try
            {
                IList<EntityDomain> objSp = objService.GetAllEntityServices();
                if (objSp != null)
                {
                    objResposne.Message = "Success";
                    objResposne.Status = true;
                }
            }
            catch (Exception ex)
            {
                objResposne.Message = ex.Message;
                objResposne.Status = false;
            }


            return objResposne;
        }
    }
}
