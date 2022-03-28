using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
     [DataContract(Namespace = "")]
    public class ForgotPasswordRequest
    {
       [DataMember]
        public string UserName {get; set;}

        [DataMember]
        public string MobileNo { get; set; }
    }
}