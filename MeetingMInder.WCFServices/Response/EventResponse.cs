using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    public class EventResponse
    {
        public IList<Event> Events { get; set; }
    }

    public class Event
    {
        public Int64 Event_Id { get; set; }
        public string Name_Of_Visitor { get; set; }
        public string Designation_Of_Visitor { get; set; }
        public string Name_Of_Visitor_Organisation { get; set; }
        public string Appointment { get; set; }
        public string Name_Designation { get; set; }
        public string Upload_Photo { get; set; }
        public string Attachment { get; set; }
        public string Information { get; set; }
        public string Event_Start { get; set; }
        public string Event_End { get; set; }
        public string CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}