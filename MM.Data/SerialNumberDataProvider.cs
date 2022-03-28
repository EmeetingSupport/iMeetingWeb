using Microsoft.ApplicationBlocks.Data;
using MM.Core;
using MM.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MM.Data
{
     public class SerialNumberDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile SerialNumberDataProvider _instance;
        private static object _synRoot = new Object();

        private SerialNumberDataProvider()
        {
        }

        public static SerialNumberDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SerialNumberDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// To Fill Serial Number Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private SerialNumberDomain ToSerialNumberDomain(IDataReader dr)
        {
            return new SerialNumberDomain()
            {
                SerialNumberId = Null.SetNullInteger(dr["SerialNumberId"]),
                SerialNumber = Null.SetNullString(dr["SerialNumber"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
               
            };
        }

        /// <summary>
        /// To Fill All SerialNumber Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<SerialNumberDomain> ToSerialNumberDomains(IDataReader dr)
        {
            IList<SerialNumberDomain> objSerialNumber = new List<SerialNumberDomain>();
            while (dr.Read())
            {
                objSerialNumber.Add(ToSerialNumberDomain(dr));
            }
            return objSerialNumber;
        }


        #endregion

        #region "Get"
        /// <summary>
        /// Get Serial Number  By ID
        /// </summary>
        /// <param name="SerialNumberId">Guid specifying SerialNumberId</param>
        /// <returns></returns>
        public SerialNumberDomain Get(int SerialNumberId)
        {
            SerialNumberDomain SerialNumberDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetSerialNumberById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SerialNumberId", SerialNumberId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetSerialNumberById", SerialNumberId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                SerialNumberDomain = ToSerialNumberDomain(dataReader);

            }
            conn.Close();
            return SerialNumberDomain;
        }

        /// <summary>
        /// Get All Serial Number Details
        /// </summary>
        /// <returns></returns>
        public IList<SerialNumberDomain> Get()
        {
            IList<SerialNumberDomain> objSerialNumberDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllSerialNumbers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllSerialNumbers", EntityId))
            {
                objSerialNumberDomains = ToSerialNumberDomains(dataReader);
            }
            conn.Close();
            return objSerialNumberDomains;
        }


        #endregion


        #region "Modify"

        /// <summary>
        /// Insert Serial Number Details
        /// </summary>
        /// <param name="SerialNumberDomain">Class object specifying SerialNumberDomain</param>
        /// <returns></returns>
        public SerialNumberDomain Insert(SerialNumberDomain SerialNumberDomain)
        {
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertSerialNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SerialNumber", SerialNumberDomain.SerialNumber);
            cmd.Parameters.AddWithValue("p_CreatedBy", SerialNumberDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", SerialNumberDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            SerialNumberDomain.SerialNumberId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            //SerialNumberDomain.SerialNumberId = (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertSerialNumber", SerialNumberDomain.SerialNumber, SerialNumberDomain.CreatedBy, SerialNumberDomain.UpdatedBy, EntityId);
            return SerialNumberDomain;
        }
        /// <summary>
        /// Update Serial Number 
        /// </summary>
        /// <param name="meetingDomain">Class object specifying meetingDomain</param>
        /// <returns></returns>
        public bool Update(SerialNumberDomain SerialNumberDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateSerialNumberById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SerialNumberId", SerialNumberDomain.SerialNumberId);
            cmd.Parameters.AddWithValue("p_SerialNumber", SerialNumberDomain.SerialNumber);
            cmd.Parameters.AddWithValue("p_UpdatedBy", SerialNumberDomain.UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateSerialNumberById", SerialNumberDomain.SerialNumberId, SerialNumberDomain.SerialNumber, SerialNumberDomain.UpdatedBy)) > 0;
        }
        /// <summary>
        /// Delete Serial Number
        /// </summary>
        /// <param name="SerialNumberId">Guid specifying SerialNumberId</param>
        /// <returns></returns>
        public bool Delete(int SerialNumberId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSerialNumberById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SerialNumberId", SerialNumberId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSerialNumberById", SerialNumberId)) > 0;
        }
      
        #endregion
    }
}
