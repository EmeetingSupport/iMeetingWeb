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
    public class ApprovalDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile ApprovalDataProvider _instance;
        private static object _synRoot = new Object();

        private ApprovalDataProvider()
        {
        }

        public static ApprovalDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApprovalDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Unapproved Entity details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ApprovalDomain ToApprovalDomain(IDataReader dr)
        {
            return new ApprovalDomain()
            {
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),
                UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"])),
                FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])),
                IsApproved = Null.SetNullString(dr["IsApproved"]),
                UpdatedBy = Null.SetNullGuid(Null.SetNullString(dr["UpdatedBy"]))
            };
        }

        /// <summary>
        /// Fill Unapproved User details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ApprovalDomain ToApprovalDomainUser(IDataReader dr)
        {
            return new ApprovalDomain()
            {
                UserId = Null.SetNullGuid(dr["UserId"]),
                Photograph = Encryptor.DecryptString(Null.SetNullString(dr["Photograph"])),
                Designation = Encryptor.DecryptString(Null.SetNullString(dr["Designation"])),
                CreaterFirstName = Encryptor.DecryptString(Null.SetNullString(dr["CreaterFirstName"])),

                CreaterLastName = Encryptor.DecryptString(Null.SetNullString(dr["CreaterLastName"])),

                UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"])),
                FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])),
                IsApproved = Null.SetNullString(dr["IsApproved"]),
                UpdatedBy = Null.SetNullGuid(Null.SetNullString(dr["UpdatedBy"]))
            };
        }



        /// <summary>
        /// Get All Unapproved Entities
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ApprovalDomain> ToApprovalDomains(IDataReader dr)
        {
            IList<ApprovalDomain> ObjApprovalDoamins = new List<ApprovalDomain>();
            while (dr.Read())
            {
                ObjApprovalDoamins.Add(ToApprovalDomain(dr));
            }
            return ObjApprovalDoamins;
        }

        /// <summary>
        /// Get All Unapproved Forums
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ApprovalDomain> ToApprovalDomainsForums(IDataReader dr)
        {
            IList<ApprovalDomain> objApprovalDoamins = new List<ApprovalDomain>();
            ApprovalDomain objApprov = new ApprovalDomain();
            while (dr.Read())
            {
                objApprov = ToApprovalDomain(dr);               
                objApprov.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objApprov.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));
                
                objApprovalDoamins.Add(objApprov);
            }
            return objApprovalDoamins;
        }


        /// <summary>
        /// Get All Unapproved Meeting
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ApprovalDomain> ToApprovalDomainsMeetings(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            IList<ApprovalDomain> objApprovalDoamins = new List<ApprovalDomain>();
            ApprovalDomain objApprov = new ApprovalDomain();
            while (dr.Read())
            {
                objApprov = ToApprovalDomain(dr);
                objApprov.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objApprov.ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]));

                objApprov.MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"]));
                objApprov.MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]));
                objApprov.MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"]));
                objApprov.MeetingId = Null.SetNullGuid(dr["MeetingId"]);

                if(clsmain.checkColumExist("MeetingNumber", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingNumber");
                // if (dr.GetSchemaTable().DefaultView.Count > 0)
                 {
                     objApprov.MeetingNumber = Null.SetNullString(dr["MeetingNumber"]); 
                 }
                if (clsmain.checkColumExist("Agenda_UpdatedBy", dr))
                 //   dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Agenda_UpdatedBy");
                 //if (dr.GetSchemaTable().DefaultView.Count > 0)
                 {
                     objApprov.UpdatedBy = Null.SetNullGuid(dr["Agenda_UpdatedBy"]);
                 }
                //if (dr.GetSchemaTable().Columns["Agenda_UpdatedBy"] != null)
                //{
                //    objApprov.UpdatedBy = Null.SetNullGuid(dr["Agenda_UpdatedBy"]); 
                //}
               
                objApprovalDoamins.Add(objApprov);
            }
            return objApprovalDoamins;
        }

        /// <summary>
        /// Get All Unapproved Users
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<ApprovalDomain> ToApprovalDomainsUser(IDataReader dr)
        {
            IList<ApprovalDomain> ObjApprovalDoamins = new List<ApprovalDomain>();
            while (dr.Read())
            {
                ObjApprovalDoamins.Add(ToApprovalDomainUser(dr));
            }
            return ObjApprovalDoamins;
        }

        #endregion

        #region "Get"
        /// <summary>
        /// Get Unapproved Entity Details
        /// </summary>
        /// <param name="CheckerId">Guid sepcifing CheckerId</param>
        /// <returns></returns>
        public IList<ApprovalDomain> GetUnApprovedEntity(Guid CheckerId)
        {
            IList<ApprovalDomain> objApprovalDomain = new List<ApprovalDomain>();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUnApprovedEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CheckerId", CheckerId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUnApprovedEntity", CheckerId))
            {
                objApprovalDomain = ToApprovalDomains(dataReader);
            }
            conn.Close();
            return objApprovalDomain;
        }

        /// <summary>
        /// Get all unapproved Forums
        /// </summary>
        /// <param name="CheckerId">Guid sepcifing CheckerId</param>
        /// <returns></returns>
        public IList<ApprovalDomain> GetUnApprovedForums(Guid CheckerId)
        {
            IList<ApprovalDomain> objApprovalDomain = new List<ApprovalDomain>();


            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUnApprovedForum", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CheckerId", CheckerId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUnApprovedForum", CheckerId))
            {
                objApprovalDomain = ToApprovalDomainsForums(dataReader);              
            }
            conn.Close();
            return objApprovalDomain;
        }

        /// <summary>
        /// Get all unapproved meetings
        /// </summary>
        /// <param name="CheckerId">Guid sepcifing CheckerId</param>
        /// <returns></returns>
        public IList<ApprovalDomain> GetUnApprovedMeetings(Guid CheckerId)
        {
            IList<ApprovalDomain> objApprovalDomain = new List<ApprovalDomain>();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUnapprovedMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CheckerId", CheckerId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUnapprovedMeeting", CheckerId))
            {
                objApprovalDomain = ToApprovalDomainsMeetings(dataReader);
            }
            conn.Close();
            return objApprovalDomain;
        }

        /// <summary>
        /// Get All Unapproved User
        /// </summary>
        /// <param name="CheckerId">Guid sepcifing CheckerId</param>
        /// <returns></returns>
        public IList<ApprovalDomain> GetUnApprovedUser(Guid CheckerId)
        {
            IList<ApprovalDomain> objApprovalDomain = new List<ApprovalDomain>();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUnApporvedUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CheckerId", CheckerId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUnApporvedUser", CheckerId))
            {
                objApprovalDomain = ToApprovalDomainsUser(dataReader);
            }
            conn.Close();
            return objApprovalDomain;
        }


        /// <summary>
        /// Get All unapproved meeting for agenda 
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public IList<ApprovalDomain> GetUnApprovedMeetingForAgenda(Guid UserId)
        {
            IList<ApprovalDomain> objMeetingDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingIdOfUnapprovedAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingIdOfUnapprovedAgenda", UserId))
            {
                objMeetingDomains = ToApprovalDomainsMeetings(dataReader);
            }
            conn.Close();
            return objMeetingDomains;
        }
        #endregion

        #region "Modify"

        /// <summary>
        /// Approve or reject entity, forum, Meeting
        /// </summary>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <param name="TransationId">Guid sepcifing TransationId</param>
        /// <param name="Action">string sepcifing TransationId</param>
        /// <param name="Transaction">string sepcifing TransationId</param>
        /// <param name="IsEnabledOnIpad">bool sepcifing IsEnabledOnIpad</param>
        /// <returns></returns>
        public bool UpdateStaus(Guid UserId, Guid TransationId, string Action, string Transaction, bool IsEnabledOnIpad, string DeclineReason)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_ApproveReject", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_Transaction", Transaction);
            cmd.Parameters.AddWithValue("p_Action", Action);
            cmd.Parameters.AddWithValue("p_Id", TransationId);
            cmd.Parameters.AddWithValue("p_Enable", IsEnabledOnIpad);
            cmd.Parameters.AddWithValue("p_DeclineReson", DeclineReason);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_ApproveReject", UserId, Transaction, Action, TransationId, IsEnabledOnIpad, DeclineReason)) > 0;
        }
        #endregion
    }
}
