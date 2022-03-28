using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class ForumRequest
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public Guid EntityId { get; set; }
    }
}