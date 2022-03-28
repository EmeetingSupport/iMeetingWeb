using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Property of AboutUS"
    public class ABoutUSDomain
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

        public string Text
        {
            get;
            set;
        }
        public string Image
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
        public string EncryptionKey
        {
            get;
            set;
        }
        
    }
    #endregion
}
