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
    public class NoticeDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile NoticeDataProvider _instance;
        private static object _synRoot = new Object();

        private NoticeDataProvider()
        {
        }

        public static NoticeDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new NoticeDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Notice details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private NoticeDomain ToNoticeDomain(IDataReader dr)
        {
            return new NoticeDomain
            {
                NoticeId = Null.SetNullGuid(dr["Noticeid"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                NoticeMessage = Null.SetNullString(dr["NoticeMessage"]),
                NoticeChecker = Null.SetNullGuid(dr["NoticeChecker"])
            };
        }

        /// <summary>
        /// Fill Notice details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<NoticeDomain> ToNoticeDomains(IDataReader dr)
        {
            IList<NoticeDomain> objNotice = new List<NoticeDomain>();
            while (dr.Read())
            {
                objNotice.Add(ToNoticeDomain(dr));
            }
            return objNotice;

        }

        /// <summary>
        /// Fill Notice details with meeting
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<NoticeDomain> ToNoticeDomainsWithMeeting(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            IList<NoticeDomain> noticeDomains = new List<NoticeDomain>();
            NoticeDomain objNotice = null;

            while (dr.Read())
            {
                objNotice = new NoticeDomain();
                objNotice = ToNoticeDomain(dr);
                objNotice.MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"]));
                objNotice.MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]));
                objNotice.MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"]));
                objNotice.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objNotice.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objNotice.FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"]));
                objNotice.LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"]));
                objNotice.IsApproved = Null.SetNullString(dr["IsApproved"]);

                if(clsmain.checkColumExist("MeetingNumber", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingNumber");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objNotice.MeetingNumber = Null.SetNullString(dr["MeetingNumber"]);
                }
                if (clsmain.checkColumExist("ForumName", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "ForumName");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objNotice.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
                }
                noticeDomains.Add(objNotice);
            }
            return noticeDomains;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Notice details by id
        /// </summary>
        /// <param name="NoticeId">Guid sepcifing NoticeId</param>
        /// <returns></returns>
        public NoticeDomain Get(Guid NoticeId)
        {
            NoticeDomain noticeDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetNoticeById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_NoticeId", NoticeId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetNoticeById", NoticeId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                noticeDomain = ToNoticeDomain(dataReader);
            }
            conn.Close();
            return noticeDomain;
        }

        /// <summary>
        /// Get all notice details
        /// </summary>
        /// <returns></returns>
        public IList<NoticeDomain> Get()
        {
            IList<NoticeDomain> noticeDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllNotceDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllNotceDetails"))
            {
                noticeDomains = ToNoticeDomains(dataReader);
            }
            conn.Close();
            return noticeDomains;
        }

        /// <summary>
        /// Get notice details with meeting
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public IList<NoticeDomain> GetNoticeWithMeeting(Guid UserId)
        {
            IList<NoticeDomain> noticeDomains = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetNoticeWithMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetNoticeWithMeeting", UserId))
            {
                noticeDomains = ToNoticeDomainsWithMeeting(dataReader);
            }
            conn.Close();
            return noticeDomains;

        }

        /// <summary>
        /// Get notce by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public NoticeDomain GetNoticeByMeeting(Guid MeetingId)
        {
            NoticeDomain noticeDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetNoticeByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetNoticeByMeetingId", MeetingId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                noticeDomain = ToNoticeDomain(dataReader);
            }
            conn.Close();
            return noticeDomain;
        }
        /// <summary>
        /// Get notice details with meeting
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public IList<NoticeDomain> GetUnapprovedNotice(Guid UserId)
        {
             IList<NoticeDomain> noticeDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUnApprovedNotice", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUnApprovedNotice", UserId))
            {
                noticeDomains = ToNoticeDomainsWithMeeting(dataReader);
            }
            conn.Close();
            return noticeDomains;

        }
        
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Notice details
        /// </summary>
        /// <param name="objNotice">Class object specifying objNotice</param>
        /// <returns></returns>
        public NoticeDomain Insert(NoticeDomain objNotice)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertNotice", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", objNotice.MeetingId);
            cmd.Parameters.AddWithValue("p_NoticeMessage", objNotice.NoticeMessage);
            cmd.Parameters.AddWithValue("p_NoticeChecker", objNotice.NoticeChecker);
            cmd.Parameters.AddWithValue("p_CreatedBy", objNotice.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objNotice.UpdatedBy);
            cmd.Parameters.AddWithValue("p_SendToChecker", objNotice.IsApproved);
            objNotice.NoticeId = (Guid)(cmd.ExecuteScalar());
            //objNotice.NoticeId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertNotice", objNotice.MeetingId, objNotice.NoticeMessage, objNotice.NoticeChecker, objNotice.CreatedBy, objNotice.UpdatedBy, objNotice.IsApproved);
            conn.Close();
            return objNotice;
        }

        /// <summary>
        /// Update Notice details
        /// </summary>
        /// <param name="objNotice">Class object specifying objNotice</param>
        /// <returns></returns>
        public bool Update(NoticeDomain objNotice)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateNotice", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_NoticeId", objNotice.NoticeId);
            cmd.Parameters.AddWithValue("p_MeetingId", objNotice.MeetingId);
            cmd.Parameters.AddWithValue("p_NoticeMessage", objNotice.NoticeMessage);
            cmd.Parameters.AddWithValue("p_NoticeChecker", objNotice.NoticeChecker);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objNotice.UpdatedBy);
            cmd.Parameters.AddWithValue("p_SendToChecker", objNotice.IsApproved);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateNotice", objNotice.NoticeId, objNotice.MeetingId, objNotice.NoticeMessage, objNotice.NoticeChecker, objNotice.UpdatedBy, objNotice.IsApproved)) > 0;
        }

        /// <summary>
        /// Delete notice by id
        /// </summary>
        /// <param name="NoticeId">Guid specifying NoticeId</param>
        /// <returns></returns>
        public bool Delete(Guid NoticeId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteNoticeById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_NoticeId", NoticeId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteNoticeById", NoticeId)) > 0;
        }
        #endregion

    }
}
