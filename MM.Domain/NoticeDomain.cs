﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
#region "Public properties of Notice"
  public  class NoticeDomain
    {
      public Guid NoticeId { get; set; }

      public Guid MeetingId { get; set; }
      
      public string NoticeMessage {get; set; }
      
      public Guid NoticeChecker { get; set;}
      
      public string IsApproved {get; set;}
      
      public Guid ApprovedBy{get; set; }
      
      public DateTime CreatedOn { get; set; }
      
      public Guid CreatedBy { get; set; }
      
      public DateTime UpdatedOn { get; set; }
      
      public Guid UpdatedBy { get; set; }

      public string MeetingDate { get; set; }
      
      public string MeetingVenue { get; set; }
      
      public string MeetingTime { get; set; }
      
      public string FirstName { get; set; }
      
      public string LastName { get; set; }    
      
      public string UserName { get; set; }

      public Guid EntityId { get; set; }
      
      public Guid ForumId { get; set; }


      public string MeetingNumber { get; set; }

      public string ForumName { get; set; }
    }
#endregion
}
