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
    public partial class CheckerMaster : System.Web.UI.Page
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
                //bind meeting list
                BindMeeting();
            }
        }

        /// <summary>
        /// Bind Meeting details
        /// </summary>
        private void BindMeeting()
        {
            try
            {
                grdMeeting.DataSource = CheckerDataProvider.Instance.GetAllUnApprovedMeeting();
                grdMeeting.DataBind();
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
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdMeeting.PageIndex = e.NewPageIndex;
                BindMeeting();
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
        protected void grdMeetingRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("view"))
                {
                    string strMeetingId = Convert.ToString(e.CommandArgument);
                    ViewState["MeetingId"] = strMeetingId;
                    grdDocument.DataSource = AgendaDataProvider.Instance.GetUnApprovedDocumet(Guid.Parse(strMeetingId));
                    grdDocument.DataBind();
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
        ///  Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = CheckerDataProvider.Instance.GetAllUnApprovedMeeting().AsDataTable();
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

                grdMeeting.DataSource = dv;
                grdMeeting.DataBind();
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
        protected void grdDocumentRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string UserId = Convert.ToString(Session["UserId"]);
                //approve agenda or agenda item
                if (e.CommandName.ToLower().Equals("approve"))
                {
                    string strAgendaId = Convert.ToString(e.CommandArgument);
                    bool isSave = AgendaDataProvider.Instance.UpdateAgendaStatus(Guid.Parse(strAgendaId), "Approved", Guid.Parse(UserId));
                    ((Label)Info.FindControl("lblName")).Text = "Agenda approved Successfully";
                }

                //reject agenda or agenda item
                if (e.CommandName.ToLower().Equals("decline"))
                {
                    string strAgendaId = Convert.ToString(e.CommandArgument);
                    bool isSave = AgendaDataProvider.Instance.UpdateAgendaStatus(Guid.Parse(strAgendaId), "Declined", Guid.Parse(UserId));
                    ((Label)Info.FindControl("lblName")).Text = "Declined approved Successfully";
                }


                Info.Visible = true;

                string strMeetingId = Convert.ToString(ViewState["MeetingId"]);
                grdDocument.DataSource = AgendaDataProvider.Instance.GetUnApprovedDocumet(Guid.Parse(strMeetingId));
                grdDocument.DataBind();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        ///  Function Used For Gridview Sorting
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdDocument_Sorting(object sender, GridViewSortEventArgs e)
        {

            //if (Convert.ToString(ViewState["sortDirection"]) == "asc")
            //{
            //    ViewState["sortDirection"] = "dsc";
            //    dv.Sort = e.SortExpression + " DESC";
            //}
            //else
            //{
            //    ViewState["sortDirection"] = "asc";
            //    dv.Sort = e.SortExpression + " ASC";
            //}

            //grdMeeting.DataSource = dv;
            //grdMeeting.DataBind();
        }
    }
}