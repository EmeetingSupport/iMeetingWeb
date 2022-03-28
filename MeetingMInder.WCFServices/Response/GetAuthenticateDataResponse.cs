using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MeetingMInder.WCFServices.Request;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract(Namespace = "")]
    public class GetAuthenticateDataResponse
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<EntityDetails> Entity { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<ForumDetails> Forum { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<MeetingDetails> Meeting { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<AgendaDetails> Agenda { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<NoticeDetails> Notice { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public List<MinutesDetails> UploadedMinutes { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string ResponseTime { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string MeetingNumber { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string UploadMinuteNumber { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string PastMeeting { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string SessionTimeOut { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string OfflineDays { get; set; }

        //[DataMember]
        //public string AboutUsImage { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string AboutUs { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public bool LostDevice { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string RequestId { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string LocalNotificationDays { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false)]
        public string ProceedingsCount { get; set; }

        [DataMember]
        public List<Proceeding> ProceedingList { get; set; }

        [DataMember]
        public bool IsUpdatedTimeStamp { get; set; }
    }
}