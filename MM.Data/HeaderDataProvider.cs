using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using MM.Core;
using MM.Domain;
using MySql.Data.MySqlClient;

namespace MM.Data
{
   public class HeaderDataProvider:DataProvider
    {
        #region "Singleton"
        private static volatile HeaderDataProvider _instance;
        private static object _synRoot = new Object();

        private HeaderDataProvider()
        {
        }

        public static HeaderDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new HeaderDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
       
        #region "Fill"

        /// <summary>
        ///To Fill Header details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private HeaderDomain ToHeaderDomain(IDataReader dr)
        {
            string ForumName = "";
            ClsMain clsmain = new ClsMain();

            if(clsmain.checkColumExist("ForumName", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "ForumName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
            }

            return new HeaderDomain
            {
                HeaderId = Null.SetNullGuid(dr["HeaderId"]),
                Header=Null.SetNullString(dr["Header"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
               ForumName=ForumName
            };
        }


        /// <summary>
        /// To Fill Header Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<HeaderDomain> ToHeaderDomains(IDataReader dr)
        {
            IList<HeaderDomain> headerDomains = new List<HeaderDomain>();
            while (dr.Read())
            {
                headerDomains.Add(ToHeaderDomain(dr));
            }
            return headerDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Header Details By ID
        /// </summary>
        /// <param name="HeaderId">Guid specifying HeaderId</param>
        /// <returns></returns>
        public HeaderDomain Get(Guid HeaderId)
        {
            HeaderDomain headerDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetHeaderById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_HeaderId", HeaderId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetHeaderById", HeaderId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                headerDomain = ToHeaderDomain(dataReader);

            }
            conn.Close();
            return headerDomain;
        }

        /// <summary>
        /// Get All Header Details
        /// </summary>
        /// <returns></returns>
        public IList<HeaderDomain> Get()
        {
            IList<HeaderDomain> objHeaderDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllHeaders", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllHeaders"))
            {
                objHeaderDomains = ToHeaderDomains(dataReader);
            }
            conn.Close();
            return objHeaderDomains;
        }

        /// <summary>
        /// Get All Header by Forum Id
        /// </summary>
        /// <returns></returns>
        public IList<HeaderDomain> GetHeaderByForum(Guid ForumId)
        {
            IList<HeaderDomain> objHeaderDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetHeaderByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetHeaderByForumId",ForumId))
            {
                objHeaderDomains = ToHeaderDomains(dataReader);
            }
            conn.Close();
            return objHeaderDomains;
        }
        #endregion
              
        #region "Modify"
        /// <summary>
        /// Insert Header
        /// </summary>
        /// <param name="HeaderDomain">Class object specifying HeaderDomain</param>
        /// <returns></returns>
        public HeaderDomain Insert(HeaderDomain headerDomain)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertHeader", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Header", headerDomain.Header);
            cmd.Parameters.AddWithValue("p_CreatedBy", headerDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", headerDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_ForumId", headerDomain.ForumId);
            headerDomain.HeaderId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //headerDomain.HeaderId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertHeader", headerDomain.Header, headerDomain.CreatedBy, headerDomain.UpdatedBy,headerDomain.ForumId);
            return headerDomain;
        }

        /// <summary>
        /// Update Header by Id
        /// </summary>
        /// <param name="headerDomain">Class object specifying headerDomain</param>
        /// <returns></returns>
        public bool Update(HeaderDomain headerDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateHeaderById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_HeaderId", headerDomain.HeaderId);
            cmd.Parameters.AddWithValue("p_Header", headerDomain.Header);
            cmd.Parameters.AddWithValue("p_UpdatedBy", headerDomain.UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateHeaderById", headerDomain.HeaderId, headerDomain.Header, headerDomain.UpdatedBy)) > 0;
        }


        /// <summary>
        /// Delete Header
        /// </summary>
        /// <param name="HeaderId">Guid specifying HeaderId</param>
        /// <returns></returns>
        public bool Delete(Guid HeaderId)
        {
            Guid LoggedInUserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteHeader", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_HeaderId", HeaderId);
            cmd.Parameters.AddWithValue("p_LoggedInUserId", LoggedInUserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteHeader", HeaderId, LoggedInUserId)) > 0;

        }


        #endregion
    }
}
