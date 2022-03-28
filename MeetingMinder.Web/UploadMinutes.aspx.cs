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
using System.Security.Cryptography;
using System.Text;

namespace MeetingMinder.Web
{
    public partial class UploadMinutes : System.Web.UI.Page
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

                if (Session["MessageUM"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["MessageUM"].ToString();
                    Info.Visible = true;
                    Session["MessageUM"] = null;
                }
            }

            if (!IsPostBack)
            {
                //bind meeting list
                // BindCheckers();

                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                //Bind Mintes to grid
                BindUploadMinutes();

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:table-row;");
                    divCheckerEntity.Attributes.Add("style", "display:table-row;");
                    rfvddUser.Enabled = true;
                    chkChecker.Checked = true;
                    chkChecker.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Bind uplod minutes to grid
        /// </summary>
        private void BindUploadMinutes()
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                grdUploadMinutes.DataSource = UploadMintesDataProvider.Instance.GetUploadMinutesByUserAcess(Guid.Parse(UserId)).OrderByDescending(p => DateTime.Parse(p.MeetingDate)).ToList();

                //grdUploadMinutes.DataSource = UploadMintesDataProvider.Instance.GetUploadMinutesByUser(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                grdUploadMinutes.DataBind();
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
        /// Bind Checkers List to drop down
        /// </summary>
        private void BindCheckers(Guid ForumId)
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker(ForumId).OrderBy(p => p.FirstName).ToList(); ;
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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strForumId = ddlForum.SelectedValue;
            if (strForumId != "0")
            {
                BindMeeting(strForumId);
                BindCheckers(Guid.Parse(strForumId));
            }
        }

        /// <summary>
        /// BInd Meeting list to drop down
        /// </summary>
        /// <param name="ForumId">string specifying ForumId</param>
        private void BindMeeting(string ForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                Guid forumId;
                if (Guid.TryParse(ForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByForUploadMinutes(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting <= dtToday)
                        //{
                        //   ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("dd MMM yyyy"), item.MeetingId.ToString()));
                        ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                        //}
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
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
        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strEntityId = ddlEntity.SelectedValue;
            if (strEntityId != "0")
            {
                BindForum(strEntityId);
            }
        }

        /// <summary>
        /// Bind forums to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                Guid entityId;
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();
                    //IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);                    
                    ddlForum.DataSource = objForum;
                    ddlForum.DataBind();
                    ddlForum.DataTextField = "ForumName";
                    ddlForum.DataValueField = "ForumId";
                    ddlForum.DataBind();
                    ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
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
        /// button submit click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UploadMinutesDomain objUploadMinutes = new UploadMinutesDomain();
                UserAcess objUser = new UserAcess();
                if (hdnUploadId.Value == "")
                {
                    if (!fuMinutes.HasFile)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Please upload pdf file.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (ddlForum.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select forum.";
                    Error.Visible = true;
                    return;
                }

                if (ddlMeeting.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select meeting.";
                    Error.Visible = true;
                    return;
                }

                objUploadMinutes.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objUploadMinutes.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                Guid CheckerId;
                if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                {
                    objUploadMinutes.UplaodMinuteChecker = CheckerId;
                }
                objUploadMinutes.UplaodMinuteChecker = CheckerId;
                string fileName = "";

                if (objUploadMinutes.CreatedBy == objUploadMinutes.UplaodMinuteChecker)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                    Error.Visible = true;
                    return;
                }


                if (chkChecker.Checked)
                {
                    objUploadMinutes.IsApproved = "Pending";
                }
                else
                {
                    objUploadMinutes.IsApproved = "Approved";
                }

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {

                    if (hdnUploadId.Value != "")
                    {
                        string EncryptionKey = ViewState["EncryptionKey"].ToString();
                        objUploadMinutes.EncryptionKey = EncryptionKey;
                        if (objUser.IsEdit(Guid.Parse(ddlEntity.SelectedValue)))
                        {

                            if (fuMinutes.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                                string contentType = fuMinutes.PostedFile.ContentType;

                                string ext = Path.GetExtension(fuMinutes.FileName);

                                if (!objUser.isValidChar_old(fuMinutes.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                if (contentType.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuMinutes.PostedFile.InputStream);
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

                                        fileName = DateTime.Now.Ticks + ext;

                                        fuMinutes.PostedFile.SaveAs(Server.MapPath(savePath + fileName));





                                        EncryptionHelper.EncryptString(Server.MapPath(savePath + fileName), EncryptionKey);
                                        objUploadMinutes.UploadFile = fileName;
                                        r.Close();
                                        if (ViewState["file"] != null)
                                        {
                                            string file = Convert.ToString(ViewState["file"]);
                                            File.Delete(Server.MapPath(savePath + file));
                                            ViewState["file"] = null;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Upload minutes file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Upload minutes file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    objUploadMinutes.UploadFile = Convert.ToString(ViewState["file"]);
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Please upload pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            objUploadMinutes.MeetingId = Guid.Parse(Convert.ToString(ViewState["Meeting"]));
                            objUploadMinutes.UploadMinuteId = Guid.Parse(hdnUploadId.Value);

                            bool status = UploadMintesDataProvider.Instance.Update(objUploadMinutes);
                            if (status)
                            {
                                if (objUploadMinutes.IsApproved == "Pending")
                                {
                                    if (chkNotifyChecker.Checked)
                                    {
                                        bool isSuccess;
                                        string Message;
                                        SendEmail objSendEmail = new SendEmail();
                                        objSendEmail.SendNotifyEmail("Minutes", objUploadMinutes.UplaodMinuteChecker, out isSuccess, out Message);
                                        if (isSuccess)
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = "Meeting minutes updated successfully and sent for approval with email notification ";
                                            Info.Visible = true;
                                        }
                                        else
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = "Meeting minutes updated successfully and sent for approval. ";
                                            ((Label)Error.FindControl("lblError")).Text = Message;
                                            Info.Visible = true;
                                            Error.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = "Meeting minutes updated successfully and sent for approval";
                                        Info.Visible = true;
                                    }
                                    //  ((Label)Info.FindControl("lblName")).Text = "Meeting minutes updated successfully and sent for approval";

                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUploadMinutes);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Pending", Convert.ToString(objUploadMinutes.UploadMinuteId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                }
                                else
                                {
                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUploadMinutes);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Success", Convert.ToString(objUploadMinutes.UploadMinuteId), "Meeting minutes updated successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = "Meeting minutes updated successfully";
                                }
                                Info.Visible = true;
                                ClearData();

                                Response.StatusCode = 302;
                                Response.AddHeader("Location", "UploadMinutes.aspx");
                                Session["MessageUM"] = ((Label)Info.FindControl("lblName")).Text;
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Failed", Convert.ToString(objUploadMinutes.UploadMinuteId), "Meeting minutes updation failed");

                                ((Label)Error.FindControl("lblError")).Text = "Meeting minutes updation failed";
                                Error.Visible = true;
                            }
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Failed", Convert.ToString(objUploadMinutes.UploadMinuteId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {

                        //check edit permission
                        if (objUser.IsAdd(Guid.Parse(ddlEntity.SelectedValue)))
                        {
                            if (fuMinutes.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                                string contentType = fuMinutes.PostedFile.ContentType;

                                if (!objUser.isValidChar_old(fuMinutes.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                string ext = Path.GetExtension(fuMinutes.FileName);
                                if (contentType.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuMinutes.PostedFile.InputStream);
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

                                        fileName = DateTime.Now.Ticks + ext;
                                        string EncryptionKey = Session["EncryptionKey"].ToString();
                                        fuMinutes.PostedFile.SaveAs(Server.MapPath(savePath + fileName));
                                        EncryptionHelper.EncryptString(Server.MapPath(savePath + fileName), EncryptionKey);
                                        objUploadMinutes.UploadFile = fileName;
                                        objUploadMinutes.EncryptionKey = EncryptionKey;
                                        r.Close();

                                        if (ViewState["file"] != null)
                                        {
                                            string file = Convert.ToString(ViewState["file"]);
                                            File.Delete(Server.MapPath(savePath + file));
                                            ViewState["file"] = null;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Upload minutes file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Upload minutes file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    objUploadMinutes.UploadFile = Convert.ToString(ViewState["file"]);
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Please upload pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            objUploadMinutes.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);

                            objUploadMinutes = UploadMintesDataProvider.Instance.Insert(objUploadMinutes);
                            if (!objUploadMinutes.UploadMinuteId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                            {
                                if (objUploadMinutes.IsApproved == "Pending")
                                {
                                    if (chkNotifyChecker.Checked)
                                    {
                                        bool isSuccess;
                                        string Message;
                                        SendEmail objSendEmail = new SendEmail();
                                        objSendEmail.SendNotifyEmail("Minutes", objUploadMinutes.UplaodMinuteChecker, out isSuccess, out Message);
                                        if (isSuccess)
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = "Meeting minutes inserted successfully and sent for approval email notification";
                                            Info.Visible = true;
                                        }
                                        else
                                        {
                                            ((Label)Error.FindControl("lblError")).Text = Message;
                                            ((Label)Info.FindControl("lblName")).Text = "Meeting minutes inserted successfully and sent for approval. ";
                                            Info.Visible = true;
                                            Error.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = "Meeting minutes inserted successfully and sent for approval";
                                        Info.Visible = true;
                                    }

                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUploadMinutes);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Pending", Convert.ToString(objUploadMinutes.UploadMinuteId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                    //((Label)Info.FindControl("lblName")).Text = "Meeting minutes inserted successfully and sent for approval";
                                }
                                else
                                {
                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objUploadMinutes);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Success", Convert.ToString(objUploadMinutes.UploadMinuteId), "Meeting minutes inserted successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = "Meeting minutes inserted successfully";
                                }
                                Info.Visible = true;
                                ClearData();
                                Response.StatusCode = 302;
                                Response.AddHeader("Location", "UploadMinutes.aspx");
                                Session["MessageUM"] = ((Label)Info.FindControl("lblName")).Text;
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Failed", Convert.ToString(objUploadMinutes.UploadMinuteId), "Meeting insertion failed");

                                ((Label)Error.FindControl("lblError")).Text = " Meeting insertion failed ";
                                Error.Visible = true;
                                Info.Visible = false;
                            }
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Failed", Convert.ToString(objUploadMinutes.UploadMinuteId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            Info.Visible = false;
                        }
                    }
                    BindUploadMinutes();
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objUploadMinutes.CreatedBy), "Upload Minutes", "Failed", Convert.ToString(objUploadMinutes.UploadMinuteId), "Sorry you are not maker");

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
        /// Clear Form data
        /// </summary>
        private void ClearData()
        {
            ddlUser.SelectedValue = "0";
            ddlMeeting.Items.Clear();
            ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
            ddlForum.Items.Clear();
            ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
            //   ddlEntity.SelectedValue = "0";
            chkChecker.Checked = false;
            btnSubmit.Text = "Submit";
            ViewState["file"] = null;
            lnkView.Visible = false;
            hdnUploadId.Value = "";
            rfvMinutes.Enabled = true;
            ddlForum.Enabled = true;
            ddlMeeting.Enabled = true;
            ViewState["Meeting"] = null;
            ViewState["EncryptionKey"] = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            BindForum(EntityId.ToString());

            bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
            bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

            if (IsMaker && !IsChecker)
            {
                divEntity.Attributes.Add("style", "display:table-row;");
                divCheckerEntity.Attributes.Add("style", "display:table-row;");
                rfvddUser.Enabled = true;
                chkChecker.Checked = true;
                chkChecker.Enabled = false;
            }

        }

        /// <summary>
        /// Button cancel event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// gridview pageindexchange event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUploadMinutes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdUploadMinutes.PageIndex = e.NewPageIndex;
            BindUploadMinutes();
        }

        /// <summary>
        /// Gridview rowcommand event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUploadMinutesRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("view"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblEncrptionkey = (Label)row.Cells[0].FindControl("lblEncrptionkey");


                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                    string fileName = Convert.ToString(e.CommandArgument);

                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    byte[] decryptedBytes = null;
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
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

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Minutes.pdf");
                        // Response.WriteFile(FilePath);
                        Response.BinaryWrite(ms.ToArray());
                        Response.Flush();
                    }


                    //Write the file directly to the HTTP content output stream.
                    //Response.WriteFile(FilePath);
                    //Response.End();
                }

                if (e.CommandName.ToLower().Equals("edit"))
                {
                    string strUploadMinId = Convert.ToString(e.CommandArgument);
                    string[] ids = strUploadMinId.Split(',');

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblMeeting = (Label)row.Cells[0].FindControl("lblTe");

                    if (objUser.IsEdit(Guid.Parse(ids[3])))
                    {

                        UploadMinutesDomain objUpload = UploadMintesDataProvider.Instance.Get(Guid.Parse(ids[0]));
                        ddlEntity.SelectedValue = ids[3];
                        //                      ddlEntity_SelectedIndexChanged(sender, e);
                        //                    ddlForum.SelectedValue = ids[2];
                        ViewState["Meeting"] = ids[4];
                        //ddlForum_SelectedIndexChanged(sender, e);
                        //ddlMeeting.SelectedValue = ids[1];
                        ddlMeeting.Items.Clear();
                        ddlForum.Items.Clear();
                        ddlForum.Items.Add(ids[2]);
                        ddlMeeting.Items.Add(lblMeeting.Text);
                        ddlForum.Enabled = false;
                        ddlMeeting.Enabled = false;
                        //ddlUser.SelectedValue = objUpload.UplaodMinuteChecker.ToString();
                        BindCheckers(Guid.Parse(ids[5]));
                        if (ddlUser.Items.FindByValue(
Convert.ToString(objUpload.UplaodMinuteChecker)) != null)
                        {
                            ddlUser.SelectedValue = objUpload.UplaodMinuteChecker.ToString();
                        }
                        else
                        {
                            ddlUser.SelectedValue = "0";
                        }

                        ViewState["EncryptionKey"] = objUpload.EncryptionKey;
                        ViewState["file"] = objUpload.UploadFile;
                        lnkView.Visible = true;
                        hdnUploadId.Value = objUpload.UploadMinuteId.ToString();
                        rfvMinutes.Enabled = false;
                        btnSubmit.Text = "Update";
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        string strUploadMinId = Convert.ToString(e.CommandArgument);
                        string[] ids = strUploadMinId.Split(',');
                        if (objUser.isDelete(Guid.Parse(ids[1])))
                        {
                            bool status = UploadMintesDataProvider.Instance.Delete(Guid.Parse(ids[0]));
                            if (status)
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Upload Minutes", "Success", Convert.ToString(Guid.Parse(ids[0])), "Meeting minutes deleted successfully");

                                ((Label)Info.FindControl("lblName")).Text = "Meeting minutes deleted successfully";
                                Info.Visible = true;
                                BindUploadMinutes();
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Upload Minutes", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Meeting minutes deletion failed");

                                ((Label)Error.FindControl("lblError")).Text = "Meeting minutes deletion failed";
                                Error.Visible = true;
                            }

                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Upload Minutes", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Upload Minutes", "Failed", "", "Sorry you are not maker");

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
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUploadMinutes_Sorting(object sender, GridViewSortEventArgs e)
        {
            string UserId = Convert.ToString(Session["UserId"]);
            //  DataTable dt = (DataTable)UploadMintesDataProvider.Instance.GetUploadMinutesByUser(Guid.Parse(UserId)).AsDataTable();

            DataTable dt = (DataTable)UploadMintesDataProvider.Instance.GetUploadMinutesByUserAcess(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList().AsDataTable();
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

            grdUploadMinutes.DataSource = dv;
            grdUploadMinutes.DataBind();
        }

        /// <summary>
        /// Gridview RowEditing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUploadMinutes_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview RowDeleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUploadMinutes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

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

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
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
    }
}