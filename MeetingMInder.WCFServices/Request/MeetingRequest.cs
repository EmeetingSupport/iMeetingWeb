using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class MeetingRequest
    {
        [DataMember]
        public Guid ForumId { get; set; }
    }
}