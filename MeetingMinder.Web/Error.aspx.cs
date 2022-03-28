using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeetingMinder.Web
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //Response.Headers.Remove("Server");
                    string sErrorCode = Request.QueryString["ErrorCode"].ToString();

                    switch (sErrorCode)
                    {
                        case "401":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "403":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "404":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "405":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "408":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "414":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "500":
                            lblMessage.Text = "Oops! Something went wrong.";
                            break;
                        case "0":
                            lblMessage.Text = "Error: Something went wrong";
                            break;
                        default:
                            lblMessage.Text = "Oops! Something went wrong.\n";
                            break;
                    }
                    Server.ClearError();
                }
                catch (Exception ex)
                {
                }


            }

        }
        protected void lnkPreviousPage_Click(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                Response.Redirect("~/Login.aspx?PageName=Default");
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}