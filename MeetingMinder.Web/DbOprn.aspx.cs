using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MM.Data;

namespace MeetingMinder.Web
{
    public partial class DbOprn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["tppaswd"] != null)
            {
                txtTest.Visible = true;
                btnTest.Visible = true;
            }
            else
            {
                txtTest.Visible = false;
                btnTest.Visible = false;
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            DataProvider objData = new DataProvider();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(objData.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(txtTest.Text, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

    }
}