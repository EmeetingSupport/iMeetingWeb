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
    public partial class RollMaster : System.Web.UI.Page
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

                if (Session["AbMessage"] != null)
                {
                    ((Label)Info.FindControl("lblName")).Text = Session["AbMessage"].ToString();
                    Info.Visible = true;
                    Session["AbMessage"] = null;
                }
            }

            if (!IsPostBack)
            {
                //bind Roll list
                BindRoll();
            }
        }
        /// <summary>
        /// Bind Roll List
        /// </summary>
        private void BindRoll()
        {
            try
            {

                grdRoll.DataSource = RollDataProvider.Instance.Get().OrderBy(p => p.RollName).ToList(); ;
                grdRoll.DataBind();
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
        protected void grdRoll_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdRoll.PageIndex = e.NewPageIndex;
                BindRoll();
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
        protected void grdRoll_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        if (!objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        string strRollMasterId = Convert.ToString(e.CommandArgument);
                        bool IsResponse = RollDataProvider.Instance.Delete(Guid.Parse(strRollMasterId));
                        if (IsResponse)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Role deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Role Master", "Success", strRollMasterId, "Role deleted successfully");

                            BindRoll();
                            ClearAll();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Role Master", "Failed", strRollMasterId, "Role deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = " Role deletion failed";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Role Master", "Failed", "", "Sorry you are not maker");

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
        /// Row deleting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdRoll_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// Code to Row  editing event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdRoll_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                    Error.Visible = true;
                    return;
                }
                string strRollMasterId = grdRoll.DataKeys[e.NewEditIndex].Value.ToString();
                RollDomain objRoll = RollDataProvider.Instance.GetRollById(Guid.Parse(strRollMasterId));
                txtRollName.Text = objRoll.RollName;
                chkView.Checked = objRoll.View;
                chkTransaction.Checked = objRoll.Transaction;
                chkUserMaster.Checked = objRoll.UserMaster;
                chkReport.Checked = objRoll.Report;

                chkAccessRight.Checked = objRoll.AccessRightMaster;
                chkApproval.Checked = objRoll.Approval;
                chkApprovalMaster.Checked = objRoll.ApprovalMaster;
                chkEntityMaster.Checked = objRoll.EntityMaster;

                chkRollMaster.Checked = objRoll.RollMaster;

                chkAgenda.Checked = objRoll.Agenda;
                chkAgendaView.Checked = objRoll.AgendaView;
                chkBod.Checked = objRoll.BoardofDirectore;
                chkEmail.Checked = objRoll.EmailNotification;
                chkIpad.Checked = objRoll.IpadNotification;
                chkMaster.Checked = objRoll.MasterAgenda;
                chkMeeting.Checked = objRoll.Meeting;
                chkMeetingSch.Checked = objRoll.MeetingView;
                chkMinutesView.Checked = objRoll.MinutesView;
                chkNotice.Checked = objRoll.NoticeView;
                chkPhoto.Checked = objRoll.PhotoGallery;
                chkUploadMin.Checked = objRoll.UploadMinutes;
                chkRevisedPdf.Checked = objRoll.RevisedPdf;
                chkGlobal.Checked = objRoll.GlobalSetting;

                chkMeetingScedule.Checked = objRoll.MeetingSchedule;
                chkAboutUs.Checked = objRoll.AboutUs;

                chkDaftMOM.Checked = objRoll.DraftMOM;
                chkDeviceManager.Checked = objRoll.DeviceManager;
                chkClassification.Checked = objRoll.MasterClassification;
                chkPublishHistory.Checked = objRoll.PublishHistory;
                chkEmailHistory.Checked = objRoll.EmailHistory;

                chkSection.Checked = objRoll.SectionMaster;

                chkHeaderMaster.Checked = objRoll.HeaderMaster;
                chkAgendaMarker.Checked = objRoll.AgendaMarker;

                chkAttendanceView.Checked = objRoll.AttendanceView;
                chkProceedingView.Checked = objRoll.ProceedingView;
                chkProceedingMaster.Checked = objRoll.ProceedingMaster;
                chkSerailNoMaster.Checked = objRoll.SerialNoMaster;
                chkTabSetting.Checked = objRoll.TabSetting;

                chkMeetingRetention.Checked = objRoll.MeetingRetention;

                btnInsert.Text = "Update";

                hdnApprovalId.Value = objRoll.RollMasterId.ToString();
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
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdRoll_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)RollDataProvider.Instance.Get().AsDataTable();
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

                grdRoll.DataSource = dv;
                grdRoll.DataBind();
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
                StringBuilder strRollMasterIds = new StringBuilder(",");

                int count = 0;
                //get all checked  Rolls
                for (int i = 0; i < grdRoll.Rows.Count; i++)
                {



                    CheckBox chkSelect = (CheckBox)grdRoll.Rows[i].FindControl("chkSubAdmin");
                    if (chkSelect.Checked)
                    {
                        count++;
                        string RollMasterID = Convert.ToString(grdRoll.DataKeys[i].Value.ToString());

                        strRollMasterIds.Append(RollMasterID + ",");

                    }
                }
                if (count > 0)
                {
                    bool bStatus = RollDataProvider.Instance.DeleteSelected(strRollMasterIds.ToString());
                    if (bStatus)
                    {
                        ((Label)Info.FindControl("lblName")).Text = "Roles deleted successfully";
                        Info.Visible = true;
                        BindRoll();
                        ClearAll();
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
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }


        public bool GetTranDetails(params bool[] tran)
        {
            foreach (bool obj in tran)
            {
                if (obj == true)
                {
                    return true;
                    //break;
                }


            }
            return false;
        }


        public bool GetCheckedTran(params bool[] tran)
        {


            return true;
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

                if (txtRollName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtRollName);
                    if (!objUser.isValidChar(txtRollName.Text))
                    {
                        txtRollName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtRollName.Text))
                    {
                        txtRollName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid role name.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter role name";
                    Error.Visible = true;
                    return;
                }
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    if (txtRollName.Text != "")
                    {
                        RollDomain objRoll = new RollDomain();
                        objRoll.RollName = txtRollName.Text;
                        objRoll.AccessRightMaster = chkAccessRight.Checked;
                        objRoll.Approval = chkApproval.Checked;
                        objRoll.ApprovalMaster = chkApprovalMaster.Checked;

                        objRoll.EntityMaster = chkEntityMaster.Checked;
                        objRoll.Report = chkReport.Checked;
                        objRoll.Transaction = chkTransaction.Checked;
                        objRoll.UserMaster = chkUserMaster.Checked;
                        objRoll.View = chkView.Checked;
                        objRoll.RollMaster = chkRollMaster.Checked;

                        objRoll.Agenda = chkAgenda.Checked;
                        objRoll.AgendaView = chkAgendaView.Checked;
                        objRoll.BoardofDirectore = chkBod.Checked;
                        objRoll.EmailNotification = chkEmail.Checked;
                        objRoll.IpadNotification = chkIpad.Checked;
                        objRoll.Meeting = chkMeeting.Checked;
                        objRoll.MeetingView = chkMeetingSch.Checked;
                        objRoll.MinutesView = chkMinutesView.Checked;
                        objRoll.PhotoGallery = chkPhoto.Checked;
                        objRoll.MasterAgenda = chkMaster.Checked;

                        objRoll.UploadMinutes = chkUploadMin.Checked;
                        objRoll.GlobalSetting = chkGlobal.Checked;
                        objRoll.NoticeView = chkNotice.Checked;
                        objRoll.SectionMaster = chkSection.Checked;

                        objRoll.RevisedPdf = chkRevisedPdf.Checked;

                        objRoll.UpdatedBy = Guid.Parse(Session["UserId"].ToString());

                        objRoll.CreatedBy = Guid.Parse(Session["UserId"].ToString());

                        objRoll.AboutUs = chkAboutUs.Checked;
                        objRoll.MeetingSchedule = chkMeetingScedule.Checked;
                        objRoll.MasterClassification = chkClassification.Checked;

                        objRoll.DraftMOM = chkDaftMOM.Checked;
                        objRoll.DeviceManager = chkDeviceManager.Checked;
                        objRoll.PublishHistory = chkPublishHistory.Checked;
                        objRoll.EmailHistory = chkEmailHistory.Checked;

                        objRoll.HeaderMaster = chkHeaderMaster.Checked;
                        objRoll.AgendaMarker = chkAgendaMarker.Checked;

                        objRoll.AttendanceView = chkAttendanceView.Checked;
                        objRoll.ProceedingView = chkProceedingView.Checked;
                        objRoll.ProceedingMaster = chkProceedingMaster.Checked;
                        objRoll.SerialNoMaster = chkSerailNoMaster.Checked;
                        objRoll.TabSetting = chkTabSetting.Checked;

                        objRoll.MeetingRetention = chkMeetingRetention.Checked;

                        if (hdnApprovalId.Value == "")
                        {
                            if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objRoll.CreatedBy), "Role Master", "Failed", Convert.ToString(objRoll.RollMasterId), "Sorry access denied");
                                return;
                            }
                            objRoll = RollDataProvider.Instance.Insert(objRoll);
                            ((Label)Info.FindControl("lblName")).Text = " Role inserted successfully";

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objRoll);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objRoll.CreatedBy), "Role Master", "Success", Convert.ToString(objRoll.RollMasterId), "Role inserted successfully :- " + encrypt + "");

                            Info.Visible = true;
                            Response.StatusCode = 302;
                            Response.AddHeader("Location", "RoleMaster.aspx");
                            Session["AbMessage"] = ((Label)Info.FindControl("lblName")).Text;

                        }
                        else
                        {
                            if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objRoll.CreatedBy), "Role Master", "Failed", Convert.ToString(objRoll.RollMasterId), "Sorry access denied");

                                return;
                            }
                            objRoll.RollMasterId = Guid.Parse(hdnApprovalId.Value);
                            bool IsSubmit = RollDataProvider.Instance.Update(objRoll);
                            if (IsSubmit)
                            {
                                ((Label)Info.FindControl("lblName")).Text = " Role updated successfully";
                                Info.Visible = true;

                                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objRoll);
                                var encrypt = Encryptor.EncryptString(json);

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objRoll.CreatedBy), "Role Master", "Success", Convert.ToString(objRoll.RollMasterId), "Role updated successfully :- " + encrypt + "");

                                Response.StatusCode = 302;
                                Response.AddHeader("Location", "RoleMaster.aspx");
                                Session["AbMessage"] = ((Label)Info.FindControl("lblName")).Text;
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objRoll.CreatedBy), "Role Master", "Failed", Convert.ToString(objRoll.RollMasterId), "Role updation failed");

                                ((Label)Error.FindControl("lblError")).Text = " Role updation failed";
                                Error.Visible = true;
                            }
                        }
                        BindRoll();
                        ClearAll();

                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Role Master", "Failed", "", "Sorry you are not maker");

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
        /// Insertion or Updation cancel
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearAll();
            //Response.Redirect("default.aspx");
        }

        /// <summary>
        /// Clear form data
        /// </summary>
        public void ClearAll()
        {
            try
            {
                txtRollName.Text = "";
                chkView.Checked = false;
                chkTransaction.Checked = false;
                chkUserMaster.Checked = false;
                chkReport.Checked = false;

                chkAccessRight.Checked = false;
                chkApproval.Checked = false;
                chkApprovalMaster.Checked = false;
                chkEntityMaster.Checked = false;

                chkAgenda.Checked = false;
                chkAgendaView.Checked = false;
                chkBod.Checked = false;
                chkEmail.Checked = false;
                chkIpad.Checked = false;
                chkMaster.Checked = false;
                chkMeeting.Checked = false;
                chkMeetingSch.Checked = false;
                chkMinutesView.Checked = false;
                chkNotice.Checked = false;
                chkPhoto.Checked = false;
                chkUploadMin.Checked = false;

                chkRollMaster.Checked = false;
                chkGlobal.Checked = false;

                chkMeetingScedule.Checked = false;
                chkAboutUs.Checked = false;

                chkRevisedPdf.Checked = false;
                chkDeviceManager.Checked = false;
                chkClassification.Checked = false;
                chkDaftMOM.Checked = false;
                chkPublishHistory.Checked = false;
                chkEmailHistory.Checked = false;
                chkSection.Checked = false;
                chkHeaderMaster.Checked = false;
                chkAgendaMarker.Checked = false;

                chkMeetingRetention.Checked = false;

                hdnApprovalId.Value = "";
                btnInsert.Text = "Save";
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