using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Domain;
using MM.Data;
using System.Data;
using MM.Core;
using System.Net.Mail;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Net;

namespace MeetingMinder.Web
{
    public partial class UserDevices : System.Web.UI.Page
    {
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                BindIpadUsers();

                #region

                if (System.Web.HttpContext.Current.Session["EntityId"] == null)
                {
                    Response.Redirect("~/login.aspx");
                }

                #endregion
            }
        }

        /// <summary>
        /// Bind ipad all users 
        /// </summary>
        private void BindIpadUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllIpadUser().OrderBy(p => p.FirstName).ToList();
                grdUser.DataSource = objUser;
                grdUser.DataBind();
                if (objUser.Count > 0)
                {
                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }
                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
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
        /// Dropdonlist selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strUserId = ddlUser.SelectedValue;
                if (strUserId != "0")
                {
                    trUsers.Visible = true;
                    DataSet ds = UserDataProvider.Instance.GetUserDevices(strUserId);
                    grdDevices.DataSource = ds.Tables[0];
                    grdDevices.DataBind();
                }
                else
                {
                    trUsers.Visible = false;
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
        /// Gridview row command event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param
        protected void grdDevices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("lost"))
                {
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        string UdId = Convert.ToString(e.CommandArgument);
                        //LinkButton lnk = (LinkButton)(e.CommandSource);
                        //if (lnk.Text.ToLower().Equals("Whitelist"))
                        //{
                        //}

                        int id = UserDataProvider.Instance.InsertLostDevice(UdId);
                        if (id == 1)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Device whitelisted successfully.";
                            Info.Visible = true;
                            ddlUser_SelectedIndexChanged(sender, e);
                        }
                        else if (id > 0)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Device blacklisted successfully.";
                            Info.Visible = true;
                            ddlUser_SelectedIndexChanged(sender, e);
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblName")).Text = "Device whitelisting/blacklisting failed.";
                            Error.Visible = true;
                        }

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Success", Convert.ToString(UdId), ((Label)Error.FindControl("lblName")).Text.Trim());

                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Failed", "", "Sorry you are not maker");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
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
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdUser.PageIndex = e.NewPageIndex;
                BindIpadUsers();
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
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // delete user
                if (e.CommandName.ToLower().Equals("flush"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    UserAcess objUser = new UserAcess();
                    //check Delete permission
                    if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                    {


                        if (row != null)
                        {
                            LinkButton lnkFlushData = (LinkButton)row.FindControl("lnkFlushData");
                            if (lnkFlushData.Text.Trim().ToLower().Equals("flush data"))
                            {
                                string UserID = Convert.ToString(e.CommandArgument.ToString());
                                Int32 bStatus = UserDataProvider.Instance.InsertDeviceLostUser(Guid.Parse(UserID));
                                //if (bStatus > 0)
                                //{
                                ((Label)Info.FindControl("lblName")).Text = "Request submited successfully. Data will be deleted on online login of user in old device.";
                                Info.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Success", Convert.ToString(UserID), "Request submited successfully. Data will be deleted on online login of user in old device");


                                if (row != null)
                                {
                                    Label lblName = (Label)row.FindControl("lblUserName");
                                    Label lblEmail = (Label)row.FindControl("lblEmail");
                                    Label lblClientName = (Label)row.FindControl("lblClientName");

                                    txtSubject.Text = "Please change password or lock account having userid " + lblName.Text;
                                    txtcc.Text = lblEmail.Text;
                                    hdnPopUp.Value = "1";
                                    txtBody.Text = "Hello, \r\n\r\nI lost my ipad, so i request you to please change my account password or lock account. My userid is " + lblName.Text + ".\r\n\r\nWarm regards,\r\n" + lblClientName.Text;
                                    ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'> ShowPopUp();</script>'");
                                }
                            }
                            else
                            {

                                string UserID = Convert.ToString(e.CommandArgument.ToString());

                                string AuthenticationType = Convert.ToString(ConfigurationManager.AppSettings["AuthType"]);
                                if (!AuthenticationType.ToLower().Equals("ad"))
                                {
                                    ViewState["UserId"] = UserID;
                                    hdnPopUpPublish.Value = "1";
                                    ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'> ShowPopUpPublish();</script>'");
                                }
                                else
                                {
                                    bool status = UserDataProvider.Instance.UpdateDeviceLostUser(Guid.Parse(UserID));
                                    //if (status)
                                    //{
                                    ViewState["UserId"] = null;
                                    ((Label)Info.FindControl("lblName")).Text = "User status updated successfully";

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Success", Convert.ToString(UserID), "User status updated successfully");

                                    Info.Visible = true;
                                    BindIpadUsers();
                                    //}
                                    //else
                                    //{
                                    //    ((Label)Error.FindControl("lblError")).Text = "Error. User status updation failed.";
                                    //    Error.Visible = true;
                                    //}                                   
                                }
                            }
                        }



                        //}
                        //else
                        //{
                        //    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                        //    Error.Visible = true;
                        //}
                        BindIpadUsers();
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Failed", "", "Sorry access denied");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
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

        protected void btnEmailSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") + "/img/Uploads/Email.xml"))
                {
                    ((Label)Error.FindControl("lblError")).Text = "Email configuration setting missing";
                    Error.Visible = true;
                    return;
                }

                SmtpClient SMTPServer = new SmtpClient();
                string Password = "";
                XmlDocument xml = new XmlDocument();
                xml.Load(Server.MapPath(".") + "/img/Uploads/Email.xml");

                string EmailFrom = ConfigurationManager.AppSettings["Email"];
                string ToEmail = txtSenderEmail.Text;//objUser.EmailID1;
                XmlNodeList serverlist = xml.SelectNodes("//email");
                foreach (XmlNode servernodes in serverlist)
                {
                    EmailFrom = servernodes.SelectSingleNode("senderemail").InnerText;
                    Password = servernodes.SelectSingleNode("password").InnerText;
                    SMTPServer.Port = Convert.ToInt32(servernodes.SelectSingleNode("port").InnerText);
                    SMTPServer.Host = servernodes.SelectSingleNode("smtpclient").InnerText;
                    SMTPServer.EnableSsl = Convert.ToBoolean(servernodes.SelectSingleNode("ssl").InnerText);
                }
                if (ToEmail != "" && ToEmail != null)
                {
                    MailMessage objmail = new MailMessage();
                    MailAddress fromAddress = new MailAddress(EmailFrom, "iMeetings App Admin");
                    objmail.From = fromAddress;

                    objmail.To.Add(ToEmail.Trim());
                    string body = txtBody.Text.Replace("\r\n", "<br/>");
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

                    objmail.Headers.Add("NAME", "Admin");
                    objmail.From = fromAddress;
                    objmail.To.Add(ToEmail);

                    objmail.IsBodyHtml = true;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);
                    System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                    SMTPServer.Send(objmail);
                    objmail.To.Clear();
                    ((Label)Info.FindControl("lblName")).Text = "Request submited successfully. Data will be deleted on online login of user in old device. Email sent Successfully.";
                    Info.Visible = true;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "User Device", "Success", ToEmail, "Request submited successfully. Data will be deleted on online login of user in old device. Email sent Successfully");

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'> HidePopUp();</script>'");
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
        /// button submit Event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strOld = "";//txtOld.Text;
                string strNew = txtNew.Text;
                Guid UserId = Guid.Parse(Convert.ToString(ViewState["UserId"]));

                bool isChange = UserDataProvider.Instance.DeviceLostChangePassword(UserId, strNew);
                if (isChange)
                {
                    ((Label)Info.FindControl("lblName")).Text = "Password changed successfully";

                    if (Session["EntityId"] == null)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!');</script>'");
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!'); </script>'");
                    }
                    bool status = UserDataProvider.Instance.UpdateDeviceLostUser(UserId);
                    //if(status)
                    //{
                    ViewState["UserId"] = null;
                    ((Label)Info.FindControl("lblName")).Text = "User status updated successfully";
                    Info.Visible = true;
                    BindIpadUsers();
                    //}
                    //else
                    //{
                    //    ((Label)Error.FindControl("lblError")).Text = "Error. User status updation failed.";
                    //    Error.Visible = true;
                    //}

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Error. Please check your old password.";
                    Error.Visible = true;
                }
                Info.Visible = true;
                //txtOld.Text = "";
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
        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine(certificate);

            return true;
        }
    }
}