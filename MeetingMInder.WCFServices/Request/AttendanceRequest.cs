﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
       [DataContract(Namespace = "")]
    public class AttendanceRequest
    {
          [DataMember]
           public Guid  MeetingId{get; set;}
       [DataMember]
           public Guid UserId {get; set;}
       [DataMember]
           public string Attending {get; set;}
       [DataMember]
           public string Reason {get; set;}
                      
         //[DataMember]
         //public string Token { get; set; }

    }
}