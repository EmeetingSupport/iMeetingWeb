using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Core;
using MM.Domain;
using MM.Data;
using System.Text;
using System.Data;

namespace MeetingMinder.Web
{
    public partial class PublishHistroy : System.Web.UI.Page
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

                #region

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
                    IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserAccess(UserId, entityId).OrderBy(p => p.ForumName).ToList();
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
                    IList<MeetingDomain> objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(forumId).OrderBy(p => DateTime.Parse(p.MeetingDate)).ToList();

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
                    BindMeeting(strForumId);
                    grdHistory.DataSource = null;
                    grdHistory.DataBind();
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
                if (strMeetingId != "0")
                {
                    BindPublish(Guid.Parse(strMeetingId));
                    grdHistory.DataSource = null;
                    grdHistory.DataBind();
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
        /// Bind data to gridview
        /// </summary>
        /// <param name="strForumId"></param>
        private void BindPublish(Guid strForumId)
        {
            try
            {
                System.Data.DataSet ds = AgendaDataProvider.Instance.GetAckHistroy(strForumId);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblPublishOn.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PublishDate"].ToString()).ToString("f");

                    grdAgendaVersions.DataSource = ds.Tables[0];
                    grdAgendaVersions.DataBind();
                }
                else
                {
                    lblPublishOn.Text = "";
                    grdAgendaVersions.DataSource = null;
                    grdAgendaVersions.DataBind();
                }

                //if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                //{
                //    List<AgendaAck> list = ds.Tables[1].AsEnumerable().Select(row => new AgendaAck
                //    {
                //        Suffix = MM.Core.Encryptor.DecryptString(row.Field<string>(15)),
                //        Name = MM.Core.Encryptor.DecryptString(row.Field<string>(13)) + " " + MM.Core.Encryptor.DecryptString(row.Field<string>(14)),
                //        AgendaRead = row.Field<bool?>(6).GetValueOrDefault(),
                //        IsNoticeRead = row.Field<bool?>(5).GetValueOrDefault(),
                //        PublishDate = row.Field<DateTime>(7),
                //        AgendaReadOn = row.Field<DateTime?>(1),

                //        NoticeReadOn = row.Field<DateTime?>(2)
                //    }
                //        ).OrderBy(p => p.Name).OrderByDescending(p => p.PublishDate).ToList();
                //    //.ToList().Sort(p=>p.Field("AckFirstName"));;
                //    grdHistory.DataSource = list;
                //    //ds.Tables[1];
                //    grdHistory.DataBind();
                //}
                //else
                //{
                //    grdHistory.DataSource = null;
                //    grdHistory.DataBind();
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



        protected void grdAgendaVersions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Equals("viewack"))
                {
                    string[] strPublishVesion = Convert.ToString(e.CommandArgument).Split(',');
                    DataSet ds = AgendaDataProvider.Instance.GetAgendAckByPublishVersion(Guid.Parse(strPublishVesion[0]), Guid.Parse(strPublishVesion[1]));
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        List<AgendaAck> list = ds.Tables[0].AsEnumerable().Select(row => new AgendaAck
                        {
                            Suffix = MM.Core.Encryptor.DecryptString(row.Field<string>(15)),
                            Name = MM.Core.Encryptor.DecryptString(row.Field<string>(13)) + " " + MM.Core.Encryptor.DecryptString(row.Field<string>(14)),
                            AgendaRead = row.Field<bool?>(6).GetValueOrDefault(),
                            IsNoticeRead = row.Field<bool?>(5).GetValueOrDefault(),
                            PublishDate = row.Field<DateTime>(7),
                            AgendaReadOn = row.Field<DateTime?>(2),

                            NoticeReadOn = row.Field<DateTime?>(1)
                        }
                        ).OrderBy(p => p.Name).OrderByDescending(p => p.PublishDate).ToList();
                        //.ToList().Sort(p=>p.Field("AckFirstName"));;
                        grdHistory.DataSource = list;
                        //ds.Tables[1];
                        grdHistory.DataBind();
                    }
                    else
                    {
                        grdHistory.DataSource = null;
                        //ds.Tables[1];
                        grdHistory.DataBind();
                    }
                }

                if (e.CommandName.ToLower().Equals("view"))
                {

                    string strPublishVesion = Convert.ToString(e.CommandArgument);

                    Guid PublishVesion;
                    if (Guid.TryParse(strPublishVesion, out PublishVesion))
                    {
                        StringBuilder strMenu = new StringBuilder("");

                        IList<AgendaDomain> objAgentList = AgendaDataProvider.Instance.GetPublishedAgendabyPublishId(PublishVesion);

                        if (objAgentList.Count > 0)
                        {

                            //objAgentList[0].MeetingTime + " on " + Convert.ToDateTime(objAgentList[0].MeetingDate).ToString("D") + " at " +
                            //ddlMeeting.SelectedItem.Text.Substring(12, ddlMeeting.SelectedItem.Text.Length - 7 - 14); 
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
                                                  AgendaNote = agend.AgendaNote,
                                                  DeletedAgenda = agend.DeletedAgenda,
                                                  Classifications = agend.Classification,
                                                  SerialNumber = agend.SerialNumber,
                                                  Presenter = agend.Presenter,
                                                  SerialNumberType = agend.SerialNumberType,
                                                  SerialTitle = agend.SerialTitle,
                                                  SerialText = agend.SerialText
                                              })).Distinct().ToList();

                            if (agendaName.Count() > 0)
                            {
                                string ClassificationOld = "";
                                string ClassificationNew = "";
                                strMenu.Append("<div id='accordion'><ol class='ddrag'>");
                                for (int i = 0; i <= agendaName.Count() - 1; i++)
                                {
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
                                                             AgendaNote = subAgenda.AgendaNote,
                                                             DeletedAgenda = subAgenda.DeletedAgenda,
                                                             Presenter = subAgenda.Presenter,
                                                             SerialNumber = subAgenda.SerialNumber,
                                                             SerialNumberType = subAgenda.SerialNumberType,
                                                             SerialTitle = subAgenda.SerialTitle,
                                                             SerialText = subAgenda.SerialText

                                                         })).ToList();
                                    if (ClassificationOld != ClassificationNew)
                                    {
                                        ClassificationOld = ClassificationNew;
                                        if (agendaName[i].Classifications != "")
                                        {
                                            strMenu.Append("<h2><b>" + agendaName[i].Classifications + "</b></h2>");
                                        }
                                        //else
                                        //{
                                        //    strMenu.Append("<h2><b>Unclassified</b></h2>");

                                        //}
                                    }

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

                                    if (agendaName[i].UplaodedAgenda.Length > 0)
                                    {
                                        string[] strDeletedAgenda = agendaName[i].DeletedAgenda.Split(',');
                                        string[] agendaNames = agendaName[i].UplaodedAgenda.Split(',');
                                        string[] agendaPdfs = agendaName[i].AgendaNote.Split(',');

                                        //strMenu.Append("<li id=" + agendaId + " class='group' ><h3> <a  href=ViewAgenda.aspx?file=" + agendaName[i].UplaodedAgenda + " Target='_blank'> " + agendaName[i].AgendaName + "</a></h3>");

                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>  " + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");

                                        //for (int ii = 0; ii <= agendaNames.Count() - 1; ii++)
                                        //{
                                        //    if (!strDeletedAgenda.Contains(agendaNames[ii].Trim()))
                                        //    {
                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ii].Trim()) ? "Agenda.pdf" : agendaPdfs[ii].Trim();
                                        //        //    strMenu.Append("<a  href=ViewAgenda.aspx?file=" + agendaNames[ii].Trim() + " Target='_blank' >" + pdfName + " </a> <br/> ");
                                        //        strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ii].Trim().Substring(0, agendaNames[ii].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> <br/> ");
                                        //    }
                                        //}

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
                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                            strMenu.Append("<a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                        }
                                    }
                                    else
                                    {
                                        strMenu.Append("<li id=" + agendaId + " class='group' ><h3>" + SerialNumber + " " + agendaName[i].AgendaName + Presenter + "</h3>");
                                    }



                                    //attach sub agenda to parent agenda list element
                                    if (subAgendaName.Count() > 0)
                                    {
                                        Dictionary<string, int> objSerial = new Dictionary<string, int>();
                                        strMenu.Append("<div><ol  style='list-style:none;margin-left:-30px;' class=ddrag>");
                                        for (int j = 0; j <= subAgendaName.Count() - 1; j++)
                                        {
                                            Presenter = "";
                                            string serialNo = "";
                                            serialNo = CreateSubAgenda.GetTitles(subAgendaName[j].SerialTitle, subAgendaName[j].SerialNumber, subAgendaName[j].SerialText);
                                            if (serialNo.Trim().Length > 0)
                                            {
                                                serialNo = serialNo + " : ";
                                            }

                                            if (subAgendaName[j].Presenter.Trim().Length > 0)
                                            {
                                                Presenter = "(Presenter : " + subAgendaName[j].Presenter + ")";
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

                                            if (subAgendaName[j].UplaodedAgenda.Length > 0)
                                            {
                                                string[] strDeletedAgenda = subAgendaName[j].DeletedAgenda.Split(',');
                                                string[] agendaNames = subAgendaName[j].UplaodedAgenda.Split(',');
                                                string[] agendaPdfs = subAgendaName[j].AgendaNote.Split(',');

                                                bool SendPdfFile = false;
                                                foreach (string str in agendaNames)
                                                {
                                                    if (!strDeletedAgenda.Contains(str))
                                                    {
                                                        SendPdfFile = true;
                                                    }
                                                }

                                                //strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><a href=ViewAgenda.aspx?file=" + subAgendaName[j].UplaodedAgenda + "  Target='_blank'> " + (i + 1) + "." + (j + 1) + " " + subAgendaName[j].AgendaName + "</a>");                                      

                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "> <span style='display:none'>" + (i + 1) + "." + (j + 1) + "</span> " + serialNo + subAgendaName[j].AgendaName + Presenter + "");
                                                //for (int ll = 0; ll <= agendaNames.Count() - 1; ll++)
                                                //{
                                                //    if (!strDeletedAgenda.Contains(agendaNames[ll].Trim()))
                                                //    {
                                                //        string pdfName = string.IsNullOrEmpty(agendaPdfs[ll].Trim()) ? "Agenda.pdf" : agendaPdfs[ll].Trim();
                                                //        strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[ll].Trim().Substring(0, agendaNames[ll].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                //    }

                                                //}
                                                if (agendaNames.Count() > 0 && SendPdfFile)
                                                {
                                                    string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                    strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                }
                                            }
                                            else
                                            {
                                                strMenu.Append("<li id=" + subAgendaName[j].AgendaId + "><span style='display:none'> " + (i + 1) + "." + (j + 1) + "</span> " + serialNo + subAgendaName[j].AgendaName + Presenter);// + "</li>");
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
                                                                        AgendaNote = subAgenda.AgendaNote,
                                                                        DeletedAgenda = subAgenda.DeletedAgenda,
                                                                        Presenter = subAgenda.Presenter,
                                                                        SerialNumber = subAgenda.SerialNumber,
                                                                        SerialNumberType = subAgenda.SerialNumberType,
                                                                        SerialTitle = subAgenda.SerialTitle,
                                                                        SerialText = subAgenda.SerialText
                                                                    })).ToList();
                                            if (subSubAgendaName.Count() > 0)
                                            {
                                                Dictionary<string, int> objSerialSub = new Dictionary<string, int>();
                                                //attach sub sub agenda to parent agenda list element
                                                strMenu.Append("<ol class=inddrag  style='list-style: lower-alpha outside none;margin-left:20px;padding-left:10px;'>");
                                                for (int y = 0; y <= subSubAgendaName.Count() - 1; y++)
                                                {
                                                    Presenter = "";

                                                    if (subSubAgendaName[y].Presenter.Trim().Length > 0)
                                                    {
                                                        Presenter = "(Presenter : " + subSubAgendaName[y].Presenter + ")";
                                                    }

                                                    string serialNoSub = "";
                                                    serialNoSub = CreateSubAgenda.GetTitles(subSubAgendaName[y].SerialTitle, subSubAgendaName[y].SerialNumber, subSubAgendaName[y].SerialText);
                                                    if (serialNoSub.Trim().Length > 0)
                                                    {
                                                        serialNoSub = serialNoSub + " : ";
                                                    }
                                                    //if (subSubAgendaName[y].SerialNumber.Trim().Length > 0)
                                                    //{
                                                    //    if (objSerialSub.ContainsKey(subSubAgendaName[y].SerialNumber))
                                                    //    {
                                                    //        if (subSubAgendaName[y].SerialNumberType != "Other")
                                                    //        {
                                                    //            objSerialSub[subSubAgendaName[y].SerialNumber] += 1;
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
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
                                                    //            objSerialSub.Add(subSubAgendaName[y].SerialNumber, 1);
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " - " + objSerialSub[subSubAgendaName[y].SerialNumber] + " : ";
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            serialNoSub = subSubAgendaName[y].SerialNumber + " : ";
                                                    //        }
                                                    //    }
                                                    //}


                                                    if (subSubAgendaName[y].UplaodedAgenda.Length > 0)
                                                    {
                                                        string[] strDeletedAgenda = subSubAgendaName[y].DeletedAgenda.Split(',');
                                                        string[] agendaNames = subSubAgendaName[y].UplaodedAgenda.Split(',');
                                                        string[] agendaPdfs = subSubAgendaName[y].AgendaNote.Split(',');
                                                        //strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "><a href=ViewAgenda.aspx?file=" + subSubAgendaName[y].UplaodedAgenda + "  Target='_blank'> " + subSubAgendaName[y].AgendaName + "</a>");

                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + "> " + serialNoSub + subSubAgendaName[y].AgendaName + Presenter + "");

                                                        //for (int pp = 0; pp <= agendaNames.Count() - 1; pp++)
                                                        //{
                                                        //    if (!strDeletedAgenda.Contains(agendaNames[pp].Trim()))
                                                        //    {
                                                        //        string pdfName = string.IsNullOrEmpty(agendaPdfs[pp].Trim()) ? "Agenda.pdf" : agendaPdfs[pp].Trim();
                                                        //        //strMenu.Append("<br/> <a  href=ViewAgenda.aspx?file=" + agendaNames[pp].Trim() + " Target='_blank' >" + pdfName + " </a> ");
                                                        //        strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[pp].Trim().Substring(0, agendaNames[pp].Trim().Length - 4)) + ".aspx Target='_blank' >" + pdfName + " </a> ");
                                                        //    }
                                                        //}

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
                                                            string pdfName = string.IsNullOrEmpty(agendaPdfs[0].Trim()) ? "Agenda.pdf" : agendaPdfs[0].Trim();
                                                            strMenu.Append("<br/> <a  href=meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[0].Trim().Substring(0, agendaNames[0].Trim().Length - 4)) + "_merge.aspx Target='_blank' >" + pdfName + " </a> ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strMenu.Append("<li id=" + subSubAgendaName[y].AgendaId + ">" + serialNoSub + subSubAgendaName[y].AgendaName + Presenter);
                                                    }
                                                }
                                                strMenu.Append("</ol>");
                                            }
                                            else
                                            {
                                                //   strMenu.Append("</li>");
                                            }
                                            strMenu.Append("</li>");
                                        }
                                        strMenu.Append("</ol></div></li>");
                                    }
                                    else
                                    {
                                        strMenu.Append("</li>");
                                    }

                                }
                                strMenu.Append("</ol></div>");
                            }


                            //  lblList.Text = strMenuList;
                        }
                        lblDetails.Text = strMenu.ToString();
                        lblAlert.Text = " <script> ShowPopUp(); </script>";
                        hdnPopUp.Value = "1";

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

    public class AgendaAck
    {
        public string Suffix { get; set; }
        public string Name { get; set; }
        public bool IsNoticeRead { get; set; }
        public bool AgendaRead { get; set; }
        public DateTime PublishDate { get; set; }

        public DateTime? AgendaReadOn { get; set; }

        public DateTime? NoticeReadOn { get; set; }
    }
}