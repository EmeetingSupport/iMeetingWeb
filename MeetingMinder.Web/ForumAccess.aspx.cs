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


namespace MeetingMinder.Web
{
    public partial class ForumAccess : System.Web.UI.Page
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
            // Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //Bind users to dropdown
                BindUsers();

                //Bind Forum access
                BindForumAccess();
            }
        }
        /// <summary>
        /// Bind forum Access details to gridview
        /// </summary>
        private void BindForumAccess()
        {
            //string UserId = Convert.ToString(Session["UserId"]);
            try
            {
                grdUserForum.DataSource = UserDataProvider.Instance.GetAllUserByForumAccess().OrderBy(p => p.FirstName).ToList(); //AccessRightDataProvider.Instance.Get();
                grdUserForum.DataBind();
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
        private void BindUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName+' ('+Username+')'";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllUserFoUserForum().OrderBy(p => p.UserName).ToList();
                if (objUser.Count > 0)
                {
                    //Bind drop down list
                    //ddlUser.DataSource = objUser;
                    //ddlUser.DataBind();
                    //ddlUser.DataTextField = "UserName";
                    //ddlUser.DataValueField = "UserId";
                    //ddlUser.DataBind();
                    //ddlUser.Items.Insert(0, new ListItem("Select User", "0"));

                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                    ddlUser.Items.Insert(0, new ListItem("Select User", "0"));

                }
                else
                {
                    ddlUser.Items.Clear();
                    ddlUser.Items.Insert(0, new ListItem("No new user added", "0"));
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
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    int count = 0;
                    Guid updateBy = Guid.Parse(Session["UserId"].ToString());
                    Guid createdBy = Guid.Parse(Session["UserId"].ToString());
                    StringBuilder strUser = new StringBuilder(",");
                    StringBuilder strAccessRights = new StringBuilder(",");

                    StringBuilder strInsertUser = new StringBuilder(",");
                    StringBuilder strInsertAccessRights = new StringBuilder(",");

                    StringBuilder strDeleteUser = new StringBuilder(",");
                    string UserId = "";
                    List<string> selectedForum = null;
                    bool edit = false; //editing in form
                    if (ViewState["edit"] != null)
                    {
                        edit = true;
                    }

                    if (ViewState["selectedForum"] != null)
                    {
                        selectedForum = (List<string>)ViewState["selectedForum"];
                    }

                    for (int i = 0; i < grdAccessRight.Rows.Count; i++)
                    {
                        CheckBox chkIsAdd = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsAdd");

                        CheckBox chkIsRead = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsRead");
                        CheckBox chkIsUpdate = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsUpdate");
                        CheckBox chkIsDelete = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsDelete");

                        HiddenField ForumId = (HiddenField)grdAccessRight.Rows[i].FindControl("hdnForumId");
                        CheckBox chkIsForum = (CheckBox)grdAccessRight.Rows[i].FindControl("chkForum");
                        count++;

                        if (chkIsForum.Checked)
                        {
                            //bool isRead = chkIsRead.Checked;

                            //bool isAdd = chkIsAdd.Checked;
                            //bool isUpdate = chkIsUpdate.Checked;
                            //bool isDelete = chkIsDelete.Checked;
                            int isRead = Convert.ToInt16(chkIsRead.Checked);

                            int isAdd = Convert.ToInt16(chkIsAdd.Checked);
                            int isUpdate = Convert.ToInt16(chkIsUpdate.Checked);
                            int isDelete = Convert.ToInt16(chkIsDelete.Checked);


                            if (edit)
                            {
                                UserId = grdAccessRight.DataKeys[i].Value.ToString();
                                if (selectedForum.Contains(ForumId.Value))
                                {
                                    //entity already exists in database will be updated
                                    strUser.Append(grdAccessRight.DataKeys[i].Value.ToString() + ",");
                                    strAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + ForumId.Value + ",");
                                    selectedForum.Remove(ForumId.Value);
                                }
                                else
                                {
                                    //entity not exists in database will be inserted
                                    strInsertUser.Append(grdAccessRight.DataKeys[i].Value.ToString() + ",");
                                    strInsertAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + ForumId.Value + ",");
                                }
                            }
                            else
                            {

                                //is user checked any check box
                                //if (isRead || isAdd || isUpdate || isDelete)
                                //{

                                strUser.Append(grdAccessRight.DataKeys[i].Value.ToString() + ",");
                                strAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + ForumId.Value + ",");
                                //}
                            }
                        }
                    }

                    if (count == 0)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please select at least one Entity";
                        Error.Visible = true;
                    }
                    else
                    {
                        if (edit) //Update
                        {
                            if (selectedForum.Count != 0)
                            {
                                foreach (var item in selectedForum)
                                {
                                    //Delete previous not selected entity
                                    strDeleteUser.Append(UserId + "," + item.ToString() + ",");
                                }
                            }

                            bool isSave = UserForumDataprovider.Instance.UpdateAll(strUser.ToString(), strAccessRights.ToString(), createdBy, updateBy, strInsertUser.ToString(), strInsertAccessRights.ToString(), strDeleteUser.ToString());
                            ((Label)Info.FindControl("lblName")).Text = "Access right updated sucessfully ";

                            var encrypt = Encryptor.EncryptString("User :- " + strUser.ToString() + ",AccessRight :- " + strAccessRights.ToString() + ",CreatedBy :- " + createdBy + ",UpdatedBy :- " + updateBy + ",InsertUser :- " + strInsertUser.ToString() + ",InsertUserAccessRight :- " + strInsertAccessRights.ToString() + ",DeleteUser :- " + strDeleteUser.ToString() + "");

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(createdBy), "Forum Access", "Success", Convert.ToString(strAccessRights.ToString()), "Access right updated sucessfully :- " + encrypt + "");

                            Info.Visible = true;
                            ViewState["edit"] = null;
                            BindUsers();
                            BindForumAccess();
                            trGrid.Visible = false;
                            grdAccessRight.DataSource = null;
                            grdAccessRight.DataBind();

                            trDdl.Visible = true;
                            trGrid.Visible = true;

                            trName.Visible = false;

                            grdAccessRight.DataSource = null;
                            grdAccessRight.DataBind();
                            ddlUser.SelectedIndex = -1;
                            txtSearch.Text = string.Empty;
                            btnReset.Visible = false;
                        }
                        else //Insert
                        {
                            bool isSave = UserForumDataprovider.Instance.InsertAll(strUser.ToString(), strAccessRights.ToString(), createdBy, updateBy);
                            ((Label)Info.FindControl("lblName")).Text = "Access right saved sucessfully ";

                            var encrypt = Encryptor.EncryptString("User :- " + strUser.ToString() + ",AccessRight :- " + strAccessRights.ToString() + ",CreatedBy :- " + createdBy + ",UpdatedBy :- " + updateBy + "");

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(createdBy), "Forum Access", "Success", Convert.ToString(strAccessRights.ToString()), "Access right saved sucessfully :- " + encrypt + "");

                            Info.Visible = true;
                            BindForumAccess();
                            BindUsers();
                            trGrid.Visible = false;
                            grdAccessRight.DataSource = null;
                            grdAccessRight.DataBind();

                            trName.Visible = false;

                            trDdl.Visible = true;
                            trGrid.Visible = true;
                            grdAccessRight.DataSource = null;
                            grdAccessRight.DataBind();
                            ddlUser.SelectedIndex = -1;
                            txtSearch.Text = string.Empty;
                            btnReset.Visible = false;
                        }
                    }
                }

                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Access", "Failed", "", "Sorry you are not maker");

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
        /// button cancel click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            trDdl.Visible = true;
            trGrid.Visible = true;

            trName.Visible = false;

            grdAccessRight.DataSource = null;
            grdAccessRight.DataBind();
            ddlUser.SelectedIndex = -1;
        }

        /// <summary>
        /// drop downlist selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strUserId = ddlUser.SelectedValue;
                if (!strUserId.Equals("0"))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetUserForums(Guid.Parse(strUserId)).OrderBy(p => p.ForumName).ToList();
                    grdAccessRight.DataSource = objForum;
                    grdAccessRight.DataBind();
                    trGrid.Visible = true;
                    trBtn.Visible = true;

                }
                else
                {
                    grdAccessRight.DataSource = null;
                    grdAccessRight.DataBind();
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
        /// Gridview page index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {

                trGrid.Visible = true;


                grdAccessRight.DataSource = null;
                grdAccessRight.DataBind();
                ddlUser.SelectedIndex = -1;

                grdUserForum.PageIndex = e.NewPageIndex;
                BindForumAccess();

                trName.Visible = false;
                trDdl.Visible = true;
                ViewState["edit"] = null;
                ddlUser.SelectedIndex = -1;
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
        /// Gridview row command event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                        return;
                    }
                    string strUserId = Convert.ToString(e.CommandArgument);

                    string UserId = Convert.ToString(e.CommandArgument);
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                    // GridViewRow row = (GridViewRow)lb.NamingContainer;
                    if (row != null)
                    {
                        int index = row.RowIndex; //gets the row index selected
                        Label lblName = (Label)row.FindControl("lblUserName");
                        //EditAccessRight(UserId, lblName.Text);

                        lblFullName.Text = lblName.Text;
                    }

                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetUserForums(Guid.Parse(strUserId)).OrderBy(p => p.ForumName).ToList();
                    grdAccessRight.DataSource = objForum;
                    grdAccessRight.DataBind();
                    //ddlUser.SelectedValue = strUserId;
                    trDdl.Visible = false;
                    trGrid.Visible = true;

                    trName.Visible = true;

                    IList<UserForumDomain> objUserForum = UserForumDataprovider.Instance.GetUserForumByUserId(Guid.Parse(strUserId));

                    List<string> selectedForum = new List<string>();
                    //Get all entity in database
                    if (objUserForum.Count > 0)
                    {
                        var selectedEntty = (from p in objUserForum
                                             select new
                                             {
                                                 ForumId = p.ForumId
                                             }).ToList();

                        foreach (var id in selectedEntty)
                        {
                            selectedForum.Add(id.ForumId.ToString());
                        }

                    }
                    ViewState["edit"] = true;
                    ViewState["selectedForum"] = selectedForum;

                    for (int i = 0; i < grdAccessRight.Rows.Count; i++)
                    {
                        HiddenField ForumId = (HiddenField)grdAccessRight.Rows[i].FindControl("hdnForumId");
                        Guid forId = Guid.Parse(ForumId.Value);
                        var objUserFor = (from p in objUserForum
                                          where p.ForumId == forId
                                          select p);
                        if (objUserFor.Count() > 0)
                        {
                            CheckBox chkIsForum = (CheckBox)grdAccessRight.Rows[i].FindControl("chkForum");
                            chkIsForum.Checked = true;
                        }

                    }
                }
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    trDdl.Visible = true;
                    trGrid.Visible = true;

                    trName.Visible = false;

                    grdAccessRight.DataSource = null;
                    grdAccessRight.DataBind();
                    ddlUser.SelectedIndex = -1;

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Access", "Failed", Convert.ToString(e.CommandArgument), "Sorry access denied");

                            return;
                        }
                        string strUserId = Convert.ToString(e.CommandArgument);
                        bool status = UserForumDataprovider.Instance.DeleteForumAccess(Guid.Parse(strUserId));
                        if (status == true)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Access right deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Forum Access", "Success", strUserId, "Access right deleted successfully");

                            BindForumAccess();
                            BindUsers();
                        }
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
        /// Gridview row databound event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdnUserId = (HiddenField)(e.Row.FindControl("hdnId"));

                    //Bind inner grid
                    GridView GvChild = (GridView)e.Row.FindControl("grdAccessRightN");

                    //GvChild.DataSource = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(hdnUserId.Value));
                    //GvChild.DataBind();


                    IList<UserForumDomain> objAccess = UserForumDataprovider.Instance.GetUserForumByUserId(Guid.Parse(hdnUserId.Value)).OrderBy(p => p.ForumName).ToList();

                    Guid User = Guid.Parse(hdnUserId.Value);
                    //IList<UserEntityDomain> objUser = UserEntityDataProvider.Instance.GetEntityListByUserId(User);
                    if (objAccess.Count > 0)
                    {
                        GvChild.DataSource = objAccess;
                        GvChild.DataBind();
                    }
                    else
                    {
                        GvChild.DataSource = null;
                        GvChild.DataBind();
                    }


                    //List<string> selectedEntity = new List<string>();
                    ////Get all entity in database
                    //if (objAccess.Count > 0)
                    //{
                    //    var selectedEntty = (from p in objAccess
                    //                         select new
                    //                         {
                    //                             EntityId = p.EntityId
                    //                         }).ToList();

                    //    foreach (var id in selectedEntty)
                    //    {
                    //        selectedEntity.Add(id.EntityId.ToString());
                    //    }

                    //}

                    //for (int i = 0; i < GvChild.Rows.Count; i++)
                    //{

                    //    HiddenField EntityId = (HiddenField)GvChild.Rows[i].FindControl("hdnEntityId");

                    //    var objRights = (from p in objAccess
                    //                     where (p.EntityId == Guid.Parse(EntityId.Value))
                    //                     select p).ToList();
                    //    if (objRights.Count > 0)
                    //    {
                    //        CheckBox chkIsAdd = (CheckBox)GvChild.Rows[i].FindControl("chkIsAdd");

                    //        CheckBox chkIsRead = (CheckBox)GvChild.Rows[i].FindControl("chkIsRead");
                    //        CheckBox chkIsUpdate = (CheckBox)GvChild.Rows[i].FindControl("chkIsUpdate");
                    //        CheckBox chkIsDelete = (CheckBox)GvChild.Rows[i].FindControl("chkIsDelete");

                    //        chkIsAdd.Checked = objRights[0].IsAdd;
                    //        chkIsDelete.Checked = objRights[0].IsDelete;
                    //        chkIsRead.Checked = objRights[0].IsRead;
                    //        chkIsUpdate.Checked = objRights[0].IsUpdate;

                    //        //  selectedEntity.Add(EntityId.Value);
                    //    }
                    //}
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
        /// Gridview row delete event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Gridview row editing event event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdUserForum_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = UserDataProvider.Instance.GetAllUserByForumAccess().AsDataTable();
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

                grdUserForum.DataSource = dv;
                grdUserForum.DataBind();

                trName.Visible = false;
                trDdl.Visible = true;
                ViewState["edit"] = null;
                ddlUser.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string strSearch = txtSearch.Text.Trim().ToLower();
                btnReset.Visible = true;
                                
                grdUserForum.DataSource = UserDataProvider.Instance.GetAllUserByForumAccess().Where(li => li.UserName.ToLower().Contains(strSearch) || li.FirstName.ToLower().Contains(strSearch) || li.LastName.ToLower().Contains(strSearch)).OrderBy(p => p.FirstName).ToList(); //AccessRightDataProvider.Instance.Get();
                grdUserForum.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            btnReset.Visible = false;
            txtSearch.Text = string.Empty;

            //bind forum user list  to grid
            BindForumAccess();

            Response.StatusCode = 302;
            Response.AddHeader("Location", "ForumAccess.aspx");
        }
    }
}