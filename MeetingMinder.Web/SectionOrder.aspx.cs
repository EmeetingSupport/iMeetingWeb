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
    public partial class SectionOrder : System.Web.UI.Page
    {    /// <summary>
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

                lblListName.Text = "Section List";
                lblItemName.Text = "Section List";
                //Bind directores list.
                BindSections();
                //}

                
            }
        }

       
        private void BindSections()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EntityId"] != null)
                {
                    Guid ForumId = Guid.Parse(System.Web.HttpContext.Current.Session["ForumId"].ToString());//Guid.Parse(Request.QueryString["est"]);
                 
                    StringBuilder strDir = new StringBuilder("");
                    strDir.Append("<div id='accordion'><ol id='sort'>");
                    IList<SectionDomain>  objSection = SectionDataProvider.Instance.Get(ForumId);
                    if (objSection.Count > 0)
                    {
                        for (int i = 0; i <= objSection.Count - 1; i++)
                        {
                            strDir.Append(" <li id=" + objSection[i].SectionId.ToString() + " class='group'>");
                            strDir.Append("<h3>" + objSection[i].SerialNumber + " " + objSection[i].Title + " </h3></li>");
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
                        bool bUpdates = SectionDataProvider.Instance.UpdateSectionOrders(strOrder.ToString(), strIds.ToString(), Guid.Parse(UserId));
                        //}

                     
                        BindSections();
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SectionMaster.aspx");

        }
    }
}