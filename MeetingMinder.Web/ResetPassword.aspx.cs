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
using System.Text.RegularExpressions;
using System.Web.Helpers;


namespace MeetingMinder.Web
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            lblError.Visible = false;
            if (Request.UrlReferrer != null)
            {
                string prvPage = Request.UrlReferrer.ToString();

                if (Session["reset"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["reset"].ToString();
                    Info.Visible = true;
                    Session["reset"] = null;
                    ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!');window.location='Login.aspx';</script>'");

                }
            }
           else 
            if (!Page.IsPostBack)
            {
                //AntiForgery.Validate();
                //Guid Uid = Guid.Parse(Request.QueryString["ActivationToken"].ToString());
                string ActivationToken =Request.QueryString["ActivationToken"].ToString().Trim();
                Guid Uid = Guid.Parse(ActivationToken);
                DataTable dtForgotMail = new DataTable();
                dtForgotMail = UserDataProvider.Instance.GetUserByActivationCode(Uid);
                if (dtForgotMail.Rows.Count>0)
                {
                    DateTime ResetTime = Convert.ToDateTime(dtForgotMail.Rows[0]["PasswordResetDate"]);
                    if (ResetTime > DateTime.Now)
                    {
                       
                    }
                    else
                    {
                       
                        lblError.Visible = true;
                        lblError.Text = "Link Expired.";
                        LoginDetails.Visible = false;
                        btnCancel.Text = "Login";
                        btnSubmit.Visible = false;
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Link Invalid.";
                    LoginDetails.Visible = false;
                    btnCancel.Text = "Login";
                    btnSubmit.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Guid Uid = Guid.Parse(Request.QueryString["ActivationToken"].ToString());
            DataTable dtForgotMail = new DataTable();
            dtForgotMail = UserDataProvider.Instance.GetUserByActivationCode(Uid);
            if (dtForgotMail.Rows.Count > 0)
            {
                DateTime ResetTime = Convert.ToDateTime(dtForgotMail.Rows[0]["PasswordResetDate"]);
                if (ResetTime > DateTime.Now)
                {
                    UserAcess objUser = new UserAcess();
                    if (txtPassword.Text != "")
                    {
                        if (!objUser.isValidChar(txtPassword.Text))
                        {
                            txtPassword.Text = "";
                            lblError.Visible = true;
                            lblError.Text = "Please enter valid characters only.";
                            return;
                        }
                    }
                    else
                    {
                        if (txtPassword.Text == "")

                        {
                            lblError.Visible = true;
                            lblError.Text = "Please enter password";
                            return;
                        }
                    }

                    if (txtConfirmPassword.Text != "" )
                    {
                        if (!objUser.isValidChar(txtConfirmPassword.Text))
                        {
                            txtConfirmPassword.Text = "";
                            lblError.Visible = true;
                            lblError.Text = "Please enter valid characters only.";
                            return;
                        }
                    }
                    else
                    {
                        if (txtConfirmPassword.Text == "")
                        {
                            lblError.Visible = true;
                            lblError.Text = "Please enter confirm password name.";
                            return;
                        }
                    }

                    string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

                    Match match = Regex.Match(txtConfirmPassword.Text, passwordRegex);

                    if (!match.Success)
                    {
                        lblError.Text = " Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character.";
                        lblError.Visible = true;
                        return;
                    }
                    UserDomain objUserDomain = new UserDomain();
                    objUserDomain.Password = txtPassword.Text.Trim();
                    objUserDomain.UserId = Guid.Parse(Convert.ToString(dtForgotMail.Rows[0]["UserID"]));
                    objUserDomain.UpdatedBy = Guid.Parse(Convert.ToString(dtForgotMail.Rows[0]["UserID"]));

                    bool status = UserDataProvider.Instance.UpdateByEmailLink(objUserDomain);
                    if (status)
                    {
                        ((Label)Info.FindControl("lblName")).Text = " User password updated successfully.";

                        //  Session["reset"] = " User password updated successfully.";
                        // Response.StatusCode = 302;
                        //Response.AddHeader("Location", "resetpassword.aspx");
                        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Your password has been changed successfully!');window.location='Login.aspx';</script>'");

                        Info.Visible = true;
                        bool ActivationCodeStatus = UserDataProvider.Instance.UpdatePasswordResetLink(Uid);
                        if (ActivationCodeStatus)
                        {
                            LoginDetails.Visible = false;
                            btnCancel.Text = "Login";
                            btnSubmit.Visible = false;
                        }
                    }
            
                }
                else
                {
                    ltlMessage.Text = "Link Expired ";
                    txtPassword.Enabled = false;
                    txtConfirmPassword.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Link Invalid.";
                LoginDetails.Visible = false;
                btnCancel.Text = "Login";
                btnSubmit.Visible = false;
            }

           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}