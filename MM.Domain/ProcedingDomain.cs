using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class ProcedingDomain
    {
        public Guid ProcedingId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid ForumId { get; set; }
        public Guid AgendaId { get; set; }
        public Guid SubAgendaId { get; set; }
        public string ProcedingName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }
       public string EncryptionKey { get; set; }
    }
}
