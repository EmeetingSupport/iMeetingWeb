using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class LoginResponse
    {
        [DataMember]
        public bool Status { get; set; }


        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public Guid UserId { get; set; }


        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<EntityDetails> Entity { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public bool IsAuthorized { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public Guid SecurityQuestionId { get; set; }

        //[DataMember(EmitDefaultValue = false, IsRequired = false)]
        //public string Answer { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string SecurityQuestion { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public bool LostDevice { get; set; }

        [DataMember]
        public string OTPSend { get; set; }


        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OTPVersion { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OTPKey { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OTPError { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string RequestId { get; set; }

        //[DataMember]
        //public string Token { get; set; }

        //[DataMember(EmitDefaultValue = false, IsRequired = false)]
        //public string EncryptionKey { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        //[DataMember(EmitDefaultValue = false, IsRequired = false)]
        [DataMember]
        public bool UserExists { get; set; }
    }
}