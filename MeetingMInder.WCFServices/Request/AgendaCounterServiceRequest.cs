using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Request
{
    public class AgendaCounterServiceRequest
    {

        [DataMember]
        public string AgendaId { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public bool IsRead { get; set; }

    }
}