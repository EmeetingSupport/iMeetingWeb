using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{

    #region "DraftMOM"

    /// <summary>
    /// Property Of DraftMOM 
    /// </summary>
    public class DraftMOMDomain
    {
        public Guid DraftMinutesId { get; set; }
        public string DraftMinute { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }

        public Guid MeetingId { get; set; }
        public Guid EntityId { get; set; }
        public string DraftMinuteName { get; set; }
    }
    #endregion
}
