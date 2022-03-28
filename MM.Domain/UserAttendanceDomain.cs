using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    #region "Public properties of attendace"
    public class UserAttendanceDomain
    {
        public Guid  Id {get; set;}
        public Guid  MeetingId{get; set;}
        public Guid UserId {get; set;}
        // public string Attending {get; set;}
        // public string Reason {get; set;}

        // public string FirstName {get; set;}

        // public string LastName {get; set;}

        //public string MeetingDate { get; set; }
        // public string MeetingVenue { get; set; }

        //   public string MeetingTime { get; set; }

        public bool Attendance { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateOn { get; set; }
        public Guid UpdatedBy { get; set; }

        public bool IsActive { get; set; }


    }
    #endregion
}
