using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class AgendaMarkerResponse
    {
        [DataMember]
        public Guid MeetingId { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string Read { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public List<string> Agenda { get; set; }
    }

    //[DataContract(Namespace = "")]
    //public class AgendRead
    //{
    //    A
    //}
}
