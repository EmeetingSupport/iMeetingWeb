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
    public class MarkedAgendaDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile MarkedAgendaDataProvider _instance;
        private static object synRoot = new Object();

        private MarkedAgendaDataProvider()
        {

        }

        public static MarkedAgendaDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new MarkedAgendaDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill marked agenda details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private MarkedAgendaDomain ToMarkedAgendaDomain(IDataReader dr)
        {
            return new MarkedAgendaDomain
            {
                AgendaMarkerId = Null.SetNullGuid(dr["AgendaMarkerId"]),
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),
                IsActive = Null.SetNullBoolean(dr["IsActive"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                IsRead = Null.SetNullBoolean(dr["IsRead"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"])
            };
        }


        /// <summary>
        /// Fill marked agenda details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private IList<MarkedAgendaDomain> ToMarkedAgendaDomains(IDataReader dr)
        {
            IList<MarkedAgendaDomain> objMarkedAgendaDomains = new List<MarkedAgendaDomain>();
            while (dr.Read())
            {
                objMarkedAgendaDomains.Add(ToMarkedAgendaDomain(dr));
            }
            return objMarkedAgendaDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get marked agenda by meeting
        /// </summary>
        /// <param name="MeetingId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<MarkedAgendaDomain> GetMarkedAgendaByMeeting(Guid MeetingId)
        {
            IList<MarkedAgendaDomain> objMarkedAgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMarkedAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMarkedAgenda",  MeetingId))
            {
                objMarkedAgendaDomain = ToMarkedAgendaDomains(dataReader);
            }
            conn.Close();
            return objMarkedAgendaDomain;
        }

        /// <summary>
        /// Get marked agenda by meeting
        /// </summary>
        /// <param name="MeetingId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<MarkedAgendaDomain> GetAllMarkedAgendaByMeeting(Guid MeetingId)
        {
            IList<MarkedAgendaDomain> objMarkedAgendaDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllMarkedAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllMarkedAgenda", MeetingId))
            {
                objMarkedAgendaDomain = ToMarkedAgendaDomains(dataReader);
            }
            conn.Close();
            return objMarkedAgendaDomain;
        }
        #endregion

         
        #region "Modify"
        /// <summary>
        /// Insert user Agenda access
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool Insert(MarkedAgendaDomain objMarkedAgenda, string AgendaIds, string UnReadAgendaIds)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAgendaMarker", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objMarkedAgenda.MeetingId);
            cmd.Parameters.AddWithValue("p_CreatedBy", objMarkedAgenda.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objMarkedAgenda.UpdatedBy);
            cmd.Parameters.AddWithValue("p_AgendaIds", AgendaIds);
            cmd.Parameters.AddWithValue("p_UnReadAgendaIds", UnReadAgendaIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAgendaMarker", objMarkedAgenda.MeetingId, objMarkedAgenda.CreatedBy, objMarkedAgenda.UpdatedBy, AgendaIds, UnReadAgendaIds)) > 0;
        }


        /// <summary>
        /// Insert user Agenda access
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool Update(MarkedAgendaDomain objUserAgenda, string InsertAgendaIds, string DeleteAgendaIds)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateUserAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objUserAgenda.MeetingId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objUserAgenda.UpdatedBy);
            cmd.Parameters.AddWithValue("p_InsertAgendaIds", InsertAgendaIds);
            cmd.Parameters.AddWithValue("p_DeleteAgendaIds", DeleteAgendaIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateUserAgenda", objUserAgenda.MeetingId, objUserAgenda.UpdatedBy, InsertAgendaIds, DeleteAgendaIds)) > 0;
        }
        #endregion
    }
}
