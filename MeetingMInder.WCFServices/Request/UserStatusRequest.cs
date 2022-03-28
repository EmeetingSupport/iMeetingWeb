using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization; 

namespace MeetingMInder.WCFServices.Request
{
    [DataContract]
    public class UserStatusRequest
    {
       [DataMember]
        public Guid UserId { get; set; }
    }
}