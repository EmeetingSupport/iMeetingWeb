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
    public class ForumDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile ForumDataProvider _instance;
        private static object _synRoot = new Object();

        private ForumDataProvider()
        {
        }

        public static ForumDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ForumDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// To Fill Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ForumDomain ToForumDomain(IDataReader dr)
        {
            return new ForumDomain()
            {
                ForumId = Null.SetNullGuid(dr["ForumId"]),
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),

                ForumShortName = Encryptor.DecryptString(Null.SetNullString(dr["ForumShortName"])),
                ForumChecker = Null.SetNullGuid(dr["ForumChecker"]),

                IsEnable = Null.SetNullBoolean(dr["IsEnable"]),
                MembersInfo = Null.SetNullString(dr["MembersInfo"]),
                //GeneralManager = Encryptor.DecryptString(Null.SetNullString(dr["GeneralManager"])),
                //GMUpdatedBy = Null.SetNullGuid(dr["GMUpdatedBy"]),
                //GMUpdatedOn = Null.SetNullDateTime(dr["GMUpdatedOn"])
            };
        }

        /// <summary>
        /// To Fill Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ForumDomain> ToForumDomains(IDataReader dr)
        {
            IList<ForumDomain> forumDoamins = new List<ForumDomain>();
            while (dr.Read())
            {
                forumDoamins.Add(ToForumDomain(dr));
            }
            return forumDoamins;
        }

        /// <summary>
        /// To Fill Forum Details with user
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ForumDomain> ToForumDomainsUser(IDataReader dr)
        {
            IList<ForumDomain> forumDoamins = new List<ForumDomain>();
            ForumDomain objForum;
            while (dr.Read())
            {
                objForum = ToForumDomain(dr);
                objForum.UserId = Null.SetNullGuid(dr["UserId"]);
                forumDoamins.Add(objForum);

            }
            return forumDoamins;
        }

        ///// <summary>
        /////  To Fill Forum Details with entity name 
        ///// </summary>
        ///// <param name="dr"></param>
        ///// <returns></returns>
        //private ForumDomain ToForumDomainWithEntity(IDataReader dr)
        //{
        //    return new ForumDomain()
        //    {
        //        ForumId = Null.SetNullGuid(dr["ForumId"]),
        //        EntityId = Null.SetNullGuid(dr["EntityId"]),
        //        ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
        //        CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
        //        UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
        //        UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
        //        CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
        //        EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"]))

        //    };
        //}

        ///// <summary>
        ///// To Fill Forum Details with entity name
        ///// </summary>
        ///// <param name="dr"></param>
        ///// <returns></returns>
        //private IList<ForumDomain> ToForumDomainsWithEntity(IDataReader dr)
        //{
        //    IList<ForumDomain> forumDoamins = new List<ForumDomain>();
        //    while (dr.Read())
        //    {
        //        forumDoamins.Add(ToForumDomainWithEntity(dr));
        //    }
        //    return forumDoamins;
        //}
        #endregion

        #region "Get"
        /// <summary>
        /// Get forums by userid with entity
        /// </summary>
        /// <param name="UserId">Forum with entity</param>
        /// <returns></returns>
        public IList<ForumDomain> GetUserForums(Guid UserId)
        {           
            IList<ForumDomain> forumDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumsForAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumsForAccess",UserId, EntityId))
            {
                forumDomains = ToForumDomainsUser(dataReader);
            }
            conn.Close();
            return forumDomains;
        }

        /// <summary>
        /// Get Forum Details By ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public ForumDomain Get(Guid ForumId)
        {
            ForumDomain entityDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumDetailsById", ForumId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                entityDomain = ToForumDomain(dataReader);
            }
            conn.Close();
            return entityDomain;
        }

       
        /// <summary>
        /// Get All Forum Details
        /// </summary>
        /// <returns></returns>
        public IList<ForumDomain> Get()
        {
            IList<ForumDomain> forumDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllForumDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllForumDetails"))
            {
                forumDomains = ToForumDomains(dataReader);
            }
            conn.Close();
            return forumDomains;
        }

        /// <summary>
        /// Get Forums by User id 
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<ForumDomain> GetForumsByUserId(Guid UserId)
        {
            IList<ForumDomain> forumDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumByUserId", UserId))
            {
                forumDomains = ToForumDomains(dataReader);
            }
            conn.Close();
            return forumDomains;
        }
                    
        /// <summary>
        /// Get Forums by User id 
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public IList<ForumDomain> GetForumsByUserAccess(Guid UserId, Guid EntityId)
        {
            IList<ForumDomain> forumDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumByUserAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumByUserAccess", UserId, EntityId))
            {
                forumDomains = ToForumDomains(dataReader);
            }
            conn.Close();
            return forumDomains;
        }

        /// <summary>
        /// Get Forum Details By Entity ID
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public IList<ForumDomain> GetForumByEntityId(Guid EntityId)
        {
            IList<ForumDomain> forumDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumDetailsByEntityId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumDetailsByEntityId", EntityId))
            {
                forumDomains = ToForumDomains(dataReader);//ToForumDomainsWithEntity(dataReader);

            }
            conn.Close();
            return forumDomains;
        }
        #endregion


        #region "Modify"

        /// <summary>
        /// Insert Forum Details
        /// </summary>
        /// <param name="forumDomain">Class object specifying forumDomain</param>
        /// <returns></returns>
        public ForumDomain Insert(ForumDomain forumDomain, string sendToChecker)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertForumDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumName", Encryptor.EncryptString(forumDomain.ForumName));
            cmd.Parameters.AddWithValue("p_EntityId", forumDomain.EntityId);
            cmd.Parameters.AddWithValue("p_CreatedBy", forumDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", forumDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_ForumShortName", Encryptor.EncryptString(forumDomain.ForumShortName));
            cmd.Parameters.AddWithValue("p_ForumChecker", forumDomain.ForumChecker);
            cmd.Parameters.AddWithValue("p_IsEnable", forumDomain.IsEnable);
            cmd.Parameters.AddWithValue("p_MembersInfo", forumDomain.MembersInfo);
            cmd.Parameters.AddWithValue("p_sendToChecker", sendToChecker);
            forumDomain.ForumId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //forumDomain.ForumId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertForumDetails", Encryptor.EncryptString(forumDomain.ForumName), forumDomain.EntityId, forumDomain.CreatedBy, forumDomain.UpdatedBy, Encryptor.EncryptString(forumDomain.ForumShortName), forumDomain.ForumChecker, forumDomain.IsEnable, forumDomain.MembersInfo, sendToChecker);

            return forumDomain;
        }
        /// <summary>
        /// Delete Forum Details BY Id
        /// </summary>
        /// <param name="ForumID">Guid specifying ForumId</param>
        /// <returns></returns>
        public bool Delete(Guid ForumID)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteForumById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumID", ForumID);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteForumById", ForumID)) > 0;
        }

        /// <summary>
        /// Delete seletd Forum by id
        /// </summary>
        /// <param name="ForumIds">string specifying ForumIds</param>
        /// <returns></returns>
        public bool DeleteSelectedForum(string ForumIds)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedForumById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumIds", ForumIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

          //  return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSelectedForumById", ForumIds)) > 0;
        }
        /// <summary>
        /// Update Forum Details BY Id
        /// </summary>
        /// <param name="forumdomain">Class object speciying forumdomain</param>
        /// <returns></returns>
        public Guid Update(ForumDomain forumdomain, string sendToChecker)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateForumDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", forumdomain.ForumId);
            cmd.Parameters.AddWithValue("p_ForumName", Encryptor.EncryptString(forumdomain.ForumName));
            cmd.Parameters.AddWithValue("p_EntityId", forumdomain.EntityId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", forumdomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_ForumShortName", Encryptor.EncryptString(forumdomain.ForumShortName));
            cmd.Parameters.AddWithValue("p_ForumChecker", forumdomain.ForumChecker);
            cmd.Parameters.AddWithValue("p_IsEnable", forumdomain.IsEnable);
            cmd.Parameters.AddWithValue("p_MembersInfo", forumdomain.MembersInfo);
            cmd.Parameters.AddWithValue("p_sendToChecker", sendToChecker);
            Guid res= (Guid)(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //return (Guid)(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateForumDetailsById", forumdomain.ForumId, Encryptor.EncryptString(forumdomain.ForumName), forumdomain.EntityId, forumdomain.UpdatedBy, Encryptor.EncryptString(forumdomain.ForumShortName), forumdomain.ForumChecker, forumdomain.IsEnable, forumdomain.MembersInfo, sendToChecker));
        }


        /// <summary>
        /// Update Forum Ordes
        /// </summary>
        /// <param name="strForumOrder">string sepcifing strForumOrder</param>
        /// <param name="strAgendaIds">string sepcifing strForumIds</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateForumOrders(string strForumOrder, string strForumIds, Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UnlockUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateForumOrder", UserId, strForumIds, strForumOrder))
            //    > 0;
        }

        public IList<ForumDomain> GetForumsForAudit(Guid EntityId)
        {
            IList<ForumDomain> forumDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumForAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumForAudit", EntityId))
            {
                forumDomains = ToForumDomains(dataReader);
            }
            conn.Close();
            return forumDomains;
        }
        #endregion

        public bool UpdateGeneralManager(ForumDomain forumdomain, Guid userid)
        {
            //bool res = false;
            //int a = SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateGeneralManagere", forumdomain.ForumId, userid, forumdomain.GMUpdatedBy, Encryptor.EncryptString(forumdomain.GeneralManager));
            //if (a > 0)
            //{
            //    res = true;
            //}
            //return res;

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateGeneralManagere", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", forumdomain.ForumId);
            cmd.Parameters.AddWithValue("p_UserId", userid);
            //cmd.Parameters.AddWithValue("p_GMUpdatedBy", forumdomain.GMUpdatedBy);
            //cmd.Parameters.AddWithValue("p_GeneralManager", Encryptor.EncryptString(forumdomain.GeneralManager));
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }
    }
}
