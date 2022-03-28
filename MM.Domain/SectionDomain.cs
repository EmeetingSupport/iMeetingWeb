using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class SectionDomain
    {
        public Guid SectionId { get; set; }
        public string Title { get; set; }
        public string SerialNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public Guid ForumId { get; set; }

        public Guid MeetingId { get; set; }
        public string Meeting { get; set; }
    }
}
