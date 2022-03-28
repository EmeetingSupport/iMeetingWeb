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
using System.Web.Services;
using System.Text;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Security.Cryptography;


namespace MeetingMinder.Web
{
    public partial class CreateAgenda : System.Web.UI.Page
    {
        private string GetPostBackControl()
        {
            string Outupt = "";

            //get the __EVENTTARGET of the Control that cased a PostBack(except Buttons and ImageButtons)
            string targetCtrl = Page.Request.Params.Get("__EVENTTARGET");
            if (targetCtrl != null && targetCtrl != string.Empty)
            {
                Outupt = targetCtrl + " button make Postback";
            }
            else
            {
                //if button is cased a postback
                foreach (string str in Request.Form)
                {
                    Control Ctrl = Page.FindControl(str);
                    if (Ctrl is Button)
                    {
                        Outupt = Ctrl.ID;
                        break;
                    }
                }
            }
            return Outupt;
        }
        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            Info.Visible = false;
            Error.Visible = false;
            lblAlert.Text = "";
            Page.MaintainScrollPositionOnPostBack = true;
            if (!IsPostBack)
            {
                //bind meeting list
                //     BindCheckers();

                ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));

                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                //Bind Entity List
                //BindEntity();

                //Bind All Classification
                BindClassification();

                //Bind agenda meeting dropdown
                BindAgendaMeeting();

                //Bind Agenda
                BindAgenda();

                //Bind Meeting
                BindMeetingS();

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    lnkPublish.Visible = false;
                }

                //Bind all agenda items
                //  BindAllAgenda();

                //Bind Presenter
                //BindPresenter();

                //if (Request.QueryString["id"] != null)
                //{
                if (Request.UrlReferrer != null)
                {
                    string prvPage = Request.UrlReferrer.ToString();

                    Guid MeetingId = Guid.Empty;
                    if (prvPage.ToLower().EndsWith("agendamaster.aspx") || prvPage.ToLower().EndsWith("createsubagenda.aspx"))
                    {
                        try
                        {
                            if (Guid.TryParse(Convert.ToString(Session["AgendaOrder"]), out MeetingId))//(Guid.TryParse(Request.QueryString["id"].ToString(), out MeetingId))
                            {
                                ListItem item = ddlAgendaMeeting.Items.FindByValue(MeetingId.ToString());
                                if (item != null)
                                {
                                    ddlAgendaMeeting.SelectedValue = MeetingId.ToString();
                                    //.Items.FindByValue(MeetingId.ToString()).Selected = true;
                                    ddlAgendaMeeting_SelectedIndexChanged(sender, e);
                                }

                            }
                            //this.Request.QueryString.Remove("id");
                        }
                        catch (Exception ex)
                        {
                            //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                            //Error.Visible = true;

                            LogError objEr = new LogError();
                            objEr.HandleException(ex);

                            //                      PropertyInfo isreadonly =
                            //typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
                            //"IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                            //                      // make collection editable
                            //                      isreadonly.SetValue(this.Request.QueryString, false, null);
                            //                      this.Request.QueryString.Remove("id");
                        }
                    }
                }
                //}
            }
        }

        /// <summary>
        /// Bind all agenda items
        /// </summary>
        private void BindAllAgenda(Guid ForumId)
        {
            try
            {
                IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAllAgendaTitles(ForumId);//.OrderBy(p => p.AgendaName).ToList(); ;
                ddlMaster.DataSource = objAgenda;
                ddlMaster.DataBind();
                ddlMaster.DataTextField = "AgendaName";
                ddlMaster.DataBind();
                ddlMaster.Items.Insert(0, new ListItem("Select ", "0"));
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
        /// Bind all agenda Classification
        /// </summary>
        private void BindClassification()
        {
            try
            {
                Guid EntityId = Guid.Parse(Session["EntityId"].ToString());
                IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAllMasterClassification(EntityId).OrderBy(p => p.AgendaName).ToList(); ;
                ddlClassification.DataSource = objAgenda;
                ddlClassification.DataBind();
                ddlClassification.DataTextField = "AgendaName";
                ddlClassification.DataBind();
                ddlClassification.Items.Insert(0, new ListItem("Select ", "0"));
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
        /// Bind approved meetings
        /// </summary>
        private void BindMeetingS()
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

                string UserId = Session["UserId"].ToString();
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByUserAccess(Guid.Parse(UserId)).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();
                //IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetApprovedMeetingByUser(Guid.Parse(UserId)); //MeetingDataProvider.Instance.GetMeetingByFroumID(forumId);

                DateTime dtToday = DateTime.Now;

                foreach (MeetingDomain item in objMeeting)
                {
                    DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);//+ " " + item.MeetingTime);
                    if (dtMeeting.AddDays(1).Date >= dtToday.Date)
                    {
                        //  ddlMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime + ' ' + item.ForumName, item.MeetingId.ToString()));
                        ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));
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


        //private void BindPresenter()
        //{
        //    try
        //    {
        //        ddlPresenter.Items.Clear();
        //        ddlPresenter.Items.Insert(0, new ListItem("Select Presenter", "0"));


        //        IList<UserDomain> objUserList = UserDataProvider.Instance.Get();
        //        //IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetApprovedMeetingByUser(Guid.Parse(UserId)); //MeetingDataProvider.Instance.GetMeetingByFroumID(forumId);

        //        foreach (UserDomain objUser in objUserList)
        //        {
        //            ddlPresenter.Items.Add(objUser.UserName);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
        //        Error.Visible = true;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);
        //    }

        //}

        /// <summary>
        /// Bind meeting list to drop down
        /// </summary>
        private void BindAgendaMeeting()
        {
            try
            {
                ddlAgendaMeeting.Items.Clear();
                ddlAgendaMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByUserAccess(UserId).OrderBy(p => p.MeetingDate).ToList(); ;
                //IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByUser(UserId);
                if (objMeeting.Count > 0)
                {
                    DateTime dtToday = DateTime.Now;
                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate);// + " " + item.MeetingTime);
                        if (dtMeeting.AddDays(1).Date >= dtToday.Date)
                        {
                            //ddlAgendaMeeting.Items.Add(new ListItem(dtMeeting.ToString("dd MMM yyyy") + ' ' + item.MeetingVenue + ' ' + item.MeetingTime + ' ' + item.ForumName, item.MeetingId.ToString()));
                            ddlAgendaMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));

                        }
                    }
                }
                else
                {
                    ddlAgendaMeeting.Items.Clear();
                    ddlAgendaMeeting.Items.Insert(0, new ListItem("No pending agenda", "0"));
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
        /// Bind agenda items
        /// </summary>
        private void BindAgenda()
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                string MeetingsId = ddlAgendaMeeting.SelectedValue;

                // IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByUser(UserId);
                // if (objMeeting.Count > 0)
                if (!MeetingsId.Equals("0"))
                {
                    if (!string.IsNullOrEmpty(MeetingsId))
                    {
                        Guid MeetingId = Guid.Parse(MeetingsId);
                        IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAgendabyMeetingId(MeetingId);
                        if (objAgenda.Count > 0)
                        {
                            IList<AgendaDomain> objParentAgenda = (from agend in objAgenda
                                                                   where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                                   select agend).ToList();


                            IList<AgendaDomain> objAgendaWithChild = (from agend in objAgenda
                                                                      where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                                      select agend).ToList();

                            var joints = from q in objAgendaWithChild
                                         join p in objParentAgenda on q.ParentAgendaId equals p.AgendaId

                                         select q;


                            var query = (from ParentAgenda in objParentAgenda
                                         join SubAgenda in joints on ParentAgenda.AgendaId equals SubAgenda.ParentAgendaId into gj
                                         from subpet in gj.DefaultIfEmpty()
                                         select new
                                         {
                                             ParentAgenda.AgendaChecker,
                                             ParentAgenda.AgendaId,
                                             ParentAgenda.AgendaName,
                                             ParentAgenda.AgendaNote,
                                             ParentAgenda.AgendaOrder,
                                             ParentAgenda.CheckerName,
                                             ParentAgenda.Classification,
                                             ParentAgenda.CreatedBy,
                                             ParentAgenda.CreatedOn,
                                             ParentAgenda.DeletedAgenda,
                                             ParentAgenda.EntityId,
                                             ParentAgenda.EntityName,
                                             ParentAgenda.ForumId,
                                             ParentAgenda.ForumName,
                                             ParentAgenda.IsPublished,
                                             ParentAgenda.MeetingDate,
                                             ParentAgenda.MeetingId,
                                             ParentAgenda.MeetingTime,
                                             ParentAgenda.MeetingVenue,
                                             ParentAgenda.Minutes,
                                             ParentAgenda.ParentAgendaId,
                                             ParentAgenda.PublishedBy,
                                             ParentAgenda.UpdatedBy,
                                             ParentAgenda.UpdateOn,
                                             ParentAgenda.UploadedAgendaNote,
                                             ParentAgenda.SerialNumber,
                                             HasChild = (subpet == null ? String.Empty : " <b style='color:green; top:1px;' title='This item has sub agenda'>+</b>")
                                         }).Distinct();

                            grdAgenda.DataSource = query.AsDataTable(); //objParentAgenda.AsDataTable();
                            grdAgenda.DataBind();



                            //Store date and time for avoid past editing
                            ViewState["MeetingTime"] = objAgenda[0].MeetingTime;
                            ViewState["MeetingDate"] = objAgenda[0].MeetingDate;



                        }
                        else
                        {
                            grdAgenda.DataSource = null;
                            grdAgenda.DataBind();
                        }
                        //  hylnkAgenda.NavigateUrl = "AgendaMaster.aspx?id=" + MeetingId;
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
        /// Bind Checkers List to drop down
        /// </summary>
        private void BindCheckers(Guid ForumId)
        {
            try
            {
                DataColumn FullName = new DataColumn();
                FullName.ColumnName = "FullName";
                FullName.DataType = System.Type.GetType("System.String");
                FullName.Expression = "Suffix+' '+FirstName+' '+LastName";
                IList<UserDomain> objUser = UserDataProvider.Instance.GetAllChecker(ForumId).OrderBy(p => p.FirstName).ToList();
                if (objUser.Count > 0)
                {
                    DataTable dt = objUser.AsDataTable();
                    dt.Columns.Add(FullName);
                    ddlUser.DataSource = dt;
                    ddlUser.DataBind();
                    ddlUser.DataValueField = "UserId";
                    ddlUser.DataTextField = "FullName";
                    ddlUser.DataBind();
                }

                //Remove logged in users id from drop down
                string UserId = Convert.ToString(Session["UserId"]);
                ListItem UserIdToRemove = ddlUser.Items.FindByValue(UserId);
                ddlUser.Items.Remove(UserIdToRemove);

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

        /// <summary>
        /// Get Agenda titles for auto complete
        /// </summary>
        /// <param name="strSearch">string specifying strSearch</param>
        /// <returns></returns>
        [WebMethod]
        public static List<string> GetAgendaTitles(string strSearch)
        {
            try
            {
                IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAgendaTitles(strSearch);
                var strAgendaNote = (from p in objAgenda

                                     select p.AgendaName).Take(5);
                return strAgendaNote.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Bind Entity list to drop down
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
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
        /// drop down list change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void ddlMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strEntityId = ddlMeeting.SelectedValue;
                if (strEntityId != "0")
                {
                    MeetingDomain objMeeting = MeetingDataProvider.Instance.GetMeetingWithEntity(Guid.Parse(ddlMeeting.SelectedValue));
                    if (objMeeting != null)
                    {
                        lblEntity.Text = objMeeting.EntityName;
                        lblForum.Text = objMeeting.ForumName;
                        lblForum1.Text = objMeeting.ForumName;
                        ViewState["entityId"] = objMeeting.EntityId;

                        BindAllAgenda(objMeeting.ForumId);
                        BindCheckers(objMeeting.ForumId);
                        //Store date and time for avoid past editing
                        ViewState["MeetingTimeAdd"] = objMeeting.MeetingTime;
                        ViewState["MeetingDateAdd"] = objMeeting.MeetingDate;

                        if (ddlAgendaMeeting.SelectedValue != ddlMeeting.SelectedValue)
                        {
                            ddlAgendaMeeting.SelectedValue = ddlMeeting.SelectedValue;
                            ddlAgendaMeeting_SelectedIndexChanged(sender, e);
                            lnkSendApproval.Visible = true;
                            hylnkAgenda.Visible = true;
                            lnkPublish.Visible = true;
                        }
                    }
                    else
                    {
                        ViewState["entityId"] = null;
                    }
                }
                else
                {
                    ddlAgendaMeeting.SelectedValue = "0";
                    grdAgenda.DataSource = null;
                    grdAgenda.DataBind();
                    lnkSendApproval.Visible = false;
                    hylnkAgenda.Visible = false;
                    lnkPublish.Visible = false;
                }

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    lnkPublish.Visible = false;
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

                Guid entityId;
                if (Guid.TryParse(EntityId, out entityId))
                {
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(entityId);
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
        /// Insert Agenda details
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();

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
                    if (!objUser.CSVValidation(txtTitle.Text))
                    {
                        txtTitle.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid agenda title.";
                        Error.Visible = true;
                        return;
                    }

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter agenda title.";
                    Error.Visible = true;
                    return;
                }

                if (ddlAgendaMeeting.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select meeting.";
                    Error.Visible = true;
                    return;
                }


                if (txtPresenter.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtPresenter);
                    if (!objUser.isValidChar(txtPresenter.Text))
                    {
                        txtPresenter.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtPresenter.Text))
                    {
                        txtPresenter.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid presenter.";
                        Error.Visible = true;
                        return;
                    }
                }

                if (txtSerialNumber.Text != "")
                {                    
                    if (!objUser.CSVValidation(txtSerialNumber.Text))
                    {
                        txtSerialNumber.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                DateTime dtNow = DateTime.Now;
                DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDateAdd"]));// + " " + Convert.ToString(ViewState["MeetingTimeAdd"]));
                //if (dtMeetingDate.Date < dtNow.Date)
                //{
                //    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't add agenda to past meetings. ";
                //    Error.Visible = true;
                //    return;
                //}

                string UploadOn = "";
                string AgendaFile = "";
                string AgendaFileInsert = "";
                AgendaDomain objAgenda = new AgendaDomain();
                objAgenda.AgendaName = txtTitle.Text;
                objAgenda.AgendaNote = txtAgendaNote.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.Presenter = txtPresenter.Text;
                if (ddlClassification.SelectedValue != "0")
                {
                    objAgenda.Classification = ddlClassification.SelectedItem.Text;
                }
                else
                {
                    objAgenda.Classification = "";
                }
                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ddlMeeting.SelectedValue));
                if (ddlUser.SelectedValue != "0")
                {
                    objAgenda.AgendaChecker = Guid.Parse(ddlUser.SelectedValue);
                }

                StringBuilder sbAgendaNames = new StringBuilder("");
                StringBuilder sbPdfNames = new StringBuilder("");
                string agendaName;
                bool isUplaoded = false;

                string EncryptionKey = Session["EncryptionKey"].ToString();
                if (hdnAgendaId.Value != "" && hdnAgendaId.Value.Length == 36)
                {
                    System.Data.DataSet ds = AgendaDataProvider.Instance.GetAgendKeys(Guid.Parse(hdnAgendaId.Value));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        EncryptionKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                    }
                }
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Files.Count - 1; i++)
                    {
                        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                        string contentType = Request.Files[i].ContentType;
                        if (Request.Files[i].ContentLength > 0)
                        {
                            if (!objUser.isValidChar_old(Request.Files[i].FileName))
                            {
                                ((Label)Error.FindControl("lblError")).Text = " Uploaded file name contains Special characters.";
                                Error.Visible = true;
                                return;
                            }

                            if (contentType.ToLower().Contains("application/pdf"))
                            {
                                string ext = Path.GetExtension(Request.Files[i].FileName);
                                if (ext.ToLower().Equals(".pdf"))
                                {
                                    string pdfPassword;
                                    isUplaoded = true;
                                    agendaName = EncryptionHelper.GetPdfName(out pdfPassword) + ext;
                                    //Guid.NewGuid() + ext; //14 Oct 2014
                                    //DateTime.Now.Ticks + ext;  //sep 2014
                                    // Request.Files[i].SaveAs(Server.MapPath(savePath + agendaName));

                                    using (Stream input = Request.Files[i].InputStream)//new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                                    {
                                        using (Stream output = new FileStream(Server.MapPath(savePath + agendaName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                                        {
                                            PdfReader reader = new PdfReader(input);
                                            PdfEncryptor.Encrypt(reader, output, true, pdfPassword, pdfPassword, PdfWriter.ALLOW_MODIFY_ANNOTATIONS);

                                        }
                                    }

                                    EncryptionHelper.EncryptString(Server.MapPath(savePath + agendaName), EncryptionKey);
                                    //     objAgenda.UploadedAgendaNote = agendaName;
                                    AgendaFileInsert = Request.Files[i].FileName;
                                    AgendaFile = AgendaFileInsert;
                                    sbAgendaNames.Append("");
                                    //Delete existing image
                                    //if (ViewState["file"] != null)
                                    //{

                                    //}
                                    sbAgendaNames.Append(agendaName + ",");
                                    sbPdfNames.Append(AgendaFileInsert + ",");
                                }

                                else
                                {
                                    ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
                                    Error.Visible = true;
                                    return;
                                }
                            }
                            else
                            {
                                ((Label)Error.FindControl("lblError")).Text = "Invalid file Uploaded";
                                Error.Visible = true;
                                return;
                            }
                        }
                    }

                    if (isUplaoded)
                    {
                        sbAgendaNames.Remove(sbAgendaNames.ToString().LastIndexOf(','), 1);
                        sbPdfNames.Remove(sbPdfNames.ToString().LastIndexOf(','), 1);

                        objAgenda.UploadedAgendaNote = sbAgendaNames.ToString();
                        objAgenda.AgendaNote = sbPdfNames.ToString();
                        //sbAgendaNames.ToString().TrimEnd(',');// (sbAgendaNames.ToString().LastIndexOf(','));
                        //sbPdfNames.ToString().TrimEnd(',');
                        UploadOn = DateTime.Now.ToString();
                    }
                    if (ViewState["file"] != null)
                    {
                        string Files = Convert.ToString(ViewState["file"]);
                        string pdfs = Convert.ToString(ViewState["pdf"]);
                        if (sbAgendaNames.ToString() != "")
                        {
                            //keep old/previously uploaded file names to starting position
                            string tempAgendaName = Files + "," + sbAgendaNames.ToString();
                            string tempPdfNames = pdfs + "," + sbPdfNames.ToString();

                            sbAgendaNames = new StringBuilder("");
                            sbPdfNames = new StringBuilder("");

                            sbAgendaNames.Append(tempAgendaName);
                            sbPdfNames.Append(tempPdfNames);
                        }
                        else
                        {
                            sbAgendaNames.Append(Files);
                            sbPdfNames.Append(pdfs);
                        }


                        objAgenda.UploadedAgendaNote = sbAgendaNames.ToString();
                        objAgenda.AgendaNote = sbPdfNames.ToString();
                    }
                }
                else if (ViewState["file"] != null)
                {
                    string Files = Convert.ToString(ViewState["file"]);
                    string pdfs = Convert.ToString(ViewState["pdf"]);
                    //sbAgendaNames.Append("," + Files);
                    //sbPdfNames.Append("," + pdfs);

                    objAgenda.UploadedAgendaNote = Files; //sbAgendaNames.ToString();
                    objAgenda.AgendaNote = pdfs;// sbPdfNames.ToString();
                }


                //if (fuAgenda.HasFile)
                //{
                //    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                //    string contentType = fuAgenda.PostedFile.ContentType;

                //    string ext = Path.GetExtension(fuAgenda.FileName);
                //    if (ext.ToLower().Equals(".pdf"))
                //    {
                //        agendaName = DateTime.Now.Ticks + ext;
                //        fuAgenda.PostedFile.SaveAs(Server.MapPath(savePath + agendaName));

                //        objAgenda.UploadedAgendaNote = agendaName;
                //        AgendaFileInsert = fuAgenda.FileName;
                //        AgendaFile = AgendaFileInsert;
                //        //Delete existing image
                //        if (ViewState["file"] != null)
                //        {
                //            if (hdnDelete.Value == "delete")
                //            {
                //                AgendaFile = "";
                //                string fileName = Convert.ToString(ViewState["file"]);
                //                File.Delete(Server.MapPath(savePath + fileName));
                //            }
                //            if (hdnDelete.Value == "don't")
                //            {
                //                AgendaFile = fuAgenda.FileName;
                //            }

                //            ViewState["file"] = null;
                //        }
                //    }
                //    else
                //    {
                //        ((Label)Error.FindControl("lblError")).Text = "File format must be pdf";
                //        Error.Visible = true;
                //        Info.Visible = false;
                //        return;
                //    }
                //}

                objAgenda.CreatedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.SerialNumber = "";
                //if (ddlSerialNumber.SelectedValue != "0")
                //{
                //    objAgenda.SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                //}
                objAgenda.SerialNumber = txtSerialNumber.Text.Trim();

                //if (objAgenda.CreatedBy == objAgenda.AgendaChecker)
                //{
                //    ((Label)Error.FindControl("lblError")).Text = "Checker and Maker cannot be same ";
                //    Error.Visible = true;
                //    Info.Visible = false;
                //    return;
                //}

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    if (isUplaoded)
                    {
                        List<string> strPdfsList = new List<string>();
                        string[] arrDeletedpdf = new string[] { };
                        string[] arrAgendaFiles = objAgenda.UploadedAgendaNote.Split(',');

                        if (ViewState["delFile"] != null)
                        {
                            arrDeletedpdf = Convert.ToString(ViewState["delFile"]).Split(',');
                        }

                        for (int pdfUp = 0; pdfUp <= arrAgendaFiles.Count() - 1; pdfUp++)
                        {
                            if (!string.IsNullOrWhiteSpace(arrAgendaFiles[pdfUp]) && !arrDeletedpdf.Contains(arrAgendaFiles[pdfUp].Trim()))
                            {
                                strPdfsList.Add(arrAgendaFiles[pdfUp].Trim());
                            }
                        }
                        bool status = MergePdf(strPdfsList, arrAgendaFiles[0].Trim());
                        if (!status)
                        {
                            return;
                        }
                    }

                    //Update
                    if (hdnAgendaId != null && hdnAgendaId.Value != "")
                    {
                        //if (ViewState["file"] != null)
                        //{
                        //    string fileName = Convert.ToString(ViewState["file"]);
                        //    objAgenda.UploadedAgendaNote = fileName;
                        //}
                        if (ViewState["delFile"] != null)
                        {
                            AgendaFile = Convert.ToString(ViewState["delFile"]);
                        }

                        objAgenda.AgendaId = Guid.Parse(hdnAgendaId.Value);
                        bool status = AgendaDataProvider.Instance.UpdateKey(objAgenda, AgendaFile, "", "", EncryptionKey, UploadOn);
                        if (ddlUser.SelectedValue != "0")
                        {
                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Pending", Convert.ToString(objAgenda.AgendaId), "Agenda updated successfully and sent for approval :- " + encrypt + "");

                            ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully and sent for approval";
                        }
                        else
                        {
                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Success", Convert.ToString(objAgenda.AgendaId), "Agenda updated successfully :- " + encrypt + "");

                            ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully";
                        }
                        BindAgendaMeeting();
                        ddlAgendaMeeting.SelectedValue = ddlMeeting.SelectedValue;
                        BindAgenda();
                        Info.Visible = true;
                        ClearData();
                    }
                    //Insert
                    else
                    {

                        //check add permission
                        if (objUser.IsAdd(Guid.Parse(Session["EntityId"].ToString())))
                        {
                            AgendaDataProvider.Instance.Insert(objAgenda, AgendaFileInsert);

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                            var encrypt = Encryptor.EncryptString(json);

                            if (ddlUser.SelectedValue != "0")
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Agenda inserted successfully and sent for approval";
                                Info.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Pending", Convert.ToString(objAgenda.AgendaId), "Agenda inserted successfully and sent for approval :- " + encrypt + "");
                            }
                            else
                            {
                                ((Label)Info.FindControl("lblName")).Text = "Agenda inserted successfully";
                                Info.Visible = true;

                                //web logs
                                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Success", Convert.ToString(objAgenda.AgendaId), "Agenda inserted successfully :- " + encrypt + "");

                            }
                            BindAgendaMeeting();
                            ddlAgendaMeeting.SelectedValue = ddlMeeting.SelectedValue;
                            BindAgenda();

                            lnkSendApproval.Visible = true;
                            hylnkAgenda.Visible = true;
                            lnkPublish.Visible = true;

                            bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                            bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                            if (IsMaker && !IsChecker)
                            {
                                lnkPublish.Visible = false;
                            }
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Failed", Convert.ToString(objAgenda.AgendaId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            Info.Visible = false;
                        }
                    }
                    ClearData();
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Failed", Convert.ToString(objAgenda.AgendaId), "Sorry you are not maker");

                    ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                    Error.Visible = true;
                    Info.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Edit agenda details
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e</param>
        private void EditAgenda(Guid AgendaId, string MeetingId, object sender, EventArgs e)
        {
            ClearData();
            AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(AgendaId);
            //ddlEntity.SelectedValue = strId[3];
            //ddlEntity_SelectedIndexChanged(sender, e);

            //ddlForum.SelectedValue = strId[2];
            //ddlForum_SelectedIndexChanged(sender, e);

            ddlMeeting.SelectedValue = MeetingId;
            ddlMeeting_SelectedIndexChanged(sender, e);
            hdnAgendaId.Value = objAgenda.AgendaId.ToString();

            ddlClassification.SelectedIndex = -1;
            ListItem item = ddlClassification.Items.FindByText(objAgenda.Classification);
            if (item != null)
            {
                ddlClassification.Items.FindByText(objAgenda.Classification).Selected = true;
            }


            txtAgendaNote.Text = objAgenda.AgendaNote;
            txtTitle.Text = objAgenda.AgendaName;
            txtPresenter.Text = objAgenda.Presenter;
            //ddlSerialNumber.SelectedValue = objAgenda.SerialNumber;
            txtSerialNumber.Text = objAgenda.SerialNumber;

            //if (objAgenda.AgendaChecker != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            //{
            // ddlUser.SelectedValue = Convert.ToString(objAgenda.AgendaChecker);
            //}

            if (ddlUser.Items.FindByValue(
Convert.ToString(objAgenda.AgendaChecker)) != null)
            {
                ddlUser.SelectedValue = objAgenda.AgendaChecker.ToString();
                chkChecker.Checked = true;
            }
            else
            {
                ddlUser.SelectedValue = "0";
                chkChecker.Checked = false;
            }

            //Uploaded agendas
            if (objAgenda.UploadedAgendaNote != null)
            {
                if (objAgenda.UploadedAgendaNote.Length > 0)
                {
                    string[] strDeletedAgenda = objAgenda.DeletedAgenda.Split(',');
                    StringBuilder sbAgenda = new StringBuilder("");
                    string[] agendaNames = objAgenda.UploadedAgendaNote.Split(',');
                    string[] agendaPdfs = objAgenda.AgendaNote.Split(',');
                    if (objAgenda.UploadedAgendaNote != "")
                    {
                        ViewState["file"] = objAgenda.UploadedAgendaNote;
                    }

                    if (objAgenda.AgendaNote != "")
                    {
                        ViewState["pdf"] = objAgenda.AgendaNote;
                    }

                    if (objAgenda.DeletedAgenda != "")
                    {
                        ViewState["delFile"] = objAgenda.DeletedAgenda;
                    }
                    for (int i = 0; i <= agendaNames.Count() - 1; i++)
                    {
                        if (!strDeletedAgenda.Contains(agendaNames[i].Trim()))
                        {
                            // sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                            sbAgenda.Append(" <a href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        }

                    }

                    lblPdfs.Text = sbAgenda.ToString();
                    lnkViewPdf.Visible = true;
                }
            }
            btnSubmit.Text = "Update";
            ClientScript.RegisterStartupScript(this.GetType(), "success", "showChecker();", true);
        }


        /// <summary>
        /// Button cancel click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// Clear form data 
        /// </summary>
        private void ClearData()
        {
            ddlUser.SelectedIndex = -1;

            //ddlMeeting.SelectedValue = "0";
            // ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));

            ddlForum.Items.Clear();
            ddlForum.Items.Insert(0, new ListItem("Select Forum", "0"));
            ddlEntity.SelectedValue = "0";
            hdnAgendaId.Value = "";
            txtTitle.Text = "";
            txtAgendaNote.Text = "";
            btnSubmit.Text = "Submit";
            ViewState["file"] = null;
            ViewState["pdf"] = null;
            lnkViewPdf.Visible = false;
            hdnDelete.Value = "";
            chkChecker.Checked = false;
            lblPdfs.Text = "";
            ddlClassification.SelectedIndex = -1;
            txtPresenter.Text = "";
            // ddlSerialNumber.SelectedIndex = -1;
            txtSerialNumber.Text = "";


            ViewState["delFile"] = null;
        }

        /// <summary>
        /// drop down list  Selected Index Change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void ddlForum_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strForumId = ddlForum.SelectedValue;
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
        /// BInd Meeting list to drop down
        /// </summary>
        /// <param name="ForumId">string specifying ForumId</param>
        private void BindMeeting(string ForumId)
        {
            try
            {
                ddlMeeting.Items.Clear();
                ddlMeeting.Items.Insert(0, new ListItem("Select Meeting", "0"));
                Guid forumId;
                if (Guid.TryParse(ForumId, out forumId))
                {
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingForAgenda(forumId); //MeetingDataProvider.Instance.GetMeetingByFroumID(forumId);

                    DateTime dtToday = DateTime.Now;

                    foreach (MeetingDomain item in objMeeting)
                    {
                        DateTime dtMeeting = Convert.ToDateTime(item.MeetingDate + " " + item.MeetingTime);
                        if (dtMeeting.AddDays(1).Date >= dtToday.Date)
                        {
                            ddlMeeting.Items.Add(new ListItem(item.MeetingNumber + ' ' + item.ForumName + ' ' + dtMeeting.ToString("MMMM dd, yyyy"), item.MeetingId.ToString()));
                        }
                    }


                    //ddlMeeting.DataSource = objMeeting;
                    //ddlMeeting.DataBind();

                    ////Past date ........
                    //ddlMeeting.DataTextField = "MeetingDate";
                    //ddlMeeting.DataValueField = "MeetingId";
                    //ddlMeeting.DataBind();


                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Invalid meeting search";
                    Error.Visible = true;
                    Info.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Grid view rowcommand event 
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgendaRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("download"))
                {
                    string fileName = Convert.ToString(e.CommandArgument);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                    //Set the appropriate ContentType.
                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    //Write the file directly to the HTTP content output stream.

                    ////FileStream sourceFile = new FileStream(Server.MapPath(@"FileName"), FileMode.Open);
                    ////float FileSize;
                    ////FileSize = sourceFile.Length;
                    ////byte[] getContent = new byte[(int)FileSize];
                    ////sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    ////sourceFile.Close();
                    ////Response.ClearContent();
                    ////Response.ClearHeaders();
                    ////Response.Buffer = true;
                    ////Response.ContentType = ReturnExtension(file.Extension.ToLower());
                    ////Response.AddHeader("Content-Length", getContent.Length.ToString());
                    ////Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
                    ////Response.BinaryWrite(getContent);
                    ////Response.Flush();
                    ////Response.End();

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.WriteFile(FilePath);
                    Response.End();
                }
                if (e.CommandName.ToLower().Equals("editagenda"))
                {
                    DateTime dtNow = DateTime.Now;
                    DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]));// + " " + Convert.ToString(ViewState["MeetingTime"]));
                    if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry you can't edit past meetings agenda. ";
                        Error.Visible = true;
                        return;
                    }
                    string strIds = Convert.ToString(e.CommandArgument);
                    string[] strId = strIds.Split(',');
                    //check edit permission
                    if (objUser.IsEdit(Guid.Parse(strId[3])))
                    {
                        EditAgenda(Guid.Parse(strId[0]), strId[1], sender, e);
                        ////                        AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(Guid.Parse(strId[0]));
                        ////                        //ddlEntity.SelectedValue = strId[3];
                        ////                        //ddlEntity_SelectedIndexChanged(sender, e);

                        ////                        //ddlForum.SelectedValue = strId[2];
                        ////                        //ddlForum_SelectedIndexChanged(sender, e);

                        ////                        ddlMeeting.SelectedValue = strId[1];
                        ////                        ddlMeeting_SelectedIndexChanged(sender, e);
                        ////                        hdnAgendaId.Value = objAgenda.AgendaId.ToString();

                        ////                        ddlClassification.SelectedIndex = -1;
                        ////                        ListItem item = ddlClassification.Items.FindByText(objAgenda.Classification);
                        ////                        if (item != null)
                        ////                        {
                        ////                            ddlClassification.Items.FindByText(objAgenda.Classification).Selected = true;
                        ////                        }


                        ////                        txtAgendaNote.Text = objAgenda.AgendaNote;
                        ////                        txtTitle.Text = objAgenda.AgendaName;

                        ////                        //if (objAgenda.AgendaChecker != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        ////                        //{
                        ////                        // ddlUser.SelectedValue = Convert.ToString(objAgenda.AgendaChecker);
                        ////                        //}

                        ////                        if (ddlUser.Items.FindByValue(
                        ////Convert.ToString(objAgenda.AgendaChecker)) != null)
                        ////                        {
                        ////                            ddlUser.SelectedValue = objAgenda.AgendaChecker.ToString();
                        ////                            chkChecker.Checked = true;
                        ////                        }
                        ////                        else
                        ////                        {
                        ////                            ddlUser.SelectedValue = "0";
                        ////                            chkChecker.Checked = false;
                        ////                        }

                        ////                        //Uploaded agendas
                        ////                        if (objAgenda.UploadedAgendaNote != null)
                        ////                        {
                        ////                            if (objAgenda.UploadedAgendaNote.Length > 0)
                        ////                            {
                        ////                                string[] strDeletedAgenda = objAgenda.DeletedAgenda.Split(',');
                        ////                                StringBuilder sbAgenda = new StringBuilder("");
                        ////                                string[] agendaNames = objAgenda.UploadedAgendaNote.Split(',');
                        ////                                string[] agendaPdfs = objAgenda.AgendaNote.Split(',');
                        ////                                if (objAgenda.UploadedAgendaNote != "")
                        ////                                {
                        ////                                    ViewState["file"] = objAgenda.UploadedAgendaNote;
                        ////                                }

                        ////                                if (objAgenda.AgendaNote != "")
                        ////                                {
                        ////                                    ViewState["pdf"] = objAgenda.AgendaNote;
                        ////                                }

                        ////                                if (objAgenda.DeletedAgenda != "")
                        ////                                {
                        ////                                    ViewState["delFile"] = objAgenda.DeletedAgenda;
                        ////                                }
                        ////                                for (int i = 0; i <= agendaNames.Count() - 1; i++)
                        ////                                {
                        ////                                    if (!strDeletedAgenda.Contains(agendaNames[i].Trim()))
                        ////                                    {
                        ////                                        // sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        ////                                        sbAgenda.Append(" <a href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        ////                                    }

                        ////                                }

                        ////                                lblPdfs.Text = sbAgenda.ToString();
                        ////                                lnkViewPdf.Visible = true;
                        ////                            }
                        ////                        }
                        ////                        btnSubmit.Text = "Update";
                        ////                        ClientScript.RegisterStartupScript(this.GetType(), "success", "showChecker();", true);
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                if (e.CommandName.ToLower().Equals("add"))
                {
                    Session["AgendaId"] = Convert.ToString(e.CommandArgument);
                    Response.Redirect("CreateSubAgenda.aspx");
                }
                if (e.CommandName.ToLower().Equals("delete"))
                {

                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        DateTime dtNow = DateTime.Now;
                        DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]));// + " " + Convert.ToString(ViewState["MeetingTime"]));
                        if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry you can't delete past meetings agenda. ";
                            Error.Visible = true;
                            return;
                        }


                        string AgendaId = Convert.ToString(e.CommandArgument);
                        string[] ids = AgendaId.Split(',');
                        //check delete permission
                        if (objUser.isDelete(Guid.Parse(ids[1])))
                        {
                            AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(Guid.Parse(ids[0]));
                            bool status = AgendaDataProvider.Instance.Delete(Guid.Parse(ids[0]));
                            ((Label)Info.FindControl("lblName")).Text = "Agenda deleted successfully";

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Success", Convert.ToString(Guid.Parse(ids[0])), "Agenda deleted successfully");

                            //if (ids[1] != null && ids[1].Length > 0)
                            //{
                            //Delete existing image              
                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                            if (!string.IsNullOrEmpty(objAgenda.UploadedAgendaNote))
                            {
                                string[] agendaNames = objAgenda.UploadedAgendaNote.Split(',');
                                string[] agendaPdfs = objAgenda.AgendaNote.Split(',');

                                for (int i = 0; i <= agendaNames.Count() - 1; i++)
                                {
                                    File.Delete(Server.MapPath(savePath + agendaNames[i]));
                                }
                            }
                            // File.Delete(Server.MapPath(savePath + ids[1]));
                            //}
                            //BindAgendaMeeting();
                            BindAgenda();
                            Info.Visible = true;
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Failed", Convert.ToString(Guid.Parse(ids[0])), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Failed", "", "Sorry you are not maker");

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

        /// <summary>
        /// Grid view delete event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgenda_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        /// <summary>
        /// Grid view sorting agenda
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgenda_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Guid MeetingId = Guid.Parse(ddlAgendaMeeting.SelectedValue);

                DataTable dt = (DataTable)AgendaDataProvider.Instance.GetAgendabyMeetingId(MeetingId).AsDataTable();
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

                grdAgenda.DataSource = dv;
                grdAgenda.DataBind();
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
        /// Gridview page index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdAgenda.PageIndex = e.NewPageIndex;
                BindAgenda();
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
        /// drop down list selected index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void ddlAgendaMeeting_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pageno = 0;
            try
            {
                if (ddlAgendaMeeting.SelectedValue != null && ddlAgendaMeeting.SelectedValue != "0")
                {
                    Guid MeetingId = Guid.Parse(ddlAgendaMeeting.SelectedValue);
                    IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAgendabyMeetingId(MeetingId);

                    try
                    {
                        ddlMeeting.SelectedValue = ddlAgendaMeeting.SelectedValue;
                        ddlMeeting_SelectedIndexChanged(sender, e);
                    }
                    catch (Exception ex)
                    {
                        ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                        Error.Visible = true;

                        LogError objEr = new LogError();
                        objEr.HandleException(ex);
                    }
                    if (objAgenda.Count > 0)
                    {
                        var objParentAgenda = from agend in objAgenda
                                              where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                              select agend;

                        //List<string>  objParentAgenda = (from agend in objAgenda
                        //                     where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                        //                     select agend.AgendaId.ToString()).ToList();

                        IList<AgendaDomain> objAgendaWithChild = (from agend in objAgenda
                                                                  where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                                  select agend).ToList();

                        var joints = from q in objAgendaWithChild
                                     join p in objParentAgenda on q.ParentAgendaId equals p.AgendaId

                                     select q;


                        var query = (from ParentAgenda in objParentAgenda
                                     join SubAgenda in joints on ParentAgenda.AgendaId equals SubAgenda.ParentAgendaId into gj
                                     from subpet in gj.DefaultIfEmpty()
                                     select new
                                     {
                                         ParentAgenda.AgendaChecker,
                                         ParentAgenda.AgendaId,
                                         ParentAgenda.AgendaName,
                                         ParentAgenda.AgendaNote,
                                         ParentAgenda.AgendaOrder,
                                         ParentAgenda.CheckerName,
                                         ParentAgenda.Classification,
                                         ParentAgenda.CreatedBy,
                                         ParentAgenda.CreatedOn,
                                         ParentAgenda.DeletedAgenda,
                                         ParentAgenda.EntityId,
                                         ParentAgenda.EntityName,
                                         ParentAgenda.ForumId,
                                         ParentAgenda.ForumName,
                                         ParentAgenda.IsPublished,
                                         ParentAgenda.MeetingDate,
                                         ParentAgenda.MeetingId,
                                         ParentAgenda.MeetingTime,
                                         ParentAgenda.MeetingVenue,
                                         ParentAgenda.Minutes,
                                         ParentAgenda.ParentAgendaId,
                                         ParentAgenda.PublishedBy,
                                         ParentAgenda.UpdatedBy,
                                         ParentAgenda.UpdateOn,
                                         ParentAgenda.UploadedAgendaNote,
                                         ParentAgenda.SerialNumber,
                                         HasChild = (subpet == null ? String.Empty : " <b style='color:green; top:1px;' title='This item has sub agenda'>+</b>")
                                     }).Distinct();

                        int counts = objAgenda.Select(p => p.AgendaChecker).Distinct().Count();
                        //if (counts > 1)
                        //{
                        //    trCheckerMessage.Visible = true;
                        //}
                        //else
                        //{
                        //    trCheckerMessage.Visible = false;
                        //}

                        //if (Request.QueryString["aid"] != null)
                        //{
                        if (Request.UrlReferrer != null)
                        {
                            string prvPage = Request.UrlReferrer.ToString();

                            if (prvPage.ToLower().EndsWith("createsubagenda.aspx"))
                            {
                                Guid AgendaId = Guid.Empty;
                                try
                                {
                                    if (Guid.TryParse(Session["AgendaId"].ToString(), out AgendaId))//(Guid.TryParse(Request.QueryString["aid"].ToString(), out AgendaId))
                                    {
                                        List<AgendaDomain> objAgendaDomainSingle = objParentAgenda.ToList<AgendaDomain>();

                                        int index = objAgendaDomainSingle.FindIndex(a => a.AgendaId == AgendaId);

                                        if (index > 0)
                                        {
                                            double pageindex = index / 10;
                                            pageno = Convert.ToInt32(Math.Round(pageindex));
                                        }

                                        // ListItem item = query.ToList<AgendaDomain>().FindIndex(AgendaId.ToString());
                                        this.Request.QueryString.Remove("aid");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    PropertyInfo isreadonly =
    typeof(System.Collections.Specialized.NameValueCollection).GetProperty(
    "IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                                    // make collection editable
                                    isreadonly.SetValue(this.Request.QueryString, false, null);
                                    this.Request.QueryString.Remove("aid");
                                }
                            }
                        }
                        //}


                        grdAgenda.DataSource = query.AsDataTable(); //objParentAgenda
                        grdAgenda.PageIndex = pageno;
                        grdAgenda.DataBind();
                        lnkSendApproval.Visible = true;
                        hylnkAgenda.Visible = true;
                        lnkPublish.Visible = true;
                        //Store date and time for avoid past editing
                        ViewState["MeetingTime"] = objAgenda[0].MeetingTime;
                        ViewState["MeetingDate"] = objAgenda[0].MeetingDate;


                        //lblMeeting.Text = Convert.ToDateTime(objAgenda[0].MeetingDate + " " + objAgenda[0].MeetingTime).ToString("f");

                        //lblForum.Text = MM.Core.Encryptor.DecryptString(objAgenda[0].ForumName);

                        if (ddlUser.Items.FindByValue(
Convert.ToString(objAgenda[0].AgendaChecker)) != null)
                        {
                            ddlUser.SelectedValue = Convert.ToString(objAgenda[0].AgendaChecker);
                            chkChecker.Checked = true;
                        }
                        else
                        {
                            ddlUser.SelectedValue = "0";
                        }
                    }
                    else
                    {
                        trCheckerMessage.Visible = false;

                        lnkSendApproval.Visible = false;
                        hylnkAgenda.Visible = false;
                        lnkPublish.Visible = false;
                        grdAgenda.DataSource = null;
                        grdAgenda.DataBind();
                    }


                    //hylnkAgenda.NavigateUrl = "AgendaMaster.aspx?id=" + MeetingId;
                }
                else
                {
                    trCheckerMessage.Visible = false;

                    ddlMeeting.SelectedValue = "0";
                    grdAgenda.DataSource = null;
                    grdAgenda.DataBind();
                    lnkSendApproval.Visible = false;
                    hylnkAgenda.Visible = false;
                    lnkPublish.Visible = false;
                }

                bool IsMaker = Convert.ToBoolean(Session["IsMaker"]);
                bool IsChecker = Convert.ToBoolean(Session["IsChecker"]);

                if (IsMaker && !IsChecker)
                {
                    lnkPublish.Visible = false;
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
        /// View uploaded file
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void lnkView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = hdnDeletePdfName.Value;//Convert.ToString(ViewState["file"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.WriteFile(FilePath);
                Response.End();
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
        /// Delete pdf agenda event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();

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

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter agenda title.";
                    Error.Visible = true;
                    return;
                }

                if (ddlAgendaMeeting.SelectedValue == "0")
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please select meeting.";
                    Error.Visible = true;
                    return;
                }


                if (txtPresenter.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtPresenter);
                    if (!objUser.isValidChar(txtPresenter.Text))
                    {
                        txtPresenter.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }

                DateTime dtNow = DateTime.Now;
                DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDateAdd"]) + " " + Convert.ToString(ViewState["MeetingTimeAdd"]));
                if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't add agenda to past meetings. ";
                    Error.Visible = true;
                    return;
                }

                AgendaDomain objAgenda = new AgendaDomain();
                objAgenda.AgendaName = txtTitle.Text;
                objAgenda.AgendaNote = txtAgendaNote.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());

                objAgenda.Presenter = txtPresenter.Text;

                objAgenda.SerialNumber = txtSerialNumber.Text.Trim();
                //if (ddlSerialNumber.SelectedValue != "0")
                //{
                //    objAgenda.SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                //}
                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ddlAgendaMeeting.SelectedValue));
                if (ddlUser.SelectedValue != "0")
                {
                    objAgenda.AgendaChecker = Guid.Parse(ddlUser.SelectedValue);
                }

                string deletePdf = hdnDeletePdfName.Value;


                if (ViewState["delFile"] != null)
                {
                    deletePdf = Convert.ToString(ViewState["delFile"]) + "," + hdnDeletePdfName.Value;
                }
                string AgendaFilePdf = "";
                string AgendaFileTimeName = "";

                string[] fileNames = Convert.ToString(ViewState["file"]).Replace(" ", "").Split(',');
                string[] pdfNames = Convert.ToString(ViewState["pdf"]).Split(',');

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                if (hdnDelete.Value == "delete")
                {
                    //  File.Delete(Server.MapPath(savePath + hdnDeletePdfName.Value));
                }
                if (hdnDelete.Value == "don't")
                {
                    int pos = Array.IndexOf(fileNames, hdnDeletePdfName.Value.Trim());
                    if (pos > -1)
                    {
                        AgendaFilePdf = pdfNames[pos];
                        AgendaFileTimeName = fileNames[pos];
                    }
                }


                //    var listFileNames = new List<string>(fileNames);
                //    listFileNames.RemoveAt(pos);

                //    var listPdfNames = new List<string>(pdfNames);
                //    listPdfNames.RemoveAt(pos);

                //    fileNames = listFileNames.ToArray();
                //    pdfNames = listPdfNames.ToArray();

                //}


                //comma sepated string
                string joinedPdfNames = String.Join(", ", pdfNames);
                string joinedFileNames = String.Join(", ", fileNames);


                objAgenda.UploadedAgendaNote = joinedFileNames;
                objAgenda.AgendaNote = joinedPdfNames;

                //create Merged pdf

                List<string> strPdfsList = new List<string>();
                string[] arrDeletedpdf = deletePdf.Split(',');
                string[] arrAgendaFiles = objAgenda.UploadedAgendaNote.Split(',');


                for (int pdfUp = 0; pdfUp <= arrAgendaFiles.Count() - 1; pdfUp++)
                {
                    if (!string.IsNullOrWhiteSpace(arrAgendaFiles[pdfUp]) && !arrDeletedpdf.Contains(arrAgendaFiles[pdfUp].Trim()))
                    {
                        strPdfsList.Add(arrAgendaFiles[pdfUp].Trim());
                    }
                }
                MergePdf(strPdfsList, arrAgendaFiles[0].Trim());

                objAgenda.AgendaId = Guid.Parse(hdnAgendaId.Value);
                string EncryptionKey = Session["EncryptionKey"].ToString();
                if (hdnAgendaId.Value != "" && hdnAgendaId.Value.Length == 36)
                {
                    System.Data.DataSet ds = AgendaDataProvider.Instance.GetAgendKeys(Guid.Parse(hdnAgendaId.Value));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        EncryptionKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                    }
                }

                bool status = AgendaDataProvider.Instance.UpdateKey(objAgenda, deletePdf, AgendaFileTimeName, AgendaFilePdf, EncryptionKey, DateTime.Now.ToString());

                //   bool status = AgendaDataProvider.Instance.Update(objAgenda, deletePdf, AgendaFileTimeName, AgendaFilePdf);
                if (ddlUser.SelectedValue != "0")
                {
                    ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully and sent for approval";
                }
                else
                {
                    ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully";
                }

                var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                var encrypt = Encryptor.EncryptString(json);

                //web logs
                UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Agenda", "Success", Convert.ToString(objAgenda.AgendaId), ((Label)Info.FindControl("lblName")).Text.Trim() + " :- " + encrypt);

                //   BindAgendaMeeting();
                //ddlAgendaMeeting.SelectedValue = ddlMeeting.SelectedValue;
                //BindAgenda();
                BindAgenda();
                Info.Visible = true;
                EditAgenda(Guid.Parse(hdnAgendaId.Value), ddlAgendaMeeting.SelectedValue, sender, e);
                // ClearData();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;
                Info.Visible = false;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        /// <summary>
        /// Button view click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = hdnDeletePdfName.Value;//Convert.ToString(ViewState["file"]);

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                //Set the appropriate ContentType.
                Response.ContentType = "application/octet-stream";
                //Get the physical path to the file.
                string FilePath = Server.MapPath(savePath + fileName);

                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.WriteFile(FilePath);
                Response.End();
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
        /// Get Pdf's uploaded 
        /// </summary>
        /// <param name="AgendaFiles">object Specifiying AgendaFiles</param>
        /// <param name="AgendPdf">object Specifiying AgendPdf</param>
        /// <param name="deletedAgenda">object Specifiying deletedAgenda</param>
        /// <returns></returns>
        protected string GetUploadedAgenda(object AgendaFiles, object AgendPdf, object deletedAgenda)
        {
            try
            {
                string AgendaFileName = Convert.ToString(AgendaFiles);
                string AgendaPdfName = Convert.ToString(AgendPdf);
                string AgendaDelete = Convert.ToString(deletedAgenda);

                string[] strDeletedAgenda = AgendaDelete.Split(',');
                StringBuilder sbAgenda = new StringBuilder("");
                string[] agendaNames = AgendaFileName.Split(',');
                string[] agendaPdfs = AgendaPdfName.Split(',');
                if (!string.IsNullOrEmpty(AgendaFileName))
                {
                    for (int i = 0; i <= agendaNames.Count() - 1; i++)
                    {
                        if (!strDeletedAgenda.Contains(agendaNames[i].Trim()))
                        {
                            string agenda = string.IsNullOrEmpty(agendaPdfs[i].Trim()) ? "Agenda.pdf" : agendaPdfs[i].Trim();

                            //  sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agenda + " </a><br/> ");
                            sbAgenda.Append(" <a class='AddView' href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agenda + " </a><br/> ");

                        }
                    }
                }
                return sbAgenda.ToString();
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
                return "Attached PDF files corrupted or unreadable";
            }
        }


        /// <summary>
        /// Merge pdf files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="strFileName"></param>
        public bool MergePdf(List<string> files, string strFileName)
        {
            try
            {
                //// Open the output document
                //PdfDocument outputDocument = new PdfDocument();
                if (files.Count == 0)
                {
                    return true;
                }

                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                //// Iterate files
                //foreach (string file in files)
                //{
                //    //   System.Web.Hosting.HostingEnvironment.MapPath();
                //    string paths = Server.MapPath(savePath + "/" + file);
                //    //System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + file);//System.Web.HttpContext.Current.Server.MapPath(savePath + "/" + file);
                //    // Open the document to import pages from it.
                //    PdfDocument inputDocument = PdfReader.Open(paths, PdfDocumentOpenMode.Import);

                //    // Iterate pages
                //    int count = inputDocument.PageCount;
                //    for (int idx = 0; idx < count; idx++)
                //    {
                //        // Get the page from the external document...
                //        PdfPage page = inputDocument.Pages[idx];
                //        // ...and add it to the output document.
                //        outputDocument.AddPage(page);
                //    }
                //}

                //// Save the document...

                //outputDocument.Save(Server.MapPath(savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));
                ////System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));


                iTextSharp.text.Document document = new iTextSharp.text.Document();

                string File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf");

                if (File.Exists(File_Name))
                {
                    File.Delete(File_Name);
                }

                File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf");

                if (File.Exists(File_Name))
                {
                    File.Delete(File_Name);
                }

                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Create));

                if (writer == null)
                {
                    return false;
                }

                // step 3: we open the document
                document.Open();
                Dictionary<string, string> dictKeys = null;
                Dictionary<string, string> dictEncrKeys = null;
                string AgendaEncrKey = "";
                AgendaEncrKey = Session["EncryptionKey"].ToString();
                if (hdnAgendaId.Value.Length == 36)
                {
                    System.Data.DataSet ds = AgendaDataProvider.Instance.GetAgendKeys(Guid.Parse(hdnAgendaId.Value));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dictKeys = ds.Tables[0].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                  row => row.Field<string>(1));

                        dictEncrKeys = ds.Tables[0].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
                                  row => row.Field<string>(2));

                        AgendaEncrKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                    }
                }

                bool pdfCreate = false;
                foreach (string fileName in files)
                {
                    string AgendaKey = "";

                    if (dictKeys != null)
                    {
                        if (dictKeys.ContainsKey(fileName))
                        {
                            AgendaKey = dictKeys[fileName];
                            AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[fileName]);
                        }
                    }

                    PdfReader reader = null;

                    string PdfPassword = EncryptionHelper.GetPassword(fileName, AgendaKey);
                    if (PdfPassword != "")
                    {
                        byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(PdfPassword);

                        #region aes 128 bit decryption
                        // we create a reader for a certain document
                        //using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(AgendaEncrKey)))
                        //{
                        //    ICryptoTransform ictD = acsp.CreateDecryptor();

                        //    //RawBytes now contains original byte array, still in Encrypted state

                        //    //Decrypt into stream                            
                        //    using (FileStream msD = new FileStream(Server.MapPath(savePath + "/" + fileName), FileMode.Open))
                        //    {
                        //        using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
                        //        {
                        //            using (FileStream fsOutput = new FileStream(Server.MapPath(savePath + "/" + "oo" + fileName), FileMode.Create))
                        //            {
                        //                int data;
                        //                while ((data = csD.ReadByte()) != -1)
                        //                {
                        //                    fsOutput.WriteByte((byte)data);
                        //                }

                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        byte[] decryptedBytes = null;
                        byte[] bytesToBeEncrypted = File.ReadAllBytes(Server.MapPath(savePath + "/" + fileName));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (RijndaelManaged rijndael = new RijndaelManaged())
                            {
                                byte[] ba = Encoding.Default.GetBytes(AgendaEncrKey);
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
                        }
                        File.WriteAllBytes(Server.MapPath(savePath + "/" + "oo" + fileName), decryptedBytes);
                        reader = new PdfReader(Server.MapPath(savePath + "/" + "oo" + fileName), password);
                    }
                    else
                    {
                        reader = new PdfReader(Server.MapPath(savePath + "/" + fileName));
                    }
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                        pdfCreate = true;
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        //writer.CopyAcroForm(reader);
                        writer.AddDocument(reader);
                    }

                    reader.Close();
                    if (File.Exists(Server.MapPath(savePath + "/" + "oo" + fileName)))
                    {
                        File.Delete(Server.MapPath(savePath + "/" + "oo" + fileName));
                    }
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();

                if (pdfCreate)
                {
                    string AgendaKey = "";

                    if (dictKeys != null)
                    {
                        if (dictKeys.ContainsKey(strFileName))
                        {
                            AgendaKey = dictKeys[strFileName];
                            AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[strFileName]);
                        }
                    }
                    string pdfPass = EncryptionHelper.GetPassword(strFileName, AgendaKey);
                    if (pdfPass != "")
                    {
                        using (Stream input = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Open))//new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (Stream output = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                            {
                                PdfReader reader = new PdfReader(input);

                                PdfEncryptor.Encrypt(reader, output, true, pdfPass, pdfPass, PdfWriter.ALLOW_MODIFY_ANNOTATIONS);

                            }
                        }
                    }
                    else
                    {
                        System.IO.File.Move(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"));
                    }

                    if (File.Exists(File_Name))
                    {
                        File.Delete(File_Name);
                    }


                    EncryptionHelper.EncryptString(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"), AgendaEncrKey);

                }
                return true;
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Uploaded Pdf Error: " + ex.Message;//"Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);

                return false;
            }
        }

        /// <summary>
        /// Show agenda for checker approval
        /// </summary>
        /// <param name="strMeetingId"></param>
        public void CheckerAgenda(string strMeetingId)
        {
            Guid meetingId;
            if (Guid.TryParse(strMeetingId, out meetingId))
            {
                lblAgndaPublish.Text = "";
                //ViewState["MeetingId"] = meetingId;

                StringBuilder strMenu = new StringBuilder("");
                IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetAgendabyMeetingId(meetingId);

                //  IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetApprovedAgendabyMeetingIdForAccess(meetingId);

                //  IList<UserAgendaDomain> objUserAgenda = UserAgendaDataProvider.Instance.GetUserAgendaByMeeting(Guid.Parse(ddlMeeting.SelectedValue), Guid.Parse(ddlUser.SelectedValue));

                //List<Guid> objUsersAgendaIds = new List<Guid>();
                //if (objUserAgenda.Count > 0)
                //{
                //    objUsersAgendaIds = (from ids in objUserAgenda select ids.AgendaId).ToList<Guid>();
                //    //ViewState["AgendaIds"] = objUsersAgendaIds;

                //    IEnumerable<string> strVals = (from ids in objUserAgenda select ids.AgendaId.ToString());
                //    hdnAgenda.Value = string.Join(",", strVals.ToArray());
                //    ViewState["AgendaIds"] = strVals.ToList();
                //    //  strVals.ToArray();
                //    //hdnAgenda.Value = objUsersAgendaIds.ToString().ToArray();
                //}

                var objOneItem = objAgentList.Where(p => p.IsApproved.ToLower().Equals("pending")).Distinct().ToList();

                if (objOneItem.Count() > 0)
                {
                    //if (objOneItem[0] != Guid.Empty)
                    //{
                    trCheckerMessage.Visible = true;
                    //}
                    //else
                    //{
                    //    trCheckerMessage.Visible = false;
                    //}
                }

                //else if (objAgentList.Select(p => p.AgendaChecker).Distinct().Count() > 1)
                //{
                //    trCheckerMessage.Visible = true;
                //}
                else
                {
                    trCheckerMessage.Visible = false;
                }

                if (objAgentList.Count > 0)
                {
                    //get only parent agenda
                    var objParentAgenda = from agend in objAgentList
                                          where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                          select agend;



                    //get only sub agenda
                    var subAgendaList = from agend in objAgentList
                                        where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                        select agend;

                    //get agenda name/note and agenda id
                    var agendaName = (from agend in objParentAgenda

                                      select (new
                                      {
                                          AgendaName = agend.AgendaName,
                                          AgendaId = agend.AgendaId,
                                          UplaodedAgenda = agend.UploadedAgendaNote,
                                          Classifications = agend.Classification,
                                          AgendaChecker = agend.AgendaChecker,
                                          IsApproved = agend.IsApproved,
                                          IsPublished = agend.IsPublished,
                                          SerialNumber = agend.SerialNumber,
                                          Presenter = agend.Presenter,
                                          SerialNumberType = agend.SerialNumberType,
                                          SerialTitle = agend.SerialTitle,
                                          SerialText = agend.SerialText
                                      })).Distinct().ToList();
                    Dictionary<string, int> objSerial = new Dictionary<string, int>();
                    Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                    if (agendaName.Count() > 0)
                    {
                        string ClassificationOld = "";
                        string ClassificationNew = "";

                        string Styles = "";
                        strMenu.Append("<div id='accordion'>  <input type='checkbox' onclick='javascript: fn_select_all(this);' /> Select All Items<ol id='sort'>");
                        for (int i = 0; i <= agendaName.Count() - 1; i++)
                        {
                            string Presenter = "";

                            if (agendaName[i].Presenter.Trim().Length > 0)
                            {
                                Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
                            }
                            string SerialNumber = "";
                            if (agendaName[i].SerialNumber.Length > 0)
                            {
                                SerialNumber = agendaName[i].SerialNumber + " : ";
                            }

                            Styles = "";
                            Guid agendaId = agendaName[i].AgendaId;
                            ClassificationNew = agendaName[i].Classifications;
                            //get sub agenda for parent agenda 
                            var subAgendaName = (from subAgenda in subAgendaList
                                                 where (subAgenda.ParentAgendaId == agendaId)
                                                 select (new
                                                 {
                                                     AgendaName = subAgenda.AgendaName,
                                                     AgendaId = subAgenda.AgendaId,
                                                     UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                     IsApproved = subAgenda.IsApproved,
                                                     AgendaChecker = subAgenda.AgendaChecker,
                                                     IsPublished = subAgenda.IsPublished,
                                                     SerialNumber = subAgenda.SerialNumber,
                                                     Presenter = subAgenda.Presenter,
                                                     SerialNumberType = subAgenda.SerialNumberType,
                                                     SerialTitle = subAgenda.SerialTitle,
                                                     SerialText = subAgenda.SerialText
                                                 })).ToList();

                            if (ClassificationOld != ClassificationNew)
                            {
                                ClassificationOld = ClassificationNew;
                                if (agendaName[i].Classifications != "")
                                {
                                    strMenu.Append(" <br/><b style='font-size: 13px;color: #656b6d;'>" + agendaName[i].Classifications + "</b> ");
                                }
                                //else
                                //{
                                //    strMenu.Append("<h2><b>Unclassified</b></h2>");

                                //}
                            }
                            if (agendaName[i].AgendaChecker != Guid.Empty)
                            {
                                Styles = "style='color:Red;'";
                            }
                            if (agendaName[i].AgendaChecker == Guid.Empty)
                            {
                                Styles = "style='color:#656b6d;'";
                            }
                            if (agendaName[i].AgendaChecker != Guid.Empty && agendaName[i].IsApproved.Equals("Approved"))
                            {
                                Styles = "style='color:#4800ff;'";
                            }
                            if (agendaName[i].IsPublished && agendaName[i].IsApproved.Equals("Approved"))
                            {
                                Styles = "style='color:green'";
                            }
                            string AgendaAccess = "";//" checked='checked' ";
                            //if (objUsersAgendaIds.Contains(agendaId))
                            //{
                            //    AgendaAccess = " checked='checked' ";
                            //}
                            if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                            {
                                strMenu.Append("<li id=" + agendaId + " " + Styles + "  class='group' ><h3> <input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                            }
                            else
                            {
                                strMenu.Append("<li id=" + agendaId + " " + Styles + " class='group' ><h3><input type='checkbox' id=" + agendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                            }



                            //attach sub agenda to parent agenda list element
                            if (subAgendaName.Count() > 0)
                            {

                                strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                {
                                    Presenter = "";


                                    if (subAgendaName[j].Presenter.Trim().Length > 0)
                                    {
                                        Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                    }
                                    string serialNo = "";
                                    serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                    if (serialNo.Trim().Length > 0)
                                    {
                                        serialNo = serialNo + " : ";
                                    }
                                    //if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                    //{
                                    //    if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                    //    {
                                    //        if (subAgendaName[j].SerialNumberType != "Other")
                                    //        {
                                    //            objSerial[subAgendaName[j].SerialNumber] += 1;
                                    //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                    //        }
                                    //        else
                                    //        {
                                    //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (subAgendaName[j].SerialNumberType != "Other")
                                    //        {
                                    //            objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                    //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                    //        }
                                    //        else
                                    //        {
                                    //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                    //        }
                                    //    }
                                    //}

                                    AgendaAccess = "";//" checked='checked' ";
                                    //if (objUsersAgendaIds.Contains(subAgendaName[j].AgendaId))
                                    //{
                                    //    AgendaAccess = " checked='checked' ";
                                    //} important

                                    if (subAgendaName[j].AgendaChecker != Guid.Empty)
                                    {
                                        Styles = "style='color:Red !important;'";
                                    }
                                    if (subAgendaName[j].AgendaChecker == Guid.Empty)
                                    {
                                        Styles = "style='color:#656b6d !important;'";
                                    }
                                    if (subAgendaName[j].AgendaChecker != Guid.Empty && subAgendaName[j].IsApproved.Equals("Approved"))
                                    {
                                        Styles = "style='color:#4800ff !important;'";
                                    }

                                    if (subAgendaName[j].IsPublished && subAgendaName[j].IsApproved.Equals("Approved"))
                                    {
                                        Styles = "style='color:green !important;'";
                                    }
                                    if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                    {
                                        //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName + "");
                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + " " + Styles + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " style='display:none;' />" + serialNo + subAgendaName[j].AgendaName + Presenter + "");
                                    }
                                    else
                                    {
                                        //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName);// + "</li>");
                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + " " + Styles + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + "  style='display:none;' /> " + serialNo + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                    }
                                    Guid subAgendaId = subAgendaName[j].AgendaId;
                                    //Get sub sub agenda
                                    var subSubAgendaName = (from subAgenda in subAgendaList
                                                            where (subAgenda.ParentAgendaId == subAgendaId)
                                                            select (new
                                                            {
                                                                AgendaName = subAgenda.AgendaName,
                                                                AgendaId = subAgenda.AgendaId,
                                                                UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                AgendaChecker = subAgenda.AgendaChecker,
                                                                IsApproved = subAgenda.IsApproved,
                                                                IsPublished = subAgenda.IsPublished,
                                                                SerialNumber = subAgenda.SerialNumber,
                                                                Presenter = subAgenda.Presenter,
                                                                SerialNumberType = subAgenda.SerialNumberType,
                                                                SerialTitle = subAgenda.SerialTitle,
                                                                SerialText = subAgenda.SerialText
                                                            })).ToList();
                                    if (subSubAgendaName.Count() > 0)
                                    {

                                        //attach sub sub agenda to parent agenda list element
                                        strMenu.Append("<ol class=inddrag  style='list-style: upper-alpha outside none;margin-left:50px;'>");
                                        for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                        {
                                            Presenter = "";
                                            string serialNoSub = "";

                                            serialNoSub = CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText);
                                            if (serialNoSub.Trim().Length > 0)
                                            {
                                                serialNoSub = serialNoSub + " : ";
                                            }
                                            //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                            //{
                                            //    if (objSerial.ContainsKey(subSubAgendaName[y].SerialNumber)) //objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                            //    {
                                            //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                            //        {
                                            //            //objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                            //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";

                                            //            objSerial[subSubAgendaName[y].SerialNumber] += 1;
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                            //        {
                                            //            //objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                            //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";

                                            //            objSerial.Add(subSubAgendaName[y].SerialNumber, 1);
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //}

                                            if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                            }

                                            AgendaAccess = "";//" checked='checked' ";
                                            //if (objUsersAgendaIds.Contains(subSubAgendaName[y].AgendaId))
                                            //{
                                            //    AgendaAccess = " checked='checked' ";
                                            //}

                                            if (subSubAgendaName[y].AgendaChecker != Guid.Empty)
                                            {
                                                Styles = "style='color:Red;'";
                                            }
                                            if (subSubAgendaName[y].AgendaChecker == Guid.Empty)
                                            {
                                                Styles = "style='color:#656b6d;'";
                                            }
                                            if (subSubAgendaName[y].AgendaChecker != Guid.Empty && subSubAgendaName[y].IsApproved.Equals("Approved"))
                                            {
                                                Styles = "style='color:#4800ff;'";
                                            }

                                            if (subSubAgendaName[y].IsPublished && subSubAgendaName[y].IsApproved.Equals("Approved"))
                                            {
                                                Styles = "style='color:green;'";
                                            }
                                            if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                            {
                                                // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' />" + subSubAgendaName[y].AgendaName + " ");
                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + " " + Styles + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + "  style='display:none;' />" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter + " ");
                                            }
                                            else
                                            {
                                                //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subSubAgendaName[y].AgendaName);
                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + " " + Styles + "> <input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " style='display:none;' />" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter);
                                            }
                                        }
                                        strMenu.Append("</ol>");
                                    }
                                    strMenu.Append("</li>");
                                }
                                strMenu.Append("</ol></div></li>");
                            }

                        }
                        strMenu.Append("</ol></div>");
                    }
                    lblList.Text = strMenu.ToString();
                    lblAlert.Text = " <script> ShowPopUp(); </script>";
                    hdnPopUp.Value = "1";

                    //  lblList.Text = strMenuList;
                }
                else
                {
                    lblList.Text = "No agenda uploaded or approved";
                }
            }
        }

        /// <summary>
        ///SendApproval button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSendApproval_Click(object sender, EventArgs e)
        {
            CheckerAgenda(ddlAgendaMeeting.SelectedValue);

        }

        /// <summary>
        /// SubmitAgenda button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmitAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnAgenda.Value.Length > 0)
                {
                    if (chkNotifyChecker.Checked)
                    {
                        bool isSuccess;
                        string Message;
                        SendEmail objSendEmail = new SendEmail();
                        objSendEmail.SendNotifyEmail("Agenda", Guid.Parse(ddlUser.SelectedValue), out isSuccess, out Message);

                        if (isSuccess)
                        {
                            lblAlert.Text = "<script> alert('Selected items sent for approval successfully and sent to checker '); ShowPopUp(); </script>";
                            //Info.Visible = true;
                        }
                        else
                        {
                            //  ((Label)Info.FindControl("lblName")).Text = 
                            lblAlert.Text = " <script> alert('Selected items sent for approval successfully" + Message + "'); ShowPopUp(); </script>";
                            // ((Label)Error.FindControl("lblError")).Text = Message;
                            //Info.Visible = true;
                            //Error.Visible = true;

                        }
                    }

                    string strVal = hdnAgenda.Value;
                    bool status = AgendaDataProvider.Instance.UpdateAgendaChecker("," + hdnAgenda.Value + ",", Guid.Parse(ddlUser.SelectedValue), Guid.Parse(Session["UserId"].ToString()));
                    hdnAgenda.Value = "";

                    CheckerAgenda(ddlAgendaMeeting.SelectedValue);

                    lblAlert.Text += " <script> alert('Selected items sent for approval successfully'); ShowPopUp(); </script>";
                    trCheckerMessage.Visible = true;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Success", strVal, lblAlert.Text);
                    strVal = "";
                }
                else
                {
                    lblAlert.Text = " <script> alert('Select at least one agenda item'); ShowPopUp(); </script>";

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Failed", "", lblAlert.Text);

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
        /// Publish agenda for checker approval
        /// </summary>
        /// <param name="strMeetingId"></param>
        public void PublishAgenda(string strMeetingId)
        {
            Guid meetingId;
            if (Guid.TryParse(strMeetingId, out meetingId))
            {
                StringBuilder strAllAgenda = new StringBuilder("");
                //ViewState["MeetingId"] = meetingId;
                lblList.Text = "";
                StringBuilder strMenu = new StringBuilder("");
                IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetAgendabyMeetingIdForPublish(meetingId);

                IList<MarkedAgendaDomain> objMarkedAgenda = MarkedAgendaDataProvider.Instance.GetAllMarkedAgendaByMeeting(Guid.Parse(ddlMeeting.SelectedValue));
                if (objMarkedAgenda.Count > 0)
                {
                    IEnumerable<string> strVals = (from ids in objMarkedAgenda select ids.AgendaId.ToString());
                    ViewState["oldUnreadAgenda"] = string.Join(",", strVals.ToArray());
                }

                if (objAgentList.Count > 0)
                {
                    //get only parent agenda
                    var objParentAgenda = from agend in objAgentList
                                          where (agend.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                          select agend;



                    //get only sub agenda
                    var subAgendaList = from agend in objAgentList
                                        where (agend.ParentAgendaId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                        select agend;

                    //get agenda name/note and agenda id
                    var agendaName = (from agend in objParentAgenda

                                      select (new
                                      {
                                          AgendaName = agend.AgendaName,
                                          AgendaId = agend.AgendaId,
                                          UplaodedAgenda = agend.UploadedAgendaNote,
                                          Status = agend.AgendaNote,
                                          IsPublished = agend.IsPublished,
                                          CheckerName = agend.CheckerName,
                                          Classifications = agend.Classification,

                                          DeletedAgenda = agend.DeletedAgenda,
                                          SerialNumber = agend.SerialNumber,
                                          Presenter = agend.Presenter,
                                          SerialNumberType = agend.SerialNumberType,
                                          SerialTitle = agend.SerialTitle,
                                          SerialText = agend.SerialText
                                      })).Distinct().ToList();

                    Dictionary<string, int> objSerial = new Dictionary<string, int>();
                    Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                    if (agendaName.Count() > 0)
                    {
                        string ClassificationOld = "";
                        string ClassificationNew = "";
                        string color = "";

                        strMenu.Append("<div id='accordion' style='padding-top:67px !important;'><br/> <input type='checkbox' onclick='javascript: fn_select_all(this);' /> Select All Items<ol id='sort'>");
                        for (int i = 0; i <= agendaName.Count() - 1; i++)
                        {
                            string Presenter = "";

                            if (agendaName[i].Presenter.Trim().Length > 0)
                            {
                                Presenter = "(Presenter : " + agendaName[i].Presenter + ")";
                            }

                            string SerialNumber = "";
                            if (agendaName[i].SerialNumber.Length > 0)
                            {
                                SerialNumber = agendaName[i].SerialNumber + " : ";
                            }
                            Guid agendaId = agendaName[i].AgendaId;
                            ClassificationNew = agendaName[i].Classifications;
                            color = "";
                            //get sub agenda for parent agenda 
                            var subAgendaName = (from subAgenda in subAgendaList
                                                 where (subAgenda.ParentAgendaId == agendaId)
                                                 select (new
                                                 {
                                                     AgendaName = subAgenda.AgendaName,
                                                     AgendaId = subAgenda.AgendaId,
                                                     UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                     Status = subAgenda.AgendaNote,
                                                     IsPublished = subAgenda.IsPublished,
                                                     CheckerName = subAgenda.CheckerName,
                                                     SerialNumber = subAgenda.SerialNumber,
                                                     Presenter = subAgenda.Presenter,
                                                     SerialNumberType = subAgenda.SerialNumberType,
                                                     DeletedAgenda = subAgenda.DeletedAgenda,
                                                     SerialTitle = subAgenda.SerialTitle,
                                                     SerialText = subAgenda.SerialText
                                                 })).ToList();

                            if (ClassificationOld != ClassificationNew)
                            {
                                ClassificationOld = ClassificationNew;
                                if (agendaName[i].Classifications != "")
                                {
                                    strMenu.Append(" <br/><b style='font-size: 13px;'>" + agendaName[i].Classifications + "</b> ");
                                }
                                //else
                                //{
                                //    strMenu.Append("<h2><b>Unclassified</b></h2>");

                                //}
                            }
                            string AgendaAccess = "";//" checked='checked' ";
                            //if (objUsersAgendaIds.Contains(agendaId))
                            //{
                            //    AgendaAccess = " checked='checked' ";
                            //}
                            if (agendaName[i].UplaodedAgenda.Length > 0)
                            {
                                string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');

                                bool SendPdfFile = false;
                                foreach (string str in agendaNames)
                                {
                                    if (!strDeletedAgenda.Contains(str))
                                    {
                                        SendPdfFile = true;
                                    }
                                }
                                if (agendaNames.Count() > 0 && SendPdfFile && subAgendaName.Count() == 0)
                                {
                                    strAllAgenda.Append(agendaId + ",");
                                }
                            }
                            else
                            {
                                if (subAgendaName.Count() == 0)
                                {
                                    strAllAgenda.Append(agendaId + ",");
                                }
                            }
                            if (agendaName[i].IsPublished && agendaName[i].Status.Equals("Approved"))
                            {
                                color = " style='color:green' ";

                                //Guid[] agendaIdArr = subAgendaList.Select(p => p.AgendaId).ToArray();

                                //var subAgendaItemsLIsts = (from subAgenda in subAgendaList
                                //                      where (agendaIdArr.Contains(subAgenda.ParentAgendaId))
                                //                     select (new
                                //                     {
                                //                         AgendaName = subAgenda.AgendaName,
                                //                         AgendaId = subAgenda.AgendaId,
                                //                         UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                //                         Status = subAgenda.AgendaNote,
                                //                         IsPublished = subAgenda.IsPublished,
                                //                         CheckerName = subAgenda.CheckerName
                                //                     })).ToList();

                                //var updatedSubAgendas = subAgendaItemsLIsts.Where(p => p.IsPublished == false || p.Status.Equals("Pending")).ToList();


                            }
                            if (!agendaName[i].IsPublished && agendaName[i].Status.Equals("Approved"))
                            { color = " style='color:red' "; }

                            if (!agendaName[i].IsPublished && !agendaName[i].Status.Equals("Approved"))
                            { color = " style='color:orchid' "; }

                            if (agendaName[i].IsPublished && !agendaName[i].Status.Equals("Approved"))
                            { color = " style='color:orchid' "; }

                            if (agendaName[i].UplaodedAgenda.Length > 0 && subAgendaName.Count() == 0)
                            {
                                strMenu.Append("<li id=" + agendaId + color + " class='group' ><h3> <input type='checkbox' id=" + agendaId + AgendaAccess + "  {disabled}  onclick='SaveAccess(this)' /> " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                            }
                            else
                            {
                                strMenu.Append("<li id=" + agendaId + color + " class='group' ><h3><input type='checkbox' id=" + agendaId + AgendaAccess + "  {disabled}  onclick='SaveAccess(this)' /> " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                            }

                            if (!agendaName[i].Status.Equals("Approved"))
                            {
                                string Checkers = string.IsNullOrWhiteSpace(agendaName[i].CheckerName) ? "Not Assigned" : agendaName[i].CheckerName;
                                strMenu.Append("<b>(Checker: " + Checkers + ")</b>");
                            }
                            int newUpdate = 0;
                            //attach sub agenda to parent agenda list element

                            if (subAgendaName.Count() > 0)
                            {

                                strMenu.Append("<div><ol  style='list-style:none' class=ddrag>");
                                for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                {
                                    Guid subAgendaId = subAgendaName[j].AgendaId;
                                    //Get sub sub agenda
                                    var subSubAgendaName = (from subAgenda in subAgendaList
                                                            where (subAgenda.ParentAgendaId == subAgendaId)
                                                            select (new
                                                            {
                                                                AgendaName = subAgenda.AgendaName,
                                                                AgendaId = subAgenda.AgendaId,
                                                                UplaodedAgenda = subAgenda.UploadedAgendaNote,
                                                                Status = subAgenda.AgendaNote,
                                                                IsPublished = subAgenda.IsPublished,
                                                                CheckerName = subAgenda.CheckerName,
                                                                SerialNumber = subAgenda.SerialNumber,
                                                                Presenter = subAgenda.Presenter,
                                                                SerialNumberType = subAgenda.SerialNumberType,
                                                                DeletedAgenda = subAgenda.DeletedAgenda,
                                                                SerialTitle = subAgenda.SerialTitle,
                                                                SerialText = subAgenda.SerialText
                                                            })).ToList();

                                    Presenter = "";

                                    if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                    {
                                        string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                        string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');


                                        bool SendPdfFile = false;
                                        foreach (string str in agendaNames)
                                        {
                                            if (!strDeletedAgenda.Contains(str))
                                            {
                                                SendPdfFile = true;
                                            }
                                        }



                                        if (agendaNames.Count() > 0 && SendPdfFile && subSubAgendaName.Count() == 0)
                                        {
                                            strAllAgenda.Append(subAgendaName[j].AgendaId + ",");
                                        }
                                    }
                                    else
                                    {
                                        if (subSubAgendaName.Count() == 0)
                                        {
                                            strAllAgenda.Append(subAgendaName[j].AgendaId + ",");
                                        }
                                    }

                                    if (subAgendaName[j].Presenter.Trim().Length > 0)
                                    {
                                        Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
                                    }
                                    string serialNo = "";

                                    serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                    if (serialNo.Trim().Length > 0)
                                    {
                                        serialNo = serialNo + " : ";
                                    }

                                    //if (subAgendaName[j].SerialNumber.Trim().Length > 0)
                                    //{
                                    //    if (objSerial.ContainsKey(subAgendaName[j].SerialNumber))
                                    //    {
                                    //        if (subAgendaName[j].SerialNumberType != "Other")
                                    //        {
                                    //            objSerial[subAgendaName[j].SerialNumber] += 1;
                                    //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                    //        }
                                    //        else
                                    //        {
                                    //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (subAgendaName[j].SerialNumberType != "Other")
                                    //        {
                                    //            objSerial.Add(subAgendaName[j].SerialNumber, 1);
                                    //            serialNo = subAgendaName[j].SerialNumber + " - " + objSerial[subAgendaName[j].SerialNumber] + " : ";
                                    //        }
                                    //        else
                                    //        {
                                    //            serialNo = subAgendaName[j].SerialNumber + " : ";
                                    //        }
                                    //    }
                                    //}
                                    color = "";
                                    AgendaAccess = "";//" checked='checked' ";
                                    //if (objUsersAgendaIds.Contains(subAgendaName[j].AgendaId))
                                    //{
                                    //    AgendaAccess = " checked='checked' ";
                                    //}

                                    if (subAgendaName[j].IsPublished && subAgendaName[j].Status.Equals("Approved"))
                                        color = " style='color:green !important' ";

                                    if (!subAgendaName[j].IsPublished && subAgendaName[j].Status.Equals("Approved"))
                                        color = " style='color:red !important' ";

                                    if (!subAgendaName[j].IsPublished && !subAgendaName[j].Status.Equals("Approved"))
                                        color = " style='color:orchid !important' ";

                                    if (subAgendaName[j].IsPublished && !subAgendaName[j].Status.Equals("Approved"))
                                        color = " style='color:orchid !important' ";

                                    if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                    {
                                        //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName + "");
                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + color + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " style='display:none;' />" + serialNo + subAgendaName[j].AgendaName + Presenter + "");
                                    }
                                    else
                                    {
                                        //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> " + (i + 1) + "." + (j + 1) + " <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subAgendaName[j].AgendaName);// + "</li>");
                                        strMenu.Append("<li id=" + subAgendaName[j].AgendaId + color + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> <input type='checkbox' id=" + subAgendaName[j].AgendaId + AgendaAccess + "  style='display:none;' /> " + serialNo + subAgendaName[j].AgendaName + Presenter);// + "</li>");
                                    }

                                    var UpdatedSubSubAgendaItems = subSubAgendaName.Where(p => p.IsPublished == false || p.Status.Equals("Pending")).ToList();
                                    if (UpdatedSubSubAgendaItems.Count > 0)
                                    {
                                        newUpdate++;
                                    }

                                    if (subSubAgendaName.Count() > 0)
                                    {

                                        //attach sub sub agenda to parent agenda list element
                                        strMenu.Append("<ol class=inddrag  style='list-style: upper-alpha outside none;margin-left:50px;'>");
                                        for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                        {
                                            string serialNoSub = "";

                                            Presenter = "";

                                            if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');


                                                bool SendPdfFile = false;
                                                foreach (string str in agendaNames)
                                                {
                                                    if (!strDeletedAgenda.Contains(str))
                                                    {
                                                        SendPdfFile = true;
                                                    }
                                                }


                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    strAllAgenda.Append(subSubAgendaName[y].AgendaId + ",");
                                                }
                                            }
                                            else
                                            {
                                                strAllAgenda.Append(subSubAgendaName[y].AgendaId + ",");
                                            }
                                            if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                            }

                                            serialNoSub = CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText);
                                            if (serialNoSub.Trim().Length > 0)
                                            {
                                                serialNoSub = serialNoSub + " : ";
                                            }
                                            //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                            //{
                                            //    if (objSerial.ContainsKey(subSubAgendaName[y].SerialNumber))//objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                            //    {
                                            //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                            //        {
                                            //            //objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                            //            objSerial[subSubAgendaName[y].SerialNumber] += 1;
                                            //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //    else
                                            //    {
                                            //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                            //        {
                                            //            //objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                            //            objSerial.Add(subSubAgendaName[y].SerialNumber, 1);
                                            //            //serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerial[subSubAgendaName[y].SerialNumber] + " : ";
                                            //        }
                                            //        else
                                            //        {
                                            //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                            //        }
                                            //    }
                                            //}

                                            color = "";
                                            AgendaAccess = "";//" checked='checked' ";
                                            //if (objUsersAgendaIds.Contains(subSubAgendaName[y].AgendaId))
                                            //{
                                            //    AgendaAccess = " checked='checked' ";
                                            //}
                                            if (subSubAgendaName[y].IsPublished && subSubAgendaName[y].Status.Equals("Approved"))
                                                color = " style='color:green' ";

                                            if (!subSubAgendaName[y].IsPublished && subSubAgendaName[y].Status.Equals("Approved"))
                                                color = " style='color:red' ";

                                            if (!subSubAgendaName[y].IsPublished && !subSubAgendaName[y].Status.Equals("Approved"))
                                                color = " style='color:orchid' ";

                                            if (subSubAgendaName[y].IsPublished && !subSubAgendaName[y].Status.Equals("Approved"))
                                                color = " style='color:orchid' ";
                                            if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                            {
                                                // strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' />" + subSubAgendaName[y].AgendaName + " ");
                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + color + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + "  style='display:none;' />" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter + " ");
                                            }
                                            else
                                            {
                                                //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " onclick='SaveAccess(this)' /> " + subSubAgendaName[y].AgendaName);
                                                strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + color + "> <input type='checkbox' id=" + subSubAgendaName[y].AgendaId + AgendaAccess + " style='display:none;' />" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter);
                                            }


                                        }
                                        strMenu.Append("</ol>");
                                    }
                                    strMenu.Append("</li>");
                                }
                                strMenu.Append("</ol></div></li>");
                            }
                            ViewState["AllAgenda"] = strAllAgenda.ToString();

                            var UpdatedSubItems = subAgendaName.Where(p => p.IsPublished == false || p.Status.Equals("Pending")).ToList();
                            if (agendaName[i].IsPublished && agendaName[i].Status.Equals("Approved"))
                            {
                                if (UpdatedSubItems.Count() == 0 && newUpdate == 0)
                                {
                                    strMenu.Replace("{disabled}", "  disabled = 'disabled'");
                                }
                                else
                                {
                                    strMenu.Replace("{disabled}", "");
                                }
                            }
                            else
                            {
                                strMenu.Replace("{disabled}", "");
                            }
                        }
                        strMenu.Append("</ol></div>");
                    }
                    lblAgndaPublish.Text = strMenu.ToString();
                    lblAlert.Text = " <script> ShowPopUpPublish(); </script>";
                    hdnPopUpPublish.Value = "1";

                    //  lblList.Text = strMenuList;
                }
                else
                {
                    lblAgndaPublish.Text = "No agenda uploaded or approved";

                }
            }
        }

        /// <summary>
        /// Publish button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPublish_Click(object sender, EventArgs e)
        {
            PublishAgenda(ddlAgendaMeeting.SelectedValue);
        }

        /// <summary>
        /// Publish selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublishAgenda_Click(object sender, EventArgs e)
        {
            try
            {

                if (hdnAgenda.Value.Length > 0)
                {
                    string strAllAgenda = Convert.ToString(ViewState["AllAgenda"]);
                    string delAgenda = "";
                    char c = ',';
                    if (ViewState["oldUnreadAgenda"] != null)
                    {

                        string oldUnreadAgenda = Convert.ToString(ViewState["oldUnreadAgenda"]);
                        string[] arrAllAgenda = strAllAgenda.Split(',');
                        string[] oldAgenda = oldUnreadAgenda.Split(',');
                        string[] unReadArray = oldAgenda.Except(arrAllAgenda).ToArray();//arrAllAgenda.Except(oldAgenda).ToArray();
                        delAgenda = string.Join(",", unReadArray);


                        if (delAgenda.EndsWith(c.ToString()))
                        {
                            delAgenda = "," + delAgenda;
                        }
                        else
                        {
                            if (delAgenda == "")
                            {
                                delAgenda = "";
                            }
                            else
                            {
                                delAgenda = "," + delAgenda + ",";
                            }
                        }
                    }

                    if (strAllAgenda.EndsWith(c.ToString()))
                    {

                        strAllAgenda = "," + strAllAgenda;
                    }
                    else
                    {
                        if (strAllAgenda == "")
                        {
                            strAllAgenda = "";
                        }
                        else
                        {
                            strAllAgenda = "," + strAllAgenda + ",";
                        }
                    }

                    bool status = AgendaDataProvider.Instance.PublishAgenda("," + hdnAgenda.Value + ",", Guid.Parse(Session["UserId"].ToString()), strAllAgenda, delAgenda);
                    hdnAgenda.Value = "";
                    PublishAgenda(ddlAgendaMeeting.SelectedValue);
                    ViewState["AllAgenda"] = null;
                    ViewState["oldUnreadAgenda"] = null;

                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Success", strAllAgenda, "Selected items published successfully");

                    lblAlert.Text = " <script> alert('Selected items published successfully'); ShowPopUpPublish(); </script>";
                }
                else
                {
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Agenda", "Failed", "", "Select at least one agenda item");

                    lblAlert.Text = " <script> alert('Select at least one agenda item '); ShowPopUpPublish(); </script>";
                }
            }
            catch (Exception ex)
            {
                ((Label)Error.FindControl("lblError")).Text = "Try again after some time"; //"Try again after some time";
                Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }

        protected void lnkRemoveChecker_Click(object sender, EventArgs e)
        {
            try
            {
                Guid UserId = Guid.Parse(Session["UserId"].ToString());
                bool status = AgendaDataProvider.Instance.RemoveChecker(Guid.Parse(ddlMeeting.SelectedValue), UserId);
                if (status)
                {
                    ((Label)Info.FindControl("lblName")).Text = "Checker removed and agenda approved successfully";

                    CheckerAgenda(ddlAgendaMeeting.SelectedValue);

                    Info.Visible = true;
                    Error.Visible = false;

                    lblAlert.Text = " <script> alert('Checker removed and agenda approved successfully'); </script>";
                    trCheckerMessage.Visible = false;
                    ddlUser.SelectedIndex = -1;

                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                    Error.Visible = true;
                    Info.Visible = false;
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

        protected void hylnkAgenda_Click(object sender, EventArgs e)
        {
            Session["AgendaOrder"] = ddlAgendaMeeting.SelectedValue;
            Response.Redirect("AgendaMaster.aspx");
        }
    }
}