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
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace MeetingMinder.Web
{
    public partial class ChangePasswordaspx : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;



            string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);
            if (AuthenticationType.ToLower().Equals("ad"))
            {
                Response.Redirect("Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                hdnUserVal.Value = EncryptionHelper.GetChar(16);

                if (Session["ans"] == null)
                {
                    Response.Redirect("AnswerQuestion.aspx", true);
                }
                else
                {
                    Session["ans"] = null;
                }

                //check user user session
                if (Session["UserId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                //check user is answered security question 
                string strPreviousUrl = Convert.ToString(Request.UrlReferrer);
                if (strPreviousUrl.Contains("AnswerQuestion.aspx"))
                {

                }
                else
                {
                    Response.Redirect("AnswerQuestion.aspx");
                }
            }
        }

        /// <summary>
        /// button submit Event
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

                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));

                string strOld = DecryptStringAES(hdnOld.Value, hdnUserVal.Value);
                string strNew = DecryptStringAES(hdnNew.Value, hdnUserVal.Value);

                //string strOld = txtOld.Text;
                //string strNew = txtNew.Text;

                hdnUserVal.Value = EncryptionHelper.GetChar(16);
                hdnOld.Value = string.Empty;
                hdnNew.Value = string.Empty;
                string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

                Match match = Regex.Match(strNew, passwordRegex);

                if (!match.Success)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Failed", "", "Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character");

                    ((Label)Error.FindControl("lblError")).Text = " Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character.";
                    Error.Visible = true;
                    return;
                }
                if (strNew == Session["UserName"].ToString())
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Failed", "", "Password is the same as username. This is no longer allowed");

                    ((Label)Error.FindControl("lblError")).Text = " Password is the same as username. This is no longer allowed.";
                    Error.Visible = true;
                    return;
                }

                if (strOld == strNew)
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Failed", "", "New Password should not be same as old password");

                    ((Label)Error.FindControl("lblError")).Text = " New Password should not be same as old password.";
                    Error.Visible = true;
                    return;
                }

                DataTable dt = new DataTable();
                dt = UserDataProvider.Instance.ChangePasswordWebNew(UserId, strNew);
                string[] data = dt.AsEnumerable().Where(s => s.Field<string>("Password") == Encryptor.EncryptString(strNew)).Select(s => s.Field<string>("Password")).ToArray();
                int arrcount = data.Length;
                if (arrcount == 0)
                {
                    bool isChange = UserDataProvider.Instance.ChangePasswordWeb(UserId, strOld, strNew);
                    if (isChange)
                    {
                        ((Label)Info.FindControl("lblName")).Text = "Password changed successfully";

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Success", "", "Password changed successfully");

                        if (Session["EntityId"] == null)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!');window.location='Redirect.aspx';</script>'");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!');window.location='Redirect.aspx';</script>'");
                        }
                        Session["IsAuthorized"] = null;

                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Failed", "", "Please check your old password");

                        ((Label)Error.FindControl("lblError")).Text = "Error. Please check your old password.";
                        Error.Visible = true;
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(UserId), "Change Password", "Failed", "", "Previous 3 password is not allow!");

                    ((Label)Error.FindControl("lblError")).Text = "Error. Previous 3 password is not allow!.";
                    Error.Visible = true;
                }


                Info.Visible = true;
                txtOld.Text = "";
                txtNew.Text = "";
                txtConfirm.Text = "";
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
        /// button cancel Event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Session["IsAuthorized"] != null)
            {
                Response.Redirect("Login.aspx?PageName=Default");
            }
            else
            {
                Response.Redirect("default.aspx");
            }
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

    }
}