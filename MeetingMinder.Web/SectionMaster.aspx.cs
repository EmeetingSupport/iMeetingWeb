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
    public partial class SectionMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;

            if (!Page.IsPostBack)
            {
                try
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    // BindAgenda(EntityId);
                    BindForum(EntityId.ToString());
                }
                catch (Exception ex)
                {
                    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    Error.Visible = true;

                    LogError objEr = new LogError();
                    objEr.HandleException(ex);
                }

            }

            if (Request.UrlReferrer != null)
            {
                string prvPage = Request.UrlReferrer.ToString();

                Guid ForumId = Guid.Empty;
                if (prvPage.ToLower().EndsWith("sectionorder.aspx"))
                {
                    if (Guid.TryParse(Convert.ToString(Session["ForumId"]), out ForumId))//(Guid.TryParse(Request.QueryString["id"].ToString(), out MeetingId))
                    {
                        ListItem item = ddlForum.Items.FindByValue(ForumId.ToString());
                        if (item != null)
                        {
                            ddlForum.SelectedValue = ForumId.ToString();
                            ddlForum_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
        }

        protected void grdSection_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ClearData();
            grdSection.PageIndex = e.NewPageIndex;
            BindSection(Guid.Parse(ddlForum.SelectedValue));
        }

        protected void grdSection_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdSection_RowCommand(object sender, GridViewCommandEventArgs e)
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
                        SectionDomain objSection = SectionDataProvider.Instance.GetAllSectionsDetailsById(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        txtTitle.Text = objSection.Title;
                        txtSerialNumber.Text = objSection.SerialNumber;
                        hdnSectionId.Value = objSection.SectionId.ToString();
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
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Section Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        bool status = SectionDataProvider.Instance.Delete(Guid.Parse(Convert.ToString(e.CommandArgument)));
                        ((Label)Info.FindControl("lblName")).Text = "Section deleted successfully";
                        Info.Visible = true;

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Section Master", "Success", Convert.ToString(e.CommandArgument), "Section deleted successfully");

                        BindSection(Guid.Parse(ddlForum.SelectedValue));
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Section Master", "Failed", "", "Sorry you are not maker");

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

        protected void grdSection_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /// <summary>
        /// Bind master SEctions to grid
        /// </summary>
        private void BindSection(Guid ForumId)
        {
            try
            {
                IList<SectionDomain> objSec = SectionDataProvider.Instance.Get(ForumId);

                grdSection.DataSource = objSec;
                grdSection.DataBind();

                if (objSec.Count > 0)
                {
                    lnkChangeOrder.Visible = true;
                }
                else
                {
                    lnkChangeOrder.Visible = false;
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
                    BindSection(Guid.Parse(ddlForum.SelectedValue));
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
            txtTitle.Text = "";
            txtSerialNumber.Text = "";
            hdnSectionId.Value = "";
            btnInsert.Text = "Save";

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
        /// Button insert click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnInsert_Click1(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (ddlForum.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select forum.";
                    Error.Visible = true;
                    return;
                }

                if (txtTitle.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtTitle);
                    if (!objUser.isValidChar(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.isValidChar(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid section title.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid section title.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter section title.";
                    Error.Visible = true;
                    return;
                }

                if (txtSerialNumber.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtSerialNumber);
                    if (!objUser.isValidChar(txtSerialNumber.Text))
                    {
                        txtSerialNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }                    
                    if (!objUser.CSVValidation(txtSerialNumber.Text))
                    {
                        txtSerialNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    SectionDomain objSection = new SectionDomain();
                    objSection.ForumId = Guid.Parse(ddlForum.SelectedValue);
                    objSection.Title = txtTitle.Text;
                    objSection.SerialNumber = txtSerialNumber.Text;
                    //Guid EntityId = Guid.Parse(ddlEntity.SelectedValue);
                    objSection.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                    if (hdnSectionId.Value == "")
                    {
                        if (!objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSection.UpdatedBy), "Section Master", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objSection.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                        SectionDataProvider.Instance.Insert(objSection);
                        ((Label)Info.FindControl("lblName")).Text = "Section inserted successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objSection);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSection.UpdatedBy), "Section Master", "Success", Convert.ToString(objSection.SectionId), "Section inserted successfully :- " + encrypt + "");

                        BindSection(Guid.Parse(ddlForum.SelectedValue));
                        ClearData();

                        Info.Visible = true;
                    }
                    else
                    {
                        if (!objUser.IsEdit(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSection.UpdatedBy), "Section Master", "Failed", Convert.ToString(objSection.SectionId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            return;
                        }
                        objSection.SectionId = Guid.Parse(hdnSectionId.Value);
                        bool status = SectionDataProvider.Instance.Update(objSection);

                        ((Label)Info.FindControl("lblName")).Text = "Section updated successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objSection);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objSection.UpdatedBy), "Section Master", "Success", Convert.ToString(objSection.SectionId), "Section updated successfully :- " + encrypt + "");

                        BindSection(Guid.Parse(ddlForum.SelectedValue));
                        ClearData();
                        Info.Visible = true;
                    }
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Section Master", "Failed", "", "Sorry you are not maker");

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        protected void lnkChangeOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    Session["ForumId"] = ddlForum.SelectedValue;
                    Response.Redirect("SectionOrder.aspx");
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