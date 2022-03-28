using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MeetingMinder.Web.EventCal
{
    public partial class SchedulerTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                ;
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
                end = DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)

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


        private static bool CheckAlphaNumeric(string str)
        {

            return Regex.IsMatch(str, @"^[a-zA-Z0-9 ]*$");


        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            
        }
    }
}