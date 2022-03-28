using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class SerialNumberDomain
    {
        public int SerialNumberId { get; set; }
        public string SerialNumber { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }

    }
}
