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
    public class VisitoreDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile VisitoreDataProvider _instance;
        private static object synRoot = new Object();

        private VisitoreDataProvider()
        {

        }

        public static VisitoreDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new VisitoreDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Visitor details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private VisitorDomain ToVisitorDomain(IDataReader dr)
        {
            return new VisitorDomain
            {
                VisiterId = Null.SetNullInteger64(dr["VisiterId"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                Designation_Of_Visitor = Null.SetNullString(dr["Designation_Of_Visitor"]),
                IsActive = Null.SetNullBoolean(dr["IsActive"]),
                LinkedInUrl = Null.SetNullString(dr["LinkedInUrl"]),
                Name_Of_Visitor_Organisation = Null.SetNullString(dr["Name_Of_Visitor_Organisation"]),
                Upload_Photo = Null.SetNullString(dr["Upload_Photo"]),
                Name_Of_Visitor = Null.SetNullString(dr["Name_Of_Visitor"]),
                Email = Null.SetNullString(dr["Email"]),
                Phone = Null.SetNullString(dr["Phone"])
            };
        }

        /// <summary>
        /// Fill user Visitor details
        /// </summary>
        /// <param name="dr">IDataReader specifying dr</param>
        /// <returns></returns>
        private IList<VisitorDomain> ToVisitorDomains(IDataReader dr)
        {
            IList<VisitorDomain> objVisitorDomain = new List<VisitorDomain>();
            while (dr.Read())
            {
                objVisitorDomain.Add(ToVisitorDomain(dr));
            }
            return objVisitorDomain;
        }

        
        #endregion

        #region "Get"
        /// <summary>
        /// Get UserForum by id
        /// </summary>
        /// <param name="UserForumId">Class object specifying UserForumId</param>
        /// <returns></returns>
        public IList<VisitorDomain> Get(Guid UserForumId)
        {
            IList<VisitorDomain> objUserForum = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetVisitorById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CreatedBy", UserForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetVisitorById", UserForumId))
            {
                objUserForum = ToVisitorDomains(dataReader);
            }
            conn.Close();
            return objUserForum;
        }


        /// <summary>
        /// Get UserForum by user id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public VisitorDomain GetVisterById(Int64 VisitorId)
        {
           VisitorDomain objVisitor = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetVisitorByVisitorId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_VisitorId", VisitorId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetVisitorByVisitorId", VisitorId))
           {
               if (!dataReader.Read())
               {
                   return null;
               }
                objVisitor = ToVisitorDomain(dataReader);
            }
            conn.Close();
            return objVisitor;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Visitor details
        /// </summary>
        /// <param name="UserId">string specifing userIds</param>
        /// <param name="Accessrights">string specifying Accessrights </param>
        /// <param name="CreatedBy">Guid specifying created by </param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <returns></returns>
        public VisitorDomain Insert(VisitorDomain objVisitor)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertVisitors", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Designation_Of_Visitor", objVisitor.Designation_Of_Visitor);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor_Organisation", objVisitor.Name_Of_Visitor_Organisation);
            cmd.Parameters.AddWithValue("p_Upload_Photo", objVisitor.Upload_Photo);
            cmd.Parameters.AddWithValue("p_LinkedInUrl", objVisitor.LinkedInUrl);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor", objVisitor.Name_Of_Visitor);
            cmd.Parameters.AddWithValue("p_CreatedBy", objVisitor.CreatedBy);
            cmd.Parameters.AddWithValue("p_Email", objVisitor.Email);
            cmd.Parameters.AddWithValue("p_Phone", objVisitor.Phone);
            objVisitor.VisiterId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            //objVisitor.VisiterId = (Int32)
            //SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertVisitors", objVisitor.Designation_Of_Visitor, objVisitor.Name_Of_Visitor_Organisation, objVisitor.Upload_Photo, objVisitor.LinkedInUrl, objVisitor.Name_Of_Visitor, objVisitor.CreatedBy, objVisitor.Email, objVisitor.Phone);
            return objVisitor;
        }

        /// <summary>
        /// Insert attendees details
        /// </summary>
        /// <param name="EventId">string specifing EventId</param>
        /// <param name="Attendees">string specifying Attendees </param>
        public bool AddAttendees(Int64 EventId, string Attendees)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("InsertAttendees", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EventId", EventId);
            cmd.Parameters.AddWithValue("p_Attendees", Attendees);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "InsertAttendees", EventId, Attendees)) > 0;
        }


        /// <summary>
        /// Update attendees details
        /// </summary>
        /// <param name="EventId">string specifing EventId</param>
        /// <param name="InsertAttendees">string specifying InsertAttendees </param>
        /// <param name="DeleteAttendees">string specifying DeleteAttendees </param>
        public bool UpdateAttendees(Int64 EventId, string InsertAttendees, string DeleteAttendees)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UpdateAttendees", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_InsertAttendees", InsertAttendees);
            cmd.Parameters.AddWithValue("p_EventId", EventId);
            cmd.Parameters.AddWithValue("p_DeleteAttendees", DeleteAttendees);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "UpdateAttendees", InsertAttendees, DeleteAttendees, EventId)) > 0;
        }

        /// <summary>
        /// Update visitor Details
        /// </summary>
        /// <param name="UserId">string specifing userIds</param>
        /// <param name="Accessrights">string specifying Accessrights </param>
        /// <param name="CreatedBy">Guid specifying created by </param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="InsertUserIds">string specifying InsertUserIds</param>
        /// <param name="InsertForumAccess">string specifying insertForumAccess</param>
        /// <param name="DeleteForums">string specifying DeleteForums</param>
        /// <returns></returns>
        public bool Update(VisitorDomain objVisitor)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateVisitor", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Designation_Of_Visitor", objVisitor.Designation_Of_Visitor);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor_Organisation", objVisitor.Name_Of_Visitor_Organisation);
            cmd.Parameters.AddWithValue("p_Upload_Photo", objVisitor.Upload_Photo);
            cmd.Parameters.AddWithValue("p_LinkedInUrl", objVisitor.LinkedInUrl);
            cmd.Parameters.AddWithValue("p_Name_Of_Visitor", objVisitor.Name_Of_Visitor);
            cmd.Parameters.AddWithValue("p_CreatedBy", objVisitor.CreatedBy);
            cmd.Parameters.AddWithValue("p_VisiterId", objVisitor.VisiterId);
            cmd.Parameters.AddWithValue("p_Email", objVisitor.Email);
            cmd.Parameters.AddWithValue("p_Phone", objVisitor.Phone);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateVisitor", objVisitor.Designation_Of_Visitor, objVisitor.Name_Of_Visitor_Organisation, objVisitor.Upload_Photo, objVisitor.LinkedInUrl, objVisitor.Name_Of_Visitor, objVisitor.CreatedBy, objVisitor.VisiterId, objVisitor.Email, objVisitor.Phone)) > 0;
        }

        /// <summary>
        /// Delete forum access by userId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool Delete(Int64 VisitorId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteVisiterById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_VisitorId", VisitorId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteVisiterById", VisitorId)) > 0;
        }
        #endregion
    }
}
