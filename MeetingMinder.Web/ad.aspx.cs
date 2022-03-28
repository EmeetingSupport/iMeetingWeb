using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices.Protocols;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Script.Serialization;

namespace MeetingMinder.Web
{
    public partial class ad : System.Web.UI.Page
    {
        public string getRandomKey(int bytelength)
        {
            byte[] buff = new byte[bytelength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            StringBuilder sb = new StringBuilder(bytelength * 2);
            for (int i = 0; i < buff.Length; i++)
                sb.Append(string.Format("{0:X2}", buff[i]));
            return sb.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(getASPNET20machinekey());
        }

        public string getASPNET20machinekey()
        {
            StringBuilder aspnet20machinekey = new StringBuilder();
            string key64byte = getRandomKey(64);
            string key32byte = getRandomKey(32);
            aspnet20machinekey.Append("<machineKey \n");
            aspnet20machinekey.Append("validationKey=\"" + key64byte + "\"\n");
            aspnet20machinekey.Append("decryptionKey=\"" + key32byte + "\"\n");
            aspnet20machinekey.Append("validation=\"SHA1\" decryption=\"AES\"\n");
            aspnet20machinekey.Append("/>\n");
            return aspnet20machinekey.ToString();
        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                LdapConnection connect = new LdapConnection(txtDomain.Text);
                NetworkCredential credent = new NetworkCredential(txtUserName.Text, txtPassword.Text);
                connect.Credential = credent;
                connect.Bind();
                lblMsg.Text = "sucess";
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        //2
        protected void btnTest_Click1(object sender, EventArgs e)
        {
            try
            {
                PrincipalContext adContext = new PrincipalContext(ContextType.Domain);

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(adContext, txtUserName.Text);

                if (user != null)
                {
                    lblMsg.Text += " your user exists -";

                }
                else
                {
                    lblMsg.Text += " your user in question does *not* exist - do something else....";
                }



                using (adContext)
                {
                    bool t = adContext.ValidateCredentials(txtUserName.Text, txtPassword.Text);
                    lblMsg.Text += t.ToString();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }


        //3
        protected void btnTest_Click2(object sender, EventArgs e)
        {
            string domainAndUsername = txtDomain.Text + @"\" + txtUserName.Text;
            DirectoryEntry entry = new DirectoryEntry(txtDomain.Text, domainAndUsername, txtPassword.Text);

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + txtUserName.Text + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    lblMsg.Text = "No User found";
                }
                else
                {
                    lblMsg.Text = "Msg : " + result;
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
        //4
        protected void btnTest_Click3(object sender, EventArgs e)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(txtDomain.Text);
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                SearchResultCollection results;
                mySearcher.Filter = "cn=" + txtUserName.Text;
                results = mySearcher.FindAll();
                Response.Write("1 :<br/>");
                foreach (SearchResult resEnt in results)
                {
                    ResultPropertyCollection propcoll = resEnt.Properties;
                    foreach (string key in propcoll.PropertyNames)
                    {
                        foreach (object values in propcoll[key])
                        {
                            Response.Write(values);
                        }
                    }
                }

                Response.Write("<br/> 2:");
                mySearcher.Filter = "name=value";
                results = mySearcher.FindAll();

                foreach (SearchResult resEnt in results)
                {
                    ResultPropertyCollection propcoll = resEnt.Properties;
                    foreach (string key in propcoll.PropertyNames)
                    {
                        foreach (object values in propcoll[key])
                        {
                            Response.Write(values);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        //5
        protected void btnTest_Click4(object sender, EventArgs e)
        {
            try
            {
                PrincipalContext adContext = new PrincipalContext(ContextType.Domain, txtDomain.Text);

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(adContext, txtUserName.Text);

                if (user != null)
                {
                    lblMsg.Text = " your user exists -";

                }
                else
                {
                    lblMsg.Text = " your user in question does *not* exist - do something else....";
                }



                using (adContext)
                {
                    bool t = adContext.ValidateCredentials(txtUserName.Text, txtPassword.Text);
                    lblMsg.Text = t.ToString();
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        //6
        protected void btnTest_Click5(object sender, EventArgs e)
        {
            try
            {
                using (var domainContext = new PrincipalContext(ContextType.Domain, txtDomain.Text))
                {
                    using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, txtDomain.Text))
                    {
                        if (foundUser != null)
                        {
                            lblMsg.Text = " found " + foundUser.UserPrincipalName;
                        }
                        else
                        {
                            lblMsg.Text = "No user found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        //7
        protected void btnTest_Click6(object sender, EventArgs e)
        {
            try
            {
                System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(txtDomain.Text);
                foreach (DirectoryEntry child in entry.Children)
                {
                    if (child.SchemaClassName == "User")
                    {
                        if (child.Name == txtUserName.Text)
                        {
                            lblMsg.Text = "Exist";
                        }
                        else
                        {
                            lblMsg.Text = "Not exists";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            try
            {
                string Domain = Convert.ToString(ConfigurationManager.AppSettings["AdDomain"]);
                string path = Convert.ToString(ConfigurationManager.AppSettings["ADSPath"]);

                string domainAndUsername = Domain + @"\" + txtUserName.Text.Trim();
                DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, txtPassword.Text.Trim());
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + txtUserName.Text.Trim() + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    path = result.Path;
                    string filterAttribute = (string)result.Properties["cn"][0];
                    lblMsg.Text += " <br/> exits <br/>";
                }
                else
                {
                    lblMsg.Text += "  <br/> not exits <br/> ";
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        public void changesPasswordAd(string userName)
        {
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName))
                {
                    user.SetPassword("newpassword");
                    // or
                    user.ChangePassword("oldPassword", "newpassword");
                }
            }
        }

        protected void btnSMSTest_Click(object sender, EventArgs e)
        {
            //try
            //{                
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/ybservice.svc/json/login";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        UserName = txtUserName.Text,
                        Password = txtPassword.Text,
                        UdId = "14258"
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var respsaonse = streamReader.ReadToEnd();
                    lblMsg.Text = respsaonse;
                }


            //}
            //catch (Exception ww)
            //{

            //    lblMsg.Text = ww.Message;
            //}
        }

        protected void btnOTPVerify_Click(object sender, EventArgs e)
        {
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/ybservice.svc/json/VerifyOTP";


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    Version = txtVersion.Text,
                    OTPKey = txtKey.Text,
                    OTPValue = txtVal.Text,
                    UserId = txtUserid.Text 
                });
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var respsaonse = streamReader.ReadToEnd();
                lblMsg.Text = respsaonse;
            }

         
        }


    }
}