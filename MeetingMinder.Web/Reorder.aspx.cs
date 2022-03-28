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
    public partial class Reorder : System.Web.UI.Page
    {
        /// <summary>
        /// Page laod Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Info.Visible = false;
            Error.Visible = false;
            Page.MaintainScrollPositionOnPostBack = true;
            if (!Page.IsPostBack)
            {
                //if (Request.QueryString["dir"] != null)
                //{

                    lblListName.Text = "Directors List";
                    lblItemName.Text = "Directors List";
                    //Bind directores list.
                    BindDirectores();
                //}

                if (Request.QueryString["For"] != null)
                {
                    lblListName.Text = "Forum List";
                    lblItemName.Text = "Forum List";

                    try
                    {
                        //Bind Forums details
                        if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                        {
                            Guid entityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
                            BindForums(entityId);
                        }
                        else
                        {
                            Response.Redirect("~/login.aspx");
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

        private void BindForums(Guid entityId)
        {
            StringBuilder strForum = new StringBuilder("");

            strForum.Append("<div id='accordion'><ol id='sort'>");
            IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
            if (objForum.Count > 0)
            {
                for (int i = 0; i <= objForum.Count - 1; i++)
                {
                    strForum.Append(" <li id=" + objForum[i].ForumId.ToString() + " class='group'>");
                    strForum.Append("<h3>" + objForum[i].ForumName + " </h3></li>");
                }
                strForum.Append("</ol></div>");
                lblList.Text = strForum.ToString();
            }
        }

        private void BindDirectores()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());//Guid.Parse(Request.QueryString["est"]);
                    ViewState["Entity"] = EntityId;


                    StringBuilder strDir = new StringBuilder("");
                    strDir.Append("<div id='accordion'><ol id='sort'>");
                    IList<KeyInformationDomain> objKey = KeyInformationDataProvider.Instance.GetKeyInfoByEntity(EntityId);
                    if (objKey.Count > 0)
                    {
                        for (int i = 0; i <= objKey.Count - 1; i++)
                        {
                            strDir.Append(" <li id=" + objKey[i].KeyInformationId.ToString() + " class='group'>");
                            strDir.Append("<h3>" + objKey[i].Suffix + " " + objKey[i].Name + " </h3><div>" + System.Net.WebUtility.HtmlDecode(objKey[i].Description) + "</div></li>");
                        }
                        strDir.Append("</ol></div>");
                        lblList.Text = strDir.ToString();
                    }
                }
                else
                {
                    Response.Redirect("~/login.aspx");
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    string UserId = Convert.ToString(Session["UserId"]);
                    StringBuilder strIds = new StringBuilder(",");
                    StringBuilder strOrder = new StringBuilder(",");
                    //ViewState["Entity"]
                    string idOrders = hdnListItems.Value;
                    if (idOrders.Length > 0)
                    {
                        // string sql = "";
                        idOrders = idOrders.Remove(idOrders.Length - 1);
                        string[] arryOrderIds = idOrders.Split(',');
                        for (int i = 1; i <= arryOrderIds.Count(); i++)
                        {
                            strOrder.Append(i + ",");
                            strIds.Append(arryOrderIds[i - 1] + ",");
                            //sql += " UPDATE Agenda set AgendaOrder= " + i + " WHERE AgendaId = '" + arryOrderIds[i - 1] + "'  UPDATE Agenda set AgendaOrder= " + i + " WHERE ParentAgendaId = '" + arryOrderIds[i - 1] + "' ";
                        }
                        //if (Request.QueryString["dir"] != null)
                        //{
                            bool bUpdates = KeyInformationDataProvider.Instance.UpdateKeyInfoOrders(strOrder.ToString(), strIds.ToString(), Guid.Parse(UserId));
                        //}

                        if (Request.QueryString["For"] != null)
                        {
                            bool bUpdate = ForumDataProvider.Instance.UpdateForumOrders(strOrder.ToString(), strIds.ToString(), Guid.Parse(UserId));
                        }
                        BindDirectores();
                    }
                    else
                    {
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
    }
}