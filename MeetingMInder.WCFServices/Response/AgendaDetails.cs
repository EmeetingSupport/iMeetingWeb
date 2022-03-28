using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MeetingMInder.WCFServices.Response
{
    [DataContract (Namespace= "")]
    public class AgendaDetails
    {
       [DataMember]
      public Guid AgendaId { get; set; }
     [DataMember]
      public string AgendaName { get; set; }
     [DataMember]
      public string AgendaNote { get; set; }
     [DataMember]
      public Guid ParentAgendaId { get; set; }
    
     [DataMember]
      public bool IsPublished { get; set; }
      //[DataMember]
      //public Guid PublishedBy { get; set; }
     [DataMember]
      public DateTime CreatedOn { get; set; }
     [DataMember]
      public Guid CreatedBy { get; set; }
     [DataMember]
      public DateTime UpdateOn { get; set; }
     [DataMember]
      public Guid UpdatedBy { get; set; }
     [DataMember]
      public Int64 AgendaOrder {  get; set;  }
     [DataMember]
      public Guid MeetingId {get; set; }
       [DataMember]
      public string EntityName { get; set;}
       [DataMember]
      public string MeetingVenue {get; set;}
       [DataMember]
      public string ForumName { get; set;}
       [DataMember]
      public Guid EntityId { get; set; }
       [DataMember]
      public Guid AgendaChecker { get; set; }
      [DataMember]
      public Guid ForumId { get; set; }
       [DataMember]
        public bool IsActive {get; set;}

       [DataMember]
        public string UploadedAgendaNote { get; set; }

       [DataMember]
        public string Classification { get; set; }



      


       [DataMember]
       public string FileUploadOn { get; set; }

       [DataMember]
       public string SerialNumber { get; set; }

       [DataMember]
       public string Presenter { get; set; }

       [DataMember]
       public string SerialKey { get; set; }

       [DataMember]
       public string SerialKeyType { get; set; }

        [DataMember]
        public string AgendaSerialNo { get; set; }
        //[DataMember]
        //public string EncryptionKey { get; set; }

        //[DataMember]
        //public List<Pdfs> UploadedAgendaNote { get; set; } //UploadedFiles
    }
}