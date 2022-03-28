using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Checker Domain"
    /// <summary>
    /// Property For Checker 
    /// </summary>
   public class CheckerDomain
    {
        public Guid MeetingId
        {
            get;
            set;
        }
        public string MeetingDate
        {
            get;
            set;
        }
        public string MeetingVenue
        {
            get;
            set;
        }
        public string MeetingTime
        {
            get;
            set;
        }

        public string ForumName
        {
            get;
            set;
        }

        public string EntityName
        {
            get;
            set;
        }

    }
    #endregion
}
