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
    public class UserAgendaDataProvider : DataProvider
    { 
        #region "Singleton"
      private static volatile UserAgendaDataProvider _instance;
        private static object synRoot = new Object();

        private UserAgendaDataProvider()
        {

        }

        public static UserAgendaDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserAgendaDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
      #endregion

        #region "Fill"
        /// <summary>
        /// Fill user forum details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private UserAgendaDomain ToUserAgendaDomain(IDataReader dr)
        {
            return new UserAgendaDomain
            {
                UserAgendaId = Null.SetNullGuid(dr["UserAgendaId"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),
                IsShow = Null.SetNullBoolean(dr["IsShow"]),
                IsActive = Null.SetNullBoolean(dr["IsActive"]),
              //  IsUpdate = Null.SetNullBoolean(dr["IsUpdate"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdateOn = Null.SetNullDateTime(dr["UpdateOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdateBy = Null.SetNullGuid(dr["UpdateBy"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"])
            };
        }

      
        /// <summary>
        /// Fill user forum details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private IList<UserAgendaDomain> ToUserAgendaDomains(IDataReader dr)
        {
            IList<UserAgendaDomain> objUserAgendaDomains = new List<UserAgendaDomain>();
            while (dr.Read())
            {
                objUserAgendaDomains.Add(ToUserAgendaDomain(dr));
            }
            return objUserAgendaDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get user agenda by meeting
        /// </summary>
        /// <param name="MeetingId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<UserAgendaDomain> GetUserAgendaByMeeting(Guid MeetingId, Guid UserId)
        {
            IList<UserAgendaDomain> objUserAgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserAgenda", UserId, MeetingId))
            {
                objUserAgendaDomain = ToUserAgendaDomains(dataReader);
            }
            conn.Close();
            return objUserAgendaDomain;
        }

        #endregion
        #region "Modify"
        /// <summary>
        /// Insert user Agenda access
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool Insert(UserAgendaDomain objUserAgenda, string AgendaIds)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAgendaAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", objUserAgenda.UserId);
            cmd.Parameters.AddWithValue("p_MeetingId", objUserAgenda.MeetingId);
            cmd.Parameters.AddWithValue("p_CreatedBy", objUserAgenda.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objUserAgenda.UpdateBy);
            cmd.Parameters.AddWithValue("p_AgendaIds", AgendaIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAgendaAccess", objUserAgenda.UserId, objUserAgenda.MeetingId, objUserAgenda.CreatedBy, objUserAgenda.UpdateBy, AgendaIds)) > 0;
        }


        /// <summary>
        /// Insert user Agenda access
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool Update(UserAgendaDomain objUserAgenda, string InsertAgendaIds, string DeleteAgendaIds)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateUserAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", objUserAgenda.UserId);
            cmd.Parameters.AddWithValue("p_MeetingId", objUserAgenda.MeetingId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objUserAgenda.UpdateBy);
            cmd.Parameters.AddWithValue("p_InsertAgendaIds", InsertAgendaIds);
            cmd.Parameters.AddWithValue("p_DeleteAgendaIds", DeleteAgendaIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



           // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateUserAgenda", objUserAgenda.UserId, objUserAgenda.MeetingId, objUserAgenda.UpdateBy, InsertAgendaIds, DeleteAgendaIds)) > 0;
        }
        #endregion
    }
}
