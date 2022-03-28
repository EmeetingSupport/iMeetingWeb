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
    public partial class AdMaster : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                BindAdMasterList();
            }
        }

        /// <summary>
        /// Bind AdMaster List
        /// </summary>
        private void BindAdMasterList()
        {
            try
            {
                grdAdDetails.DataSource = AdMasterDataProvider.Instance.Get();
                grdAdDetails.DataBind();
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
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    if (txtFirstName.Text != "" && txtLastName.Text != "" && txtUserId.Text != "")
                    {
                        AdDomain objAdDomain = new AdDomain();
                        objAdDomain.FirstName = txtFirstName.Text;
                        objAdDomain.LastName = txtLastName.Text;
                        objAdDomain.UserId = txtUserId.Text;
                        objAdDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        objAdDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                        if (hdnAdMasterId.Value != null && hdnAdMasterId.Value != "")
                        {
                            objAdDomain.AdMasterId = Convert.ToInt16(hdnAdMasterId.Value);
                            bool status = AdMasterDataProvider.Instance.Upadate(objAdDomain);
                            if (status)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Ad details Updated successfully";
                                ClearData();
                                BindAdMasterList();
                                Info.Visible = true;
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Ad details Update failed";
                                Error.Visible = true;
                            }
                        }
                        else
                        {
                            objAdDomain = AdMasterDataProvider.Instance.Insert(objAdDomain);
                            ((Label)Info.FindControl("lblName")).Text = "Ad details inserted successfully";
                            ClearData();
                            BindAdMasterList();
                            Info.Visible = true;
                        }

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please fill all required details";
                        Error.Visible = true;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                    Error.Visible = true;
                    Info.Visible = false;
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
        /// Clear form data
        /// </summary>
        private void ClearData()
        {
            txtUserId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            hdnAdMasterId.Value = "";
            btnInsert.Text = "Save";
        }

        /// <summary>
        /// button cancel click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Gridview page index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAdDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAdDetails.PageIndex = e.NewPageIndex;
                BindAdMasterList();
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
        protected void grdAdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
            try
            {
                if (e.CommandName.ToLower().Equals("edit"))
                {

                    if (isMaker)
                    {
                        int AdMasterId = Convert.ToInt16(e.CommandArgument);
                        AdDomain objAddomain = AdMasterDataProvider.Instance.Get(AdMasterId);
                        if (objAddomain != null)
                        {
                            txtFirstName.Text = objAddomain.FirstName;
                            txtLastName.Text = objAddomain.LastName;
                            txtUserId.Text = objAddomain.UserId;
                            hdnAdMasterId.Value = Convert.ToString(objAddomain.AdMasterId);
                            btnInsert.Text = "Update";
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }

                }
                if (e.CommandName.ToLower().Equals("delete"))
                {

                    if (isMaker)
                    {
                        int AdMasterId = Convert.ToInt16(e.CommandArgument);
                        bool status = AdMasterDataProvider.Instance.Delete(AdMasterId);
                        if (status)
                        {
                            ClearData();
                            BindAdMasterList();
                            ((Label)Info.FindControl("lblName")).Text = "Ad details deleted successfully";
                            Info.Visible = true;
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Ad details deletion failed";
                            Error.Visible = true;
                        }
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
        /// Gridview row delete event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAdDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Gridview editing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAdDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview sorting change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAdDetails_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)AdMasterDataProvider.Instance.Get().AsDataTable();
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

                grdAdDetails.DataSource = dv;
                grdAdDetails.DataBind();
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