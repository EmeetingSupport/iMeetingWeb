using MM.Core;
using MM.Data;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MeetingMinder.Web
{
    public partial class UpcomingMeeting : System.Web.UI.Page
    {
        /// <summary>
        /// page laod event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //Bind Meeting List
                #region

                BindMeeting();

                #endregion
            }
        }

        /// <summary>
        /// bind upcoming meeting list
        /// </summary>
        private void BindMeeting()
        {
            try
            {
                DateTime dtToday = DateTime.Today;
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetAllDepartmentMeeting().OrderByDescending(m => m.CreatedOn).Where(m => Convert.ToDateTime(m.MeetingDate) >= dtToday).ToList();
                IList<MeetingDomain> objMeeting1 = objMeeting.OrderBy(m => Convert.ToDateTime(m.MeetingDate + " " + m.MeetingTime)).Where(m => Convert.ToDateTime(m.MeetingDate) >= dtToday).ToList();

                if (objMeeting.Count > 0)
                {
                    grdMeeting.DataSource = objMeeting1;
                    grdMeeting.DataBind();

                    ViewState["meetingList"] = objMeeting1;
                }
                else
                {
                    grdMeeting.DataSource = null;
                    grdMeeting.DataBind();
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
        /// Gridview pageindex change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMeeting.PageIndex = e.NewPageIndex;
            BindMeeting();
        }

        /// <summary>
        /// sort by department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                IList<MeetingDomain> objMeeting = ViewState["meetingList"] as IList<MeetingDomain>;

                DataTable dt = (DataTable)objMeeting.AsDataTable();
                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);

                    if (Convert.ToString(ViewState["sortDirection"]) == "asc")
                    {
                        ViewState["sortDirection"] = "desc";
                        dv.Sort = e.SortExpression + " DESC";
                    }
                    else
                    {
                        ViewState["sortDirection"] = "asc";
                        dv.Sort = e.SortExpression + " ASC";
                    }

                    grdMeeting.DataSource = dv;
                    grdMeeting.DataBind();
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