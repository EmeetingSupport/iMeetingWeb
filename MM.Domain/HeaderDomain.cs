using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
   public class HeaderDomain
    {
       public Guid HeaderId { get; set; }
       public string Header { get; set;}
       public DateTime CreatedOn { get; set; }
       public Guid CreatedBy { get; set; }
       public DateTime UpdatedOn { get; set; }
       public Guid UpdatedBy { get; set; }
       public bool IsActive { get; set; }
       public Guid ForumId { get; set; }
       public string ForumName { get; set; }
    }
}
