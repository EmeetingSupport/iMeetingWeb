using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Public properties of AdDomain"
    public class AdDomain
    {
        public int AdMasterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public Guid UpdatedBy { get; set; }
    }
    #endregion
}
