using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MeetingMInder.WCFServices.Request
{
    public class DataCleanRequest
    {
        [DataMember]
        public string DeviceToken { get; set; }
    }
}