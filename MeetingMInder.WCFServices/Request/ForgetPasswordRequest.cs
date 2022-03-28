﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class ForgetPasswordRequest
    {
       [DataMember]
        public string UserName {get; set;}

       [DataMember] 
        public Guid SecurityQuestionId { get; set; }

       [DataMember]
        public string Answer { get; set; }

        
         //[DataMember]
         //public string Token { get; set; }
        [DataMember]
       public Guid UserId { get; set; }
    }
}