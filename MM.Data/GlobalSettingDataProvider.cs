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
   public class GlobalSettingDataProvider : DataProvider
    {
         #region "Singleton"

        private static volatile GlobalSettingDataProvider instance;
        private static object synRoot = new Object();

        private GlobalSettingDataProvider()
        {
        }

        public static GlobalSettingDataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (synRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GlobalSettingDataProvider();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

         #region "Fill"
        /// <summary>
        /// Get One module
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private GlobalSettingDomain ToGlobalSetting(IDataReader dr)
        {
            return new GlobalSettingDomain()
            {
                Id = Null.SetNullGuid(dr["Id"]),
                Value = Null.SetNullString(dr["Value"]),
                Key = Null.SetNullString(dr["Key"]),
            };
        }


        /// <summary>
        /// get list of global values
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<GlobalSettingDomain> ToGlobalSettingList(IDataReader dr)
        {
            IList<GlobalSettingDomain> news = new List<GlobalSettingDomain>();
            while (dr.Read())
            {
                news.Add(ToGlobalSetting(dr));
            }
            return news;
        }


        #endregion
       
         #region "Get"
        /// <summary>
        /// GET ALL Global Values OBJECT
        /// </summary>
        /// <returns></returns>
        public IList<GlobalSettingDomain> GetGlobalSettingByEntity(Guid EntityId)
        {

            IList<GlobalSettingDomain> GlobalValueList = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllGlobalSetting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllGlobalSetting", EntityId))
            {
                GlobalValueList = ToGlobalSettingList(dataReader);
            }
            conn.Close();
            return GlobalValueList;
        }

        /// <summary>
        /// Get By Global Value ID
        /// </summary>
        /// <param name="GlobalValueID">Guid specifying GlobalValueID</param>
        /// <returns></returns>
        public GlobalSettingDomain GetByGlobalSettingID(Guid GlobalValueID)
        {
            GlobalSettingDomain GlobalSettingObj = new GlobalSettingDomain();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetGlobalSettingById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Id", GlobalValueID);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetGlobalSettingById", GlobalValueID))
            {
                while (dataReader.Read())
                    GlobalSettingObj = ToGlobalSetting(dataReader);
            }
            conn.Close();
            return GlobalSettingObj;
        }


        #endregion

         #region "Modify"

        /// <summary>
        /// Update Global Values
        /// </summary>
        /// <param name="objGlobal">Class object specifying objGlobal</param>
        /// <returns></returns>
        public bool Update(GlobalSettingDomain objGlobal)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateGlobalSetting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Id", objGlobal.Id);
            cmd.Parameters.AddWithValue("p_Value", objGlobal.Value);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateGlobalSetting", objGlobal.Id, objGlobal.Value)) > 0;
        }


        #endregion

    }
}
