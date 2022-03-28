using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using MySql.Data.MySqlClient;

namespace MM.Data
{
  public class RevisedPdfDataProvider : DataProvider
    {
    
      #region "Singleton"
        private static volatile RevisedPdfDataProvider _instance;
        private static object _synRoot = new Object();

        private RevisedPdfDataProvider()
        {
        }

        public static RevisedPdfDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new RevisedPdfDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

      #region "Fill"
        /// <summary>
        /// Fill Revised pdf details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public RevisedPdfDomain ToRevisedPdfDomain(IDataReader dr)
        {
            return new RevisedPdfDomain
            {
                AgendaId = Null.SetNullGuid(dr["AgendaId"]),               
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                CreateOn = Null.SetNullDateTime(dr["CreateOn"]),         
                Pdf_New = Null.SetNullString(dr["Pdf_New"]),
                Pdf_Old =  Null.SetNullString(dr["Pdf_New"]),
                RevisedId =  Null.SetNullInteger(dr["RevisedId"]),
                Pdf_NewName = Null.SetNullString(dr["Pdf_NewName"]),

                PdfOldName = Null.SetNullString(dr["PdfOldName"]),
                MeetingId = Null.SetNullGuid(dr["MeetingId"]),
                AgendaNew = Null.SetNullString(dr["AgendaNew"]),
                AgendaOld = Null.SetNullString(dr["AgendaOld"])
            };
        }

        /// <summary>
        /// Get all Revised pdf details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<RevisedPdfDomain> ToRevisedPdfDomains(IDataReader dr)
        {
            IList<RevisedPdfDomain> objRevised = new List<RevisedPdfDomain>();
            while (dr.Read())
            {
                objRevised.Add(ToRevisedPdfDomain(dr));
            }
            return objRevised;
        }
        #endregion

        #region "Get"
        //stp_GetRevisedPdfByMeeting

        public IList<RevisedPdfDomain> Get( Guid MeetingId)
        {
            IList<RevisedPdfDomain> objRev = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetRevisedPdfByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetRevisedPdfByMeeting", MeetingId))
            {
                objRev = ToRevisedPdfDomains(dataReader);
            }
            conn.Close();
            return objRev;
        }
        #endregion
    }
}
