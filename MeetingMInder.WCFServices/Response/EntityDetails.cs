﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace="")]
    public class EntityDetails
    {
     [DataMember]
      public Guid EntityId { get; set; }

     [DataMember]
      public string EntityName { get; set; }

     [DataMember]
      public DateTime CreatedOn { get; set; }

     [DataMember]
      public Guid CreatedBy { get; set; }

     [DataMember]
      public DateTime UpdatedOn { get; set; }

     [DataMember]
      public Guid UpdatedBy { get; set; }

     [DataMember]
      public string EntityShortName {  get; set;  }

     [DataMember]
      public string EntityLogo {  get; set;  }

        [DataMember]
      public string EntityMeeting {  get; set;  }

     [DataMember]
      public Guid EntityChecker {  get; set;  }

     [DataMember]
      public bool IsEnable {  get; set;  }

       [DataMember]
        public bool IsActive {get; set;}
    }
}