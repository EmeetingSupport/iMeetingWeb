using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class AgendaMarkerRequest
    {
        [DataMember]
        public Guid MeetingId { get; set; }

        [DataMember]
        public Guid UserId { get; set; }
    }
}