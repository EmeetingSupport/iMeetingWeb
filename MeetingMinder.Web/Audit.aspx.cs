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
using System.IO;
using System.Collections;

namespace MeetingMinder.Web
{
    public partial class Audit : System.Web.UI.Page
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

                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
                ///Added for hide entity name
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                    BindForum(EntityId.ToString());
                    BindUsers();
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                //   ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

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
                string strForumId = ddlForum.SelectedValue;
                ddlMeeting.Items.Clear();

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                ddlSerialNumber.SelectedValue = "0";
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

                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumIDForAudit(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting > dtToday)
                        //{
                        //ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime, item.MeetingId.ToString()));
                        ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));
                        //}
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



        /// <summary>
        /// Bind forums to drop down
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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsForAudit(entityId).OrderBy(p => p.ForumName).ToList();
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


        private void PopulatePager(int recordCount, int currentPage)
        {
            double dblPageCount = (double)((decimal)recordCount / grdReport.PageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0)
            {
                //pages.Add(new ListItem("First", "1", currentPage > 1));
                if (currentPage == 1 && pageCount <= 10)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage == 1 && pageCount > 10)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));
                    }
                    pages.Add(new ListItem("...", "1", false));
                }
                else if (currentPage != 1 && pageCount <= 10)
                {
                    //pages.Add(new ListItem("...", "1", true));
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                else if (currentPage != 1 && pageCount > 10 && currentPage != pageCount)
                {
                   // pages.Add(new ListItem("...", "1", true));
                    int z = currentPage - 5;
                    int x=0, y;

                    if (z < 1)
                    {
                        z = 1;
                        for (int i = z; i <= 10; i++)
                        {
                            pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));
                        }
                    }
                    else if (z >= 1)
                    {
                        if (pageCount < currentPage + 4)
                        {
                            y = (z - ((currentPage + 4) - pageCount));
                            x = pageCount;
                            pages.Add(new ListItem("...", "1", false));
                        }
                        else
                        {
                            x = currentPage + 4;
                            y = z;
                            if (y != 1)
                            {
                                pages.Add(new ListItem("...", "1", false));
                            }
                            
                        }
                        for (int i = y; i <= x; i++)
                        {
                            pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));                           
                        }
                        //if (x != pageCount)
                        //{
                        //    pages.Add(new ListItem("...", "1", false));
                        //}
                    }

                    if (x != pageCount)
                    {
                        pages.Add(new ListItem("...", "1", false));
                    }
                   
                    //pages.Add(new ListItem("...", "1", true));
                }
                else if (currentPage == pageCount)
                {
                    for (int i = pageCount - 9; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(" "+i.ToString(), i.ToString(), i != currentPage));
                    }
                }
                //pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        ///// Bind Entity list to drop down
        ///// </summary>
        ///// <param name="sender">Object specifying sender</param>
        ///// <param name="e">EventArgs specifying e </param>
        //protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    { }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);
        //    }
        //}


        private void BindConsolidateUser(int pageIndex)
        {
            Guid MeetingId = Guid.Empty;

            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;
            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }

            IList<UserDepartmentAuditDomain> objUserDepAuditDomain = new List<UserDepartmentAuditDomain>();
            IList<UserAuditDomain> objUserAuditDomain = AuditDataProvider.Instance.GetUserAudit(out totalCount,out objUserDepAuditDomain,StartDate, EndDate, pageIndex, grdReport.PageSize );
            grdReport.DataSource = objUserAuditDomain;
            grdReport.DataBind();

            //grdDep.Visible = true;


            if (objUserAuditDomain != null && objUserAuditDomain.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count = totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }
            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }


            //grdDep.DataSource = objUserDepAuditDomain;
            //grdDep.DataBind();

            //if (objUserDepAuditDomain.Count > 0)
            //{
            //    btnExport.Visible = true;
            //}
            //else
            //{
            //    btnExport.Visible = false;
            //}

            if (objUserAuditDomain.Count > 0 || objUserDepAuditDomain.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }

        private void BindConsolidateAgenda(int pageIndex)
        {

            Guid MeetingId = Guid.Empty;

            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            string SerialNumber = null;
            string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;
            
            if (ddlMeeting.SelectedValue.ToString() != "0")
            {
                MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                //varWhereCondition = "Where Meeting.MeetingId =" +""+ MeetingId;
            }

            if (ddlForum.SelectedValue.ToString() != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                //varWhereCondition = "Where Meeting.MeetingId =" + MeetingId;
            }

            if (ddlSerialNumber.SelectedValue.ToString() != "0")
            {
                SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                //varWhereCondition = "Where Meeting.MeetingId =" + MeetingId;
            }

            if (txtPresenter.Text != "")
            {
                Presenter = txtPresenter.Text;
            }

            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }
            IList<AgendaAuditDomain> objAgendaAuditDomain = AuditDataProvider.Instance.GetAgendaAuditByMeetingId(out totalCount,MeetingId, ForumId, SerialNumber, Presenter, StartDate, EndDate, pageIndex, grdReport.PageSize);
            grdReport.DataSource = objAgendaAuditDomain;
            grdReport.DataBind();

            if (objAgendaAuditDomain != null && objAgendaAuditDomain.Count > 0)
            {
               // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count = totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }

            else
            {
                rptPager.DataSource = null;
                DataBind();
            }

            if (objAgendaAuditDomain.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }

        private void BindForumToGrid( int pageIndex)
        {
            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;

            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }

            if (ddlForum.SelectedValue.ToString() != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());

            }
            IList<ForumAuditDomain> objForumAudit = AuditDataProvider.Instance.GetForumAuditByForumId(out totalCount,ForumId, StartDate, EndDate ,pageIndex, grdReport.PageSize);
            grdReport.DataSource = objForumAudit;
            grdReport.DataBind();
            //grdDep.Visible = false;


            if (objForumAudit != null && objForumAudit.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count = totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }

            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }

            if (objForumAudit.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }


           
        }

        private void BindForumAccessToGrid(int pageIndex)
        {
            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;

            if (ddlUser.SelectedValue.ToString() != "0")
            {
                UserId = Guid.Parse(ddlUser.SelectedValue.ToString());

            }
            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }

            IList<ForumAccessAuditDomain> objForum = AuditDataProvider.Instance.GetForumAccessAuditByUserId(out totalCount, UserId, StartDate, EndDate, pageIndex, grdReport.PageSize);
            grdReport.DataSource = objForum;
            grdReport.DataBind();
            //grdDep.Visible = false;

            if (objForum != null && objForum.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count =totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }

            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }

            if (objForum.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }

        private void BindMeetingToGrid(int pageIndex)
        {
            Guid MeetingId = Guid.Empty;
            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            string Number = null;
            string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;

            if (txtNumber.Text != "")
            {
                Number = txtNumber.Text;
            }
            if (ddlMeetingType.SelectedValue.ToString() != "0")
            {
                MeetingType = ddlMeetingType.SelectedValue.ToString();
            }

            if (ddlMeeting.SelectedValue.ToString() != "0")
            {
                MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
            }

            if (ddlForum.SelectedValue.ToString() != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
            }

            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }



            IList<NoticeAuditDomain> objNotice = new List<NoticeAuditDomain>();
            IList<MeetingAuditDomain> objMeeting = AuditDataProvider.Instance.GetMeetingAuditByMeetingId(out totalCount,ForumId, MeetingId, Number, MeetingType, StartDate, EndDate, out objNotice, pageIndex, grdReport.PageSize);
            grdReport.DataSource = objMeeting;
            grdReport.DataBind();

            //grdDep.Visible = true;
            //grdDep.DataSource = objNotice;
            //grdDep.DataBind();


            if (objMeeting != null && objMeeting.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count = totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }
            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }

            //if (objMeeting.Count > 0)
            //{
            //    btnExport.Visible = true;
            //}
            //else
            //{
            //    btnExport.Visible = false;
            //}

            if (objNotice.Count > 0 || objMeeting.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }


        private void BindMinutesToGrid(int pageIndex)
        {

            Guid MeetingId = Guid.Empty;
            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;

            if (ddlForum.SelectedValue.ToString() != "0")
            {
                ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());

            }

            if (ddlMeeting.SelectedValue.ToString() != "0")
            {
                MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());

            }

            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }

            IList<UploadMinuteAuditDomain> objUploadMinuteAuditDomain = AuditDataProvider.Instance.GetUploadMinutesAuditByMeetingId(out totalCount,ForumId, MeetingId, StartDate, EndDate, pageIndex, grdReport.PageSize);
            grdReport.DataSource = objUploadMinuteAuditDomain;
            grdReport.DataBind();
            //grdDep.Visible = false;


            if (objUploadMinuteAuditDomain != null && objUploadMinuteAuditDomain.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count =totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }
            }
            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }

            if (objUploadMinuteAuditDomain.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }

        private void BindUsersToGrid(int pageIndex)
        {

            Guid MeetingId = Guid.Empty;
            Guid ForumId = Guid.Empty;
            Guid UserId = Guid.Empty;
            //string SerialNumber = null;
            //string Presenter = null;
            //string Venue = null;
            //string Number = null;
            //string MeetingType = null;
            DateTime StartDate = Convert.ToDateTime("01/01/1978");
            DateTime EndDate = Convert.ToDateTime("01/01/1978");
            int totalCount = 0;

            if (ddlUser.SelectedValue.ToString() != "0")
            {
                UserId = Guid.Parse(ddlUser.SelectedValue.ToString());

            }

            if (txtFrom.Text != "" && txtTo.Text != "")
            {
                StartDate = Convert.ToDateTime(txtFrom.Text);
                EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
            }

            IList<UserDepartmentAuditDomain> objUserDepAuditDomain = new List<UserDepartmentAuditDomain>();
            IList<UserAuditDomain> objUserAuditDomain = AuditDataProvider.Instance.GetUserAuditByUserId(out totalCount,out objUserDepAuditDomain, UserId, StartDate, EndDate,  pageIndex, grdReport.PageSize);
            grdReport.DataSource = objUserAuditDomain;
            grdReport.DataBind();


            if (objUserAuditDomain != null && objUserAuditDomain.Count > 0)
            {
                // Int32 totalCount = Convert.ToInt16(ds.Tables[1].Rows[0][0]);
                Int32 Count = totalCount;
                if (Count > grdReport.PageSize)
                {
                    PopulatePager(Count, pageIndex);
                }
                else
                {
                    rptPager.DataSource = null;
                    rptPager.DataBind();
                }

            }

            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }
            //grdDep.Visible = true;
            //grdDep.DataSource = objUserDepAuditDomain;
            //grdDep.DataBind();

            //if (objUserDepAuditDomain.Count > 0)
            //{
            //    btnExport.Visible = true;
            //}
            //else
            //{
            //    btnExport.Visible = false;
            //}

            if (objUserAuditDomain.Count > 0 || objUserDepAuditDomain.Count > 0)
            {
                lbtnExportToExcel.Visible = true;
            }
            else
            {
                lbtnExportToExcel.Visible = false;
            }
        }

        protected void PageSize_Changed(object sender, EventArgs e)
        {
            if (ddlReport.SelectedValue == "ConsolidateAgenda")
            {
                BindConsolidateAgenda(1);
            }
            if (ddlReport.SelectedValue == "ConsolidateUser")
            {
                BindConsolidateUser(1);
            }

            if (ddlReport.SelectedValue == "Forum")
            {
                BindForumToGrid(1);
            }

            if (ddlReport.SelectedValue == "Forum_Access")
            {
                BindForumAccessToGrid(1);
            }
            if (ddlReport.SelectedValue == "Meeting")
            {
                BindMeetingToGrid(1);
            }

            if (ddlReport.SelectedValue == "Minutes")
            {
                BindMinutesToGrid(1);
            }
            if (ddlReport.SelectedValue == "User")
            {
                BindUsersToGrid(1);
            }
        }


        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            ViewState["PageIndex"] = pageIndex;
            if (ddlReport.SelectedValue == "ConsolidateAgenda")
            {
                BindConsolidateAgenda(pageIndex);
            }
            if (ddlReport.SelectedValue == "ConsolidateUser")
            {
                BindConsolidateUser(pageIndex);
            }

            if (ddlReport.SelectedValue == "Forum")
            {
                BindForumToGrid(pageIndex);
            }
            if (ddlReport.SelectedValue == "Forum_Access")
            {
                BindForumAccessToGrid(pageIndex);
            }
            if (ddlReport.SelectedValue == "Meeting")
            {
                BindMeetingToGrid(pageIndex);
            }

            if (ddlReport.SelectedValue == "Minutes")
            {
                BindMinutesToGrid(pageIndex);
            }
            if (ddlReport.SelectedValue == "User")
            {
                BindUsersToGrid(pageIndex);
            }
        }


        /// <summary>
        /// Button click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                rfvTo.Enabled = false;
                rfvTo.Enabled = false;
                //DateTime StartDate;
                //DateTime EndDate;

                //if (txtFrom.Text != "" || txtTo.Text != "")
                //{
                //    if (txtFrom.Text == "")
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = "Please select the start date.";
                //        Error.Visible = true;
                //        return;
                //    }

                //    if (txtTo.Text == "")
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = "Please select the end date.";
                //        Error.Visible = true;
                //        return;
                //    }

                //    StartDate = Convert.ToDateTime(txtFrom.Text);
                //    EndDate = Convert.ToDateTime(txtTo.Text);

                //    if(StartDate > EndDate)
                //    {
                //        //txtFrom.Text = "";
                //        //txtTo.Text = "";
                //        ((Label)Error.FindControl("lblError")).Text = "End date should be greater than start date.";
                //        Error.Visible = true;
                //        return;
                //    }
                //}
                //Guid MeetingId = Guid.Empty;
                //Guid ForumId = Guid.Empty;
                //Guid UserId = Guid.Empty;
                //DateTime StartDate = Convert.ToDateTime("01/01/1978");
                //DateTime EndDate = Convert.ToDateTime("01/01/1978");
               

                if (ddlReport.SelectedValue == "ConsolidateAgenda")
                {
                    BindConsolidateAgenda(1);
                }



                if (ddlReport.SelectedValue == "Agenda")
                {
                    //IList<AgendaAuditDomain> objAgneda = AuditDataProvider.Instance.GetAgendaAuditByMeetingId(Guid.Parse(ddlMeeting.SelectedValue));
                    DataSet objAgneda = AuditDataProvider.Instance.GetAgendaReportDataByMeetingId(Guid.Parse(ddlMeeting.SelectedValue));

                    if (objAgneda != null && objAgneda.Tables.Count > 0 && objAgneda.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= objAgneda.Tables[0].Rows.Count - 1; i++)
                        {
                            for (int j = 0; j <= objAgneda.Tables[0].Columns.Count - 1; j++)
                            {
                                string colName = objAgneda.Tables[0].Columns[j].ColumnName;

                                if (colName.ToLower().Equals("forumname"))
                                {
                                    objAgneda.Tables[0].Rows[i][j] = Encryptor.DecryptString(objAgneda.Tables[0].Rows[i][j].ToString());
                                }

                                if (colName.ToLower().Equals("meeting"))
                                {
                                    string Name = "";
                                    string Meeting = objAgneda.Tables[0].Rows[i][j].ToString();
                                    string[] strName = Meeting.Split(',');
                                    for (int p = 0; p <= strName.Count() - 1; p++)
                                    {
                                        Name += MM.Core.Encryptor.DecryptString(strName[p]) + " ";
                                    }
                                    objAgneda.Tables[0].Rows[i][j] = Convert.ToDateTime(Name).ToString("f");
                                }

                                if (colName.ToLower().Equals("makername") || colName.ToLower().Equals("cs") || colName.ToLower().Equals("md & ceo") || colName.ToLower().Equals("final approver") || (colName.ToLower().StartsWith("checker") && !colName.ToLower().EndsWith("approvedon")))
                                {
                                    string Name = "";
                                    string FullName =   objAgneda.Tables[0].Rows[i][j].ToString();
                                    string[] strName = FullName.Split(',');
                                    for (int n = 0; n <= strName.Count() - 1; n++)
                                    {
                                        if (n == strName.Count() - 1)
                                        {
                                            Name += string.IsNullOrWhiteSpace(MM.Core.Encryptor.DecryptString(strName[n])) ? "" : "(" + MM.Core.Encryptor.DecryptString(strName[n]) + ")";
                                        }
                                        else
                                        {
                                            Name += MM.Core.Encryptor.DecryptString(strName[n]) + " ";
                                        }
                                    }

                                    objAgneda.Tables[0].Rows[i][j] = Name;
                                }
                            }
                        }
                    }
                    grdReport.DataSource = objAgneda;
                    grdReport.DataBind();

                    //if (objAgneda.Count > 0)
                    //{
                    //    btnExport.Visible = true;
                    //}
                    //else
                    //{
                    //    btnExport.Visible = false;
                    //}

                   // grdDep.Visible = false;
                }

                if (ddlReport.SelectedValue == "Meeting")
                {

                    BindMeetingToGrid(1);
                }
                if (ddlReport.SelectedValue == "ConsolidateUser")
                {
                    BindConsolidateUser(1);
                }

                if (ddlReport.SelectedValue == "User")
                {
                    BindUsersToGrid(1);
                }

                if (ddlReport.SelectedValue == "Forum_Access")
                {
                    BindForumAccessToGrid(1);

                }

                if (ddlReport.SelectedValue == "Forum")
                {
                    BindForumToGrid(1);
                }

                if (ddlReport.SelectedValue == "Minutes")
                {

                    BindMinutesToGrid(1);
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
        /// Bind User grid
        /// </summary>
        private void BindUsers()
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";

                IList<UserDomain> objUserList = UserDataProvider.Instance.GetAuditUser().OrderBy(p => p.FirstName).ToList();
                if (objUserList.Count > 0)
                {
                    DataTable dt = objUserList.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }

                //Remove logged in users id from drop down

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

        protected void grdReport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell cell in e.Row.Controls)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Checked = false;
                        chk.ID = "chk" + cell.Text;
                        chk.Text = " " + cell.Text;
                        chk.CssClass = "btnCheck1";

                        cell.Controls.Add(chk);
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

        
        protected void lbtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Guid MeetingId = Guid.Empty;

                Guid ForumId = Guid.Empty;
                Guid UserId = Guid.Empty;
                string SerialNumber = null;
                string Presenter = null;
                string Venue = null;
                string Number = null;
                string MeetingType = null;
                DateTime StartDate = Convert.ToDateTime("01/01/1978");
                DateTime EndDate = Convert.ToDateTime("01/01/1978");
                int pageIndex = 1;
                int totalCount = 0;



                bool isCheckedGridCheckbox = false;
                bool isCheckedGridCheckbox2 = false;

                ArrayList arr = new ArrayList();
                ArrayList arr2 = new ArrayList();

                if (grdReport.HeaderRow.Cells != null)
                {
                    for (int i = 0; i < grdReport.HeaderRow.Cells.Count; i++)
                    {
                        CheckBox chkCol0 = (CheckBox)grdReport.HeaderRow.Cells[i].Controls[0];//FindControl("btnCheck1");
                        if (chkCol0 != null)
                        {
                            if (chkCol0.Checked)
                            {
                                isCheckedGridCheckbox = true;
                            }
                            arr.Add(chkCol0.Checked);
                        }
                    }
                }

                //if (grdDep.HeaderRow != null)
                //{
                //    for (int i = 0; i < grdDep.HeaderRow.Cells.Count; i++)
                //    {
                //        CheckBox chkCol0 = (CheckBox)grdDep.HeaderRow.Cells[i].Controls[0];//FindControl("btnCheck1");
                //        if (chkCol0 != null)
                //        {
                //            if (chkCol0.Checked)
                //            {
                //                isCheckedGridCheckbox2 = true;
                //            }
                //            arr2.Add(chkCol0.Checked);
                //        }
                //    }
                //}

                GridView grdRep = new GridView();
                GridView grdNotices = new GridView();

                if (ddlReport.SelectedValue == "ConsolidateAgenda")
                {
                    if (ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                        //varWhereCondition = "Where Meeting.MeetingId =" +""+ MeetingId;
                    }

                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                        //varWhereCondition = "Where Meeting.MeetingId =" + MeetingId;
                    }

                    if (ddlSerialNumber.SelectedValue.ToString() != "0")
                    {
                        SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                        //varWhereCondition = "Where Meeting.MeetingId =" + MeetingId;
                    }

                    if (txtPresenter.Text != "")
                    {
                        Presenter = txtPresenter.Text;
                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if(ViewState["PageIndex"]==null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }
                    IList<AgendaAuditDomain> objAgendaAuditDomain = AuditDataProvider.Instance.GetAgendaAuditByMeetingId(out totalCount, MeetingId, ForumId, SerialNumber, Presenter, StartDate, EndDate, pageIndex, grdReport.PageSize);
                    grdRep.DataSource = objAgendaAuditDomain;
                    grdRep.DataBind();

                }

                if (ddlReport.SelectedValue == "ConsolidateUser")
                {

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    IList<UserDepartmentAuditDomain> objUserDepAuditDomain = new List<UserDepartmentAuditDomain>();
                    IList<UserAuditDomain> objUserAuditDomain = AuditDataProvider.Instance.GetUserAudit(out totalCount, out objUserDepAuditDomain, StartDate, EndDate, pageIndex, grdReport.PageSize);
                    grdRep.DataSource = objUserAuditDomain;
                    grdRep.DataBind();
                }


                if (ddlReport.SelectedValue == "Agenda")
                {
                    // IList<AgendaAuditDomain> objAgneda = AuditDataProvider.Instance.GetAgendaAuditByMeetingId(Guid.Parse(ddlMeeting.SelectedValue));
                    DataSet objAgneda = AuditDataProvider.Instance.GetAgendaReportDataByMeetingId(Guid.Parse(ddlMeeting.SelectedValue));

                    if (objAgneda != null && objAgneda.Tables.Count > 0 && objAgneda.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= objAgneda.Tables[0].Rows.Count - 1; i++)
                        {
                            for (int j = 0; j <= objAgneda.Tables[0].Columns.Count - 1; j++)
                            {
                                string colName = objAgneda.Tables[0].Columns[j].ColumnName;

                                if (colName.ToLower().Equals("forumname"))
                                {
                                    objAgneda.Tables[0].Rows[i][j] = Encryptor.DecryptString(objAgneda.Tables[0].Rows[i][j].ToString());
                                }

                                if (colName.ToLower().Equals("meeting"))
                                {
                                    string Name = "";
                                    string Meeting = objAgneda.Tables[0].Rows[i][j].ToString();
                                    string[] strName = Meeting.Split(',');
                                    for (int p = 0; p <= strName.Count() - 1; p++)
                                    {
                                        Name += MM.Core.Encryptor.DecryptString(strName[p]) + " ";
                                    }
                                    objAgneda.Tables[0].Rows[i][j] = Convert.ToDateTime(Name).ToString("f");
                                }

                                if (colName.ToLower().Equals("makername") || colName.ToLower().Equals("cs") || colName.ToLower().Equals("md & ceo") || colName.ToLower().Equals("final approver") || (colName.ToLower().StartsWith("checker") && !colName.ToLower().EndsWith("approvedon")))
                                {
                                    string Name = "";
                                    string FullName = objAgneda.Tables[0].Rows[i][j].ToString();
                                    string[] strName = FullName.Split(',');
                                    for (int n = 0; n <= strName.Count() - 1; n++)
                                    {
                                        if (n == strName.Count() - 1)
                                        {
                                            Name += string.IsNullOrWhiteSpace(MM.Core.Encryptor.DecryptString(strName[n])) ? "" : "(" + MM.Core.Encryptor.DecryptString(strName[n]) + ")";
                                        }
                                        else
                                        {
                                            Name += MM.Core.Encryptor.DecryptString(strName[n]) + " ";
                                        }
                                    }

                                    objAgneda.Tables[0].Rows[i][j] = Name;
                                }
                            }
                        }
                    }
                    grdRep.DataSource = objAgneda;
                    grdRep.DataBind();


                }

                if (ddlReport.SelectedValue == "Meeting")
                {

                    if (txtNumber.Text != "")
                    {
                        Number = txtNumber.Text;
                    }
                    if (ddlMeetingType.SelectedValue.ToString() != "0")
                    {
                        MeetingType = ddlMeetingType.SelectedValue.ToString();
                    }

                    if (ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());
                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    IList<NoticeAuditDomain> objNotice = new List<NoticeAuditDomain>();

                    IList<MeetingAuditDomain> objMeeting = AuditDataProvider.Instance.GetMeetingAuditByMeetingId(out totalCount,ForumId, MeetingId, Number, MeetingType, StartDate, EndDate, out objNotice,pageIndex,grdReport.PageSize);
                    grdRep.DataSource = objMeeting;
                    grdRep.DataBind();

                    grdNotices.DataSource = objNotice;
                    grdNotices.DataBind();
                }


                if (ddlReport.SelectedValue == "User")
                {
                    if (ddlUser.SelectedValue.ToString() != "0")
                    {
                        UserId = Guid.Parse(ddlUser.SelectedValue.ToString());

                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    IList<UserDepartmentAuditDomain> objUserDepAuditDomain = new List<UserDepartmentAuditDomain>();
                    IList<UserAuditDomain> objUserAuditDomain = AuditDataProvider.Instance.GetUserAuditByUserId(out totalCount, out objUserDepAuditDomain, UserId, StartDate, EndDate, pageIndex, grdReport.PageSize);
                    grdRep.DataSource = objUserAuditDomain;
                    grdRep.DataBind();


                    //grdNotices.DataSource = objUserDepAuditDomain;
                    //grdNotices.DataBind();
                }

                if (ddlReport.SelectedValue == "Forum_Access")
                {
                    if (ddlUser.SelectedValue.ToString() != "0")
                    {
                        UserId = Guid.Parse(ddlUser.SelectedValue.ToString());

                    }
                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    IList<ForumAccessAuditDomain> objForum = AuditDataProvider.Instance.GetForumAccessAuditByUserId(out totalCount, UserId, StartDate, EndDate,pageIndex,grdReport.PageSize);
                    grdRep.DataSource = objForum;
                    grdRep.DataBind();

                }

                if (ddlReport.SelectedValue == "Forum")
                {
                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());

                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }

                    IList<ForumAuditDomain> objForumAudit = AuditDataProvider.Instance.GetForumAuditByForumId(out totalCount,ForumId, StartDate, EndDate,pageIndex,grdReport.PageSize);
                    grdRep.DataSource = objForumAudit;
                    grdRep.DataBind();

                }

                if (ddlReport.SelectedValue == "Minutes")
                {
                    if (ddlForum.SelectedValue.ToString() != "0")
                    {
                        ForumId = Guid.Parse(ddlForum.SelectedValue.ToString());

                    }

                    if (ddlMeeting.SelectedValue.ToString() != "0")
                    {
                        MeetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());

                    }

                    if (txtFrom.Text != "" && txtTo.Text != "")
                    {
                        StartDate = Convert.ToDateTime(txtFrom.Text);
                        EndDate = Convert.ToDateTime(txtTo.Text).AddMonths(1).AddSeconds(-1);
                    }

                    if (ViewState["PageIndex"] == null)
                    {
                        pageIndex = 1;
                    }
                    else
                    {
                        pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                    }


                    IList<UploadMinuteAuditDomain> objUploadMinuteAuditDomain = AuditDataProvider.Instance.GetUploadMinutesAuditByMeetingId(out totalCount, ForumId, MeetingId, StartDate, EndDate,pageIndex,grdReport.PageSize);
                    grdRep.DataSource = objUploadMinuteAuditDomain;
                    grdRep.DataBind();
                }

                if (isCheckedGridCheckbox && grdReport.HeaderRow != null)
                {
                    for (int i = 0; i < grdRep.HeaderRow.Cells.Count; i++)
                    {
                        grdRep.HeaderRow.Cells[i].Visible = Convert.ToBoolean(arr[i]); //chkCol0.Checked;
                    }


                    for (int i = 0; i < grdRep.Rows.Count; i++)
                    {
                        GridViewRow row = grdRep.Rows[i];
                        for (int k = 0; k < row.Cells.Count; k++)
                        {
                            row.Cells[k].Visible = Convert.ToBoolean(arr[k]);
                        }
                    }
                }


                //if (isCheckedGridCheckbox2 && grdDep.HeaderRow != null)
                //{
                //    for (int i = 0; i < grdNotices.HeaderRow.Cells.Count; i++)
                //    {
                //        grdNotices.HeaderRow.Cells[i].Visible = Convert.ToBoolean(arr2[i]); //chkCol0.Checked;
                //    }


                //    for (int i = 0; i < grdNotices.Rows.Count; i++)
                //    {
                //        GridViewRow row = grdNotices.Rows[i];
                //        for (int k = 0; k < row.Cells.Count; k++)
                //        {
                //            row.Cells[k].Visible = Convert.ToBoolean(arr2[k]);
                //        }
                //    }
                //}



                Response.Clear();

                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");

                Response.Charset = "";

                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);

                grdReport.AllowPaging = false;

                Table tb = new Table();

                TableRow tr1 = new TableRow();

                TableCell cell1 = new TableCell();

                cell1.Controls.Add(grdRep);

                tr1.Cells.Add(cell1);

                TableCell cell3 = new TableCell();

                cell3.Controls.Add(grdNotices);

                TableCell cell2 = new TableCell();

                cell2.Text = "&nbsp;";

                //if (rbPreference.SelectedValue == "2")
                //{

                //    tr1.Cells.Add(cell2);

                //    tr1.Cells.Add(cell3);

                //    tb.Rows.Add(tr1);

                //}

                //else
                //{

                TableRow tr2 = new TableRow();

                tr2.Cells.Add(cell2);

                TableRow tr3 = new TableRow();

                tr3.Cells.Add(cell3);

                tb.Rows.Add(tr1);

                tb.Rows.Add(tr2);

                tb.Rows.Add(tr3);

                //}

                tb.RenderControl(hw);



                //style to format numbers to string

                string style = @"<style> .textmode { mso-number-format:\@; } </style>";

                Response.Write(style);

                Response.Output.Write(sw.ToString());

                Response.Flush();

                Response.End();

                //grdRep.RenderControl(hw);

                //string style = @"<style> .textmode { mso-number-format:\@; } </style>";

                //Response.Write(style);

                //Response.Output.Write(sw.ToString());

                //Response.End();
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