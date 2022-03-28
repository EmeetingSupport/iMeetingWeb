using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class UserLockResponse
    {
        [DataMember]
        public string Status { get; set; }
    }
}