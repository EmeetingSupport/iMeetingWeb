using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class en : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Sbi"] == "12345" || Request.QueryString["a"] == "yes123")
            {
                form1.Visible = true;
            }
            else
            {
                Response.Redirect("Error.aspx?ErrorCode=0");
                form1.Visible = false;
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["a"] != null && Request.QueryString["a"] == "yes123")
            {
                lblMsg.Text = AesEncryption.EncryptString(txtDomain.Text);
            }
            else
            {
                lblMsg.Text = Encryptor.EncryptString(txtDomain.Text);
            }
        }

        protected void btnTest_Click1(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["a"] != null && Request.QueryString["a"] == "yes123")
                {
                    lblMsg.Text = AesEncryption.DecryptString(txtDomain.Text);
                }
                else
                {
                    lblMsg.Text = Encryptor.DecryptString(txtDomain.Text);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }
    }
}