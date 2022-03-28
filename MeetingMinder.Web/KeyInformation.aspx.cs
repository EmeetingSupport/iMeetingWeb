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
using System.Text;
using System.IO;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;

namespace MeetingMinder.Web
{
    public partial class KeyInformation : System.Web.UI.Page
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

                //Bind key information
                // BindKeyInfo();

                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindKeyInfo(EntityId);
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                //Bind suffix to dropdown
                BindSuffix();

            }
        }

        /// <summary>
        /// Bind suffix to drop down list
        /// </summary>
        private void BindSuffix()
        {
            string strSuffix = ConfigurationManager.AppSettings["Suffix"].ToString();
            string[] Suffix = strSuffix.Split(',');
            if (Suffix.Count() > 0)
            {
                ddlSuffix.DataSource = Suffix;
                ddlSuffix.DataBind();


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
                    //Bind key information
                    BindKeyInfo(Guid.Parse(strEntityId));
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
        /// Bind key information data to grid
        /// </summary>
        private void BindKeyInfo(Guid EntityId)
        {
            try
            {

                grdKeyInfo.DataSource = KeyInformationDataProvider.Instance.GetKeyInfoByEntity(EntityId);
                grdKeyInfo.DataBind();

                hlChangeOrder.Visible = true;
                hlChangeOrder.NavigateUrl = "Reorder.aspx";//?dir=1&est=" + ddlEntity.SelectedValue;
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
        /// Get formated description
        /// </summary>
        /// <param name="Description">object specifying Description</param>
        /// <returns></returns>
        protected string GetDesc(object Description)
        {
            string desc = System.Net.WebUtility.HtmlDecode(Convert.ToString(Description));
            if (desc.Length > 200)
            {
                desc = desc.Substring(0, 200) + "...";
            }
            WebUtility.HtmlEncode(desc);
            return desc;
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
                UserAcess objUser = new UserAcess();

                if (txtName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtName);
                    if (!objUser.isValidChar(txtName.Text))
                    {
                        txtName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtName.Text))
                    {
                        txtName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid name.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter name.";
                    Error.Visible = true;
                    return;
                }

                if (txtDescription.Text != "")
                {
                    string desc = System.Net.WebUtility.HtmlDecode(Convert.ToString(txtDescription.Text)).Replace("<p>", "").Replace("</p>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");

                    //   objUser.TextHtmlEncode(ref txtDescription);

                    bool isValid = Regex.IsMatch(desc, "^[a-z0-9 ]+$", RegexOptions.IgnoreCase);

                    if (!isValid)
                    {
                        txtDescription.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                    if (!objUser.isValidChar(desc))
                    {
                        txtDescription.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(desc))
                    {
                        txtDescription.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid description.";
                        Error.Visible = true;
                        return;
                    }

                    //if (!objUser.isValidChar(txtDescription.Text))
                    //{
                    //    txtDescription.Text = "";
                    //    ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                    //    Error.Visible = true;
                    //    return;
                    //}
                    //if (!objUser.CSVValidation(txtDescription.Text))
                    //{
                    //    txtDescription.Text = "";
                    //    ((Label)Error.FindControl("lblError")).Text = " Please enter valid description.";
                    //    Error.Visible = true;
                    //    return;
                    //}
                }


                if (txtDesignation.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtDesignation);
                    if (!objUser.isValidChar(txtDesignation.Text))
                    {
                        txtDesignation.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtDesignation.Text))
                    {
                        txtDesignation.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid designation.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter designation.";
                    Error.Visible = true;
                    return;
                }

                if (hdnKeyInfoId.Value == "")
                {
                    if (!fuPhoto.HasFile)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please upload photograph.";
                        Error.Visible = true;
                        return;
                    }
                }
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    string str_Photo = "";
                    KeyInformationDomain objKeyInfo = new KeyInformationDomain();
                    objKeyInfo.Description = txtDescription.Text.Trim().EncodeHtml();
                    objKeyInfo.Designation = txtDesignation.Text;
                    objKeyInfo.Name = txtName.Text;
                    objKeyInfo.Suffix = ddlSuffix.SelectedValue;
                    if (fuPhoto.HasFile)
                    {
                        if (!objUser.isValidChar_old(fuPhoto.FileName))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                            Error.Visible = true;
                            return;
                        }

                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);
                        string contentType = fuPhoto.PostedFile.ContentType;
                        string extn = Path.GetExtension(fuPhoto.FileName);
                        if (contentType.ToLower().Contains("image/"))
                        {
                            if (extn.ToLower().Equals(".jpg") || extn.ToLower().Equals(".jpeg") || extn.ToLower().Equals(".png") || extn.ToLower().Equals(".bmp"))
                            {
                                System.IO.BinaryReader r = new System.IO.BinaryReader(fuPhoto.PostedFile.InputStream);
                                string fileclass = "";
                                byte buffer;
                                try
                                {
                                    buffer = r.ReadByte();
                                    fileclass = buffer.ToString();
                                    buffer = r.ReadByte();
                                    fileclass += buffer.ToString();

                                }
                                catch (Exception ex)
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                    Error.Visible = true;

                                    LogError objEr = new LogError();
                                    objEr.HandleException(ex);
                                }

                                if (fileclass == "255216" || fileclass == "7173" || fileclass == "6677" || fileclass == "13780")
                                {
                                    string ext = Path.GetExtension(fuPhoto.FileName);

                                    str_Photo = DateTime.Now.Ticks + ext;
                                    fuPhoto.PostedFile.SaveAs(Server.MapPath(savePath + str_Photo));

                                    if (ViewState["file"] != null)
                                    {
                                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                                        {
                                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                            Error.Visible = true;
                                            return;
                                        }
                                        string fileName = Convert.ToString(ViewState["file"]);
                                        if (fileName != "sample_logo.gif")
                                            File.Delete(Server.MapPath(savePath + fileName));
                                        ViewState["file"] = null;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                    Error.Visible = true;
                                    return;
                                }
                                r.Close();
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid image file uploaded.";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Invalid image Uploaded file";
                            Error.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        if (ViewState["file"] != null)
                        {
                            str_Photo = Convert.ToString(ViewState["file"]);
                        }
                    }

                    objKeyInfo.UpdatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                    objKeyInfo.Photograph = str_Photo;
                    //Update
                    if (hdnKeyInfoId.Value != "")
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Failed", Convert.ToString(hdnKeyInfoId.Value), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objKeyInfo.KeyInformationId = Guid.Parse(Convert.ToString(hdnKeyInfoId.Value));
                        bool status = KeyInformationDataProvider.Instance.UpdateKeyInfo(objKeyInfo);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Key information updated successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objKeyInfo);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Success", Convert.ToString(objKeyInfo.KeyInformationId), "Key information updated successfully :- " + encrypt + "");

                            BindKeyInfo(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Failed", Convert.ToString(objKeyInfo.KeyInformationId), "Key information updation failed");

                            ((Label)Error.FindControl("lblError")).Text = " Key information updation failed ";
                            Error.Visible = true;
                        }
                    }
                    //insert
                    else
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objKeyInfo.EntityId = Guid.Parse(ddlEntity.SelectedValue);
                        objKeyInfo.CreatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                        objKeyInfo = KeyInformationDataProvider.Instance.Insert(objKeyInfo);
                        if (!objKeyInfo.KeyInformationId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Key information inserted successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objKeyInfo);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Success", Convert.ToString(objKeyInfo.KeyInformationId), "Key information inserted successfully :- " + encrypt + "");

                            BindKeyInfo(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objKeyInfo.UpdatedBy), "Board of Director", "Failed", Convert.ToString(objKeyInfo.KeyInformationId), "Key information insertion failed");

                            ((Label)Error.FindControl("lblError")).Text = " Key information insertion failed ";
                            Error.Visible = true;
                        }
                    }

                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Board of Director", "Failed", "", "Sorry you are not maker");

                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                    Error.Visible = true;
                    rfvPhoto.Enabled = true;
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
            ViewState["file"] = null;
            txtName.Text = "";
            txtDescription.Text = "";
            txtDesignation.Text = "";
            hdnKeyInfoId.Value = "";
            btnInsert.Text = "Save";
            rfvPhoto.Enabled = true;
        }

        /// <summary>
        /// gridview pageindex change event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdKeyInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdKeyInfo.PageIndex = e.NewPageIndex;
            BindKeyInfo(Guid.Parse(ddlEntity.SelectedValue));
        }

        /// <summary>
        /// gridview row command event event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdKeyInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    if (e.CommandName.ToLower().Equals("edit"))
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        Guid KeyInfoId = Guid.Parse(Convert.ToString(e.CommandArgument));
                        KeyInformationDomain objKeyInfo = KeyInformationDataProvider.Instance.Get(KeyInfoId);
                        txtDescription.Text = objKeyInfo.Description;
                        txtDesignation.Text = objKeyInfo.Designation;
                        txtName.Text = objKeyInfo.Name;
                        btnInsert.Text = "Update";
                        hdnKeyInfoId.Value = objKeyInfo.KeyInformationId.ToString();
                        ddlSuffix.SelectedValue = objKeyInfo.Suffix;

                        ViewState["file"] = objKeyInfo.Photograph;
                        rfvPhoto.Enabled = false;
                    }
                    if (e.CommandName.ToLower().Equals("delete"))
                    {
                        ClearData();
                        if (!objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        Guid KeyInfoId = Guid.Parse(Convert.ToString(e.CommandArgument));
                        KeyInformationDomain objKeyInfo = KeyInformationDataProvider.Instance.Get(KeyInfoId);
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["KeyInfo"]);
                        try
                        {
                            File.Delete(Server.MapPath(savePath + objKeyInfo.Photograph));
                        }
                        catch (Exception ex)
                        {
                            ((Label)Error.FindControl("lblError")).Text = ex.Message;
                            Error.Visible = true;
                        }
                        bool status = KeyInformationDataProvider.Instance.DeleteKeyInfoById(KeyInfoId);
                        if (status)
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Board of Director", "Success", Convert.ToString(KeyInfoId), "Key information deleted successfully");

                            ((Label)Info.FindControl("lblName")).Text = " Key information deleted successfully";
                            Info.Visible = true;
                            BindKeyInfo(Guid.Parse(ddlEntity.SelectedValue));
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Board of Director", "Failed", Convert.ToString(KeyInfoId), "Key information deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "Key information deletion failed";
                            Error.Visible = true;
                        }
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Board of Director", "Failed", "", "Sorry you are not maker");

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
        /// gridview row delete event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdKeyInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Gridview row editing event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdKeyInfo_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdKeyInfo_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)KeyInformationDataProvider.Instance.GetKeyInfoByEntity(Guid.Parse(ddlEntity.SelectedValue)).AsDataTable();
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

                grdKeyInfo.DataSource = dv;
                grdKeyInfo.DataBind();
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