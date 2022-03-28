﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace="")]
    public class ForumDetails
    {
       [DataMember]
        public Guid ForumId { get; set; }
       [DataMember]
        public string ForumName { get; set; }
       [DataMember]
        public Guid EntityId { get; set; }
       [DataMember]
        public DateTime CreatedOn { get; set; }
       [DataMember]
        public Guid CreatedBy { get; set; }
       [DataMember]
        public DateTime UpdatedOn { get; set; }
       [DataMember]
        public Guid UpdatedBy { get; set; }
       [DataMember]
        public string EntityName { get; set; }
       [DataMember]
        public Guid ForumChecker { get; set; }
       [DataMember]
        public string ForumShortName { get; set; }
       [DataMember]
        public bool IsEnable {  get; set;  }
       [DataMember]
        public string MembersInfo { get; set; }
       [DataMember]
        public int ForumOrder { get; set; }
       [DataMember]
        public bool IsActive {get; set;}
    }
}