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
    public partial class GlobalSetting : System.Web.UI.Page
    {
        /// <summary>
        /// Page load event
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
                    BindIPA(EntityId);
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion
            }
        }

        /// <summary>
        /// Bind Photos to grid
        /// </summary>
        private void BindIPA(Guid EntityId)
        {
            try
            {
                IList<IPADomain> objAboutUs = IPADataprovider.Instance.GetIPADomainByEntity(EntityId);
                if (objAboutUs.Count > 0)
                {
                    grdPhoto.DataSource = objAboutUs;
                    grdPhoto.DataBind();
                    //divdetails.Visible = false;
                }
                else
                {
                    grdPhoto.DataSource = objAboutUs;
                    grdPhoto.DataBind();
                    //divdetails.Visible = true;
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
                IList<GlobalSettingDomain> objGlobal = GlobalSettingDataProvider.Instance.GetGlobalSettingByEntity(EntityId).OrderBy(p => p.Key).ToList();
                GlobalSettingDomain item = objGlobal.FirstOrDefault(p => p.Key == "Encryption Key");

                if (item != null)
                {
                    objGlobal.Remove(item);
                }

                item = objGlobal.FirstOrDefault(p => p.Key == "Encryption `Key`");

                if (item != null)
                {
                    objGlobal.Remove(item);
                }

                GlobalSettingDomain items = objGlobal.FirstOrDefault(p => p.Key == "Past Meeting Count (In Number)");

                if (items != null)
                {
                    objGlobal.Remove(items);
                }

                GlobalSettingDomain itemss = objGlobal.FirstOrDefault(p => p.Key == "Meeting Expiry (In Days)");

                if (itemss != null)
                {
                    itemss.Key = "Other Expiry [Proceeding & Noting] (In Days)";
                }

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
        /// gridview row command event event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdPhoto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {


                    if (e.CommandName.ToLower().Equals("edititem"))
                    {
                        Int32 AboutUsId = Int32.Parse(Convert.ToString(e.CommandArgument));
                        IPADomain objPDFTemplate = IPADataprovider.Instance.Get(AboutUsId);
                        txtName.Text = objPDFTemplate.IPAName;
                        txtLink.Text = objPDFTemplate.IPA;
                        btnInsert.Text = "Update";
                        hdnPhotoId.Value = objPDFTemplate.Id.ToString();
                        ViewState["file"] = objPDFTemplate.IPA;

                        divdetails.Visible = true;

                    }

                    if (e.CommandName.ToLower().Equals("delete"))
                    {
                        Int32 AboutUsId = Int32.Parse(Convert.ToString(e.CommandArgument));
                        IPADomain objPDFTemplateDomain = IPADataprovider.Instance.Get(AboutUsId);
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IPA"]);
                        File.Delete(Server.MapPath(savePath + objPDFTemplateDomain.IPA));

                        bool status = IPADataprovider.Instance.Delete(AboutUsId);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "IPA deleted successfully";
                            Info.Visible = true;
                            BindIPA(Guid.Parse(ddlEntity.SelectedValue));
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "IPA deletion failed";
                            Error.Visible = true;
                        }
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
                UserAcess objUser = new UserAcess();

                if (txtVal.Text != "")
                {
                    if (!objUser.isValidChar(txtVal.Text))
                    {
                        txtVal.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter value";
                    Error.Visible = true;
                    return;
                }
                Guid id = Guid.Parse(grdGlobal.DataKeys[e.RowIndex].Value.ToString());
                Label lblKey = (Label)grdGlobal.Rows[e.RowIndex].FindControl("lblKey");
                GlobalSettingDomain objGlobal = new GlobalSettingDomain();
                objGlobal.Id = id;
                objGlobal.Value = txtVal.Text.Trim();
                if (lblKey.Text.ToLower().Equals("encryption key"))
                {
                    objGlobal.Value = MM.Core.Encryptor.EncryptString(txtVal.Text.Trim());
                }

                if (txtVal.Text.Length == 0)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Global setting updatation failed. Please enter value in text box";
                    Error.Visible = true;
                }
                if (lblKey.Text.ToLower().Equals("encryption key"))
                {
                    if (txtVal.Text.Length == 0)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Key must be 16 characters long without whitespace.";
                        Error.Visible = true;
                        return;
                    }
                }
                bool status = GlobalSettingDataProvider.Instance.Update(objGlobal);
                if (status)
                {
                    if (lblKey.Text.ToLower().Equals("encryption key"))
                    {
                        Session["EncryptionKey"] = txtVal.Text.Trim();
                    }
                    ((Label)Info.FindControl("lblName")).Text = "Global setting updated successfully!";
                    Info.Visible = true;

                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objGlobal);
                    var encrypt = Encryptor.EncryptString(json);

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Global Setting", "Success", Convert.ToString(objGlobal.Id), "Global setting updated successfully! :- " + encrypt + "");

                    grdGlobal.EditIndex = -1;
                    BindGlobalVal(Guid.Parse(ddlEntity.SelectedValue));
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "Global Setting", "Failed", Convert.ToString(objGlobal.Id), "Global setting updatation failed");

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


        /// <summary>
        /// button save click event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    string str_Photo = "";
                    IPADomain objPDFTemplate = new IPADomain();
                    objPDFTemplate.IPAName = txtName.Text.Trim();
                    objPDFTemplate.IPA = txtLink.Text.Trim();
                    if (hdnPhotoId.Value != "")
                    {
                        objPDFTemplate.Id = Int32.Parse(Convert.ToString(hdnPhotoId.Value));

                        bool status = IPADataprovider.Instance.Update(objPDFTemplate);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " IPA  updated successfully";
                            Info.Visible = true;
                            BindIPA(Guid.Parse(Session["EntityId"].ToString()));
                            ClearData();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded ";
                            Error.Visible = true;
                        }
                    }
                    //insert
                    else
                    {
                        objPDFTemplate.EntityId = Guid.Parse(Session["EntityId"].ToString());
                        objPDFTemplate.CreatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                        objPDFTemplate = IPADataprovider.Instance.Insert(objPDFTemplate);
                        if (objPDFTemplate.Id > 0)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " IPA inserted successfully";
                            Info.Visible = true;
                            BindIPA(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = " IPA insertion failed ";
                            Error.Visible = true;
                        }
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
                //divdetails.Visible = false;
                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// button cancel click event.
        /// </summary>
        /// <param name="sender">object specifying sender</param>
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
            divdetails.Visible = false;
            txtName.Text = "";
            hdnPhotoId.Value = "";
            btnInsert.Text = "Save";
            txtLink.Text = "";
        }

    }
}