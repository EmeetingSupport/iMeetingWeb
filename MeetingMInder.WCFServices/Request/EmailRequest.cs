using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class EmailRequest
    {
        [DataMember]
        public string To { get; set; }

        [DataMember]
        public string CC { get; set; }

        [DataMember]
        public string BCC { get; set; }

        [DataMember]
        public string ReplyTo { get; set; }

        [DataMember]
        public string Body { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public Guid UserId { get; set; }
    }
}