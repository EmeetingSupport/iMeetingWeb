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
using System.Configuration;
using System.Net.Mail;
using System.Web.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;

namespace MeetingMinder.Web
{
    public partial class UserMaster : System.Web.UI.Page
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

            if (Request.UrlReferrer != null)
            {
                string prvPage = Request.UrlReferrer.ToString();

                if (Session["Message"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["Message"].ToString();
                    Info.Visible = true;
                    Session["Message"] = null;
                }
            }

            //       Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                hdnUserVal.Value = EncryptionHelper.GetChar(16);

                //bind user list  to grid
                BindUsers();

                //bind entity list
                BindEntity();

                //Bind checkers list
                BindCheckers();

                //Bind suffix to drop down
                BindSuffix();

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (IsSuperAdmin)
                {
                    divIsSuperAdmin.Visible = true;
                    divDepartment.Visible = true;
                }
                else
                {
                    divIsSuperAdmin.Visible = false;
                    divDepartment.Visible = false;
                }


                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:block;");
                    rfvddUser.Enabled = true;
                    chkChecker.Enabled = false;
                    chkChecker.Checked = true;
                }
            }
            string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);
            if (AuthenticationType.ToLower().Equals("ad"))
            {
                divConfirmPassword.Visible = false;
                divPassword.Visible = false;
                rfvConfPassword.Enabled = false;

                //cfvConformPass.Enabled = false;
                //rgePassword.Enabled = false;
                rfvPassword.Enabled = false;
            }
            else
            {
                divConfirmPassword.Visible = true;
                divPassword.Visible = true;
                rfvConfPassword.Enabled = true;
                rfvPassword.Enabled = true;
                //cfvConformPass.Enabled = true;
                //rgePassword.Enabled = true;

            }

            string DeviceAuthType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
            if (DeviceAuthType.ToLower().Equals("ad"))
            {
                divConfirmPassword.Visible = false;
                divPassword.Visible = false;
                rfvConfPassword.Enabled = false;
                rfvPassword.Enabled = false;
                //cfvConformPass.Enabled = false;
                //rgePassword.Enabled = false;
            }
            else if (!DeviceAuthType.ToLower().Equals("ad") && AuthenticationType.ToLower().Equals("ad"))
            {
                chkIpad.Attributes.Add("onclick", "showPassword()");
                divPassword.Attributes.Add("style", "display:none");
                divConfirmPassword.Attributes.Add("style", "display:none");
                divConfirmPassword.Visible = true;
                divPassword.Visible = true;
                rfvConfPassword.Enabled = false;
                rfvPassword.Enabled = false;
                //cfvConformPass.Enabled = false;
                //rgePassword.Enabled = false;
                lblPassword.InnerHtml = "iPad Password<span>*</span>";
                lblConfPassword.InnerHtml = "iPad Confirm Password<span>*</span>";
            }
        }

        /// <summary>
        /// Bind suffix to drop down list
        /// </summary>
        private void BindSuffix()
        {
            string strSuffix = ConfigurationManager.AppSettings["Suffix"].ToString();
            string[] Suffix = strSuffix.Split(',');
            if (Suffix.Count() > 0)
            {
                ddlSuffix.DataSource = Suffix;
                ddlSuffix.DataBind();


            }
        }

        /// <summary>
        /// Check Email id existance in database.
        /// </summary>
        /// <param name="Email">string specifying Email</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static string CheckEmail(string Email)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                {
                    return "Error";
                }
                string strEmail = "";
                Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                if (Email != "")
                {
                    int Count = UserDataProvider.Instance.CheckEmailExistance(Email, UserId);
                    if (Count > 0)
                    {
                        //  strEmail = "<span style='color:red'> Email id already exists </span>";
                    }
                    else
                    {
                        strEmail = "";
                    }
                }
                return strEmail;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Check userName existance 
        /// </summary>
        /// <param name="Email">string specifying Email</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static string CheckUserName(string UserName)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                {
                    return "Error";
                }
                string strEmail = "";
                if (UserName != "")
                {
                    Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                    int Count = UserDataProvider.Instance.CheckUserNameExistance(UserName.ToLower(), UserId);
                    if (Count > 0)
                    {
                        //strEmail = "<span style='color:red'> UserName already exists </span>";
                        Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());
                        int Department = UserDataProvider.Instance.CheckUserIdDepartmentExitance(UserName.ToLower(), EntityId);
                        if (Department > 0)
                        {
                            strEmail = "<span style='color:red;margin-left:265px;'> UserName already exists </span>";
                        }
                        else
                        {
                            strEmail = "<span style='color:red;margin-left:265px;'> UserName already exists </span><div style='font-size:12px; margin-left:315px'><strong><a style='color:#3f79c1;' href='javascript:void(0)' onclick='DepartmentFun()'>Add department</a></strong></div>";
                        }
                    }
                    else
                    {
                        strEmail = "";
                    }
                }
                return strEmail;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        [WebMethod(EnableSession = true)]
        public static string AddCurrentDepartment(string UserName)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                {
                    return "Error";
                }
                string strEmail = "";
                if (UserName != "")
                {
                    Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                    Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());

                    //int Count = UserDataProvider.Instance.CheckUserNameExistance(UserName.ToLower(), UserId);
                    //if (Count > 0)
                    //{

                    //}
                    //else
                    //{
                    //    strEmail = "";
                    //}
                }
                return strEmail;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Bind User List to drop down
        /// </summary>
        private void BindCheckers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker().OrderBy(p => p.FirstName).ToList();
                //Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                //IList<UserDomain> objUser = UserDataProvider.Instance.GetAllCheckerByEntity(EntityId);
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
                //Remove logged in users id from drop down
                string UserId = Convert.ToString(Session["UserId"]);
                ListItem UserIdToRemove = ddlUser.Items.FindByValue(UserId);
                ddlUser.Items.Remove(UserIdToRemove);

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
        /// Bind Entity names to drop down
        /// </summary>
        private void BindEntity()
        {
            try
            {
                //Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                //ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("Default.aspx") + "' style='text-decoration:none;'>Home</a>&nbsp;";
                string entityId = Session["EntityId"].ToString();
                ddlEntityList.DataSource = EntityDataProvider.Instance.Get().OrderBy(p => p.EntityName).ToList();
                ddlEntityList.DataBind();
                ddlEntityList.DataTextField = "EntityName";
                ddlEntityList.DataValueField = "EntityId";
                ddlEntityList.DataBind();

                foreach (ListItem lstItem in ddlEntityList.Items)
                {
                    if (lstItem.Value.ToLower().Equals(entityId))
                    {
                        lstItem.Selected = true;
                    }
                }
                //ddlEntityList.Items.Insert(0, new ListItem("Select Entity", "-1"));
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
        /// Bind User grid
        /// </summary>
        private void BindUsers()
        {
            try
            {
                Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home</a>&nbsp;";
                // Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                IList<UserDomain> objUserList = UserDataProvider.Instance.Get().OrderBy(p => p.FirstName).ToList();
                // IList<UserDomain> objUserList = UserDataProvider.Instance.GetByEntity(EntityId);


                //hide super Admin edit and delete button
                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (!IsSuperAdmin)
                {
                    for (int i = 0; i < objUserList.Count; i++)
                    {
                        if (objUserList[i].IsSuperAdmin)
                        {
                            objUserList[i].hideButton = true;
                        }
                    }
                }




                grdUser.DataSource = objUserList;
                grdUser.DataBind();

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
        /// Insert or edit values 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            SendEmail objSendEmail = new SendEmail();
            try
            {
                string strPassword = string.Empty;
                string strConfirmPassword = string.Empty;
                if (txtPassword.Enabled)
                {
                    strPassword = DecryptStringAES(txtPassword.Text, hdnUserVal.Value);
                    strConfirmPassword = DecryptStringAES(txtConformPass.Text, hdnUserVal.Value);
                    txtPassword.Text = strPassword;
                    txtConformPass.Text = strConfirmPassword;
                }

                UserAcess objUser = new UserAcess();
                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (btnInsert.Text.ToLower() == "update")
                {
                    if (!IsSuperAdmin)
                    {
                        txtPassword.Enabled = false;
                        txtConformPass.Enabled = false;

                        rfvConfPassword.Enabled = false;
                        rfvPassword.Enabled = false;
                        //cfvConformPass.Enabled = false;
                        //rgePassword.Enabled = false;
                    }
                }

                if (txtUserName.Text != "")
                {
                    if (!objUser.isValidChar(txtUserName.Text))
                    {
                        txtUserName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    if (!objUser.CSVValidation(txtUserName.Text))
                    {
                        txtUserName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid username.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter username";
                    Error.Visible = true;
                    return;
                }

                if (txtFirstName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtFirstName);
                    if (!objUser.isValidChar(txtFirstName.Text))
                    {
                        txtFirstName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtFirstName.Text))
                    {
                        txtUserName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid first name.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter first name";
                    Error.Visible = true;
                    return;
                }

                string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);
                if (!AuthenticationType.ToLower().Equals("ad"))
                {
                    if (txtPassword.Enabled)
                    {
                        if (txtPassword.Text != "" && txtPassword.Text != "!!1A ***** b1!!")
                        {
                            if (!objUser.isValidChar(txtPassword.Text))
                            {
                                txtPassword.Text = "";
                                ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (txtPassword.Text == "")
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Please enter password";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }

                    if (txtConformPass.Enabled)
                    {
                        if (txtConformPass.Text != "" && txtConformPass.Text != "!!1A ***** b1!!")
                        {
                            if (!objUser.isValidChar(txtConformPass.Text))
                            {
                                txtConformPass.Text = "";
                                ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (txtConformPass.Text == "")
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Please enter confirm password";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }

                }

                string DeviceAuthType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
                if (!DeviceAuthType.ToLower().Equals("ad") && chkIpad.Checked)
                {
                    if (txtPassword.Enabled)
                    {
                        if (txtPassword.Text != "" && txtPassword.Text != "!!1A ***** b1!!")
                        {
                            if (!objUser.isValidChar(txtPassword.Text))
                            {
                                txtPassword.Text = "";
                                ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (txtPassword.Text == "")
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Please enter password";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }

                    if (txtConformPass.Enabled)
                    {
                        if (txtConformPass.Text != "" && txtConformPass.Text != "!!1 ***** 1!!")
                        {
                            if (!objUser.isValidChar(txtConformPass.Text))
                            {
                                txtConformPass.Text = "";
                                ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (txtConformPass.Text == "")
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Please enter confirm password name";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }
                }

                if (txtMiddleName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtMiddleName);
                    if (!objUser.isValidChar(txtMiddleName.Text))
                    {
                        txtMiddleName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtMiddleName.Text))
                    {
                        txtMiddleName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid middle name.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtLastName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtLastName);
                    if (!objUser.isValidChar(txtLastName.Text))
                    {
                        txtLastName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtLastName.Text))
                    {
                        txtLastName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid last name.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtDesignation.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtDesignation);
                    if (!objUser.isValidChar(txtDesignation.Text))
                    {
                        txtDesignation.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtDesignation.Text))
                    {
                        txtDesignation.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid designation.";
                        Error.Visible = true;
                        return;
                    }
                }

                string strPanNo = DecryptStringAES(hdnPanNo.Value, hdnUserVal.Value);

                if (strPanNo != "")
                {
                    objUser.TextHtmlEncode(ref strPanNo);
                    if (strPanNo.Length > 10)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid pan no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(strPanNo))
                    {
                        txtPanNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(strPanNo))
                    {
                        txtPanNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid pan no.";
                        Error.Visible = true;
                        return;
                    }
                }

                string strPassportNumber = DecryptStringAES(hdnPassport.Value, hdnUserVal.Value);

                if (strPassportNumber != "")
                {
                    objUser.TextHtmlEncode(ref strPassportNumber);
                    if (strPassportNumber.Length > 10)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid passport no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(strPassportNumber))
                    {
                        txtPassportNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(strPassportNumber))
                    {
                        txtPassportNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid passport no.";
                        Error.Visible = true;
                        return;
                    }
                }

                //if (txtPanNo.Text != "")
                //{
                //    objUser.TextHtmlEncode(ref txtPanNo);
                //    if (txtPanNo.Text.Length > 10)
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = " Please enter valid pan no.";
                //        Error.Visible = true;
                //        return;
                //    }
                //    if (!objUser.isValidChar(txtPanNo.Text))
                //    {
                //        txtPanNo.Text = "";
                //        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                //        Error.Visible = true;
                //        return;
                //    }
                //    if (!objUser.CSVValidation(txtPanNo.Text))
                //    {
                //        txtPanNo.Text = "";
                //        ((Label)Error.FindControl("lblError")).Text = " Please enter valid pan no.";
                //        Error.Visible = true;
                //        return;
                //    }
                //}

                if (txtDinNo.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtDinNo);
                    if (txtDinNo.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid din no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtDinNo.Text))
                    {
                        txtDinNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtDinNo.Text))
                    {
                        txtDinNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid din no.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtAddress.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtAddress);
                    if (!objUser.isValidChar(txtAddress.Text))
                    {
                        txtAddress.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtAddress.Text))
                    {
                        txtAddress.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid residential address";
                        Error.Visible = true;
                        return;
                    }
                }

                //string strRegexContact = @"^[0-9]{1,12}$";
                string strRegexContact = @"^[0-9]{1,13}$";
                Regex RegexContact = new Regex(strRegexContact, RegexOptions.None);
                if (txtContactNumber.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtContactNumber);
                    if (!objUser.isValidChar(txtContactNumber.Text))
                    {
                        txtContactNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtContactNumber.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid contact no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtContactNumber.Text))
                    {
                        txtContactNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    Match match = Regex.Match(txtContactNumber.Text, strRegexContact);

                    if (!match.Success)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid contact no.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtMobileNumebr.Text != "")
                {

                    if (!objUser.isValidChar(txtMobileNumebr.Text))
                    {
                        txtMobileNumebr.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtMobileNumebr.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid mobile no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtMobileNumebr.Text))
                    {
                        txtMobileNumebr.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    Match match = Regex.Match(txtMobileNumebr.Text, strRegexContact);

                    if (!match.Success)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid mobile no.";
                        Error.Visible = true;
                        return;
                    }
                }
                //else
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Please enter Moblie Number";
                //    Error.Visible = true;
                //    return;
                //}


                if (txtOfficeAddress.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtOfficeAddress);
                    if (!objUser.isValidChar(txtOfficeAddress.Text))
                    {
                        txtOfficeAddress.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtOfficeAddress.Text))
                    {
                        txtOfficeAddress.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid office address";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtOfficeContactNo.Text != "")
                {

                    if (!objUser.isValidChar(txtOfficeContactNo.Text))
                    {
                        txtOfficeContactNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtOfficeContactNo.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid office contact no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtOfficeContactNo.Text))
                    {
                        txtOfficeContactNo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    Match match = Regex.Match(txtOfficeContactNo.Text, strRegexContact);

                    if (!match.Success)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid office contact no.";
                        Error.Visible = true;
                        return;
                    }

                }

                string strRegexEmail = @"((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;))(,\s*((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)|(['\&quot;][^\&lt;\&gt;'\&quot;]*['\&quot;]\s*\&lt;\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\&gt;)))*";
                Regex RegexEmail = new Regex(strRegexEmail, RegexOptions.None);

                if (txtEmail.Text != "")
                {
                    if (!objUser.isValidChar(txtEmail.Text))
                    {
                        txtEmail.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtEmail.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid email address.";
                        Error.Visible = true;
                        return;
                    }

                    try
                    {
                        MailAddress m = new MailAddress(txtEmail.Text);
                    }
                    catch (FormatException)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid email address.";
                        Error.Visible = true;
                        return;
                    }

                }
                //else
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Please enter email Id.";
                //    Error.Visible = true;
                //    return;
                //}

                if (txtEmailTwo.Text != "")
                {
                    if (!objUser.isValidChar(txtEmailTwo.Text))
                    {
                        txtEmailTwo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtEmailTwo.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid alternating email address.";
                        Error.Visible = true;
                        return;
                    }

                    try
                    {
                        MailAddress m = new MailAddress(txtEmailTwo.Text);
                    }
                    catch (FormatException)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid alternating email address.";
                        Error.Visible = true;
                        return;
                    }
                }
                if (txtSecretoryName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtSecretoryName);
                    if (!objUser.isValidChar(txtSecretoryName.Text))
                    {
                        txtSecretoryName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSecretoryName.Text))
                    {
                        txtSecretoryName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretory name.";
                        Error.Visible = true;
                        return;
                    }
                }
                if (txtSecreroryResiCont.Text != "")
                {
                    if (!objUser.isValidChar(txtSecreroryResiCont.Text))
                    {
                        txtSecreroryResiCont.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtSecreroryResiCont.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary residential contact no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSecreroryResiCont.Text))
                    {
                        txtSecreroryResiCont.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    foreach (Match myMatch in RegexContact.Matches(txtSecreroryResiCont.Text))
                    {
                        if (myMatch.Success)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary residential contact no.";
                            Error.Visible = true;
                            return;
                        }
                    }
                }

                if (txtSecreroryOffiCont.Text != "")
                {
                    if (!objUser.isValidChar(txtSecreroryOffiCont.Text))
                    {
                        txtSecreroryOffiCont.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtSecreroryOffiCont.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary office contact no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSecreroryOffiCont.Text))
                    {
                        txtSecreroryOffiCont.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    Match match = Regex.Match(txtSecreroryOffiCont.Text, strRegexContact);

                    if (!match.Success)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary office contact no.";
                        Error.Visible = true;
                        return;
                    }

                }

                if (txtSecretoryMobile.Text != "")
                {
                    if (!objUser.isValidChar(txtSecretoryMobile.Text))
                    {
                        txtSecretoryMobile.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtSecretoryMobile.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary mobile no.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSecretoryMobile.Text))
                    {
                        txtSecretoryMobile.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    Match match = Regex.Match(txtSecretoryMobile.Text, strRegexContact);

                    if (!match.Success)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary mobile no.";
                        Error.Visible = true;
                        return;
                    }
                }
                if (txtSecretoryEmail.Text != "")
                {
                    if (!objUser.isValidChar(txtSecretoryEmail.Text))
                    {
                        txtSecretoryEmail.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtSecretoryEmail.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary email address.";
                        Error.Visible = true;
                        return;
                    }
                    try
                    {
                        MailAddress m = new MailAddress(txtSecretoryEmail.Text);
                    }
                    catch (FormatException)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary email address.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtSecretoryEmailTwo.Text != "")
                {
                    if (!objUser.isValidChar(txtSecretoryEmailTwo.Text))
                    {
                        txtSecretoryEmailTwo.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtSecretoryEmailTwo.Text.Length > 149)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary alternative email address.";
                        Error.Visible = true;
                        return;
                    }

                    try
                    {
                        MailAddress m = new MailAddress(txtSecretoryEmailTwo.Text);
                    }
                    catch (FormatException)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid secretary alternative email address.";
                        Error.Visible = true;
                        return;
                    }
                }

                UserDomain objUserDomain = new UserDomain();
                objUserDomain.UserName = txtUserName.Text.Trim().ToLower();
                objUserDomain.Suffix = ddlSuffix.SelectedValue;
                objUserDomain.Designation = txtDesignation.Text;
                objUserDomain.DINNumber = txtDinNo.Text;
                objUserDomain.EmailID1 = txtEmail.Text;
                objUserDomain.EmailID2 = txtEmailTwo.Text;
                // objUserDomain.FoodPreference = txtFoodPreference.Text;

                objUserDomain.FirstName = txtFirstName.Text;
                objUserDomain.LastName = txtLastName.Text;
                objUserDomain.Mobile = txtMobileNumebr.Text;
                objUserDomain.OfficeAddress = txtOfficeAddress.Text;
                objUserDomain.OfficePhone = txtOfficeContactNo.Text;
                //objUserDomain.PANNo = txtPanNo.Text;               
                //objUserDomain.PassportNumber = txtPassportNumber.Text;

                objUserDomain.PANNo = strPanNo;
                objUserDomain.PassportNumber = strPassportNumber;

                //objUserDomain.Password = txtConformPass.Text;

                if (txtPassword.Enabled)
                {
                    if (txtPassword.Text != "!!1A ***** b1!!")
                    {
                        string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$";

                        Match match = Regex.Match(txtPassword.Text, passwordRegex);

                        if (!match.Success)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Password must contain: Minimum 8 and Maximum 15 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character.";
                            Error.Visible = true;
                            return;
                        }

                        if (txtPassword.Text == txtUserName.Text)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Password is the same as username. This is no longer allowed.";
                            Error.Visible = true;
                            return;
                        }

                        if (!objUser.CSVValidation(txtPassword.Text))
                        {
                            txtPassword.Text = "";
                            ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                            Error.Visible = true;
                            return;
                        }

                        objUserDomain.Password = txtPassword.Text.Trim();
                    }
                    else
                    {
                        objUserDomain.Password = Convert.ToString(ViewState["pswd"]);
                    }
                }
                else
                {
                    objUserDomain.Password = Convert.ToString(ViewState["pswd"]);
                }

                if (txtPassword.Text.Trim() != txtConformPass.Text.Trim())
                {
                    ((Label)Error.FindControl("lblError")).Text = " Password not match";
                    Error.Visible = true;
                    return;
                }
                
                //if (chkIpad.Checked && txtPassword.Text.Length == 0)
                //{
                //    ((Label)Error.FindControl("lblError")).Text = "Please set password for  ";
                //    Error.Visible = true;
                //    return;
                //}

                objUserDomain.ResidencePhone = txtContactNumber.Text;
                objUserDomain.ResidentialAddress = txtAddress.Text;
                objUserDomain.SecretaryEmailID1 = txtSecretoryEmail.Text;
                objUserDomain.SecretaryEmailID2 = txtSecretoryEmailTwo.Text;
                objUserDomain.SecretaryMobile = txtSecretoryMobile.Text;
                objUserDomain.SecretaryName = txtSecretoryName.Text;
                objUserDomain.SecretaryOfficePhone = txtSecreroryOffiCont.Text;
                objUserDomain.SecretaryResidentalPhone = txtSecreroryResiCont.Text;
                objUserDomain.UserId = Guid.NewGuid();
                objUserDomain.MiddleName = txtMiddleName.Text.Trim();
                string SendToChecker;
                if (chkChecker.Checked)
                {
                    SendToChecker = "Pending";
                }
                else
                {
                    SendToChecker = "Approved";
                }

                if (rdbCheckerNo.Checked)
                {
                    objUserDomain.IsChecker = false;
                }
                if (rdbCheker.Checked)
                {
                    objUserDomain.IsChecker = true;
                }

                if (rdbMaker.Checked)
                {
                    objUserDomain.IsMaker = true;
                }

                if (rdbMakerNo.Checked)
                {
                    objUserDomain.IsMaker = false;
                }
                if (chkIpad.Checked == true)
                {
                    objUserDomain.IsEnabledOnIpad = true;
                }
                if (chkWebApp.Checked == true)
                {

                    objUserDomain.IsEnabledOnWebApp = true;
                }

                if (chkIsSuperAdmin.Checked)
                    objUserDomain.IsSuperAdmin = true;
                else
                    objUserDomain.IsSuperAdmin = false;

                //  objUserDomain.DefaultUser = chkDefaultUser.Checked;

                Guid CheckerId;
                if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                {
                    objUserDomain.UserChecker = CheckerId;
                }
                objUserDomain.UserChecker = CheckerId;
                // objUserDomain.UserChecker = Guid.Parse(ddlUser.SelectedValue);

                //insert default image name
                string imageName = "sample_avatar.jpg";



                string UserId = Convert.ToString(Guid.NewGuid());
                StringBuilder strEntity = new StringBuilder(",");
                StringBuilder strDelete = new StringBuilder(",");

                objUserDomain.CreatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                objUserDomain.UpdatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));

                if (objUserDomain.CreatedBy == objUserDomain.UserChecker)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                    Error.Visible = true;
                    return;
                }
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    //Update user
                    if (hdnUserId.Value != null && hdnUserId.Value != "")
                    {
                        if (fuPhotograph.HasFile)
                        {
                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ImagePath"]);
                            string contentType = fuPhotograph.PostedFile.ContentType;
                            string extn = Path.GetExtension(fuPhotograph.FileName);
                            if (!objUser.isValidChar_old(fuPhotograph.FileName))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                Error.Visible = true;
                                return;
                            }

                            if (contentType.ToLower().Contains("image/"))
                            {
                                if (extn.ToLower().Equals(".jpg") || extn.ToLower().Equals(".jpeg") || extn.ToLower().Equals(".png") || extn.ToLower().Equals(".bmp"))
                                {
                                    System.IO.BinaryReader r = new System.IO.BinaryReader(fuPhotograph.PostedFile.InputStream);
                                    string fileclass = "";
                                    byte buffer;
                                    try
                                    {
                                        buffer = r.ReadByte();
                                        fileclass = buffer.ToString();
                                        buffer = r.ReadByte();
                                        fileclass += buffer.ToString();

                                    }
                                    catch
                                    {

                                    }

                                    if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780")
                                    {
                                        string ext = Path.GetExtension(fuPhotograph.FileName);

                                        imageName = DateTime.Now.Ticks + ext;
                                        fuPhotograph.PostedFile.SaveAs(Server.MapPath(savePath + imageName));

                                        if (ViewState["file"] != null)
                                        {
                                            if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                                            {
                                                if (hdnUserId.Value != null || hdnUserId.Value != "")
                                                {
                                                    ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                                    Error.Visible = true;
                                                    return;
                                                }
                                            }
                                            string fileName = Convert.ToString(ViewState["file"]);
                                            if (fileName != "sample_avatar.jpg")
                                                File.Delete(Server.MapPath(savePath + fileName));
                                            ViewState["file"] = null;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                        Error.Visible = true;
                                        return;
                                    }
                                    r.Close();
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                    Error.Visible = true;
                                    return;
                                }

                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid image Uploaded file";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (ViewState["file"] != null)
                            {
                                imageName = Convert.ToString(ViewState["file"]);
                            }
                        }

                        bool isEntitySelected = false;

                        if (ViewState["selectedEntity"] != null)
                        {
                            List<string> selectedEntity = (List<string>)ViewState["selectedEntity"];

                            foreach (ListItem lstItem in ddlEntityList.Items)
                            {
                                if (lstItem.Selected)
                                {
                                    isEntitySelected = true;
                                    if (!selectedEntity.Contains(lstItem.Value))        //if value not exist in current list add it
                                    {
                                        //                                    strEntity.Append(@" INSERT INTO UserEntity
                                        //           ( UserEntityId, UserId, EntityId, CreatedBy, UpdatedBy, CreatedOn ,UpdatedOn)
                                        //     VALUES (newid(),'" + hdnUserId.Value + @"' , '" + lstItem.Value + "','" + objUserDomain.CreatedBy + "', '" + objUserDomain.UpdatedBy + "', getdate(), getdate()) ");
                                        strEntity.Append(lstItem.Value + ",");
                                    }
                                    else
                                    {
                                        selectedEntity.Remove(lstItem.Value);
                                    }
                                }
                            }

                            if (selectedEntity.Count != 0)
                            {
                                foreach (var item in selectedEntity)
                                {
                                    //Delete previous not selected entity
                                    // strEntity.Append(@" DELETE FROM UserEntity WHERE EntityID = '"+item.ToString() +"' and UserId = '"+hdnUserId.Value+"' ");
                                    strDelete.Append(item.ToString() + ",");
                                }
                            }
                        }
                        else
                        {
                            //Insert Multiple entity into entity table
                            //foreach (ListItem lstItem in ddlEntityList.Items)
                            //{
                            //    if (lstItem.Selected)
                            //    {                                 
                            //        strEntity.Append(lstItem.Value + ",");
                            //    }
                            //}

                            strEntity.Append(Session["EntityId"].ToString() + ",");
                        }

                        if (ViewState["adminEntity"] != null)
                        {
                            isEntitySelected = true;
                            strEntity.Append(Session["EntityId"].ToString() + ",");
                        }

                        if (!isEntitySelected)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Please select atleast one department  ";
                            Error.Visible = true;
                            return;

                        }
                        objUserDomain.Photograph = imageName;
                        objUserDomain.UpdatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                        objUserDomain.UserId = Guid.Parse(hdnUserId.Value);

                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Failed", Convert.ToString(objUserDomain.UserId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }


                        bool status = UserDataProvider.Instance.Update(objUserDomain, strEntity.ToString(), strDelete.ToString(), SendToChecker);
                        if (status)
                        {
                            if (SendToChecker == "Pending")
                            {
                                //UserDomain objUser = UserDataProvider.Instance.Get(ddlUser.SelectedValue);
                                //if (objUser != null)
                                //{

                                //    lblmsg.Visible = true;
                                //}

                                if (chkNotifyChecker.Checked)
                                {
                                    bool isSuccess;
                                    string Message;
                                    //SendEmail objSendEmail = new SendEmail();
                                    objSendEmail.SendNotifyEmail("User", objUserDomain.UserChecker, out isSuccess, out Message);

                                    if (isSuccess)
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = " User updated successfully and sent for approval with email notification";
                                        Info.Visible = true;
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = " User updated successfully and sent for approval.";
                                        ((Label)Error.FindControl("lblError")).Text = Message;
                                        Info.Visible = true;
                                        Error.Visible = true;

                                    }

                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUserDomain);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Pending", Convert.ToString(objUserDomain.UserId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                }
                                else
                                {
                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUserDomain);
                                    var encrypt = Encryptor.EncryptString(json);
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Pending", Convert.ToString(objUserDomain.UserId), "User updated successfully and sent for approval :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = " User updated successfully and sent for approval";
                                    Info.Visible = true;
                                }
                            }
                            else
                            {
                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUserDomain);
                                var encrypt = Encryptor.EncryptString(json);
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Success", Convert.ToString(objUserDomain.UserId), "User updated successfully :- " + encrypt + "");

                                ((Label)Info.FindControl("lblName")).Text = " User updated successfully";
                                Info.Visible = true;
                            }
                            BindUsers();
                            ClearFields();
                            //  Info.Visible = true;
                            Response.StatusCode = 302;
                            Response.AddHeader("Location", "UserMaster.aspx");
                            Session["Message"] = ((Label)Info.FindControl("lblName")).Text;
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Failed", Convert.ToString(objUserDomain.UserId), "User updation failed Username already exists");

                            ((Label)Error.FindControl("lblError")).Text = "User updation failed Username already exists..";
                            Error.Visible = true;
                        }
                    }
                    //insert user
                    else
                    {

                        //check add permission
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            if (hdnUserId.Value == null || hdnUserId.Value == "")
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;
                                return;
                            }
                        }

                        if (fuPhotograph.HasFile)
                        {
                            if (!objUser.isValidChar_old(fuPhotograph.FileName))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                Error.Visible = true;
                                return;
                            }

                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ImagePath"]);
                            string contentType = fuPhotograph.PostedFile.ContentType;
                            if (contentType.ToLower().Contains("image/"))
                            {
                                string extn = Path.GetExtension(fuPhotograph.FileName);
                                if (extn.ToLower().Equals(".jpg") || extn.ToLower().Equals(".jpeg") || extn.ToLower().Equals(".png") || extn.ToLower().Equals(".bmp"))
                                {
                                    System.IO.BinaryReader r = new System.IO.BinaryReader(fuPhotograph.PostedFile.InputStream);
                                    string fileclass = "";
                                    byte buffer;
                                    try
                                    {
                                        buffer = r.ReadByte();
                                        fileclass = buffer.ToString();
                                        buffer = r.ReadByte();
                                        fileclass += buffer.ToString();
                                    }
                                    catch
                                    {

                                    }

                                    if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780")
                                    {
                                        string ext = Path.GetExtension(fuPhotograph.FileName);

                                        imageName = DateTime.Now.Ticks + ext;
                                        fuPhotograph.PostedFile.SaveAs(Server.MapPath(savePath + imageName));

                                        if (ViewState["file"] != null)
                                        {
                                            string fileName = Convert.ToString(ViewState["file"]);
                                            if (fileName != "sample_avatar.jpg")
                                                File.Delete(Server.MapPath(savePath + fileName));
                                            ViewState["file"] = null;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                        Error.Visible = true;
                                        return;
                                    }
                                    r.Close();
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                    Error.Visible = true;
                                    return;
                                }

                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid image Uploaded file";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            if (ViewState["file"] != null)
                            {
                                imageName = Convert.ToString(ViewState["file"]);
                            }
                        }
                        bool isEntitySelected = false;
                        //Insert Multiple entity into entity table
                        foreach (ListItem lstItem in ddlEntityList.Items)
                        {
                            if (lstItem.Selected)
                            {
                                //                            strEntity.Append(@" INSERT INTO UserEntity
                                //           ( UserEntityId, UserId, EntityId, CreatedBy, UpdatedBy, CreatedOn ,UpdatedOn)
                                //     VALUES (newid(),'" + UserId + @"' , '" + lstItem.Value + "','" + objUserDomain.CreatedBy + "', '" + objUserDomain.UpdatedBy + "', getdate(), getdate()) ");
                                isEntitySelected = true;
                                strEntity.Append(lstItem.Value + ",");
                            }
                        }

                        //     strEntity.Append(Session["EntityId"].ToString() + ",");
                        objUserDomain.Photograph = imageName;

                        //Ad Auth
                        //string AdDomain = Convert.ToString(ConfigurationManager.AppSettings["AdDomain"]);
                        //bool IsValid = UserAuthenticate.CheckExists(AdDomain, objUserDomain.UserName);
                        //if (!IsValid)
                        //{
                        //    ((Label)Error.FindControl("lblError")).Text = " UserId does not exist in Active directory. ";
                        //    Error.Visible = true;
                        //    return;
                        //}


                        if (!isEntitySelected)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Please select atleast one entiity  ";
                            Error.Visible = true;
                            return;
                        }

                        objUserDomain = UserDataProvider.Instance.Insert(objUserDomain, strEntity.ToString(), Guid.Parse(UserId), SendToChecker);
                        if (objUserDomain.UserId.ToString().Equals("11000000-0000-0000-0000-000000000011"))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Failed", Convert.ToString(objUserDomain.UserId), "User insertion failed Username already exists");

                            ((Label)Error.FindControl("lblError")).Text = "User insertion failed Username already exists..";
                            Error.Visible = true;
                            return;
                        }
                        if (SendToChecker == "Pending")
                        {
                            if (chkNotifyChecker.Checked)
                            {
                                bool isSuccess;
                                string Message;
                                //SendEmail objSendEmail = new SendEmail();
                                objSendEmail.SendNotifyEmail("User", objUserDomain.UserChecker, out isSuccess, out Message);

                                if (isSuccess)
                                {
                                    ((Label)Info.FindControl("lblName")).Text = "User inserted successfully and sent for approval with email notification";
                                    Info.Visible = true;

                                }
                                else
                                {
                                    ((Label)Info.FindControl("lblName")).Text = "User inserted successfully and sent for approval. ";
                                    ((Label)Error.FindControl("lblError")).Text = Message;
                                    Info.Visible = true;
                                    Error.Visible = true;
                                }
                            }
                            else
                            {
                                ((Label)Info.FindControl("lblName")).Text = "User inserted successfully and sent for approval. ";
                                Info.Visible = true;

                            }

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUserDomain);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Pending", Convert.ToString(objUserDomain.UserId), ((Label)Info.FindControl("lblName")).Text.Trim() + ":- " + encrypt);
                        }
                        else
                        {
                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUserDomain);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Success", Convert.ToString(objUserDomain.UserId), "User inserted successfully :- " + encrypt + "");

                            ((Label)Info.FindControl("lblName")).Text = "User inserted successfully";
                            Info.Visible = true;
                            objSendEmail.SendUserMail(objUserDomain.UserId);
                        }
                        BindUsers();
                        ClearFields();
                        Response.StatusCode = 302;
                        Response.AddHeader("Location", "UserMaster.aspx");
                        Session["Message"] = ((Label)Info.FindControl("lblName")).Text;
                        //Info.Visible = true;
                        // SendEmail(objUserDomain.EmailID1, objUserDomain.UserName, objUserDomain.Password);
                    }


                }

                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUserDomain.CreatedBy), "User Master", "Failed", Convert.ToString(objUserDomain.UserId), "Sorry you are not maker");

                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                    Error.Visible = true;
                }

                //updatePanel.Update();
            }

            catch (Exception ex)
            {
                //// Get stack trace for the exception with source file information
                //var st = new StackTrace(Ex, true);
                //// Get the top stack frame
                //var frame = st.GetFrame(0);
                //// Get the line number from the stack frame
                //var line = frame.GetMethod();
                //if (line.ToString().ToLower().Equals("insert"))
                //{
                //((Label)Error.FindControl("lblError")).Text = "User insertion failed check email or umsername";
                //Error.Visible = true;
                //}
                ((Label)Error.FindControl("lblError")).Text = "User insertion failed";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }

        }
        /// <summary>
        /// Insertion or Updation cancel
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearFields();
            Response.StatusCode = 302;
            Response.AddHeader("Location", "UserMaster.aspx");
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

                if (e.CommandName.ToLower().Equals("unlock"))
                {
                    UserAcess objUser = new UserAcess();

                    //check Delete permission
                    if (objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        ClearFields();
                        string UserID = Convert.ToString(e.CommandArgument.ToString());
                        bool bStatus = UserDataProvider.Instance.UnlockAccount(Guid.Parse(UserID));
                        if (bStatus == false)
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Success", UserID, "User unlocked successfully");

                            ((Label)Info.FindControl("lblName")).Text = "User unlocked successfully";
                            Info.Visible = true;
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Failed", UserID, "User unlocked failed");

                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                        BindUsers();
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Failed", "", "Sorry access denied");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }

                }



                // delete user
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearFields();
                    UserAcess objUser = new UserAcess();
                    //check Delete permission
                    if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        string UserID = Convert.ToString(e.CommandArgument.ToString());

                        //check user
                        bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);
                        bool bStatus = false;
                        string msg = string.Empty;
                        if (IsSuperAdmin)
                        {
                            bStatus = UserDataProvider.Instance.Delete(Guid.Parse(UserID));
                            msg = "User deleted successfully";
                        }
                        else
                        {
                            bStatus = UserDataProvider.Instance.DeleteEntityByUserEntity(Guid.Parse(UserID), Guid.Parse(Session["EntityId"].ToString()));
                            msg = "User entity successfully";
                        }

                        if (bStatus == true)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "User deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Success", UserID, msg);
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Failed", UserID, "User deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                        BindUsers();

                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "User Master", "Failed", "", "Sorry access denied");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }

                }

                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strUserId = Convert.ToString(e.CommandArgument);

                    UserDomain objUser = UserDataProvider.Instance.Get(Guid.Parse(strUserId));
                    IList<UserEntityDomain> objEntity = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(strUserId));
                    string strEntity = "";
                    foreach (UserEntityDomain entity in objEntity)
                    {
                        strEntity += entity.EntityName + "<br/>";
                    }
                    string str = @"<div  style='margin-bottom:15px'>
                    <table class='datatable'>
<tr>
<td><label>
                                    Photograh  :
                                    </label></td>
<td> <img src='img/Uploads/ProfilePic/" + objUser.Photograph + @"' width='50px' height='50px' alt='Photo' /></td>
</tr>
     
<tr>
<td><label>
                                     First  Name  :
                                    </label></td>
<td>" + objUser.FirstName + @"</td>
</tr>
                       <tr><td>
                                    <label>
                                     Last Name:
                                    </label>
                                </td>
                          <td>
                  " + objUser.LastName + @"
                                </td>

                                        </tr>

<tr>
                                   <td>
                                    <label>
                                   Designation :
                                    </label>
                                </td>
                                <td>
                              " + objUser.Designation + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Pan No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.PANNo + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Residential Address :
                                    </label>
                                </td>
                                <td>
                              " + objUser.ResidentialAddress + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   Residential Phone No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.ResidencePhone + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                 Mobile No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.Mobile + @"
                                  
                                </td></tr>

<tr>
                                   <td>
                                    <label>
                                   DIN No :
                                    </label>
                                </td>
                                <td>
                              " + objUser.DINNumber + @"
                                  
                                </td></tr>
                            <tr><td>
                                    <label>
                                  Office Address  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.OfficeAddress + @"
                                </td>
</tr>
                            <tr><td>
                                    <label>
                                   Office Phone No.  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.OfficePhone + @"
                                </td>
</tr>
                       <tr><td>
                                    <label>
                                    Email Id 1  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.EmailID1 + @"
                                </td>
</tr>
                       <tr><td>
                                    <label>
                                    Email Id 2 :
                                    </label>
                                </td>

                                <td>
                        " + objUser.EmailID2 + @"
                                </td>
</tr>
                           <tr><td>
                                    <label>
                                   Secretary Name  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryName + @"
                                </td>
</tr>

  <tr><td>
                                    <label>
                                   Secretary Residental Phone No.:
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryResidentalPhone + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                Secretary Office Phone  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryOfficePhone + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Mobile No.  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryMobile + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Email Id 1  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryEmailID1 + @"
                                </td>
</tr>
<tr><td>
                                    <label>
                                   Secretary Email Id 2  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.SecretaryEmailID2 + @"
                                </td>
</tr>

<tr><td>
                                    <label>
                                   User Enabled on Ipad :
                                    </label>
                                </td>

                                <td>
                        " + objUser.IsEnabledOnIpad + @"
                                </td>
</tr>

<tr><td>
                                    <label>
                                   User Enabled on Web  :
                                    </label>
                                </td>

                                <td>
                        " + objUser.IsEnabledOnWebApp + @"
                                </td>
</tr>
 <tr><td>
                                    <label>
                                    Entity :
                                    </label>
                                </td>
                                <td>
" + strEntity + @"
</td></tr>
                                   <tr><td>
                                    <label>
                                    Created On  :
                                    </label>
                                </td>
                                <td>
" + objUser.CreatedOn.ToString("dd/MM/yyyy") + @"
</td></tr></table>
<div>";
                    lblDetails.Text = str;
                    string strScript = @"<script type='text/javascript'> $(function() {
$( '#dialog' ).attr('title','User : " + objUser.UserName + @"' );
$( '#dialog' ).attr('style','display:block');
$( '#dialog' ).dialog({
                    maxWidth:700,
                    maxHeight: 300,
                    width: 900,
                    height: 300 });});</script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "Success", strScript);

                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }
        /// <summary>
        /// Row deleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Row editing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                ClearFields();

                //Disable username
                txtUserName.Enabled = false;
                txtCheckUser.Enabled = false;
                rfvCheckUser.Enabled = false;
                revCheckUser.Enabled = false;

                UserAcess objUser = new UserAcess();
                //check edit permission
                if (objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                {
                    string UserId = grdUser.DataKeys[e.NewEditIndex].Value.ToString();
                    if (UserId.Length == 36)
                    {
                        UserDomain objUserDomain = UserDataProvider.Instance.Get(Guid.Parse(UserId));
                        txtAddress.Text = objUserDomain.ResidentialAddress;

                        txtContactNumber.Text = objUserDomain.ResidencePhone;
                        txtDesignation.Text = objUserDomain.Designation;
                        txtDinNo.Text = objUserDomain.DINNumber;
                        txtEmail.Text = objUserDomain.EmailID1;
                        txtEmailTwo.Text = objUserDomain.EmailID2;
                        txtFirstName.Text = objUserDomain.FirstName;
                        txtLastName.Text = objUserDomain.LastName;
                        txtMobileNumebr.Text = objUserDomain.Mobile;
                        txtOfficeAddress.Text = objUserDomain.OfficeAddress;
                        txtOfficeContactNo.Text = objUserDomain.OfficePhone;
                        txtPanNo.Text = objUserDomain.PANNo;
                        txtPassportNumber.Text = objUserDomain.PassportNumber;
                        if (objUserDomain.Photograph != null)
                        {
                            if (objUserDomain.Photograph.Length > 0)
                            {
                                ViewState["file"] = objUserDomain.Photograph;
                                lnkView.Visible = true;
                            }
                        }

                        txtSecreroryOffiCont.Text = objUserDomain.SecretaryOfficePhone;
                        txtSecreroryResiCont.Text = objUserDomain.SecretaryResidentalPhone;
                        txtSecretoryEmail.Text = objUserDomain.SecretaryEmailID1;
                        txtSecretoryEmailTwo.Text = objUserDomain.SecretaryEmailID2;
                        txtSecretoryMobile.Text = objUserDomain.SecretaryMobile;
                        txtSecretoryName.Text = objUserDomain.SecretaryName;
                        ddlSuffix.SelectedValue = objUserDomain.Suffix;
                        txtUserName.Text = objUserDomain.UserName;
                        txtCheckUser.Text = objUserDomain.UserName;

                        txtMiddleName.Text = objUserDomain.MiddleName;


                        bool IsChecker = objUserDomain.IsChecker;
                        if (IsChecker)
                        {
                            rdbCheker.Checked = true;
                            rdbCheckerNo.Checked = false;
                        }
                        else
                        {
                            rdbCheckerNo.Checked = true;
                            rdbCheker.Checked = false;
                        }


                        bool IsMaker = objUserDomain.IsMaker;
                        if (IsMaker)
                        {
                            rdbMaker.Checked = true;
                            rdbMakerNo.Checked = false;
                        }
                        else
                        {
                            rdbMakerNo.Checked = true;
                            rdbMaker.Checked = false;
                        }

                        if (objUserDomain.IsEnabledOnWebApp == true)
                        {
                            chkWebApp.Checked = true;
                        }
                        else
                        {
                            chkWebApp.Checked = false;
                        }
                        if (objUserDomain.IsEnabledOnIpad == true)
                        {
                            chkIpad.Checked = true;
                            //divPassword.Attributes.Add("style", "display:block");
                            //rfvPassword.Enabled = true;
                            //rfvConfPassword.Enabled = true;
                        }
                        else
                        {
                            chkIpad.Checked = false;
                        }

                        if (objUserDomain.IsSuperAdmin)
                            chkIsSuperAdmin.Checked = true;
                        else
                            chkIsSuperAdmin.Checked = false;

                        //   ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);

                        if (ddlUser.Items.FindByValue(
       Convert.ToString(objUserDomain.UserChecker)) != null)
                        {
                            ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);
                        }

                        //txtConformPass.Text = objUserDomain.Password;
                        //txtPassword.Text = objUserDomain.Password;

                        //txtConformPass.Attributes.Add("value", objUserDomain.Password);
                        //txtPassword.Attributes.Add("value", objUserDomain.Password);

                        ViewState["pswd"] = objUserDomain.Password;
                        txtConformPass.Attributes.Add("value", "!!1A ***** b1!!");
                        txtPassword.Attributes.Add("value", "!!1A ***** b1!!");

                        hdnUserId.Value = UserId;
                        hdnPhoto.Value = Convert.ToString(objUserDomain.Photograph);

                        bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);
                        if (!IsSuperAdmin)
                        {
                            txtPassword.Enabled = false;
                            txtConformPass.Enabled = false;

                            rfvConfPassword.Enabled = false;
                            rfvPassword.Enabled = false;
                            //cfvConformPass.Enabled = false;
                            //rgePassword.Enabled = false;
                        }

                        //  chkDefaultUser.Checked = objUserDomain.DefaultUser;

                        IList<UserEntityDomain> userEntityList = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId));

                        int entityCount = userEntityList.Count;
                        if (entityCount > 0)
                        {
                            int itemIndex = 0;
                            List<string> selectedVal = new List<string>();

                            var selectedEntty = (from p in userEntityList
                                                 select new
                                                 {
                                                     EntityId = p.EntityId
                                                 }).ToList();

                            foreach (var id in selectedEntty)
                            {
                                selectedVal.Add(id.EntityId.ToString());
                            }


                            foreach (ListItem lstItem in ddlEntityList.Items)
                            {
                                if (entityCount == itemIndex)
                                {
                                    //return after selecting all users entity
                                    break;
                                }

                                var objEntityId = (from p in userEntityList
                                                   where (p.EntityId == Guid.Parse(lstItem.Value))
                                                   select p).ToList();
                                if (objEntityId.Count > 0)
                                {
                                    //check list item is equal to user entity
                                    //if (lstItem.Value == userEntityList[itemIndex].EntityId.ToString())
                                    //{
                                    lstItem.Selected = true;
                                    // selectedVal.Add(lstItem.Value);
                                    itemIndex++;

                                    //}
                                }
                            }
                            ViewState["selectedEntity"] = selectedVal;
                        }

                        string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);

                        string DeviceAuthType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
                        if (!DeviceAuthType.ToLower().Equals("ad") && objUserDomain.IsEnabledOnIpad && AuthenticationType.ToLower().Equals("ad"))
                        {
                            chkIpad.Attributes.Add("onclick", "showPassword()");
                            divPassword.Attributes.Add("style", "display:block");
                            divConfirmPassword.Attributes.Add("style", "display:block");
                            divConfirmPassword.Visible = true;
                            divPassword.Visible = true;
                            rfvConfPassword.Enabled = true;
                            rfvPassword.Enabled = true;
                            //cfvConformPass.Enabled = true;
                            //rgePassword.Enabled = true;
                            //lblPassword.InnerHtml = "iPad Password<span>*</span>";
                            //lblConfPassword.InnerHtml = "iPad Confirm Password<span>*</span>";
                        }

                        btnInsert.Text = "Update";
                        lblAction.Text = "Edit User";
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                    Error.Visible = true;
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
        /// Code To SelectIndex Chage
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUser_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (ViewState["sortDirection"] == null)
                {
                    ViewState["sortDirection"] = "asc";
                }


                if (UserDataProvider.Instance.Get().Count > 0)
                {
                    DataTable dt = (DataTable)UserDataProvider.Instance.Get().AsDataTable();
                    DataView dv = new DataView(dt);
                    if (Convert.ToString(ViewState["sortDirection"]) == "asc")
                    {
                        ViewState["sortDirection"] = "dsc";
                        dv.Sort = e.SortExpression + " DESC";
                    }
                    else
                    {
                        ViewState["sortDirection"] = "asc";
                        dv.Sort = e.SortExpression + " ASC";
                    }

                    grdUser.DataSource = dv;
                    grdUser.DataBind();
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
                ClearFields();
                grdUser.PageIndex = e.NewPageIndex;
                BindUsers();
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
        /// Used To Clear TextBoxes
        /// </summary>
        public void ClearFields()
        {
            try
            {
                txtAddress.Text = "";
                txtConformPass.Text = "";
                txtContactNumber.Text = "";
                txtDesignation.Text = "";
                txtDinNo.Text = "";
                txtEmail.Text = "";
                txtEmailTwo.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMobileNumebr.Text = "";
                txtOfficeAddress.Text = "";
                txtOfficeContactNo.Text = "";
                txtPanNo.Text = "";
                txtPassword.Text = "";
                txtFoodPreference.Text = "";
                txtConformPass.Text = "";

                txtSecreroryOffiCont.Text = "";
                txtSecreroryResiCont.Text = "";
                txtSecretoryEmail.Text = "";
                txtSecretoryEmailTwo.Text = "";
                txtSecretoryMobile.Text = "";
                txtSecretoryName.Text = "";
                txtUserName.Text = "";
                ddlEntityList.SelectedIndex = -1;

                hdnPhoto.Value = "";
                hdnUserId.Value = "";
                btnInsert.Text = "Save";

                rdbMakerNo.Checked = true;
                rdbMaker.Checked = false;

                rdbCheker.Checked = false;
                rdbCheckerNo.Checked = true;

                ddlUser.SelectedIndex = -1;
                ddlSuffix.SelectedIndex = -1;

                lnkView.Visible = false;
                ViewState["file"] = null;
                chkChecker.Checked = false;

                chkNotifyChecker.Checked = false;

                chkDefaultUser.Checked = false;

                txtConformPass.Attributes.Add("value", "");
                txtPassword.Attributes.Add("value", "");

                //divPassword.Attributes.Add("style", "display:none");
                //rfvPassword.Enabled = false;
                //rfvConfPassword.Enabled = false;

                chkIpad.Checked = false;
                txtMiddleName.Text = "";
                lblAction.Text = "Add New User";

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:block;");
                    rfvddUser.Enabled = true;
                    chkChecker.Enabled = false;
                    chkChecker.Checked = true;
                }

                chkIsSuperAdmin.Checked = false;
                //spnUser.InnerHtml = "";

                txtPassword.Enabled = true;
                txtConformPass.Enabled = true;

                rfvConfPassword.Enabled = true;
                rfvPassword.Enabled = true;
                //cfvConformPass.Enabled = true;
                //rgePassword.Enabled = true;

                chkWebApp.Checked = false;
                txtCheckUser.Text = "";
                spnUser.InnerHtml = "";
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
        /// Remove selected User
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lbRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                //StringBuilder strQuery = new StringBuilder("");
                StringBuilder strUserIds = new StringBuilder(",");
                int count = 0;
                //get all checked user
                for (int i = 0; i < grdUser.Rows.Count; i++)
                {
                    CheckBox chkSelect = (CheckBox)grdUser.Rows[i].FindControl("chkSubAdmin");
                    if (chkSelect.Checked)
                    {
                        count++;
                        string UserID = Convert.ToString(grdUser.DataKeys[i].Value.ToString());
                        //Delete all selected user
                        //  strUserIds.Append("	UPDATE [User] SET IsActive = 0  WHERE  UserId= '" + UserID + "'  ");
                        strUserIds.Append(UserID + ",");
                    }
                }
                if (count > 0)
                {
                    bool bStatus = UserDataProvider.Instance.DeleteSelected(strUserIds.ToString());
                    if (!bStatus)
                    {
                        ((Label)Info.FindControl("lblName")).Text = "User deleted successfully";
                        Info.Visible = true;
                        BindUsers();
                        BindCheckers();
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                        Error.Visible = true;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Please select at least one checkbox";
                    Error.Visible = true;
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
        /// Send email to username and password
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="UserName"></param>
        /// <param name="Pass"></param>
        //public void SendEmail(string EmailId, string UserName, string Pass)
        //{
        //    try
        //    {
        //        if (txtUserName != null && txtUserName.Text != "")
        //        {
        //            UserDomain objUser = UserDataProvider.Instance.GetPassword(txtUserName.Text);
        //            if (objUser != null)
        //            {
        //                Email user name and password

        //                string ToEmail = EmailId;
        //                string EmailFrom = ConfigurationManager.AppSettings["Email"];
        //                string Password = ConfigurationManager.AppSettings["Password"];
        //                string EmailSubject = "Your Username and Password";

        //                string strlink = @"http:///Login.aspx";
        //                string EmailBody = "<p>";
        //                EmailBody += "<table style=\"border-style:none;\"><tr style=\" border-style:none;\"><td>";

        //                <td><img src=" + ("http:// /images/logo.png") + "></img></td>
        //                EmailBody += "<font color=\"Teal\"></b> MEETING MINDER  USERNAME AND PASSWORD</b></font></td></tr></table><BR />";
        //                EmailBody += "Dear User <BR /><BR />";
        //                EmailBody += "Following are your Login Details :-<BR />";

        //                EmailBody += "<Table width=\"700px\" style=\"border-color:Teal; height:100px; width:50%; border-style:double;\">";
        //                EmailBody += "<tr style=\" border-style:none;\"><td>";

        //                EmailBody += "Login Email</td><td> :</td><td>" + UserName + "</td></tr>";
        //                EmailBody += "<tr style=\" border-style:none;\"><td>Password</td><td> :</td><td>" + Pass + "</td></tr>";
        //                EmailBody += "</table><br /><br /><br />";
        //                EmailBody += "&nbsp; Regards <br /> Meeting Minder Admin Team";
        //                EmailBody += "</p>";

        //                MailMessage objmail = new MailMessage();//(EmailFrom, ToEmail, EmailSubject, EmailBody);
        //                SmtpClient SMTPServer = new SmtpClient();

        //                SMTPServer = new SmtpClient();
        //                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(EmailFrom, Password);
        //                SMTPServer.UseDefaultCredentials = false;
        //                SMTPServer.Credentials = SMTPUserInfo;
        //                SMTPServer.EnableSsl = true;
        //                MailAddress fromAddress = new MailAddress(EmailFrom, "Meeting Minder");
        //                objmail.From = fromAddress;
        //                objmail.To.Add(ToEmail);
        //                objmail.Body = EmailBody;
        //                objmail.Subject = EmailSubject;

        //                objmail.IsBodyHtml = true;



        //                objmail.Headers.Add("NAME", "Admin");

        //                SMTPServer.Host = "smtp.gmail.com";
        //                SMTPServer.Port = 587;
        //                SMTPServer.EnableSsl = true;
        //                SMTPServer.Send(objmail);
        //                objmail.To.Clear();

        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;
        //    }
        //}


        /// <summary>
        /// View uploaded file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["file"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ImagePath"]);

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.WriteFile(FilePath);
                Response.End();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }



        public void ExportToExcel(IList<UserDetails> ds, string filename)
        {
            HttpResponse response = HttpContext.Current.Response;

            // first let's clean up the response.object
            response.Clear();
            response.Charset = "";

            // set the response mime type for excel
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + ".xls\"");

            // create a string writer
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // instantiate a datagrid
                    GridView gdvContact = new GridView();
                    gdvContact.DataSource = ds;
                    gdvContact.DataBind();
                    gdvContact.HeaderRow.Font.Bold = true;


                    gdvContact.RenderControl(htw);
                    response.Write(sw.ToString());
                    response.End();
                }
            }
        }

        protected void lbExportToExcel_Click(object sender, EventArgs e)
        {
            IList<UserDomain> objUserList = UserDataProvider.Instance.Get().OrderBy(p => p.FirstName.Trim()).ToList();

            if (objUserList.Count > 0)
            {
                IList<UserDetails> objUserDetailsList = new List<UserDetails>();
                UserDetails objUsersDetails;
                foreach (UserDomain objUser in objUserList)
                {
                    objUsersDetails = new UserDetails();
                    objUsersDetails.Designation = WebUtility.HtmlDecode(objUser.Designation);
                    //objUsersDetails.DINNumber = WebUtility.HtmlDecode(objUser.DINNumber);
                    objUsersDetails.EmailAddress = WebUtility.HtmlDecode(objUser.EmailID1);
                    //objUsersDetails.EmailID2 = WebUtility.HtmlDecode(objUser.EmailID2);
                    objUsersDetails.FirstName = WebUtility.HtmlDecode(objUser.FirstName);
                    objUsersDetails.LastName = WebUtility.HtmlDecode(objUser.LastName);
                    objUsersDetails.MiddleName = WebUtility.HtmlDecode(objUser.MiddleName);
                    objUsersDetails.MobileNo = WebUtility.HtmlDecode(objUser.Mobile);
                    //objUsersDetails.OfficeAddress = WebUtility.HtmlDecode(objUser.OfficeAddress);
                    //objUsersDetails.OfficePhone = WebUtility.HtmlDecode(objUser.OfficePhone);
                    //objUsersDetails.PANNo = WebUtility.HtmlDecode(objUser.PANNo);
                    //objUsersDetails.PassportNumber = WebUtility.HtmlDecode(objUser.PassportNumber);
                    //objUsersDetails.Password = objUser.Password;
                    //objUsersDetails.Photograph = objUser.Photograph;
                    //objUsersDetails.ResidencePhone = WebUtility.HtmlDecode(objUser.ResidencePhone);
                    //objUsersDetails.ResidentialAddress = WebUtility.HtmlDecode(objUser.ResidentialAddress);
                    //objUsersDetails.SecretaryEmailID1 = WebUtility.HtmlDecode(objUser.SecretaryEmailID1);
                    //objUsersDetails.SecretaryEmailID2 = WebUtility.HtmlDecode(objUser.SecretaryEmailID2);
                    //objUsersDetails.SecretaryMobile = WebUtility.HtmlDecode(objUser.SecretaryMobile);
                    //objUsersDetails.SecretaryName = WebUtility.HtmlDecode(objUser.SecretaryName);
                    //objUsersDetails.SecretaryOfficePhone = WebUtility.HtmlDecode(objUser.SecretaryOfficePhone);
                    //objUsersDetails.SecretaryResidentalPhone = WebUtility.HtmlDecode(objUser.SecretaryResidentalPhone);
                    objUsersDetails.UserName = objUser.UserName;
                    objUsersDetails.Suffix = WebUtility.HtmlDecode(objUser.Suffix);
                    if (objUser.IsChecker == true)
                    {
                        objUsersDetails.IsChecker = "Yes";
                    }

                    else if (objUser.IsChecker == false)
                    {
                        objUsersDetails.IsChecker = "No";
                    }


                    if (objUser.IsMaker == true)
                    {
                        objUsersDetails.IsMaker = "Yes";
                    }

                    else if (objUser.IsMaker == false)
                    {
                        objUsersDetails.IsMaker = "No";
                    }


                    if (objUser.IsEnabledOnIpad == true)
                    {
                        objUsersDetails.IsEnabledOnIpad = "Yes";
                    }

                    else if (objUser.IsEnabledOnIpad == false)
                    {
                        objUsersDetails.IsEnabledOnIpad = "No";
                    }


                    if (objUser.IsEnabledOnWebApp == true)
                    {
                        objUsersDetails.IsEnabledOnWebApp = "Yes";
                    }

                    else if (objUser.IsEnabledOnWebApp == false)
                    {
                        objUsersDetails.IsEnabledOnWebApp = "No";
                    }


                    objUserDetailsList.Add(objUsersDetails);
                }
                ExportToExcel(objUserDetailsList, "User Info");
            }


        }

        public class UserDetails
        {
            public string Suffix
            {
                get;
                set;
            }
            // public Guid UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public string MiddleName { get; set; }

            //  public string Email { get; set; }
            public string UserName { get; set; }
            // public string Password { get; set; }
            //  public string ContactNo { get; set; }
            //public string PANNo { get; set; }
            //public Guid CreatedBy { get; set; }
            //public DateTime CreatedOn { get; set; }
            //public Guid UpdatedBy { get; set; }
            // public DateTime UpdatedOn { get; set; }
            //public string PassportNumber { get; set; }

            public string Designation
            {
                get;
                set;
            }
            //public Guid EntityId
            //{
            //    get;
            //    set;
            //}

            //public string OfficeAddress
            //{
            //    get;
            //    set;
            //}
            //public string ResidentialAddress
            //{
            //    get;
            //    set;
            //}
            //public string DINNumber
            //{
            //    get;
            //    set;
            //}

            //public string Photograph
            //{
            //    get;
            //    set;
            //}

            //public string OfficePhone
            //{
            //    get;
            //    set;
            //}
            //public string ResidencePhone
            //{
            //    get;
            //    set;
            //}
            public string MobileNo
            {
                get;
                set;
            }
            public string EmailAddress
            {
                get;
                set;
            }
            //public string EmailID2
            //{
            //    get;
            //    set;
            //}

            //public string SecretaryName
            //{
            //    get;
            //    set;
            //}

            //public string SecretaryResidentalPhone
            //{
            //    get;
            //    set;
            //}

            //public string SecretaryOfficePhone
            //{
            //    get;
            //    set;
            //}
            //public string SecretaryMobile
            //{
            //    get;
            //    set;
            //}

            //public string SecretaryEmailID1
            //{
            //    get;
            //    set;
            //}

            //public string SecretaryEmailID2
            //{
            //    get;
            //    set;
            //}

            //public bool IsAuthorized
            //{
            //    get;
            //    set;
            //}

            public string IsChecker { get; set; }
            public string IsMaker { get; set; }
            public string IsEnabledOnIpad { get; set; }
            public string IsEnabledOnWebApp { get; set; }
            //public Guid UserChecker { get; set; }

            //public Guid SecurityQuestionId
            //{
            //    get;
            //    set;
            //}

            //public string Answer
            //{
            //    get;
            //    set;
            //}

            //public string Token
            //{
            //    get;
            //    set;
            //}



            //public string DeviceToken
            //{
            //    get;
            //    set;
            //}

            //public string FoodPreference
            //{
            //    get;
            //    set;
            //}

            //public string EncryptionKey
            //{
            //    get;
            //    set;
            //}

            //public int SessionTimeout
            //{
            //    get;
            //    set;
            //}

            //public bool IsDeviceLost
            //{
            //    get;
            //    set;
            //}

        }

        protected void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null)
                {
                    return;

                }
                string UserName = txtUserName.Text.Trim();
                string strEmail = "";
                if (UserName != "")
                {
                    Guid UserId = Guid.Parse(HttpContext.Current.Session["UserId"].ToString());
                    int Count = UserDataProvider.Instance.CheckUserNameExistance(UserName.ToLower(), UserId);
                    if (Count > 0)
                    {
                        //strEmail = "<span style='color:red'> UserName already exists </span>";
                        Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());
                        int Department = UserDataProvider.Instance.CheckUserIdDepartmentExitance(UserName.ToLower(), EntityId);
                        if (Department > 0)
                        {
                            strEmail = "<span style='color:red;margin-left:195px;'> Username & department already exists. </span>";
                            ClearFields1();
                            BindEntity();
                        }
                        else
                        {
                            ClearFields1();
                            strEmail = "<span style='color:red;margin-left:265px;'> Username already exists. </span>";

                            //strEmail = "<span style='color:red;margin-left:265px;'> UserName already exists </span><div style='font-size:12px; margin-left:315px'><strong><a style='color:#3f79c1;' href='javascript:void(0)' onclick='DepartmentFun()'>Add department</a></strong></div>";
                            UserDomain objUserDomain = UserDataProvider.Instance.GetUserByUserName(UserName);
                            txtAddress.Text = objUserDomain.ResidentialAddress;

                            txtContactNumber.Text = objUserDomain.ResidencePhone;
                            txtDesignation.Text = objUserDomain.Designation;
                            txtDinNo.Text = objUserDomain.DINNumber;
                            txtEmail.Text = objUserDomain.EmailID1;
                            txtEmailTwo.Text = objUserDomain.EmailID2;
                            txtFirstName.Text = objUserDomain.FirstName;
                            txtLastName.Text = objUserDomain.LastName;
                            txtMobileNumebr.Text = objUserDomain.Mobile;
                            txtOfficeAddress.Text = objUserDomain.OfficeAddress;
                            txtOfficeContactNo.Text = objUserDomain.OfficePhone;
                            txtPanNo.Text = objUserDomain.PANNo;
                            txtPassportNumber.Text = objUserDomain.PassportNumber;
                            if (objUserDomain.Photograph != null)
                            {
                                if (objUserDomain.Photograph.Length > 0)
                                {
                                    ViewState["file"] = objUserDomain.Photograph;
                                    lnkView.Visible = true;
                                }
                            }

                            txtSecreroryOffiCont.Text = objUserDomain.SecretaryOfficePhone;
                            txtSecreroryResiCont.Text = objUserDomain.SecretaryResidentalPhone;
                            txtSecretoryEmail.Text = objUserDomain.SecretaryEmailID1;
                            txtSecretoryEmailTwo.Text = objUserDomain.SecretaryEmailID2;
                            txtSecretoryMobile.Text = objUserDomain.SecretaryMobile;
                            txtSecretoryName.Text = objUserDomain.SecretaryName;
                            ddlSuffix.SelectedValue = objUserDomain.Suffix;
                            txtUserName.Text = objUserDomain.UserName;

                            txtMiddleName.Text = objUserDomain.MiddleName;


                            bool IsChecker = objUserDomain.IsChecker;
                            if (IsChecker)
                            {
                                rdbCheker.Checked = true;
                                rdbCheckerNo.Checked = false;
                            }
                            else
                            {
                                rdbCheckerNo.Checked = true;
                                rdbCheker.Checked = false;
                            }


                            bool IsMaker = objUserDomain.IsMaker;
                            if (IsMaker)
                            {
                                rdbMaker.Checked = true;
                                rdbMakerNo.Checked = false;
                            }
                            else
                            {
                                rdbMakerNo.Checked = true;
                                rdbMaker.Checked = false;
                            }

                            if (objUserDomain.IsEnabledOnWebApp == true)
                            {
                                chkWebApp.Checked = true;
                            }
                            else
                            {
                                chkWebApp.Checked = false;
                            }
                            if (objUserDomain.IsEnabledOnIpad == true)
                            {
                                chkIpad.Checked = true;
                                //divPassword.Attributes.Add("style", "display:block");
                                //rfvPassword.Enabled = true;
                                //rfvConfPassword.Enabled = true;
                            }
                            else
                            {
                                chkIpad.Checked = false;
                            }

                            if (objUserDomain.IsSuperAdmin)
                                chkIsSuperAdmin.Checked = true;
                            else
                                chkIsSuperAdmin.Checked = false;

                            //   ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);

                            if (ddlUser.Items.FindByValue(
                            Convert.ToString(objUserDomain.UserChecker)) != null)
                            {
                                ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);
                            }

                            //txtConformPass.Text = objUserDomain.Password;
                            //txtPassword.Text = objUserDomain.Password;

                            //txtConformPass.Attributes.Add("value", objUserDomain.Password);
                            //txtPassword.Attributes.Add("value", objUserDomain.Password);

                            ViewState["pswd"] = objUserDomain.Password;
                            txtConformPass.Attributes.Add("value", "!!1A ***** b1!!");
                            txtPassword.Attributes.Add("value", "!!1A ***** b1!!");

                            hdnUserId.Value = Convert.ToString(objUserDomain.UserId);
                            hdnPhoto.Value = Convert.ToString(objUserDomain.Photograph);

                            //  chkDefaultUser.Checked = objUserDomain.DefaultUser;

                            IList<UserEntityDomain> userEntityList = UserEntityDataProvider.Instance.GetEntityListByUserId(objUserDomain.UserId);

                            int entityCount = userEntityList.Count;
                            if (entityCount > 0)
                            {
                                int itemIndex = 0;
                                List<string> selectedVal = new List<string>();

                                var selectedEntty = (from p in userEntityList
                                                     select new
                                                     {
                                                         EntityId = p.EntityId
                                                     }).ToList();

                                foreach (var id in selectedEntty)
                                {
                                    selectedVal.Add(id.EntityId.ToString());
                                }


                                foreach (ListItem lstItem in ddlEntityList.Items)
                                {
                                    if (entityCount == itemIndex)
                                    {
                                        //return after selecting all users entity
                                        break;
                                    }

                                    var objEntityId = (from p in userEntityList
                                                       where (p.EntityId == Guid.Parse(lstItem.Value))
                                                       select p).ToList();
                                    if (objEntityId.Count > 0)
                                    {
                                        //check list item is equal to user entity
                                        //if (lstItem.Value == userEntityList[itemIndex].EntityId.ToString())
                                        //{
                                        lstItem.Selected = true;
                                        // selectedVal.Add(lstItem.Value);
                                        itemIndex++;

                                        //}
                                    }
                                }
                                ViewState["selectedEntity"] = selectedVal;
                            }

                            string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);

                            string DeviceAuthType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
                            if (!DeviceAuthType.ToLower().Equals("ad") && objUserDomain.IsEnabledOnIpad && AuthenticationType.ToLower().Equals("ad"))
                            {
                                chkIpad.Attributes.Add("onclick", "showPassword()");
                                divPassword.Attributes.Add("style", "display:block");
                                divConfirmPassword.Attributes.Add("style", "display:block");
                                divConfirmPassword.Visible = true;
                                divPassword.Visible = true;
                                rfvConfPassword.Enabled = true;
                                rfvPassword.Enabled = true;
                                //cfvConformPass.Enabled = true;
                                //rgePassword.Enabled = true;
                                //lblPassword.InnerHtml = "iPad Password<span>*</span>";
                                //lblConfPassword.InnerHtml = "iPad Confirm Password<span>*</span>";
                            }

                            btnInsert.Text = "Update";
                            lblAction.Text = "Edit User";
                        }
                    }
                    else
                    {
                        strEmail = "";
                        ClearFields1();
                        BindEntity();
                    }
                }
                spnUser.InnerHtml = strEmail.ToString();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        public void ClearFields1()
        {
            try
            {
                txtAddress.Text = "";
                txtConformPass.Text = "";
                txtContactNumber.Text = "";
                txtDesignation.Text = "";
                txtDinNo.Text = "";
                txtEmail.Text = "";
                txtEmailTwo.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtMobileNumebr.Text = "";
                txtOfficeAddress.Text = "";
                txtOfficeContactNo.Text = "";
                txtPanNo.Text = "";
                txtPassword.Text = "";
                txtFoodPreference.Text = "";
                txtConformPass.Text = "";

                txtSecreroryOffiCont.Text = "";
                txtSecreroryResiCont.Text = "";
                txtSecretoryEmail.Text = "";
                txtSecretoryEmailTwo.Text = "";
                txtSecretoryMobile.Text = "";
                txtSecretoryName.Text = "";
                //txtUserName.Text = "";
                ddlEntityList.SelectedIndex = -1;

                hdnPhoto.Value = "";
                hdnUserId.Value = "";
                btnInsert.Text = "Save";

                rdbMakerNo.Checked = true;
                rdbMaker.Checked = false;

                rdbCheker.Checked = false;
                rdbCheckerNo.Checked = true;

                ddlUser.SelectedIndex = -1;
                ddlSuffix.SelectedIndex = -1;

                lnkView.Visible = false;
                ViewState["file"] = null;
                chkChecker.Checked = false;

                chkNotifyChecker.Checked = false;

                chkDefaultUser.Checked = false;

                txtConformPass.Attributes.Add("value", "");
                txtPassword.Attributes.Add("value", "");

                //divPassword.Attributes.Add("style", "display:none");
                //rfvPassword.Enabled = false;
                //rfvConfPassword.Enabled = false;

                chkIpad.Checked = false;
                txtMiddleName.Text = "";
                lblAction.Text = "Add New User";

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:block;");
                    rfvddUser.Enabled = true;
                    chkChecker.Enabled = false;
                    chkChecker.Checked = true;
                }

                chkIsSuperAdmin.Checked = false;
                //spnUser.InnerHtml = "";

                txtPassword.Enabled = true;
                txtConformPass.Enabled = true;

                rfvConfPassword.Enabled = true;
                rfvPassword.Enabled = true;
                //cfvConformPass.Enabled = true;
                //rgePassword.Enabled = true;

                chkWebApp.Checked = false;
                //txtCheckUser.Text = "";
                //spnUser.InnerHtml = "";
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
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

        protected void lnkViewUser_Click(object sender, EventArgs e)
        {
            try
            {
                //Disable username               
                txtUserName.Enabled = false;

                string UserName = txtCheckUser.Text.Trim();
                string strMsg = string.Empty;
                spnUser.InnerHtml = "";

                UserDomain objUserDomain = UserDataProvider.Instance.GetUserByUserName(UserName.ToLower());
                if (objUserDomain != null)
                {
                    Guid EntityId = Guid.Parse(HttpContext.Current.Session["EntityId"].ToString());
                    int Department = UserDataProvider.Instance.CheckUserIdDepartmentExitance(UserName.ToLower(), EntityId);
                    if (Department > 0)
                    {
                        strMsg = "<span style='color:red;margin-left:195px;'> Username & department already exists. </span>";
                        ClearFields1();
                        spnUser.InnerHtml = strMsg.ToString();
                        txtUserName.Text = UserName;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "TopUp();", true);
                        return;
                    }

                    txtAddress.Text = objUserDomain.ResidentialAddress;

                    txtContactNumber.Text = objUserDomain.ResidencePhone;
                    txtDesignation.Text = objUserDomain.Designation;
                    txtDinNo.Text = objUserDomain.DINNumber;
                    txtEmail.Text = objUserDomain.EmailID1;
                    txtEmailTwo.Text = objUserDomain.EmailID2;
                    txtFirstName.Text = objUserDomain.FirstName;
                    txtLastName.Text = objUserDomain.LastName;
                    txtMobileNumebr.Text = objUserDomain.Mobile;
                    txtOfficeAddress.Text = objUserDomain.OfficeAddress;
                    txtOfficeContactNo.Text = objUserDomain.OfficePhone;
                    txtPanNo.Text = objUserDomain.PANNo;
                    txtPassportNumber.Text = objUserDomain.PassportNumber;
                    if (objUserDomain.Photograph != null)
                    {
                        if (objUserDomain.Photograph.Length > 0)
                        {
                            ViewState["file"] = objUserDomain.Photograph;
                            lnkView.Visible = true;
                        }
                    }

                    txtSecreroryOffiCont.Text = objUserDomain.SecretaryOfficePhone;
                    txtSecreroryResiCont.Text = objUserDomain.SecretaryResidentalPhone;
                    txtSecretoryEmail.Text = objUserDomain.SecretaryEmailID1;
                    txtSecretoryEmailTwo.Text = objUserDomain.SecretaryEmailID2;
                    txtSecretoryMobile.Text = objUserDomain.SecretaryMobile;
                    txtSecretoryName.Text = objUserDomain.SecretaryName;
                    ddlSuffix.SelectedValue = objUserDomain.Suffix;
                    txtUserName.Text = objUserDomain.UserName;

                    txtMiddleName.Text = objUserDomain.MiddleName;


                    bool IsChecker = objUserDomain.IsChecker;
                    if (IsChecker)
                    {
                        rdbCheker.Checked = true;
                        rdbCheckerNo.Checked = false;
                    }
                    else
                    {
                        rdbCheckerNo.Checked = true;
                        rdbCheker.Checked = false;
                    }


                    bool IsMaker = objUserDomain.IsMaker;
                    if (IsMaker)
                    {
                        rdbMaker.Checked = true;
                        rdbMakerNo.Checked = false;
                    }
                    else
                    {
                        rdbMakerNo.Checked = true;
                        rdbMaker.Checked = false;
                    }

                    if (objUserDomain.IsEnabledOnWebApp == true)
                    {
                        chkWebApp.Checked = true;
                    }
                    else
                    {
                        chkWebApp.Checked = false;
                    }
                    if (objUserDomain.IsEnabledOnIpad == true)
                    {
                        chkIpad.Checked = true;
                    }
                    else
                    {
                        chkIpad.Checked = false;
                    }

                    if (objUserDomain.IsSuperAdmin)
                        chkIsSuperAdmin.Checked = true;
                    else
                        chkIsSuperAdmin.Checked = false;

                    //   ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);

                    if (ddlUser.Items.FindByValue(
   Convert.ToString(objUserDomain.UserChecker)) != null)
                    {
                        ddlUser.SelectedValue = Convert.ToString(objUserDomain.UserChecker);
                    }

                    //txtConformPass.Text = objUserDomain.Password;
                    //txtPassword.Text = objUserDomain.Password;

                    //txtConformPass.Attributes.Add("value", objUserDomain.Password);
                    //txtPassword.Attributes.Add("value", objUserDomain.Password);

                    ViewState["pswd"] = objUserDomain.Password;
                    txtConformPass.Attributes.Add("value", "!!1A ***** b1!!");
                    txtPassword.Attributes.Add("value", "!!1A ***** b1!!");

                    hdnUserId.Value = objUserDomain.UserId.ToString();
                    hdnPhoto.Value = Convert.ToString(objUserDomain.Photograph);



                    //  chkDefaultUser.Checked = objUserDomain.DefaultUser;
                    ddlEntityList.SelectedIndex = -1;
                    IList<UserEntityDomain> userEntityList = UserEntityDataProvider.Instance.GetEntityListByUserId(objUserDomain.UserId);

                    int entityCount = userEntityList.Count;
                    if (entityCount > 0)
                    {
                        int itemIndex = 0;
                        List<string> selectedVal = new List<string>();

                        var selectedEntty = (from p in userEntityList
                                             select new
                                             {
                                                 EntityId = p.EntityId
                                             }).ToList();

                        foreach (var id in selectedEntty)
                        {
                            selectedVal.Add(id.EntityId.ToString());
                        }


                        foreach (ListItem lstItem in ddlEntityList.Items)
                        {
                            if (entityCount == itemIndex)
                            {
                                //return after selecting all users entity
                                break;
                            }

                            var objEntityId = (from p in userEntityList
                                               where (p.EntityId == Guid.Parse(lstItem.Value))
                                               select p).ToList();
                            if (objEntityId.Count > 0)
                            {
                                //check list item is equal to user entity
                                //if (lstItem.Value == userEntityList[itemIndex].EntityId.ToString())
                                //{
                                lstItem.Selected = true;
                                // selectedVal.Add(lstItem.Value);
                                itemIndex++;

                                //}
                            }
                            else
                            {
                                lstItem.Selected = false;
                            }
                        }
                        ViewState["selectedEntity"] = selectedVal;
                    }

                    bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);
                    if (!IsSuperAdmin)
                    {
                        txtPassword.Enabled = false;
                        txtConformPass.Enabled = false;

                        rfvConfPassword.Enabled = false;
                        rfvPassword.Enabled = false;
                        //cfvConformPass.Enabled = false;
                        //rgePassword.Enabled = false;
                        ViewState["adminEntity"] = "admin";
                    }

                    string AuthenticationType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AuthType"]);

                    string DeviceAuthType = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DeviceAuthType"]);
                    if (!DeviceAuthType.ToLower().Equals("ad") && objUserDomain.IsEnabledOnIpad && AuthenticationType.ToLower().Equals("ad"))
                    {
                        chkIpad.Attributes.Add("onclick", "showPassword()");
                        divPassword.Attributes.Add("style", "display:block");
                        divConfirmPassword.Attributes.Add("style", "display:block");
                        divConfirmPassword.Visible = true;
                        divPassword.Visible = true;
                        rfvConfPassword.Enabled = true;
                        rfvPassword.Enabled = true;
                        //cfvConformPass.Enabled = true;
                        //rgePassword.Enabled = true;
                        //lblPassword.InnerHtml = "iPad Password<span>*</span>";
                        //lblConfPassword.InnerHtml = "iPad Confirm Password<span>*</span>";
                    }

                    btnInsert.Text = "Update";
                    //lblAction.Text = "Edit User";
                }
                else
                {
                    ClearFields1();
                    txtUserName.Text = UserName;
                    BindEntity();
                }
                divloader.Attributes.Add("style", "display:none");
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "TopUp();", true);
            }
            catch (Exception ex)
            {
                LogError objEr = new LogError();
                objEr.HandleException(ex);

                divloader.Attributes.Add("style", "display:none");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string strSearch = txtSearch.Text.Trim().ToLower();
                btnReset.Visible = true;

                Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home</a>&nbsp;";
                IList<UserDomain> objUserList = UserDataProvider.Instance.Get().Where(li => li.UserName.ToLower().Contains(strSearch) || li.FirstName.ToLower().Contains(strSearch) || li.LastName.ToLower().Contains(strSearch)).OrderBy(p => p.FirstName).ToList();
                // IList<UserDomain> objUserList = UserDataProvider.Instance.GetByEntity(EntityId);

                //hide super Admin edit and delete button
                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (!IsSuperAdmin)
                {
                    for (int i = 0; i < objUserList.Count; i++)
                    {
                        if (objUserList[i].IsSuperAdmin)
                        {
                            objUserList[i].hideButton = true;
                        }
                    }
                }
                grdUser.DataSource = objUserList;
                grdUser.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.Visible = false;
            txtSearch.Text = string.Empty;

            //bind user list  to grid
            BindUsers();

            ClearFields();
            Response.StatusCode = 302;
            Response.AddHeader("Location", "UserMaster.aspx");
        }

    }
}


