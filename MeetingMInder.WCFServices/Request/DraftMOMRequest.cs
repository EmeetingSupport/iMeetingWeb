using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class DraftMOMRequest
    {
       [DataMember]
        public Guid UserId { get; set; }

       [DataMember]
        public string StartTime { get; set; }
    }
}
