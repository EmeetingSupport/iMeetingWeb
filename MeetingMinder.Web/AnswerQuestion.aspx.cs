﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MM.Domain;
using MM.Data;
using System.IO;
using MM.Core;
using System.Text;
using System.Web.Helpers;

namespace MeetingMinder.Web
{
    public partial class AnswerQuestion : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                try
                {
                    
                    //check user user session
                    if (Session["UserId"] == null)
                    {
                        Response.Redirect("Login.aspx");
                    }

                    //check user action only for update
                    if (Request.QueryString["act"] != null)
                    {
                        Session["update"] = true;
                    }
                    //Bind Question to dropdown

                    IList<SecurityQuestionDomain> objSecurity = SecurityQuestionDataProvider.Instance.Get();
                    ddlSecurityQuestion.DataSource = objSecurity;
                    ddlSecurityQuestion.DataBind();
                    ddlSecurityQuestion.DataTextField = "SecurityQuestion";
                    ddlSecurityQuestion.DataValueField = "SecurityQuestionId";
                    ddlSecurityQuestion.DataBind();
                    ddlSecurityQuestion.Items.Insert(0, new ListItem("Select Question", "0"));
                    if (!IsPostBack)
                    {
                        BindQuest();
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

            if (IsPostBack)
            {
                //AntiForgery
                //AntiForgery.Validate();
            }
        }

        /// <summary>
        /// Bind security question to drop down
        /// </summary>
        private void BindQuest()
        {
            try
            {
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                SecurityQuestionDomain objSecurityQuestionDomain = SecurityQuestionDataProvider.Instance.GetSecurityQuestion(UserId);
                if (objSecurityQuestionDomain != null)
                {
                    lblQuest.Text = objSecurityQuestionDomain.SecurityQuestion;
                    ddlSecurityQuestion.SelectedValue = objSecurityQuestionDomain.SecurityQuestionId.ToString();
                    ddlSecurityQuestion.Visible = false;
                    lblQuest.Visible = true;
                    rfvQuestion.Enabled = false;
                }
                else
                {
                    ddlSecurityQuestion.Visible = true;
                    lblQuest.Visible = false;
                    lblQuest.Visible = false;
                    rfvQuestion.Enabled = true;
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
        /// Button submit click Event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));

                string strUserAnswer = txtSecurityAnswer.Text;
                string strSelectedQuestion = Convert.ToString(ddlSecurityQuestion.SelectedValue);

                SecurityQuestionDomain objSecurityQuestion = SecurityQuestionDataProvider.Instance.GetSecurityQuestion(UserId);
                //check security question existance in datatbase 
                if (objSecurityQuestion != null && Session["update"] == null)
                {

                    string strDbAnswer = Convert.ToString(objSecurityQuestion.Answer);
                    string strQuestionId = Convert.ToString(objSecurityQuestion.SecurityQuestionId);


                    //check selected question is same as in database question
                    if (strSelectedQuestion.Equals(strQuestionId))
                    {
                        //check Answer  is same as in database answer
                        if (strDbAnswer.ToLower().Equals(strUserAnswer.ToLower()))
                        {
                            Session["ans"] = 1;
                            Response.Redirect("ChangePassword.aspx");
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Wrong question or answer! Can not process your request";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Wrong question or answer! Can not process your request";
                        Error.Visible = true;
                    }
                }

                //Insert Question 
                else
                {
                    bool isCorrect = SecurityQuestionDataProvider.Instance.InsertSecurityQuestion(strUserAnswer, Guid.Parse(strSelectedQuestion), UserId);
                    //update Question
                    if (Session["update"] != null)
                    {
                        ((Label)Info.FindControl("lblName")).Text = " Security question changed successfully.";
                        Session["update"] = null;
                        Info.Visible = true;

                        ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Security question changed successfully!');window.location='Default.aspx';</script>'");
                        return;
                    }
                    Session["ans"] = 1;
                    //change password
                    Response.Redirect("ChangePassword.aspx");

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
        /// Button cancel click Event
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
    }
}