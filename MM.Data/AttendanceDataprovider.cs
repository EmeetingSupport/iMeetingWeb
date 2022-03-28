using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class AttendanceDataprovider : DataProvider
    {
        #region "Singleton"
        private static volatile AttendanceDataprovider _instance;
        private static object _synRoot = new Object();

        private AttendanceDataprovider()
        {
        }

        public static AttendanceDataprovider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AttendanceDataprovider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        #region "Fill"
        /// <summary>
        /// Get attendence details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private AttendanceDomain ToAttendanceDomain(IDataReader dr)
        {
            return new AttendanceDomain
            {
                AttendanceId = Null.SetNullGuid(dr["AttendanceId"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                Attending = Null.SetNullString(dr["Attending"]),
                Reason = Null.SetNullString(dr["Reason"]),
                FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])),
                MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),
                MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])),
                MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                Suffix = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"]))
            };
        }

        /// <summary>
        /// Fill attendance list
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AttendanceDomain> ToAttendanceDomains(IDataReader dr)
        {
            IList<AttendanceDomain> objAttendance = new List<AttendanceDomain>();
            while (dr.Read())
            {
                objAttendance.Add(ToAttendanceDomain(dr));
            }
            return objAttendance;
        }

        #endregion

        #region "Get"
        /// <summary>
        /// Get Attendace details by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AttendanceDomain> GetAttendanceByMeetingId(Guid MeetingId)
        {
            IList<AttendanceDomain> attendanceDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAttendanceByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAttendanceByMeeting", MeetingId))
            {
                attendanceDomains = ToAttendanceDomains(dataReader);
            }
            conn.Close();
            return attendanceDomains;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Attendance
        /// </summary>
        /// <param name="objAttendance">Class object sepcifing objAttendance</param>
        /// <returns></returns>
        public AttendanceDomain InsertAttendance(AttendanceDomain objAttendance)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertOrUpdateAttendance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objAttendance.MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", objAttendance.UserId);
            cmd.Parameters.AddWithValue("p_Attending", objAttendance.Attending);
            cmd.Parameters.AddWithValue("p_Reason", objAttendance.Reason);
            Object obj = cmd.ExecuteScalar();
            if (obj != null && obj.ToString().Length > 35)
            {
                objAttendance.AttendanceId = Guid.Parse(obj.ToString());
            }
            conn.Close();
            //objAttendance.AttendanceId = (Guid) SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertOrUpdateAttendance", objAttendance.MeetingId, objAttendance.UserId, objAttendance.Attending ,objAttendance.Reason); 
            return objAttendance;
        }
        #endregion

    }

}
