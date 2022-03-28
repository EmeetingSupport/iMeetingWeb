using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class EmailResponse
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string Error { get; set; }
    }
}
