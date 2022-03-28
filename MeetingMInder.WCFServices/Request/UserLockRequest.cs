using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract]
    public class UserLockRequest
    {
        [DataMember]
        public string UserName { get; set; }
    }
}