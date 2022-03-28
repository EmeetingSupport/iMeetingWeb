﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace="")]
    public class ForgotPasswordResponse_old
    {
       [DataMember]
        public Guid SecurityQuestionId { get; set; }

       [DataMember]
        public string SecurityQuestion { get;  set; }

       [DataMember]
        public string Answer { get;  set; }

       [DataMember]
        public string Password { get;  set; }

       [DataMember(EmitDefaultValue = false, IsRequired = false)]
       public bool LostDevice { get; set; }
    }
}