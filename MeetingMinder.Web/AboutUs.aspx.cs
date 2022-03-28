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
using System.Security.Cryptography;

namespace MeetingMinder.Web
{
    public partial class AboutUs : System.Web.UI.Page
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

            if (Request.UrlReferrer != null)
            {
                string prvPage = Request.UrlReferrer.ToString();

                if (Session["AMessage"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["AMessage"].ToString();
                    Info.Visible = true;
                    Session["AMessage"] = null;
                }
            }
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
                    BindAboutUs(EntityId);
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
                    BindAboutUs(Guid.Parse(strEntityId));
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
        private void BindAboutUs(Guid EntityId)
        {
            try
            {
                IList<ABoutUSDomain> objAboutUs = AboutUsDataProvider.Instance.GetABoutUSDomainByEntity(EntityId);
                if (objAboutUs.Count > 0)
                {
                    grdPhoto.DataSource = objAboutUs;
                    grdPhoto.DataBind();
                    divdetails.Visible = false;
                }
                else
                {
                    grdPhoto.DataSource = objAboutUs;
                    grdPhoto.DataBind();
                    divdetails.Visible = true;
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
                if (hdnPhotoId.Value == "")
                {
                    if (!fuPhoto.HasFile)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please enter upload about us.";
                        Error.Visible = true;
                        return;
                    }
                }

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    string str_Photo = "";
                    ABoutUSDomain objAboutUs = new ABoutUSDomain();
                    objAboutUs.Text = txtDescription.Text.Trim();

                    if (fuPhoto.HasFile)
                    {
                        if (!objUser.isValidChar_old(fuPhoto.FileName))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                            Error.Visible = true;
                            return;
                        }

                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        string contentType = fuPhoto.PostedFile.ContentType;


                        if (contentType.ToLower().Contains("application/pdf"))
                        {
                            string ext = Path.GetExtension(fuPhoto.FileName);

                            if (ext.ToLower().Equals(".pdf"))
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

                                if (fileclass != "3780")
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid uploaded file";
                                    Error.Visible = true;
                                    return;
                                }

                                str_Photo = DateTime.Now.Ticks + ext;
                                string EncryptionKey = Session["EncryptionKey"].ToString();
                                if (hdnPhotoId.Value != "")
                                {
                                    EncryptionKey = ViewState["EncryptionKey"].ToString();
                                }
                                fuPhoto.PostedFile.SaveAs(Server.MapPath(savePath + str_Photo));
                                EncryptionHelper.EncryptString(Server.MapPath(savePath + str_Photo), EncryptionKey);


                                objAboutUs.EncryptionKey = EncryptionKey;
                                if (ViewState["file"] != null)
                                {
                                    string fileName = Convert.ToString(ViewState["file"]);

                                    File.Delete(Server.MapPath(savePath + fileName));
                                    ViewState["file"] = null;
                                }
                                r.Close();
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
                                Error.Visible = true;
                                return;
                            }
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
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

                        ((Label)Error.FindControl("lblError")).Text = "Please select file for update";
                        Error.Visible = true;
                        return;
                    }

                    objAboutUs.ModifiedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                    objAboutUs.Image = str_Photo;
                    //Update
                    if (hdnPhotoId.Value != "")
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Failed", Convert.ToString(hdnPhotoId.Value), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objAboutUs.Id = Int32.Parse(Convert.ToString(hdnPhotoId.Value));
                        bool status = AboutUsDataProvider.Instance.Update(objAboutUs);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " About us  updated successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAboutUs);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Success", Convert.ToString(objAboutUs.Id), "About us updated successfully :- " + encrypt + "");

                            BindAboutUs(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                            Response.StatusCode = 302;
                            Response.AddHeader("Location", "aboutus.aspx");
                            Session["AMessage"] = ((Label)Info.FindControl("lblName")).Text;
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Failed", Convert.ToString(objAboutUs.Id), "About us updation failed");

                            ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded ";
                            Error.Visible = true;
                            return;
                        }
                    }
                    //insert
                    else
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Failed", Convert.ToString(objAboutUs.Id), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objAboutUs.EntityId = Guid.Parse(ddlEntity.SelectedValue);
                        objAboutUs.CreatedBy = Guid.Parse(Convert.ToString(Session["UserId"]));
                        objAboutUs = AboutUsDataProvider.Instance.Insert(objAboutUs);
                        if (objAboutUs.Id > 0)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " About us inserted successfully";
                            Info.Visible = true;

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAboutUs);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Success", Convert.ToString(objAboutUs.Id), "About us inserted successfully :- " + encrypt + "");

                            BindAboutUs(Guid.Parse(ddlEntity.SelectedValue));
                            ClearData();
                            Response.StatusCode = 302;
                            Response.AddHeader("Location", "aboutus.aspx");
                            Session["AMessage"] = ((Label)Info.FindControl("lblName")).Text;
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAboutUs.ModifiedBy), "About US", "Failed", Convert.ToString(objAboutUs.Id), "About us insertion failed");

                            ((Label)Error.FindControl("lblError")).Text = " About us insertion failed ";
                            Error.Visible = true;
                        }
                    }

                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "About US", "Failed", "", "Sorry you are not maker");

                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                    Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                divdetails.Visible = false;
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
            grdPhoto.PageIndex = e.NewPageIndex;
            BindAboutUs(Guid.Parse(ddlEntity.SelectedValue));
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
                    if (e.CommandName.ToLower().Equals("view"))
                    {
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        string fileName = Convert.ToString(e.CommandArgument);
                        string FilePath = Server.MapPath(savePath + fileName);
                        byte[] decryptedBytes = null;
                        byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                        GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        Label lblEncrptionkey = (Label)row.Cells[0].FindControl("lblEncrptionkey");

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (RijndaelManaged rijndael = new RijndaelManaged())
                            {
                                byte[] ba = Encoding.Default.GetBytes(lblEncrptionkey.Text);
                                var hexString = BitConverter.ToString(ba);
                                hexString = hexString.Replace("-", "");

                                byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                                var hexStrings = BitConverter.ToString(bsa);
                                hexStrings = hexStrings.Replace("-", "");

                                rijndael.Mode = CipherMode.CBC;
                                rijndael.Padding = PaddingMode.PKCS7;
                                rijndael.KeySize = 256;
                                rijndael.BlockSize = 128;
                                rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                                rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                                using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                                {
                                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                    cs.Close();
                                }
                                decryptedBytes = ms.ToArray();
                            }
                            //Set the appropriate ContentType.
                            Response.ContentType = "Application/pdf";
                            //Get the physical path to the file.
                            // string FilePath = Server.MapPath(savePath + fileName);

                            Response.AddHeader("Content-Disposition", "attachment; filename=" + "AboutUs.pdf");
                            // Response.WriteFile(FilePath);
                            Response.BinaryWrite(ms.ToArray());
                            Response.Flush();
                        }

                        //string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        //string fileName = Convert.ToString(e.CommandArgument);
                        ////Set the appropriate ContentType.
                        //Response.ContentType = "Application/pdf";
                        ////Get the physical path to the file.
                        //string FilePath = Server.MapPath(savePath + fileName);
                        ////Write the file directly to the HTTP content output stream.
                        //Response.WriteFile(FilePath);
                        //Response.End();
                    }

                    if (e.CommandName.ToLower().Equals("edit"))
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }

                        Int32 AboutUsId = Int32.Parse(Convert.ToString(e.CommandArgument));
                        ABoutUSDomain objAbout = AboutUsDataProvider.Instance.Get(AboutUsId);
                        txtDescription.Text = objAbout.Text;

                        btnInsert.Text = "Update";
                        hdnPhotoId.Value = objAbout.Id.ToString();
                        ViewState["file"] = objAbout.Image;
                        ViewState["EncryptionKey"] = objAbout.EncryptionKey;
                        rfvPhoto.Enabled = false;
                        divdetails.Visible = true;
                        lnkView.Visible = true;
                    }

                    if (e.CommandName.ToLower().Equals("delete"))
                    {

                        if (!objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "About US", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        Int32 AboutUsId = Int32.Parse(Convert.ToString(e.CommandArgument));
                        ABoutUSDomain objABoutUSDomain = AboutUsDataProvider.Instance.Get(AboutUsId);
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PhotoGallery"]);
                        File.Delete(Server.MapPath(savePath + objABoutUSDomain.Image));

                        bool status = AboutUsDataProvider.Instance.Delete(AboutUsId);
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "About us deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "About US", "Success", Convert.ToString(AboutUsId), "About us deleted successfully");

                            BindAboutUs(Guid.Parse(ddlEntity.SelectedValue));
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "About US", "Failed", Convert.ToString(AboutUsId), "About us deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "About us deletion failed";
                            Error.Visible = true;
                        }
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Session["UserId"]), "About US", "Failed", "", "Sorry you are not maker");

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
                DataTable dt = (DataTable)AboutUsDataProvider.Instance.GetABoutUSDomainByEntity(Guid.Parse(ddlEntity.SelectedValue)).AsDataTable();
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
                string FilePath = Server.MapPath(savePath + fileName);

                byte[] decryptedBytes = null;
                byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged rijndael = new RijndaelManaged())
                    {
                        byte[] ba = Encoding.Default.GetBytes(ViewState["EncryptionKey"].ToString());
                        var hexString = BitConverter.ToString(ba);
                        hexString = hexString.Replace("-", "");

                        byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                        var hexStrings = BitConverter.ToString(bsa);
                        hexStrings = hexStrings.Replace("-", "");

                        rijndael.Mode = CipherMode.CBC;
                        rijndael.Padding = PaddingMode.PKCS7;
                        rijndael.KeySize = 256;
                        rijndael.BlockSize = 128;
                        rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                        rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                        using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                    //Set the appropriate ContentType.
                    Response.ContentType = "Application/pdf";
                    //Get the physical path to the file.
                    // string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "AboutUs.pdf");
                    // Response.WriteFile(FilePath);
                    Response.BinaryWrite(ms.ToArray());
                    Response.Flush();
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