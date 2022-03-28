using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MeetingMInder.WCFServices.Response;

namespace MeetingMInder.WCFServices.Request
{
     [DataContract(Namespace = "")]
    public class MOMUpdateRequest
    {
       [DataMember]
        public Int32 EventId { get; set; }

       [DataMember]
        public string MOM { get; set; }

       [DataMember]
        public string StartTime { get; set; }

       [DataMember]
        public Guid UserId { get; set; }
    }
}