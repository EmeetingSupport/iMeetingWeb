using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MeetingMInder.WCFServices.Response;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class EventAddRequest
    {
        //[DataMember]
        //public List<EventsDetails> Events { get; set; }

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
        public string StartTime { get; set; }
       [DataMember]
        public string EndTime { get; set; }
       [DataMember]
        public string EventDay { get; set; }
       [DataMember]
        public bool IsActive { get; set; }

       [DataMember]
        public string UpdateStartTime { get; set; }

       [DataMember]
        public Guid UserId { get; set; }
       
    }
}