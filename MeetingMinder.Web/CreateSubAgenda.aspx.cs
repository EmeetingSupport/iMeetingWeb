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
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
using iTextSharp.text.pdf;
using System.Security.Cryptography;
using System.Reflection;
using System.Web.Helpers;

namespace MeetingMinder.Web
{
    public partial class CreateSubAgenda : System.Web.UI.Page
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
                //Bind sub agenda
                BindSubAgenda();

                BindSerialNumber();

                //if (Request.UrlReferrer.ToString() != null)
                //{
                //    string previousUrl = Request.UrlReferrer.ToString();
                //    ViewState["previousUrl"] = previousUrl;
                //}
                //else
                //{
                //    Response.Redirect("default.aspx");
                //}

                //Bind all agenda items
                //   BindAllAgenda();
            }
        }

        /// <summary>
        /// Bind serial No
        /// </summary>
        private void BindSerialNumber()
        {
            try
            {
                IList<SerialNumberDomain> objSerialNumber = SerialNumberDataProvider.Instance.Get().OrderBy(p => p.SerialNumber).ToList();
                ddlSerialNumber.DataSource = objSerialNumber;
                ddlSerialNumber.DataBind();
                ddlSerialNumber.DataTextField = "SerialNumber";
                ddlSerialNumber.DataBind();
                ddlSerialNumber.Items.Insert(0, new ListItem("Select ", "0"));

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
        /// Bind all agenda items
        /// </summary>
        private void BindAllAgenda(Guid ForumId)
        {
            try
            {
                IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetAllAgendaTitles(ForumId);
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
        /// Bind sub agenda to grid view
        /// </summary>
        private void BindSubAgenda()
        {
            try
            {
                if (Session["AgendaId"] != null) //(Request.QueryString["id"] != null)
                {
                    //  int pageNo = 0;
                    string AgendaId = Convert.ToString(Session["AgendaId"]); //Convert.ToString(Request.QueryString["id"]);
                    IList<AgendaDomain> objAgenda = AgendaDataProvider.Instance.GetSubAgendaByAgendaId(Guid.Parse(AgendaId));

                    IList<AgendaDomain> objAgendaDetails = AgendaDataProvider.Instance.GetAgendabyMeetingId(objAgenda[0].MeetingId);

                    BindAllAgenda(objAgendaDetails[0].ForumId);
                    ViewState["entityId"] = objAgendaDetails[0].EntityId;

                    ViewState["MeetingId"] = objAgenda[0].MeetingId;

                    List<AgendaDomain> agenda = (from p in objAgenda
                                                 where (p.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                                 select p).ToList();

                    var serial = (from p in objAgendaDetails
                                  where (p.ParentAgendaId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                  select new { SerialNumber = p.SerialNumber }).Distinct().OrderBy(p => p.SerialNumber).ToList();

                    ddlSerialTitle.DataSource = serial;
                    ddlSerialTitle.DataBind();
                    ddlSerialTitle.DataTextField = "SerialNumber";
                    ddlSerialTitle.DataBind();
                    ddlSerialTitle.Items.Insert(0, new ListItem("Select ", "0"));

                    var objParentAgenda = from agend in objAgenda
                                          where (agend.ParentAgendaId == Guid.Parse(AgendaId))
                                          select agend;

                    var joints = from q in objAgendaDetails
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
                                     ParentAgenda.Presenter,
                                     ParentAgenda.SerialText,
                                     ParentAgenda.SerialTitle,
                                     //Serial = (string.IsNullOrWhiteSpace(ParentAgenda.SerialTitle) ? String.Empty  : ParentAgenda.SerialTitle + (string.IsNullOrWhiteSpace(ParentAgenda.SerialNumber) ? string.Empty : "-"+ ParentAgenda.SerialNumber) + (string.IsNullOrWhiteSpace(ParentAgenda.SerialText)? string.Empty: ParentAgenda.SerialText)),
                                     //   a = (string.IsNullOrWhiteSpace(ParentAgenda.SerialTitle) ? String.Empty : ParentAgenda.SerialTitle + (string.IsNullOrWhiteSpace(ParentAgenda.SerialNumber) ? string.Empty : "-" + ParentAgenda.SerialNumber) + (string.IsNullOrWhiteSpace(ParentAgenda.SerialText) ? string.Empty : ParentAgenda.SerialText)),
                                     HasChild = (subpet == null ? String.Empty : " <b style='color:green; top:1px;' title='This item has sub sub agenda'>+</b>")
                                 }).Distinct();

                    hylnkAgenda.NavigateUrl = "AgendaMaster.aspx?id=" + objAgenda[0].MeetingId;

                    lblAgenda.Text = agenda[0].AgendaName;
                    objAgenda.Remove(agenda[0]);


                    if (Session["SubAgendaId"] != null) //(Request.QueryString["aid"] != null)
                    {
                        Guid AgendasId = Guid.Empty;
                        try
                        {
                            if (Guid.TryParse(Convert.ToString(Session["SubAgendaId"]), out AgendasId)) //(Guid.TryParse(Request.QueryString["aid"].ToString(), out AgendasId))
                            {
                                List<AgendaDomain> objAgendaDomainSingle = objAgenda.ToList<AgendaDomain>();

                                int index = objAgendaDomainSingle.FindIndex(a => a.AgendaId == AgendasId);

                                if (index > 0)
                                {
                                    double pageindex = index / 10;
                                    grdSubAgenda.PageIndex = Convert.ToInt32(Math.Round(pageindex));
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

                    grdSubAgenda.DataSource = query.AsDataTable();//objAgenda;
                    // grdSubAgenda.PageIndex = pageNo;
                    grdSubAgenda.DataBind();
                    ViewState["AgendaId"] = AgendaId;

                    //Store date and time for avoid past editing
                    ViewState["MeetingTime"] = objAgendaDetails[0].MeetingTime;
                    ViewState["MeetingDate"] = objAgendaDetails[0].MeetingDate;

                    lblMeeting.Text = Convert.ToDateTime(objAgendaDetails[0].MeetingDate + " " + objAgendaDetails[0].MeetingTime).ToString("f");

                    lblForum.Text = MM.Core.Encryptor.DecryptString(objAgendaDetails[0].ForumName);

                }
                else
                {
                    Response.Redirect("CreateAgenda.aspx");
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
        /// Button submit click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (txtSubAgenda.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtSubAgenda);
                    if (!objUser.isValidChar(txtSubAgenda.Text))
                    {
                        txtSubAgenda.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSubAgenda.Text))
                    {
                        txtSubAgenda.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid sub agenda.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter sub agenda";
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

                if (txtSerialText.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtSerialText);
                    if (!objUser.isValidChar(txtSerialText.Text))
                    {
                        txtSerialText.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                    if (!objUser.CSVValidation(txtSerialText.Text))
                    {
                        txtSerialText.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid serial no.";
                        Error.Visible = true;
                        return;
                    }
                }



                DateTime dtNow = DateTime.Now;
                DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]) + " " + Convert.ToString(ViewState["MeetingTime"]));
                if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't add agenda to past meetings. ";
                    Error.Visible = true;
                    return;
                }

                AgendaDomain objAgenda = new AgendaDomain();

                string UploadOn = "";
                string AgendaFile = "";
                string AgendaFileInsert = "";

                if (ddlSerialTitle.SelectedValue != "0")
                {
                    objAgenda.SerialTitle = ddlSerialTitle.SelectedItem.Text;
                }

                objAgenda.SerialText = txtSerialText.Text;

                objAgenda.AgendaName = txtSubAgenda.Text;
                objAgenda.AgendaNote = txtAgendaNote.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.Presenter = txtPresenter.Text;
                // objAgenda.UploadedAgendaNote = txtSubAgenda.Text;

                objAgenda.SerialNumber = "";
                if (ddlSerialNumber.SelectedValue != "0")
                {
                    objAgenda.SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                }

                if (ddlSerialNumber.SelectedValue.ToLower().Equals("other"))
                {
                    objAgenda.SerialNumber = txtOtherSerialNo.Text.Trim();
                    objAgenda.SerialNumberType = "Other";
                }


                objAgenda.ParentAgendaId = Guid.Parse(ViewState["AgendaId"].ToString());

                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ViewState["MeetingId"]));

                string EncryptionKey = Session["EncryptionKey"].ToString();
                if (hdnAgendaId.Value != "" && hdnAgendaId.Value.Length == 36)
                {
                    System.Data.DataSet ds = AgendaDataProvider.Instance.GetAgendKeys(Guid.Parse(hdnAgendaId.Value));
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        EncryptionKey = MM.Core.Encryptor.DecryptString(ds.Tables[0].Rows[0]["EncryptionKey"].ToString());
                    }
                }

                StringBuilder sbAgendaNames = new StringBuilder("");
                StringBuilder sbPdfNames = new StringBuilder("");

                string agendaName;
                bool isUplaoded = false;
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
                            string ext = Path.GetExtension(Request.Files[i].FileName);
                            if (contentType.ToLower().Contains("application/pdf"))
                            {
                                if (ext.ToLower().Equals(".pdf"))
                                {
                                    //string pdfPassword;
                                    //isUplaoded = true;
                                    //agendaName = EncryptionHelper.GetPdfName(out pdfPassword) + ext;
                                    ////Guid.NewGuid() + ext; //14 Oct 2014
                                    ////DateTime.Now.Ticks + ext;  //sep 2014
                                    //// Request.Files[i].SaveAs(Server.MapPath(savePath + agendaName));

                                    //using (Stream input = Request.Files[i].InputStream)//new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                                    //{
                                    //    using (Stream output = new FileStream(Server.MapPath(savePath + agendaName), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                                    //    {
                                    //        PdfReader reader = new PdfReader(input);
                                    //        PdfEncryptor.Encrypt(reader, output, true, pdfPassword, pdfPassword, PdfWriter.ALLOW_MODIFY_ANNOTATIONS);

                                    //    }
                                    //}

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
                                    //isUplaoded = true;
                                    // agendaName = Guid.NewGuid() + ext;//DateTime.Now.Ticks + ext;
                                    //  Request.Files[i].SaveAs(Server.MapPath(savePath + agendaName));
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
                        UploadOn = DateTime.Now.ToString();

                        //sbAgendaNames.ToString().TrimEnd(',');// (sbAgendaNames.ToString().LastIndexOf(','));
                        //sbPdfNames.ToString().TrimEnd(',');
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

                bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                if (isMaker)
                {
                    //create Merged pdf
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
                        //string agendaName;
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
                        //        AgendaFile = fuAgenda.FileName;

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
                        ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully";

                        var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                        var encrypt = Encryptor.EncryptString(json);

                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.UpdatedBy), "Sub Agenda", "Success", Convert.ToString(objAgenda.AgendaId), "Agenda updated successfully :- " + encrypt + "");

                        Info.Visible = true;
                        BindSubAgenda();
                        ClearData();
                    }
                    //Insert
                    else
                    {

                        // check add permission
                        if (objUser.IsAdd(Guid.Parse(ViewState["entityId"].ToString())))
                        {
                            //string agendaName;
                            //if (fuAgenda.HasFile)
                            //{
                            //    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
                            //    string contentType = fuAgenda.PostedFile.ContentType;

                            //    string ext = Path.GetExtension(fuAgenda.FileName);
                            //    if (ext.ToLower().Equals(".pdf"))
                            //    {
                            //        agendaName = DateTime.Now.Ticks + ext;
                            //        AgendaFileInsert = fuAgenda.FileName;
                            //        fuAgenda.PostedFile.SaveAs(Server.MapPath(savePath + agendaName));

                            //        objAgenda.UploadedAgendaNote = agendaName;


                            //        //Delete existing image
                            //        if (ViewState["file"] != null)
                            //        {
                            //            //string fileName = Convert.ToString(ViewState["file"]);
                            //            //File.Delete(Server.MapPath(savePath + fileName));
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
                            AgendaDataProvider.Instance.Insert(objAgenda, AgendaFileInsert);
                            ((Label)Info.FindControl("lblName")).Text = "Sub Agenda inserted successfully";

                            var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                            var encrypt = Encryptor.EncryptString(json);

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Sub Agenda", "Success", Convert.ToString(objAgenda.AgendaId), "Sub Agenda inserted successfully :- " + encrypt + "");

                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Sub Agenda", "Failed", Convert.ToString(objAgenda.AgendaId), "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                            Info.Visible = false;
                        }
                    }
                    Info.Visible = true;
                    BindSubAgenda();
                    ClearData();
                }
                else
                {
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
            txtAgendaNote.Text = "";
            txtSubAgenda.Text = "";
            hdnAgendaId.Value = "";
            btnSubmit.Text = "Submit";

            hdnAgendaId.Value = "";
            ViewState["file"] = null;
            lnkViewPdf.Visible = false;
            lblPdfs.Text = "";
            txtPresenter.Text = "";
            ddlSerialNumber.SelectedIndex = -1;
            ViewState["delFile"] = null;
            rfvOtherSerialNo.Enabled = false;
            txtOtherSerialNo.Attributes.Add("style", "width:230px;display:none;");
            txtOtherSerialNo.Text = "";

            ddlSerialTitle.SelectedIndex = -1;
            txtSerialText.Text = "";
        }

        /// <summary>
        /// grid view row command event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdSubAgenda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                UserAcess objUser = new UserAcess();
                if (e.CommandName.ToLower().Equals("del"))
                {
                    bool isMaker = Convert.ToBoolean(Session["IsMaker"].ToString());
                    if (isMaker)
                    {
                        DateTime dtNow = DateTime.Now;
                        DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]) + " " + Convert.ToString(ViewState["MeetingTime"]));
                        if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry you can't edit past meetings agenda. ";
                            Error.Visible = true;
                            return;
                        }
                        // check delete permission
                        if (objUser.isDelete(Guid.Parse(ViewState["entityId"].ToString())))
                        {
                            string AgendaId = Convert.ToString(e.CommandArgument);
                            string[] ids = AgendaId.Split(',');
                            //if (ids.Count() == 2)
                            //{


                            //    //Delete existing image                                               
                            //    // File.Delete(Server.MapPath(savePath + ids[1]));
                            //}

                            string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                            AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(Guid.Parse(ids[0]));
                            bool status = AgendaDataProvider.Instance.Delete(Guid.Parse(ids[0]));
                            ((Label)Info.FindControl("lblName")).Text = "Agenda deleted successfully";

                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.CreatedBy), "Sub Agenda", "Success", Convert.ToString(Guid.Parse(ids[0])), "Agenda deleted successfully");

                            if (!string.IsNullOrEmpty(objAgenda.UploadedAgendaNote))
                            {
                                string[] agendaNames = objAgenda.UploadedAgendaNote.Split(',');
                                string[] agendaPdfs = objAgenda.AgendaNote.Split(',');

                                for (int i = 0; i <= agendaNames.Count() - 1; i++)
                                {
                                    File.Delete(Server.MapPath(savePath + agendaNames[i]));
                                }
                            }
                            BindSubAgenda();
                            Info.Visible = true;
                            ClearData();
                        }
                        else
                        {
                            //web logs
                            UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Sub Agenda", "Failed", "", "Sorry access denied");

                            ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                            Error.Visible = true;
                        }
                    }
                    else
                    {
                        //web logs
                        UserDataProvider.Instance.InsertWebLog(Convert.ToString(Guid.Parse(Session["UserId"].ToString())), "Sub Agenda", "Failed", "", "Sorry you are not maker");

                        ((Label)Error.FindControl("lblError")).Text = " Sorry you are not maker ";
                        Error.Visible = true;
                    }
                }

                if (e.CommandName.ToLower().Equals("editagenda"))
                {
                    if (objUser.IsEdit(Guid.Parse(ViewState["entityId"].ToString())))
                    {

                        DateTime dtNow = DateTime.Now;
                        DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]) + " " + Convert.ToString(ViewState["MeetingTime"]));
                        if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                        {
                            ((Label)Error.FindControl("lblError")).Text = " Sorry you can't delete past meetings agenda. ";
                            Error.Visible = true;
                            return;
                        }

                        string strAgendaId = Convert.ToString(e.CommandArgument);
                        EditAgenda(Guid.Parse(strAgendaId));
                        ////AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(Guid.Parse(strAgendaId));

                        ////hdnAgendaId.Value = objAgenda.AgendaId.ToString();

                        ////txtAgendaNote.Text = objAgenda.AgendaNote;
                        ////txtSubAgenda.Text = objAgenda.AgendaName;

                        ////if (objAgenda.UploadedAgendaNote.Length > 0)
                        ////{
                        ////    string[] strDeletedAgenda = objAgenda.DeletedAgenda.Split(',');
                        ////    StringBuilder sbAgenda = new StringBuilder("");
                        ////    string[] agendaNames = objAgenda.UploadedAgendaNote.Split(',');
                        ////    string[] agendaPdfs = objAgenda.AgendaNote.Split(',');
                        ////    if (objAgenda.UploadedAgendaNote != "")
                        ////    {
                        ////        ViewState["file"] = objAgenda.UploadedAgendaNote;
                        ////    }

                        ////    if (objAgenda.AgendaNote != "")
                        ////    {
                        ////        ViewState["pdf"] = objAgenda.AgendaNote;
                        ////    }

                        ////    if (objAgenda.DeletedAgenda != "")
                        ////    {
                        ////        ViewState["delFile"] = objAgenda.DeletedAgenda;
                        ////    }
                        ////    for (int i = 0; i <= agendaNames.Count() - 1; i++)
                        ////    {
                        ////        if (!strDeletedAgenda.Contains(agendaNames[i].Trim()))
                        ////        {
                        ////            //  sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        ////            sbAgenda.Append(" <a href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        ////        }
                        ////        //   sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i] + "') >" + agendaPdfs[i] + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i] + "') >Delete </a><br/> ");
                        ////        //  sbAgenda.Append("<a href='javascript:void(0)' onclick='ViewAgendaPdf('" + agendaNames[i] + "')' >" + agendaPdfs[i] + "</a>&nbsp;&nbsp;<a href='javascript:void(0)' onclick='DelAgendaPdf('" + agendaNames[i] + "')'>Delete</a><br/>");
                        ////    }

                        ////    lblPdfs.Text = sbAgenda.ToString();
                        ////    lnkViewPdf.Visible = true;
                        ////}
                        ////btnSubmit.Text = "Update";
                    }
                    else
                    {
                        ((Label)Error.FindControl("lblError")).Text = " Sorry access denied ";
                        Error.Visible = true;
                    }
                }
                if (e.CommandName.ToLower().Equals("add"))
                {
                    Session["SubAgendaId"] = Convert.ToString(e.CommandArgument);
                    Response.Redirect("CreateSubSubAgenda.aspx");
                }
                if (e.CommandName.ToLower().Equals("download"))
                {
                    string fileName = Convert.ToString(e.CommandArgument);
                    string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);

                    //Set the appropriate ContentType.
                    Response.ContentType = "application/octet-stream";
                    //Get the physical path to the file.
                    string FilePath = Server.MapPath(savePath + fileName);
                    //Write the file directly to the HTTP content output stream.

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.WriteFile(FilePath);
                    Response.End();
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
        /// Grid view page index change event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">GridViewPageEventArgs specifying e </param>
        protected void grdSubAgenda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdSubAgenda.PageIndex = e.NewPageIndex;
                BindSubAgenda();
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
                string fileName = hdnDeletePdfName.Value; //Convert.ToString(ViewState["file"]);

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
        /// Back button click event
        /// </summary>
        /// <param name="sender">Object specifying sender</param>
        /// <param name="e">EventArgs specifying e </param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //string url = Convert.ToString(ViewState["previousUrl"]);
            //if (url != null && url != "")
            //{
            //    Response.Redirect(url);
            //}
            //else
            //{
            //    Response.Redirect("default.aspx");
            //}
            try
            {
                if (ViewState["MeetingId"] != null)
                {
                    Guid MeetingId = Guid.Parse(ViewState["MeetingId"].ToString());
                    Session["AgendaOrder"] = MeetingId;

                    // string AgendaId = Convert.ToString(Request.QueryString["id"]);
                    Response.Redirect("CreateAgenda.aspx");//?id=" + MeetingId + "&aid=" + AgendaId);
                }
                Response.Redirect("CreateAgenda.aspx");
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;
                //Info.Visible = false;

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
                if (txtSubAgenda.Text != "")
                {
                    objUser.TextHtmlEncode(ref txtSubAgenda);
                    if (!objUser.isValidChar(txtSubAgenda.Text))
                    {
                        txtSubAgenda.Text = "";
                        ((Label)Error.FindControl("lblError")).Text = " Please enter valid characters only.";
                        Error.Visible = true;
                        return;
                    }
                }
                else
                {
                    ((Label)Error.FindControl("lblError")).Text = " Please enter sub agenda";
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
                DateTime dtMeetingDate = Convert.ToDateTime(Convert.ToString(ViewState["MeetingDate"]) + " " + Convert.ToString(ViewState["MeetingTime"]));
                if (dtMeetingDate.AddDays(1).Date < dtNow.Date)
                {
                    ((Label)Error.FindControl("lblError")).Text = " Sorry you can't edit past meetings agenda. ";
                    Error.Visible = true;
                    return;
                }


                AgendaDomain objAgenda = new AgendaDomain();

                objAgenda.AgendaName = txtSubAgenda.Text;
                objAgenda.AgendaNote = txtAgendaNote.Text;

                objAgenda.PublishedBy = Guid.Parse(Session["UserId"].ToString());
                objAgenda.UpdatedBy = Guid.Parse(Session["UserId"].ToString());

                objAgenda.SerialNumber = "";
                if (ddlSerialNumber.SelectedValue != "0")
                {
                    objAgenda.SerialNumber = ddlSerialNumber.SelectedValue.ToString();
                }

                if (ddlSerialNumber.SelectedValue.ToLower().Equals("other"))
                {
                    objAgenda.SerialNumber = txtOtherSerialNo.Text.Trim();
                    objAgenda.SerialNumberType = "Other";
                }

                objAgenda.ParentAgendaId = Guid.Parse(ViewState["AgendaId"].ToString());

                objAgenda.MeetingId = Guid.Parse(Convert.ToString(ViewState["MeetingId"]));
                objAgenda.Presenter = txtPresenter.Text;

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
                    //File.Delete(Server.MapPath(savePath + hdnDeletePdfName.Value));
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

                //int pos = Array.IndexOf(fileNames, deletePdf);
                //if (pos > -1)
                //{
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

                if (status)
                {
                    ((Label)Info.FindControl("lblName")).Text = "Agenda updated successfully";

                    var json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objAgenda);
                    var encrypt = Encryptor.EncryptString(json);
                    //web logs
                    UserDataProvider.Instance.InsertWebLog(Convert.ToString(objAgenda.UpdatedBy), "Sub Agenda", "Success", Convert.ToString(objAgenda.AgendaId), "Agenda updated successfully :- " + encrypt + "");

                }
                //   BindAgendaMeeting();
                //ddlAgendaMeeting.SelectedValue = ddlMeeting.SelectedValue;
                //BindAgenda();
                BindSubAgenda();
                Info.Visible = true;
                EditAgenda(Guid.Parse(hdnAgendaId.Value));
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
        /// Get Pdf's uploaded 
        /// </summary>
        /// <param name="AgendaFiles">object Specifiying AgendaFiles</param>
        /// <param name="AgendPdf">object Specifiying AgendPdf</param>
        /// <param name="deletedAgenda">object Specifiying deletedAgenda</param>
        /// <returns></returns>
        protected string GetUploadedAgenda(object AgendaFiles, object AgendPdf, object deletedAgenda)
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
                        //sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agenda + " </a><br/> ");
                        sbAgenda.Append(" <a class='AddView' href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agenda + " </a><br/> ");

                    }
                }
            }
            return sbAgenda.ToString();
        }


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
                        // writer.CopyAcroForm(reader);
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
        /// Merge pdf files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="strFileName"></param>
        //public bool MergePdf(List<string> files, string strFileName)
        //{
        //    try
        //    {
        //        //// Open the output document
        //        //PdfDocument outputDocument = new PdfDocument();
        //        if (files.Count == 0)
        //        {
        //            return true;
        //        }

        //        string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Agenda"]);
        //        //// Iterate files
        //        //foreach (string file in files)
        //        //{
        //        //    //   System.Web.Hosting.HostingEnvironment.MapPath();
        //        //    string paths = Server.MapPath(savePath + "/" + file);
        //        //    //System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + file);//System.Web.HttpContext.Current.Server.MapPath(savePath + "/" + file);
        //        //    // Open the document to import pages from it.
        //        //    PdfDocument inputDocument = PdfReader.Open(paths, PdfDocumentOpenMode.Import);

        //        //    // Iterate pages
        //        //    int count = inputDocument.PageCount;
        //        //    for (int idx = 0; idx < count; idx++)
        //        //    {
        //        //        // Get the page from the external document...
        //        //        PdfPage page = inputDocument.Pages[idx];
        //        //        // ...and add it to the output document.
        //        //        outputDocument.AddPage(page);
        //        //    }
        //        //}

        //        //// Save the document...

        //        //outputDocument.Save(Server.MapPath(savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));
        //        ////System.Web.Hosting.HostingEnvironment.MapPath("~/" + savePath + "/" + strFileName.Split('.')[0] + "_merge.pdf"));


        //        iTextSharp.text.Document document = new iTextSharp.text.Document();

        //        string File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf");

        //        if (File.Exists(File_Name))
        //        {
        //            File.Delete(File_Name);
        //        }

        //        File_Name = Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf");

        //        if (File.Exists(File_Name))
        //        {
        //            File.Delete(File_Name);
        //        }

        //        // step 2: we create a writer that listens to the document
        //        PdfCopy writer = new PdfCopy(document, new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Create));

        //        if (writer == null)
        //        {
        //            return false;
        //        }

        //        // step 3: we open the document
        //        document.Open();
        //        Dictionary<string, string> dictKeys = null;
        //        Dictionary<string, string> dictEncrKeys = null; 
        //        if (hdnAgendaId.Value.Length == 36)
        //        {
        //            System.Data.DataSet ds = AgendaDataProvider.Instance.GetAgendKeys(Guid.Parse(hdnAgendaId.Value));
        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                dictKeys = ds.Tables[0].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
        //                          row => row.Field<string>(1));
        //                dictEncrKeys = ds.Tables[0].AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<string>(0),
        //                        row => row.Field<string>(2));
        //            }
        //        }

        //        bool pdfCreate = false;
        //        foreach (string fileName in files)
        //        {
        //            string AgendaKey = "";
        //            string AgendaEncrKey = "";
        //            AgendaEncrKey = Session["EncryptionKey"].ToString();
        //            if (dictKeys != null)
        //            {
        //                if (dictKeys.ContainsKey(fileName))
        //                {
        //                    AgendaKey = dictKeys[fileName];
        //                    AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[fileName]);
        //                }
        //            }

        //            PdfReader reader = null;

        //            string PdfPassword = EncryptionHelper.GetPassword(fileName, AgendaKey);
        //            if (PdfPassword != "")
        //            {
        //                byte[] password = System.Text.ASCIIEncoding.ASCII.GetBytes(PdfPassword);

        //                // we create a reader for a certain document
        //                using (AesCryptoServiceProvider acsp = EncryptionHelper.GetProvider(Encoding.Default.GetBytes(AgendaEncrKey)))
        //                {
        //                    ICryptoTransform ictD = acsp.CreateDecryptor();

        //                    //RawBytes now contains original byte array, still in Encrypted state

        //                    //Decrypt into stream
        //                    //  MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
        //                    using (FileStream msD = new FileStream(Server.MapPath(savePath + "/" + fileName), FileMode.Open))
        //                    {
        //                        using (CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read))
        //                        {
        //                            using (FileStream fsOutput = new FileStream(Server.MapPath(savePath + "/" + "oo" + fileName), FileMode.Create))
        //                            {
        //                                int data;
        //                                while ((data = csD.ReadByte()) != -1)
        //                                {
        //                                    fsOutput.WriteByte((byte)data);
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //                reader = new PdfReader(Server.MapPath(savePath + "/" + fileName), password);
        //            }
        //            else
        //            {
        //                reader = new PdfReader(Server.MapPath(savePath + "/" + fileName));
        //            }
        //            reader.ConsolidateNamedDestinations();

        //            // step 4: we add content
        //            for (int i = 1; i <= reader.NumberOfPages; i++)
        //            {
        //                PdfImportedPage page = writer.GetImportedPage(reader, i);
        //                writer.AddPage(page);
        //                pdfCreate = true;
        //            }

        //            PRAcroForm form = reader.AcroForm;
        //            if (form != null)
        //            {
        //                writer.CopyAcroForm(reader);
        //            }

        //            reader.Close();
        //            if (File.Exists(Server.MapPath(savePath + "/" + "oo" + fileName)))
        //            {
        //                File.Delete(Server.MapPath(savePath + "/" + "oo" + fileName));
        //            }
        //        }

        //        // step 5: we close the document and writer
        //        writer.Close();
        //        document.Close();

        //        if (pdfCreate)
        //        {
        //            string AgendaKey = "";
        //            string AgendaEncrKey = "";
        //            if (dictKeys != null)
        //            {
        //                if (dictKeys.ContainsKey(strFileName))
        //                {
        //                    AgendaKey = dictKeys[strFileName];
        //                    AgendaEncrKey = MM.Core.Encryptor.DecryptString(dictEncrKeys[strFileName]);
        //                }
        //            }
        //            string pdfPass = EncryptionHelper.GetPassword(strFileName, AgendaKey);
        //            if (pdfPass != "")
        //            {
        //                using (Stream input = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), FileMode.Open))//new FileStream(InputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //                {
        //                    using (Stream output = new FileStream(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        //                    {
        //                        PdfReader reader = new PdfReader(input);

        //                        PdfEncryptor.Encrypt(reader, output, true, pdfPass, pdfPass, PdfWriter.ALLOW_MODIFY_ANNOTATIONS);

        //                    }
        //                }
        //            }
        //            else
        //            {
        //                System.IO.File.Move(Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge1.pdf"), Server.MapPath(savePath + "/" + strFileName.Trim().Substring(0, strFileName.Length - 4) + "_merge.pdf"));
        //            }

        //            if (File.Exists(File_Name))
        //            {
        //                File.Delete(File_Name);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ((Label)Error.FindControl("lblError")).Text = "Uploaded Pdf Error: " + ex.Message;//"Try again after some time";
        //        Error.Visible = true;

        //        LogError objEr = new LogError();
        //        objEr.HandleException(ex);

        //        return false;
        //    }
        //}

        private void EditAgenda(Guid AgendaId)
        {
            ClearData();
            AgendaDomain objAgenda = AgendaDataProvider.Instance.Get(AgendaId);

            hdnAgendaId.Value = objAgenda.AgendaId.ToString();

            txtAgendaNote.Text = objAgenda.AgendaNote;
            txtSubAgenda.Text = objAgenda.AgendaName;
            txtPresenter.Text = objAgenda.Presenter;

            if (ddlSerialTitle.Items.FindByValue(
Convert.ToString(objAgenda.SerialTitle)) != null)
            {
                ddlSerialTitle.SelectedValue = objAgenda.SerialTitle;
            }

            txtSerialText.Text = objAgenda.SerialText;

            //  ddlSerialNumber.SelectedValue = objAgenda.SerialNumber;


            if (ddlSerialNumber.Items.FindByValue(
Convert.ToString(objAgenda.SerialNumber)) != null)
            {
                ddlSerialNumber.SelectedValue = Convert.ToString(objAgenda.SerialNumber);
            }
            else
            {
                // ddlSerialNumber.SelectedValue = "Other";
                //txtOtherSerialNo.Text = objAgenda.SerialNumber;
                //txtOtherSerialNo.Attributes.Add("style", "width:230px;display:block;");
                //rfvOtherSerialNo.Enabled = true;
            }

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
                        //  sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i].Trim() + "') >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                        sbAgenda.Append(" <a href='meeting_agenda-" + HttpUtility.UrlEncode(agendaNames[i].Trim().Substring(0, agendaNames[i].Trim().Length - 4)) + ".aspx' >" + agendaPdfs[i].Trim() + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i].Trim() + "') >Delete </a><br/> ");
                    }
                    //   sbAgenda.Append(" <a href='javascript:void(0)' onclick=ViewAgendaPdf('" + agendaNames[i] + "') >" + agendaPdfs[i] + " </a>&nbsp;&nbsp; <a href='javascript:void(0)' onclick=DelAgendaPdf('" + agendaNames[i] + "') >Delete </a><br/> ");
                    //  sbAgenda.Append("<a href='javascript:void(0)' onclick='ViewAgendaPdf('" + agendaNames[i] + "')' >" + agendaPdfs[i] + "</a>&nbsp;&nbsp;<a href='javascript:void(0)' onclick='DelAgendaPdf('" + agendaNames[i] + "')'>Delete</a><br/>");
                }

                lblPdfs.Text = sbAgenda.ToString();
                lnkViewPdf.Visible = true;
            }
            btnSubmit.Text = "Update";
        }


        public static string GetTitles(string SerialAlhapa, string SerialTitle, string SerialNumber)
        {
            string Serial = "";
            //if (SerialAlhapa.Trim().Length > 0 && SerialTitle.Trim().Length > 0 && SerialNumber.Trim().Length > 0)
            //{
            //    return SerialAlhapa + "-" + SerialTitle + "-" + SerialNumber;
            //}
            //else if (SerialAlhapa.Trim().Length == 0 && SerialTitle.Trim().Length == 0 && SerialNumber.Trim().Length == 0)
            //{
            //    return "";
            //}

            if (SerialAlhapa.Trim().Length != 0)
            {
                Serial = SerialAlhapa;
            }

            if (SerialTitle.Trim().Length != 0)
            {
                if (Serial.Trim().Length == 0)
                {
                    Serial = SerialTitle;
                }
                else
                {
                    Serial = Serial + "-" + SerialTitle;
                }
            }

            if (SerialNumber.Trim().Length != 0)
            {

                if (Serial.Trim().Length == 0)
                {
                    Serial = SerialNumber;
                }
                else
                {
                    Serial = Serial + "-" + SerialNumber;
                }
            }
            return Serial;
        }
    }
}