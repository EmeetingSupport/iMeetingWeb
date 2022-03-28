using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ServerResponse
    {
        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}