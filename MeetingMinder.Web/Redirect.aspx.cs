using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.Data;
using System.Web.Helpers;

namespace MeetingMinder.Web
{
    public partial class Redirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!IsPostBack)
            {
                //bind entity list
                BindEntity();                
            }

            if(IsPostBack)
            {
                //AntiForgery
                //AntiForgery.Validate();
            }
            
        }


        /// <summary>
        /// Bind Entity List
        /// </summary>
        private void BindEntity()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"].ToString());
                IList<UserEntityDomain> objUserEntityDomain = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); //EntityDataProvider.Instance.Get();
                ddlEntity.DataSource = objUserEntityDomain;
                ddlEntity.DataBind();
                ddlEntity.DataValueField = "EntityId";
                ddlEntity.DataTextField = "EntityName";
                ddlEntity.DataBind();
                ddlEntity.Items.Insert(0, new ListItem("Select Department", "0"));


                if (objUserEntityDomain.Count == 1)
                {
                    Session["EntityId"] = objUserEntityDomain[0].EntityId;
                    Session["EntityName"] = objUserEntityDomain[0].EntityName;
                    Session["EncryptionKey"] = objUserEntityDomain[0].EncryptionKey;
                    Response.Redirect("Default.aspx");
                }
                else if (objUserEntityDomain.Count > 1)
                {
                    Session["EntityCount"] = true;
                }
            }
            catch (Exception ex)
            {

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(ddlEntity.SelectedValue));
                Session["EntityId"] = objEntity.EntityId;
                Session["EntityName"] = objEntity.EntityName;
                Session["EncryptionKey"] = objEntity.EncryptionKey;

                IList<AccessRightDomain> objAccess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(Session["UserId"].ToString()));

                //var parent = from obj in objAccess where obj.

                RollDomain objRoll = RollDataProvider.Instance.GetRollByUserId(Guid.Parse(Session["UserId"].ToString()));

                Session["Roll"] = objRoll;
                Session["AccessRight"] = objAccess;
                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        
    }
}