using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using MM.Domain;
using MM.Data;
using System.Text;

namespace MeetingMinder.Web.EventCal
{
    public partial class eventCal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { //check user user session
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                BindAllVisitors();
            }
        }

        protected void fileUploadPhotoAdd_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string filename = e.FileName;
            string strDestPath = Server.MapPath("~/Photos/");
            //  fileUploadPhotoAdd.SaveAs(@strDestPath + Guid.NewGuid() + filename);
        }
        protected void fileUploadAttachmentsAdd_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string filename = e.FileName;
            string strDestPath = Server.MapPath("~/Files/");
            // fileUploadPhotoAdd.SaveAs(@strDestPath + Guid.NewGuid() + filename);
        }
        [System.Web.Services.WebMethod(true)]

        public static string UpdateEvent(ScheduleEvent sevent)
        {

            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
            if (idList != null && idList.Contains(sevent.id))
            {
                //if (CheckAlphaNumeric(sevent.nameOfVisitor) && CheckAlphaNumeric(sevent.designtionOfVisitor) && CheckAlphaNumeric(sevent.nameOfVisitorOrganisation) && CheckAlphaNumeric(sevent.appointment) && CheckAlphaNumeric(sevent.nameDesignation) && CheckAlphaNumeric(sevent.uploadPhoto) && CheckAlphaNumeric(sevent.attachments) && CheckAlphaNumeric(sevent.information))
                //{
                SchedulerDAO.updateEvent(sevent.id, sevent.nameOfVisitor, sevent.designtionOfVisitor, sevent.nameOfVisitorOrganisation, sevent.appointment, sevent.nameDesignation, sevent.uploadPhoto, sevent.attachments, sevent.information, sevent.start.ToString(), sevent.end.ToString(), sevent.MOM);

                return "updated event with id:" + sevent.id + " update NameOfVisitor to: " + sevent.nameOfVisitor + " update DesignationOfVisitor to: " + sevent.designtionOfVisitor + " update NameOfVisitorOrganisation to: " + sevent.nameOfVisitorOrganisation + " update Appointment to: " + sevent.appointment + " update NameDesignation to: " + sevent.nameDesignation + " update UploadPhoto to: " + sevent.uploadPhoto + " update Attachments to: " + sevent.attachments + " update Information to: " + sevent.information;
                //   }

            }

            return "unable to update event with id:" + sevent.id + " NameOfVisitor : " + sevent.nameOfVisitor + " DesignationOfVisitor : " + sevent.designtionOfVisitor + "NameOfVisitorOrganisation : " + sevent.nameOfVisitorOrganisation + " Appointment : " + sevent.appointment + " NameDesignation: " + sevent.nameDesignation + " UploadPhoto: " + sevent.uploadPhoto + " Attachments : " + sevent.attachments + " Information: " + sevent.information;

        }


        [System.Web.Services.WebMethod(true)]
        public static string UpdateEventTime(ImproperSchedulerEvent improperEvent)
        {
            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
            if (idList != null && idList.Contains(improperEvent.id))
            {
                SchedulerDAO.updateEventTime(improperEvent.id,
                    DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture));

                return "updated event with id:" + improperEvent.id + "update start to: " + improperEvent.start +
                    " update end to: " + improperEvent.end;
            }

            return "unable to update event with id: " + improperEvent.id;
        }


        [System.Web.Services.WebMethod(true)]
        public static String deleteEvent(int id)
        {
            //idList is stored in Session by JsonResponse.ashx for security reasons
            //whenever any event is update or deleted, the event id is checked
            //whether it is present in the idList, if it is not present in the idList
            //then it may be a malicious user trying to delete someone elses events
            //thus this checking prevents misuse
            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
            if (idList != null && idList.Contains(id))
            {
                SchedulerDAO.deleteEvent(id);
                return "deleted event with id:" + id;
            }

            return "unable to delete event with id: " + id;

        }

        [System.Web.Services.WebMethod]
        public static int addEvent(ImproperSchedulerEvent improperEvent)
        {

            ScheduleEvent sevent = new ScheduleEvent()
            {
                nameOfVisitor = improperEvent.nameOfVisitor,
                designtionOfVisitor = improperEvent.designtionOfVisitor,
                nameOfVisitorOrganisation = improperEvent.nameOfVisitorOrganisation,
                appointment = improperEvent.appointment,
                nameDesignation = improperEvent.nameDesignation,
                uploadPhoto = improperEvent.uploadPhoto,
                attachments = improperEvent.attachments,
                information = improperEvent.information,

                start = DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                end = DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                MOM = improperEvent.MOM

            };

            //if (CheckAlphaNumeric(sevent.nameOfVisitor) && CheckAlphaNumeric(sevent.designtionOfVisitor) && CheckAlphaNumeric(sevent.nameOfVisitorOrganisation) && CheckAlphaNumeric(sevent.appointment) && CheckAlphaNumeric(sevent.nameDesignation) && CheckAlphaNumeric(sevent.uploadPhoto) && CheckAlphaNumeric(sevent.attachments)  && CheckAlphaNumeric(sevent.information))
            //{
            int key = SchedulerDAO.addEvent(sevent);

            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];

            if (idList != null)
            {
                idList.Add(key);
            }
            //else
            //{
            //    idList = new List<int>();
            //    idList.Add(key);
            //    System.Web.HttpContext.Current.Session["idList"] = idList;
            //}
            return key;//return the primary key of the added cevent object

            // }

            // return -1;//return a negative number just to signify nothing has been added

        }

        /// <summary>
        /// Bind visitors
        /// </summary>
        private void BindAllVisitors()
        {
            try
            {
                IList<VisitorDomain> objVisitor = VisitoreDataProvider.Instance.Get(Guid.Parse(Session["UserId"].ToString())).OrderBy(p => p.Name_Of_Visitor).ToList();
                ddlAttendees.DataSource = objVisitor;
                ddlAttendees.DataBind();
                ddlAttendees.DataTextField = "Name_Of_Visitor";
                ddlAttendees.DataValueField = "VisiterId";
                ddlAttendees.DataBind();

                ddlAttendeesUpdate.DataSource = objVisitor;
                ddlAttendeesUpdate.DataBind();
                ddlAttendeesUpdate.DataTextField = "Name_Of_Visitor";
                ddlAttendeesUpdate.DataValueField = "VisiterId";
                ddlAttendeesUpdate.DataBind();
            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }
        private static bool CheckAlphaNumeric(string str)
        {

            return Regex.IsMatch(str, @"^[a-zA-Z0-9 ]*$");


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string webRootPath = Server.MapPath("~");
                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                if (eventId.Value != "")
                {
                    string ext = "";
                    string FileName = "";
              
                    if (fileUploadPhotosAdd.HasFile)
                    {
                        ext = Path.GetExtension(fuPhotoUp.FileName);//fileUploadPhotosAdd.FileName);
                        FileName = DateTime.Now.Ticks + ext;
                        fileUploadPhotosAdd.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileName));
                    }
                    string extAtt = "";
                    string FileNameAtt = "";
                    FileNameAtt = hdnFileName.Value.Trim();
                    if (fuAttUp.HasFile)
                    {
                        if (hdnFileName.Value != "")
                        {                           
                            try
                            {
                                File.Delete(Path.Combine(webRootPath + savePath + FileNameAtt));
                                extAtt = Path.GetExtension(fuAttUp.FileName);//fileUploadAttachmentsAdd.FileName);
                                FileNameAtt = DateTime.Now.Ticks + extAtt;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            extAtt = Path.GetExtension(fuAttUp.FileName);//fileUploadAttachmentsAdd.FileName);
                            FileNameAtt = DateTime.Now.Ticks + extAtt;
                        }
                        fuAttUp.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileNameAtt));
                    }
                    ScheduleEvent sevent = new ScheduleEvent()
                    {
                        id = Convert.ToInt32(eventId.Value),
                        nameOfVisitor = txtNameOfVisitorUpdate.Text,
                        designtionOfVisitor = txtDesignOfVisitorUpdate.Text,
                        nameOfVisitorOrganisation = txtNameOfVisitorOrgUpdate.Text,
                        appointment = txtAppointmentUpdate.Text,
                        nameDesignation = txtNameDesignationUpdate.Text,
                        uploadPhoto = FileName,
                        attachments = FileNameAtt,
                        information = txtInformationUpdate.Text,
                        start = Convert.ToDateTime(hdnEventStartUpdate.Value + " " + ddlStartTimeUp.Value), //DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        end = Convert.ToDateTime(hdnEventEndUpdate.Value + " " + ddlEndUp.Value), //DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                        MOM = txtMOMUpdate.Text.Trim()
                    };

                    SchedulerDAO.updateEvent(sevent.id, sevent.nameOfVisitor, sevent.designtionOfVisitor, sevent.nameOfVisitorOrganisation, sevent.appointment, sevent.nameDesignation, sevent.uploadPhoto, sevent.attachments, sevent.information, sevent.start.ToString(), sevent.end.ToString(), sevent.MOM);

                    StringBuilder sbAdd = new StringBuilder(",");
                    StringBuilder sbDelete = new StringBuilder(",");

                    List<string> oldAttendees = txtDesignOfVisitorUpdate.Text.Trim().Replace(" ", "").Split(',').ToList<string>();
                    List<string> newAttendees = hdnAtttendees.Value.Trim().Replace(" ", "").Split(',').ToList<string>();

                    List<string> objDeleteList = oldAttendees.Except(newAttendees).ToList();
                    List<string> objAddList = newAttendees.Except(oldAttendees).ToList();

                    foreach (string objId in objAddList)
                    {
                        sbAdd.Append(objId.ToString() + ",");
                    }

                    foreach (string objId in objDeleteList)
                    {
                        sbDelete.Append(objId.ToString() + ",");
                    }
                    VisitoreDataProvider.Instance.UpdateAttendees(Convert.ToInt64(eventId.Value), sbAdd.ToString(), sbDelete.ToString());
                    ddlAttendeesUpdate.SelectedIndex = -1;
                    ddlAttendees.SelectedIndex = -1;
                }

                else
                {
                    string edxt = "";
                    string FileNames = "";
                    string extsAtt = "";
                    string FileNameAtts = "";
                    if (fileUploadPhotosAdd.HasFile)
                    {
                        edxt = Path.GetExtension(fileUploadPhotosAdd.FileName);
                        FileNames = DateTime.Now.Ticks + edxt;
                        fileUploadPhotosAdd.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileNames));
                    }
                    if (fileUploadAttachmentsAdd.HasFile)
                    {
                        extsAtt = Path.GetExtension(fileUploadAttachmentsAdd.FileName);
                        FileNameAtts = DateTime.Now.Ticks + extsAtt;
                        fileUploadAttachmentsAdd.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileNameAtts));
                    }
                    ScheduleEvent sevenst = new ScheduleEvent()
                    {
                        nameOfVisitor = txtAddNameOfVisitor.Text,
                        designtionOfVisitor = txtAddDesignOfVisitor.Text,
                        nameOfVisitorOrganisation = txtAddNameOfVisitorOrg.Text,
                        appointment = txtAddAppointment.Text,
                        nameDesignation = txtAddNameDesignation.Text,
                        uploadPhoto = FileNames,
                        attachments = FileNameAtts,
                        information = AddtxtInformation.Text,
                        start = Convert.ToDateTime(hdnEventStartDate.Value + " " + ddlStartTime.Value),
                        // DateTime.ParseExact(hdnEventStartDate.Value + " " + ddlStartTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(Convert.ToDateTime(ddlMonth.Value + "/" + ddlDay.Value + "/" + ddlYear.Value + ", " + ddlStartTime.Value).ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(ddlMonth.Value + "/" + ddlDay.Value + "/" + ddlYear.Value + ", " + ddlStartTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        // DateTime.Now, //DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        end = Convert.ToDateTime(hdnEventEndDate.Value + " " + ddlEndTime.Value),
                        MOM = txtMOM.Text.Trim()
                        //DateTime.ParseExact(hdnEventEndDate.Value + " " + ddlEndTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        //DateTime.ParseExact(Convert.ToDateTime(ddlEndMonth.Value + "/" + ddlEndDay.Value + "/" + ddlEndYear.Value + ", " + ddlEndTime.Value).ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                        //DateTime.ParseExact(ddlEndMonth.Value + "/" + ddlEndDay.Value + "/" + ddlEndYear.Value + ", " + ddlEndTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        //start = DateTime.Now, //DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                        //end = DateTime.Now //DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                    };

                    int key = SchedulerDAO.addEvent(sevenst);

                    // System.Text.StringBuilder sbAtttendees = new System.Text.StringBuilder(",");
                    //foreach (ListItem lstItem in ddlAttendees.Items)
                    //{
                    //    if (lstItem.Selected)
                    //    {
                    //        sbAtttendees.Append(lstItem.Value + ",");
                    //    }
                    //}
                    string AgendaIds = hdnAtttendees.Value;
                    VisitoreDataProvider.Instance.AddAttendees(key, "," + hdnAtttendees.Value + ",");
                    List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];

                    if (idList != null)
                    {
                        idList.Add(key);
                    }

                    ddlAttendees.SelectedIndex = -1;
                    ddlAttendeesUpdate.SelectedIndex = -1;

                }
                ClearData();
            }
            catch (Exception ex)
            {
             
              //StringBuilder sb = new StringBuilder();
              //sb.Append("Message: " + ex.Message + Environment.NewLine);
              //sb.Append("Source: " + ex.Source + Environment.NewLine);
              //sb.Append("TargetSite: " + ex.TargetSite + Environment.NewLine);
              //sb.Append("StackTrace: " + ex.StackTrace + Environment.NewLine);
              //sb.Append(Environment.NewLine + Environment.NewLine+ ex.InnerException);

                Response.Write(ex.Message);
            }
        }

        private void ClearData()
        {
            txtAddNameOfVisitor.Text = "";
            txtAddDesignOfVisitor.Text = "";
            txtAddNameOfVisitorOrg.Text = "";
            txtAddAppointment.Text = "";
            txtAddNameDesignation.Text = "";
            AddtxtInformation.Text = "";
            hdnAtttendees.Value = "";
            eventId.Value = "";
            hdnFileName.Value = "";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string webRootPath = Server.MapPath("~");
                string savePath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EntityLogo"]);
                string ext = Path.GetExtension(fuPhotoUp.FileName);//fileUploadPhotosAdd.FileName);
                string FileName = DateTime.Now.Ticks + ext;
                fileUploadPhotosAdd.PostedFile.SaveAs(Server.MapPath(Path.Combine(webRootPath + savePath + FileName)));

                string extAtt = Path.GetExtension(fuAttUp.FileName);//fileUploadAttachmentsAdd.FileName);
                string FileNameAtt = DateTime.Now.Ticks + extAtt;
                fileUploadAttachmentsAdd.PostedFile.SaveAs(Path.Combine(webRootPath + savePath + FileNameAtt));
                //SaveAs(Server.MapPath(Path.Combine(webRootPath + savePath + FileNameAtt + extAtt)));
                ScheduleEvent sevent = new ScheduleEvent()
                {
                    //  id = Convert.ToInt32(eventId.Value),
                    nameOfVisitor = txtNameOfVisitorUpdate.Text,
                    designtionOfVisitor = txtDesignOfVisitorUpdate.Text,
                    nameOfVisitorOrganisation = txtNameOfVisitorOrgUpdate.Text,
                    appointment = txtAppointmentUpdate.Text,
                    nameDesignation = txtNameDesignationUpdate.Text,
                    uploadPhoto = FileName,
                    attachments = FileNameAtt,
                    information = txtInformationUpdate.Text,
                    start = DateTime.ParseExact(hdnEventEndDate.Value + " " + ddlStartTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    //DateTime.ParseExact(ddlMonth.Value+"/"+ddlDay.Value+"/"+ddlYear.Value+", " +ddlStartTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    // DateTime.Now, //DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    end = DateTime.ParseExact(hdnEventStartDate.Value + " " + ddlEndTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    //DateTime.ParseExact(ddlEndMonth.Value + "/" + ddlEndDay.Value + "/" + ddlEndYear.Value + ", " + ddlEndTime.Value, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    //DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                };

                //SchedulerDAO.updateEvent(sevent.id, sevent.nameOfVisitor, sevent.designtionOfVisitor, sevent.nameOfVisitorOrganisation, sevent.appointment, sevent.nameDesignation, sevent.uploadPhoto, sevent.attachments, sevent.information);

            }
            catch (Exception ex)
            {
                //((Label)Error.FindControl("lblError")).Text = "Try again after some time";
                //Error.Visible = true;

                LogError objEr = new LogError();
                objEr.HandleException(ex);
            }
        }
    }
}