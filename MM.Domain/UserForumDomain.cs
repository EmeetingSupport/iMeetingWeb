using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Public properties of UserForumDoamin"
    public  class UserForumDomain
    {
        public Guid  UserForumId { get; set; }
        public Guid UserId { get; set; }
        public Guid ForumId { get; set; }
        public bool IsAdd { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdateOn { get; set; }

        public string ForumName { get; set; }
        public string UserName  { get; set; }
        public string EntityName  { get; set; }

    }
    #endregion
}
