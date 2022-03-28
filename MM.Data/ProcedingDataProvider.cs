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
    public class ProcedingDataProvider :DataProvider
    {
        #region "Singleton"
        private static volatile ProcedingDataProvider _instance;
        private static object _synRoot = new Object();

        private ProcedingDataProvider()
        {
        }

        public static ProcedingDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ProcedingDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"

        /// <summary>
        ///To Fill Proceding details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ProcedingDomain ToProcedingDomain(IDataReader dr)
        {
            string EncryptionKey = "";
            ClsMain clsmain = new ClsMain();
            if(clsmain.checkColumExist("EncryptionKey", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "EncryptionKey");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]));
            }

            return new ProcedingDomain
            {
                ProcedingId = Null.SetNullGuid(dr["ProcedingId"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                ForumId = Null.SetNullGuid(dr["ForumId"]),
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),
                SubAgendaId = Null.SetNullGuid(dr["SubAgendaId"]),
                ProcedingName=Null.SetNullString(dr["ProcedingName"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                //IsActive=Null.SetNullBoolean(dr["IsActive"]),
                EncryptionKey = EncryptionKey
            };
        }

        /// <summary>
        /// To Fill Proceding Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ProcedingDomain> ToProcedingDomains(IDataReader dr)
        {
            IList<ProcedingDomain> procedingDomains = new List<ProcedingDomain>();
            while (dr.Read())
            {
                procedingDomains.Add(ToProcedingDomain(dr));
            }
            return procedingDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get all Proceding details 
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet Get()
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllProcedings", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAllProcedings");
            return ds;

        }


        /// <summary>
        /// Get  Proceding details 
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet Get(Guid ForumId ,Guid MeetingId )
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedings", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetProcedings", ForumId, MeetingId);
            return ds;

        }


        /// <summary>
        /// Get  Proceding for search 
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet GetProcedingsForSearch(Guid ForumId, Guid MeetingId, int PageNo = 1, int PageSize = 10)
        {
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedingsForSearch", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_PageIndex", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();


            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetProcedingsForSearch", ForumId, MeetingId,PageNo,PageSize, UserId, EntityId);
            return ds;

        }

        /// <summary>
        /// Get Proceding by id
        /// </summary>
        /// <param name="UploadMinutesId">Guid specifying ProcedingId</param>
        /// <returns></returns>
        public ProcedingDomain Get(Guid ProcedingId)
        {
            ProcedingDomain procedingDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedingsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ProcedingId", ProcedingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetProcedingsById", ProcedingId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                procedingDomain = ToProcedingDomain(dataReader);
            }
            conn.Close();
            return procedingDomain;
        }


        public DataSet GetProcedingReport(DateTime startDate, DateTime endDate)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedingReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_startDate", startDate);
            cmd.Parameters.AddWithValue("p_endDate", endDate);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetProcedingReport", startDate, endDate);
            return ds;

        }


        public DataSet GetProcedingForView()
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedingForView", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetProcedingForView");
            return ds;

        }

        /// <summary>		      
        /// Get All proceeding information for wcf		
        /// </summary>		
        /// <returns></returns>		
        // IList<ProceedingDomain>		
        public DataSet GetProceedingInfo(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllProceedingInfoForWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("p_EndTime", EndTime);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();

            //DataSet ds = (DataSet)SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAllProceedingInfoForWcf", StartTime, EndTime, EntityId);
            return ds;
        }


        /// <summary>
        /// Get  Proceding for Report 
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet GetProcedingsForReport(Guid ForumId, Guid MeetingId, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetProcedingsForReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageIndex", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetProcedingsForReport", ForumId, MeetingId, StartDate, EndDate, PageNo, PageSize);
            return ds;

        }

        #endregion


        #region "Modify"
        /// <summary>
        /// Insert Procedings
        /// </summary>
        /// <param name="procedingDomain">Class object specifying procedingDomain</param>
        /// <returns></returns>
        public ProcedingDomain Insert(ProcedingDomain procedingDomain)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertProceding", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", procedingDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_ForumId", procedingDomain.ForumId);
            cmd.Parameters.AddWithValue("p_ProcedingName", procedingDomain.ProcedingName);
            cmd.Parameters.AddWithValue("p_CreatedBy", procedingDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", procedingDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(procedingDomain.EncryptionKey));
            procedingDomain.ProcedingId = (Guid)(cmd.ExecuteScalar());
            conn.Close();


            //procedingDomain.ProcedingId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertProceding", procedingDomain.MeetingId, procedingDomain.ForumId, procedingDomain.ProcedingName, procedingDomain.CreatedBy, procedingDomain.UpdatedBy, MM.Core.Encryptor.EncryptString(procedingDomain.EncryptionKey));
            return procedingDomain;
        }

        /// <summary>
        /// Update Proceding by Id
        /// </summary>
        /// <param name="procedingDomain">Class object specifying procedingDomain</param>
        /// <returns></returns>
        public bool Update(ProcedingDomain procedingDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateProcedingById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", procedingDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_ForumId", procedingDomain.ForumId);
            cmd.Parameters.AddWithValue("p_ProcedingName", procedingDomain.ProcedingName);
            cmd.Parameters.AddWithValue("p_UpdatedBy", procedingDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_ProcedingId", procedingDomain.ProcedingId);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(procedingDomain.EncryptionKey));
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateProcedingById", procedingDomain.MeetingId, procedingDomain.ForumId, procedingDomain.ProcedingName, procedingDomain.UpdatedBy, procedingDomain.ProcedingId, MM.Core.Encryptor.EncryptString(procedingDomain.EncryptionKey))) > 0;
        }


        /// <summary>
        /// Delete Proceding
        /// </summary>
        /// <param name="ProcedingId">Guid specifying ProcedingId</param>
        /// <returns></returns>
        public bool Delete(Guid ProcedingId)
        {
            Guid LoggedInUserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteProceding", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ProcedingId", ProcedingId);
            cmd.Parameters.AddWithValue("p_UserId", LoggedInUserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteProceding", ProcedingId, LoggedInUserId)) > 0;

        }


        #endregion
    }
}
