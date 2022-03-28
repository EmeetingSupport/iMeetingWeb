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


namespace MeetingMinder.Web
{
    public partial class MeetingSchedule : System.Web.UI.Page
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
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
                #endregion

                //ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
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
            for (int i = 2012; i <= currentYear + 1; i++)
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
                ddlEntity.DataSource = UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId)).OrderBy(p => p.EntityName).ToList(); ;
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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList(); ;
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
        /// Gridview pageindex change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMeeting.PageIndex = e.NewPageIndex;
            ddlForum_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// gridview sorting event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdMeeting_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    Guid forumId;
                    if (Guid.TryParse(strForumId, out forumId))
                    {
                        IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();
                        IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                        DateTime dtToday = DateTime.Now.Date;

                        foreach (MeetingDomain item in objMeeting)
                        {
                            DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                            //if (dtMeeting >= dtToday)
                            //{
                            objMeetingBind.Add(item);
                            //}
                        }

                        DataTable dt = (DataTable)objMeetingBind.AsDataTable();

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
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    ForumDomain objForum = ForumDataProvider.Instance.Get(Guid.Parse(strForumId));

                    Guid forumId;
                    if (Guid.TryParse(strForumId, out forumId))
                    {
                        IList<MeetingDomain> objMeetingBind = new List<MeetingDomain>();
                        if (ddlYear.SelectedValue != "0")
                        {

                            IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).Where(p => DateTime.Parse(p.MeetingDate).Year == Convert.ToInt32(ddlYear.SelectedItem.Text)).ToList();

                            DateTime dtToday = DateTime.Now.Date;

                            foreach (MeetingDomain item in objMeeting)
                            {
                                DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                                //if (dtMeeting >= dtToday)
                                //{
                                objMeetingBind.Add(item);
                                //}
                            }

                            grdMeeting.DataSource = objMeetingBind;

                            grdMeeting.DataBind();
                            grdMeeting.Visible = true;
                            //if (objMeeting.Count > 0)
                            //{
                            //    EntityName = objMeeting[0].EntityName;
                            //    EntityId = objMeeting[0].EntityId.ToString();
                            //}
                        }
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
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
        /// drop down list Selected Index Change event
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
                    grdMeeting.Visible = false;
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