using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;
using MM.Domain;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class HeaderMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;

            divdetails.Visible = false;

            if (!Page.IsPostBack)
            {
                Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                // BindAgenda(EntityId);
                BindForum(EntityId.ToString());
            }
        }

        protected void grdHeader_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdHeader_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdHeader.PageIndex = e.NewPageIndex;
            string strForumId = ddlForum.SelectedValue;
            if (strForumId != "0")
            {

                BindHeader(Guid.Parse(strForumId));
            }
        }

        protected void grdHeader_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdHeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                //Edit
                if (e.CommandName.ToLower().Equals("edit"))
                {
                    if (isMaker)
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }

                        divdetails.Visible = true;

                        HeaderDomain objHeader = HeaderDataProvider.Instance.Get(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        txtHeader.Text = objHeader.Header;
                        hdnHeaderId.Value = objHeader.HeaderId.ToString();
                        btnInsert.Text = "Update";
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
                }
                //delete 
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    ClearData();
                    if (isMaker)
                    {
                        if (!objUser.isDelete(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Header Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        bool status = HeaderDataProvider.Instance.Delete(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        ((Label)Info.FindControl("lblName")).Text = "Header deleted successfully";
                        Info.Visible = true;

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Header Master", "Success", Convert.ToString(e.CommandArgument), "Header deleted successfully");

                        BindHeader(Guid.Parse(ddlForum.SelectedValue));
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Header Master", "Failed", "", "Sorry you are not maker");

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

        protected void grdHeader_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Bind headers to grid
        /// </summary>
        private void BindHeader(Guid ForumId)
        {
            try
            {
                IList<HeaderDomain> objHeader = HeaderDataProvider.Instance.GetHeaderByForum(ForumId);
                if (objHeader.Count > 0)
                {
                    divdetails.Visible = false;
                }
                else
                {
                    divdetails.Visible = true;
                }
                grdHeader.DataSource = objHeader;//HeaderDataProvider.Instance.GetHeaderByForum(ForumId);
                grdHeader.DataBind();

            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    HeaderDomain objHeader = new HeaderDomain();

                    objHeader.Header = txtHeader.Text;
                    objHeader.ForumId = Guid.Parse(ddlForum.SelectedValue);

                    objHeader.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    if (hdnHeaderId.Value == "")
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objHeader.UpdatedBy), "Header Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objHeader.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        HeaderDataProvider.Instance.Insert(objHeader);

                        ((Label)Info.FindControl("lblName")).Text = "Header inserted successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objHeader);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objHeader.UpdatedBy), "Header Master", "Success", Convert.ToString(objHeader.HeaderId), "Header inserted successfully :- " + encrypt + "");

                        BindHeader(Guid.Parse(ddlForum.SelectedValue));
                        ClearData();

                        Info.Visible = true;
                    }
                    else
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objHeader.UpdatedBy), "Header Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objHeader.HeaderId = Guid.Parse(hdnHeaderId.Value);
                        bool status = HeaderDataProvider.Instance.Update(objHeader);

                        ((Label)Info.FindControl("lblName")).Text = "Header updated successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objHeader);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objHeader.UpdatedBy), "Header Master", "Success", Convert.ToString(objHeader.HeaderId), "Header updated successfully :- " + encrypt + "");

                        BindHeader(Guid.Parse(ddlForum.SelectedValue));
                        ClearData();
                        Info.Visible = true;
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Header Master", "Failed", "", "Sorry you are not maker");

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
        /// clear form data
        /// </summary>
        private void ClearData()
        {
            txtHeader.Text = "";
            hdnHeaderId.Value = "";
            btnInsert.Text = "Save";
            // divdetails.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
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

        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {

                    BindHeader(Guid.Parse(strForumId));
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