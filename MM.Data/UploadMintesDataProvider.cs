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
    public class UploadMintesDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile UploadMintesDataProvider _instance;
        private static object _synRoot = new Object();

        private UploadMintesDataProvider()
        {
        }

        public static UploadMintesDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UploadMintesDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"

        /// <summary>
        ///To Fill Upload Minutes details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UploadMinutesDomain ToUploadMinutesDomain(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string EncryptionKey = "";
            if(clsmain.checkColumExist("EncryptionKey", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "EncryptionKey");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]));
            }

            return new UploadMinutesDomain
            {
                UploadMinuteId = Null.SetNullGuid(dr["UploadMinuteId"]),
                UplaodMinuteChecker = Null.SetNullGuid(dr["UplaodMinuteChecker"]),
                UploadFile = Null.SetNullString(dr["UploadFile"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                EncryptionKey = EncryptionKey
            };
        }

        /// <summary>
        /// To Fill uplaod Minutes Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UploadMinutesDomain> ToUploadMinuteDomains(IDataReader dr)
        {
            IList<UploadMinutesDomain> uploadMinutesDomains = new List<UploadMinutesDomain>();
            while (dr.Read())
            {
                uploadMinutesDomains.Add(ToUploadMinutesDomain(dr));
            }
            return uploadMinutesDomains;
        }

        /// <summary>
        /// To Fill All Upload minutes Details with forum and entity name and user details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UploadMinutesDomain> ToUploadMinutesDomainWithUser(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            IList<UploadMinutesDomain> objUploadMinutesDomain = new List<UploadMinutesDomain>();
            UploadMinutesDomain objMinutes = null;
            while (dr.Read())
            {
                objMinutes = new UploadMinutesDomain();

                objMinutes.FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"]));
                objMinutes.LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"]));
                objMinutes.UploadFile = Null.SetNullString(dr["UploadFile"]);
                objMinutes.MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"]));
                objMinutes.MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]));
                objMinutes.MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"]));

                objMinutes.MeetingId = Null.SetNullGuid(dr["MeetingId"]);
                objMinutes.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objMinutes.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objMinutes.UploadMinuteId = Null.SetNullGuid(dr["UploadMinuteId"]);
                objMinutes.IsApproved = Null.SetNullString(dr["IsApproved"]);
                objMinutes.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));

                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "UpdatedBy");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                if (clsmain.checkColumExist("UpdatedBy", dr))
                {
                    objMinutes.UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]);
                }

                string EncryptionKey = "";
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "EncryptionKey");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                if (clsmain.checkColumExist("EncryptionKey", dr))
                {
                    EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]));
                }

                string MeetingNumber = "";
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingNumber");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                if (clsmain.checkColumExist("MeetingNumber", dr))
                {
                    MeetingNumber =Null.SetNullString(dr["MeetingNumber"]);
                }
                objMinutes.MeetingNumber = MeetingNumber;
                objMinutes.EncryptionKey = EncryptionKey;

                //if (dr.GetSchemaTable().Columns["UpdatedBy"] != null)
                //{
                //    objMinutes.UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]);
                //}
                objUploadMinutesDomain.Add(objMinutes);
            }
            return objUploadMinutesDomain;

        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get  Upload minutes details by meeting
        /// </summary>
        /// <returns></returns>
        public UploadMinutesDomain GetUploadMinutesByMeetingId(Guid MeetingId)
        {
            UploadMinutesDomain uploadMinutesDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMinutesBymeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMinutesBymeetingId", MeetingId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                uploadMinutesDomains = ToUploadMinutesDomain(dataReader);
            }
            conn.Close();
            return uploadMinutesDomains;
        }



        /// <summary>
        /// Get all Upload minutes details
        /// </summary>
        /// <returns></returns>
        public IList<UploadMinutesDomain> Get()
        {
            IList<UploadMinutesDomain> uploadMinutesDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUploadMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUploadMinutes"))
            {
                uploadMinutesDomains = ToUploadMinuteDomains(dataReader);
            }
            conn.Close();
            return uploadMinutesDomains;
        }

        /// <summary>
        /// Get All unapproved minutes
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public IList<UploadMinutesDomain> GetUnapprovedMinutes(Guid UserId)
        {
            IList<UploadMinutesDomain> uploadMinutesDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUnApporvedMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUnApporvedMinutes", UserId))
            {
                uploadMinutesDomains = ToUploadMinutesDomainWithUser(dataReader);
            }
            conn.Close();
            return uploadMinutesDomains;
        }

        /// <summary>
        /// Get upload minutes by id
        /// </summary>
        /// <param name="UploadMinutesId">Guid specifying UploadMinutesId</param>
        /// <returns></returns>
        public UploadMinutesDomain Get(Guid UploadMinutesId)
        {
            UploadMinutesDomain uploadMinutesDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUploadMinutesById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UploadMinutesId", UploadMinutesId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUploadMinutesById", UploadMinutesId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                uploadMinutesDomain = ToUploadMinutesDomain(dataReader);
            }
            conn.Close();
            return uploadMinutesDomain;
        }

        /// <summary>
        /// Get Upload minutes by user id(only allowed)
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<UploadMinutesDomain> GetUploadMinutesByUser(Guid UserId)
        {
            IList<UploadMinutesDomain> uploadMinutesDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUploadMinutesByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUploadMinutesByUser", UserId))
            {
                uploadMinutesDomains = ToUploadMinutesDomainWithUser(dataReader);
            }
            conn.Close();
            return uploadMinutesDomains;
        }

        /// <summary>
        /// Get Upload minutes by user id(only allowed)
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<UploadMinutesDomain> GetUploadMinutesByUserAcess(Guid UserId)
        {
            IList<UploadMinutesDomain> uploadMinutesDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUploadMinutesByUserAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUploadMinutesByUserAccess", UserId, EntityId ))
            {
                uploadMinutesDomains = ToUploadMinutesDomainWithUser(dataReader);
            }
            conn.Close();
            return uploadMinutesDomains;
        }

        /// <summary>
        /// Get Upload minutes by user id for report(only allowed)
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public DataSet GetUploadMinutesByUserAcessForReport(Guid UserId, Guid ForumId, Guid MeetingId, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUploadMinutesByUserAccessForReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
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
            da.Fill(ds);
            conn.Close();


            // DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUploadMinutesByUserAccessForReport", UserId, ForumId, MeetingId, StartDate, EndDate, PageNo, PageSize);
            return ds;
        }

        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Upload minutes
        /// </summary>
        /// <param name="uploadMinutesDomain">Class object specifying uploadMinutesDomain</param>
        /// <returns></returns>
        public UploadMinutesDomain Insert(UploadMinutesDomain uploadMinutesDomain)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertUploadMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UploadFile", uploadMinutesDomain.UploadFile);
            cmd.Parameters.AddWithValue("p_MeetingId", uploadMinutesDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_CreatedBy", uploadMinutesDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", uploadMinutesDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_UplaodMinuteChecker", uploadMinutesDomain.UplaodMinuteChecker);
            cmd.Parameters.AddWithValue("p_SendToChecker", uploadMinutesDomain.IsApproved);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(uploadMinutesDomain.EncryptionKey));
            uploadMinutesDomain.UploadMinuteId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //uploadMinutesDomain.UploadMinuteId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertUploadMinutes", uploadMinutesDomain.UploadFile, uploadMinutesDomain.MeetingId, uploadMinutesDomain.CreatedBy, uploadMinutesDomain.UpdatedBy, uploadMinutesDomain.UplaodMinuteChecker, uploadMinutesDomain.IsApproved, MM.Core.Encryptor.EncryptString(uploadMinutesDomain.EncryptionKey));
            return uploadMinutesDomain;
        }

        /// <summary>
        /// Update Upload minutes by Id
        /// </summary>
        /// <param name="uploadMinutesDomain">Class object specifying uploadMinutesDomain</param>
        /// <returns></returns>
        public bool Update(UploadMinutesDomain uploadMinutesDomain)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateUploadMinutesById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UploadFile", uploadMinutesDomain.UploadFile);
            cmd.Parameters.AddWithValue("p_MeetingId", uploadMinutesDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", uploadMinutesDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_UplaodMinuteChecker", uploadMinutesDomain.UplaodMinuteChecker);
            cmd.Parameters.AddWithValue("p_UploadMinuteId", uploadMinutesDomain.UploadMinuteId);
            cmd.Parameters.AddWithValue("p_SendToChecker", uploadMinutesDomain.IsApproved);
            cmd.Parameters.AddWithValue("p_EncryptionKey", MM.Core.Encryptor.EncryptString(uploadMinutesDomain.EncryptionKey));
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateUploadMinutesById", uploadMinutesDomain.UploadFile, uploadMinutesDomain.MeetingId, uploadMinutesDomain.UpdatedBy, uploadMinutesDomain.UplaodMinuteChecker, uploadMinutesDomain.UploadMinuteId, uploadMinutesDomain.IsApproved,  MM.Core.Encryptor.EncryptString(uploadMinutesDomain.EncryptionKey))) > 0;
        }


        /// <summary>
        /// Delete Upload Minutes
        /// </summary>
        /// <param name="UploadMinutesId">Guid specifying UploadMinutesId</param>
        /// <returns></returns>
        public bool Delete(Guid UploadMinutesId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteUploadMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UploadMinuteId", UploadMinutesId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteUploadMinutes", UploadMinutesId)) > 0;

        }


        #endregion
    }
}
