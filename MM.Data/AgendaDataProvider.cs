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
    public class AgendaDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile AgendaDataProvider _instance;
        private static object synRoot = new Object();

        private AgendaDataProvider()
        {

        }

        public static AgendaDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AgendaDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"


        /// <summary>
        /// Fill Agenda Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private AgendaDomain ToAgendaDomain(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string DelAgenda = "";
            if(clsmain.checkColumExist("DeletedAgenda", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "DeletedAgenda");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                DelAgenda = Null.SetNullString(dr["DeletedAgenda"]);
            }

            string Classification = "";
            if (clsmain.checkColumExist("Classification", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Classification");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                Classification = Null.SetNullString(dr["Classification"]);
            }

            string Presenter = "";
            if (clsmain.checkColumExist("Presenter", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Presenter");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                Presenter = Null.SetNullString(dr["Presenter"]);
            }

            string SerialNumber = "";
            if (clsmain.checkColumExist("SerialNumber", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialNumber");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                SerialNumber = Null.SetNullString(dr["SerialNumber"]);
            }

            string SerialTitle = "";
            if (clsmain.checkColumExist("SerialTitle", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialTitle");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                SerialTitle = Null.SetNullString(dr["SerialTitle"]);
            }


            string SerialText = "";
            if (clsmain.checkColumExist("SerialText", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialText");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                SerialText = Null.SetNullString(dr["SerialText"]);
            }


            return new AgendaDomain()
            {
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),
                AgendaName = Null.SetNullString(dr["AgendaName"]),
                AgendaNote = Null.SetNullString(dr["AgendaNote"]),
                ParentAgendaId = Null.SetNullGuid(dr["ParentAgendaId"]),
                IsPublished = Null.SetNullBoolean(dr["IsPublished"]),
                PublishedBy = Null.SetNullGuid(dr["PublishedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdateOn = Null.SetNullDateTime(dr["UpdateOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                AgendaOrder = Null.SetNullInteger(dr["AgendaOrder"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UploadedAgendaNote = Null.SetNullString(dr["UploadedAgendaNote"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                DeletedAgenda = DelAgenda,
                Classification = Classification,
                Presenter = Presenter,
                SerialNumber = SerialNumber,
                SerialText = SerialText,
                SerialTitle = SerialTitle
            };
        }

        /// <summary>
        /// Get Agenda domain details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AgendaDomain> ToAgendaDomains(IDataReader dr)
        {
            IList<AgendaDomain> objAgendaDomain = new List<AgendaDomain>();
            while (dr.Read())
            {
                objAgendaDomain.Add(ToAgendaDomain(dr));
            }
            return objAgendaDomain;
        }

        /// <summary>
        /// Get Agenda titles
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AgendaDomain> ToAgendaTitles(IDataReader dr)
        {
            IList<AgendaDomain> objAgendaDomain = new List<AgendaDomain>();
            AgendaDomain objAgenda = null;
            while (dr.Read())
            {
                objAgenda = new AgendaDomain();
                objAgenda.AgendaName = Null.SetNullString(dr["AgendaName"]);
                objAgenda.AgendaId = Null.SetNullGuid(dr["AgendaId"]);
                objAgendaDomain.Add(objAgenda);
            }
            return objAgendaDomain;
        }

        /// <summary>
        /// Get Agenda with forum and entity
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AgendaDomain> ToAgendaWithForum(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            IList<AgendaDomain> objAgendaDomain = new List<AgendaDomain>();
            AgendaDomain objAgenda = null;
            while (dr.Read())
            {

                objAgenda = new AgendaDomain();
                objAgenda = ToAgendaDomain(dr);
                objAgenda.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objAgenda.EntityName = Null.SetNullString(dr["EntityName"]);
                objAgenda.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objAgenda.ForumName = Null.SetNullString(dr["ForumName"]);
                objAgenda.MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"]));
                objAgenda.MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]));
                objAgenda.AgendaChecker = Null.SetNullGuid(dr["AgendaChecker"]);
                if(clsmain.checkColumExist("Status", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "Status");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objAgenda.IsApproved = Null.SetNullString(dr["Status"]);
                }
                if (clsmain.checkColumExist("SerialNumberType", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialNumberType");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objAgenda.SerialNumberType = Null.SetNullString(dr["SerialNumberType"]);
                }
                if (clsmain.checkColumExist("MeetingVenue", dr))
                //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingVenue");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objAgenda.MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"]));
                }
                if (clsmain.checkColumExist("MeetingNumber", dr))
                // if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objAgenda.MeetingNumber = Null.SetNullString(dr["MeetingNumber"]);
                }
                objAgendaDomain.Add(objAgenda);
            }
            return objAgendaDomain;
        }

        /// <summary>
        /// Get Agenda with forum and entity and minute
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AgendaDomain> ToAgendaWithMinutes(IDataReader dr)
        {
            IList<AgendaDomain> objAgendaDomain = new List<AgendaDomain>();
            AgendaDomain objAgenda = null;
            while (dr.Read())
            {
                objAgenda = new AgendaDomain();
                objAgenda = ToAgendaDomain(dr);
                objAgenda.EntityId = Null.SetNullGuid(dr["EntityId"]);
                objAgenda.EntityName = Null.SetNullString(dr["EntityName"]);
                objAgenda.ForumId = Null.SetNullGuid(dr["ForumId"]);
                objAgenda.ForumName = Null.SetNullString(dr["ForumName"]);
                objAgenda.MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"]));
                objAgenda.MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]));
                objAgenda.AgendaChecker = Null.SetNullGuid(dr["AgendaChecker"]);

                objAgenda.Minutes = Null.SetNullString(dr["Minutes"]);
                objAgendaDomain.Add(objAgenda);
            }
            return objAgendaDomain;
        }

        /// <summary>
        /// Get Agenda with forum and entity and minute
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<AgendaDomain> ToAgendaWithChecker(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();

            IList<AgendaDomain> objAgendaDomain = new List<AgendaDomain>();
            AgendaDomain objAgenda = null;
            while (dr.Read())
            {
                objAgenda = new AgendaDomain();
                objAgenda = ToAgendaDomain(dr);
                string Suffix = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"]));
                string FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"]));
                string LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"]));
                objAgenda.CheckerName = Suffix + " " + FirstName + " " + LastName;

                if(clsmain.checkColumExist("SerialNumberType", dr))
                //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialNumberType");
                //if (dr.GetSchemaTable().DefaultView.Count > 0)
                {
                    objAgenda.SerialNumberType = Null.SetNullString(dr["SerialNumberType"]);
                }
                objAgendaDomain.Add(objAgenda);
            }
            return objAgendaDomain;
        }
        #endregion

        #region "Get"

        ///// <summary>
        ///// Get All Agenda Details
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public IList<AgendaDomain> Get()
        //{
        //    IList<AgendaDomain> AgendaDomain = null;
        //    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAgendaDetails"))
        //    {
        //        AgendaDomain = ToAgendaDomains(dataReader);
        //    }

        //    return AgendaDomain;
        //}

        /// <summary>
        /// Get all agendas
        /// </summary>
        /// <returns></returns>
        public IList<AgendaDomain> Get()
        {
            IList<AgendaDomain> AgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllAgendaDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAgendaDetails"))
            {
                AgendaDomain = ToAgendaDomains(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        /// <summary>
        /// Get Agenda By Id
        /// </summary>
        /// <param name="AgendaId">Guid sepcifing AgendaId</param>
        /// <returns></returns>
        public AgendaDomain Get(Guid AgendaId)
        {
            AgendaDomain objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaDetailsById", AgendaId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objAgenda = ToAgendaDomain(dataReader);
                objAgenda.AgendaChecker = Null.SetNullGuid(dataReader["AgendaChecker"]);

            }
            conn.Close();
            return objAgenda;
        }

        /// <summary>
        /// Get agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetAgendabyMeetingId(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaByMeetingId", MeetingId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithForum(dataReader);

            }
            conn.Close();
            return objAgenda;
        }



        /// <summary>
        /// Get agenda for report
        /// </summary>
        /// <returns></returns>
        public DataSet GetAgendaForReport(Guid ForumId, string AgendaName, string SerialNumber, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaForReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_AgendaName", AgendaName);
            cmd.Parameters.AddWithValue("p_SerialNumber", SerialNumber);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageIndex", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaForReport", ForumId, AgendaName, SerialNumber, StartDate, EndDate, PageNo, PageSize);
            return ds;
        }

        /// <summary>
        /// Get agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetAgendabyMeetingIdForPublish(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaByMeetingIdToPublish", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaByMeetingIdToPublish", MeetingId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithChecker(dataReader);

            }
            conn.Close();
            return objAgenda;
        }
        /// <summary>
        /// Get agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetAgendabyMeetingId(Guid MeetingId, Guid UserId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaByMeetingIdAndChecker", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_CheckerId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaByMeetingIdAndChecker", MeetingId, UserId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithForum(dataReader);

            }
            conn.Close();
            return objAgenda;
        }


        /// <summary>
        /// Get approved agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetApprovedAgendabyMeetingId(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedAgendaByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedAgendaByMeetingId", MeetingId, UserId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithForum(dataReader);

            }
            conn.Close();
            return objAgenda;
        }


        /// <summary>
        /// Get approved agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetApprovedPublishedAgendabyMeetingId(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedPublishedAgendaByMeetingId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedPublishedAgendaByMeetingId", MeetingId, UserId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithForum(dataReader);

            }
            conn.Close();
            return objAgenda;
        }


        /// <summary>
        /// Get approved agenda items by meeting id for minutes
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetApprovedAgendaForMinutes(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedAgendaByMeetingIdForMinutes", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedAgendaByMeetingIdForMinutes", MeetingId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithMinutes(dataReader);

            }
            conn.Close();
            return objAgenda;
        }
        /// <summary>
        /// Get Agenda titles
        /// </summary>
        /// <param name="strSearch">string sepcifing strSearch</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetAgendaTitles(string strSearch)
        {
            IList<AgendaDomain> AgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaTitle", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_strSearch", strSearch);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaTitle", strSearch))
            {
                AgendaDomain = ToAgendaTitles(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        /// <summary>
        ///  Get All Agenda titles
        /// </summary>
        /// <returns></returns>
        public IList<AgendaDomain> GetAllAgendaTitles(Guid ForumId)
        {
            IList<AgendaDomain> AgendaDomain = null;

            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAgendaTitles"))

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllAgendaTitlesByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllAgendaTitlesByEntity", EntityId, ForumId))
            {
                AgendaDomain = ToAgendaTitles(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        /// <summary>
        ///  Get All Agenda titles
        /// </summary>
        /// <returns></returns>
        public IList<AgendaDomain> GetAllMasterAgenda(Guid EntityId, Guid MeetingId)
        {
            IList<AgendaDomain> AgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllMasterAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllMasterAgenda", EntityId, MeetingId))
            {
                AgendaDomain = ToAgendaTitles(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        /// <summary>
        ///  Get All Agenda Classification
        /// </summary>
        /// <returns></returns>
        public IList<AgendaDomain> GetAllMasterClassification(Guid EntityId)
        {
            IList<AgendaDomain> AgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllMasterClassification", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllMasterClassification", EntityId))
            {
                AgendaDomain = ToAgendaTitles(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        /// <summary>
        /// Get Sub agenda by agenda id
        /// </summary>
        /// <param name="AgendaId"></param>
        /// <returns></returns>
        public IList<AgendaDomain> GetSubAgendaByAgendaId(Guid AgendaId)
        {
            IList<AgendaDomain> AgendaDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetSubAgendaByAgendaId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ParentAgendaId", AgendaId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //            using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetSubAgendaByAgendaId", AgendaId))
            {
                AgendaDomain = ToAgendaDomains(dataReader);
            }
            conn.Close();
            return AgendaDomain;
        }

        #endregion

        #region "Modify"
        /// <summary>
        /// Delete agenda details
        /// </summary>
        /// <param name="AgendaId">Guid sepcifing AgendaId</param>
        /// <returns></returns>
        public bool Delete(Guid AgendaId)
        {
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteAgendaDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteAgendaDetailsById", AgendaId, UserId)) > 0;
        }

        /// <summary>
        /// Update Agenda Ordes
        /// </summary>
        /// <param name="strAgendaOrder">string sepcifing strAgendaOrder</param>
        /// <param name="strAgendaIds">string sepcifing strAgendaIds</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateAgendaOrders(string strAgendaOrder, string strAgendaIds, Guid UserId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAgendaOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserId);
            cmd.Parameters.AddWithValue("p_AgendaIds", strAgendaIds);
            cmd.Parameters.AddWithValue("p_AgendaOrders", strAgendaOrder);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();

            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateAgendaOrder", UserId, strAgendaIds, strAgendaOrder))
            //    > 0;
        }

        /// <summary>
        /// Insert Agenda Details
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        /// stp_InsertAgendaDetails
        public AgendaDomain Insert(AgendaDomain agendaDomain, string AgendaFile = "", Guid MasterEntity = new Guid())
        {
            string EncryptionKey = System.Web.HttpContext.Current.Session["EncryptionKey"].ToString();

            string AgendaKey = MM.Extended.Encryptor.KeyValue;
            Random random = new Random();
            AgendaKey = AgendaKey + random.Next(1000, 9999).ToString();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAgendaDetailsNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaName",  agendaDomain.AgendaName);
            cmd.Parameters.AddWithValue("p_AgendaNote",  agendaDomain.AgendaNote);
            cmd.Parameters.AddWithValue("p_ParentAgendaId", agendaDomain.ParentAgendaId);
            cmd.Parameters.AddWithValue("p_UploadedAgendaNote", agendaDomain.UploadedAgendaNote);
            cmd.Parameters.AddWithValue("p_IsPublished", false);
            cmd.Parameters.AddWithValue("p_PublishedBy", agendaDomain.PublishedBy);
            cmd.Parameters.AddWithValue("p_CreatedBy", agendaDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", agendaDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_MeetingId", agendaDomain.MeetingId);
            cmd.Parameters.AddWithValue("p_AgendaChecker", agendaDomain.AgendaChecker);
            cmd.Parameters.AddWithValue("p_PdfName", AgendaFile);
            cmd.Parameters.AddWithValue("p_MasterEntity", MasterEntity);
            cmd.Parameters.AddWithValue("p_AgendaKey", Encryptor.EncryptString(AgendaKey));
            cmd.Parameters.AddWithValue("p_Classification", agendaDomain.Classification);
            cmd.Parameters.AddWithValue("p_EncryptionKey", Encryptor.EncryptString(EncryptionKey));
            cmd.Parameters.AddWithValue("p_Presenter", agendaDomain.Presenter);
            cmd.Parameters.AddWithValue("p_SerialNumber", agendaDomain.SerialNumber);
            cmd.Parameters.AddWithValue("p_SerialNumberType", agendaDomain.SerialNumberType);
            cmd.Parameters.AddWithValue("p_SerialTitle", agendaDomain.SerialTitle);
            cmd.Parameters.AddWithValue("p_SerialText", agendaDomain.SerialText);
            agendaDomain.AgendaId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //agendaDomain.AgendaId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAgendaDetailsNew", agendaDomain.AgendaName, agendaDomain.AgendaNote, agendaDomain.ParentAgendaId, agendaDomain.UploadedAgendaNote, false, agendaDomain.PublishedBy, agendaDomain.CreatedBy, agendaDomain.UpdatedBy, agendaDomain.MeetingId, agendaDomain.AgendaChecker, AgendaFile, MasterEntity, Encryptor.EncryptString(AgendaKey), agendaDomain.Classification, Encryptor.EncryptString(EncryptionKey), agendaDomain.Presenter, agendaDomain.SerialNumber, agendaDomain.SerialNumberType, agendaDomain.SerialTitle, agendaDomain.SerialText);
            return agendaDomain;
        }

        /// <summary>
        /// Update agenda details
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool Update(AgendaDomain agendaDomain, string DeletedAgendaFull = "", string AgendaFile = "", string DeletedAgendaPdf = "")
        {
            string EncryptionKey = System.Web.HttpContext.Current.Session["EncryptionKey"].ToString();

            string AgendaKey = MM.Extended.Encryptor.KeyValue;
            Random random = new Random();
            AgendaKey = AgendaKey + random.Next(1000, 9999).ToString();

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAgendaDetailsNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", agendaDomain.AgendaId);
            cmd.Parameters.AddWithValue("p_AgendaName", agendaDomain.AgendaName);
            cmd.Parameters.AddWithValue("p_AgendaNote", agendaDomain.AgendaNote);
            cmd.Parameters.AddWithValue("p_ParentAgendaId", agendaDomain.ParentAgendaId);
            cmd.Parameters.AddWithValue("p_UploadedAgendaNote", agendaDomain.UploadedAgendaNote);
            cmd.Parameters.AddWithValue("p_IsPublished", agendaDomain.IsPublished);
            cmd.Parameters.AddWithValue("p_PublishedBy", agendaDomain.PublishedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", agendaDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_AgendaChecker", agendaDomain.AgendaChecker);
            cmd.Parameters.AddWithValue("p_DeletedAgenda", DeletedAgendaFull);
            cmd.Parameters.AddWithValue("p_FileName", AgendaFile);
            cmd.Parameters.AddWithValue("p_DeletedPdf", DeletedAgendaPdf);
            cmd.Parameters.AddWithValue("p_AgendaKey", Encryptor.EncryptString(AgendaKey));
            cmd.Parameters.AddWithValue("p_Classification", agendaDomain.Classification);
            cmd.Parameters.AddWithValue("p_EncryptionKey", Encryptor.EncryptString(EncryptionKey));
            cmd.Parameters.AddWithValue("p_Presenter",agendaDomain.Presenter);
            cmd.Parameters.AddWithValue("p_SerialNumber", agendaDomain.SerialNumber);
            
            cmd.Parameters.AddWithValue("p_SerialNumberType", agendaDomain.SerialNumberType);
            cmd.Parameters.AddWithValue("p_SerialTitle", agendaDomain.SerialTitle);
            cmd.Parameters.AddWithValue("p_SerialText", agendaDomain.SerialText);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateAgendaDetailsNew", agendaDomain.AgendaId, agendaDomain.AgendaName, agendaDomain.AgendaNote, agendaDomain.ParentAgendaId, agendaDomain.UploadedAgendaNote, agendaDomain.IsPublished, agendaDomain.PublishedBy, agendaDomain.UpdatedBy, agendaDomain.AgendaChecker, DeletedAgendaFull, AgendaFile, DeletedAgendaPdf, Encryptor.EncryptString(AgendaKey), agendaDomain.Classification, Encryptor.EncryptString(EncryptionKey))) > 0;
        }

        /// <summary>
        /// Update agenda details
        /// </summary>
        /// <param name="agendaDomain">Class object sepcifing agendaDomain</param>
        /// <returns></returns>
        public bool UpdateKey(AgendaDomain agendaDomain, string DeletedAgendaFull = "", string AgendaFile = "", string DeletedAgendaPdf = "", string EncryptionKey = "", string UploadOn = "")
        {
            string AgendaKey = MM.Extended.Encryptor.KeyValue;
            Random random = new Random();
            AgendaKey = AgendaKey + random.Next(1000, 9999).ToString();

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAgendaDetailsNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", agendaDomain.AgendaId);
            cmd.Parameters.AddWithValue("p_AgendaName", agendaDomain.AgendaName);
            cmd.Parameters.AddWithValue("p_AgendaNote", (object)(string.IsNullOrEmpty(agendaDomain.AgendaNote) ? string.Empty : agendaDomain.AgendaNote) );
            cmd.Parameters.AddWithValue("p_ParentAgendaId", agendaDomain.ParentAgendaId);
            cmd.Parameters.AddWithValue("p_UploadedAgendaNote", (object)(string.IsNullOrEmpty(agendaDomain.UploadedAgendaNote) ? string.Empty : agendaDomain.UploadedAgendaNote) );
            cmd.Parameters.AddWithValue("p_IsPublished", agendaDomain.IsPublished);
            cmd.Parameters.AddWithValue("p_PublishedBy", agendaDomain.PublishedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", agendaDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_AgendaChecker", agendaDomain.AgendaChecker);
            cmd.Parameters.AddWithValue("p_DeletedAgenda", (object)(string.IsNullOrEmpty(DeletedAgendaFull) ? string.Empty : DeletedAgendaFull) );
            cmd.Parameters.AddWithValue("p_FileName", (object)(string.IsNullOrEmpty(AgendaFile) ? string.Empty : AgendaFile) );
            cmd.Parameters.AddWithValue("p_DeletedPdf", (object)(string.IsNullOrEmpty(DeletedAgendaPdf) ? string.Empty : DeletedAgendaPdf) );
            cmd.Parameters.AddWithValue("p_AgendaKey", (object)(string.IsNullOrEmpty(Encryptor.EncryptString(AgendaKey)) ? string.Empty : Encryptor.EncryptString(AgendaKey)));
            cmd.Parameters.AddWithValue("p_Classification", (object)(string.IsNullOrEmpty(agendaDomain.Classification) ? string.Empty : agendaDomain.Classification) );
            cmd.Parameters.AddWithValue("p_EncryptionKey", Encryptor.EncryptString(EncryptionKey));
            cmd.Parameters.AddWithValue("p_Presenter", (object)(string.IsNullOrEmpty(agendaDomain.Presenter) ? string.Empty : agendaDomain.Presenter) );
            cmd.Parameters.AddWithValue("p_SerialNumber", (object)(string.IsNullOrEmpty(agendaDomain.SerialNumber) ? string.Empty : agendaDomain.SerialNumber) );
            cmd.Parameters.AddWithValue("p_FileUploadOn", (object)(string.IsNullOrEmpty(UploadOn) ? string.Empty : UploadOn) );
            cmd.Parameters.AddWithValue("p_SerialNumberType", (object)(string.IsNullOrEmpty(agendaDomain.SerialNumberType) ? string.Empty : agendaDomain.SerialNumberType) );
            cmd.Parameters.AddWithValue("p_SerialTitle", (object)(string.IsNullOrEmpty(agendaDomain.SerialTitle) ? string.Empty : agendaDomain.SerialTitle) );
            cmd.Parameters.AddWithValue("p_SerialText", (object)(string.IsNullOrEmpty(agendaDomain.SerialText) ? string.Empty : agendaDomain.SerialText) );
            
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



//            return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateAgendaDetailsNew", agendaDomain.AgendaId, agendaDomain.AgendaName, agendaDomain.AgendaNote, agendaDomain.ParentAgendaId, agendaDomain.UploadedAgendaNote, agendaDomain.IsPublished, agendaDomain.PublishedBy, agendaDomain.UpdatedBy, agendaDomain.AgendaChecker, DeletedAgendaFull, AgendaFile, DeletedAgendaPdf, Encryptor.EncryptString(AgendaKey), agendaDomain.Classification, Encryptor.EncryptString(EncryptionKey), agendaDomain.Presenter, agendaDomain.SerialNumber, UploadOn, agendaDomain.SerialNumberType, agendaDomain.SerialTitle, agendaDomain.SerialText)) > 0;
        }
        /// <summary>
        /// Get All unapproved agenda document
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetUnApprovedDocumet(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUnApprovedAgendaDocument", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUnApprovedAgendaDocument", MeetingId))
            {
                objAgenda = ToAgendaDomains(dataReader);
            }
            conn.Close();
            return objAgenda;
        }

        /// <summary>
        /// Change agenda status
        /// </summary>
        /// <param name="AgendaId">Guid sepcifing AgendaId</param>
        /// <param name="Status">Guid sepcifing Status</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateAgendaStatus(Guid AgendaId, string Status, Guid UserId)
        {
            bool Res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAgendaStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            cmd.Parameters.AddWithValue("p_Status", Status);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { Res = true; }
            conn.Close();
            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateAgendaStatus", AgendaId, Status, UserId)) > 0;
            return Res;
        }

        /// <summary>
        /// Get agenda keys
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public DataSet GetAgendKeys(Guid AgendaId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaKeys", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaKeys", AgendaId);
            return ds;

        }

        /// <summary>
        /// Get Agenda key by agenda id
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public DataSet GetAgendKeysByName(string AgendaName)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaKeyByName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaName", AgendaName);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaKeyByName", AgendaName);
            return ds;
        }

        /// <summary>
        /// Get Agenda key by agenda id
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public DataSet GetAgendKeysByName(string AgendaName, Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaKeyByNameWithUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaName", AgendaName);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaKeyByNameWithUserId", AgendaName, UserId);
            return ds;
        }
        /// <summary>
        /// Change agenda status
        /// </summary>
        /// <param name="AgendaId">string sepcifing AgendaId</param>
        /// <param name="Status">Guid sepcifing Status</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateAgendaChecker(string AgendaIds, Guid ChekerId, Guid UserId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateAgendaChecker", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_CheckerId", ChekerId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserId);
            cmd.Parameters.AddWithValue("p_AgendaIds", AgendaIds);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateAgendaChecker", ChekerId, UserId, AgendaIds)) > 0;

        }

        /// <summary>
        /// Publish agenda
        /// </summary>
        /// <param name="AgendaId">string sepcifing AgendaId</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool PublishAgenda(string AgendaIds, Guid UserId, string UnReadAgendaIds = "", string DeleteAgenda = "")
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_PublishAgenda", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserId);
            cmd.Parameters.AddWithValue("p_AgendaIds", AgendaIds);
            cmd.Parameters.AddWithValue("p_UnReadAgenda", UnReadAgendaIds);
            cmd.Parameters.AddWithValue("p_DeleteAgenda", DeleteAgenda);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_PublishAgenda", UserId, AgendaIds, UnReadAgendaIds, DeleteAgenda)) > 0;

        }

        /// <summary>
        /// Get approved agenda items by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetApprovedAgendabyMeetingIdForAccess(Guid MeetingId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedAgendaByMeetingIdForAccesss", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedAgendaByMeetingIdForAccesss", MeetingId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaWithForum(dataReader);

            }
            conn.Close();
            return objAgenda;
        }
        #endregion

        public bool RemoveChecker(Guid MeetingId, Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UnlockUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_RemoveAgendaChecker", MeetingId, UserId)) > 0;
        }

        /// <summary>
        /// Insert agenda Ack 
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="AgendaReadOn">DateTime specifying AgendaReadOn</param>
        /// <param name="NoticeReadOn">DateTime specifying NoticeReadOn</param>
        /// <returns>Int Id</returns>
        public Int32 InsertAgendaAck(Guid MeetingId, Guid UserId, DateTime? AgendaReadOn, DateTime? NoticeReadOn, bool IsNoticeRead, bool IsAgendaRead, Guid PublishVersion, string AccessToken)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertAgendaAck", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_NoticeReadOn", NoticeReadOn);
            cmd.Parameters.AddWithValue("p_AgendaReadOn", AgendaReadOn);
            cmd.Parameters.AddWithValue("p_IsNoticeRead", IsNoticeRead);

            cmd.Parameters.AddWithValue("p_AgendaRead", IsAgendaRead);
            cmd.Parameters.AddWithValue("p_PublishVesrion", PublishVersion);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_RequestToken", AccessToken);

            Int32 Id = Convert.ToInt32(cmd.ExecuteScalar());
            //Int32 Id = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            
            //Int32 Id = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertAgendaAck", 
            //    MeetingId, NoticeReadOn, AgendaReadOn, UserId, IsNoticeRead, IsAgendaRead, PublishVersion, AccessToken));
            return Id;
        }

        /// <summary>
        /// Get ack histroy by id
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public DataSet GetAckHistroy(Guid MeetingId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPublishHistoryByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Meeting", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //return SqlHelper.ExecuteDataset(ConnectionString, "stp_GetPublishHistoryByMeeting", MeetingId);
            return ds;
        }

        /// <summary>
        /// Get Email histroy by id
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public DataSet GetEmailkHistroy(Guid MeetingId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetEmailHistory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("p_Meeting", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //return SqlHelper.ExecuteDataset(ConnectionString, "stp_GetEmailHistory", MeetingId);
            return ds;
        }

        /// <summary>
        /// Get Agenda read unread
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public DataSet GetAgendaReadUnread(Guid MeetingId, Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaReadUread", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //return SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaReadUread", MeetingId, UserId);
            return ds;
        }

        /// <summary>
        /// Get Published agenda items by publish id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifing MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetPublishedAgendabyPublishId(Guid PublishId)
        {
            IList<AgendaDomain> objAgenda = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetApprovedAgendaByPublishVersion", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_PublishVersion", PublishId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetApprovedAgendaByPublishVersion", PublishId))
            {
                //if (!dataReader.Read())
                //{
                //    return null;
                //}
                objAgenda = ToAgendaDomains(dataReader);

            }
            conn.Close();
            return objAgenda;
        }

        /// <summary>
        /// Get agenda Read coun
        /// </summary>
        /// <param name="AgendaId">string sepcifing AgendaId</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <param name="IsRead">bool specifying IsRead</param>
        /// <returns></returns>
        public DataSet GetAgendaCounter(string AgendaId, Guid UserId, bool IsRead)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UpdateReadStaus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_IsRead", IsRead);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            // return SqlHelper.ExecuteDataset(ConnectionString, "UpdateReadStaus", AgendaId, UserId, IsRead);
            return ds;
        }

        /// <summary>
        /// Get Agenda key by Pdfnames
        /// </summary>
        /// <param name="AgendaPDFNames">DataTable specifying AgendaPDFNames</param>
        /// <returns></returns>
        public DataSet GetAgendKeysByPDFName(DataTable AgendaPDFNames)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("GetAgendaKeys", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AgendaId", AgendaPDFNames);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "GetAgendaKeys", AgendaPDFNames);
            return ds;
        }

        /// <summary>
        /// Get Agend Ack User by PublishVersion
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="PublishVersion">Guid specifying PublishVersion</param>
        /// <returns></returns>
        public DataSet GetAgendAckByPublishVersion(Guid MeetingId, Guid PublishVersion)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPublishHistoryByPublishVersion", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_PublishVersion", PublishVersion);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetPublishHistoryByPublishVersion", MeetingId, PublishVersion);
            return ds;
        }
        
    }
}
