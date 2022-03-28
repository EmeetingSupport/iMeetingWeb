using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Response
{
    public class Tabs
    {
        [DataMember]
        public int GlobalTabId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string UploadedPDF { get; set; }
        [DataMember]

        public bool IsActive { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Guid CreatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedOn { get; set; }
        [DataMember]
        public Guid UpdatedBy { get; set; }
 
 
      
    }
}