using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ForumResponse
    {
        [DataMember]
        public Guid ForumId { get; set; }
        
        [DataMember]
        public string ForumName { get; set; }

        [DataMember]
        public string ForumShortName { get; set; }

        [DataMember]
        public Guid EntityId { get; set; }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime UpdatedOn { get; set; }
    }
}