using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Public property of key informatin domain"
    public  class KeyInformationDomain
    {
        public Guid KeyInformationId { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Photograph { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive  { get; set; }
        public string Suffix { get; set; }
        public Guid EntityId { get; set; }
        public int DirectorOrder  { get; set; }
    }
    #endregion
}
