using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MM.Core;
using System.Web;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class AdMasterDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile AdMasterDataProvider _instance;
        private static object _synRoot = new object();

        private AdMasterDataProvider()
        {

        }

        public static AdMasterDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AdMasterDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Ad domain details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public AdDomain ToAdDomain(IDataReader dr)
        {
            return new AdDomain
            {
                AdMasterId = Null.SetNullInteger(dr["AdMasterId"]),
                FirstName = Null.SetNullString(dr["FirstName"]),
                LastName = Null.SetNullString(dr["LastName"]),
                UserId = Null.SetNullString(dr["UserId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdateBy"]),
                UpdateOn = Null.SetNullDateTime(dr["UpdateOn"])
            };
        }

        /// <summary>
        /// Get all Ad domain details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<AdDomain> ToAdDomains(IDataReader dr)
        {
            IList<AdDomain> objAdomainList = new List<AdDomain>();
            while (dr.Read())
            {
                objAdomainList.Add(ToAdDomain(dr));
            }
            return objAdomainList;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get all AdDomain 
        /// </summary>
        /// <returns></returns>
        public IList<AdDomain> Get()
        {
            IList<AdDomain> objAdDoamin = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllAdUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAdUser"))
            {
                objAdDoamin = ToAdDomains(dataReader);

            }
            conn.Close();
            return objAdDoamin;
        }

        /// <summary>
        /// Get ad Domain by id
        /// </summary>
        /// <param name="AdDomainId">Integer specifying AdDomainId</param>
        /// <returns></returns>
        public AdDomain Get(int AdDomainId)
        {
            AdDomain objAdDoamin = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAdUserById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AdMasterId", AdDomainId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAdUserById", AdDomainId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objAdDoamin = ToAdDomain(dataReader);
            }
            conn.Close();
            return objAdDoamin;
        }

        public AdDomain GetUserByUserId(string UserId)
        {
            AdDomain objAdDoamin = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByAdId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByAdId", UserId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objAdDoamin = ToAdDomain(dataReader);
            }
            conn.Close();
            return objAdDoamin;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert AdDomain Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying objAdDomain</param>
        /// <returns></returns>
        public AdDomain Insert(AdDomain objAdDomain)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAdUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_FirstName", objAdDomain.FirstName);
            cmd.Parameters.AddWithValue("p_LastName", objAdDomain.LastName);
            cmd.Parameters.AddWithValue("p_UserId", objAdDomain.UserId);
            cmd.Parameters.AddWithValue("p_CreatedBy", objAdDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objAdDomain.UpdatedBy);
            objAdDomain.AdMasterId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            //            objAdDomain.AdMasterId = (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAdUser", objAdDomain.FirstName, objAdDomain.LastName, objAdDomain.UserId, objAdDomain.CreatedBy, objAdDomain.UpdatedBy);
            return objAdDomain;
        }

        /// <summary>
        /// Update AdDomain Detail
        /// </summary>
        /// <param name="objAdDomain">Class object specifying objAdDomain</param>
        /// <returns></returns>
        public bool Upadate(AdDomain objAdDomain)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAdUsers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_FirstName", objAdDomain.FirstName);
            cmd.Parameters.AddWithValue("p_LastName", objAdDomain.LastName);
            cmd.Parameters.AddWithValue("p_UserId", objAdDomain.UserId);
            cmd.Parameters.AddWithValue("p_AdMasterId", objAdDomain.AdMasterId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objAdDomain.UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateAdUsers",  objAdDomain.FirstName, objAdDomain.LastName, objAdDomain.UserId, objAdDomain.UpdatedBy, objAdDomain.AdMasterId)) > 0;
        }

        public bool Delete(int AdDomainId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteAdMaster", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AdDomainId", AdDomainId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;

//            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteAdMaster", AdDomainId)) > 0;
        }
        #endregion
    }
}
