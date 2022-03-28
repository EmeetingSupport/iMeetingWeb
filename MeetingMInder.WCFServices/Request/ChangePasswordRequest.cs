using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Request
{
    [DataContract(Namespace = "")]
    public class ChangePasswordRequest
    {
       [DataMember]
        public Guid UserId { get; set; }

       //[DataMember]
       // public string OldPassword { get; set; }

       //[DataMember]
       // public string NewPassword { get; set; }    

      [DataMember]      
      public string Password { get; set; }     
        
      [DataMember]
      public string RequestId { get; set; }            
        
    }   
}