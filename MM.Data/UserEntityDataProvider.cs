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
    public class UserEntityDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile UserEntityDataProvider _instance;
        private static object _synRoot = new Object();

        private UserEntityDataProvider()
        {
        }

        public static UserEntityDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserEntityDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill user entity details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UserEntityDomain ToUserEntityDomain(IDataReader dr)
        {
            string EncryptionKey = "";
            dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "EncryptionKey");
            if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dr["EncryptionKey"]));
            }
            return new UserEntityDomain()
            {
                UserEntityId = Null.SetNullGuid(dr["UserEntityId"]),
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                UserId = Null.SetNullGuid(dr["UserId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),
                EntityLogo =  Encryptor.DecryptString(Null.SetNullString(dr["EntityLogo"])),
                EncryptionKey = EncryptionKey
            };
        }

        /// <summary>
        /// Get All User Entities
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UserEntityDomain> ToUserEntityDoamins(IDataReader dr)
        {
            IList<UserEntityDomain> userEntityDoamins = new List<UserEntityDomain>();
            while (dr.Read())
            {
                userEntityDoamins.Add(ToUserEntityDomain(dr));
            }
            return userEntityDoamins;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Entity Details By UserId
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public IList<UserEntityDomain> GetEntityListByUserId(Guid UserId)
        {
            IList<UserEntityDomain> userEntityDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEntityByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //   Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetEntityByUserId", UserId))
            {
                userEntityDomain = ToUserEntityDoamins(dataReader);
            }
            conn.Close();
            return userEntityDomain;
        }

        public IList<UserEntityDomain> GetEntityListByUserIdForDepartment(Guid UserId)
        {
            IList<UserEntityDomain> userEntityDomain = null;
              Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEntityByUserIdForDepartemnt", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetEntityByUserIdForDepartemnt", UserId, EntityId))
            {
                userEntityDomain = ToUserEntityDoamins(dataReader);
            }
            conn.Close();
            return userEntityDomain;
        }
        
        /// <summary>
        /// Get data for wcf services
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="StartTime">DateTime specifying StartTime</param>
        /// <param name="EndTime">DateTime specifying EndTime</param>
        /// <returns></returns>
        public DataSet GetDetailsForWcf(Guid UserId, DateTime StartTime, DateTime EndTime, string Password)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_AllDataForWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("p_EndTime", EndTime);
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(Password));
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_AllDataForWcf", UserId, StartTime, EndTime, Encryptor.EncryptString(Password));
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Get data with entity for wcf services
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="StartTime">DateTime specifying StartTime</param>
        /// <param name="EndTime">DateTime specifying EndTime</param>
        /// <returns></returns>
        public DataSet GetDetailsForWcfByEntiy(Guid UserId, DateTime StartTime, DateTime EndTime, string UdId, Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_AllDataForWcfNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("p_EndTime", EndTime);
            cmd.Parameters.AddWithValue("p_UdId", UdId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_AllDataForWcfNew", UserId, StartTime, EndTime, UdId, EntityId);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Get data for wcf services
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="StartTime">string specifying StartTime</param>
        /// <param name="EndTime">string specifying EndTime</param>
        /// <returns></returns>
        public DataSet GetDetailsForWcfData(Guid UserId, string Password)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_AllDataForWcfData", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(Password));
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            // DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_AllDataForWcfData", UserId, Encryptor.EncryptString(Password));
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Get data with entity for wcf services
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="UdId">string specifying UdId</param>
        /// <param name="EndTime">Guid specifying EntityId</param>
        /// <returns></returns>
        public DataSet GetDetailsForWcfDataByEntity(Guid UserId, string UdId, Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_AllDataForWcfDataNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_UdId", UdId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_AllDataForWcfDataNew", UserId, UdId, EntityId);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Get Events for event calender
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventsDetailsWcf()
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEventsForWCF", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetEventsForWCF");
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Get Events for event calender by UserId
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventsDetailsByUserIdWcf(Guid UserId, DateTime startTime)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEventsByUser1WCF", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_startTime", startTime);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);


            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetEventsByUser1WCF", UserId, startTime);
            conn.Close();
            return ds;
        }

               
        /// <summary>
        /// Insert Event details
        /// </summary>
        /// <param name="nameOfVisitor"></param>
        /// <param name="designtionOfVisitor"></param>
        /// <param name="nameOfVisitorOrganisation"></param>
        /// <param name="appointment"></param>
        /// <param name="nameDesignation"></param>
        /// <param name="Attachment"></param>
        /// <param name="information"></param>
        /// <param name="start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public Int32 InsertEvent(string nameOfVisitor, string designtionOfVisitor, string nameOfVisitorOrganisation, string appointment, string nameDesignation, string Attachment, string information, DateTime start, DateTime End, Guid UserId, string eventDay)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("sp_insertScheduleEventWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor", nameOfVisitor);
            cmd.Parameters.AddWithValue("p_Designation_Of_Visitor", designtionOfVisitor);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor_Organisation", nameOfVisitorOrganisation);
            cmd.Parameters.AddWithValue("p_Appointment", appointment);
            cmd.Parameters.AddWithValue("p_Name_Designation", nameDesignation);
            cmd.Parameters.AddWithValue("p_Upload_Photo", "");
            cmd.Parameters.AddWithValue("p_Attachment", Attachment);
            cmd.Parameters.AddWithValue("p_Information", information);
            cmd.Parameters.AddWithValue("p_Event_Start", start);
            cmd.Parameters.AddWithValue("p_Event_End", End);
            cmd.Parameters.AddWithValue("p_CreatedBy", UserId);
            cmd.Parameters.AddWithValue("p_EventDay", eventDay);
            int id= Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return id;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "sp_insertScheduleEventWcf", nameOfVisitor, designtionOfVisitor, nameOfVisitorOrganisation, appointment,
            //    nameDesignation, "", Attachment, information, start, End, UserId, eventDay));
        }

        /// <summary>
        /// Update users event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nameOfVisitor"></param>
        /// <param name="designtionOfVisitor"></param>
        /// <param name="nameOfVisitorOrganisation"></param>
        /// <param name="appointment"></param>
        /// <param name="nameDesignation"></param>
        /// <param name="uploadPhoto"></param>
        /// <param name="attachments"></param>
        /// <param name="information"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="IsActive"></param>
        /// <param name="eventDay"></param>
        /// <returns></returns>
        public bool UpdateEvent(Int32 id, String nameOfVisitor, String designtionOfVisitor, String nameOfVisitorOrganisation, String appointment, String nameDesignation, String uploadPhoto, String attachments, String information, string start, string end, bool IsActive, string eventDay)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("sp_updateSchedulerEventWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Event_Id", id);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor", nameOfVisitor);
            cmd.Parameters.AddWithValue("p_Designation_Of_Visitor", designtionOfVisitor);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor_Organisation", nameOfVisitorOrganisation);
            cmd.Parameters.AddWithValue("p_Appointment", appointment);
            cmd.Parameters.AddWithValue("p_Name_Designation", nameDesignation);
            cmd.Parameters.AddWithValue("p_Upload_Photo", uploadPhoto);
            cmd.Parameters.AddWithValue("p_Attachment", attachments);
            cmd.Parameters.AddWithValue("p_Information", information);
            cmd.Parameters.AddWithValue("p_Event_Start", start);
            cmd.Parameters.AddWithValue("p_Event_End", end);
            cmd.Parameters.AddWithValue("p_IsActive", IsActive);
            cmd.Parameters.AddWithValue("p_EventDay", eventDay);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "sp_updateSchedulerEventWcf", id, nameOfVisitor, designtionOfVisitor, nameOfVisitorOrganisation, appointment, nameDesignation, uploadPhoto, attachments, information, start, end, IsActive, eventDay))
            //> 0;
        }

        /// <summary>
        /// Delete user created event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataSet DeleteEvent(Int32 id)
        {
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "sp_DeleteSchedulerEventWcf", id))
            //> 0;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("sp_DeleteSchedulerEventWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Event_Id", id);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();

            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "sp_DeleteSchedulerEventWcf", id);
            return ds;
            
        }

        /// <summary>
        /// Update Mom of Event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MOM"></param>
        /// <returns></returns>
        public bool UpdateEventMOM(Int32 id, string MOM)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("sp_UpdateEventMOMWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Event_Id", id);
            cmd.Parameters.AddWithValue("p_MOM", MOM);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "sp_UpdateEventMOMWcf", id, MOM))
            //> 0;
        }

        /// <summary>
        /// Get User delete staus by Id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public DataSet GetAuthoizedUser(Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_AuthorizeUserByIdWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();

            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_AuthorizeUserByIdWcf", UserId);
            return ds;
        }



        /// <summary>
        /// Insert session details
        /// </summary>
        /// <param name="sessionId">string specifiying sessionId</param>
        /// <param name="userId">guid specifying userId</param>
        public int InsertSession(string sessionId, Guid userId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertSessions", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_SessionId", sessionId);
            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //return (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertSessions",  sessionId,userId);
        }


        /// <summary>
        /// update session details
        /// </summary>
        /// <param name="sessionId">string specifiying sessionId</param>
        /// <param name="userId">guid specifying userId</param>
        public void UpdateSession(string sessionId, Guid userId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateSession", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", userId);
            cmd.Parameters.AddWithValue("p_SessionId", sessionId);

            cmd.ExecuteNonQuery();
            conn.Close();
            //SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateSession", sessionId, userId);
        }

        /// <summary>
        /// Get Published Agenda By MeetingId
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="UdId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        public DataSet GetPublishedAgendaByMeetingId(Guid MeetingId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPublishedAgendaByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            conn.Close();
            return ds;
        }
        #endregion
    }
}
