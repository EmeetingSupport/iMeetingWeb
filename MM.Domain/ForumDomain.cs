﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Forum Property"
    /// <summary>
    /// Property 
    /// </summary>
    public class ForumDomain
    {
        public Guid ForumId { get; set; }
        public string ForumName { get; set; }
        public Guid EntityId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public string EntityName { get; set; }

        public Guid ForumChecker { get; set; }
        public string ForumShortName { get; set; }
        
        public bool IsEnable {  get; set;  }

        public string MembersInfo { get; set; }

        public Guid UserId { get; set; }


        //public string GeneralManager { get; set; }
        //public Guid GMUpdatedBy { get; set; }
        //public DateTime GMUpdatedOn { get; set; }

    }

    #endregion
}
