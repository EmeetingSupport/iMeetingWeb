using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class KeyInformationRequest
    {
       [DataMember]
        public string StartTime { get; set; }

       [DataMember]
        public Guid EntityId { get; set; }

       [DataMember]
       public Guid UserId { get; set; }
    }
}