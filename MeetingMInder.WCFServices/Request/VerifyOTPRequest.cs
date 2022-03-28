using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class VerifyOTPRequest
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public Guid OTPKey { get; set; }

        [DataMember]
        public string OTPValue { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string RequestId { get; set; }
    }
}
