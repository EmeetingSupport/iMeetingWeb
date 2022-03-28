using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MM.Domain;

namespace MeetingMInder.WCFServices.Response
{
     [DataContract (Namespace="")]
    public class KeyInformationResponse
    {
        [DataMember]
         public List<BoardOfDirectors> BoardOfDirector{get; set; }

        [DataMember]
         public List<PhotoGallery> PhotoGallery{get; set; }

         [DataMember]
         public string ResponseTime { get; set; }

         [DataMember(EmitDefaultValue = false, IsRequired = false)]
         public bool LostDevice { get; set; }

        [DataMember]
        public List<Tabs> GlobalTabList { get; set; }
    }
}