using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class GetUserEventsResponse
    {
       [DataMember]
        public IList<EventsDetails> Events { get; set; }

       [DataMember]
        public IList<AttendeesDetails> Attendees { get; set; }

       [DataMember]
        public string ResponseTime { get; set; }
    }

    [DataContract(Namespace = "")]
    public class EventsDetails
    {
       [DataMember]
        public Int32 EventId { get; set; }
       [DataMember]
        public string EventTitle { get; set; }
       [DataMember]
        //public string Upload_Photo { get; set; }
        //[DataMember]
        public string Attachment { get; set; }
       [DataMember]
        public string EventDescription { get; set; }
       [DataMember]
        public string  StartTime { get; set; }
       [DataMember]
        public string  EndTime { get; set; }
       [DataMember]
        public string  EventDay { get; set; }
       [DataMember]
        public bool IsActive { get; set; }
       [DataMember]
        public string MOM { get; set; }
    }

     [DataContract(Namespace = "")]
    public class AttendeesDetails
    {
        [DataMember]
         public Int64 EventId { get; set; }
       [DataMember] 
        public Int64 Attendees_Id { get; set; }
       [DataMember]
        public string Name_Of_Visitor { get; set; }
       [DataMember]
        public string Designation_Of_Visitor { get; set; }
       [DataMember]
        public string Name_Of_Visitor_Organisation { get; set; }
        //[DataMember]
        //public string Name_Designation { get; set; }
       [DataMember]
        public string PhoneNo { get; set; }

       [DataMember]
        public string EmailId { get; set; }
       [DataMember]
        public string Url { get; set; }
       [DataMember]
        public string Photo { get; set; }

       [DataMember]
        public bool IsActive { get; set; }
    }

}