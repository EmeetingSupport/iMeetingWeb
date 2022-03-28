using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract]
    public class UserStatusResponse
    {
        //public string Name { get; set; }
       [DataMember]
        public bool IsActive { get; set; }

       [DataMember(EmitDefaultValue = false, IsRequired = false)]
       public bool LostDevice { get; set; }
        //[DataMember]
        //public bool IsEnableOnIpad { get; set; }
    }
}