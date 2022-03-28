using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class AgendaAckRequest
    {
        [DataMember]
        public List<Ack> AgendaAck { get; set; }
    }

    [DataContract(Namespace = "")]
    public class Ack
    {
        [DataMember]
        public Guid MeetingId { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string AgendaReadOn { get; set; }

        [DataMember]
        public string NoticeReadOn { get; set; }

        [DataMember]
        public bool IsAgendaRead { get; set; }

        [DataMember]
        public bool IsNoticeRead { get; set; }

        [DataMember]
        public Guid PublishVersion { get; set; }
    }

}