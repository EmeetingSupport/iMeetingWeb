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
using System.Globalization;


namespace MeetingMinder.Web
{
    public partial class WebForm1 : System.Web.UI.Page
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
            BindMeetingVenue();
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //Bind User to drop down
                // BindUsers();

                //Bind Entity List
                #region
                ///Comment for hide entity name
                BindEntity();
                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                    // BindConvenors();
                    ddlEntity.SelectedValue = EntityId.ToString();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                txtMeetingDate.Attributes.Add("ReadOnly", "ReadOnly");
                txtTime.Attributes.Add("ReadOnly", "ReadOnly");

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:table-row;");
                    divEntity1.Attributes.Add("style", "display:table-row;");
                    rfvddUser.Enabled = true;
                    chkChecker.Checked = true;
                    chkChecker.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Bind Meeting venues
        /// </summary>
        private void BindMeetingVenue()
        {
            try
            {
                IList<string> objMeetingVenue = MeetingDataProvider.Instance.GetMeetingVenue().Select(s => System.Web.HttpUtility.HtmlDecode(s)).Distinct().ToList();
                IList<string> distinctList = objMeetingVenue.Select(s => s.Trim()).Distinct().ToList();
                string strMeetVenue = string.Join(",", distinctList.ToArray());
                hdnMeetingVenue.Value = strMeetVenue;
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
        /// Bind convenors to drop down list
        /// </summary>
        private void BindConvenors()
        {
            try
            {
                ddlConvenor.DataSource = UserDataProvider.Instance.Get().OrderBy(p => p.FirstName).ToList();
                ddlConvenor.DataBind();
                ddlConvenor.DataTextField = "UserName";
                ddlConvenor.DataValueField = "UserId";
                ddlConvenor.DataBind();
                // ddlConvenor.Items.Insert(0, new ListItem("Select User", "0"));
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }

        /// <summary> 
        /// Bind Forum List to drop down
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
                    //ScriptManager.RegisterStartupScript(ResultsUpdatePanel, ResultsUpdatePanel.GetType(), "OpenConfirm", "Confirm();", true);
                    //return;
                    BindForum(strEntityId);

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
        /// Bind User List to drop down
        /// </summary>
        private void BindUsers(Guid ForumId)
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";

                ddlUser.DataSource = null;
                ddlUser.DataBind();

                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker(ForumId).OrderBy(p => p.FirstName).ToList();
                if (objUser.Count > 0)
                {
                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();

                    ddlConvenor.DataSource = dt;
                    ddlConvenor.DataBind();
                    ddlConvenor.DataTextField = "FullName";
                    ddlConvenor.DataValueField = "UserId";
                    ddlConvenor.DataBind();
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
        /// Bind Meeintg List
        /// </summary>
        private void BindMeeting()
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetApprovedMeetingByUser(UserId);
                if (objMeeting.Count > 0)
                {
                    DataTable orderedList = objMeeting.OrderByDescending(p => DateTime.Parse(p.MeetingDate)).ToList().AsDataTable();
                    grdMeeting.DataSource = orderedList;
                    grdMeeting.DataBind();
                }
                else
                {
                    grdMeeting.DataSource = null;
                    grdMeeting.DataBind();
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
        /// Delelte Seleted Meeting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lbRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                //check Delete permission
                if (objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                {

                    //StringBuilder strQuery = new StringBuilder("");
                    StringBuilder strMeetingIds = new StringBuilder(",");
                    int count = 0;
                    // get all checked meeting
                    for (int i = 0; i < grdMeeting.Rows.Count; i++)
                    {


                        CheckBox chkSelect = (CheckBox)grdMeeting.Rows[i].FindControl("chkSubAdmin");
                        if (chkSelect.Checked)
                        {
                            count++;
                            string MeetingID = Convert.ToString(grdMeeting.DataKeys[i].Value.ToString());
                            // strQuery.Append("	DELETE FROM [Meeting]   WHERE  MeetingId = '" + MeetingID + "'  ");
                            strMeetingIds.Append(MeetingID + ",");
                        }
                    }
                    if (count > 0)
                    {
                        bool bStatus = MeetingDataProvider.Instance.DeleteSelectedMeeting(strMeetingIds.ToString());
                        if (!bStatus)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Meeting deleted successfully";
                            Info.Visible = true;
                            BindMeeting();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please select at least one checkbox";
                        Error.Visible = true;
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                    Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

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
                if (txtMeetingDate.Text != "")
                {
                    if (!objUser.isValidChar(txtMeetingDate.Text))
                    {
                        txtMeetingDate.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtMeetingDate.Text))
                    {
                        txtMeetingDate.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid meeting date.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter meeting date";
                    Error.Visible = true;
                    return;
                }

                if (txtMeetingVenue.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtMeetingVenue);
                    if (!objUser.isValidChar(txtMeetingVenue.Text))
                    {
                        txtMeetingVenue.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtMeetingVenue.Text))
                    {
                        txtMeetingVenue.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid meeting venue.";
                        Error.Visible = true;
                        return;
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter meeting venue";
                    Error.Visible = true;
                    return;
                }

                if (txtTime.Text != "")
                {
                    if (!objUser.isValidChar(txtTime.Text))
                    {
                        txtTime.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtTime.Text))
                    {
                        txtTime.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid meeting time.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter meeting time";
                    Error.Visible = true;
                    return;
                }

                if (txtNumber.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtNumber);
                    if (!objUser.isValidChar(txtNumber.Text))
                    {
                        txtNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtNumber.Text))
                    {
                        txtNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtNotice.Text == "")
                {
                    txtNotice.Text = "";
                    ((Label)Error.FindControl("lblError")).Text = " Please enter meeting notice.";
                    Error.Visible = true;
                    return;
                }

                if (ddlForum.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select forum.";
                    Error.Visible = true;
                    return;
                }

                if (txtConvenorName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtConvenorName);
                    if (!objUser.isValidChar(txtConvenorName.Text))
                    {
                        txtConvenorName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtConvenorName.Text))
                    {
                        txtConvenorName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtEditReason.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtEditReason);
                    if (!objUser.isValidChar(txtEditReason.Text))
                    {
                        txtEditReason.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (txtEditReason.Text.Length >= 150)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " You have exceeded the maximum character limit.  150.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtEditReason.Text))
                    {
                        txtEditReason.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtMeetingDate.Text != "" && txtMeetingVenue.Text != "")
                {
                    MeetingDomain objMeeting = new MeetingDomain();
                    objMeeting.MeetingNumber = txtNumber.Text.Trim();
                    objMeeting.MeetingVenue = txtMeetingVenue.Text.Trim();
                    string dtMeet = txtMeetingDate.Text.Replace('.', '/');
                    string dtMeetTime = txtTime.Text.Replace('.', ':');

                    objMeeting.Header = txtHeader.Text;

                    DateTime dtMeeting = Convert.ToDateTime(dtMeet + " " + dtMeetTime, new CultureInfo("hi-IN"));
                    //  DateTime dtMeeting = Convert.ToDateTime(txtMeetingDate.Text + " " + txtTime.Text);
                    DateTime dtToday = DateTime.Now;
                    if (dtMeeting < dtToday)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Meeting time can not be earlier than current time.";
                        Error.Visible = true;
                        return;
                    }
                    objMeeting.MeetingDate = dtMeeting.ToString("MM/dd/yyyy");
                    objMeeting.MeetingTime = dtMeetTime;
                    Guid CheckerId;
                    if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                    {
                        objMeeting.MeetingChecker = CheckerId;
                    }

                    objMeeting.ForumId = Guid.Parse(ddlForum.SelectedValue);

                    objMeeting.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                    objMeeting.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    objMeeting.MeetingType = ddlMeetingType.SelectedValue.ToString();

                    if (ddlConvenor.SelectedValue != "0")

                    {//      objMeeting.Convenor = Guid.Parse(ddlConvenor.SelectedValue);
                        List<string> selected = ddlConvenor.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();

                        string Users = string.Join(",", selected);
                        objMeeting.Convenor = Users;
                    }
                    objMeeting.ConvenorName = txtConvenorName.Text.Trim();
                    objMeeting.EditOrCancelReason = txtEditReason.Text.Trim();

                    NoticeDomain objNotice = new NoticeDomain();

                    objNotice.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    objNotice.CreatedBy = Guid.Parse(Session["UserId"].ToString());

                    //txtNotice.Text.Replace("{General_Manager}", txtGM.Text);

                    objNotice.NoticeChecker = CheckerId;//Guid.Parse(ddlUser.SelectedValue);
                    objNotice.NoticeMessage = txtNotice.Text;
                    //"Notice is hereby given that the meeting of the " + ddlForum.SelectedItem.Text + " of " + ddlEntity.SelectedItem.Text + " will be held on " + Convert.ToDateTime(txtMeetingDate.Text, new CultureInfo("hi-IN")).ToString("D") + " at " + txtTime.Text + " in " + txtMeetingVenue.Text + ". The Agenda of the Meeting is enclosed.";

                    string SendToChecker;
                    if (chkChecker.Checked)
                    {
                        SendToChecker = "Pending";
                        objNotice.IsApproved = "Pending";

                    }
                    else
                    {
                        SendToChecker = "Approved";
                        objNotice.IsApproved = "Approved";
                    }

                    if (objMeeting.CreatedBy == objMeeting.MeetingChecker)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                        Error.Visible = true;
                        Info.Visible = false;
                        return;
                    }

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        //Update Meeting 
                        if (hdnMeetingId.Value != "" && hdnMeetingId.Value != null)
                        {

                            //check update permission
                            if (objUser.IsEdit(Guid.Parse(ddlEntity.SelectedValue)))
                            {
                                //objMeeting.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                                objMeeting.MeetingId = Guid.Parse(hdnMeetingId.Value);

                                //Guid CreatedBy = Guid.Parse(ViewState["CreatedBy"].ToString());

                                //if (CreatedBy == objMeeting.MeetingChecker)
                                //{
                                //    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                                //    Error.Visible = true;
                                //    return;
                                //}
                                objNotice.NoticeId = objMeeting.MeetingId;
                                bool status = MeetingDataProvider.Instance.Update(objMeeting, SendToChecker);
                                if (status)
                                {
                                    NoticeDataProvider.Instance.Update(objNotice);
                                    if (SendToChecker == "Pending")
                                    {
                                        if (chkNotifyChecker.Checked)
                                        {
                                            bool isSuccess;
                                            string Message;
                                            SendEmail objSendEmail = new SendEmail();
                                            objSendEmail.SendNotifyEmail("Meeting and Notice", objMeeting.MeetingChecker, out isSuccess, out Message);

                                            if (isSuccess)
                                            {
                                                ((Label)Info.FindControl("lblName")).Text = "  Meeting updated with notice and sent for approval with email notification";
                                                Info.Visible = true;
                                            }
                                            else
                                            {
                                                ((Label)Info.FindControl("lblName")).Text = "  Meeting updated with notice and sent for approval with email notification. ";
                                                ((Label)Error.FindControl("lblError")).Text = Message;
                                                Error.Visible = true;
                                                Info.Visible = true;
                                            }
                                        }
                                        else
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = "  Meeting updated with notice and sent for approval";
                                            Info.Visible = true;
                                        }

                                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objMeeting);
                                        var encrypt = Encryptor.EncryptString(json);

                                        //web logs
                                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Pending", Convert.ToString(objMeeting.MeetingId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                        // ((Label)Info.FindControl("lblName")).Text = "  Meeting updated with notice and sent for approval";
                                    }
                                    else
                                    {
                                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objMeeting);
                                        var encrypt = Encryptor.EncryptString(json);

                                        //web logs
                                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Success", Convert.ToString(objMeeting.MeetingId), "Meeting updated with notice successfully :- " + encrypt + "");

                                        ((Label)Info.FindControl("lblName")).Text = " Meeting updated with notice successfully";
                                    }
                                }
                                else
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Failed", Convert.ToString(objMeeting.MeetingId), "Meeting update failed, Meeting already exists");

                                    ((Label)Error.FindControl("lblError")).Text = "Meeting update failed, Meeting already exists..";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Failed", Convert.ToString(objMeeting.MeetingId), "Sorry access denied");

                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;
                                Info.Visible = false;
                                return;
                            }
                        }
                        //Insert Meeting
                        else
                        {

                            //check add permission
                            if (objUser.IsAdd(Guid.Parse(ddlEntity.SelectedValue)))
                            {

                                objMeeting = MeetingDataProvider.Instance.Insert(objMeeting, SendToChecker);
                                if (objMeeting.MeetingId.ToString().Equals("11000000-0000-0000-0000-000000000011"))
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Failed", Convert.ToString(objMeeting.MeetingId), "Meeting already exists");

                                    ((Label)Error.FindControl("lblError")).Text = "Meeting already exists..";
                                    Error.Visible = true;
                                    return;
                                }
                                objNotice.MeetingId = objMeeting.MeetingId;
                                NoticeDataProvider.Instance.Insert(objNotice);
                                if (SendToChecker == "Pending")
                                {
                                    if (chkNotifyChecker.Checked)
                                    {
                                        bool isSuccess;
                                        string Message;
                                        SendEmail objSendEmail = new SendEmail();
                                        objSendEmail.SendNotifyEmail("Meeting and Notice", objMeeting.MeetingChecker, out isSuccess, out Message);
                                        if (isSuccess)
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = " Meeting inserted with notice and sent for approval with email notification";
                                            Info.Visible = true;
                                        }
                                        else
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = " Meeting inserted with notice and sent for approval. ";
                                            ((Label)Error.FindControl("lblError")).Text = Message;
                                            Error.Visible = true;
                                            Info.Visible = true;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = " Meeting inserted with notice and sent for approval";
                                        Info.Visible = true;
                                    }

                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objMeeting);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Pending", Convert.ToString(objMeeting.MeetingId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                    //((Label)Info.FindControl("lblName")).Text = " Meeting inserted with notice and sent for approval";
                                }
                                else
                                {
                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objMeeting);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Success", Convert.ToString(objMeeting.MeetingId), "Meeting inserted with notice successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = " Meeting inserted with notice successfully";
                                }
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Failed", Convert.ToString(objMeeting.MeetingId), "Sorry access denied");

                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;
                                Info.Visible = false;
                                return;
                            }
                        }

                        Info.Visible = true;
                        //  BindMeeting();
                        ddlForum_SelectedIndexChanged(sender, e);
                        ClearData();

                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objMeeting.CreatedBy), "Meeting", "Failed", Convert.ToString(objMeeting.MeetingId), "Sorry you are not maker");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                        Info.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
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
            //ddlForum.Items.Clear();
            //ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
            txtMeetingDate.Text = "";
            txtMeetingVenue.Text = "";
            hdnMeetingId.Value = "";
            ddlUser.SelectedValue = "0";
            ddlConvenor.SelectedIndex = -1;
            //ddlEntity.SelectedValue = "0";
            chkChecker.Checked = false;
            txtTime.Text = "";
            txtNumber.Text = "";
            txtNotice.Text = "";
            btnInsert.Text = "Save";
            ddlMeetingType.SelectedValue = "0";
            txtHeader.Text = "";
            txtConvenorName.Text = "";

            bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
            bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

            if (IsMaker && !IsChecker)
            {
                divEntity.Attributes.Add("style", "display:table-row;");
                divEntity1.Attributes.Add("style", "display:table-row;");
                rfvddUser.Enabled = true;
                chkChecker.Checked = true;
                chkChecker.Enabled = false;
            }

            trEdit.Visible = false;
        }

        /// <summary>
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ClearData();
                grdMeeting.PageIndex = e.NewPageIndex;
                BindMeeting();
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
        protected void grdMeetingRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //delete meeting
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        string MeetingId = Convert.ToString(e.CommandArgument.ToString());
                        string[] ids = MeetingId.Split(',');
                        UserAcess objUser = new UserAcess();
                        //check Delete permission
                        if (objUser.isDelete(Guid.Parse(ids[1])))
                        {
                            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                            Label lblMeeting = (Label)row.Cells[0].FindControl("lblMeetingDate");
                            Label lblMeetingTime = (Label)row.Cells[0].FindControl("lblMeetingTime");


                            if (lblMeeting.Text.Length > 0)
                            {
                                DateTime dtNow = DateTime.Now;
                                DateTime dtMeetingDate = Convert.ToDateTime(lblMeeting.Text + " " + lblMeetingTime.Text);
                                if (dtMeetingDate.Date < dtNow.Date)
                                {
                                    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't delete past meeting. ";
                                    Error.Visible = true;

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Sorry you can't delete past meeting");

                                    return;
                                }
                            }

                            string DeleteReason = hdnDeleteReason.Value.Trim();
                            if (DeleteReason.Length >= 150)
                            {
                                ((Label)Error.FindControl("lblError")).Text = " You have exceeded the maximum character limit.  150.";
                                Error.Visible = true;
                                return;
                            }

                            bool bStatus = MeetingDataProvider.Instance.Delete(Guid.Parse(ids[0]), DeleteReason);

                            ((Label)Info.FindControl("lblName")).Text = "Meeting deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Success", Convert.ToString(Guid.Parse(ids[0])), "Meeting deleted successfully");

                            //if (bStatus == true)
                            //{
                            //    ((Label)Info.FindControl("lblName")).Text = "Meeting deleted successfully";
                            //    Info.Visible = true;

                            //    //web logs
                            //    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Success", Convert.ToString(Guid.Parse(ids[0])), "Meeting deleted successfully");
                            //}
                            //else
                            //{
                            //    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            //    Error.Visible = true;

                            //    //web logs
                            //    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Meeting deletion failed");
                            //}
                            // BindMeeting();
                            ddlForum_SelectedIndexChanged(sender, e);
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", "", "Sorry you are not maker");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
                }

                //Edit 
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    UserAcess objUser = new UserAcess();

                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    string[] ids = strMeetingId.Split(',');
                    //check edit permission
                    if (objUser.IsEdit(Guid.Parse(ids[2])))
                    {
                        NoticeDomain objNotice = NoticeDataProvider.Instance.GetNoticeByMeeting(Guid.Parse(ids[0]));
                        if (objNotice == null)
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Please approve notice to edit");

                            ((Label)Error.FindControl("lblError")).Text = " Please approve notice to edit.";
                            Error.Visible = true;
                            return;
                        }
                        txtNotice.Text = objNotice.NoticeMessage;
                        MeetingDomain objMeetingDomain = MeetingDataProvider.Instance.Get(Guid.Parse(ids[0]));
                        DateTime dtNow = DateTime.Now;
                        DateTime dtMeetingDate = Convert.ToDateTime(objMeetingDomain.MeetingDate + " " + objMeetingDomain.MeetingTime);
                        if (dtMeetingDate.Date < dtNow.Date)
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Meeting", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Sorry you cant edit past meeting");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry you cant edit past meeting. ";
                            Error.Visible = true;
                            return;
                        }
                        ddlEntity.SelectedValue = Convert.ToString(ids[2]);
                        ddlEntity_SelectedIndexChanged(sender, e);

                        ddlForum.SelectedValue = Convert.ToString(ids[1]);

                        //convert date to dd/mm/yyyy formate
                        // txtMeetingDate.Text = Convert.ToDateTime(objMeetingDomain.MeetingDate).ToString("MM/dd/yyyy");
                        txtMeetingDate.Text = Convert.ToDateTime(objMeetingDomain.MeetingDate).ToString("dd.MM.yyyy");
                        txtMeetingVenue.Text = objMeetingDomain.MeetingVenue;
                        hdnMeetingId.Value = Convert.ToString(objMeetingDomain.MeetingId);
                        txtTime.Text = Convert.ToString(objMeetingDomain.MeetingTime.Replace(':', '.'));
                        txtNumber.Text = objMeetingDomain.MeetingNumber;
                        txtConvenorName.Text = objMeetingDomain.ConvenorName;

                        trEdit.Visible = true;
                        txtEditReason.Text = objMeetingDomain.EditOrCancelReason;

                        //                        if (ddlConvenor.Items.FindByValue(
                        //Convert.ToString(objMeetingDomain.Convenor)) != null)
                        //                        {
                        //                            ddlConvenor.SelectedValue = objMeetingDomain.Convenor.ToString();
                        //                        }
                        //                        else
                        //                        {
                        //                            ddlConvenor.SelectedValue = "0";
                        //                        }
                        ddlConvenor.SelectedIndex = -1;
                        string[] Users = objMeetingDomain.Convenor.ToLower().Split(',');

                        List<ListItem> selected = ddlConvenor.Items.Cast<ListItem>().ToList();

                        foreach (ListItem lstItem in ddlConvenor.Items)
                        {
                            if (Users.Contains(lstItem.Value.ToLower()))
                            {
                                lstItem.Selected = true;
                            }

                        }

                        if (ddlMeetingType.Items.FindByValue(
Convert.ToString(objMeetingDomain.MeetingType)) != null)
                        {
                            ddlMeetingType.SelectedValue = Convert.ToString(objMeetingDomain.MeetingType);
                        }
                        else
                        {
                            ddlMeetingType.SelectedValue = "0";
                        }

                        // ddlMeetingType.SelectedValue = objMeetingDomain.MeetingType;
                        // ddlUser.SelectedValue = Convert.ToString(objMeetingDomain.MeetingChecker);
                        txtHeader.Text = objMeetingDomain.Header;
                        if (ddlUser.Items.FindByValue(
Convert.ToString(objMeetingDomain.MeetingChecker)) != null)
                        {
                            ddlUser.SelectedValue = Convert.ToString(objMeetingDomain.MeetingChecker);
                        }
                        else
                        {
                            ddlUser.SelectedValue = "0";
                        }

                        ViewState["CreatedBy"] = objMeetingDomain.CreatedBy;
                        btnInsert.Text = "Update";
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }

                // view meeting
                if (e.CommandName.ToLower().Equals("view"))
                {

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
        protected void grdMeeting_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{



            //}
            //catch (Exception ex)
            //{
            //    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
            //    Error.Visible = true;
            //}
        }

        /// <summary>
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                //Guid UserId = Guid.Parse(Session["UserId"].ToString());

                //DataTable dt = (DataTable)MeetingDataProvider.Instance.GetApprovedMeetingByUser(UserId).AsDataTable();

                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    Guid forumId;
                    if (Guid.TryParse(strForumId, out forumId))
                    {
                        IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                        DateTime dtToday = DateTime.Now.Date;

                        foreach (MeetingDomain item in objMeeting)
                        {
                            DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                            if (dtMeeting >= dtToday)
                            {
                                objMeetingBind.Add(item);
                            }
                        }

                        DataTable dt = (DataTable)objMeetingBind.AsDataTable();

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

                        grdMeeting.DataSource = dv;
                        grdMeeting.DataBind();
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
        /// ddlForum selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e</param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    //if (ddlForum.SelectedIndex != 0)
                    //{
                    //    btnChangeGM.Visible = true;
                    //}
                    //else
                    //{
                    //    btnChangeGM.Visible = false;
                    //}

                    ForumDomain objforum = new ForumDomain();
                    Session["ForumId"] = strForumId;

                    objforum = ForumDataProvider.Instance.Get(Guid.Parse(strForumId));
                    //txtGM.Text = Convert.ToString(objforum.GeneralManager);
                    //generateMeetingNumber(objforum, strForumId);

                    string strHeader = "";
                    Guid forumId;
                    if (Guid.TryParse(strForumId, out forumId))
                    {
                        IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();

                        //        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId, out strHeader).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                        DateTime dtToday = DateTime.Now;
                        BindUsers(forumId);
                        foreach (MeetingDomain item in objMeeting)
                        {
                            DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate + " " + item.MeetingTime);
                            if (dtMeeting >= dtToday)
                            {
                                objMeetingBind.Add(item);
                            }
                        }
                        grdMeeting.DataSource = objMeetingBind;

                        grdMeeting.DataBind();

                        hdnHeader.Value = strHeader;
                        if (txtHeader.Text.Length > 0)
                        {
                            txtHeader.Text = strHeader;
                        }
                    }
                }
                else
                {
                    //btnChangeGM.Visible = false;
                    //txtGM.Text = "";
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

        //General Manager
        //protected void btnChangeGM_Click(object sender, EventArgs e)
        //{
        //    txtGM.Enabled = true;
        //    btnSave.Visible = true;
        //    btnCancel1.Visible = true;
        //    btnChangeGM.Visible = false;
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ForumDomain objentity = new ForumDomain();
        //        Guid userid = Guid.Parse(Session["UserId"].ToString());
        //        objentity.GeneralManager = txtGM.Text;
        //        objentity.GMUpdatedBy = Guid.Parse(Session["UserId"].ToString());
        //        objentity.ForumId = Guid.Parse(Session["ForumId"].ToString());
        //        bool status = ForumDataProvider.Instance.UpdateGeneralManager(objentity, userid);
        //        if (status == true)
        //        {
        //            ((Label)Info.FindControl("lblName")).Text = " General Manager Updated Successfully";
        //            Info.Visible = true;
        //        }
        //        btnSave.Visible = false;
        //        btnCancel1.Visible = false;
        //        txtGM.Enabled = false;
        //        btnChangeGM.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;
        //        Info.Visible = false;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);
        //    }
        //}

        //protected void btnCancel1_Click(object sender, EventArgs e)
        //{
        //    ForumDomain objentity = ForumDataProvider.Instance.Get(Guid.Parse(Session["ForumId"].ToString()));
        //    txtGM.Text = objentity.GeneralManager;
        //    txtGM.Enabled = false;
        //    btnSave.Visible = false;
        //    btnCancel1.Visible = false;
        //    btnChangeGM.Visible = true;
        //}

    }
}