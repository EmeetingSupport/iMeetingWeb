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
    public class UserAttendanceDataprovider : DataProvider
    {
        #region "Singleton"
        private static volatile UserAttendanceDataprovider _instance;
        private static object _synRoot = new Object();

        private UserAttendanceDataprovider()
        {
        }

        public static UserAttendanceDataprovider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserAttendanceDataprovider();
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
        private UserAttendanceDomain ToUserAttendanceDomain(IDataReader dr)
        {
            return new UserAttendanceDomain
            {
                Id = Null.SetNullGuid(dr["Id"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                Attendance=Null.SetNullBoolean(dr["Attendance"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdateOn = Null.SetNullDateTime(dr["UpdateOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
 
            };
        }

        /// <summary>
        /// Fill attendance list
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UserAttendanceDomain> ToUserAttendanceDomains(IDataReader dr)
        {
            IList<UserAttendanceDomain> objAttendance = new List<UserAttendanceDomain>();
            while (dr.Read())
            {
                objAttendance.Add(ToUserAttendanceDomain(dr));
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
        public IList<UserAttendanceDomain> GetAttendanceByMeetingId(Guid MeetingId)
        {
            IList<UserAttendanceDomain> UserAttendanceDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserAttendanceByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserAttendanceByMeeting", MeetingId))
            {
                UserAttendanceDomains = ToUserAttendanceDomains(dataReader);
            }
            conn.Close();
            return UserAttendanceDomains;
        }


        public DataSet GetAttendanceReport(Guid ForumId,Guid MeetingId,Guid UserId,DateTime StartDate,DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAttendanceReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();


            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAttendanceReport", ForumId, MeetingId, UserId, StartDate, EndDate, PageNo, PageSize);

            return ds;
        }

        public DataSet GetUserbyMeetingForUserAttendance(Guid MeetingId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserbyMeetingForUserAttendance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            // DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUserbyMeetingForUserAttendance", MeetingId);

            return ds;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Attendance
        /// </summary>
        /// <param name="objAttendance">Class object sepcifing objAttendance</param>
        /// <returns></returns>
        public UserAttendanceDomain InsertUserAttendance(UserAttendanceDomain objAttendance)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertOrUpdateUserAttendance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objAttendance.MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", objAttendance.UserId);
            cmd.Parameters.AddWithValue("p_Attendance", objAttendance.Attendance);
            cmd.Parameters.AddWithValue("p_CreatedBy", objAttendance.CreatedBy);
            objAttendance.Id = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //objAttendance.Id = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertOrUpdateUserAttendance", objAttendance.MeetingId, objAttendance.UserId, objAttendance.Attendance,objAttendance.CreatedBy);
            return objAttendance;
        }
        #endregion

    }

}
