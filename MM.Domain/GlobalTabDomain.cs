using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class GlobalTabDomain
    {
        public int GlobalTabId { get; set; }
        public string Title { get; set; }
        public string UploadedPDF { get; set; }
        public string DeletedPDF { get; set;}
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }

        public string GlobalTabNote { get; set; }

        public Guid EntityId { get; set; }
    }
}
