using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace="")]
    public class NoticeDetails
    {
       [DataMember]
      public Guid NoticeId { get; set; }
     [DataMember]
      public Guid MeetingId { get; set; }
     [DataMember]
      public string NoticeMessage {get; set; }
     [DataMember]
      public Guid NoticeChecker { get; set;}
     [DataMember]
      public string IsApproved {get; set;}
     //[DataMember]
     // public Guid ApprovedBy{get; set; }
     [DataMember]
      public string CreatedOn { get; set; }
     [DataMember]
      public Guid CreatedBy { get; set; }
     [DataMember]
      public string UpdatedOn { get; set; }
     [DataMember]
      public Guid UpdatedBy { get; set; }
       [DataMember]
      public string MeetingDate { get; set; }
     [DataMember]
      public string MeetingVenue { get; set; }
     [DataMember]
      public string MeetingTime { get; set; }
      //[DataMember]
      //public string FirstName { get; set; }
      //[DataMember]
      //public string LastName { get; set; }
      //[DataMember]
      //public string UserName { get; set; }
       [DataMember]
      public Guid EntityId { get; set; }
    [DataMember]
      public Guid ForumId { get; set; }

       [DataMember]
        public bool IsActive {get; set;}

       [DataMember]
        public Boolean IsSigned {get; set;}

       [DataMember]
      public string DisplayName { get; set; }
    }
}