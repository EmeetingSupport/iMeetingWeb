using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
  public class UserAgendaDomain
  {
      #region "Public properties of UserAgendaDomain"
   
          public Guid UserAgendaId { get; set; }
          public Guid UserId { get; set; }
          public Guid MeetingId { get; set; }
          public Guid AgendaId { get; set; }

          public bool IsShow { get; set; }
          public Guid CreatedBy { get; set; }
          public DateTime CreatedOn { get; set; }
          public Guid UpdateBy { get; set; }
          public DateTime UpdateOn { get; set; }

          public bool IsActive { get; set; }
          //public string ForumName { get; set; }
          //public string UserName { get; set; }
          //public string EntityName { get; set; }

      
      #endregion
    }
}
