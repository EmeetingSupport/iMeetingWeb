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
    public partial class EntityMaster : System.Web.UI.Page
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
            //  Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //bind entity list
                BindEntity();

                //Bind User drop down
                BindUsers();

                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (IsSuperAdmin)
                {
                    divdetails.Visible = true;
                }
                else
                {
                    divdetails.Visible = false;
                }
            }
        }

        /// <summary>
        /// Bind User List to drop down
        /// </summary>
        private void BindUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker().OrderBy(p => p.FirstName).ToList();
                if (objUser.Count > 0)
                {
                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }

                //Remove logged in users id from drop down
                string UserId = Convert.ToString(Session["UserId"]);
                ListItem UserIdToRemove = ddlUser.Items.FindByValue(UserId);
                ddlUser.Items.Remove(UserIdToRemove);

                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
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
        /// Bind Entity List
        /// </summary>
        private void BindEntity()
        {
            try
            {
                Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                ltl_bredcrumbs.Text = "";
                ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home</a>&nbsp;";
                string UserId = Convert.ToString(Session["UserId"].ToString());
                grdEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); //EntityDataProvider.Instance.Get();
                grdEntity.DataBind();
                //DataColumn SuperAdmin = new DataColumn();
                //SuperAdmin.ColumnName = "SuperAdmin";
                //SuperAdmin.DataType = System.Type.GetType("System.Boolean");
                //SuperAdmin.Expression = Session["IsSuperAdmin"].ToString();

                //IList<UserEntityDomain> objUserEntityDomain = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); //EntityDataProvider.Instance.Get();

                //System.Data.DataTable dt = objUserEntityDomain.AsDataTable();
                //dt.Columns.Add(SuperAdmin);

                //grdEntity.DataSource = dt;
                //grdEntity.DataBind();
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
        /// Insert or edit values 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();

                if (txtEntityName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtEntityName);

                    if (!objUser.isValidChar(txtEntityName.Text))
                    {
                        txtEntityName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtEntityName.Text))
                    {
                        txtEntityName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid department name.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter department name.";
                    Error.Visible = true;
                    return;
                }


                if (txtShortName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtShortName);
                    if (!objUser.isValidChar(txtShortName.Text))
                    {
                        txtShortName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtShortName.Text))
                    {
                        txtShortName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid short name.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter short name.";
                    Error.Visible = true;
                    return;
                }

                if (hdnEntityId.Value == null || hdnEntityId.Value == "")
                {
                    //Commented on 08-03-2017 hide logo field so need to commented code related to logo.
                    //if (!fuEntityLogo.HasFile)
                    //{
                    //    ((Label)Error.FindControl("lblError")).Text = " Please upload logo.";
                    //    Error.Visible = true;
                    //    return;
                    //}
                }

                if (txtEntityName.Text.Length > 0 && txtShortName.Text.Length > 0)
                {
                    EntityDomain objEntity = new EntityDomain();
                    objEntity.EntityName = txtEntityName.Text;

                    Guid CheckerId;
                    if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                    {
                        objEntity.EntityChecker = CheckerId;
                    }

                    objEntity.EntityShortName = txtShortName.Text;
                    objEntity.IsEnable = chkEnable.Checked;

                    //insert default image name
                    string logoName = "sample_logo.gif";
                    string logoNameMeeting = "";

                    objEntity.UpdatedBy = Guid.Parse(Session["UserId"].ToString());

                    objEntity.CreatedBy = Guid.Parse(Session["UserId"].ToString());

                    string SendToChecker;
                    if (chkChecker.Checked)
                    {
                        SendToChecker = "Pending";
                        objEntity.IsEnable = false;
                    }
                    else
                    {
                        SendToChecker = "Approved";
                        objEntity.IsEnable = true;
                    }
                    if (objEntity.CreatedBy == objEntity.EntityChecker)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                        Error.Visible = true;
                        return;
                    }

                    string EncryptionKey = Session["EncryptionKey"].ToString();
                    if (hdnEntityId.Value != "")
                    {
                        EncryptionKey = ViewState["EncryptionKey"].ToString();
                    }
                    objEntity.EncryptionKey = EncryptionKey;

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {

                        //update Entity
                        if (hdnEntityId.Value != null && hdnEntityId.Value != "")
                        {
                            //Guid CreatedBy = Guid.Parse(ViewState["CreatedBy"].ToString());

                            //if (CreatedBy == objEntity.EntityChecker)
                            //{
                            //    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                            //    Error.Visible = true;
                            //    return;
                            //}

                            if (fuEntityLogo.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                                string contentType = fuEntityLogo.PostedFile.ContentType;
                                string extn = Path.GetExtension(fuEntityLogo.FileName);
                                if (!objUser.isValidChar_old(fuEntityLogo.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                if (contentType.ToLower().Contains("image/"))
                                {
                                    if (extn.ToLower().Equals(".jpg") || extn.ToLower().Equals(".jpeg") || extn.ToLower().Equals(".png") || extn.ToLower().Equals(".bmp"))
                                    {

                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuEntityLogo.PostedFile.InputStream);
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
                                            string ext = Path.GetExtension(fuEntityLogo.FileName);

                                            logoName = DateTime.Now.Ticks + ext;
                                            fuEntityLogo.PostedFile.SaveAs(Server.MapPath(savePath + logoName));

                                            if (ViewState["file"] != null)
                                            {
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
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid image uploaded file";
                                    Error.Visible = true;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    logoName = Convert.ToString(ViewState["file"]);
                                }
                            }

                            if (fuMeeting.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                                string contentType = fuEntityLogo.PostedFile.ContentType;
                                string ext = Path.GetExtension(fuMeeting.FileName);
                                string contentTypeMeet = fuMeeting.PostedFile.ContentType;

                                if (!objUser.isValidChar_old(fuMeeting.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                if (contentTypeMeet.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {

                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuMeeting.PostedFile.InputStream);
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

                                        logoNameMeeting = DateTime.Now.Ticks + ext;
                                        fuMeeting.PostedFile.SaveAs(Server.MapPath(savePath + logoNameMeeting));

                                        //string EncryptionKey = Session["EncryptionKey"].ToString();
                                        //if(hdnEntityId.Value != "")
                                        //{
                                        //    EncryptionKey = ViewState["EncryptionKey"].ToString();
                                        //}
                                        EncryptionHelper.EncryptString(Server.MapPath(savePath + logoNameMeeting), EncryptionKey);
                                        //objEntity.EncryptionKey = EncryptionKey;
                                        if (ViewState["fileMeeting"] != null)
                                        {
                                            string fileName = Convert.ToString(ViewState["fileMeeting"]);

                                            File.Delete(Server.MapPath(savePath + fileName));
                                            ViewState["fileMeeting"] = null;
                                        }
                                        r.Close();
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["fileMeeting"] != null)
                                {
                                    logoNameMeeting = Convert.ToString(ViewState["fileMeeting"]);
                                }
                            }
                            objEntity.EntityLogo = logoName;
                            objEntity.EntityMeeting = logoNameMeeting;

                            objEntity.EntityId = Guid.Parse(hdnEntityId.Value);
                            bool status = EntityDataProvider.Instance.Update(objEntity, SendToChecker);
                            if (status)
                            {

                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objEntity);
                                var encrypt = Encryptor.EncryptString(json);

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Success", Convert.ToString(objEntity.EntityId), "Department updated successfully :- " + encrypt + "");

                                ((Label)Info.FindControl("lblName")).Text = " Department updated successfully";
                                Info.Visible = true;
                                ClearData();
                                BindEntity();
                                rfvEntityLogo.Enabled = true;
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Failed", Convert.ToString(objEntity.EntityId), "Department updation failed");

                                ((Label)Error.FindControl("lblError")).Text = "Department updation failed";
                                Error.Visible = true;
                            }
                        }
                        //Insert Entity
                        else
                        {

                            if (fuEntityLogo.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                                string contentType = fuEntityLogo.PostedFile.ContentType;
                                string extn = Path.GetExtension(fuEntityLogo.FileName);
                                if (!objUser.isValidChar_old(fuEntityLogo.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                if (contentType.ToLower().Contains("image/"))
                                {
                                    if (extn.ToLower().Equals(".jpg") || extn.ToLower().Equals(".jpeg") || extn.ToLower().Equals(".png") || extn.ToLower().Equals(".bmp"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuEntityLogo.PostedFile.InputStream);
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
                                            string ext = Path.GetExtension(fuEntityLogo.FileName);

                                            logoName = DateTime.Now.Ticks + ext;
                                            fuEntityLogo.PostedFile.SaveAs(Server.MapPath(savePath + logoName));

                                            EncryptionHelper.EncryptString(Server.MapPath(savePath + logoNameMeeting), EncryptionKey);

                                            if (ViewState["file"] != null)
                                            {
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
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid image uploaded file";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    logoName = Convert.ToString(ViewState["file"]);
                                }
                            }

                            if (fuMeeting.HasFile)
                            {
                                if (!objUser.isValidChar_old(fuMeeting.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                                string contentType = fuEntityLogo.PostedFile.ContentType;
                                string ext = Path.GetExtension(fuMeeting.FileName);
                                string contentTypeMeet = fuMeeting.PostedFile.ContentType;


                                if (contentTypeMeet.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuMeeting.PostedFile.InputStream);
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

                                        logoNameMeeting = DateTime.Now.Ticks + ext;
                                        fuMeeting.PostedFile.SaveAs(Server.MapPath(savePath + logoNameMeeting));

                                        if (ViewState["fileMeeting"] != null)
                                        {
                                            string fileName = Convert.ToString(ViewState["fileMeeting"]);
                                            if (fileName != "sample_avatar.jpg")
                                                File.Delete(Server.MapPath(savePath + fileName));
                                            ViewState["fileMeeting"] = null;
                                        }
                                        r.Close();
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["fileMeeting"] != null)
                                {
                                    logoNameMeeting = Convert.ToString(ViewState["fileMeeting"]);
                                }
                            }
                            objEntity.EntityLogo = logoName;
                            objEntity.EntityMeeting = logoNameMeeting;

                            objEntity = EntityDataProvider.Instance.Insert(objEntity, SendToChecker);
                            if (objEntity.EntityId.ToString().Equals("11000000-0000-0000-0000-000000000011"))
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Failed", Convert.ToString(objEntity.EntityId), "Sorry Maximum limit reached");

                                ((Label)Error.FindControl("lblError")).Text = " Sorry Maximum limit reached";
                                Error.Visible = true;
                            }
                            else if (!objEntity.EntityId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                            {
                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objEntity);
                                var encrypt = Encryptor.EncryptString(json);

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Success", Convert.ToString(objEntity.EntityId), "Department inserted successfully :- " + encrypt + "");

                                ((Label)Info.FindControl("lblName")).Text = " Department inserted successfully";
                                Info.Visible = true;
                                ClearData();
                                BindEntity();
                                rfvEntityLogo.Enabled = true;
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Failed", Convert.ToString(objEntity.EntityId), "Department insertion failed");

                                ((Label)Error.FindControl("lblError")).Text = " Department insertion failed";
                                Error.Visible = true;
                            }

                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objEntity.CreatedBy), "Department Master", "Failed", Convert.ToString(objEntity.EntityId), "Sorry you are not maker");

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

                divdetails.Visible = false;
            }
        }

        /// <summary>
        /// Insertion or Updation cancel
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Clear form data
        /// </summary>
        private void ClearData()
        {
            try
            {
                txtEntityName.Text = "";
                hdnEntityId.Value = "";
                txtShortName.Text = "";
                chkEnable.Checked = false;
                ddlUser.SelectedValue = "0";
                btnInsert.Text = "Save";
                hdnLogo.Value = "";
                lnkView.Visible = false;
                chkChecker.Checked = false;
                ViewState["file"] = null;
                rfvEntityLogo.Enabled = true;
                bool IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"]);

                if (IsSuperAdmin)
                {
                    divdetails.Visible = true;
                }
                else
                {
                    divdetails.Visible = false;
                }

                //divdetails.Visible = false;
                ViewState["EncryptionKey"] = null;
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
        /// Delelte Seleted Entity
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lbRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                ////StringBuilder strQuery = new StringBuilder("");
                //StringBuilder strEntityIds = new StringBuilder(",");
                //int count = 0;
                ////get all checked entity
                //for (int i = 0; i < grdEntity.Rows.Count; i++)
                //{



                //    CheckBox chkSelect = (CheckBox)grdEntity.Rows[i].FindControl("chkSubAdmin");
                //    if (chkSelect.Checked)
                //    {
                //        count++;
                //        string EntityID = Convert.ToString(grdEntity.DataKeys[i].Value.ToString());
                //        //Delete all selected entity
                //        // strQuery.Append("	DELETE FROM [Entity]   WHERE  EntityId = '" + EntityID + "'  ");
                //        strEntityIds.Append(EntityID + ",");

                //    }
                //}
                //if (count > 0)
                //{
                //    bool bStatus = EntityDataProvider.Instance.DeleteSelectedEntity(strEntityIds.ToString());
                //    if (!bStatus)
                //    {
                //        ((Label)Info.FindControl("lblName")).Text = "Entity Deleted Successfully";
                //        Info.Visible = true;
                //        BindEntity();
                //    }
                //    else
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //        Error.Visible = true;
                //    }
                //}
                //else
                //{
                //    ((Label)Error.FindControl("lblError")).Text = "Please Select at least one checkbox";
                //    Error.Visible = true;
                //}

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
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // delete entity
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    string EntityId = Convert.ToString(e.CommandArgument.ToString());
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {

                        UserAcess objUser = new UserAcess();

                        //check edit permission
                        if (objUser.IsEdit(Guid.Parse(EntityId)))
                        {
                            bool bStatus = EntityDataProvider.Instance.Delete(Guid.Parse(EntityId));
                            if (bStatus)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Department deleted successfully";
                                Info.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Department Master", "Success", EntityId, "Department deleted successfully");

                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Department Master", "Failed", EntityId, "Department deletion failed");

                                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                Error.Visible = true;
                            }
                            BindEntity();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Department Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Department Master", "Failed", "", "Sorry you are not maker");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
                }


                // view entity
                if (e.CommandName.ToLower().Equals("view"))
                {
                    string EntityId = Convert.ToString(e.CommandArgument.ToString());
                    //    Session["EntityId"] = EntityId;
                    Session["EntityId_Assigneed"] = EntityId;
                    Response.Redirect("ForumMaster.aspx");//?id=" + EntityId);
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
        /// Row deleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Code to Row  editing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdEntity_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                string EntityId = grdEntity.DataKeys[e.NewEditIndex].Value.ToString();
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    //check edit permission
                    if (objUser.IsEdit(Guid.Parse(EntityId)))
                    {
                        EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(EntityId));
                        txtEntityName.Text = objEntity.EntityName;
                        txtShortName.Text = objEntity.EntityShortName;
                        chkEnable.Checked = objEntity.IsEnable;
                        //  ddlUser.SelectedValue = Convert.ToString(objEntity.EntityChecker);
                        if (ddlUser.Items.FindByValue(
        Convert.ToString(objEntity.EntityChecker)) != null)
                        {
                            ddlUser.SelectedValue = objEntity.EntityChecker.ToString();
                        }
                        else
                        {
                            ddlUser.SelectedValue = "0";
                        }
                        hdnEntityId.Value = Convert.ToString(objEntity.EntityId);
                        hdnLogo.Value = objEntity.EntityLogo;
                        ViewState["CreatedBy"] = objEntity.CreatedBy;

                        if (objEntity.EntityLogo != null)
                        {
                            if (objEntity.EntityLogo.Length > 0)
                            {
                                ViewState["file"] = objEntity.EntityLogo;
                                lnkView.Visible = true;
                            }
                        }

                        if (objEntity.EntityMeeting != null)
                        {
                            if (objEntity.EntityMeeting.Length > 0)
                            {
                                ViewState["fileMeeting"] = objEntity.EntityMeeting;
                                lnkViewMeeting.Visible = true;
                                lnkRemoveFile.Visible = true;
                            }
                            else
                            {
                                lnkViewMeeting.Visible = false;
                                lnkRemoveFile.Visible = false;
                            }
                        }
                        rfvEntityLogo.Enabled = false;
                        ViewState["EncryptionKey"] = objEntity.EncryptionKey;
                        btnInsert.Text = "Update";
                        divdetails.Visible = true;
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
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
                //  divdetails.Visible = false;
            }
        }
        /// <summary>
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdEntity_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"].ToString());

                DataTable dt = (DataTable)UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).AsDataTable();
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

                grdEntity.DataSource = dv;
                grdEntity.DataBind();
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
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdEntity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClearData();
                grdEntity.PageIndex = e.NewPageIndex;
                BindEntity();
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
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["file"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);

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
        /// View meeting pdf
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkViewMeeting_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["fileMeeting"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);

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

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + "Minutes.pdf");
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

        /// <summary>
        /// Delete Uploaded pdf file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">Eventargs specifying e</param>
        protected void lnkRemoveFile_Click(object sender, EventArgs e)
        {
            try
            {

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    UserAcess objUser = new UserAcess();
                    // check add permission
                    if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        EntityDomain objEntity = EntityDataProvider.Instance.Get(Guid.Parse(hdnEntityId.Value));
                        objEntity.EntityMeeting = "";

                        try
                        {
                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);

                            string fileName = Convert.ToString(ViewState["fileMeeting"]);
                            //if (fileName != "sample_logo.gif")
                            File.Delete(Server.MapPath(savePath + fileName));

                            ViewState["fileMeeting"] = null;
                        }
                        catch (Exception ex)
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;

                            LogError objEr = new LogError();
                            objEr.HandleException(ex);
                        }

                        bool status = EntityDataProvider.Instance.Update(objEntity, "Approved");
                        if (status)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " File deleted successfully";
                            Info.Visible = true;
                            ClearData();
                            BindEntity();
                            rfvEntityLogo.Enabled = true;
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Department updation failed";
                            Error.Visible = true;
                        }

                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                        Info.Visible = false;
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

                // divdetails.Visible = false;
            }
        }
    }
}