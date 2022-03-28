using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class GetUserEventsRequest
    {
       [DataMember]
        public Guid UserId { get; set; }

       [DataMember]
        public string StartTime { get; set; }
    }
}