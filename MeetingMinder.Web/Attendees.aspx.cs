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

namespace MeetingMinder.Web.EventCal
{
    public partial class Attendis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //Bind Visitors
                BindAllVisitors();
            }
        }

        /// <summary>
        /// Bind visitors
        /// </summary>
        private void BindAllVisitors()
        {
            try
            {
                IList<VisitorDomain> objVisitor = VisitoreDataProvider.Instance.Get(Guid.Parse(Session["UserId"].ToString()));
                grdAttendis.DataSource = objVisitor;
                grdAttendis.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                VisitorDomain objVisitor = new VisitorDomain();
                objVisitor.Name_Of_Visitor = txtName.Text.Trim();
                objVisitor.LinkedInUrl = txtLinkedInUrl.Text.Trim();
                objVisitor.Name_Of_Visitor_Organisation = txtOrgnaization.Text.Trim();
                objVisitor.Designation_Of_Visitor = txtDesignation.Text.Trim();
                objVisitor.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objVisitor.Phone = txtPhoneNo.Text;
                objVisitor.Email = txtEmail.Text;
                string webRootPath = Server.MapPath("~");
                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    //Update
                    if (hdnVisitorId.Value != "" && hdnVisitorId.Value != null)
                    {
                        if (fuProfile.HasFile)
                        {

                            string ext = Path.GetExtension(fuProfile.FileName);
                            string contentType = fuProfile.PostedFile.ContentType;
                            if (contentType.ToLower().Contains("image/"))
                            {
                                ext = Path.GetExtension(fuProfile.FileName);//fileUploadPhotosAdd.FileName);
                                string FileName = DateTime.Now.Ticks + ext;
                                fuProfile.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileName));
                                objVisitor.Upload_Photo = FileName;
                                //Delete existing image
                                if (ViewState["Photo"] != null)
                                {
                                    string Photo = Convert.ToString(ViewState["Photo"]);
                                    File.Delete(Server.MapPath(savePath + Photo));
                                    ViewState["Photo"] = null;
                                }
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Profile photo must be image file";
                                Error.Visible = true;
                                return;
                            }
                        }


                        if (ViewState["Photo"] != null)
                        {
                            objVisitor.Upload_Photo = Convert.ToString(ViewState["Photo"]);
                        }

                        objVisitor.VisiterId = Convert.ToInt64(hdnVisitorId.Value);
                        bool status = VisitoreDataProvider.Instance.Update(objVisitor);
                        if (status)
                        {

                            ((Label)Info.FindControl("lblName")).Text = " Attendees updated successfully";

                            Info.Visible = true;
                            BindAllVisitors();
                            ClearData();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Attendees updation failed";
                            Error.Visible = true;
                        }
                    }
                    //insert
                    else
                    {
                        string ext = "";
                        string FileName = "";

                        if (fuProfile.HasFile)
                        {
                            string contentType = fuProfile.PostedFile.ContentType;

                            if (contentType.ToLower().Contains("image/"))
                            {
                                ext = Path.GetExtension(fuProfile.FileName);//fileUploadPhotosAdd.FileName);
                                FileName = DateTime.Now.Ticks + ext;
                                fuProfile.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileName));
                                objVisitor.Upload_Photo = FileName;
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Profile photo must be image file";
                                Error.Visible = true;
                                return;
                            }
                        }

                        objVisitor = VisitoreDataProvider.Instance.Insert(objVisitor);
                        if (objVisitor.VisiterId > 0)
                        {
                            ((Label)Info.FindControl("lblName")).Text = " Attendees created successfully";
                            Info.Visible = true;
                            BindAllVisitors();
                            ClearData();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = "Attendees creation failed";
                            Error.Visible = true;
                        }
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

        private void ClearData()
        {
            txtDesignation.Text = "";
            txtLinkedInUrl.Text = "";
            txtName.Text = "";
            txtOrgnaization.Text = "";
            btnSubmit.Text = "Submit";
            hdnVisitorId.Value = "";
            ViewState["Photo"] = null;
            txtPhoneNo.Text = "";
            txtEmail.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void grdAttendis_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAttendis.PageIndex = e.NewPageIndex;
                BindAllVisitors();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void grdAttendis_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("edits"))
                {
                    string VisitorId = Convert.ToString(e.CommandArgument);
                    VisitorDomain objVisitor = VisitoreDataProvider.Instance.GetVisterById(Convert.ToInt32(VisitorId));

                    txtDesignation.Text = objVisitor.Designation_Of_Visitor;
                    txtLinkedInUrl.Text = objVisitor.LinkedInUrl;
                    txtName.Text = objVisitor.Name_Of_Visitor;
                    txtOrgnaization.Text = objVisitor.Name_Of_Visitor_Organisation;
                    hdnVisitorId.Value = objVisitor.VisiterId.ToString();
                    txtEmail.Text = objVisitor.Email.ToString();
                    txtPhoneNo.Text = objVisitor.Phone.ToString();
                    if (objVisitor.Upload_Photo != null)
                    {
                        if (objVisitor.Upload_Photo.Length > 0)
                        {
                            ViewState["Photo"] = objVisitor.Upload_Photo;
                            //lnkView.Visible = true;
                            //lnkDelete.Visible = true;
                        }
                        else
                        {
                            //lnkView.Visible = false;
                            //lnkDelete.Visible = false;
                        }
                    }
                    btnSubmit.Text = "Update";
                }

                //delete forum
                if (e.CommandName.ToLower().Equals("delete"))
                {
                    UserAcess objUser = new UserAcess();
                    //check Delete permission
                    //if (objUser.isDelete(Guid.Parse(se["entityId"].ToString())))
                    //{
                        bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                        if (isMaker)
                        {
                            string strVisitorId = Convert.ToString(e.CommandArgument.ToString());

                            bool bStatus = VisitoreDataProvider.Instance.Delete(Convert.ToInt64(strVisitorId));
                            if (bStatus)
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Attendees Deleted Successfully";
                                Info.Visible = true;
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                                Error.Visible = true;
                            }
                            ClearData();
                            BindAllVisitors();
                        }
                        else
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                            Error.Visible = true;
                        }
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

        protected void grdAttendis_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

    }
}