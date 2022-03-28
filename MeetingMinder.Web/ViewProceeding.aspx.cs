using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;
using MM.Domain;
using System.IO;
using System.Security.Cryptography;

namespace MeetingMinder.Web
{
    public partial class ViewProceding : System.Web.UI.Page
    {
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
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
            }

        }

        protected void grdProceding_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void grdProceding_Sorting(object sender, GridViewSortEventArgs e)
        {

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
        /// Bind Meetings list to drop down
        /// </summary>
        /// <param name="strForumId">string specifying strForumId</param>
        private void BindMeeting(string strForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                //trMinutes.Visible = false;
                Guid forumId;
                if (Guid.TryParse(strForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting;
                    if (ddlYear.SelectedValue != "0")
                    {
                          objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).Where(p => DateTime.Parse(p.MeetingDate).Year == Convert.ToInt32(ddlYear.SelectedItem.Text)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                    }
                    else
                    {
                        objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

                    }
                    DateTime dtToday = DateTime.Now.Date;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);
                        //if (dtMeeting < dtToday)
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

        protected void ddlEntity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               

                string strForumId = ddlForum.SelectedValue;
                if (strForumId != "0")
                {
                    BindMeeting(strForumId);
                }
                //DataSet ds = ProcedingDataProvider.Instance.GetProcedingForView().Where;

                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    grdProceding.DataSource = ds;
                //    grdProceding.DataBind();
                //}
                //else
                //{
                //    grdProceding.DataSource = null;
                //    grdProceding.DataBind();
                //}
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMeetingId = ddlMeeting.SelectedValue;
            Guid ForumId = Guid.Empty;
            Guid MeetingId = Guid.Empty;
            //Session["MeetingId"] = strMeetingId;

            if (strMeetingId != "0")
            {
                
                

                if (ddlForum.SelectedValue != "0")
                {
                    ForumId = Guid.Parse(ddlForum.SelectedValue);
                }
                if (ddlMeeting.SelectedValue != "0")
                {
                    MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
                }
               

                grdProceding.DataSource = ProcedingDataProvider.Instance.Get(ForumId, MeetingId);
                grdProceding.DataBind();
            }
        }


        //private void BindSubAgenda(string strAgendaId)
        //{
        //    Guid agendaId;
        //    if (Guid.TryParse(strAgendaId, out agendaId))
        //    {
        //        ViewState["AgendaId"] = agendaId;
        //        Guid meetingId = Guid.Parse(ddlMeeting.SelectedValue.ToString());
        //        StringBuilder strMenu = new StringBuilder("");
        //        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

        //        if (objAgentList.Count > 0)
        //        {
        //            //hdnMeeting.Value = objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
        //            //    ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
        //            //get only parent agenda
        //            var objParentAgenda = from agend in objAgentList
        //                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                  select agend;

        //            //get only sub agenda
        //            var subAgendaList = from agend in objAgentList
        //                                where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                select agend;


        //            var subAgendaName = (from subAgenda in subAgendaList
        //                                 where (subAgenda.ParentAgendaId == agendaId)
        //                                 select (new
        //                                 {
        //                                     AgendaName = subAgenda.AgendaName,
        //                                     AgendaId = subAgenda.AgendaId,
        //                                     UplaodedAgenda = subAgenda.UploadedAgendaNote,
        //                                     AgendaNote = subAgenda.AgendaNote,
        //                                     DeletedAgenda = subAgenda.DeletedAgenda

        //                                 })).ToList();

        //            ddlSubAgenda.DataSource = subAgendaName;
        //            ddlSubAgenda.DataBind();
        //            ddlSubAgenda.DataTextField = "AgendaName";
        //            ddlSubAgenda.DataValueField = "AgendaId";
        //            ddlSubAgenda.DataBind();
        //            ddlSubAgenda.Items.Insert(0, new ListItem("Select Sub Agenda", "0"));
        //        }
        //    }
        //}


        //private void BindAgenda(string strMeetingId)
        //{
        //    Guid meetingId;
        //    if (Guid.TryParse(strMeetingId, out meetingId))
        //    {
        //        ViewState["MeetingId"] = meetingId;

        //        StringBuilder strMenu = new StringBuilder("");
        //        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingId(meetingId);

        //        if (objAgentList.Count > 0)
        //        {
        //            //hdnMeeting.Value = objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
        //            //    ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
        //            //get only parent agenda
        //            var objParentAgenda = from agend in objAgentList
        //                                  where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //                                  select agend;

        //            ddlAgenda.DataSource = objParentAgenda;
        //            ddlAgenda.DataBind();
        //            ddlAgenda.DataTextField = "AgendaName";
        //            ddlAgenda.DataValueField = "AgendaId";
        //            ddlAgenda.DataBind();
        //            ddlAgenda.Items.Insert(0, new ListItem("Select Agenda", "0"));
        //        }
        //    }
        //}


        //protected void ddlAgenda_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlSubAgenda.Items.Clear();
        //    ddlSubAgenda.Items.Insert(0, new ListItem("Select Sub Agenda", "0"));
        //    string strAgendaId = ddlAgenda.SelectedValue;
        //    // Session["AgendaId"] = strAgendaId;
        //    if (strAgendaId != "0")
        //    {
        //        BindSubAgenda(strAgendaId);
        //    }
        //}

        //protected void ddlSubAgenda_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Guid ForumId = Guid.Empty;
        //    Guid MeetingId = Guid.Empty;
        //    //Guid AgendaId = Guid.Empty;
        //    //Guid SubAgendaId = Guid.Empty;


        //    if (ddlForum.SelectedValue != "0")
        //    {
        //        ForumId = Guid.Parse(ddlForum.SelectedValue);
        //    }
        //    if (ddlMeeting.SelectedValue != "0")
        //    {
        //        MeetingId = Guid.Parse(ddlMeeting.SelectedValue);
        //    }
        //    //if (ddlAgenda.SelectedValue != "0")
        //    //{
        //    //    AgendaId = Guid.Parse(ddlAgenda.SelectedValue);
        //    //}
        //    //if (ddlSubAgenda.SelectedValue != "0")
        //    //{
        //    //    SubAgendaId = Guid.Parse(ddlSubAgenda.SelectedValue);
        //    //}


        //    grdProceding.DataSource = ProcedingDataProvider.Instance.Get(ForumId, MeetingId, AgendaId, SubAgendaId);
        //    grdProceding.DataBind();

        //}

        protected void grdProceding_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("view"))
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblEncrptionkey = (Label)row.Cells[0].FindControl("lblEncrptionkey");


                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["UploadMinutes"]);
                    string fileName = Convert.ToString(e.CommandArgument);

                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    byte[] decryptedBytes = null;
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(FilePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (RijndaelManaged rijndael = new RijndaelManaged())
                        {
                            byte[] ba = Encoding.Default.GetBytes(MM.Core.Encryptor.DecryptString(lblEncrptionkey.Text));
                            var hexString = BitConverter.ToString(ba);
                            hexString = hexString.Replace("-", "");

                            byte[] bsa = Encoding.Default.GetBytes("0000000000000000");
                            var hexStrings = BitConverter.ToString(bsa);
                            hexStrings = hexStrings.Replace("-", "");

                            rijndael.Mode = CipherMode.CBC;
                            rijndael.Padding = PaddingMode.PKCS7;
                            rijndael.KeySize = 256;
                            rijndael.BlockSize = 128;
                            rijndael.Key = EncryptionHelper.StringToByteArray(hexString);//"3030303030303030303030303030303030303030303030303030303030303030"); //prekey2; //key;

                            rijndael.IV = EncryptionHelper.StringToByteArray(hexStrings);//"30303030303030303030303030303030");//iv;

                            using (var cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                                cs.Close();
                            }
                            decryptedBytes = ms.ToArray();
                        }
                        //Set the appropriate ContentType.
                        Response.ContentType = "Application/pdf";
                        //Get the physical path to the file.
                        // string FilePath = Server.MapPath(savePath + fileName);

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "Proceeding.pdf");
                        // Response.WriteFile(FilePath);
                        Response.BinaryWrite(ms.ToArray());
                        Response.Flush();
                    }


                    //Write the file directly to the HTTP content output stream.
                    //Response.WriteFile(FilePath);
                    //Response.End();
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