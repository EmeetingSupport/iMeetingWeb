using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // IList<MM.Domain.ABoutUSDomain> objList = new List<MM.Domain.ABoutUSDomain>();
           // objList.Add(new MM.Domain.ABoutUSDomain { CreatedBy = Guid.NewGuid(), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, EncryptionKey = "encr", EntityId = Guid.NewGuid(), Image = "img", ModifiedBy = Guid.NewGuid(), Text = "tenxt" });
           // objList.Add(new MM.Domain.ABoutUSDomain { CreatedBy = Guid.NewGuid(), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, EncryptionKey = "encr1", EntityId = Guid.NewGuid(), Image = "img1", ModifiedBy = Guid.NewGuid(), Text = "tenxt1" });
           // objList.Add(new MM.Domain.ABoutUSDomain { CreatedBy = Guid.NewGuid(), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, EncryptionKey = "encr2", EntityId = Guid.NewGuid(), Image = "img2", ModifiedBy = Guid.NewGuid(), Text = "tenxt2" });

           //System.Data.SqlClient.SqlConnection objcon =new System.Data.SqlClient.SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=Yes16Sept2015;uid=sa;password=pass123!@#");
           // objcon.Open();

           //   SqlCommand cmd = new SqlCommand("dbo.InsertMyDataTable", objcon);
           //cmd.CommandType = CommandType.StoredProcedure;

           //SqlParameter tvparam = cmd.Parameters.AddWithValue("@dt", objList.AsDataTable());
           //   // tvparam.SqlDbType = SqlDbType.Structured;
           //     cmd.ExecuteNonQuery();
             

            //Response.Write(Request.Url.Host);
            //Response.Write(" "+(Request.Url.Authority));
          //  Uri uri = new Uri("http://www.mywebsite.com:80/pages/page1.aspx");
          ////  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host +" <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath + "<br/>");
          //    uri = new Uri("http://10.0.10.1:80/pages/page1.aspx");
          //  //  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host + " <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath + "<br/>");

          //  uri = new Uri("http://10.0.10.1/pages/page1.aspx");
          //  //  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host + " <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath + "<br/>");
          //  uri = new Uri("http://yesbank.aspnetdevelopment.in/EmailNotifications.aspx");
          //  //  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host + " <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath + "<br/>");

          //  uri = new Uri("http://aspnetdevelopment.in/EmailNotifications.aspx");
          //  //  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host + " <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath + "<br/>");
          //  uri = new Uri("http://www.aspnetdevelopment.in/EmailNotifications.aspx");
          //  //  string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
          //  Response.Write(uri.Host + " <br/>");
          //  Response.Write(uri.DnsSafeHost + "<br/>");
          //  Response.Write(Request.ApplicationPath);
        }
    }
}