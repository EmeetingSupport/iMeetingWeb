using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Domain;
using System.Data;
using MM.Core;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class GlobalTabDataProvider:DataProvider
    {
        #region "Singleton"
        private static volatile GlobalTabDataProvider _instance;
        private static object _synRoot = new Object();

        private GlobalTabDataProvider()
        {
        }

        public static GlobalTabDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalTabDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        #region "Fill"
        /// <summary>
        /// To Fill Global Tab Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private GlobalTabDomain ToGlobalTabDomain(IDataReader dr)
        {
            return new GlobalTabDomain()
            {
                GlobalTabId = Null.SetNullInteger(dr["GlobalTabId"]),
                Title = Null.SetNullString(dr["Title"]),
                UploadedPDF=Null.SetNullString(dr["UploadedPDF"]),
                DeletedPDF = Null.SetNullString(dr["DeletedPDF"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                GlobalTabNote=Null.SetNullString(dr["GlobalTabNote"]),
                EntityId=Null.SetNullGuid(dr["EntityId"])

            };
        }

        /// <summary>
        /// To Fill All Global tab Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<GlobalTabDomain> ToGlobalTabDomains(IDataReader dr)
        {
            IList<GlobalTabDomain> objGlobalTab = new List<GlobalTabDomain>();
            while (dr.Read())
            {
                objGlobalTab.Add(ToGlobalTabDomain(dr));
            }
            return objGlobalTab;
        }


        #endregion

        #region "Get"
        /// <summary>
        /// Get GlobalTab By ID
        /// </summary>
        /// <param name="GlobalTabId">Guid specifying GlobalTabId</param>
        /// <returns></returns>
        public GlobalTabDomain Get(int GlobalTabId)
        {
            GlobalTabDomain GlobalTabDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetGlobalTabById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_GlobalTabId", GlobalTabId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetGlobalTabById", GlobalTabId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                GlobalTabDomain = ToGlobalTabDomain(dataReader);

            }
            conn.Close();
            return GlobalTabDomain;
        }

        /// <summary>
        /// Get All Serial Number Details
        /// </summary>
        /// <returns></returns>
        public IList<GlobalTabDomain> Get()
        {
            IList<GlobalTabDomain> objGlobalTabDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllGlobalTabs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllGlobalTabs", EntityId))
            {
                objGlobalTabDomains = ToGlobalTabDomains(dataReader);
            }
            conn.Close();
            return objGlobalTabDomains;
        }


        /// <summary>
        /// Get All global tab information for wcf
        /// </summary>
        /// <returns></returns>
        // IList<GlobalTabDomain>
        public DataSet GetGlobalTabInfo(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllGlobalTabInfoForWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("p_EndTime", EndTime);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();

            //            DataSet ds = (DataSet)SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAllGlobalTabInfoForWcf", StartTime, EndTime, EntityId);
            return ds;
        }
        #endregion

        #region "Modify"

        /// <summary>
        /// Insert GlobalTab Details
        /// </summary>
        /// <param name="GlobalTabDomain">Class object specifying GlobalTabDomain</param>
        /// <returns></returns>
        public GlobalTabDomain Insert(GlobalTabDomain GlobalTabDomain)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertGlobalTab", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Title", GlobalTabDomain.Title);
            cmd.Parameters.AddWithValue("p_UploadedPDF", GlobalTabDomain.UploadedPDF);
            cmd.Parameters.AddWithValue("p_DeletedPDF", GlobalTabDomain.DeletedPDF);
            cmd.Parameters.AddWithValue("p_CreatedBy", GlobalTabDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", GlobalTabDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_GlobalTabNote", GlobalTabDomain.GlobalTabNote);
            cmd.Parameters.AddWithValue("p_EntityId", GlobalTabDomain.EntityId);
            GlobalTabDomain.GlobalTabId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            // GlobalTabDomain.GlobalTabId = (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertGlobalTab", GlobalTabDomain.Title, GlobalTabDomain.UploadedPDF, GlobalTabDomain.DeletedPDF, GlobalTabDomain.CreatedBy, GlobalTabDomain.UpdatedBy, GlobalTabDomain.GlobalTabNote,GlobalTabDomain.EntityId);
            return GlobalTabDomain;
        }
        /// <summary>
        /// Update GlobalTab 
        /// </summary>
        /// <param name="GlobalTabDomain">Class object specifying GlobalTabDomain</param>
        /// <returns></returns>
        public bool Update(GlobalTabDomain GlobalTabDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateGlobalTabById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_GlobalTabId", GlobalTabDomain.GlobalTabId);
            cmd.Parameters.AddWithValue("p_Title", GlobalTabDomain.Title);
            cmd.Parameters.AddWithValue("p_UploadedPDF", GlobalTabDomain.UploadedPDF);
            cmd.Parameters.AddWithValue("p_DeletedPDF", GlobalTabDomain.DeletedPDF);
            cmd.Parameters.AddWithValue("p_UpdatedBy", GlobalTabDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_GlobalTabNote", GlobalTabDomain.GlobalTabNote);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateGlobalTabById", GlobalTabDomain.GlobalTabId, GlobalTabDomain.Title, GlobalTabDomain.UploadedPDF, GlobalTabDomain.DeletedPDF, GlobalTabDomain.UpdatedBy,GlobalTabDomain.GlobalTabNote)) > 0;
        }
        /// <summary>
        /// Delete GlobalTab
        /// </summary>
        /// <param name="GlobalTabId">Guid specifying GlobalTabId</param>
        /// <returns></returns>
        public bool Delete(int GlobalTabId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteGlobalTabById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_GlobalTabId", GlobalTabId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



  //          return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteGlobalTabById", GlobalTabId)) > 0;
        }

        #endregion
    }
}
