using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MM.Domain;
using MM.Data;
using System.IO;
using MM.Core;
using System.Text;

namespace MeetingMinder.Web
{
    public partial class AccessRightAdmin : System.Web.UI.Page
    {
        /// <summary>
        /// page Load event 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;

            trBtn.Visible = false;
            trGrid.Visible = false;

            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //Bind users to drop down
                BindUser();


                //Bind Rolls
                BindRoll();

                //bind access rigths to grid
                BindAccessRight();             

                if (Request.QueryString["id"] != null)
                {
                    string UserId = Convert.ToString(Request.QueryString["id"]);
                    //EditAccessRight(UserId);
                }
            }
        }

        /// <summary>
        /// Bind Rolls to drop down
        /// </summary>
        private void BindRoll()
        {
            try
            {
                ddlRoll.DataSource = RollDataProvider.Instance.Get().OrderBy(p => p.RollName).ToList();
                ddlRoll.DataBind();
                ddlRoll.DataTextField = "RollName";
                ddlRoll.DataValueField = "RollMasterId";
                ddlRoll.DataBind();
                ddlRoll.Items.Insert(0, new ListItem("Select Role", "0"));
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
        /// Bind All Access Rights
        /// </summary>
        private void BindAccessRight()
        {
            try
            {
                //  Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                //UserDataProvider.Instance.GetAllUserByAccessRightByEntity(EntityId);
                grdAccessRightUser.DataSource = UserDataProvider.Instance.GetAllUserByAccessRight().OrderBy(p => p.FirstName).ToList(); //AccessRightDataProvider.Instance.Get();
                grdAccessRightUser.DataBind();
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
        /// Event That Fired on RowDataBound
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRight_RowDataBound(object sender, GridViewRowEventArgs e)
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


                    IList<AccessRightDomain> objAccess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(hdnUserId.Value));

                    Guid User = Guid.Parse(hdnUserId.Value);
                    IList<UserEntityDomain> objUser = UserEntityDataProvider.Instance.GetEntityListByUserIdForDepartment(User).OrderBy(p => p.EntityName).ToList();
                    if (objUser.Count > 0)
                    {
                        GvChild.DataSource = objUser;
                        GvChild.DataBind();
                    }
                    else
                    {
                        GvChild.DataSource = null;
                        GvChild.DataBind();
                    }


                    List<string> selectedEntity = new List<string>();
                    //Get all entity in database
                    if (objAccess.Count > 0)
                    {
                        var selectedEntty = (from p in objAccess
                                             select new
                                             {
                                                 EntityId = p.EntityId
                                             }).ToList();

                        foreach (var id in selectedEntty)
                        {
                            selectedEntity.Add(id.EntityId.ToString());
                        }

                    }

                    for (int i = 0; i < GvChild.Rows.Count; i++)
                    {

                        HiddenField EntityId = (HiddenField)GvChild.Rows[i].FindControl("hdnEntityId");

                        var objRights = (from p in objAccess
                                         where (p.EntityId == Guid.Parse(EntityId.Value))
                                         select p).ToList();
                        if (objRights.Count > 0)
                        {
                            CheckBox chkIsAdd = (CheckBox)GvChild.Rows[i].FindControl("chkIsAdd");

                            CheckBox chkIsRead = (CheckBox)GvChild.Rows[i].FindControl("chkIsRead");
                            CheckBox chkIsUpdate = (CheckBox)GvChild.Rows[i].FindControl("chkIsUpdate");
                            CheckBox chkIsDelete = (CheckBox)GvChild.Rows[i].FindControl("chkIsDelete");

                            chkIsAdd.Checked = objRights[0].IsAdd;
                            chkIsDelete.Checked = objRights[0].IsDelete;
                            chkIsRead.Checked = objRights[0].IsRead;
                            chkIsUpdate.Checked = objRights[0].IsUpdate;

                            //  selectedEntity.Add(EntityId.Value);
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
        /// Delelte Seleted Forum
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
                    StringBuilder strUserIds = new StringBuilder(",");
                    int count = 0;
                    for (int i = 0; i < grdAccessRightUser.Rows.Count; i++)
                    {
                        CheckBox chkSelect = (CheckBox)grdAccessRightUser.Rows[i].FindControl("chkSubAdmin");
                        //Get all checked users 
                        if (chkSelect.Checked)
                        {
                            count++;
                            string UserID = Convert.ToString(grdAccessRightUser.DataKeys[i].Value.ToString());
                            //Delete all selected forum
                            strUserIds.Append(UserID + ",");

                        }
                    }
                    if (count > 0)
                    {
                        bool bStatus = AccessRightDataProvider.Instance.DeleteSelectedAccessRightByUserId(strUserIds.ToString());
                        if (!bStatus)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Access rights deleted successfully";
                            Info.Visible = true;
                            BindAccessRight();
                            BindUser();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please Select at least one checkbox";
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

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Bind users to drop down
        /// </summary>
        private void BindUser()
        {
            try
            {
                Literal ltl_bredcrumbs = (Literal)Master.FindControl("ltl_bredcrumbs");
                ltl_bredcrumbs.Text = "<a href='" + VirtualPathUtility.ToAbsolute("~/default.aspx") + "' >Home</a>&nbsp;";
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName+' ('+Username+')'";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllUserForAccessRight().Where(p => p.IsEnabledOnWebApp).OrderBy(p => p.FirstName).ToList();
                //Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                //IList<UserDomain> objUser = UserDataProvider.Instance.GetAllUserForAccessRightByEntity(EntityId);
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
        /// Event That Fired on SelectedIndexChanged of drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //System.Threading.Thread.Sleep(5000);
                string userId = Convert.ToString(ddlUser.SelectedValue);
                if (!userId.Equals("0"))
                {
                    BindUserEntity(userId);
                    trBtn.Visible = true;
                    trGrid.Visible = true;
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
        /// Bind Entity of user to grid
        /// </summary>
        /// <param name="userId"></param>
        private void BindUserEntity(string userId)
        {
            try
            {
                Guid User = Guid.Parse(userId);
                IList<UserEntityDomain> objUser = UserEntityDataProvider.Instance.GetEntityListByUserIdForDepartment(User);
                if (objUser.Count > 0)
                {
                    grdAccessRight.DataSource = objUser;
                    grdAccessRight.DataBind();
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
        /// Button submit event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                Guid updateBy = Guid.Parse(Session["UserId"].ToString());
                Guid createdBy = Guid.Parse(Session["UserId"].ToString());

                if (isMaker)
                {
                    int count = 0;
                    Guid RollId;
                    if (ddlRoll.SelectedValue != "0")
                    {
                        RollId = Guid.Parse(ddlRoll.SelectedValue);
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please select roll";
                        Error.Visible = true;
                        return;
                    }

                    StringBuilder strUser = new StringBuilder(",");
                    StringBuilder strAccessRights = new StringBuilder(",");

                    StringBuilder strInsertUser = new StringBuilder(",");
                    StringBuilder strInsertAccessRights = new StringBuilder(",");

                    StringBuilder strDeleteUser = new StringBuilder(",");
                    string UserId = "";
                    List<string> selectedEntity = null;
                    bool edit = false; //editing in form
                    if (ViewState["edit"] != null)
                    {
                        edit = true;
                    }

                    if (ViewState["selectedEntity"] != null)
                    {
                        selectedEntity = (List<string>)ViewState["selectedEntity"];
                    }
                    //get all checked user Entity           
                    for (int i = 0; i < grdAccessRight.Rows.Count; i++)
                    {
                        CheckBox chkIsAdd = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsAdd");

                        CheckBox chkIsRead = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsRead");
                        CheckBox chkIsUpdate = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsUpdate");
                        CheckBox chkIsDelete = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsDelete");

                        HiddenField EntityId = (HiddenField)grdAccessRight.Rows[i].FindControl("hdnEntityId");
                        count++;

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
                            if (selectedEntity.Contains(EntityId.Value))
                            {
                                //entity already exists in database will be updated
                                strUser.Append(Convert.ToString(grdAccessRight.DataKeys[i].Value.ToString()) + ",");
                                strAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + EntityId.Value + ",");
                                selectedEntity.Remove(EntityId.Value);
                            }
                            else
                            {
                                //entity not exists in database will be inserted
                                strInsertUser.Append(Convert.ToString(grdAccessRight.DataKeys[i].Value.ToString()) + ",");
                                strInsertAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + EntityId.Value + ",");
                            }
                        }
                        else
                        {

                            //is user checked any check box
                            //if (isRead || isAdd || isUpdate || isDelete)
                            //{

                            strUser.Append(Convert.ToString(grdAccessRight.DataKeys[i].Value.ToString()) + ",");
                            strAccessRights.Append(isRead + "," + isAdd + "," + isUpdate + "," + isDelete + "," + EntityId.Value + ",");
                            //}
                        }
                    }

                    if (count == 0)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please select at least one entity";
                        Error.Visible = true;
                    }
                    else
                    {
                        if (edit) //Update
                        {
                            if (selectedEntity.Count != 0)
                            {
                                foreach (var item in selectedEntity)
                                {
                                    //Delete previous not selected entity
                                    strDeleteUser.Append(UserId + "," + item.ToString() + ",");
                                }
                            }

                            bool isSave = AccessRightDataProvider.Instance.UpdateAccessRight(strUser.ToString(), strAccessRights.ToString(), createdBy, updateBy, RollId, strInsertUser.ToString(), strInsertAccessRights.ToString(), strDeleteUser.ToString());
                            ((Label)Info.FindControl("lblName")).Text = "Access right updated sucessfully ";

                            var encrypt = Encryptor.EncryptString("User :- " + strUser.ToString() + ",AccessRight :- " + strAccessRights.ToString() + ",CreatedBy :- " + createdBy + ",UpdatedBy :- " + updateBy + ",RollId :- " + RollId + ",InsertUser :- " + strInsertUser.ToString() + ",InsertUserAccessRight :- " + strInsertAccessRights.ToString() + ",DeleteUser :- " + strDeleteUser.ToString() + "");

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(createdBy), "Access Right", "Success", Convert.ToString(RollId), "Access right updated sucessfully :- " + encrypt + "");

                            Info.Visible = true;
                            ViewState["edit"] = null;
                            ddlRoll.SelectedValue = "0";
                            txtSearch.Text = string.Empty;
                            btnReset.Visible = false;
                            BindUser();
                        }
                        else //Insert
                        {
                            bool isSave = AccessRightDataProvider.Instance.InserAccessRight(strUser.ToString(), strAccessRights.ToString(), createdBy, updateBy, RollId);

                            var encrypt = Encryptor.EncryptString("User :- " + strUser.ToString() + ",AccessRight :- " + strAccessRights.ToString() + ",CreatedBy :- " + createdBy + ",UpdatedBy :- " + updateBy + ",RollId :- " + RollId + "");

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(createdBy), "Access Right", "Success", Convert.ToString(RollId), "Access right saved sucessfully :- " + encrypt + "");

                            ((Label)Info.FindControl("lblName")).Text = "Access right saved sucessfully ";
                            ddlRoll.SelectedValue = "0";
                            Info.Visible = true;
                        }
                        trDdl.Visible = true;
                        trName.Visible = false;
                        BindAccessRight();
                        trBtn.Visible = false;
                        trGrid.Visible = false;
                        txtSearch.Text = string.Empty;
                        btnReset.Visible = false;
                        BindUser();
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(createdBy), "Access Right", "Failed", "", "Sorry you are not maker");

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
        /// Button cancel event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("AccessRightAdmin.aspx");
        }

        /// <summary>
        /// Edit Access rights 
        /// </summary>
        /// <param name="UserId">string specifying UserId</param>
        public void EditAccessRight(string UserId, string FullName)
        {
            try
            {
                IList<AccessRightDomain> objAccess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(UserId));


                var Roll = (from roll in objAccess
                            select roll.RollId.ToString()).ToList();

                if (ddlRoll.Items.FindByValue(
     Convert.ToString(Roll[0])) != null)
                {
                    ddlRoll.SelectedValue = Convert.ToString(Roll[0]);
                }
                else
                {
                    ddlRoll.SelectedIndex = -1;// = Roll[0].ToString();
                }
                BindUserEntity(UserId);
                trDdl.Visible = false;
                ViewState["edit"] = true;
                //check user Entity
                trBtn.Visible = true;
                trGrid.Visible = true;

                trName.Visible = true;
                lblFullName.Text = FullName;
                List<string> selectedEntity = new List<string>();
                //Get all entity in database
                if (objAccess.Count > 0)
                {
                    var selectedEntty = (from p in objAccess
                                         select new
                                         {
                                             EntityId = p.EntityId
                                         }).ToList();

                    foreach (var id in selectedEntty)
                    {
                        selectedEntity.Add(id.EntityId.ToString());
                    }

                }

                for (int i = 0; i < grdAccessRight.Rows.Count; i++)
                {

                    HiddenField EntityId = (HiddenField)grdAccessRight.Rows[i].FindControl("hdnEntityId");

                    var objRights = (from p in objAccess
                                     where (p.EntityId == Guid.Parse(EntityId.Value))
                                     select p).ToList();
                    if (objRights.Count > 0)
                    {
                        CheckBox chkIsAdd = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsAdd");

                        CheckBox chkIsRead = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsRead");
                        CheckBox chkIsUpdate = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsUpdate");
                        CheckBox chkIsDelete = (CheckBox)grdAccessRight.Rows[i].FindControl("chkIsDelete");

                        chkIsAdd.Checked = objRights[0].IsAdd;
                        chkIsDelete.Checked = objRights[0].IsDelete;
                        chkIsRead.Checked = objRights[0].IsRead;
                        chkIsUpdate.Checked = objRights[0].IsUpdate;

                        //  selectedEntity.Add(EntityId.Value);
                    }
                }
                ViewState["selectedEntity"] = selectedEntity;
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
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRight_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //edit
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    string UserId = Convert.ToString(e.CommandArgument);

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);


                    // GridViewRow row = (GridViewRow)lb.NamingContainer;
                    if (row != null)
                    {
                        int index = row.RowIndex; //gets the row index selected
                        Label lblName = (Label)row.FindControl("lblUserName");
                        EditAccessRight(UserId, lblName.Text);
                    }
                }

                //delete
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        string UserID = Convert.ToString(e.CommandArgument.ToString());
                        bool bStatus = AccessRightDataProvider.Instance.DeleteAccessRightByuserId(Guid.Parse(UserID));
                        if (bStatus == false)
                        {
                            ((Label)Info.FindControl("lblName")).Text = "Access right deleted successfully";
                            Info.Visible = true;

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Access Right", "Success", UserID, "Access right deleted successfully");

                            BindAccessRight();
                            BindUser();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Access Right", "Failed", UserID, "Access right deletion failed");

                            ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Access Right", "Failed", "", "Sorry you are not maker");

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
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRight_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Event That Fired on RowEditCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRight_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        ///  Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRight_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)UserDataProvider.Instance.GetAllUserByAccessRight().AsDataTable();
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

                grdAccessRightUser.DataSource = dv;
                grdAccessRightUser.DataBind();

                trName.Visible = false;
                trDdl.Visible = true;
                ViewState["edit"] = null;
                ddlUser.SelectedIndex = -1;
                ddlRoll.SelectedIndex = -1;
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
        ///  Function Used For Gridview Paging
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewRowEventArgs specifying e </param>
        protected void grdAccessRightUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAccessRightUser.PageIndex = e.NewPageIndex;
                BindAccessRight();

                trName.Visible = false;
                trDdl.Visible = true;
                ViewState["edit"] = null;
                ddlUser.SelectedIndex = -1;
                ddlRoll.SelectedIndex = -1;
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

                grdAccessRightUser.DataSource = UserDataProvider.Instance.GetAllUserByAccessRight().Where(li => li.UserName.ToLower().Contains(strSearch) || li.FirstName.ToLower().Contains(strSearch) || li.LastName.ToLower().Contains(strSearch)).OrderBy(p => p.FirstName).ToList(); //AccessRightDataProvider.Instance.Get();
                grdAccessRightUser.DataBind();

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

            //bind user list  to grid
            BindAccessRight();
            
            Response.StatusCode = 302;
            Response.AddHeader("Location", "AccessRightAdmin.aspx");
        }
    }
}