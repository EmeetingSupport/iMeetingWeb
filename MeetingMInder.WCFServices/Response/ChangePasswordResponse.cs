using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ChangePasswordResponse
    {
       [DataMember]
        public string Status { get; set; }

       [DataMember(EmitDefaultValue = false, IsRequired = false)]
       public bool LostDevice { get; set; }

       [DataMember(EmitDefaultValue = false, IsRequired = false)]
       public string Message { get; set; }
    }
}
