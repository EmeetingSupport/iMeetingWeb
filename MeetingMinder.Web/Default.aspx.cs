using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;

namespace MeetingMinder.Web
{
    public partial class Default : System.Web.UI.Page
    {
        public string userCount ="0";
        public string directorCount = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                System.Data.DataSet ds = UserDataProvider.Instance.GetCntOfUserAndDirectorsWithRollByEntity(EntityId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    userCount = ds.Tables[0].Rows[0]["UserCount"].ToString();
                    directorCount = ds.Tables[1].Rows[0]["BoardOfDirectorCount"].ToString();
                }
                else
                {
                    userCount = "0";
                    directorCount = "0";
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

        
    }
}