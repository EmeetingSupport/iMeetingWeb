﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Approval Property"
    /// <summary>
    /// Property 
    /// </summary>
   public class ApprovalDomain
    {
      public Guid EntityId { get; set; }
      public string EntityName { get; set; }

      public Guid ForumId { get; set; }
      public string ForumName { get; set; }

      public Guid MeetingId { get; set; }
      public string MeetingDate { get; set; }
      public string MeetingVenue { get; set; }
      public string MeetingTime { get; set; }

      public string FirstName { get; set; }
      public string LastName { get; set; }    
      public string UserName { get; set; }

      public string Designation { get; set; }
      public string Photograph { get; set; }
      public Guid UserId { get; set; }
      public string CreaterFirstName { get; set; }
      public string CreaterLastName { get; set; }
      public string IsApproved { get; set; }

      public Guid UpdatedBy { get; set; }
      public string MeetingNumber { get; set; }
    }
    #endregion
}
