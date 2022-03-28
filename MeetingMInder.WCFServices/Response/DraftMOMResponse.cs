using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract]
    public   class DraftMOMResponse
    {
       [DataMember]
        public IList<DraftMOMDetails> DraftMOM { get; set; }
       
       [DataMember]
        public string ResponseTime { get; set; }
    }
}
