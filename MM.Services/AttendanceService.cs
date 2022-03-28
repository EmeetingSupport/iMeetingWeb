using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
  public class AttendanceService
    {
      /// <summary>
      /// Insert Attendace
      /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="Reason">string specifying Reason</param>
        /// <param name="Attendance">string specifying Attendance</param>
      /// <returns></returns>
      public AttendanceDomain InsertOrUpdateAttendance(Guid UserId, Guid MeetingId, string Reason, string Attendance)
      {
          AttendanceDomain objAttendace = new AttendanceDomain();
          objAttendace.Attending = Attendance;
          objAttendace.Reason = Reason;
          objAttendace.UserId = UserId;
          objAttendace.MeetingId = MeetingId;

          objAttendace = AttendanceDataprovider.Instance.InsertAttendance(objAttendace);
          return objAttendace;
      }

      /// <summary>
      /// Attendance by meeting id
      /// </summary>
      /// <param name="MeetingId">Guid specifying MeetingId</param>
      /// <returns></returns>
      public IList<AttendanceDomain> GetAttendaceByMeetingID(Guid MeetingId)
      {
          return AttendanceDataprovider.Instance.GetAttendanceByMeetingId(MeetingId);
      }
    }
}
