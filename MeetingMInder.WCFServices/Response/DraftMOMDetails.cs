using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class DraftMOMDetails
    {
       [DataMember]
        public Guid DraftMinutesId { get; set; }

       [DataMember]
        public string DraftMOMName { get; set; }

       [DataMember]
        public string DraftMinute { get; set; }

       [DataMember]
        public DateTime CreatedOn { get; set; }

       [DataMember]
        public Guid CreatedBy { get; set; }

       [DataMember]
        public DateTime UpdateOn { get; set; }

       [DataMember]
        public Guid UpdatedBy { get; set; }

       [DataMember]
        public string CreatedByName { get; set; }

       [DataMember]
        public string UpdatedByName { get; set; }

       [DataMember]
        public string CreatedOnString { get; set; }

       [DataMember]
        public string UpdateOnOnString { get; set; }

       [DataMember]
        public bool IsActive { get; set; }
    }
}