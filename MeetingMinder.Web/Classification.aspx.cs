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

namespace MeetingMinder.Web
{
    public partial class Classification : System.Web.UI.Page
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
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                //bind user list  to grid
                //BindAgenda();

                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindAgenda(EntityId);
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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList();
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
                    //Bind agenda
                    BindAgenda(Guid.Parse(strEntityId));
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
        /// Bind master agendas to grid
        /// </summary>
        private void BindAgenda(Guid EntityId)
        {
            try
            {               
                grdAgenda.DataSource = AgendaDataProvider.Instance.GetAllMasterClassification(EntityId);
                grdAgenda.DataBind();

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
        /// Button insert click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    AgendaDomain objAgenda = new AgendaDomain();
                    objAgenda.AgendaName = txtClassification.Text;
                    objAgenda.AgendaNote = txtClassification.Text;
                    objAgenda.MeetingId = Guid.Parse("00000000-1111-1111-0000-000000000000");
                    Guid EntityId = Guid.Parse(ddlEntity.SelectedValue);
                    objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    if (hdnAgendaId.Value == "")
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        AgendaDataProvider.Instance.Insert(objAgenda, "", EntityId);
                        ((Label)Info.FindControl("lblName")).Text = "Agenda inserted successfully";
                        BindAgenda(Guid.Parse(ddlEntity.SelectedValue));
                        ClearData();

                        Info.Visible = true;
                    }
                    else
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objAgenda.AgendaId = Guid.Parse(hdnAgendaId.Value);
                        bool status = AgendaDataProvider.Instance.Update(objAgenda);

                        ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully";
                        BindAgenda(Guid.Parse(ddlEntity.SelectedValue));
                        ClearData();
                        Info.Visible = true;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
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
        /// Button cancel click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// clear form data
        /// </summary>
        private void ClearData()
        {
            txtClassification.Text = "";
            hdnAgendaId.Value = "";
            btnInsert.Text = "Save";

        }

        /// <summary>
        /// Gridview paging index
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewDeleteEventArgs specifying e </param>
        protected void grdAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAgenda.PageIndex = e.NewPageIndex;
            BindAgenda(Guid.Parse(ddlEntity.SelectedValue));
        }

        /// <summary>
        /// Gridview rowcommand event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewDeleteEventArgs specifying e </param>
        protected void grdAgenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                //Edit
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    if (isMaker)
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        txtClassification.Text = objAgenda.AgendaName;
                        hdnAgendaId.Value = objAgenda.AgendaId.ToString();
                        btnInsert.Text = "Update";
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
                }
                //delete 
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    if (isMaker)
                    {
                        if (!objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        bool status = AgendaDataProvider.Instance.Delete(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        ((Label)Info.FindControl("lblName")).Text = "Agenda deleted successfully";
                        Info.Visible = true;
                        BindAgenda(Guid.Parse(ddlEntity.SelectedValue));
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
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
        /// Gridview RowDeleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewDeleteEventArgs specifying e </param>
        protected void grdRoll_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Gridview RowEditing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewDeleteEventArgs specifying e </param>
        protected void grdAgenda_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview Sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewDeleteEventArgs specifying e </param>
        protected void grdAgenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)AgendaDataProvider.Instance.GetAllMasterAgenda(Guid.Parse(ddlEntity.SelectedValue), Guid.Parse("00000000-1111-1111-0000-000000000000")).AsDataTable();
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

                grdAgenda.DataSource = dv;
                grdAgenda.DataBind();
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