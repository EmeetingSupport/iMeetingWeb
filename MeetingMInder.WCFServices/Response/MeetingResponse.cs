using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class MeetingResponse
    {
        [DataMember]
        public Guid MeetingId { get; set; }

        [DataMember]
        public string MeetingDate { get; set; }

        [DataMember]
        public string MeetingVenue { get; set; }

        [DataMember]
        public string MeetingTime { get; set; }

        [DataMember]
        public string MeetingNumber { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime UpdatedOn { get; set; }

        [DataMember]
        public Guid ForumId { get; set; }

        [DataMember]
        public Guid EntityId { get; set; }
    }
}