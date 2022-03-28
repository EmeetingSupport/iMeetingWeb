using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.IO;
using System.Data;

namespace MeetingMinder.Web
{
    public partial class YearlyMeeting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //bind Gloabal list
                //  BindGlobalVal();

                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindGlobalVal(EntityId);
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion
            }
        }

        /// <summary>
        /// Bind Entity list to drop down
        /// </summary>
        private void BindEntity()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); ;
                ddlEntity.DataBind();
                ddlEntity.DataTextField = "EntityName";
                ddlEntity.DataValueField = "EntityId";
                ddlEntity.DataBind();
                ddlEntity.Items.Insert(0, new ListItem("Select Entity", "0"));
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
        /// drop down list Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlEntity.SelectedValue;
                if (strEntityId != "0")
                {
                    //Bind Gloabal information
                    BindGlobalVal(Guid.Parse(strEntityId));
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
        /// Bind global values
        /// </summary>
        private void BindGlobalVal(Guid EntityId)
        {
            try
            {
                IList<GlobalSettingDomain> objGlobal = GlobalSettingDataProvider.Instance.GetGlobalSettingByEntity(EntityId);
                grdGlobal.DataSource = objGlobal;
                grdGlobal.DataBind();
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
        /// Gridview pageindexchange event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdGlobal.PageIndex = e.NewPageIndex;
                BindGlobalVal(Guid.Parse(ddlEntity.SelectedValue));
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
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// Gridview rowediting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grdGlobal.EditIndex = e.NewEditIndex;
                BindGlobalVal(Guid.Parse(ddlEntity.SelectedValue));
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
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)GlobalSettingDataProvider.Instance.GetGlobalSettingByEntity(Guid.Parse(ddlEntity.SelectedValue)).AsDataTable();
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

                grdGlobal.DataSource = dv;
                grdGlobal.DataBind();
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
        /// Gridview row update event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TextBox txtVal = (TextBox)grdGlobal.Rows[e.RowIndex].FindControl("txtVal");
                Guid id = Guid.Parse(grdGlobal.DataKeys[e.RowIndex].Value.ToString());
                GlobalSettingDomain objGlobal = new GlobalSettingDomain();
                objGlobal.Id = id;
                objGlobal.Value = txtVal.Text.Trim();
                bool status = GlobalSettingDataProvider.Instance.Update(objGlobal);
                if (status)
                {
                    ((Label)Info.FindControl("lblName")).Text = "Global setting updated successfully!";
                    Info.Visible = true;
                    grdGlobal.EditIndex = -1;
                    BindGlobalVal(Guid.Parse(ddlEntity.SelectedValue));
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Global setting updatation failed";
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
        /// Gridview row cancelling event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdGlobal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grdGlobal.EditIndex = -1;
                BindGlobalVal(Guid.Parse(ddlEntity.SelectedValue));
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