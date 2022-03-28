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

using System.IdentityModel;
using System.Security.Claims;
using System.IdentityModel.Protocols.WSFederation;
using System.Text.RegularExpressions;

namespace MeetingMinder.Web
{
    public partial class Login : System.Web.UI.Page
    {

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {

            // txtPasswordNew.Attributes.Add("value", "************");
            string UserName = Encryptor.EncryptString("user");
            string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
            if (AuthenticationType.ToLower().Equals("ad"))
            {
                divForgotPassword.Visible = false;
            }
            else
            {
                divForgotPassword.Visible = true;
            }

            string Password = Encryptor.EncryptString("password");
            if (Request.QueryString["PageName"] == "Default")
            {
                string userLoggedIn = (string)Session["UserId"];

                System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
                 as System.Collections.Generic.Dictionary<string, string>;
                if (d != null && userLoggedIn != null)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(userLoggedIn, "Logout", "Success", "", "Logout successfully");

                    if (d.Count > 0)
                    {
                        lock (d)
                        {
                            d.Remove(userLoggedIn);
                            Application["UsersLoggedIn"] = d;
                        }
                    }
                }
                if (d != null && userLoggedIn != null)
                {
                    lock (d)
                    {
                        if (d.Count > 0)
                            d.Remove(userLoggedIn);
                        Application["UsersLoggedIn"] = d;
                    }
                }

                UserEntityDataProvider.Instance.UpdateSession(Session.SessionID, Guid.Parse(userLoggedIn));
                Session.Abandon();
                Session.Clear();
                //HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
                //myCookie.Value = Guid.NewGuid().ToString();
                //myCookie.Path = "meetinguat;SameSite=Strict";
                //Response.Cookies.Add(myCookie);

                HttpCookie myCookie = new HttpCookie("iMeet");
                myCookie.Value = Guid.NewGuid().ToString();
                myCookie.Path = "meetinguat;SameSite=Strict";
                Response.Cookies.Add(myCookie);

                Response.Redirect("Login.aspx");
            }


            if (Request.QueryString["PageName"] == "Defaults")
            {

                string userLoggedIn = (string)Session["UserId"];

                System.Collections.Generic.Dictionary<string, string> d = Application["UsersLoggedIn"]
              as System.Collections.Generic.Dictionary<string, string>;
                if (d != null && userLoggedIn != null)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(userLoggedIn, "Logout", "Success", "", "Logout successfully");

                    if (d.Count > 0)
                    {
                        lock (d)
                        {
                            d.Remove(userLoggedIn);
                            Application["UsersLoggedIn"] = d;
                        }
                    }
                }

                if (d != null && userLoggedIn != null)
                {
                    lock (d)
                    {
                        d.Remove(userLoggedIn);
                        Application["UsersLoggedIn"] = d;
                    }
                }


                UserEntityDataProvider.Instance.UpdateSession(Session.SessionID, Guid.Parse(userLoggedIn));
                Session.Abandon();
                Session.Clear();

                //HttpCookie myCookie = new HttpCookie("ASP.NET_SessionId");
                //myCookie.Value = Guid.NewGuid().ToString();
                //myCookie.Path = "meetinguat;SameSite=Strict";
                //Response.Cookies.Add(myCookie);

                HttpCookie myCookie = new HttpCookie("iMeet");
                myCookie.Value = Guid.NewGuid().ToString();
                myCookie.Path = "meetinguat;SameSite=Strict";
                Response.Cookies.Add(myCookie);

                Response.Redirect("Login.aspx");
            }

            //check user user session
            if (Session["UserId"] != null)
            {
                //string prvPage = Request.UrlReferrer.ToString();

                //if (prvPage.ToLower().EndsWith("redirect.aspx"))
                //{
                //    lblmsg.Text = " No department assigend, Please contact Administrator.";
                //    lblmsg.Visible = true;
                //    return;
                //}
                Response.Redirect("Default.aspx");
            }
            else
            {

                if (!IsPostBack)
                {
                    //  UpdateCaptchaText();
                    Session.Abandon();
                    Session.Clear();

                    //remove all cookies
                    foreach (string key in Request.Cookies.AllKeys)
                    {
                        HttpCookie c = Request.Cookies[key];
                        c.Expires = DateTime.Now.AddMonths(-1);
                        Response.AppendCookie(c);
                    }

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                    hdnUserVal.Value = EncryptionHelper.GetChar(16);

                }

            }
        }

        /// <summary>
        /// Generate token for login
        /// </summary>
        /// <param name="e"></param>

        protected void ValidateCaptcha(object sender, ServerValidateEventArgs e)
        {
            CaptchaLogin.ValidateCaptcha(txtCaptchaText.Text.Trim());
            e.IsValid = CaptchaLogin.UserValidated;
            if (e.IsValid)
            {
                txtLogin.Text = "";
                txtPassword.Text = "";
                txtCaptchaText.Text = "";
                hdnPassword.Value = string.Empty;
                lblmsg.Text = "Invalid Captcha.";
                //  ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Valid Captcha!');", true);
            }
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //only post method allow
                if (!string.Equals(Request.HttpMethod, "POST"))
                {
                    Response.StatusCode = 405;
                    Response.Redirect("Login.aspx");
                    Response.End();
                }

                if (txtLogin.Text == "" || hdnPassword.Value == "")
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please enter Username and Password.";

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
                    return;
                }

                UserAcess objUser = new UserAcess();
                if (txtLogin.Text != "")
                {
                    if (!objUser.isValidChar(txtLogin.Text))
                    {
                        txtLogin.Text = "";
                        lblmsg.Text = " Please enter valid characters only.";
                        return;
                    }

                    if (!objUser.CSVValidation(txtLogin.Text))
                    {
                        txtLogin.Text = "";
                        lblmsg.Text = " Please enter valid characters only.";
                        return;
                    }
                }


                string decodedPass = DecryptStringAES(hdnPassword.Value, hdnUserVal.Value);
                string[] strSplit = decodedPass.Split('|');
                txtPassword.Text = strSplit[0];

                //txtPassword.Text = DecryptStringAES(hdnPassword.Value, hdnUserVal.Value);
                string str = "";
                string AdDomain = Convert.ToString(ConfigurationManager.AppSettings["AdDomain"]);
                string ADSPath = Convert.ToString(ConfigurationManager.AppSettings["ADSPath"]);
                UserDomain list;
                CaptchaLogin.ValidateCaptcha(txtCaptchaText.Text.Trim());
                if (!CaptchaLogin.UserValidated)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Failed", "", "Invalid Captcha");

                    lblmsg.Visible = true;
                    txtLogin.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtCaptchaText.Text = string.Empty;
                    hdnPassword.Value = string.Empty;

                    lblmsg.Text = "Invalid Captcha.";

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
                    return;
                }
                txtCaptchaText.Text = string.Empty;

                // UserDomain list;
                string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
                if (AuthenticationType.ToLower().Equals("ad"))
                {
                    bool IsValid = Check(ADSPath, AdDomain, txtLogin.Text.Trim(), txtPassword.Text.Trim());
                    if (!IsValid)
                    {


                        lblmsg.Visible = true;
                        lblmsg.Text = " Invalid Username and Password, Please contact Administrator.";
                        txtLogin.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        hdnPassword.Value = string.Empty;

                        string sessioni = Guid.NewGuid().ToString();
                        //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                        //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                        Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                        HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
                        return;
                    }
                    list = UserDataProvider.Instance.GetUserByUserName(txtLogin.Text.Trim().ToLower());
                    hdnUserVal.Value = EncryptionHelper.GetChar(16);
                    hdnPassword.Value = string.Empty;
                }
                else
                {
                    list = UserDataProvider.Instance.UserLogin(txtLogin.Text.Trim().ToLower(), txtPassword.Text.Trim());
                    hdnUserVal.Value = EncryptionHelper.GetChar(16);
                    hdnPassword.Value = string.Empty;

                    string dateString = @"" + strSplit[1];
                    DateTime date3 = Convert.ToDateTime(dateString, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat);
                    string strDate = date3.ToString();
                    //date3 \{05/24/19 1:34:47 PM}";

                    if (list != null)
                    {
                        if (list.LoginRequest != strDate)
                        {
                            bool bStatus = UserDataProvider.Instance.UpdateLoginRequest(list.UserId, strSplit[1]);
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = " Invalid Username or Password, Please contact Administrator.";
                            txtLogin.Text = string.Empty;
                            txtPassword.Text = string.Empty;
                            hdnPassword.Value = string.Empty;

                            string sessioni = Guid.NewGuid().ToString();
                            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                            //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                            Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                            HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;

                            return;
                        }
                    }

                }

                if (list == null)
                {
                    lblMessage.Text = string.Empty;
                    System.Data.DataSet ds = UserDataProvider.Instance.LockAccount(txtLogin.Text.Trim());

                    //comment 18Jan19
                    //if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    //{

                    //    if (ds.Tables[2].Rows[0]["WrongPasswordAttempt"].ToString() == "0")
                    //    {
                    //        lblMessage.Text = "Wrong password attempt:" + 1;
                    //    }
                    //    else
                    //    {
                    //        lblMessage.Text = "Wrong password attempt:" + ds.Tables[2].Rows[0]["WrongPasswordAttempt"].ToString();
                    //    }
                    //    string isLocked = ds.Tables[2].Rows[0]["IsLocked"].ToString();

                    //    if (isLocked.ToLower().Equals("true"))
                    //    {
                    //        lblMessage.Text = "";
                    //        lblmsg.Text = " Your account is locked, Please contact Administrator.";
                    //        lblmsg.Visible = true;

                    //        string sessionid = Guid.NewGuid().ToString();
                    //        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessionid));
                    //        HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    //        return;
                    //    }
                    //}

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {

                        if (ds.Tables[0].Rows[0]["WrongPasswordAttempt"].ToString() == "0")
                        {
                            lblMessage.Text = "Wrong password attempt:" + 1;
                        }
                        else
                        {
                            lblMessage.Text = "Wrong password attempt:" + ds.Tables[0].Rows[0]["WrongPasswordAttempt"].ToString();
                        }
                        string isLocked = ds.Tables[0].Rows[0]["IsLocked"].ToString();

                        if (isLocked.ToLower().Equals("true"))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Failed", "", "Your account is locked, Please contact Administrator");

                            lblMessage.Text = "";
                            lblmsg.Text = " Your account is locked, Please contact Administrator.";
                            lblmsg.Visible = true;

                            string sessionid = Guid.NewGuid().ToString();
                            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessionid));
                            //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                            Response.Cookies.Add(new HttpCookie("iMeet", sessionid));
                            HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
                            return;
                        }
                    }

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Failed", "", lblMessage.Text == "" ? "Invalid Username or Password" : lblMessage.Text);

                    lblmsg.Visible = true;
                    lblmsg.Text = " Invalid Username or Password, Please contact Administrator.";
                    txtLogin.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    hdnPassword.Value = string.Empty;

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
                    return;
                }
                else if (list.IsLocked)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Failed", "", "Your account is locked, Please contact Administrator");

                    lblMessage.Text = string.Empty;
                    lblmsg.Visible = true;
                    lblmsg.Text = " Your account is locked, Please contact Administrator.";
                    txtLogin.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    hdnPassword.Value = string.Empty;

                    string sessioni = Guid.NewGuid().ToString();
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                    //HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Path = Request.ApplicationPath;

                    Response.Cookies.Add(new HttpCookie("iMeet", sessioni));
                    HttpContext.Current.Response.Cookies["iMeet"].Path = Request.ApplicationPath;
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

                    if (list.IsEnabledOnWebApp)
                    {
                        IList<UserEntityDomain> objUserEntityDomain = UserEntityDataProvider.Instance.GetEntityListByUserId(list.UserId).OrderBy(p => p.EntityName).ToList(); //EntityDataProvider.Instance.Get();
                        if (objUserEntityDomain.Count == 0)
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Failed", "", "No department assigend, Please contact Administrator");

                            lblmsg.Text = " No department assigend, Please contact Administrator.";
                            lblmsg.Visible = true;
                            return;
                        }

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(txtLogin.Text, "Login", "Success", "", "Login successfully");

                        Session["UserPFId"] = txtLogin.Text.Trim();
                        Session["UserId"] = list.UserId.ToString();
                        Session["UserName"] = list.UserName.ToString();
                        Session["IsMaker"] = list.IsMaker;
                        Session["IsChecker"] = list.IsChecker;
                        Session["FName"] = list.FirstName;
                        Session["LName"] = list.LastName;

                        Session["IsSuperAdmin"] = list.IsSuperAdmin;

                        Session.Timeout = list.SessionTimeout;
                        txtLogin.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                        hdnPassword.Value = string.Empty;

                        //check for password change                        
                        str += " session set " + System.DateTime.Now; ;

                        //       IList<AccessRightDomain> objAccess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(list.UserId.ToString()));


                        ////var parent = from obj in objAccess where obj.

                        //RollDomain objRoll = RollDataProvider.Instance.GetRollByUserId(Guid.Parse(list.UserId.ToString()));

                        //Session["Roll"] = objRoll;
                        //Session["AccessRight"] = objAccess;



                        int sessionIdValue = UserEntityDataProvider.Instance.InsertSession(Session.SessionID, list.UserId);
                        if (sessionIdValue == 0)
                        {
                            Session.Abandon();
                            Session.Clear();
                            string sessioni = Guid.NewGuid().ToString();
                            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", sessioni));
                            Response.Cookies.Add(new HttpCookie("iMeet", sessioni));

                            lblmsg.Visible = true;
                            lblmsg.Text = "Invalid Username or Password, Please contact Administrator.";
                            return;
                        }

                        bool IsLoggedIn = AddLoginList(list.UserId.ToString());
                        if (!AuthenticationType.ToLower().Equals("ad"))
                        {
                            if (!list.IsAuthorized)
                            {
                                Session["IsAuthorized"] = false;
                                Response.Redirect("ChangePassword.aspx");
                            }
                        }


                        if (!IsLoggedIn)
                        {
                            //txtLogin.Text = "";
                            //txtPassword.Text = "";
                            //lblmsg.Visible = true;
                            //lblmsg.Text = "You are already logged in";
                            //return;
                            ClientScript.RegisterStartupScript(GetType(), "alert", "alert('You are already logged in');window.location='redirect.aspx'", true);
                            return;
                        }
                        string cookieId = Guid.NewGuid().ToString();


                        //Response.Redirect("Default.aspx");
                        Response.Redirect("Redirect.aspx");
                        Context.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {

                        lblmsg.Visible = true;
                        lblmsg.Text = "You are not registered to access the web portal, Please contact Administrator.";
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
                //  Response.Write(ex);
                lblmsg.Visible = true;
                lblmsg.Text = "Invalid Username or Password, Please contact Administrator.";

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
            ClaimsPrincipal claimsPrincipal = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
            {

            }
            //    string AdValue = Convert.ToString(ConfigurationManager.AppSettings["AdValue"]);
            //switch (AdValue)
            //{
            //    case "1":
            //        try
            //        {
            //            LdapConnection connect = new LdapConnection(Domain);
            //            NetworkCredential credent = new NetworkCredential(UserName, Password);
            //            connect.Credential = credent;
            //            connect.Bind();
            //            return true;
            //            break;
            //        }
            //        catch (Exception ex)
            //        {
            //            LogError objEr = new LogError();
            //            objEr.HandleException(ex);
            //            return false;
            //            break;
            //        }

            //    case "2":
            //        try
            //        {
            //            PrincipalContext adContext = new PrincipalContext(ContextType.Domain);
            //            // find a user
            //            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, UserName);
            //            using (adContext)
            //            {
            //                bool t = adContext.ValidateCredentials(UserName, Password);
            //                return t;
            //                break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            LogError objEr = new LogError();
            //            objEr.HandleException(ex);
            //            return false;
            //            break;
            //        }

            //    case "3":
            //        string domainaAndUsername = Domain + @"\" + UserName;
            //        DirectoryEntry entrys = new DirectoryEntry(Domain, domainaAndUsername, Password);
            //        try
            //        {
            //            object obj = entrys.NativeObject;
            //            DirectorySearcher search = new DirectorySearcher(entrys);
            //            search.Filter = "(SAMAccountName=" + UserName + ")";
            //            search.PropertiesToLoad.Add("cn");
            //            SearchResult result = search.FindOne();
            //            if (null == result)
            //            {
            //                return false;
            //                break;
            //            }
            //            else
            //            {
            //                return true;
            //                break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            LogError objEr = new LogError();
            //            objEr.HandleException(ex);
            //            return false;
            //            break;
            //        }

            //    case "4":
            //        try
            //        {
            //            string domainAndUsername = Domain + @"\" + UserName;
            //            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, Password);
            //            DirectorySearcher search = new DirectorySearcher(entry);
            //            search.Filter = "(SAMAccountName=" + UserName + ")";
            //            search.PropertiesToLoad.Add("cn");
            //            SearchResult result = search.FindOne();
            //            if (result != null)
            //            {
            //                path = result.Path;
            //                string filterAttribute = (string)result.Properties["cn"][0];
            //                return true;
            //                break;
            //            }
            //            else
            //            { return false; break; }

            //        }
            //        catch (Exception ex)
            //        {
            //            LogError objEr = new LogError();
            //            objEr.HandleException(ex);
            //            return false; break;
            //        }
            //}
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

        //protected void ValidateNoUrls(object sender, ServerValidateEventArgs e)
        //{
        //    e.IsValid = !Regex.IsMatch(e.Value, @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?");
        //}

    }

    public class SessionManager : SessionIDManager, ISessionIDManager
    {
        void ISessionIDManager.SaveSessionID(HttpContext context, string id, out bool redirected, out bool cookieAdded)
        {
            base.SaveSessionID(context, id, out redirected, out cookieAdded);
            if (HttpContext.Current.Request.Path.ToLower().EndsWith("login.aspx"))
            {
                if (cookieAdded)
                {
                    //var name = "ASP.NET_SessionId";
                    var name = "iMeet";
                    var cookie = context.Response.Cookies[name];
                    //cookie.Domain = HttpContext.Current.Request.Url.Host;
                    cookie.Path = HttpContext.Current.Request.ApplicationPath; //"/emeeting";
                    // cookie.Secure = true;
                    //cookie.Path = "meetinguat;SameSite=Strict";
                    //cookie.Domain = "meetinguat.sbi.co.in";
                    //cookie.Secure = true;
                }
            }
            else
            {
                if (!HttpContext.Current.Request.Path.ToLower().EndsWith("default.aspx"))
                {
                    base.RemoveSessionID(HttpContext.Current);
                }
            }
            //if (HttpContext.Current.Request.UrlReferrer != null)
            //{
            //    string prvPage = HttpContext.Current.Request.UrlReferrer.ToString();

            //    if (prvPage.ToLower().EndsWith("login.aspx") && HttpContext.Current.Request.Path.ToLower().EndsWith("default.aspx"))
            //    {
            //        if (cookieAdded)
            //        {
            //            var name = "ASP.NET_SessionId";
            //            var cookie = context.Response.Cookies[name];
            //            cookie.Domain = HttpContext.Current.Request.Url.Host;
            //            cookie.Path = HttpContext.Current.Request.ApplicationPath; //"/emeeting";
            //            cookie.Secure = true;
            //        }
            //    }
            //}
        }
    }

    public static class Network
    {
        public static string GetBaseDomain(string domainName)
        {
            var tokens = domainName.Split('.');

            // only split 3 segments like www.west-wind.com
            if (tokens == null || tokens.Length != 3)
                return domainName;

            var tok = new List<string>(tokens);
            var remove = tokens.Length - 2;
            tok.RemoveRange(0, remove);

            return tok[0] + "." + tok[1]; ;
        }

        /// <summary>
        /// Returns the base domain from a domain name
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetBaseDomain(this Uri uri)
        {
            if (uri.HostNameType == UriHostNameType.Dns)
                return GetBaseDomain(uri.DnsSafeHost);

            return uri.Host;
        }


    }


}