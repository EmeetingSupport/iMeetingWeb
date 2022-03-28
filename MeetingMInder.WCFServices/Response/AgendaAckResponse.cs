using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class AgendaAckResponse : MeetingMInder.WCFServices.Request.Ack
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string Error { get; set; }
    }

    [DataContract(Namespace = "")]
    public class AgendaAckResponse_New
    {
        [DataMember]
        public List<AgendaAckResponse> Response { get; set; }
    }

}
