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
    public class AboutUsDataProvider : DataProvider
    {
           #region "Singleton"
        private static volatile AboutUsDataProvider _instance;
        private static object _synRoot = new object();

        private AboutUsDataProvider()
        {
        }

        public static AboutUsDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AboutUsDataProvider();
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
        private ABoutUSDomain ToABoutUSDomain(IDataReader dr)
        {
            return new ABoutUSDomain()
            {
                Id = Null.SetNullInteger(dr["Id"]),
                Text = Null.SetNullString(dr["Text"]),
                Image = Null.SetNullString(dr["Image"]),
            
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                ModifiedBy = Null.SetNullGuid(dr["ModifiedBy"]),
                ModifiedOn = Null.SetNullDateTime(dr["ModifiedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),             
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                EncryptionKey = MM.Core.Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]))
            };

        }

        /// <summary>
        /// Get All Key info details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<ABoutUSDomain> ToABoutUSDomains(IDataReader dr)
        {
            IList<ABoutUSDomain> objABoutUSDomain = new List<ABoutUSDomain>();
            while (dr.Read())
            {
                objABoutUSDomain.Add(ToABoutUSDomain(dr));
            }
            return objABoutUSDomain;
        }
        #endregion

           #region "Get"
        /// <summary>
        /// Get Key info by id
        /// </summary>
        /// <param name="KeyInfoId">Int specifying AboutUSId</param>
        /// <returns></returns>
        public ABoutUSDomain Get(Int32 AboutUsId)
        {
            ABoutUSDomain objABoutUSDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAboutUsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AboutUsId", AboutUsId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAboutUsById", AboutUsId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objABoutUSDomain = ToABoutUSDomain(dataReader);
            }
            conn.Close();
            return objABoutUSDomain;
        }

        /// <summary>
        /// Get all About us list
        /// </summary>
        /// <param name="AboutUsId"></param>
        /// <returns></returns>
        public ABoutUSDomain Get()
        {
            ABoutUSDomain objABoutUSDomain = null;


            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllAboutUs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAboutUs"))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objABoutUSDomain = ToABoutUSDomain(dataReader);
            }
            conn.Close();
            return objABoutUSDomain;
        }

        /// <summary>
        /// Get All about us by entityId 
        /// </summary>
        /// <returns></returns>
        public IList<ABoutUSDomain> GetABoutUSDomainByEntity(Guid EntityId)
        {
            IList<ABoutUSDomain> objABoutUSDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAboutUsByEntityId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAboutUsByEntityId", EntityId))
            {
                objABoutUSDomain = ToABoutUSDomains(dataReader);
            }
            conn.Close();
            return objABoutUSDomain;
        }

         #endregion

        #region "Modify"

        /// <summary>
        /// Insert Aboutus Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying entitydomain</param>
        /// <returns></returns>
        public ABoutUSDomain Insert(ABoutUSDomain objABoutUSDomain)
        {
            //SqlParameter p = new SqlParameter();
            //p.ParameterName = "EntityId";
            //p.DbType = DbType.Guid;
            //p.Direction = ParameterDirection.Output;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAboutUs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Text", objABoutUSDomain.Text);
            cmd.Parameters.AddWithValue("p_Image", objABoutUSDomain.Image);
            cmd.Parameters.AddWithValue("p_CreatedBy", objABoutUSDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_ModifiedBy", objABoutUSDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", objABoutUSDomain.EntityId);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(objABoutUSDomain.EncryptionKey));
            objABoutUSDomain.Id = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            // objABoutUSDomain.Id = (Int32)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAboutUs", objABoutUSDomain.Text, objABoutUSDomain.Image, objABoutUSDomain.CreatedBy, objABoutUSDomain.CreatedBy, objABoutUSDomain.EntityId,MM.Core.Encryptor.EncryptString(objABoutUSDomain.EncryptionKey));

            return objABoutUSDomain;
        }

        /// <summary>
        /// Delete about us By ID
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId </param>
        /// <returns></returns>
        public bool Delete(Int32 AboutUsId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteAboutUs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AboutUsId", AboutUsId);

            int result = cmd.ExecuteNonQuery();
            if (result > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteAboutUs", AboutUsId)) > 0;
        }

        /// <summary>
        /// Update About us By ID
        /// </summary>
        /// <param name="entitydoamin">Class object specifying entitydomain</param>
        /// <returns></returns>
        public bool Update(ABoutUSDomain objABoutUSDomain)
        {
            bool res=false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAboutUs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Text", objABoutUSDomain.Text);
            cmd.Parameters.AddWithValue("p_Image",  objABoutUSDomain.Image);
            cmd.Parameters.AddWithValue("p_ModifiedBy", objABoutUSDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", objABoutUSDomain.EntityId);
            cmd.Parameters.AddWithValue("p_AboutUsId", objABoutUSDomain.Id);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(objABoutUSDomain.EncryptionKey));

            int result = cmd.ExecuteNonQuery();
            if (result > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateAboutUs", objABoutUSDomain.Text, objABoutUSDomain.Image, objABoutUSDomain.CreatedBy, objABoutUSDomain.EntityId, objABoutUSDomain.Id, MM.Core.Encryptor.EncryptString(objABoutUSDomain.EncryptionKey))) > 0;
        }

              
        #endregion
    }
}
