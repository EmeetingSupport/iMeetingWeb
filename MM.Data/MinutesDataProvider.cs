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
    public class MinutesDataProvider : DataProvider
    {
         #region "Singleton"
        private static volatile MinutesDataProvider _instance;
        private static object _synRoot = new Object();

        private MinutesDataProvider()
        {
        }

        public static MinutesDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new MinutesDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

         #region "Fill"
        /// <summary>
        /// To Fill Minutes Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private MinutesDomain ToMinutesDomain(IDataReader dr)
        {
            return new MinutesDomain()
            {
                MinuteId = Null.SetNullGuid(dr["MinuteId"]),
                IsActive=Null.SetNullBoolean(dr["IsActive"]),
                Minutes = Null.SetNullString(dr["Minutes"]),
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"])         
            };
        }

        /// <summary>
        /// To Fill All Minutes Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<MinutesDomain> ToMinutesDomains(IDataReader dr)
        {
            IList<MinutesDomain> objMinute = new List<MinutesDomain>();
            while (dr.Read())
            {
                objMinute.Add(ToMinutesDomain(dr));
            }
            return objMinute;
        } 
  

        #endregion

        #region "Modify"

        /// <summary>
        /// Insert Minutes Details
        /// </summary>
        /// <param name="meetingDomain">Class object specifying MinutesgDomain</param>
        /// <returns></returns>
        public MinutesDomain MinuteOperation(MinutesDomain meetingDomain)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertUpdateMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", meetingDomain.AgendaId);
            cmd.Parameters.AddWithValue("p_Minutes", meetingDomain.Minutes);
            cmd.Parameters.AddWithValue("p_CreatedBy", meetingDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", meetingDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_MeetingId", meetingDomain.MeetingId);
            meetingDomain.MinuteId = (Guid)(cmd.ExecuteScalar());

            conn.Close();
            //meetingDomain.MinuteId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertUpdateMinutes", meetingDomain.AgendaId, meetingDomain.Minutes, meetingDomain.CreatedBy, meetingDomain.UpdatedBy,meetingDomain.MeetingId);
            return meetingDomain;
        }
       
        /// <summary>
        /// Delete Mom uploaded
        /// </summary>
        /// <param name="AgendaId">Guid specifying agendaId</param>
        /// <returns></returns>
        public bool DeleteMinutes(Guid AgendaId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("sp_DeleteMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "sp_DeleteMinutes", AgendaId)) > 0;
        }
        #endregion
    }
}
