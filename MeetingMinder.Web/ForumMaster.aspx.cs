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
    public partial class ForumMaster : System.Web.UI.Page
    {
        /// <summary>
        /// page laod event
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
                //bind forum list
                BindForum();

                //Bind User to drop down
                BindUsers();

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:block;");
                    rfvddUser.Enabled = true;
                    chkChecker.Checked = true;
                    chkChecker.Enabled = false;
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
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker().OrderBy(p => p.FirstName).ToList(); ;
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
        /// Bind Froums by Entity Id
        /// </summary>
        private void BindForum()
        {
            try
            {
                // check for entity id
                if (Session["EntityId_Assigneed"] != null)//(Request.QueryString["id"] != null)
                {
                    string strEntiyId = Convert.ToString(Session["EntityId_Assigneed"]); //Convert.ToString(Request.QueryString["id"]);

                    Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));

                    Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                    ltl_bredcrumbs.Text = "";
                    string EntityName = EntityDataProvider.Instance.Get(Guid.Parse(strEntiyId)).EntityName;

                    ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home</a>&nbsp;/&nbsp;<a href=EntityMaster.aspx>" + EntityName + "</a>&nbsp;";
                    Guid entityId;
                    if (Guid.TryParse(strEntiyId, out entityId))
                    {
                        IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();

                        // IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
                        ViewState["entityId"] = entityId;
                        grdForum.DataSource = objForum;
                        grdForum.DataBind();

                        hlChangeOrder.Visible = true;
                        hlChangeOrder.NavigateUrl = "Reorder.aspx?for=1&est=" + strEntiyId;
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid forum search";
                        Error.Visible = true;
                    }
                }
                else
                {
                    //Response.Redirect("EntityMaster.aspx");
                    Response.Redirect("Default.aspx");
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
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdForum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdForum.PageIndex = e.NewPageIndex;
                BindForum();
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
        protected void grdForum_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdForum_RowEditing(object sender, GridViewEditEventArgs e)
        {

            try
            {
                UserAcess objUser = new UserAcess();
                //check edit permission
                if (objUser.IsEdit(Guid.Parse(ViewState["entityId"].ToString())))
                {
                    string strForumId = grdForum.DataKeys[e.NewEditIndex].Value.ToString();

                    ForumDomain objForumDomain = ForumDataProvider.Instance.Get(Guid.Parse(strForumId));
                    txtForumName.Text = objForumDomain.ForumName;
                    txtShortName.Text = objForumDomain.ForumShortName;
                    //ddlUser.SelectedValue = Convert.ToString(objForumDomain.ForumChecker);

                    if (ddlUser.Items.FindByValue(
Convert.ToString(objForumDomain.ForumChecker)) != null)
                    {
                        ddlUser.SelectedValue = objForumDomain.ForumChecker.ToString();
                    }
                    else
                    {
                        ddlUser.SelectedValue = "0";
                    }

                    hdnForumId.Value = Convert.ToString(objForumDomain.ForumId);

                    chkEnable.Checked = objForumDomain.IsEnable;
                    if (objForumDomain.MembersInfo != null)
                    {
                        if (objForumDomain.MembersInfo.Length > 0)
                        {
                            ViewState["Member"] = objForumDomain.MembersInfo;
                            lnkView.Visible = true;
                            lnkDelete.Visible = true;
                        }
                        else
                        {
                            lnkView.Visible = false;
                            lnkDelete.Visible = false;
                        }
                    }
                    ViewState["CreatedBy"] = objForumDomain.CreatedBy;

                    btnInsert.Text = "Update";
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

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdForum_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid entityId = Guid.Parse(ViewState["entityId"].ToString());
                DataTable dt = (DataTable)ForumDataProvider.Instance.GetForumByEntityId(entityId).AsDataTable();
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

                grdForum.DataSource = dv;
                grdForum.DataBind();
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
        /// Delelte Seleted Forum
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void lbRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                //check Delete permission
                if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                {
                    // StringBuilder strQuery = new StringBuilder("");
                    StringBuilder strForumIds = new StringBuilder(",");
                    int count = 0;
                    for (int i = 0; i < grdForum.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdForum.Rows[i].FindControl("chkSubAdmin");
                        if (chkSelect.Checked)
                        {
                            count++;
                            string ForumID = Convert.ToString(grdForum.DataKeys[i].Value.ToString());
                            //Delete all selected forum
                            // strQuery.Append("	DELETE FROM [Forum]   WHERE  ForumId = '" + ForumID + "'  ");
                            strForumIds.Append(ForumID + ",");
                        }
                    }
                    if (count > 0)
                    {
                        bool bStatus = ForumDataProvider.Instance.DeleteSelectedForum(strForumIds.ToString());
                        if (!bStatus)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Forum deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Master", "Success", strForumIds.ToString(), "Forum deleted successfully");

                            BindForum();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Master", "Failed", strForumIds.ToString(), "Forum deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Master", "Failed", strForumIds.ToString(), "Please select at least one checkbox");

                        ((Label)Error.FindControl("lblError")).Text = "Please select at least one checkbox";
                        Error.Visible = true;
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Master", "Failed", "", "Sorry access denied");

                    ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
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
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdForum_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //delete forum
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    UserAcess objUser = new UserAcess();
                    //check Delete permission
                    if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                    {
                        bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                        if (isMaker)
                        {
                            string strForumId = Convert.ToString(e.CommandArgument.ToString());
                            string[] ids = strForumId.Split(',');
                            bool bStatus = ForumDataProvider.Instance.Delete(Guid.Parse(ids[0]));
                            if (bStatus)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Forum deleted successfully";
                                Info.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Master", "Success", Convert.ToString(Guid.Parse(ids[0])), "Forum deleted successfully");


                                if (ids[1] != null && ids[1].Length > 0)
                                {
                                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Member"]);
                                    //Delete existing image                                               
                                    //    File.Delete(Server.MapPath(savePath + ids[1]));
                                }

                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                Error.Visible = true;
                            }
                            BindForum();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }

                }

                //view forum
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
        /// Insert or edit values 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (txtForumName.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtForumName);
                    if (!objUser.isValidChar(txtForumName.Text))
                    {
                        txtForumName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtForumName.Text))
                    {
                        txtForumName.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter forum name";
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
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    //txtShortName.Text = txtShortName.Text.EncodeHtml();
                }



                if (txtForumName.Text != null)//&& ddlUser.SelectedValue != "0"
                {
                    ForumDomain objForumDomain = new ForumDomain();
                    objForumDomain.EntityId = Guid.Parse(ViewState["entityId"].ToString());
                    objForumDomain.ForumName = txtForumName.Text;
                    objForumDomain.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    objForumDomain.IsEnable = chkEnable.Checked;

                    Guid CheckerId;
                    if (Guid.TryParse(ddlUser.SelectedValue, out CheckerId))
                    {
                        objForumDomain.ForumChecker = CheckerId;
                    }

                    objForumDomain.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                    if (objForumDomain.CreatedBy == objForumDomain.ForumChecker)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                        Error.Visible = true;
                        return;
                    }
                    string SendToChecker;
                    if (chkChecker.Checked)
                    {
                        SendToChecker = "Pending";
                        objForumDomain.IsEnable = false;
                    }
                    else
                    {
                        SendToChecker = "Approved";
                        objForumDomain.IsEnable = true;
                    }
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {

                        objForumDomain.ForumShortName = txtShortName.Text;
                        //Update Forum
                        if (hdnForumId.Value != null && hdnForumId.Value != "")
                        {
                            //Guid CreatedBy = Guid.Parse(ViewState["CreatedBy"].ToString());

                            //if (CreatedBy == objForumDomain.ForumChecker)
                            //{
                            //    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                            //    Error.Visible = true;
                            //    return;
                            //}

                            if (fuMemberInfo.HasFile)
                            {
                                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MembesInfo"]);

                                string ext = Path.GetExtension(fuMemberInfo.FileName);
                                if (ext.ToLower().Equals(".pdf"))
                                {
                                    string MembersInfo = DateTime.Now.Ticks + ext;
                                    fuMemberInfo.PostedFile.SaveAs(Server.MapPath(savePath + MembersInfo));
                                    objForumDomain.MembersInfo = MembersInfo;
                                    //Delete existing image
                                    if (ViewState["Member"] != null)
                                    {
                                        string MemberFile = Convert.ToString(ViewState["Member"]);
                                        File.Delete(Server.MapPath(savePath + MemberFile));
                                        ViewState["Member"] = null;
                                    }
                                }
                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Member information must be pdf";
                                    Error.Visible = true;
                                    return;
                                }
                            }


                            if (ViewState["Member"] != null)
                            {
                                objForumDomain.MembersInfo = Convert.ToString(ViewState["Member"]);
                            }

                            objForumDomain.ForumId = Guid.Parse(hdnForumId.Value);
                            Guid status = ForumDataProvider.Instance.Update(objForumDomain, SendToChecker);
                            if (!status.ToString().Equals("11000000-0000-0100-0100-000000000011"))
                            {
                                if (SendToChecker == "Pending")
                                {
                                    if (chkNotifyChecker.Checked)
                                    {
                                        bool isSuccess;
                                        string Message;
                                        SendEmail objSendEmail = new SendEmail();
                                        objSendEmail.SendNotifyEmail("Forum", objForumDomain.ForumChecker, out isSuccess, out Message);

                                        if (isSuccess)
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = " Forum updated successfully ant send for approval with email notification";
                                            Info.Visible = true;
                                        }
                                        else
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = " Forum updated successfully ant send for approval";
                                            ((Label)Error.FindControl("lblError")).Text = Message;
                                            Error.Visible = true;
                                            Info.Visible = true;

                                        }
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = " Forum updated successfully ant send for approval";
                                        Info.Visible = true;
                                    }

                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objForumDomain);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Pending", Convert.ToString(objForumDomain.ForumId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                    //  ((Label)Info.FindControl("lblName")).Text = " Forum updated successfully ant send for approval";
                                }
                                else
                                {
                                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objForumDomain);
                                    var encrypt = Encryptor.EncryptString(json);

                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Success", Convert.ToString(objForumDomain.ForumId), "Forum updated successfully :- " + encrypt + "");

                                    ((Label)Info.FindControl("lblName")).Text = " Forum updated successfully";
                                }
                                Info.Visible = true;
                                BindForum();
                                ClearData();
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Failed", Convert.ToString(objForumDomain.ForumId), "Forum update failed, Forum already exists");

                                ((Label)Error.FindControl("lblError")).Text = "Forum update failed, Forum already exists";
                                Error.Visible = true;
                                return;
                            }
                        }
                        //Insert Forum
                        else
                        {


                            //check add permission
                            if (objUser.IsAdd(Guid.Parse(ViewState["entityId"].ToString())))
                            {
                                if (fuMemberInfo.HasFile)
                                {


                                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MembesInfo"]);

                                    string ext = Path.GetExtension(fuMemberInfo.FileName);
                                    if (ext.ToLower().Equals(".pdf"))
                                    {
                                        string MembersInfo = DateTime.Now.Ticks + ext;
                                        fuMemberInfo.PostedFile.SaveAs(Server.MapPath(savePath + MembersInfo));
                                        objForumDomain.MembersInfo = MembersInfo;
                                        //Delete existing image
                                        if (ViewState["Member"] != null)
                                        {
                                            string MemberFile = Convert.ToString(ViewState["Member"]);
                                            File.Delete(Server.MapPath(savePath + MemberFile));
                                            ViewState["Member"] = null;
                                        }
                                    }
                                    else
                                    {
                                        ((Label)Error.FindControl("lblError")).Text = "Member information file must be pdf";
                                        Error.Visible = true;
                                        return;
                                    }
                                }

                                objForumDomain = ForumDataProvider.Instance.Insert(objForumDomain, SendToChecker);

                                if (objForumDomain.ForumId.ToString().Equals("11000000-0000-0100-0100-000000000011"))
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Failed", Convert.ToString(objForumDomain.ForumId), "Forum already exists");

                                    ((Label)Error.FindControl("lblError")).Text = " Forum already exists ";
                                    Error.Visible = true;
                                    return;
                                }
                                if (!objForumDomain.ForumId.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                                {
                                    if (SendToChecker == "Pending")
                                    {
                                        if (chkNotifyChecker.Checked)
                                        {
                                            bool isSuccess;
                                            string Message;
                                            SendEmail objSendEmail = new SendEmail();
                                            objSendEmail.SendNotifyEmail("Forum", objForumDomain.ForumChecker, out isSuccess, out Message);

                                            if (isSuccess)
                                            {
                                                ((Label)Info.FindControl("lblName")).Text = " Forum inserted successfully and sent for approval with email notification.";// and it will available in list after assigned by administrator ";
                                                Info.Visible = true;
                                            }
                                            else
                                            {
                                                ((Label)Error.FindControl("lblError")).Text = Message;
                                                ((Label)Info.FindControl("lblName")).Text = " Forum inserted successfully and sent for approval.";// and it will available in list after assigned by administrator. ";
                                                Info.Visible = true;
                                                Error.Visible = true;
                                            }
                                        }
                                        else
                                        {
                                            ((Label)Info.FindControl("lblName")).Text = " Forum inserted successfully and sent for approval.";// and it will available in list after assigned by administrator. ";
                                            Info.Visible = true;
                                        }

                                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objForumDomain);
                                        var encrypt = Encryptor.EncryptString(json);

                                        //web logs
                                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Pending", Convert.ToString(objForumDomain.ForumId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                        // ((Label)Info.FindControl("lblName")).Text = " Forum inserted successfully and sent for approval and it will available in list after assigned by administrator ";
                                    }
                                    else
                                    {
                                        ((Label)Info.FindControl("lblName")).Text = " Forum inserted successfully.";// It will be visible in the listing after it is assigned by administrator.";

                                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objForumDomain);
                                        var encrypt = Encryptor.EncryptString(json);

                                        //web logs
                                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Success", Convert.ToString(objForumDomain.ForumId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                                    }
                                    Info.Visible = true;
                                    BindForum();
                                    ClearData();
                                }
                                else
                                {
                                    //web logs
                                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Failed", Convert.ToString(objForumDomain.ForumId), "Forum insertion failed");

                                    ((Label)Error.FindControl("lblError")).Text = " Forum insertion failed";
                                    Error.Visible = true;
                                }
                            }
                            else
                            {
                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Failed", Convert.ToString(objForumDomain.ForumId), "Sorry access denied");

                                ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                                Error.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objForumDomain.UpdatedBy), "Forum Master", "Failed", Convert.ToString(objForumDomain.ForumId), "Sorry you are not maker");

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
        public void ClearData()
        {
            try
            {
                txtForumName.Text = "";
                hdnForumId.Value = "";
                txtShortName.Text = "";
                ddlUser.SelectedValue = "0";

                chkEnable.Checked = true;

                ViewState["Member"] = null;
                lnkView.Visible = false;
                lnkDelete.Visible = false;
                chkChecker.Checked = false;
                btnInsert.Text = "Save";

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    divEntity.Attributes.Add("style", "display:block;");
                    rfvddUser.Enabled = true;
                    chkChecker.Checked = true;
                    chkChecker.Enabled = false;
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
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = Convert.ToString(ViewState["Member"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MembesInfo"]);

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
        /// Delete uploaded file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkDelete_Click(object sender, EventArgs e)
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
                        ForumDomain objForumDomain = ForumDataProvider.Instance.Get(Guid.Parse(hdnForumId.Value));
                        objForumDomain.MembersInfo = "";
                        try
                        {
                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MembesInfo"]);
                            string MemberFile = Convert.ToString(ViewState["Member"]);
                            File.Delete(Server.MapPath(savePath + MemberFile));

                            ViewState["file"] = null;
                        }
                        catch (Exception ex)
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;

                            LogError objEr = new LogError();
                            objEr.HandleException(ex);
                        }

                        Guid status = ForumDataProvider.Instance.Update(objForumDomain, "Approved");
                        if (!status.ToString().Equals("11000000-0000-0100-0100-000000000011"))
                        {
                            ((Label)Info.FindControl("lblName")).Text = " File deleted successfully";

                            Info.Visible = true;
                            BindForum();
                            ClearData();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "File deletion failed";
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
            }
        }
    }
}