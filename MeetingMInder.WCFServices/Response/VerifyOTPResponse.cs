using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class VerifyOTPResponse
    {
        [DataMember]
        public string IsActive { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OTPError { get; set; }

        [DataMember]
        public string RequestId { get; set; }

    }
}