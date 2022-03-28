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
    public class UserForumDataprovider : DataProvider
    {
        #region "Singleton"
        private static volatile UserForumDataprovider _instance;
        private static object synRoot = new Object();

        private UserForumDataprovider()
        {

        }

        public static UserForumDataprovider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserForumDataprovider();
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
        private UserForumDomain ToUserForumDomain(IDataReader dr)
        {
            return new UserForumDomain
            {
                UserForumId = Null.SetNullGuid(dr["UserForumId"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                ForumId = Null.SetNullGuid(dr["ForumId"]),
                IsAdd = Null.SetNullBoolean(dr["IsAdd"]),
                IsDelete = Null.SetNullBoolean(dr["IsDelete"]),
                IsUpdate = Null.SetNullBoolean(dr["IsUpdate"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdateOn = Null.SetNullDateTime(dr["UpdateOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdateBy = Null.SetNullGuid(dr["UpdateBy"])
            };
        }

        /// <summary>
        /// Fill user forum details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private IList<UserForumDomain> ToUserForumDomains(IDataReader dr)
        {
            IList<UserForumDomain> objUserForumDomain = new List<UserForumDomain>();
            while (dr.Read())
            {
                objUserForumDomain.Add(ToUserForumDomain(dr));
            }
            return objUserForumDomain;
        }

        /// <summary>
        /// Fill user forum details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private UserForumDomain ToUserForumDomainWithEntity(IDataReader dr)
        {
            UserForumDomain objUserForum = ToUserForumDomain(dr);
            objUserForum.UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"]));
            objUserForum.EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"]));
            objUserForum.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
            objUserForum.UserId = Null.SetNullGuid(dr["UserId"]);
            return objUserForum;
        }

        /// <summary>
        /// Fill user forum details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private IList<UserForumDomain> ToUserForumDomainsWithEntity(IDataReader dr)
        {
            IList<UserForumDomain> objUserForumDomain = new List<UserForumDomain>();
            while (dr.Read())
            {
                objUserForumDomain.Add(ToUserForumDomainWithEntity(dr));
            }
            return objUserForumDomain;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get UserForum by id
        /// </summary>
        /// <param name="UserForumId">Class object specifying UserForumId</param>
        /// <returns></returns>
        public IList<UserForumDomain> Get(Guid UserForumId)
        {
            IList<UserForumDomain> objUserForum = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumAccessById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserForumId", UserForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumAccessById", UserForumId))
            {
                objUserForum = ToUserForumDomains(dataReader);
            }
            conn.Close();
            return objUserForum;
        }


        /// <summary>
        /// Get UserForum by user id
        /// </summary>
        /// <param name="UserId"></param>F
        /// <returns></returns>
        public IList<UserForumDomain> GetUserForumByUserId(Guid UserId)
        {
            IList<UserForumDomain> objUserForum = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumAccessByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumAccessByUser", UserId, EntityId))
            {
                objUserForum = ToUserForumDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objUserForum;
        }

        public IList<UserForumDomain> GetUserForumByUserId(Guid UserId, Guid EntityId)
        {
            IList<UserForumDomain> objUserForum = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumAccessByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumAccessByUser", UserId, EntityId))
            {
                objUserForum = ToUserForumDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objUserForum;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert forum access rights 
        /// </summary>
        /// <param name="UserId">string specifing userIds</param>
        /// <param name="Accessrights">string specifying Accessrights </param>
        /// <param name="CreatedBy">Guid specifying created by </param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <returns></returns>
        public bool InsertAll(string UserIds, string Accessrights, Guid CreatedBy, Guid UpdatedBy)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertUserForums", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserIds);
            cmd.Parameters.AddWithValue("p_AccessRights", Accessrights);
            cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertUserForums", UserIds, Accessrights, CreatedBy, UpdatedBy)) > 0;

        }

        /// <summary>
        /// Update forum access rights
        /// </summary>
        /// <param name="UserId">string specifing userIds</param>
        /// <param name="Accessrights">string specifying Accessrights </param>
        /// <param name="CreatedBy">Guid specifying created by </param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="InsertUserIds">string specifying InsertUserIds</param>
        /// <param name="InsertForumAccess">string specifying insertForumAccess</param>
        /// <param name="DeleteForums">string specifying DeleteForums</param>
        /// <returns></returns>
        public bool UpdateAll(string UserIds, string Accessrights, Guid CreatedBy, Guid UpdatedBy, string InsertUserIds, string InsertForumAccess, string DeleteForums)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateForums", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserIds);
            cmd.Parameters.AddWithValue("p_AccessRights", Accessrights);
            cmd.Parameters.AddWithValue("p_CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("p_InsertUserIds", InsertUserIds);
            cmd.Parameters.AddWithValue("p_InsertAcceessRights", InsertForumAccess);
            cmd.Parameters.AddWithValue("p_DeleteUserIds", DeleteForums);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateForums", UserIds, Accessrights, CreatedBy, UpdatedBy, InsertUserIds, InsertForumAccess, DeleteForums)) > 0;
        }

        /// <summary>
        /// Delete forum access by userId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteForumAccess(Guid UserId)
        {
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["Entityid"].ToString());
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteUserForumById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_DeleteUserForumById", UserId, EntityId)) > 0;
        }
        #endregion
    }
}
