using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;
using MM.Domain;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class Proceding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                string EntityId = Session["EntityId"].ToString();

                //ddlAgenda.Items.Insert(0, new ListItem("Select Agenda", "0"));
                //ddlSubAgenda.Items.Insert(0, new ListItem("Select Sub Agenda", "0"));
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                BindForum(EntityId);
                BindProceding1(1);

            }
            if (Session["MessageProceeding"] != null)
            {
                ((Label)Info.FindControl("lblName")).Text = Session["MessageProceeding"].ToString();
                Info.Visible = true;
                //BindProceding(sender, e);

                Session["MessageProceeding"] = null;
            }

        }

        protected void grdProceding_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdProceding_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            byte[] ba = Encoding.Default.GetBytes(MM.Core.Encryptor.DecryptString(lblEncrptionkey.Text));
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

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Proceding.pdf");
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
                    string strProcedingId = Convert.ToString(e.CommandArgument);
                    //string[] ids = strUploadMinId.Split(',');

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblMeeting = (Label)row.Cells[0].FindControl("lblTe");
                    string EntityId = Session["EntityId"].ToString();

                    if (objUser.IsEdit(Guid.Parse(EntityId)))
                    {

                        ProcedingDomain objProceding = ProcedingDataProvider.Instance.Get(Guid.Parse(strProcedingId));

                        //ddlMeeting.Items.Clear();
                        //ddlForum.Items.Clear();
                        //ddlForum.Items.Add(ids[2]);
                        //ddlMeeting.Items.Add(lblMeeting.Text);
                        //ddlForum.Enabled = false;
                        //ddlMeeting.Enabled = false;
                        //ddlUser.SelectedValue = objUpload.UplaodMinuteChecker.ToString();

                        ddlForum.Enabled = false;
                        ddlMeeting.Enabled = false;
                        ddlForum.SelectedValue = objProceding.ForumId.ToString();
                        //ddlForum_SelectedIndexChanged(sender, e);
                        BindAllMeetingByForum(ddlForum.SelectedValue);
                        ddlMeeting.SelectedValue = objProceding.MeetingId.ToString();
                        ddlMeeting_SelectedIndexChanged(sender, e);




                        ViewState["EncryptionKey"] = objProceding.EncryptionKey;
                        ViewState["file"] = objProceding.ProcedingName;
                        // lnkView.Visible = true;
                        hdnUploadId.Value = objProceding.ProcedingId.ToString();
                        rfvProceding.Enabled = false;
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
                    string EntityId = Session["EntityId"].ToString();
                    if (isMaker)
                    {
                        string strProcedingId = Convert.ToString(e.CommandArgument);
                        //string[] ids = strUploadMinId.Split(',');
                        if (objUser.isDelete(Guid.Parse(EntityId)))
                        {
                            bool status = ProcedingDataProvider.Instance.Delete(Guid.Parse(strProcedingId));
                            if (status)
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Proceeding", "Success", Convert.ToString(strProcedingId), "Proceeding deleted successfully");

                                ((Label)Info.FindControl("lblName")).Text = "Proceeding deleted successfully";
                                Info.Visible = true;
                                BindProceding1(1);
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Proceeding", "Failed", Convert.ToString(strProcedingId), "Proceeding deletion failed");

                                ((Label)Error.FindControl("lblError")).Text = "Proceeding deletion failed";
                                Error.Visible = true;
                            }

                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Proceeding", "Failed", Convert.ToString(strProcedingId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Proceeding", "Failed", "", "Sorry you are not maker");

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

        protected void grdProceding_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdProceding.PageIndex = e.NewPageIndex;

            BindProceding1(e.NewPageIndex);

        }

        protected void grdProceding_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdProceding_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

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
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByForProceeding(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting <= dtToday)
                        //{
                        //   ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("dd MMM yyyy"), item.MeetingId.ToString()));
                        ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                        // }
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
        /// BInd All Meeting list by forum to drop down
        /// </summary>
        /// <param name="ForumId">string specifying ForumId</param>
        private void BindAllMeetingByForum(string ForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                Guid forumId;
                if (Guid.TryParse(ForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetAllMeetingByForProceeding(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
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

        //private void BindAgenda(string strMeetingId)
        //{
        //    Guid meetingId;
        //    if (Guid.TryParse(strMeetingId, out meetingId))
        //    {
        //        ViewState["MeetingId"] = meetingId;

        //        StringBuilder strMenu = new StringBuilder("");
        //        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

        //        if (objAgentList.Count > 0)
        //        {
        //            //hdnMeeting.Value = objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
        //            //    ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
        //            //get only parent agenda
        //            var objParentAgenda = from agend in objAgentList
        //                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                  select agend;

        //            ddlAgenda.DataSource = objParentAgenda;
        //            ddlAgenda.DataBind();
        //            ddlAgenda.DataTextField = "AgendaName";
        //            ddlAgenda.DataValueField = "AgendaId";
        //            ddlAgenda.DataBind();
        //            ddlAgenda.Items.Insert(0, new ListItem("Select Agenda", "0"));
        //        }
        //    }
        //}


        //private void BindSubAgenda(string strAgendaId)
        //{
        //    Guid agendaId;
        //    if (Guid.TryParse(strAgendaId, out agendaId))
        //    {
        //        ViewState["AgendaId"] = agendaId;
        //        Guid meetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
        //        StringBuilder strMenu = new StringBuilder("");
        //        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

        //        if (objAgentList.Count > 0)
        //        {
        //            //hdnMeeting.Value = objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
        //            //    ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
        //            //get only parent agenda
        //            var objParentAgenda = from agend in objAgentList
        //                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                  select agend;

        //            //get only sub agenda
        //            var subAgendaList = from agend in objAgentList
        //                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                select agend;


        //            var subAgendaName = (from subAgenda in subAgendaList
        //                                 where (subAgenda.ParentAgendaId == agendaId)
        //                                 select (new
        //                                 {
        //                                     AgendaName = subAgenda.AgendaName,
        //                                     AgendaId = subAgenda.AgendaId,
        //                                     UplaodedAgenda = subAgenda.UploadedAgendaNote,
        //                                     AgendaNote = subAgenda.AgendaNote,
        //                                     DeletedAgenda = subAgenda.DeletedAgenda

        //                                 })).ToList();

        //            ddlSubAgenda.DataSource = subAgendaName;
        //            ddlSubAgenda.DataBind();
        //            ddlSubAgenda.DataTextField = "AgendaName";
        //            ddlSubAgenda.DataValueField = "AgendaId";
        //            ddlSubAgenda.DataBind();
        //            ddlSubAgenda.Items.Insert(0, new ListItem("Select Sub Agenda", "0"));
        //        }
        //    }
        //}

        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strForumId = ddlForum.SelectedValue;

            if (strForumId != "0")
            {
                BindMeeting(strForumId);

            }
            if (sender.ToString() != "System.Web.UI.WebControls.GridView")
            {
                BindProceding1(1);
            }
        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMeetingId = ddlMeeting.SelectedValue;
            //Session["MeetingId"] = strMeetingId;

            //if (strMeetingId != "0")
            //{
            //    BindAgenda(strMeetingId);
            //}

            if (sender.ToString() != "System.Web.UI.WebControls.GridView")
            {
                BindProceding1(1);
            }
        }

        //protected void ddlAgenda_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string strAgendaId = ddlAgenda.SelectedValue;
        //    // Session["AgendaId"] = strAgendaId;
        //    if (strAgendaId != "0")
        //    {
        //        BindSubAgenda(strAgendaId);
        //    }

        //    if (sender.ToString() != "System.Web.UI.WebControls.GridView")
        //    {
        //        BindProceding1(1);
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ProcedingDomain objProcedingDomain = new ProcedingDomain();
                UserAcess objUser = new UserAcess();
                if (hdnUploadId.Value == "")
                {
                    if (!fuProceding.HasFile)
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

                //if (ddlAgenda.SelectedValue == "0")
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Please select Agenda.";
                //    Error.Visible = true;
                //    return;
                //}

                //if (ddlSubAgenda.SelectedValue == "0")
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Please select Sub Agenda.";
                //    Error.Visible = true;
                //    return;
                //}

                objProcedingDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objProcedingDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objProcedingDomain.ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                //objProcedingDomain.AgendaId = Guid.Parse(ddlAgenda.SelectedValue.ToString());
                //objProcedingDomain.SubAgendaId = Guid.Parse(ddlSubAgenda.SelectedValue.ToString());

                string fileName = "";

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                if (isMaker)
                {

                    if (hdnUploadId.Value != "")
                    {
                        string EncryptionKey = ViewState["EncryptionKey"].ToString();
                        objProcedingDomain.EncryptionKey = EncryptionKey;
                        if (objUser.IsEdit(EntityId))
                        {

                            if (fuProceding.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                                string contentType = fuProceding.PostedFile.ContentType;

                                string ext = Path.GetExtension(fuProceding.FileName);

                                if (!objUser.isValidChar_old(fuProceding.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                if (contentType.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuProceding.PostedFile.InputStream);
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

                                        fuProceding.PostedFile.SaveAs(Server.MapPath(savePath + fileName));





                                        EncryptionHelper.EncryptString(Server.MapPath(savePath + fileName), EncryptionKey);
                                        objProcedingDomain.ProcedingName = fileName;
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
                                        ((Label)Error.FindControl("lblError")).Text = "Proceeding file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Proceeding file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    objProcedingDomain.ProcedingName = Convert.ToString(ViewState["file"]);
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Please upload pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            // objProcedingDomain.MeetingId = Guid.Parse(Convert.ToString(ViewState["Meeting"]));

                            objProcedingDomain.ProcedingId = Guid.Parse(hdnUploadId.Value);

                            bool status = ProcedingDataProvider.Instance.Update(objProcedingDomain);
                            if (status)
                            {
                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objProcedingDomain);
                                var encrypt = Encryptor.EncryptString(json);

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Success", Convert.ToString(objProcedingDomain.ProcedingId), "Proceeding updated successfully :- " + encrypt + "");

                                ((Label)Info.FindControl("lblName")).Text = "Proceeding updated successfully";

                                Info.Visible = true;
                                // ClearData();
                                Session["ForumId"] = ddlForum.SelectedValue;
                                Session["MeetingId"] = ddlMeeting.SelectedValue;


                                Response.StatusCode = 302;
                                Response.AddHeader("Location", "ProceedingMaster.aspx");
                                Session["MessageProceeding"] = ((Label)Info.FindControl("lblName")).Text;

                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Failed", Convert.ToString(objProcedingDomain.ProcedingId), "Proceeding updation failed");

                                ((Label)Error.FindControl("lblError")).Text = "Proceeding updation failed";
                                Error.Visible = true;
                            }
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {

                        //check edit permission
                        if (objUser.IsAdd(EntityId))
                        {
                            if (fuProceding.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                                string contentType = fuProceding.PostedFile.ContentType;

                                if (!objUser.isValidChar_old(fuProceding.FileName))
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                    Error.Visible = true;
                                    return;
                                }

                                string ext = Path.GetExtension(fuProceding.FileName);
                                if (contentType.ToLower().Contains("application/pdf"))
                                {
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        System.IO.BinaryReader r = new System.IO.BinaryReader(fuProceding.PostedFile.InputStream);
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
                                        fuProceding.PostedFile.SaveAs(Server.MapPath(savePath + fileName));
                                        EncryptionHelper.EncryptString(Server.MapPath(savePath + fileName), EncryptionKey);
                                        objProcedingDomain.ProcedingName = fileName;
                                        objProcedingDomain.EncryptionKey = EncryptionKey;
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
                                        ((Label)Error.FindControl("lblError")).Text = "Upload Proceeding file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Upload Proceeding file must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                if (ViewState["file"] != null)
                                {
                                    objProcedingDomain.ProcedingName = Convert.ToString(ViewState["file"]);
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Please upload pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            objProcedingDomain.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);

                            objProcedingDomain = ProcedingDataProvider.Instance.Insert(objProcedingDomain);
                            if (!objProcedingDomain.ProcedingId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                            {
                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objProcedingDomain);
                                var encrypt = Encryptor.EncryptString(json);

                                if (objProcedingDomain.ProcedingId.ToString().Equals("11000000-0000-0100-0100-000000000011"))
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Success", Convert.ToString(objProcedingDomain.ProcedingId), "Proceeding updated successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = "Proceeding Updated successfully";
                                    Info.Visible = true;
                                }
                                else
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Success", Convert.ToString(objProcedingDomain.ProcedingId), "Proceeding inserted successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = "Proceeding inserted successfully";
                                    Info.Visible = true;
                                }
                                Session["ForumId"] = ddlForum.SelectedValue;
                                Session["MeetingId"] = ddlMeeting.SelectedValue;
                                //Session["SubAgendaId"] = ddlSubAgenda.SelectedValue;
                                //Session["AgendaId"] = ddlAgenda.SelectedValue;


                                //ClearData();
                                Response.StatusCode = 302;
                                Response.AddHeader("Location", "ProceedingMaster.aspx");
                                Session["MessageProceeding"] = ((Label)Info.FindControl("lblName")).Text;
                            }


                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Failed", Convert.ToString(objProcedingDomain.ProcedingId), "Proceeding insertion failed");

                                ((Label)Error.FindControl("lblError")).Text = " Proceeding insertion failed ";
                                Error.Visible = true;
                                Info.Visible = false;
                            }
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objProcedingDomain.UpdatedBy), "Proceeding", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            Info.Visible = false;
                        }
                    }
                    //BindProceding();
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


        private void ClearData()
        {

            ddlMeeting.Items.Clear();
            ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
            ddlForum.Items.Clear();
            ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
            //ddlAgenda.Items.Clear();
            //ddlAgenda.Items.Insert(0, new ListItem("Select Agenda", "0"));
            //ddlSubAgenda.Items.Clear();
            //ddlSubAgenda.Items.Insert(0, new ListItem("Select SubAgenda", "0"));

            //ddlMeeting.SelectedValue = "0";
            //ddlForum.SelectedValue = "0";
            //ddlAgenda.SelectedValue = "0";
            //ddlSubAgenda.SelectedValue = "0";
            ddlForum.Enabled = true;
            ddlMeeting.Enabled = true;
            btnSubmit.Text = "Submit";
            ViewState["file"] = null;
            //lnkView.Visible = false;
            hdnUploadId.Value = "";
            rfvProceding.Enabled = true;
            // ddlForum.Enabled = true;
            // ddlMeeting.Enabled = true;
            ViewState["Meeting"] = null;
            ViewState["EncryptionKey"] = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            BindForum(EntityId.ToString());
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProceedingMaster.aspx");
        }

        private void BindProceding1(int pageIndex)
        {

            Guid ForumId = Guid.Empty;
            Guid MeetingId = Guid.Empty;
            //Guid AgendaId = Guid.Empty;
            //Guid SubAgendaId = Guid.Empty;


            if (ddlForum.SelectedValue != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue);
            }

            if (ddlMeeting.SelectedValue != "0")
            {
                MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
            }

            //if (ddlAgenda.SelectedValue != "0")
            //{
            //    AgendaId = Guid.Parse(ddlAgenda.SelectedValue);
            //}

            //if (ddlSubAgenda.SelectedValue != "0")
            //{
            //    SubAgendaId = Guid.Parse(ddlSubAgenda.SelectedValue);
            //}


            System.Data.DataSet ds = ProcedingDataProvider.Instance.GetProcedingsForSearch(ForumId, MeetingId, pageIndex, grdProceding.PageSize);

            if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                if (totalCount > grdProceding.PageSize)
                {
                    PopulatePager(totalCount, pageIndex);
                }
                else
                {
                    //clear page data
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdProceding.DataSource = ds;
                //grdProceding.PageCount = 
                grdProceding.DataBind();
            }
            else
            {

                grdProceding.DataSource = null;
                grdProceding.DataBind();
            }


        }


        protected void PageSize_Changed(object sender, EventArgs e)
        {
            BindProceding1(1);
        }


        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            BindProceding1(pageIndex);
        }

        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / grdProceding.PageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                if (currentPage == 1 && pageCount <= 10)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage == 1 && pageCount > 10)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage != 1 && pageCount <= 10)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage != 1 && pageCount > 10 && currentPage != pageCount)
                {
                    int z = currentPage - 5;
                    int x, y;
                    if (z < 1)
                    {
                        z = 1;
                        for (int i = z; i <= 10; i++)
                        {
                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                        }
                    }
                    else if (z >= 1)
                    {
                        if (pageCount < currentPage + 4)
                        {
                            y = (z - ((currentPage + 4) - pageCount));
                            x = pageCount;
                        }
                        else
                        {
                            x = currentPage + 4;
                            y = z;
                        }
                        for (int i = y; i <= x; i++)
                        {
                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                        }
                    }
                }
                else if (currentPage == pageCount)
                {
                    for (int i = pageCount - 9; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                    }
                }

                //for (int i = 1; i <= pageCount; i++)
                //{
                //    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                //}

            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }


        private void BindProceding(object sender, EventArgs e)
        {
            Guid ForumId = Guid.Empty;
            Guid MeetingId = Guid.Empty;
            Guid AgendaId = Guid.Empty;
            Guid SubAgendaId = Guid.Empty;


            if (ddlForum.SelectedValue != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue);
            }

            if (ddlMeeting.SelectedValue != "0")
            {
                MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
            }




            DataSet ds = ProcedingDataProvider.Instance.GetProcedingsForSearch(ForumId, MeetingId);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdProceding.DataSource = ds;
                grdProceding.DataBind();
            }
            //Guid ForumId = Guid.Empty;
            //Guid MeetingId = Guid.Empty;
            //Guid AgendaId = Guid.Empty;
            //Guid SubAgendaId = Guid.Empty;
            //if (Session["ForumId"] == null)
            //{
            //    if (ddlForum.SelectedValue!="0")
            //    {
            //        ForumId = Guid.Parse(ddlForum.SelectedValue);
            //    }

            //}
            //if (Session["ForumId"] != null)
            //{
            //    ForumId = Guid.Parse(Session["ForumId"].ToString());
            //    ddlForum.SelectedValue = Session["ForumId"].ToString();
            //    ddlForum_SelectedIndexChanged(sender, e);
            //}


            //if (Session["MeetingId"] == null)
            //{
            //    if (ddlMeeting.SelectedValue!="0")
            //    {
            //        MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
            //    }

            //}
            //if (Session["MeetingId"] != null)
            //{
            //    MeetingId = Guid.Parse(Session["MeetingId"].ToString());
            //    ddlMeeting.SelectedValue = Session["MeetingId"].ToString();
            //    ddlMeeting_SelectedIndexChanged(sender, e);
            //}


            //if (Session["AgendaId"] == null)
            //{
            //    if (ddlAgenda.SelectedValue!="0")
            //    {
            //        AgendaId = Guid.Parse(ddlAgenda.SelectedValue);
            //    }

            //}
            //if (Session["AgendaId"] != null)
            //{
            //    AgendaId = Guid.Parse(Session["AgendaId"].ToString());
            //    ddlAgenda.SelectedValue = Session["AgendaId"].ToString();
            //    ddlAgenda_SelectedIndexChanged(sender, e);
            //}

            //if (Session["SubAgendaId"] == null)
            //{
            //    if (ddlSubAgenda.SelectedValue!="0")
            //    {
            //        SubAgendaId = Guid.Parse(ddlSubAgenda.SelectedValue);
            //    }

            //}
            //if (Session["SubAgendaId"] != null)
            //{
            //    SubAgendaId = Guid.Parse(Session["SubAgendaId"].ToString());
            //    ddlSubAgenda.SelectedValue = Session["SubAgendaId"].ToString();
            //    //ddlSubAgenda_SelectedIndexChanged(sender, e);
            //}


            //DataSet ds=ProcedingDataProvider.Instance.Get(ForumId, MeetingId, AgendaId, SubAgendaId);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    grdProceding.DataSource = ds;

            //    //grdUploadMinutes.DataSource = UploadMintesDataProvider.Instance.GetUploadMinutesByUser(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

            //    grdProceding.DataBind();
            //}
            //Session["ForumId"] = null;
            //Session["MeetingId"] = null;
            //Session["SubAgendaId"] = null;
            //Session["AgendaId"] = null;
        }

        //protected void ddlSubAgenda_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string strSubAgenda = ddlSubAgenda.SelectedValue;
        //        //   Session["SubAgendaId"] = strSubAgenda;
        //        //BindProceding(sender, e);
        //        BindProceding1(1);
        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);
        //    }
        //}
    }
}