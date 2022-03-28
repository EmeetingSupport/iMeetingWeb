using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeetingMinder.Web
{
    public partial class DraftMinute : System.Web.UI.Page
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
                //Bind Entity List
                #region
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());

                    // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

            }
        }

        /// <summary>
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    BindMeeting(strForumId);
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
        /// Bind Meetings list to drop down
        /// </summary>
        /// <param name="strForumId">string specifying strForumId</param>
        private void BindMeeting(string strForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting > dtToday)
                        //{
                        ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));

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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId);
                    // IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
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


        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {

                    IList<DraftMOMDomain> objDraftMom = DraftMOMDataProvider.Instance.GetDraftMOMDomainByMeetingid(Guid.Parse(strMeetingId));
                    grdDraftMOM.DataSource = objDraftMom;
                    grdDraftMOM.DataBind();
                }
                else
                {
                    grdDraftMOM.DataSource = null;
                    grdDraftMOM.DataBind();
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
        /// gridview row command event event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdDraftMOM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("download"))
                {
                    string fileName = Convert.ToString(e.CommandArgument);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);

                    //Set the appropriate ContentType.
                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.WriteFile(FilePath);
                    Response.End();
                }
                if (e.CommandName.ToLower().Equals("editagenda"))
                {
                    //check edit permission
                    if (objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        string fileName = Convert.ToString(e.CommandArgument);
                        DraftMOMDomain objDraft = DraftMOMDataProvider.Instance.Get(Guid.Parse(fileName));
                        btnInsert.Text = "Update";
                        hdnMOMId.Value = objDraft.DraftMinutesId.ToString();
                        ViewState["file"] = objDraft.DraftMinute;

                        lnkView.Visible = true;
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                if (e.CommandName.ToLower().Equals("delete"))
                {

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        // check delete permission
                        if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            Guid DraftMOMId = Guid.Parse(e.CommandArgument.ToString());
                            bool status = DraftMOMDataProvider.Instance.Delete(DraftMOMId);
                            if (status)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Draft minutes deleted successfully";
                                Info.Visible = true;
                                Error.Visible = false;
                                ddlMeeting_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Draft minutes deletion failed";
                                Error.Visible = true;
                                Info.Visible = false;
                            }
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
        protected void grdDraftMOM_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// button save click event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    DraftMOMDomain objDraftMom = new DraftMOMDomain();
                    string MOMName = "";
                    bool isUploaded = false;
                    if (Request.Files.Count > 0)
                    {
                        for (int i = 0; i <= Request.Files.Count - 1; i++)
                        {
                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                            string contentType = Request.Files[i].ContentType;
                            if (Request.Files[i].ContentLength > 0)
                            {
                                string ext = Path.GetExtension(Request.Files[i].FileName);
                                if (ext.ToLower().Equals(".pdf"))
                                {
                                    isUploaded = true;
                                    MOMName = Guid.NewGuid() + ext;
                                    Request.Files[i].SaveAs(Server.MapPath(savePath + MOMName));
                                    objDraftMom.DraftMinute = MOMName;
                                    objDraftMom.DraftMinuteName = Request.Files[i].FileName;
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Please select a pdf file to upload ";
                                    Error.Visible = true;
                                }
                            }
                        }
                    }
                    objDraftMom.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    objDraftMom.MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
                    objDraftMom.EntityId = Guid.Parse(Session["EntityId"].ToString());
                    UserAcess objUser = new UserAcess();
                    if (hdnMOMId.Value.Length > 0)
                    {
                        // check add permission
                        if (objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            objDraftMom.DraftMinutesId = Guid.Parse(hdnMOMId.Value);
                            bool staus = DraftMOMDataProvider.Instance.Update(objDraftMom);
                            if (staus)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Draft minutes updated successfully";
                                Info.Visible = true;
                                Error.Visible = false;
                                ddlMeeting_SelectedIndexChanged(sender, e);
                                hdnMOMId.Value = "";
                              
                                ViewState["file"] = null;

                                lnkView.Visible = false;
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Draft minutes updatation failed ";
                                Error.Visible = true;
                                Info.Visible = false;
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
                        // check add permission
                        if (objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            objDraftMom.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                            if (isUploaded)
                            {
                                DraftMOMDataProvider.Instance.Insert(objDraftMom);
                                ((Label)Info.FindControl("lblName")).Text = "Draft minutes inserted successfully";
                                Info.Visible = true;
                                ddlMeeting_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Please select a file to upload ";
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
        /// button cancel click event
        /// </summary>
        /// <param name="sender">object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("DraftMinute.aspx");
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

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=MOM.pdf");
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
    }
}