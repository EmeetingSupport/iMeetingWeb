using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.IO;
using System.Data;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp;
using PdfSharp.Drawing.Layout;

namespace MeetingMinder.Web
{
    public partial class ViewNotice : System.Web.UI.Page
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
            if (!IsPostBack)
            {

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
                    ddlEntity.SelectedValue = EntityId.ToString();
                    // ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion



                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                BindYear();
            }
        }


        /// <summary>
        /// Bind Year to drop down 
        /// </summary>
        
        private void BindYear()
        {
            int currentYear = DateTime.Now.Year;
            ddlYear.Items.Insert(0, new ListItem("Select Year", "0"));
            for (int i = 2012; i <= currentYear+1; i++)
            {
                
                ddlYear.Items.Add(i.ToString());
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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId));
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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ForumDomain objforum = new ForumDomain();
                string strForumId = ddlForum.SelectedValue;

                objforum = ForumDataProvider.Instance.Get(Guid.Parse(strForumId));
                //Session["General Manager"] = Convert.ToString(objforum.GeneralManager);

                //string strForumId = ddlForum.SelectedValue;
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
                trNotice.Visible = false;
                btnPrint.Visible = false;
                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    if (ddlYear.SelectedValue != "0")
                    {
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).Where(p => DateTime.Parse(p.MeetingDate).Year == Convert.ToInt32(ddlYear.SelectedItem.Text)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                        DateTime dtToday = DateTime.Now.Date;
                        foreach (MeetingDomain item in objMeeting)
                        {
                            DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                            //if (dtMeeting < dtToday)
                            //{
                            //   ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                            ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                            //}
                        }
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

        /// Bind Entity list to drop down
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
        /// Bind forums to drop down
        /// </summary>
        /// <param name="EntityId">string specifying EntityId</param>
        private void BindForum(string EntityId)
        {
            try
            {

                ddlForum.Items.Clear();
                trNotice.Visible = false;
                btnPrint.Visible = false;
                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"]));
                Guid entityId;
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p=>p.ForumName).ToList();
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
                //string GM = Session["General Manager"].ToString();
                if (strMeetingId != "0")
                {
                    trNotice.Visible = true;
                    NoticeDomain objNotice = NoticeDataProvider.Instance.GetNoticeByMeeting(Guid.Parse(strMeetingId));

                    if (objNotice != null)
                    {
                        if (objNotice.NoticeMessage.Length > 0)
                        {
                            UserDomain objUser = UserDataProvider.Instance.Get(Guid.Parse(Convert.ToString(Session["UserId"])));
                            string UserName = objUser.Suffix + " " + objUser.FirstName + " " + objUser.LastName;
                            lblView.ForeColor = System.Drawing.Color.Gray;

                            string company = "Company Secretary";
                            string dateNotice = "Date: " + objNotice.CreatedOn.ToString("MMMM dd, yyyy.");

                            MeetingDomain objMeeting = MeetingDataProvider.Instance.GetMeetingWithEntity(objNotice.MeetingId);

                           // string Note = "Notice is hereby given that the " + objMeeting.MeetingNumber + " meeting of the <u>" + objMeeting.ForumName + "</u> of <u>" + objMeeting.EntityName + "</u> will be held on <u>" + Convert.ToDateTime(objMeeting.MeetingDate).ToString("D") + "</u> at <u>" + objMeeting.MeetingTime + "</u> in <u>" + objMeeting.MeetingVenue + ".</u> Kindly make it convenient to attend the meeting.";
                            string Note = objNotice.NoticeMessage.Replace("{Meeting_Number}", objMeeting.MeetingNumber).Replace("{Committee}", objMeeting.ForumName).Replace("{Entity}", objMeeting.EntityName).Replace("{Date}", Convert.ToDateTime(objMeeting.MeetingDate).ToString("D")).Replace("{Time}", objMeeting.MeetingTime).Replace("{Venue}", objMeeting.MeetingVenue);
                            //string Note = objNotice.NoticeMessage.Replace("{Meeting_Number}", objMeeting.MeetingNumber).Replace("{General Manager}", GM).Replace("{Committee}", objMeeting.ForumName).Replace("{Entity}", objMeeting.EntityName).Replace("{Date}", Convert.ToDateTime(objMeeting.MeetingDate).ToString("D")).Replace("{Time}", objMeeting.MeetingTime).Replace("{Venue}", objMeeting.MeetingVenue);

                            lblPrint.Text = @"<style type='text/css'> #page_1 {position:relative; font-size:20px; overflow: hidden;margin: 0px 0px 0px 0px;padding: 0px;border: none;width: 1100px; }

.dclr {clear:both;float:none;height:1px;margin:0px;padding:0px;overflow:hidden;}
.ft0{font: italic 15px 'Arial';color: #141b29;line-height: 15px;}
.ft1{font: italic 15px 'Arial';color: #141b29;line-height: 14px;}
.p0{    margin-bottom: 0;     padding-left: 127px;    padding-right: 30px;    text-align: center;    width: 91%;}
.p1{text-align: left;padding-left: 237px;margin-bottom: 0px;} </style>";

                            lblPrint.Text += "<DIV id='page_1'><DIV style='padding-right: 50px;' align='right' id='dimg1'><IMG src='SimplpanAdminPanelfiles/sbi.png' id='img1'></DIV><div style='height:80%'><div align='center'><h3>Notice</h3></div><br><br><label>" + UserName + "</label><br/><br/><p style='margin-left:20px;'>" + Note + "</p><br><br/><b><label>" + company + "<label></b><br/>" + dateNotice + "</div>";
                            //lblPrint.Text += @"<div style='position: relative;'><div class='p0'><b style='color: rgb(57, 108, 158);'>EROS INTERNATIONAL MEDIA LIMITED</b><br>Corporate Office: 901/902, Supreme Chambers, Off Veera Des&amp; Road, Andheri (VV), Mumbai - 400 053.<br>Tel.: +91-22-6602 1500 Fax: +91.22-6602 1540 E-mail: eroggerosinti.com  www.erosintl.com<br>Regd. Office: Kailash Plaza, 2nd Floor, Plot No. 12, Off Veera Desai Road, Andheri (W), Mumba' - 400 053.<br>CIN No. L99999MH1994PLC080502</div>   <div style='clear: both;' class='clearBoth'></div> <div class='clearBoth' style='clear: both;'></div></div>";

                         //   lblPrint.Text += "<div style='background-image:url(images/IFC.jpg)' align='center'><h3>Notice</h3></div><br><br><label>" + UserName + "</label><br/><br/><p style='margin-left:20px;'>" + Note + "</p><br><br/><b><label>" + company + "<label></b><br/>" + dateNotice;
                            lblView.Text = Note;
                            lblView.Visible = true;
                            btnPrint.Visible = true;
                        }
                        else
                        {
                            lblView.Text = "No notice is added yet";
                            lblView.ForeColor = System.Drawing.Color.Red;
                            lblView.Visible = true;
                            btnPrint.Visible = false;
                        }
                    }
                    else
                    {
                        lblView.Text = "No notice is added yet";
                        lblView.ForeColor = System.Drawing.Color.Red;
                        lblView.Visible = true;
                        btnPrint.Visible = false;
                    }
                }
                else
                {
                    trNotice.Visible = false;
                    btnPrint.Visible = false;
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


        public void PrintNotice()
        {
            try
            {
                string strMeetingId = ddlMeeting.SelectedValue;
                if (strMeetingId != "0")
                {
                    NoticeDomain objNotice = NoticeDataProvider.Instance.GetNoticeByMeeting(Guid.Parse(strMeetingId));

                    if (objNotice != null)
                    {
                        if (objNotice.NoticeMessage.Length > 0)
                        {
                            UserDomain objUser = UserDataProvider.Instance.Get(Guid.Parse(Convert.ToString(Session["UserId"])));
                            // Create a new PDF document
                            PdfDocument document = new PdfDocument();

                            // Create an empty page
                            PdfPage page = document.AddPage();

                            //     // Get an XGraphics object for drawing
                            //     XGraphics gfx = XGraphics.FromPdfPage(page);

                            //     // Create a font
                            //     XFont font = new XFont("Times New Roman", 14, XFontStyle.Bold);

                            //     XFont fontNotice = new XFont("Times New Roman", 12, XFontStyle.Regular);

                            string UserName = objUser.Suffix + " " + objUser.FirstName + " " + objUser.LastName;

                            //     // Draw the text
                            //     gfx.DrawString(UserName, font, XBrushes.Black,
                            //       new XRect(50,50, page.Width, 20),
                            //       XStringFormat.TopLeft);                       

                            //      gfx.DrawString(objNotice.NoticeMessage, fontNotice, XBrushes.Black,
                            //new XRect(65, 50, page.Width - 20, 80),
                            //XStringFormat.Center);


                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            XFont font = new XFont("Times New Roman", 12, XFontStyle.Regular);
                            XFont fontText = new XFont("Times New Roman", 14, XFontStyle.Bold);
                            XFont fontHead = new XFont("Times New Roman", 16, XFontStyle.Bold);
                            XTextFormatter tf = new XTextFormatter(gfx);



                            XRect rect = new XRect(40, 100, page.Width - 100, 220);
                            gfx.DrawRectangle(XBrushes.White, rect);
                            //tf.Alignment = ParagraphAlignment.Left;

                            //    tf.DrawString("Notice",fontHead,XBrushes.Black,new XRect(0,0,page.Width-100,30),XStringFormat.Center);
                            tf.DrawString(UserName, fontText, XBrushes.Black, rect, XStringFormats.TopLeft);

                            rect = new XRect(70, 200, page.Width - 100, 120);
                            gfx.DrawRectangle(XBrushes.White, rect);
                            tf.Alignment = XParagraphAlignment.Justify;
                            tf.DrawString(objNotice.NoticeMessage, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                            string company = "Company Secretory";
                            string dateNotice = "Date:" + objNotice.UpdatedOn.ToString("MMMM dd, yyyy.");
                            rect = new XRect(40, 100, page.Width - 100, 20);
                            gfx.DrawRectangle(XBrushes.White, rect);
                            //tf.Alignment = ParagraphAlignment.Left;
                            tf.DrawString(company, fontText, XBrushes.Black, rect, XStringFormats.TopLeft);

                            rect = new XRect(40, 410, page.Width - 100, 100);
                            gfx.DrawRectangle(XBrushes.White, rect);
                            //tf.Alignment = ParagraphAlignment.Left;
                            tf.DrawString(dateNotice, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                            // Save the document...
                            string filename = "Notice.pdf";
                            document.Save(Path.Combine(Server.MapPath("/" + filename)));
                        }
                        else
                        {
                            lblView.Text = "No notice is added yet";
                            lblView.ForeColor = System.Drawing.Color.Red;
                            lblView.Visible = true;
                        }
                    }
                    else
                    {
                        lblView.Text = "No notice is added yet";
                        lblView.ForeColor = System.Drawing.Color.Red;
                        lblView.Visible = true;
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

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}