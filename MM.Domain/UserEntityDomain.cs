﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Property Of Entity"
    /// <summary>
    /// Property Of User Entity Table
    /// </summary>
   public class UserEntityDomain
    {
        public Guid UserEntityId
        {
            get;
            set;
        }
        public Guid EntityId
        {
            get;
            set;
        }

        public Guid UserId
        {
            get;
            set;
        }
        public DateTime CreatedOn
        {
            get;
            set;
        }
        public Guid CreatedBy
        {
            get;
            set;
        }
        public DateTime UpdatedOn
        {
            get;
            set;
        }
        public Guid UpdatedBy
        {
            get;
            set;
        }

        public string EntityName
        {
            get;
            set;
        }

        public string EntityLogo
        {
            get;
            set;
        }

        public string EncryptionKey
        {
            get;
            set;
        }
    }
    #endregion
}
