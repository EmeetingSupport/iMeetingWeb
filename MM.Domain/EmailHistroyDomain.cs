using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM.Domain
{
    public class EmailHistroyDomain
    {
        public Int32 EmailHistoryId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CC { get; set; }
        public Guid MeetingId { get; set; }

    }
}
