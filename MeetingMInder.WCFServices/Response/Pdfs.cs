using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class Pdfs
    {
       [DataMember]
        public string FileUrl { get; set; }

       [DataMember]
        public string PdfName { get; set; }

       [DataMember]
        public bool IsActive { get; set; }
    }
}