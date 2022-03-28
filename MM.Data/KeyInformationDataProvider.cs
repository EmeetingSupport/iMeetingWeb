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
    public class KeyInformationDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile KeyInformationDataProvider _instance;
        private static object _synRoot = new object();

        private KeyInformationDataProvider()
        {
        }

        public static KeyInformationDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new KeyInformationDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Key info. details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private KeyInformationDomain ToKeyInformationDomain(IDataReader dr)
        {
            return new KeyInformationDomain()
        {
            KeyInformationId = Null.SetNullGuid(dr["KeyInformationId"]),
            Description = Null.SetNullString(dr["Description"]),
            Designation = Null.SetNullString(dr["Designation"]),
            Photograph = Null.SetNullString(dr["Photograph"]),
            CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
            UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
            UpdatedOn = Null.SetNullDateTime(dr["UpdateOn"]),
            CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
            Name = Null.SetNullString(dr["Name"]),
            IsActive = Null.SetNullBoolean(dr["IsActive"]),
            Suffix = Null.SetNullString(dr["Suffix"]),
            EntityId = Null.SetNullGuid(dr["EntityId"])
        };

        }

        /// <summary>
        /// Get All Key info details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<KeyInformationDomain> ToKeyInformationDomains(IDataReader dr)
        {
            IList<KeyInformationDomain> objKeyInfoDomain = new List<KeyInformationDomain>();
            while (dr.Read())
            {
                objKeyInfoDomain.Add(ToKeyInformationDomain(dr));
            }
            return objKeyInfoDomain;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Key info by id
        /// </summary>
        /// <param name="KeyInfoId">Guid specifying KeyInfoId</param>
        /// <returns></returns>
        public KeyInformationDomain Get(Guid KeyInfoId)
        {
            KeyInformationDomain objKeyInfo = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetKeyInfoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_KeyInformationId", KeyInfoId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetKeyInfoById", KeyInfoId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objKeyInfo = ToKeyInformationDomain(dataReader);
            }
            conn.Close();
            return objKeyInfo;
        }

        /// <summary>
        /// Get All Key information 
        /// </summary>
        /// <returns></returns>
        public IList<KeyInformationDomain> GetKeyInfoByEntity(Guid EntityId)
        {
            IList<KeyInformationDomain> objKeyInfo = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllKeyInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllKeyInfo", EntityId))
            {
                objKeyInfo = ToKeyInformationDomains(dataReader);
            }
            conn.Close();
            return objKeyInfo;
        }

        /// <summary>
        /// Get All Key information for wcf
        /// </summary>
        /// <returns></returns>
        // IList<KeyInformationDomain>
        public DataSet  GetKeyInfo(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
             

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllKeyInfoForWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("p_EndTime", EndTime);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(ds);
         
            conn.Close();
            //DataSet ds = (DataSet)SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAllKeyInfoForWcf", StartTime, EndTime, EntityId);
            return ds;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Key information
        /// </summary>
        /// <param name="KeyInfo">Class object specifying KeyInformationDomain</param>
        /// <returns></returns>
        public KeyInformationDomain Insert(KeyInformationDomain KeyInfo)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertKeyInfo", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Designation", KeyInfo.Designation);
            cmd.Parameters.AddWithValue("p_Description", KeyInfo.Description);
            cmd.Parameters.AddWithValue("p_Photograph", KeyInfo.Photograph);
            cmd.Parameters.AddWithValue("p_CreatedBy", KeyInfo.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", KeyInfo.UpdatedBy);
            cmd.Parameters.AddWithValue("p_Name", KeyInfo.Name);
            cmd.Parameters.AddWithValue("p_EntityId", KeyInfo.EntityId);
            cmd.Parameters.AddWithValue("p_Suffix", KeyInfo.Suffix);

            //string res =Convert.ToString( cmd.ExecuteScalar());

            KeyInfo.KeyInformationId = (Guid)cmd.ExecuteScalar();
            conn.Close();

            //KeyInfo.KeyInformationId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertKeyInfo", KeyInfo.Designation, KeyInfo.Description, KeyInfo.Photograph, KeyInfo.CreatedBy, KeyInfo.UpdatedBy,KeyInfo.Name, KeyInfo.EntityId, KeyInfo.Suffix);
            return KeyInfo;
        }

        /// <summary>
        /// Delete key information by id
        /// </summary>
        /// <param name="KeyInfoId">Guid specifiying KeyInfoId</param>
        /// <returns></returns>
        public bool DeleteKeyInfoById(Guid KeyInfoId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteKeyInfoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_KeyInformationId", KeyInfoId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteKeyInfoById", KeyInfoId)) > 0;
        }

        /// <summary>
        /// Update Key information
        /// </summary>
        /// <param name="KeyInfo">Class object specifying KeyInfoDomain</param>
        /// <returns></returns>
        public bool UpdateKeyInfo(KeyInformationDomain KeyInfoDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateKeyInfoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_KeyInformationId", KeyInfoDomain.KeyInformationId);
            cmd.Parameters.AddWithValue("p_Designation", KeyInfoDomain.Designation);
            cmd.Parameters.AddWithValue("p_Description", KeyInfoDomain.Description);
            cmd.Parameters.AddWithValue("p_Photograph", KeyInfoDomain.Photograph);
            cmd.Parameters.AddWithValue("p_UpdatedBy", KeyInfoDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_Name", KeyInfoDomain.Name);
            cmd.Parameters.AddWithValue("p_EntityId", KeyInfoDomain.EntityId);
            cmd.Parameters.AddWithValue("p_Suffix", KeyInfoDomain.Suffix);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateKeyInfoById", KeyInfoDomain.KeyInformationId, KeyInfoDomain.Designation, KeyInfoDomain.Description, KeyInfoDomain.Photograph, KeyInfoDomain.UpdatedBy, KeyInfoDomain.Name,KeyInfoDomain.EntityId, KeyInfoDomain.Suffix)) > 0;
        }

        /// <summary>
        /// Update KeyInfo Ordes
        /// </summary>
        /// <param name="strKeyInfoOrder">string sepcifing strKeyInfoOrder</param>
        /// <param name="strKeyInfoIds">string sepcifing strKeyInfoIds</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateKeyInfoOrders(string strKeyInfoOrder, string strKeyInfoIds, Guid UserId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateKeyInformationOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserId);
            cmd.Parameters.AddWithValue("p_KeyInformationIds", strKeyInfoIds);
            cmd.Parameters.AddWithValue("p_KeyInformationOrders", strKeyInfoOrder);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
            
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateKeyInformationOrder", UserId, strKeyInfoIds, strKeyInfoOrder))
            //    > 0;
        }
        #endregion
    }
}
