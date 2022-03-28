using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class AccessRightDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile AccessRightDataProvider _instance;
        private static object _synRoot = new Object();

        private AccessRightDataProvider()
        {
        }

        public static AccessRightDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccessRightDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Access rights
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public AccessRightDomain ToAccessRightDomain(IDataReader dr)
        {
            return new AccessRightDomain
            {
                AccessRightId = Null.SetNullGuid(dr["AccessRightId"]),
                IsDelete = Null.SetNullBoolean(dr["IsDelete"]),
                IsAdd = Null.SetNullBoolean(dr["IsAdd"]),
                IsRead = Null.SetNullBoolean(dr["IsRead"]),
                IsUpdate = Null.SetNullBoolean(dr["IsUpdate"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                EntityId =  Null.SetNullGuid(dr["EntityId"]),
                UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"])),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),

                RollId =  Null.SetNullGuid(dr["RollId"]),
            };
        }

        /// <summary>
        /// Get all access right
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<AccessRightDomain> ToAccessRightDomains(IDataReader dr)
        {
            IList<AccessRightDomain> objAccessRights = new List<AccessRightDomain>();
            while (dr.Read())
            {
                objAccessRights.Add(ToAccessRightDomain(dr));
            }
            return objAccessRights;
        }
        #endregion

        #region "Get"

        /// <summary>
        /// Get all agendas
        /// </summary>
        /// <returns></returns>
        public IList<AccessRightDomain> Get()
        {
            IList<AccessRightDomain> AccessRightDomain = null;


            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllAccessRight", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAccessRight"))
            {
                AccessRightDomain = ToAccessRightDomains(dataReader);
            }
            conn.Close();
            return AccessRightDomain;
        }
        
        /// <summary>
        /// Get Access right by id
        /// </summary>
        /// <param name="UserId">Guid sepcifying UserId</param>
        /// <returns></returns>
        public IList<AccessRightDomain> GetAccessRightByUserId(Guid UserId)
        {
            IList<AccessRightDomain> objAccessRight = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAccessRightByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAccessRightByUserId", UserId, EntityId))
            {
                objAccessRight =ToAccessRightDomains(dataReader);
            }
            conn.Close();
            return objAccessRight;
        }

        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Access Right
        /// </summary>
        /// <param name="AccessRights">String specifying AccessRights</param>
        /// <returns></returns>
        public bool Insert(string AccessRights)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAccessRight", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AccessRights", AccessRights);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAccessRight", AccessRights)) > 0;
        }

         /// <summary>
         /// Insert Access Right
         /// </summary>
         /// <param name="UserIds">string specifying UserIds </param>
        /// <param name="AccessRights">string specifying AccessRights</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="RollId">Guid specifying RollId</param>
         /// <returns></returns>
        public bool InserAccessRight(string UserIds, string AccessRights, Guid CreatedBy, Guid UpdatedBy, Guid RollId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAccessRights", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserIds);
            cmd.Parameters.AddWithValue("p_AccessRights", AccessRights);
            cmd.Parameters.AddWithValue("p_RollId", RollId);
            cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAccessRights", UserIds, AccessRights, RollId, CreatedBy, UpdatedBy)) > 0;
        }


        /// <summary>
        ///  Update Access Right
        /// </summary>
        /// <param name="UserIds">string specifying UserIds</param>
        /// <param name="AccessRights">string specifying UserIds</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="RollId">Guid specifying RollId</param>
        /// <param name="InsertUserIds">string specifying InsertUserIds</param>
        /// <param name="InsertAccessRights">string specifying InsertAccessRights</param>
        /// <param name="DeleteUserIds">string specifying DeleteUserIds</param>
       /// <returns></returns>
        public bool UpdateAccessRight(string UserIds, string AccessRights, Guid CreatedBy, Guid UpdatedBy, Guid RollId, string InsertUserIds, string InsertAccessRights, string DeleteUserIds)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAccessRights", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserIds);
            cmd.Parameters.AddWithValue("p_AccessRights", AccessRights);
            cmd.Parameters.AddWithValue("p_RollId", RollId);
            cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("p_InsertUserIds", InsertUserIds);
            cmd.Parameters.AddWithValue("p_InsertAcceessRights", InsertAccessRights);
            cmd.Parameters.AddWithValue("p_DeleteUserIds", DeleteUserIds);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateAccessRights", UserIds, AccessRights, RollId, CreatedBy, UpdatedBy, InsertUserIds,InsertAccessRights,DeleteUserIds)) > 0;
        }
        /// <summary>
        /// Delete Access right by id
        /// </summary>
        /// <param name="AccessRightId">Guid specifying AccessRightId</param>
        /// <returns></returns>
        public bool DeleteAccessRight(Guid AccessRightId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteAcessRightById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AccessRightId", AccessRightId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_DeleteAcessRightById", AccessRightId)) > 0;
        }

        /// <summary>
        /// Delete Access right by User id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool DeleteAccessRightByuserId(Guid UserId)
        {
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteAccessRightByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_DeleteAccessRightByUserId", UserId, EntityId)) > 0;
        }

        /// <summary>
        /// Delete selected Access right by User ids
        /// </summary>
        /// <param name="UserId">string specifing userId </param>
        /// <returns></returns>
        public bool DeleteSelectedAccessRightByUserId(string UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedAccessRight", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_DeleteSelectedAccessRight", UserId)) > 0;
        }
        
        #endregion
    }
}
