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
    public class IPADataprovider : DataProvider
    {
        #region "Singleton"
        private static volatile IPADataprovider _instance;
        private static object _synRoot = new object();

        private IPADataprovider()
        {
        }

        public static IPADataprovider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IPADataprovider();
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
        private IPADomain ToIPADomain(IDataReader dr)
        {
            return new IPADomain()
            {
                Id = Null.SetNullInteger(dr["Id"]),
                IPAName = Null.SetNullString(dr["IPAName"]),
                IPA = Null.SetNullString(dr["IPA"]),

                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                ModifiedBy = Null.SetNullGuid(dr["ModifiedBy"]),
                ModifiedOn = Null.SetNullDateTime(dr["ModifiedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                EntityId = Null.SetNullGuid(dr["EntityId"])
            };

        }

        /// <summary>
        /// Get All Key info details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<IPADomain> ToIPADomains(IDataReader dr)
        {
            IList<IPADomain> objPDFTemplateDomain = new List<IPADomain>();
            while (dr.Read())
            {
                objPDFTemplateDomain.Add(ToIPADomain(dr));
            }
            return objPDFTemplateDomain;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Key info by id
        /// </summary>
        /// <param name="KeyInfoId">Int specifying AboutUSId</param>
        /// <returns></returns>
        public IPADomain Get(Int32 IPAId)
        {
            IPADomain objIPADomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetIPAById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_TemplateId", IPAId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetIPAById", IPAId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objIPADomain = ToIPADomain(dataReader);
            }
            conn.Close();
            return objIPADomain;
        }

        /// <summary>
        /// Get all About us list
        /// </summary>
        /// <param name="AboutUsId"></param>
        /// <returns></returns>
        public IPADomain Get()
        {
            IPADomain objIPADomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllIPA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllIPA"))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objIPADomain = ToIPADomain(dataReader);
            }
            conn.Close();
            return objIPADomain;
        }

        /// <summary>
        /// Get All about us by entityId 
        /// </summary>
        /// <returns></returns>
        public IList<IPADomain> GetIPADomainByEntity(Guid EntityId)
        {
            IList<IPADomain> objIPADomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetIPAByEntityId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetIPAByEntityId", EntityId))
            {
                objIPADomain = ToIPADomains(dataReader);
            }
            conn.Close();
            return objIPADomain;
        }

        #endregion

        #region "Modify"

        /// <summary>
        /// Insert Aboutus Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying entitydomain</param>
        /// <returns></returns>
        public IPADomain Insert(IPADomain objIPADomain)
        {
            //SqlParameter p = new SqlParameter();
            //p.ParameterName = "EntityId";
            //p.DbType = DbType.Guid;
            //p.Direction = ParameterDirection.Output;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertIPA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_IPAName", objIPADomain.IPAName);
            cmd.Parameters.AddWithValue("p_IPA", objIPADomain.IPA);
            cmd.Parameters.AddWithValue("p_CreatedBy", objIPADomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_ModifiedBy", objIPADomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_EntityId",  objIPADomain.EntityId);
            objIPADomain.Id = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            //            objIPADomain.Id = (Int32)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertIPA", objIPADomain.IPAName, objIPADomain.IPA, objIPADomain.CreatedBy, objIPADomain.CreatedBy, objIPADomain.EntityId);

            return objIPADomain;
        }

        /// <summary>
        /// Delete about us By ID
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId </param>
        /// <returns></returns>
        public bool Delete(Int32 IPAId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteIPA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_IPAId", IPAId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteIPA", IPAId)) > 0;
        }

        /// <summary>
        /// Update About us By ID
        /// </summary>
        /// <param name="entitydoamin">Class object specifying entitydomain</param>
        /// <returns></returns>
        public bool Update(IPADomain objIPADomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateIPA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_IPAName", objIPADomain.IPAName);
            cmd.Parameters.AddWithValue("p_IPA",objIPADomain.IPA);
            cmd.Parameters.AddWithValue("p_ModifiedBy", objIPADomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", objIPADomain.EntityId);
            cmd.Parameters.AddWithValue("p_IPAId", objIPADomain.Id);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateIPA", objIPADomain.IPAName, objIPADomain.IPA, objIPADomain.CreatedBy, objIPADomain.EntityId, objIPADomain.Id)) > 0;
        }


        #endregion

    }

}
