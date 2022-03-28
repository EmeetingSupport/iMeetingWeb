﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class GetAuthenticateDataRequest
    {
       [DataMember]
        public Guid UserId { get; set; }

       [DataMember]
        public string StartTime  { get; set; }

        //[DataMember]
        //public string Password  { get; set; }

       [DataMember]
        public Guid EntityId {get; set; }

       [DataMember]
        public string UdId {get; set; }

       [DataMember]
       public Guid RequestId { get; set; }

        [DataMember]
        public string UpdatedTimeStamp { get; set; }
        //[DataMember]
        //public string Token { get; set; }
    }
}