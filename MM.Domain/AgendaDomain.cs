﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Agenda Property"
    /// <summary>
    /// Property OF Agenda Table
    /// </summary>
  public  class AgendaDomain
    {

      public Guid AgendaId { get; set; }
      public string AgendaName { get; set; }
      public string AgendaNote { get; set; }
      public Guid ParentAgendaId { get; set; }
      public string UploadedAgendaNote { get; set; }
      public bool IsPublished { get; set; }
      public Guid PublishedBy { get; set; }
      public DateTime CreatedOn { get; set; }
      public Guid CreatedBy { get; set; }
      public DateTime UpdateOn { get; set; }
      public Guid UpdatedBy { get; set; }
      public int AgendaOrder {  get; set;  }

      public string MeetingDate { get; set; }
      public string MeetingTime { get; set; }

     
      public Guid MeetingId
      {
          get;
          set;
      }

      public string EntityName
      {
          get;
          set;
      }

      public string MeetingVenue
      {
          get;
          set;
      }

      public string ForumName
      {
          get;
          set;
      }

      public Guid EntityId
      {
          get;
          set;
      }

      public Guid AgendaChecker { get; set; }
      public Guid ForumId { get; set; }

      public string Minutes { get; set; }

      public string DeletedAgenda { get; set; }

      public string Classification { get; set; }

      public string CheckerName { get; set; }

      public string IsApproved { get; set; }

      public string Presenter { get; set; }

      public string SerialNumber { get; set; }

      public string SerialNumberType { get; set; }
        public string MeetingNumber { get; set; }

        public string SerialTitle { get; set; }
        public string SerialText { get; set; }

       
    }
    #endregion
}
