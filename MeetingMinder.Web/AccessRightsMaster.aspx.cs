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
    public partial class AccessRightsMaster : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
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
                //Bind users to drop down
                BindUser();

                //Bind Entity to List box
                BindEntity();

                //bind access rigths to drop down
                BindAccessRight();
            }
        }

        /// <summary>
        /// Bind All Access Rights
        /// </summary>
        private void BindAccessRight()
        {
            try
            {
                grdAccessRight.DataSource = AccessRightDataProvider.Instance.Get().OrderBy(p => p.UserName).ToList(); ;
                grdAccessRight.DataBind();
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
                lstEntity.DataSource = EntityDataProvider.Instance.Get().OrderBy(p => p.EntityName).ToList(); ;
                lstEntity.DataBind();
                lstEntity.DataTextField = "EntityName";
                lstEntity.DataValueField = "EntityId";
                lstEntity.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }

        /// <summary>
        /// Bind users to drop down list
        /// </summary>
        private void BindUser()
        {
            try
            {
                ddlUser.DataSource = UserDataProvider.Instance.Get().OrderBy(p => p.FirstName).ToList(); 
                ddlUser.DataBind();
                ddlUser.DataTextField = "UserName";
                ddlUser.DataValueField = "UserId";
                ddlUser.DataBind();
                ddlUser.Items.Insert(0, new ListItem("Select User", "0"));
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
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
                Guid userId = Guid.Parse(ddlUser.SelectedValue);
                Guid rollId = Guid.NewGuid();
                //bool isRead = chkIsRead.Checked;
                //bool isAdd = chkIsAdd.Checked;
                //bool isUpdate = chkIsUpdate.Checked;
                //bool isDelete = chkIsDelete.Checked;
                int isRead =Convert.ToInt16 (chkIsRead.Checked);
                int isAdd = Convert.ToInt16(chkIsAdd.Checked);
                int isUpdate = Convert.ToInt16 (chkIsUpdate.Checked);
                int isDelete = Convert.ToInt16( chkIsDelete.Checked);


                Guid updateBy = Guid.Parse(Session["UserId"].ToString());
                Guid createdBy = Guid.Parse(Session["UserId"].ToString());
                StringBuilder strQuery = new StringBuilder("");

                //is user checked any check box
                if (isRead==1 || isAdd==1 || isUpdate==1 || isDelete==1)
                {
                    foreach (ListItem lstItem in lstEntity.Items)
                    {
                        if (lstItem.Selected)
                        {
                            //Insert access rights for multiple entity
                            strQuery.Append(@"  INSERT INTO [AccessRight]([AccessRightId],[RollId],[UserId],[EntityId],[IsRead],[IsAdd],[IsUpdate],[IsDelete],[CreatedBy],[CreatedOn],[UpdatedBy],[UpdatedOn])  VALUES(newId(),'" + rollId + "','" + userId + "','" + lstItem.Value + "','" + isRead + "','" + isAdd + "', '" + isUpdate + "','" + isDelete + "','" + createdBy + "',getdate(),'" + updateBy + "',getdate())  ");
                        }
                    }
                    if (!strQuery.ToString().Equals(""))
                    {
                        bool isSave = AccessRightDataProvider.Instance.Insert(strQuery.ToString());
                        ((Label)Info.FindControl("lblName")).Text = "Access Right saved sucessfully ";
                        Info.Visible = true;
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Please select at least one Entity";
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
            }

        }

        /// <summary>
        /// Button cancel event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// To make Drop-Dwonlist item Clear
        /// </summary>
        public void clearData()
        {
            ddlUser.SelectedValue = "0";
            lstEntity.SelectedIndex = -1;
        }

        /// <summary>
        ///  Code to PageIndex Changing
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAccessRight_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        /// <summary>
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAccessRight_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //edit
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    string UserId = Convert.ToString(e.CommandArgument);

                    IList<AccessRightDomain> objAccess = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(UserId));
                    ddlUser.SelectedValue = UserId;
                    int itemIndex = 0;
                    List<string> selectedVal = new List<string>();

                    foreach (ListItem lstItem in lstEntity.Items)
                    {
                        if (lstItem.Value == objAccess[itemIndex].EntityId.ToString())
                        {
                            lstItem.Selected = true;
                            selectedVal.Add(lstItem.Value);
                            itemIndex++;
                        }
                    }
                }

                //delete
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    string AccessRightID = Convert.ToString(e.CommandArgument.ToString());
                    bool bStatus = AccessRightDataProvider.Instance.DeleteAccessRight(Guid.Parse(AccessRightID));
                    if (bStatus == false)
                    {
                        ((Label)Info.FindControl("lblName")).Text = "Access right deleted successfully";
                        Info.Visible = true;
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                        Error.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }

        }

        /// <summary>
        /// Event That Fired on RowCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAccessRight_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Event That Fired on RowEditCommand
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAccessRight_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        ///  Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        /// <param name="e"></param>
        protected void grdAccessRight_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        /// <summary>
        /// Gridview RowDataBoundEvent
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAccessRight_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField hdnUserId = (HiddenField)(e.Row.FindControl("hdnId"));

                    //   Label lbltoggel = (Label)e.Row.FindControl("lbltoggel");

                    DataGrid GvChild = (DataGrid)e.Row.FindControl("grdAccessRightN");

                    GvChild.DataSource = AccessRightDataProvider.Instance.GetAccessRightByUserId(Guid.Parse(hdnUserId.Value));
                    GvChild.DataBind();
                    //lbltoggel.Attributes.Add("onClick", "toggle('" + GvChild.ClientID + "' ,'" + lbltoggel.ClientID + "');");

                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
            }
        }
        /// <summary>
        /// MergeRows Event
        /// </summary>
        /// <param name="gridView">Gridview specifying gridView </param>
        public static void MergeRows(GridView gridView)
        {
            try
            {
                for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row = gridView.Rows[rowIndex];
                    GridViewRow previousRow = gridView.Rows[rowIndex + 1];


                    Label str = (Label)(previousRow.Cells[2].FindControl("lblUserName"));

                    Label sted = (Label)(row.Cells[2].FindControl("lblUserName"));

                    if (str.Text.Equals(sted.Text))
                    {
                        row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 : previousRow.Cells[2].RowSpan + 1;
                        previousRow.Cells[2].Visible = false;
                    }

                    //for (int i = 0; i < row.Cells.Count; i++)
                    //{
                    //    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    //    {
                    //        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 : previousRow.Cells[i].RowSpan + 1;
                    //        previousRow.Cells[i].Visible = false;
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Gridview preRender event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void grdAccessRight_PreRender(object sender, EventArgs e)
        {
            // MergeRows(grdAccessRight);
        }
    }
}