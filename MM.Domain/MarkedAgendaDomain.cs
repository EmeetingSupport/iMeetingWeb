using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
 public  class MarkedAgendaDomain
 {
     public Guid AgendaMarkerId { get; set; }
      public Guid MeetingId { get; set; }
     public Guid AgendaId { get; set; }
     public bool IsRead { get; set; }
      public Guid CreatedBy { get; set; }
     public DateTime CreatedOn { get; set; }
     public Guid UpdatedBy { get; set; }
     public DateTime UpdatedOn { get; set; }

     public bool IsActive { get; set; }
    }
}
