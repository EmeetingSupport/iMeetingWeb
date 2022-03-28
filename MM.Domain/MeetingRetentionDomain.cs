using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM.Domain
{
    public class MeetingRetentionDomain
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public Guid UpdatedBy { get; set; }

        public string MeetingDate { get; set; }

        public string MeetingTime { get; set; }
    }
}
