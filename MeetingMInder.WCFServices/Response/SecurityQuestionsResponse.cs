using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
     [DataContract(Namespace = "")]
    public class SecurityQuestionsResponse
    {
         [DataMember]
        public Guid SecurityQuestionId
        {
            get;
            set;
        }

         [DataMember]
        public string SecurityQuestion
        {
            get;
            set;
        }
    }
}