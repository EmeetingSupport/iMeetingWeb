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
    public class EntityDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile EntityDataProvider _instance;
        private static object _synRoot = new Object();

        private EntityDataProvider()
        {
        }

        public static EntityDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new EntityDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Entity details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private EntityDomain ToEntityDomain(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string EncryptionKey = "";
            if(clsmain.checkColumExist("EncryptionKey",dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "EncryptionKey");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]));
            }
            return new EntityDomain()
            {
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),

                EntityLogo = Encryptor.DecryptString(Null.SetNullString(dr["EntityLogo"])),
                EntityMeeting = Encryptor.DecryptString(Null.SetNullString(dr["EntityMeeting"])),
                EntityChecker = Null.SetNullGuid(dr["EntityChecker"]),
                IsEnable = Null.SetNullBoolean(dr["IsEnable"]),
                EntityShortName = Encryptor.DecryptString(Null.SetNullString(dr["EntityShortName"])),
                EncryptionKey = EncryptionKey
            };
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<EntityDomain> ToEntityDoamins(IDataReader dr)
        {
            IList<EntityDomain> entityDoamins = new List<EntityDomain>();
            while (dr.Read())
            {
                entityDoamins.Add(ToEntityDomain(dr));
            }
            return entityDoamins;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Entity Details By Id
        /// </summary>
        /// <param name="EntityId">Guid sepcifing EntityId</param>
        /// <returns></returns>
        public EntityDomain Get(Guid EntityId)
        {
            EntityDomain entityDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEntityDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetEntityDetailsById", EntityId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                entityDomain = ToEntityDomain(dataReader);
            }
            conn.Close();
            return entityDomain;
        }

        /// <summary>
        /// Get Entity Details By Get
        /// </summary>
        /// <returns></returns>
        public IList<EntityDomain> Get()
        {
            IList<EntityDomain> entityDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllEntityDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllEntityDetails"))

            entityDomains = ToEntityDoamins(dataReader);
            conn.Close();
            return entityDomains;
        }

        /// <summary>
        /// Get Entity by user id
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public IList<EntityDomain> GetEntityByUserId(Guid UserId)
        {
            IList<EntityDomain> entityDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllEntityByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllEntityByUserId", UserId))
            {
                entityDomains = ToEntityDoamins(dataReader);
            }
            conn.Close();
            return entityDomains;
        }
        #endregion


        #region "Modify"

        /// <summary>
        /// Insert Entity Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying entitydomain</param>
        /// <returns></returns>
        public EntityDomain Insert(EntityDomain entitydomain, string sendToChecker, int MaxEntity = 3)
        {
            //SqlParameter p = new SqlParameter();
            //p.ParameterName = "EntityId";
            //p.DbType = DbType.Guid;
            //p.Direction = ParameterDirection.Output;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertEntityDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityName", Encryptor.EncryptString(entitydomain.EntityName));
            cmd.Parameters.AddWithValue("p_CreatedBy", entitydomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", entitydomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EntityShortName", Encryptor.EncryptString(entitydomain.EntityShortName));
            cmd.Parameters.AddWithValue("p_IsEnable", entitydomain.IsEnable);
            cmd.Parameters.AddWithValue("p_EntityLogo", Encryptor.EncryptString(entitydomain.EntityLogo));
            cmd.Parameters.AddWithValue("p_EntityChecker", entitydomain.EntityChecker);
            cmd.Parameters.AddWithValue("p_EntityMeeting", Encryptor.EncryptString(entitydomain.EntityMeeting));
            cmd.Parameters.AddWithValue("p_sendToChecker", sendToChecker);
            cmd.Parameters.AddWithValue("p_AllowedEntity", MaxEntity);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(entitydomain.EncryptionKey));

            entitydomain.EntityId = (Guid)(cmd.ExecuteScalar());

            conn.Close();
            //entitydomain.EntityId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertEntityDetails", Encryptor.EncryptString(entitydomain.EntityName), entitydomain.CreatedBy, entitydomain.UpdatedBy, Encryptor.EncryptString(entitydomain.EntityShortName), entitydomain.IsEnable, Encryptor.EncryptString(entitydomain.EntityLogo), entitydomain.EntityChecker, Encryptor.EncryptString(entitydomain.EntityMeeting), sendToChecker, MaxEntity, MM.Core.Encryptor.EncryptString(entitydomain.EncryptionKey));

            return entitydomain;
        }

        /// <summary>
        /// Delete Entity By ID
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId </param>
        /// <returns></returns>
        public bool Delete(Guid EntityId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteEntityById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            
            int a=cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteEntityById", EntityId)) > 0;
        }

        /// <summary>
        /// Update Entity By ID
        /// </summary>
        /// <param name="entitydoamin">Class object specifying entitydomain</param>
        /// <returns></returns>
        public bool Update(EntityDomain entitydomain, string sendToChecker)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateEnityDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", entitydomain.EntityId);
            cmd.Parameters.AddWithValue("p_EntityName", Encryptor.EncryptString(entitydomain.EntityName));
            cmd.Parameters.AddWithValue("p_UpdatedBy", entitydomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EntityShortName", Encryptor.EncryptString(entitydomain.EntityShortName));
            cmd.Parameters.AddWithValue("p_IsEnable", entitydomain.IsEnable);
            cmd.Parameters.AddWithValue("p_EntityLogo", Encryptor.EncryptString(entitydomain.EntityLogo));
            cmd.Parameters.AddWithValue("p_EntityChecker", entitydomain.EntityChecker);
            cmd.Parameters.AddWithValue("p_EntityMeeting", Encryptor.EncryptString(entitydomain.EntityMeeting));
            cmd.Parameters.AddWithValue("p_sendToChecker", sendToChecker);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(entitydomain.EncryptionKey));

            int a=cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();

            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateEnityDetailsById", entitydomain.EntityId, Encryptor.EncryptString(entitydomain.EntityName), entitydomain.UpdatedBy, Encryptor.EncryptString(entitydomain.EntityShortName), entitydomain.IsEnable, Encryptor.EncryptString(entitydomain.EntityLogo), entitydomain.EntityChecker, Encryptor.EncryptString(entitydomain.EntityMeeting), sendToChecker, MM.Core.Encryptor.EncryptString(entitydomain.EncryptionKey))) > 0;
        }


        /// <summary>
        /// Delete Seletecd Entity by Entity ID
        /// </summary>
        /// <param name="SeletedEntity">string specifying SeletedEntity</param>
        /// <returns></returns>
        public bool DeleteSelectedEntity(string SeletedEntity)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedEntityById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityIds", SeletedEntity);

            int a=cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSelectedEntityById", SeletedEntity)) > 0;
        }
        #endregion
    }
}
