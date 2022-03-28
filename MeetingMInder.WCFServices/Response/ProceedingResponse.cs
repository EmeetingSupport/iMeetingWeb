using MeetingMInder.WCFServices.Request;
using MM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class ProceedingResponse
    {
        [DataMember]
        public List<Proceeding> ProceedingList { get; set; }

        [DataMember]
        public string ResponseTime { get; set; }
    }
}