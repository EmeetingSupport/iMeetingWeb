using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class CheckUpdateResponse
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Link { get; set; }
    }
}
