using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    [Serializable]
    #region "Meeting Property"
    /// <summary>
/// Property Of Meeting Table
/// </summary>
    public class MeetingDomain
    {
        public Guid MeetingId { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingVenue { get; set; }
        public Guid ForumId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public string MeetingTime { get; set; }
        public string Status  { get; set; }
        public string MeetingType { get; set; }
       

        public string EntityName
        {
            get;
            set;
        }

        public string ForumName
        {
            get;
            set;
        }

        public Guid EntityId
        {
            get;
            set;
        }

       public Guid MeetingChecker { get; set; }

       public string MeetingNumber { get; set; }
       public string Header { get; set; }

       public string Convenor { get; set; }

       public string ConvenorName { get; set; }

       public string EditOrCancelReason { get; set; }

       public string Action { get; set; }

       public string PastMeeting { get; set; }
       public int PastMeetingExpiry { get; set; }
    }
    #endregion
}
