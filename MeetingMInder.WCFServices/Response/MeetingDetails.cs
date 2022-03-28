using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace="")]
    public class MeetingDetails
    {
       [DataMember]
        public Guid MeetingId { get; set; }
       [DataMember]
        public string MeetingDate { get; set; }
       [DataMember]
        public string MeetingVenue { get; set; }
       [DataMember]
        public Guid ForumId { get; set; }
       [DataMember]
        public DateTime CreatedOn { get; set; }
       [DataMember]
        public Guid CreatedBy { get; set; }
       [DataMember]
        public DateTime UpdatedOn { get; set; }
       [DataMember]
        public Guid UpdatedBy { get; set; }
       [DataMember]
        public string MeetingTime { get; set; }
       //[DataMember]
       // public string Status  { get; set; }
      [DataMember]
        public string EntityName {get; set;}
       [DataMember]
        public string ForumName {get; set;}
       [DataMember]
        public Guid EntityId{ get; set;}
       [DataMember]
        public Guid MeetingChecker { get; set; }
       [DataMember]
        public bool IsActive {get; set;}

         [DataMember]
        public string MeetingTimeStamp { get; set; }

         [DataMember]
          public string MeetingNumber { get; set; }

         [DataMember]
         public string MeetingHeader { get; set; }

         [DataMember]
         public string MeetingType { get; set; }

        [DataMember]
        public string Convenor { get; set; }

        [DataMember]
        public string PublishVersion { get; set; }

        [DataMember]
        public string PastMeeting { get; set; }
    }
}