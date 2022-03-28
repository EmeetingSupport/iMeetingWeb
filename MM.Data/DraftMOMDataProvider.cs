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
    public class DraftMOMDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile DraftMOMDataProvider _instance;
        private static object _synRoot = new object();

        private DraftMOMDataProvider()
        {
        }

        public static DraftMOMDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new DraftMOMDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Draft MOM  details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private DraftMOMDomain ToDraftMOMDomain(IDataReader dr)
        {
            return new DraftMOMDomain()
            {
                DraftMinutesId = Null.SetNullGuid(dr["DraftMinutesId"]),
                DraftMinute = Null.SetNullString(dr["DraftMinute"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdateBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                DraftMinuteName = Null.SetNullString(dr["DraftMOMName"])
            };

        }

        //<summary>
        //Get All DraftMOM 
        //</summary>
        //<param name="dr">IDataReader sepcifing dr</param>
        //<returns></returns>
        public IList<DraftMOMDomain> ToDraftMOMDomains(IDataReader dr)
        {
            IList<DraftMOMDomain> objDraftMOMDomains = new List<DraftMOMDomain>();
            while (dr.Read())
            {
                objDraftMOMDomains.Add(ToDraftMOMDomain(dr));
            }
            return objDraftMOMDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Draft MOM by id
        /// </summary>
        /// <param name="KeyInfoId">Int specifying AboutUSId</param>
        /// <returns></returns>
        public DraftMOMDomain Get(Guid DraftMOMId)
        {
            DraftMOMDomain objDraftMOMDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetDraftMOMById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_DraftMinutesId", DraftMOMId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetDraftMOMById", DraftMOMId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objDraftMOMDomain = ToDraftMOMDomain(dataReader);
            }
            conn.Close();
            return objDraftMOMDomain;
        }

        /// <summary>
        /// Get All Draft MOM by entityId 
        /// </summary>
        /// <returns></returns>
        public IList<DraftMOMDomain> GetDraftMOMDomainByEntity(Guid EntityId)
        {
            IList<DraftMOMDomain> objDraftMOMDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllDraftMOM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllDraftMOM", EntityId))
            {
                objDraftMOMDomain = ToDraftMOMDomains(dataReader);
            }
            conn.Close();
            return objDraftMOMDomain;
        }

        /// <summary>
        /// Get All Draft MOM by entityId 
        /// </summary>
        /// <returns></returns>
        public IList<DraftMOMDomain> GetDraftMOMDomainByMeetingid(Guid MeetingId)
        {
            IList<DraftMOMDomain> objDraftMOMDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetDraftMOMByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetDraftMOMByMeetingId", MeetingId))
            {
                objDraftMOMDomain = ToDraftMOMDomains(dataReader);
            }
            conn.Close();
            return objDraftMOMDomain;
        }


        #endregion

        #region "Modify"

        /// <summary>
        /// Insert DraftMOM Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying entitydomain</param>
        /// <returns></returns>
        public DraftMOMDomain Insert(DraftMOMDomain objDraftMOMDomain)
        {
            //SqlParameter p = new SqlParameter();
            //p.ParameterName = "EntityId";
            //p.DbType = DbType.Guid;
            //p.Direction = ParameterDirection.Output;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertDraftMoM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_DraftMinute", objDraftMOMDomain.DraftMinute);
            cmd.Parameters.AddWithValue("p_CreatedBy", objDraftMOMDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", objDraftMOMDomain.EntityId);
            cmd.Parameters.AddWithValue("p_MeetingId", objDraftMOMDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_DraftMOMName", objDraftMOMDomain.DraftMinuteName);
            objDraftMOMDomain.DraftMinutesId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //objDraftMOMDomain.DraftMinutesId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertDraftMoM", objDraftMOMDomain.DraftMinute, objDraftMOMDomain.CreatedBy, objDraftMOMDomain.EntityId, objDraftMOMDomain.MeetingId, objDraftMOMDomain.DraftMinuteName);

            return objDraftMOMDomain;
        }

        /// <summary>
        /// Delete about us By ID
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId </param>
        /// <returns></returns>
        public bool Delete(Guid DraftMOMId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteDraftMOM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_DraftMinutesId", DraftMOMId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteDraftMOM", DraftMOMId)) > 0;
        }

        /// <summary>
        /// Update About us By ID
        /// </summary>
        /// <param name="entitydoamin">Class object specifying entitydomain</param>
        /// <returns></returns>
        public bool Update(DraftMOMDomain objDraftMOMDomain)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateDraftMOM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_DraftMinutesId", objDraftMOMDomain.DraftMinutesId);
            cmd.Parameters.AddWithValue("p_DraftMinute", objDraftMOMDomain.DraftMinute);
            cmd.Parameters.AddWithValue("p_UpdateBy", objDraftMOMDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", objDraftMOMDomain.EntityId);
            cmd.Parameters.AddWithValue("p_MeetingId", objDraftMOMDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_DraftMOMName", objDraftMOMDomain.DraftMinuteName);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

//            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateDraftMOM", objDraftMOMDomain.DraftMinutesId, objDraftMOMDomain.DraftMinute, objDraftMOMDomain.UpdatedBy, objDraftMOMDomain.EntityId, objDraftMOMDomain.MeetingId, objDraftMOMDomain.DraftMinuteName)) > 0;
        }

        /// <summary>
        /// Get draft MOM details
        /// </summary>
        /// <param name="UserId">Guid specifying userid</param>
        /// <param name="StartTime">Datetime specifying StartTime</param>
        /// <returns></returns>
        public DataSet DraftMOMWcf(Guid UserId, DateTime StartTime)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetDraftMOM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();

            // DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetDraftMOM", UserId, StartTime);
            return ds;
        }

        #endregion
    }
}
