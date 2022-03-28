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
    public class SectionDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile SectionDataProvider _instance;
        private static object synRoot = new Object();

        private SectionDataProvider()
        {

        }

        public static SectionDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SectionDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        #region "Fill"


        /// <summary>
        /// Fill Section Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private SectionDomain ToSectionDomain(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string Meeting = "";
            Guid MeetingId = Guid.Empty;
            DateTime CreatedOn = Null.SetNullDateTime(null);
            if(clsmain.checkColumExist("MeetingId", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MeetingId");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                MeetingId = Null.SetNullGuid(dr["MeetingId"]);
                Meeting = Null.SetNullString(dr["MeetingNumber"]) + " " + Encryptor.DecryptString(Null.SetNullString(Null.SetNullString(dr["ForumName"]))) + " " + Convert.ToDateTime(Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]))).ToString("f");
                CreatedOn = Convert.ToDateTime(Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])));
            }

            return new SectionDomain()
{
    SectionId = Null.SetNullGuid(dr["SectionId"]),
    Title = Null.SetNullString(dr["Title"]),
    SerialNumber = Null.SetNullString(dr["SerialNumber"]),

   // CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
    UpdateOn = Null.SetNullDateTime(dr["UpdateOn"]),
    CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
    UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
    Meeting = Meeting,
    MeetingId = MeetingId,
    CreatedOn = CreatedOn

};
        }

        /// <summary>
        /// Get Section domain details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<SectionDomain> ToSectionDomains(IDataReader dr)
        {
            IList<SectionDomain> objSectionDomain = new List<SectionDomain>();
            while (dr.Read())
            {
                objSectionDomain.Add(ToSectionDomain(dr));
            }
            return objSectionDomain;
        }

        #endregion


        #region "Get"

        /// <summary>
        /// Get all Sections
        /// </summary>
        /// <returns></returns>
        public IList<SectionDomain> Get()
        {
            IList<SectionDomain> SectionDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllSectionDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllSectionDetails"))
            {
                SectionDomain = ToSectionDomains(dataReader);
            }
            conn.Close();
            return SectionDomain;
        }

        /// <summary>
        /// Get Section By ForumId
        /// </summary>
        /// <param name="SectionId">Guid sepcifing SectionId</param>
        /// <returns></returns>
        public IList<SectionDomain> Get(Guid ForumId)
        {
            IList<SectionDomain> objSectionDomain = new List<SectionDomain>();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllSectionDetailsByForumId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllSectionDetailsByForumId", ForumId))
            {
                while (dataReader.Read())
                {
                    objSectionDomain.Add(ToSectionDomain(dataReader));
                }
            }
            conn.Close();
            return objSectionDomain;
        }


        /// <summary>
        /// Get Section By Id
        /// </summary>
        /// <param name="SectionId">Guid sepcifing SectionId</param>
        /// <returns></returns>
        public SectionDomain GetAllSectionsDetailsById(Guid SectionId)
        {
            SectionDomain objSection = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetSectionDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SectionId", SectionId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetSectionDetailsById", SectionId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objSection = ToSectionDomain(dataReader);
                //objSection.AgendaChecker = Null.SetNullGuid(dataReader["AgendaChecker"]);

            }
            conn.Close();
            return objSection;
        }

        /// <summary>
        /// Get Section By Id
        /// </summary>
        /// <param name="SectionId">Guid sepcifing SectionId</param>
        /// <returns></returns>
        public IList<SectionDomain> GetMeetingsBySectionId(Guid SectionId, DateTime StartDate, DateTime EndDate)
        {
            IList<SectionDomain> objSectionDomain = new List<SectionDomain>();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingBySection", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SectionId", SectionId);
            cmd.Parameters.AddWithValue("p_StartTime", StartDate);
            cmd.Parameters.AddWithValue("p_EndTime", EndDate);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingBySection", SectionId, StartDate, EndData))
            {
                while (dataReader.Read())
                {
                    objSectionDomain.Add(ToSectionDomain(dataReader));
                }

            }
            conn.Close();
            return objSectionDomain;

        }


        #endregion


        #region "Modify"
        /// <summary>
        /// Delete Section details
        /// </summary>
        /// <param name="SectionId">Guid sepcifing SectionId</param>
        /// <returns></returns>
        public bool Delete(Guid SectionId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSectionDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_SectionId", SectionId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSectionDetailsById", SectionId)) > 0;
        }


        /// <summary>
        /// Update Section details
        /// </summary>
        /// <param name="sectionDomain">Class object sepcifing SectionDomain</param>
        /// <returns></returns>
        public bool Update(SectionDomain SectionDomain)
        {
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateSectionDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_SectionId", SectionDomain.SectionId);
            cmd.Parameters.AddWithValue("p_Title", SectionDomain.Title);
            cmd.Parameters.AddWithValue("p_SerialNumber", SectionDomain.SerialNumber);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateSectionDetails", UserId, SectionDomain.SectionId, SectionDomain.Title, SectionDomain.SerialNumber)) > 0;
        }

        /// <summary>
        /// Insert Section Details
        /// </summary>
        /// <param name="SectionDomain">Class object sepcifing SectionDomain</param>
        /// <returns></returns>
        public SectionDomain Insert(SectionDomain SectionDomain)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertSectionDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Title", SectionDomain.Title);
            cmd.Parameters.AddWithValue("p_SerialNumber", SectionDomain.SerialNumber);
            cmd.Parameters.AddWithValue("p_IsActive", SectionDomain.IsActive);
            cmd.Parameters.AddWithValue("p_CreatedBy", SectionDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", SectionDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_ForumId", SectionDomain.ForumId);
            SectionDomain.SectionId = (Guid)(cmd.ExecuteScalar()); conn.Close();
            //            SectionDomain.SectionId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertSectionDetails", SectionDomain.Title, SectionDomain.SerialNumber, SectionDomain.IsActive, SectionDomain.CreatedBy, SectionDomain.UpdatedBy, SectionDomain.ForumId);
            return SectionDomain;
        }


        /// <summary>
        /// Update section Ordes
        /// </summary>
        /// <param name="strKeyInfoOrder">string sepcifing strsectionOrder</param>
        /// <param name="strKeyInfoIds">string sepcifing strsectionds</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <returns></returns>
        public bool UpdateSectionOrders(string strSectionOrder, string strSectionIds, Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateSectionOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_SectionIds", strSectionIds);
            cmd.Parameters.AddWithValue("p_SectionOrders", strSectionOrder);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateSectionOrder", UserId, strSectionIds, strSectionOrder))
             //   > 0;
        }
        #endregion
    }
}
