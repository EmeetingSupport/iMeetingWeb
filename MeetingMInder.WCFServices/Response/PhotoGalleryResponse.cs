using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class PhotoGalleryResponse
    {
       [DataMember]
       public List<PhotoGallery> PhotoGallery{get; set; }

       [DataMember]
      public  string ResponseTime { get; set ;}
    }
}