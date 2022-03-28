﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Property Of Entity"
    /// <summary>
    /// Property Of Entity Table
    /// </summary>
    public class EntityDomain
    {
        public Guid EntityId { get; set; }
        public string EntityName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }

        public string EntityShortName { get; set; }
        public string EntityLogo { get; set; }
        public Guid EntityChecker { get; set; }
        public bool IsEnable { get; set; }

        public string EntityMeeting { get; set; }
        public string EncryptionKey { get; set; }
    }
    #endregion
}
