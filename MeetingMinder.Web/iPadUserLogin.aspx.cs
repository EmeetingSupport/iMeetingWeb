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
    public partial class Test123 : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {

            //string UserName = Encryptor.EncryptString("user");
            //string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
            //if (AuthenticationType.ToLower().Equals("ad"))
            //{
            //    divForgotPassword.Visible = false;
            //}
            //else
            //{
            //    divForgotPassword.Visible = true;
            //}

            //string Password = Encryptor.EncryptString("password");
            //if (Request.QueryString["PageName"] == "menifest")
            //{
            //    string userLoggedIn = (string)Session["UserIdDownload"];
            //    System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
            //   as System.Collections.Generic.Dictionary<string, string>;
            //    //if (d != null)
            //    //{
            //    //    lock (d)
            //    //    {
            //    //        d.Remove(userLoggedIn);
            //    //        Application["UsersLoggedIn"] = d;
            //    //    }
            //    //}
            //    if (d != null && userLoggedIn !=null)
            //    {
            //        if (d.Count > 0)
            //        {
            //            lock (d)
            //            {
            //                d.Remove(userLoggedIn);
            //                Application["UsersLoggedIn"] = d;
            //            }
            //        }
            //    }
            //    if (d != null && userLoggedIn != null)
            //    {
            //        lock (d)
            //        {
            //            if (d.Count > 0)
            //                d.Remove(userLoggedIn);
            //            Application["UsersLoggedIn"] = d;
            //        }
            //    }

            //UserEntityDataProvider.Instance.UpdateSession(Session.SessionID, Guid.Parse(userLoggedIn));

            //Session.Abandon();
            //Session.Clear();
            ////Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", Guid.NewGuid().ToString()));
            ////   Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            //HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
            //myCookie.Value = Guid.NewGuid().ToString();
            //myCookie.Path = Request.ApplicationPath;
            ////myCookie.Secure = true;
            //// myCookie.Domain = Request.Url.Host.Replace("www.", "");
            //Response.Cookies.Add(myCookie);

            //Response.Redirect("download.aspx");
            //}

            //#region
            //if (Request.QueryString["PageName"] == "menifest")
            //{

            //    string userLoggedIn = (string)Session["UserIdDownload"];
            //    System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
            //  as System.Collections.Generic.Dictionary<string, string>;
            //    if (d != null)
            //    {
            //        if (d.Count > 0)
            //        {
            //            lock (d)
            //            {
            //                d.Remove(userLoggedIn);
            //                Application["UsersLoggedIn"] = d;
            //            }
            //        }
            //    }

            //    if (d != null)
            //    {
            //        lock (d)
            //        {
            //            d.Remove(userLoggedIn);
            //            Application["UsersLoggedIn"] = d;
            //        }
            //    }


            //    UserEntityDataProvider.Instance.UpdateSession(Session.SessionID, Guid.Parse(userLoggedIn));
            //    Session.Abandon();
            //    Session.Clear();

            //    HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
            //    myCookie.Value = Guid.NewGuid().ToString();
            //    myCookie.Path = Request.ApplicationPath;


            //    Response.Cookies.Add(myCookie);

            //    Response.Redirect("download.aspx");
            //}
            //#endregion

            //check user user session
            if (!IsPostBack)
            {
                //  UpdateCaptchaText();
                Session.Abandon();
                Session.Clear();
                string sessioni = Guid.NewGuid().ToString();
                //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                hdnUserVal.Value = EncryptionHelper.GetChar(16);


            }

            //if (Session["UserIdDownload"] != null)
            //{
            //    Response.Redirect("download.html");
            //}
            //else
            //{
            //    if (Request.QueryString["id"] != null)
            //    {

            //    }

            //    if (!IsPostBack)
            //    {
            //        //  UpdateCaptchaText();
            //        Session.Abandon();
            //        Session.Clear();
            //        string sessioni = Guid.NewGuid().ToString();
            //        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
            //        HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;
            //        hdnUserVal.Value = EncryptionHelper.GetChar(16);
            //    }
            //}
        }

        public bool Check(string Domain, string UserName, string Password)
        {
            try
            {
                LdapConnection connect = new LdapConnection(Domain);
                NetworkCredential credent = new NetworkCredential(UserName, Password);
                connect.Credential = credent;
                connect.Bind();
                return true;
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Login button click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            try
            {

                if (txtLogin.Text == "" || txtPassword.Text == "")
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please enter Username and Password.";
                    return;
                }

                txtPassword.Text = DecryptStringAES(txtPassword.Text, hdnUserVal.Value);

                string str = "";
                string AdDomain = Convert.ToString(ConfigurationManager.AppSettings["AdDomain"]);
                string ADSPath = Convert.ToString(ConfigurationManager.AppSettings["ADSPath"]);

                CaptchaLogin1.ValidateCaptcha(txtCaptchaText.Text.Trim());
                if (!CaptchaLogin1.UserValidated)
                {

                    lblmsg.Visible = true;
                    txtLogin.Text = "";
                    txtPassword.Text = "";
                    txtCaptchaText.Text = "";

                    lblmsg.Text = "Invalid Captcha.";

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;
                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                    return;
                }
                txtCaptchaText.Text = "";


                UserDomain list;
                string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
                if (AuthenticationType.ToLower().Equals("ad"))
                {
                    bool IsValid = Check(ADSPath, AdDomain, txtLogin.Text.Trim(), txtPassword.Text.Trim());
                    if (!IsValid)
                    {


                        lblmsg.Visible = true;
                        lblmsg.Text = " Invalid Username and Password, Please contact Administrator.";
                        txtLogin.Text = "";
                        txtPassword.Text = "";
                        return;
                    }
                    list = UserDataProvider.Instance.GetUserByUserName(txtLogin.Text.Trim().ToLower());
                    hdnUserVal.Value = EncryptionHelper.GetChar(16);
                }
                else
                {
                    list = UserDataProvider.Instance.UserLoginDownload(txtLogin.Text.Trim().ToLower(), txtPassword.Text.Trim());
                    hdnUserVal.Value = EncryptionHelper.GetChar(16);
                }

                if (list == null)
                {
                    lblMessage.Text = "";
                    System.Data.DataSet ds = UserDataProvider.Instance.LockAccount(txtLogin.Text.Trim());

                    if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        lblMessage.Text = "Wrong password attempt:" + ds.Tables[2].Rows[0]["WrongPasswordAttempt"].ToString();
                        string isLocked = ds.Tables[2].Rows[0]["IsLocked"].ToString();

                        if (isLocked.ToLower().Equals("true"))
                        {
                            lblMessage.Text = "";
                            lblmsg.Text = " Your account is locked, Please contact Administrator.";
                            lblmsg.Visible = true;
                            return;
                        }
                    }

                    lblmsg.Visible = true;
                    lblmsg.Text = " Invalid Username or Password, Please contact Administrator.";
                    txtLogin.Text = "";
                    txtPassword.Text = "";
                    return;
                }
                else if (list.IsLocked)
                {
                    lblMessage.Text = "";
                    lblmsg.Visible = true;
                    lblmsg.Text = " Your account is locked, Please contact Administrator.";
                    txtLogin.Text = "";
                    txtPassword.Text = "";
                    return;
                }
                else
                {
                    bool bStatus = UserDataProvider.Instance.UnlockAccount(list.UserId);
                }

                //  string strPassword = GetSHA256(txtPassword.Text);

                if (list != null)
                {

                    //bool IsLoggedIn = AddLoginList(list.UserId.ToString());
                    //if (!IsLoggedIn)
                    //{
                    //    txtLogin.Text = "";
                    //    txtPassword.Text = "";
                    //    lblmsg.Visible = true;
                    //    lblmsg.Text = "You are already logged in";
                    //    return;
                    //}

                    if (list.Mobile == null || list.Mobile.Length == 0)
                    {
                        lblMessage.Text = "Your Mobile Number is not registered for OTP, Please Contact Administrator";
                        return;
                    }

                    if (!(list.Mobile.Length > 9))
                    {
                        lblMessage.Text = "Invalid Mobile Number, Please Contact Administrator";
                        return;
                    }

                    if (list.IsEnabledOnIpad)
                    {
                        Session["UserIdDownload"] = list.UserId;
                        //Session["UserNameDownload"] = list.UserName.ToString();
                        ////Session["IsMaker"] = list.IsMaker;
                        ////Session["IsChecker"] = list.IsChecker;
                        //Session["FNameDownload"] = list.FirstName;
                        //Session["LNameDownload"] = list.LastName;
                        Session["OTPId"] = list.OTPId;

                        Session.Timeout = list.SessionTimeout;
                        txtLogin.Text = "";
                        txtPassword.Text = "";
                        //check for password change

                        str += " session set " + System.DateTime.Now; ;

                        string cookieId = Guid.NewGuid().ToString();

                        //Response.Redirect("Default.aspx");
                        //Response.Redirect("Redirect.aspx");                      
                        //Response.Redirect("download.html");

                        //bool res = SendOTPMail(list.OTP, list.EmailID1);

                        string mob = list.Mobile.Substring(0, 2);
                        if (mob == "91")
                        {
                            Session["UserType"] = null;
                            Session["MobileNo"] = list.Mobile;
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
                            UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "OTP", "", "", Encryptor.EncryptString(response));
                        }
                        else
                        {
                            Session["UserType"] = "International";
                            Session["MobileNo"] = list.Mobile;
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
                            UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Internation OTP", "", "", Encryptor.EncryptString(response));

                            //Session["EmailID1"] = list.EmailID1;
                            //bool res = SendOTPMail(list.OTP, list.EmailID1);
                            //if (!res)
                            //{
                            //    lblmsg.Visible = true;
                            //    lblmsg.Text = "Invalid emaild address, Please contact Administrator.";
                            //    Session.Abandon();
                            //    return;
                            //}

                            //string RedirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WebIPAUrl"]);
                            //Response.Redirect(RedirectUrl);
                        }

                        Response.Redirect("iPadUserVerifyOtp.aspx");
                        Context.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {

                        lblmsg.Visible = true;
                        lblmsg.Text = "Invalid Username or Password, Please contact Administrator.";
                    }
                }
                else
                {

                    Session.Abandon();
                    Session.Clear();
                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                    lblmsg.Visible = true;
                    lblmsg.Text = "Invalid Username or Password, Please contact Administrator";
                }
            }
            catch (Exception ex)
            {
                Session.Abandon();
                Session.Clear();
                string sessioni = Guid.NewGuid().ToString();
                Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                //  Response.Write(ex);
                lblmsg.Visible = true;
                lblmsg.Text = "Please try after sometime.";

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        private void UpdateCaptchaText()
        {
            //txtCaptchaText.Text = string.Empty;
            //lblStatus.Visible = false;
            //Store the captcha text in session to validate
            Session["Captcha"] = Guid.NewGuid().ToString().Substring(0, 6);
        }
        public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }
        public static string DecryptStringAES(string cipherText, string keyVal)
        {

            var keybytes = Encoding.UTF8.GetBytes(keyVal);
            var iv = Encoding.UTF8.GetBytes(keyVal);

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string GetSHA256(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
        public bool Check(string path, string Domain, string UserName, string Password)
        {
            string AdValue = Convert.ToString(ConfigurationManager.AppSettings["AdValue"]);
            switch (AdValue)
            {
                case "1":
                    try
                    {
                        LdapConnection connect = new LdapConnection(Domain);
                        NetworkCredential credent = new NetworkCredential(UserName, Password);
                        connect.Credential = credent;
                        connect.Bind();
                        return true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        LogError objEr = new LogError();
                        objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "2":
                    try
                    {
                        PrincipalContext adContext = new PrincipalContext(ContextType.Domain);
                        // find a user
                        UserPrincipal user = UserPrincipal.FindByIdentity(adContext, UserName);
                        using (adContext)
                        {
                            bool t = adContext.ValidateCredentials(UserName, Password);
                            return t;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError objEr = new LogError();
                        objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "3":
                    string domainaAndUsername = Domain + @"\" + UserName;
                    DirectoryEntry entrys = new DirectoryEntry(Domain, domainaAndUsername, Password);
                    try
                    {
                        object obj = entrys.NativeObject;
                        DirectorySearcher search = new DirectorySearcher(entrys);
                        search.Filter = "(SAMAccountName=" + UserName + ")";
                        search.PropertiesToLoad.Add("cn");
                        SearchResult result = search.FindOne();
                        if (null == result)
                        {
                            return false;
                            break;
                        }
                        else
                        {
                            return true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError objEr = new LogError();
                        objEr.HandleException(ex);
                        return false;
                        break;
                    }

                case "4":
                    try
                    {
                        string domainAndUsername = Domain + @"\" + UserName;
                        DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, Password);
                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + UserName + ")";
                        search.PropertiesToLoad.Add("cn");
                        SearchResult result = search.FindOne();
                        if (result != null)
                        {
                            path = result.Path;
                            string filterAttribute = (string)result.Properties["cn"][0];
                            return true;
                            break;
                        }
                        else
                        { return false; break; }

                    }
                    catch (Exception ex)
                    {
                        LogError objEr = new LogError();
                        objEr.HandleException(ex);
                        return false; break;
                    }
            }
            return false;
        }

        private string GetControlThatCausedPostBack(Page page)
        {
            if (!page.IsPostBack)
                return string.Empty;

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                // ReSharper disable TooWideLocalVariableScope
                string controlId;
                Control foundControl;
                // ReSharper restore TooWideLocalVariableScope

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        protected void btnReGenerate_Click(object sender, EventArgs e)
        {
            UpdateCaptchaText();
        }

        public bool AddLoginList(string userId)
        {
            System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
         as System.Collections.Generic.Dictionary<string, string>;
            if (d != null)
            {
                lock (d)
                {
                    if (d.Keys.Contains(userId))
                    {
                        d[userId] = Session.SessionID;
                        // User is already logged in!!!
                        return false;
                    }
                    d.Add(userId, Session.SessionID);
                    Application["UsersLoggedIn"] = d;
                }
            }

            return true;
        }

        protected void ValidateCaptcha(object sender, ServerValidateEventArgs e)
        {
            CaptchaLogin1.ValidateCaptcha(txtCaptchaText.Text.Trim());
            e.IsValid = CaptchaLogin1.UserValidated;
            if (e.IsValid)
            {
                txtLogin.Text = "";
                txtPassword.Text = "";
                txtCaptchaText.Text = "";
                hdnPassword.Value = "";
                lblmsg.Text = "Invalid Captcha.";
                //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Valid Captcha!');", true);
            }
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
                string OTP = Session["OTPPassword"].ToString();
                if (txtOtp.Text.Trim() == OTP)
                {
                    CaptchaControl2.ValidateCaptcha(txtCaptcha2.Text.Trim());
                    if (!CaptchaControl2.UserValidated)
                    {

                        lblmsg.Visible = true;
                        txtOtp.Text = "";
                        txtCaptcha2.Text = "";

                        lblmsg.Text = "Invalid Captcha.";

                        string sessioni = Guid.NewGuid().ToString();
                        //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                        //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;
                        Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                        HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                        return;
                    }
                    txtCaptcha2.Text = "";

                    //Response.Redirect("download.html");
                    Response.Redirect("https://emeeting.sbi.co.in/updates/download.html");
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Invalid OTP, Please try again.";
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

                    lblMessage.Text = "OTP Error";//e.Message;
                }
            }
            else
            {
                //objResponse.OTPSend = AesEncryption.EncryptString256(RequestIdCurr + "!@#$" + Convert.ToBoolean(OTPSend).ToString().ToLower());
            }
        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
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