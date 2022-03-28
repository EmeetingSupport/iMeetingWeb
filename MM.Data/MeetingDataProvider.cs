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
    public class MeetingDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile MeetingDataProvider _instance;
        private static object _synRoot = new Object();

        private MeetingDataProvider()
        {
        }

        public static MeetingDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new MeetingDataProvider();
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
        private MeetingDomain ToMeetingDomain(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string MeetingNumber = "";
            if (clsmain.checkColumExist("MeetingNumber", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingNumber");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                MeetingNumber = Null.SetNullString(dr["MeetingNumber"]);
            }
            string ForumName = "";
            if (clsmain.checkColumExist("ForumName", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "ForumName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
            }

            string MeetingType = "";
            if (clsmain.checkColumExist("MeetingType", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingType");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                MeetingType = Null.SetNullString(dr["MeetingType"]);
            }

            string Header = "";
            if (clsmain.checkColumExist("Header", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Header");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                Header = Null.SetNullString(dr["Header"]);
            }

            string Convenor = "";
            if (clsmain.checkColumExist("Convenor", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Convenor");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                Convenor = Null.SetNullString(dr["Convenor"]);
            }

            string ConvenorName = "";
            if (clsmain.checkColumExist("ConvenorName", dr))
            {
                ConvenorName = Null.SetNullString(dr["ConvenorName"]);
            }

            string EditOrCancelReason = "";
            if (clsmain.checkColumExist("EditOrCancelReason", dr))
            {
                EditOrCancelReason = Null.SetNullString(dr["EditOrCancelReason"]);
            }

            string Action = "";
            if (clsmain.checkColumExist("Action", dr))
            {
                Action = Null.SetNullString(dr["Action"]);
            }

            string PastMeeting = "";
            if (clsmain.checkColumExist("PastMeeting", dr))
            {
                PastMeeting = Null.SetNullString(dr["PastMeeting"]);
            }

            return new MeetingDomain()
            {
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),
                MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])),
                MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                ForumId = Null.SetNullGuid(dr["ForumId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                MeetingNumber = MeetingNumber,
                MeetingChecker = Null.SetNullGuid(dr["MeetingChecker"]),
                ForumName = ForumName,
                MeetingType = MeetingType,
                Header = Header,
                Convenor = Convenor,
                ConvenorName = Encryptor.DecryptString(ConvenorName),
                EditOrCancelReason = EditOrCancelReason,
                Action = Action,
                PastMeeting = PastMeeting
            };
        }

        /// <summary>
        /// To Fill All Meeting Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<MeetingDomain> ToMeetingDomainsWithEntity(IDataReader dr, out string strHeader)
        {
            IList<MeetingDomain> objMeeting = new List<MeetingDomain>();
            MeetingDomain objMeet = new MeetingDomain();
            string header = "";

            while (dr.Read())
            {
                objMeet = ToMeetingDomain(dr);
                objMeet.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objMeet.EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"]));
                objMeet.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
                objMeeting.Add(objMeet);
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {

                    header = Null.SetNullString(dr["Header"]);
                }
            }
            strHeader = header;
            return objMeeting;

        }
        /// <summary>
        /// To Fill All Meeting Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<MeetingDomain> ToMeetingDomains(IDataReader dr)
        {
            IList<MeetingDomain> objMeeting = new List<MeetingDomain>();
            while (dr.Read())
            {
                objMeeting.Add(ToMeetingDomain(dr));
            }
            return objMeeting;

        }


        /// <summary>
        /// To Fill All Meeting Details with forum and entity name
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<MeetingDomain> ToMeetingDomainsWithEntity(IDataReader dr)
        {
            IList<MeetingDomain> objMeeting = new List<MeetingDomain>();
            MeetingDomain objMeet = new MeetingDomain();
            while (dr.Read())
            {
                objMeet = ToMeetingDomain(dr);
                objMeet.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objMeet.EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"]));
                objMeet.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
                objMeeting.Add(objMeet);
            }
            return objMeeting;

        }


        /// <summary>
        /// To Fill Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private string ToMeetingVenue(IDataReader dr)
        {
            string strVenue =
            Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"]));
            return strVenue;
        }

        /// <summary>
        /// To Fill All Meeting Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<string> ToMeetingVenues(IDataReader dr)
        {
            IList<string> objMeeting = new List<string>();
            while (dr.Read())
            {
                objMeeting.Add(ToMeetingVenue(dr));
            }
            return objMeeting;

        }

        #endregion

        #region "Get"
        /// <summary>
        /// Get Meeting Details By ID
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public MeetingDomain Get(Guid MeetingId)
        {
            MeetingDomain meetingDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingDetailsById", MeetingId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                meetingDomain = ToMeetingDomain(dataReader);

            }
            conn.Close();
            return meetingDomain;
        }

        /// <summary>
        /// Get All Meeting Details
        /// </summary>
        /// <returns></returns>
        public IList<MeetingDomain> Get()
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllMeetingDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllMeetingDetails"))
            {
                objMeetingDomains = ToMeetingDomains(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get Meeting Details by Forum ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByFroumID(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingDetailsByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();




            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingDetailsByForumId", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get Meeting Details by Forum ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByFroumID(Guid ForumId, out string strHeader)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingDetailsWithHeaderByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingDetailsWithHeaderByForumId", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader, out strHeader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get Meeting for agneda
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingForAgenda(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingForAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingForAgenda", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }


        /// <summary>
        /// Get Meeting Details by Forum ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByForUploadMinutes(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingsForUploadMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingsForUploadMinutes", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }
        /// <summary>
        /// Get Meeting Details with entity 
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public MeetingDomain GetMeetingWithEntity(Guid MeetingId)
        {
            MeetingDomain meetingDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingByIdWithEntiy", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingByIdWithEntiy", MeetingId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                meetingDomain = ToMeetingDomain(dataReader);
                meetingDomain.EntityId = Null.SetNullGuid(dataReader["EntityId"]);
                meetingDomain.EntityName = Encryptor.DecryptString(Null.SetNullString(dataReader["EntityName"]));
                meetingDomain.ForumName = Encryptor.DecryptString(Null.SetNullString(dataReader["ForumName"]));
            }
            conn.Close();
            return meetingDomain;
        }

        /// <summary>
        /// Get Meeting by User id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByUser(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingByUserId", UserId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }


        /// <summary>
        /// Get Meeting by User Access
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByUserAccess(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingByUserAcess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingByUserAcess", UserId, EntityId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get Approved Meeting by User id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetApprovedMeetingByUser(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedMeetingByUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedMeetingByUser", UserId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }


        /// <summary>
        /// Get Approved Meeting by User Access
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetApprovedMeetingByUserAccess(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedMeetingByUserAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedMeetingByUserAccess", UserId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }
        /// <summary>
        /// Get All unapproved meeting for agenda 
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetUnApprovedMeetingForAgenda(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingIdOfUnapprovedAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingIdOfUnapprovedAgenda", UserId))
            {
                objMeetingDomains = ToMeetingDomains(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get Meeting Details by Forum ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByForProceeding(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingsForProceeding", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingsForProceeding", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get All  meeting report by year 

        public IList<MeetingDomain> GetMeetingReportByYear(DateTime startDate, DateTime endDate)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingReportByYear", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_startDate", startDate);
            cmd.Parameters.AddWithValue("p_endDate", endDate);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingReportByYear", startDate, endDate, UserId, EntityId))
            {
            }
            objMeetingDomains = ToMeetingDomains(dataReader);
            conn.Close();
            return objMeetingDomains;
        }

        public IList<MeetingDomain> GetMeetingByFroumIDForAudit(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingDetailsByForumIdForAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingDetailsByForumIdForAudit", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>		
        /// Get Meeting Report by Entity id		
        /// </summary>		
        /// <param name="UserId">Guid specifying EntityId</param>		
        /// <returns></returns>		
        public DataSet GetMeetingReportByEntityId(Guid EntityId, DateTime StartDate, DateTime EndDate)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingReportByEntityId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            conn.Open();
            //MySqlDataReader dataReader = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetMeetingReportByEntityId", EntityId, StartDate, EndDate);
            return ds;
        }


        /// <summary>
        /// Get All Meeting Details by Forum ID
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetAllMeetingByForProceeding(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingsForProceedingByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingsForProceedingByForumId", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }
        #endregion

        #region "Modify"

        /// <summary>
        /// Insert Meeting Details
        /// </summary>
        /// <param name="meetingDomain">Class object specifying meetingDomain</param>
        /// <returns></returns>
        public MeetingDomain Insert(MeetingDomain meetingDomain, string SendToChecker)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertMeetingDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingDate", Encryptor.EncryptString(meetingDomain.MeetingDate));
            cmd.Parameters.AddWithValue("p_MeetingVenue", Encryptor.EncryptString(meetingDomain.MeetingVenue));
            cmd.Parameters.AddWithValue("p_MeetingTime", Encryptor.EncryptString(meetingDomain.MeetingTime));
            cmd.Parameters.AddWithValue("p_ForumId", meetingDomain.ForumId);
            cmd.Parameters.AddWithValue("p_CreatedBy", meetingDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", meetingDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_MeetingChecker", meetingDomain.MeetingChecker);
            cmd.Parameters.AddWithValue("p_SendToChecker", SendToChecker);
            cmd.Parameters.AddWithValue("p_MeetingNumber", meetingDomain.MeetingNumber);
            cmd.Parameters.AddWithValue("p_MeetingType", meetingDomain.MeetingType);
            cmd.Parameters.AddWithValue("p_Header", meetingDomain.Header);
            cmd.Parameters.AddWithValue("p_Convenor", meetingDomain.Convenor);
            cmd.Parameters.AddWithValue("p_ConvenorName", Encryptor.EncryptString(meetingDomain.ConvenorName));
            meetingDomain.MeetingId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //            meetingDomain.MeetingId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertMeetingDetails", Encryptor.EncryptString(meetingDomain.MeetingDate), Encryptor.EncryptString(meetingDomain.MeetingVenue), Encryptor.EncryptString(meetingDomain.MeetingTime), meetingDomain.ForumId, meetingDomain.CreatedBy, meetingDomain.UpdatedBy, meetingDomain.MeetingChecker, SendToChecker, meetingDomain.MeetingNumber, meetingDomain.MeetingType, meetingDomain.Header, meetingDomain.Convenor);
            return meetingDomain;
        }
        /// <summary>
        /// Update Meeting Details
        /// </summary>
        /// <param name="meetingDomain">Class object specifying meetingDomain</param>
        /// <returns></returns>
        public bool Update(MeetingDomain meetingDomain, string SendToChecker)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateMeetingDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", meetingDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_MeetingDate", Encryptor.EncryptString(meetingDomain.MeetingDate));
            cmd.Parameters.AddWithValue("p_MeetingTime", Encryptor.EncryptString(meetingDomain.MeetingTime));
            cmd.Parameters.AddWithValue("p_MeetingVenue", Encryptor.EncryptString(meetingDomain.MeetingVenue));
            cmd.Parameters.AddWithValue("p_ForumId", meetingDomain.ForumId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", meetingDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_MeetingChecker", meetingDomain.MeetingChecker);
            cmd.Parameters.AddWithValue("p_SendToChecker", SendToChecker);
            cmd.Parameters.AddWithValue("p_MeetingNumber", meetingDomain.MeetingNumber);
            cmd.Parameters.AddWithValue("p_MeetingType", meetingDomain.MeetingType);
            cmd.Parameters.AddWithValue("p_Header", meetingDomain.Header);
            cmd.Parameters.AddWithValue("p_Convenor", meetingDomain.Convenor);
            cmd.Parameters.AddWithValue("p_ConvenorName", Encryptor.EncryptString(meetingDomain.ConvenorName));
            cmd.Parameters.AddWithValue("p_EditOrCancelReason", meetingDomain.EditOrCancelReason);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateMeetingDetailsById", meetingDomain.MeetingId, Encryptor.EncryptString(meetingDomain.MeetingDate), Encryptor.EncryptString(meetingDomain.MeetingTime), Encryptor.EncryptString(meetingDomain.MeetingVenue), meetingDomain.ForumId, meetingDomain.UpdatedBy, meetingDomain.MeetingChecker, SendToChecker, meetingDomain.MeetingNumber, meetingDomain.MeetingType, meetingDomain.Header, meetingDomain.Convenor)) > 0;
        }
        /// <summary>
        /// Delete Meeting Details
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public bool Delete(Guid MeetingId, string DeleteReason)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteMeetingDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_EditOrCancelReason", DeleteReason);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteMeetingDetailsById", MeetingId)) > 0;
        }

        /// <summary>
        /// Delete Seleted meetings
        /// </summary>
        /// <param name="MeetingId">string specifying MeetingId</param>
        /// <returns></returns>
        public bool DeleteSelectedMeeting(string MeetingId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedMeetingById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingIds", MeetingId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSelectedMeetingById", MeetingId)) > 0;
        }



        /// <summary>
        /// Change Meeting Status
        /// </summary>
        /// <param name="Status">string specifying Status</param>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public bool UpdateMeetingStatus(string Status, Guid MeetingId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateMeetingStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_Status", Status);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //    return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateMeetingStatus", MeetingId, Status)) > 0;
        }

        /// <summary>
        /// Guid specifying UserId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public IList<MeetingDomain> GeApprovedtMeetingByUser(Guid UserId)
        {
            IList<MeetingDomain> objMeetingDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllApprovedMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllApprovedMeeting", UserId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// Get meeting Venues
        /// </summary>
        /// <returns></returns>
        public IList<string> GetMeetingVenue()
        {
            IList<string> objMeetingDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingsVenue", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingsVenue", EntityId))
            {
                objMeetingDomains = ToMeetingVenues(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        #endregion

        /// <summary>
        /// Insert Email history Details
        /// </summary>
        /// <param name="meetingDomain">Class object specifying meetingDomain</param>
        /// <returns></returns>
        public EmailHistroyDomain InsertEmailHistory(EmailHistroyDomain objEmailHistroyDomain)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertEmailHistory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Subject", objEmailHistroyDomain.Subject);
            cmd.Parameters.AddWithValue("p_Body", objEmailHistroyDomain.Body);
            cmd.Parameters.AddWithValue("p_Sender", objEmailHistroyDomain.Sender);
            cmd.Parameters.AddWithValue("p_Receiver", objEmailHistroyDomain.Receiver);
            cmd.Parameters.AddWithValue("p_CreatedBy", objEmailHistroyDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_CC", objEmailHistroyDomain.CC);
            cmd.Parameters.AddWithValue("p_MeetingId", objEmailHistroyDomain.MeetingId);
            objEmailHistroyDomain.EmailHistoryId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            //            objEmailHistroyDomain.EmailHistoryId = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertEmailHistory", objEmailHistroyDomain.Subject, objEmailHistroyDomain.Body, objEmailHistroyDomain.Sender, objEmailHistroyDomain.Receiver, objEmailHistroyDomain.CreatedBy, objEmailHistroyDomain.CC, objEmailHistroyDomain.MeetingId));
            return objEmailHistroyDomain;
        }

        /// <summary>
        /// Published meeting and agenda by forum id
        /// </summary>
        /// <param name="ForumId"></param>
        /// <returns></returns>
        public IList<MeetingDomain> GetPublishedMeetingAndAgendaByForumId(Guid ForumId)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPublishedMeetingAndAgendaByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();




            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingDetailsByForumId", ForumId))
            {
                objMeetingDomains = ToMeetingDomainsWithEntity(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }

        /// <summary>
        /// get all meeting shadow
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingShadowReportByYear(DateTime startDate, DateTime endDate)
        {
            IList<MeetingDomain> objMeetingDomains = null;

            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingShadowReportByYear", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_startDate", startDate);
            cmd.Parameters.AddWithValue("p_endDate", endDate);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            {
            }
            objMeetingDomains = ToMeetingDomains(dataReader);
            conn.Close();
            return objMeetingDomains;
        }

        public bool UpdateMeetingRetention(MeetingRetentionDomain objMeeting)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UpdateMeetingRetention", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objMeeting.Id);
            cmd.Parameters.AddWithValue("p_Value", objMeeting.Value);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objMeeting.UpdatedBy);
            cmd.Parameters.AddWithValue("p_MeetingDate", Encryptor.EncryptString(objMeeting.MeetingDate));
            cmd.Parameters.AddWithValue("p_MeetingTime", Encryptor.EncryptString(objMeeting.MeetingTime));
            conn.Open();
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }

        public IList<MeetingDomain> GetAllDepartmentMeeting()
        {
            IList<MeetingDomain> objMeetingDomain = null;
            string constr = ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllDepartmentMeetings", con);
            con.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            {
                objMeetingDomain = ToMeetingDomainsWithEntity(dataReader);
            }
            con.Close();
            return objMeetingDomain;
        }

    }
}

