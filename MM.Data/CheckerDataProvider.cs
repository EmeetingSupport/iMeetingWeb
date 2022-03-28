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
    public class CheckerDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile CheckerDataProvider _instance;
        private static object synRoot = new Object();

        private CheckerDataProvider()
        {

        }

        public static CheckerDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new CheckerDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// To Checker Entity Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private CheckerDomain ToCheckerDomain(IDataReader dr)
        {
            return new CheckerDomain()
            {
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),
                MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])),
                MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                EntityName = Encryptor.DecryptString(Null.SetNullString(dr["EntityName"])),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"]))
            };
        }

        /// <summary>
        /// To Checker Entity Details List
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<CheckerDomain> ToCheckerDomains(IDataReader dr)
        {
            IList<CheckerDomain> objChecker = new List<CheckerDomain>();
            while (dr.Read())
            {
                objChecker.Add(ToCheckerDomain(dr));
            }
            return objChecker;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get all UnApproved Meetings
        /// </summary>
        /// <returns></returns>
        public IList<CheckerDomain> GetAllUnApprovedMeeting()
        {

            IList<CheckerDomain> objCheckerDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUnAprovedMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUnAprovedMeeting" ))
            {
                objCheckerDomains = ToCheckerDomains(dataReader);
            }
            conn.Close();
            return objCheckerDomains;         

        }

        public DataSet GetPendingDetails()
        {
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            Guid UserId = Guid.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPendingDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetPendingDetails", EntityId, UserId);
            return ds;
        }
        #endregion

    }
}

