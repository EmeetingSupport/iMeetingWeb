using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ResetPasswordResponse
    {
       [DataMember]
        public bool IsPasswordChanged { get; set; }

       [DataMember(EmitDefaultValue = false, IsRequired = false)]
       public bool LostDevice { get; set; }
    }
}