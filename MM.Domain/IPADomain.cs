using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Property of PDFTemplateDomain"

   public class IPADomain
    {
        public int Id
        {
            get;
            set;
        }

        public Guid EntityId
        {
            get;
            set;
        }

        public string IPAName
        {
            get;
            set;
        }
        public string IPA
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }

    }
    #endregion
}
