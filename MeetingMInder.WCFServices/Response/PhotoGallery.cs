using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class PhotoGallery
    {
       [DataMember]
         public Guid PhotoGalleryId { get; set; }
         [DataMember]
        public string Description { get; set; }
         [DataMember]
        public string Photograph { get; set; }
         [DataMember]
        public DateTime CreatedOn { get; set; }
         [DataMember]
        public Guid CreatedBy { get; set; }
         [DataMember]
        public DateTime UpdatedOn { get; set; }
         [DataMember]
        public Guid UpdatedBy { get; set; }
         [DataMember]
        public bool IsActive  { get; set; }
         [DataMember]
         public Guid EntityId { get; set; }   
    }
}