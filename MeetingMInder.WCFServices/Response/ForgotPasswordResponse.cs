using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ForgotPasswordResponse
    {
        [DataMember]
        public string Status { get; set; }
    }
}