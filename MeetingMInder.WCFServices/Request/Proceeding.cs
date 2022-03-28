using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    public class Proceeding
    {
        [DataMember]
        public Guid ProcedingId { get; set; }
        [DataMember]
        public Guid MeetingId { get; set; }
        [DataMember]
        public Guid ForumId { get; set; }
        [DataMember]
        public Guid AgendaId { get; set; }
        [DataMember]
        public Guid SubAgendaId { get; set; }
        [DataMember]
        public string ProcedingName { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Guid CreatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedOn { get; set; }
        [DataMember]
        public Guid UpdatedBy { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public string ForumName { get; set; }

        [DataMember]
        public string MeetingDate { get; set; }
        [DataMember]
        public string MeetingVenue { get; set; }
        [DataMember]
        public string MeetingTime { get; set; }
 
    }
}