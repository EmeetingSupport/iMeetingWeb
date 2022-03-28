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

namespace MeetingMinder.Web
{
    public partial class PhotoGallery : System.Web.UI.Page
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
                //Bind Entity List
                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindPhotoGallery(EntityId);
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                //Bind key information
                // BindPhotoGallery();
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
                    BindPhotoGallery(Guid.Parse(strEntityId));
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
        /// Bind Photos to grid
        /// </summary>
        private void BindPhotoGallery(Guid EntityId)
        {
            try
            {
                grdPhoto.DataSource = PhotoGalleryDataProvider.Instance.GetPhotoByEntity(EntityId);
                grdPhoto.DataBind();
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
            string desc = Convert.ToString(Description);
            if (desc.Length > 200)
            {
                desc = desc.Substring(0, 200) + "...";
            }
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

                if (txtDescription.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtDescription);
                    if (!objUser.isValidChar(txtDescription.Text))
                    {
                        txtDescription.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtDescription.Text))
                    {
                        txtDescription.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid description.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter description.";
                    Error.Visible = true;
                    return;
                }

                if (hdnPhotoId.Value == "")
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
                    PhotoGalleryDomain objPhoto = new PhotoGalleryDomain();
                    objPhoto.Description = txtDescription.Text.Trim().EncodeHtml();

                    if (fuPhoto.HasFile)
                    {
                        if (!objUser.isValidChar_old(fuPhoto.FileName))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                            Error.Visible = true;
                            return;
                        }

                        string extn = Path.GetExtension(fuPhoto.FileName);
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        string contentType = fuPhoto.PostedFile.ContentType;
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
                                r.Close();
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid image uploaded file";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Invalid image uploaded file";
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

                    objPhoto.UpdatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                    objPhoto.Photograph = str_Photo;
                    //Update
                    if (hdnPhotoId.Value != "")
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Failed", Convert.ToString(hdnPhotoId.Value), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objPhoto.PhotoGalleryId = Guid.Parse(Convert.ToString(hdnPhotoId.Value));
                        bool status = PhotoGalleryDataProvider.Instance.UpdatePhoto(objPhoto);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Image  updated successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objPhoto);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Success", Convert.ToString(objPhoto.PhotoGalleryId), "Image  updated successfully :- " + encrypt + "");

                            BindPhotoGallery(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Failed", Convert.ToString(objPhoto.PhotoGalleryId), "Image updation failed");

                            ((Label)Error.FindControl("lblError")).Text = " Image updation failed ";
                            Error.Visible = true;
                        }
                    }
                    //insert
                    else
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objPhoto.EntityId = Guid.Parse(ddlEntity.SelectedValue);
                        objPhoto.CreatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                        objPhoto = PhotoGalleryDataProvider.Instance.Insert(objPhoto);
                        if (!objPhoto.PhotoGalleryId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Image inserted successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objPhoto);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Success", Convert.ToString(objPhoto.PhotoGalleryId), "Image inserted successfully :- " + encrypt + "");

                            BindPhotoGallery(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objPhoto.UpdatedBy), "Photo Gallery", "Failed", Convert.ToString(objPhoto.PhotoGalleryId), "Image insertion failed");

                            ((Label)Error.FindControl("lblError")).Text = " Image insertion failed ";
                            Error.Visible = true;
                        }
                    }

                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Photo Gallery", "Failed", "", "Sorry you are not maker");

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
            txtDescription.Text = "";
            hdnPhotoId.Value = "";
            btnInsert.Text = "Save";
            rfvPhoto.Enabled = true;
            lnkView.Visible = false;
        }

        /// <summary>
        /// gridview pageindex change event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdPhoto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdPhoto.PageIndex = e.NewPageIndex;
            BindPhotoGallery(Guid.Parse(ddlEntity.SelectedValue));
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
                        Guid PhotoId = Guid.Parse(Convert.ToString(e.CommandArgument));
                        PhotoGalleryDomain objPhoto = PhotoGalleryDataProvider.Instance.Get(PhotoId);
                        txtDescription.Text = objPhoto.Description;

                        btnInsert.Text = "Update";
                        hdnPhotoId.Value = objPhoto.PhotoGalleryId.ToString();
                        ViewState["file"] = objPhoto.Photograph;
                        rfvPhoto.Enabled = false;
                        lnkView.Visible = true;
                    }

                    if (e.CommandName.ToLower().Equals("delete"))
                    {
                        ClearData();
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Photo Gallery", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        Guid PhotoId = Guid.Parse(Convert.ToString(e.CommandArgument));
                        PhotoGalleryDomain objKeyInfo = PhotoGalleryDataProvider.Instance.Get(PhotoId);
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        File.Delete(Server.MapPath(savePath + objKeyInfo.Photograph));

                        bool status = PhotoGalleryDataProvider.Instance.DeletePhotoById(PhotoId);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Image deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Photo Gallery", "Success", Convert.ToString(PhotoId), "Image deleted successfully");

                            BindPhotoGallery(Guid.Parse(ddlEntity.SelectedValue));
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Photo Gallery", "Failed", Convert.ToString(PhotoId), "Key information deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "Key information deletion failed";
                            Error.Visible = true;
                        }
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Convert.ToString(Session["UserId"]))), "Photo Gallery", "Failed", "", "Sorry you are not maker");

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
        /// View uploaded file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["file"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.WriteFile(FilePath);
                Response.End();
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
        protected void grdPhoto_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Gridview row editing event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdPhoto_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdPhoto_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)PhotoGalleryDataProvider.Instance.GetPhotoByEntity(Guid.Parse(ddlEntity.SelectedValue)).AsDataTable();
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

                grdPhoto.DataSource = dv;
                grdPhoto.DataBind();
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