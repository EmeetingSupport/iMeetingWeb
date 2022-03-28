using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    /// <summary>
    /// Property Of Minutes Table
    /// </summary>
   public class MinutesDomain 
    {
        public Guid MinuteId { get; set; }
        public Guid AgendaId { get; set; }
        public string Minutes { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public Guid MeetingId { get; set; } 
    }
}
