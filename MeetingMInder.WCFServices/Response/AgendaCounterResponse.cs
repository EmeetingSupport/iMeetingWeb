using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    public class AgendaCounterResponse 
    {
        [DataMember]
        public string Read  { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<string> Agenda { get; set; }
    }
}