using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MM.Core;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class AuditDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile AuditDataProvider _instance;
        private static object synRoot = new Object();

        private AuditDataProvider()
        {

        }

        public static AuditDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new AuditDataProvider();
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
        private AgendaAuditDomain ToAgendaAuditDomain(IDataReader dr)
        {
            return new AgendaAuditDomain()
            {
                AgendaTitle = Null.SetNullString(dr["AgendaName"]),
                Meeting = Convert.ToDateTime(Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]))).ToString("f"),
                IsPublished = Null.SetNullString(dr["IsPublished"]),
                //     PublishedBy = Null.SetNullGuid(dr["PublishedBy"]),
                Date = Null.SetNullDateTime(dr["UpdateOn"]),
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["MakerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MakerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["MakerLastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["MakerUserName"])) + ")",

                CheckerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + (string.IsNullOrEmpty(Null.SetNullString(dr["UserName"])) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")"),

                UploadedPdfs = GetAgendaPdfNames(Null.SetNullString(dr["UploadedAgendaNote"]), Null.SetNullString(dr["DeletedPdfs"]), Null.SetNullString(dr["AgendaNote"])),//Null.SetNullString(dr["UploadedAgendaNote"]),
               // MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),
              //  Department = Null.SetNullString(dr["DepartmentName"]),
                Status = Null.SetNullString(dr["Status"]),
                //MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                IsDelete = Null.SetNullString(dr["IsActive"]),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
             //   MeetingNumber = Null.SetNullString(dr["MeetingNumber"]),

                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }

        /// <summary>
        /// Get all access right
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<AgendaAuditDomain> ToAgendaAuditDomains(IDataReader dr,out int totalCount)
        {
            IList<AgendaAuditDomain> objAgendaAuditDomains = new List<AgendaAuditDomain>();
            int count = 0;

            while (dr.Read())
            {
                objAgendaAuditDomains.Add(ToAgendaAuditDomain(dr));
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    count = Null.SetNullInteger(dr["RecordCount"]);
                    
                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount = count;
            return objAgendaAuditDomains;
        }




        /// <summary>
        /// Fill Agenda Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private AgendaReportDomain ToAgendaReportDomain(IDataReader dr)
        {
            return new AgendaReportDomain()
            {
                AgendaTitle = Null.SetNullString(dr["AgendaName"]),
                MeetingDate_Time = Convert.ToDateTime(Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"]))).ToString("f"),

                FinalApprover = Encryptor.DecryptString(Null.SetNullString(dr["FinalSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FinalFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["FinalLastName"])) + (string.IsNullOrEmpty(Encryptor.DecryptString(Null.SetNullString(dr["FinalUserName"]))) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["FinalUserName"])) + ")"),

                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + (string.IsNullOrEmpty(Null.SetNullString(dr["UserName"])) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")"),

                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                CS = GetName(Null.SetNullString(dr["CS"])),

                MD_CEO = GetName(Null.SetNullString(dr["MD & CEO"])),
                AgendaChecker = new string[] { "1","2","3"}
            };
        }

        /// <summary>
        /// Get all access right
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<AgendaReportDomain> ToAgendaReportDomains(IDataReader dr)
        {
            IList<AgendaReportDomain> objAgendaReportDomains = new List<AgendaReportDomain>();
            while (dr.Read())
            {
                objAgendaReportDomains.Add(ToAgendaReportDomain(dr));
            }
            return objAgendaReportDomains;
        }



        /// <summary>
        /// Fill Notice Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private MeetingAuditDomain ToMeetingAuditDomain(IDataReader dr)
        {
            return new MeetingAuditDomain()
            {
                //   Notice = Null.SetNullString(dr["NoticeMessage"]),
                MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])),

                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",

                CheckerName = Encryptor.DecryptString(Null.SetNullString(dr["ApproverSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["ApproverFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["ApproverLastName"])) + (string.IsNullOrEmpty(Null.SetNullString(dr["ApproverUserName"])) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["ApproverUserName"])) + ")"),

                MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),
                MeetingNumber = Null.SetNullString(dr["MeetingNumber"]),
                MeetingStatus = Null.SetNullString(dr["IsMeetingApproved"]),
                //     NoticeStatus = Null.SetNullString(dr["IsNoticeApproved"]),
                MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                IsDelete = Null.SetNullString(dr["IsActive"]),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }

        /// <summary>
        /// Fill Notice Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private NoticeAuditDomain ToNoticeAuditDomain(IDataReader dr)
        {
            return new NoticeAuditDomain()
            {
                Notice = Null.SetNullString(dr["NoticeMessage"]),

                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",
                CheckerName = Encryptor.DecryptString(Null.SetNullString(dr["ApproverSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["ApproverFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["ApproverLastName"])) + (string.IsNullOrEmpty(Null.SetNullString(dr["ApproverUserName"])) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["ApproverUserName"])) + ")"),
                NoticeStatus = Null.SetNullString(dr["IsApproved"]),
                IsPublished = Null.SetNullString(dr["IsEnable"]),
                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }

        /// <summary>
        /// Get Meeting Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<MeetingAuditDomain> ToMeetingAuditDomains(IDataReader dr,out int totalCount, out IList<NoticeAuditDomain> objNotice)
        {
            IList<MeetingAuditDomain> objMeetingAuditDomain = new List<MeetingAuditDomain>();
            IList<NoticeAuditDomain> objNoticeAuditDomain = new List<NoticeAuditDomain>();
            int count=0;
            while (dr.Read())
            {
                objMeetingAuditDomain.Add(ToMeetingAuditDomain(dr));
            }

            //if (dr.NextResult())
            //{
            //    while (dr.Read())
            //    {
            //       // objNoticeAuditDomain.Add(ToNoticeAuditDomain(dr));
            //        //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
            //    }
            //}

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    count = Null.SetNullInteger(dr["RecordCount"]);

                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount = count;


            objNotice = objNoticeAuditDomain;
            return objMeetingAuditDomain;
        }

        /// <summary>
        /// Get Notice Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<NoticeAuditDomain> ToNoticeAuditDomains(IDataReader dr)
        {
            IList<NoticeAuditDomain> objNoticeAuditDomain = new List<NoticeAuditDomain>();
            while (dr.Read())
            {
                objNoticeAuditDomain.Add(ToNoticeAuditDomain(dr));
            }
            return objNoticeAuditDomain;
        }

        /// <summary>
        /// Fill User Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UserAuditDomain ToUserAuditDomain(IDataReader dr)
        {
            return new UserAuditDomain()
            {
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["MakerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MakerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["MakerLastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["MakerUserName"])) + ")",

                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                Name = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",

                CheckerName = Encryptor.DecryptString(Null.SetNullString(dr["CheckerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["CheckerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["CheckerLastName"])) + (string.IsNullOrEmpty(Null.SetNullString(dr["CheckerUserName"])) ? "" : "(" + Encryptor.DecryptString(Null.SetNullString(dr["CheckerUserName"])) + ")"),

                Email = Encryptor.DecryptString(Null.SetNullString(dr["Email"])),
                IsChecker = Null.SetNullString(dr["IsChecker"]),
                IsMaker = Null.SetNullString(dr["IsMaker"]),

                EnabledOnIpad = Null.SetNullString(dr["EnabledOnIpad"]),

                EnabledOnWeb = Null.SetNullString(dr["EnabledOnWeb"]),
               

                UserStatus = Null.SetNullString(dr["IsApproved"]),
                Moblie = Encryptor.DecryptString(Null.SetNullString(dr["Moblie"])),
                Designation = Encryptor.DecryptString(Null.SetNullString(dr["Designation"])),
                IsDelete = Null.SetNullString(dr["IsActive"]),

                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }


        /// <summary>
        /// Fill User Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UserDepartmentAuditDomain ToUserDepartmentAuditDomain(IDataReader dr)
        {
            return new UserDepartmentAuditDomain()
            {
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["MakerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MakerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["MakerLastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["MakerUserName"])) + ")",
                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                DepartmentName = Null.SetNullString(dr["DepartmentName"]),
                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }

        /// <summary>
        /// Get all User details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<UserAuditDomain> ToUserAuditDomains(IDataReader dr,out int totalCount, out IList<UserDepartmentAuditDomain> objDepartment)
        {
            IList<UserAuditDomain> objUserAuditDomain = new List<UserAuditDomain>();
            IList<UserDepartmentAuditDomain> objUserDepAuditDomain = new List<UserDepartmentAuditDomain>();
            int count=0;
            while (dr.Read())
            {
                objUserAuditDomain.Add(ToUserAuditDomain(dr));
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                     count = Null.SetNullInteger(dr["RecordCount"]);
                   // objUserDepAuditDomain.Add(ToUserDepartmentAuditDomain(dr));
                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount=count;
            objDepartment = objUserDepAuditDomain;
            return objUserAuditDomain;
        }



        /// <summary>
        /// Fill Forum access 
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ForumAccessAuditDomain ToForumAccessAuditDomain(IDataReader dr)
        {
            return new ForumAccessAuditDomain()
            {
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["MakerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MakerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["MakerLastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["MakerUserName"])) + ")",
                Date = Null.SetNullDateTime(dr["UpdateOn"]).ToString("F"),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                AuditAction = Null.SetNullString(dr["AuditAction"]),
                Name = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",
                IsDelete = Null.SetNullString(dr["IsActive"])
            };
        }


        /// <summary>
        /// Get all Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<ForumAccessAuditDomain> ToForumAccessAuditDomains(IDataReader dr,out int totalCount)
        {
            IList<ForumAccessAuditDomain> objForumAccessAuditDomains = new List<ForumAccessAuditDomain>();
            int count = 0;
            while (dr.Read())
            {
                objForumAccessAuditDomains.Add(ToForumAccessAuditDomain(dr));
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    count = Null.SetNullInteger(dr["RecordCount"]);

                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount = count;

            return objForumAccessAuditDomains;
        }


        /// <summary>
        /// Fill Forum access 
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private ForumAuditDomain ToForumAuditDomain(IDataReader dr)
        {
            return new ForumAuditDomain()
            {
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["MakerSuffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["MakerFirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["MakerLastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["MakerUserName"])) + ")",
                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                AuditAction = Null.SetNullString(dr["AuditAction"]),
                //     CheckerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                //   Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",
                IsDelete = Null.SetNullString(dr["IsActive"])
            };
        }

        /// <summary>
        /// Get all Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<ForumAuditDomain> ToForumAuditDomains(IDataReader dr,out int totalCount)
        {
            IList<ForumAuditDomain> objForumAuditDomains = new List<ForumAuditDomain>();
            int count = 0;
            while (dr.Read())
            {
                objForumAuditDomains.Add(ToForumAuditDomain(dr));
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    count = Null.SetNullInteger(dr["RecordCount"]);

                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount = count;

            return objForumAuditDomains;
        }


        /// <summary>
        /// Fill Upload Minutes Detail
        /// </summary> 
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UploadMinuteAuditDomain ToUploadMinuteAuditDomain(IDataReader dr)
        {
            return new UploadMinuteAuditDomain()
            {
              
                MeetingDate = Encryptor.DecryptString(Null.SetNullString(dr["MeetingDate"])),

                Date = Null.SetNullDateTime(dr["UpdatedOn"]).ToString("F"),
                MakerName = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])) + " " + Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])) + " " +
                Encryptor.DecryptString(Null.SetNullString(dr["LastName"])) + "(" + Encryptor.DecryptString(Null.SetNullString(dr["UserName"])) + ")",

              
                MeetingVenue = Encryptor.DecryptString(Null.SetNullString(dr["MeetingVenue"])),


                MeetingNumber = Null.SetNullString(dr["MeetingNumber"]),
                MeetingTime = Encryptor.DecryptString(Null.SetNullString(dr["MeetingTime"])),
                IsDelete = Null.SetNullString(dr["IsActive"]),
                ForumName = Encryptor.DecryptString(Null.SetNullString(dr["ForumName"])),
                AuditAction = Null.SetNullString(dr["AuditAction"])
            };
        }

        /// <summary>
        /// Get all Forum Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<UploadMinuteAuditDomain> ToUploadMinuteAuditDomains(IDataReader dr,out int totalCount)
        {
            IList<UploadMinuteAuditDomain> objUploadMinuteAuditDomains = new List<UploadMinuteAuditDomain>();
            int count = 0;
            while (dr.Read())
            {
                objUploadMinuteAuditDomains.Add(ToUploadMinuteAuditDomain(dr));
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    count = Null.SetNullInteger(dr["RecordCount"]);

                    //  MessageBox.Show(reader.GetString(0), "Table2.Column2");
                }
            }
            totalCount = count;
            return objUploadMinuteAuditDomains;
        }
        #endregion


        #region "Get"
        /// <summary>
        /// Get Agenda Audit by id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifying MeetingId</param>
        /// <returns></returns>
        public IList<AgendaAuditDomain> GetAgendaAuditByMeetingId(out int totalCount,Guid MeetingId, Guid ForumId, string SerialNumber, string Presenter, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {


            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaAuditByMeeting", MeetingId, ForumId, SerialNumber, Presenter, StartDate, EndDate, PageNo, PageSize);
            //return ds;
            IList<AgendaAuditDomain> objAgendaAuditDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaAuditByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_SerialNumber", SerialNumber);
            cmd.Parameters.AddWithValue("p_Presenter", Presenter);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaAuditByMeeting", MeetingId, ForumId, SerialNumber, Presenter, StartDate, EndDate, PageNo, PageSize))
            {
                objAgendaAuditDomain = ToAgendaAuditDomains(dataReader,out totalCount);
            }
            conn.Close();
            return objAgendaAuditDomain;
        }


        /// <summary>
        /// Get Agenda report by id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifying MeetingId</param>
        /// <returns></returns>
        public IList<AgendaReportDomain> GetAgendaReportByMeetingId(Guid MeetingId)
        {
            IList<AgendaReportDomain> objAgendaReportDomain = null;
            
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaReportByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAgendaReportByMeeting", MeetingId))
            {
                objAgendaReportDomain = ToAgendaReportDomains(dataReader);
            }
            conn.Close();
            return objAgendaReportDomain;
        }


        /// <summary>
        /// Get Agenda report by id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifying MeetingId</param>
        /// <returns></returns>
        public DataSet GetAgendaReportDataByMeetingId(Guid MeetingId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAgendaReportByMeeting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.Fill(ds);
            conn.Close();

            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetAgendaReportByMeeting", MeetingId);
            return ds;
        }

        /// <summary>
        /// Get Meeting Audit by id
        /// </summary>
        /// <param name="UserId">Guid sepcifying UserId</param>
        /// <returns></returns>
        public IList<MeetingAuditDomain> GetMeetingAuditByMeetingId(out int totalCount,Guid ForumId, Guid MeetingId, string Number, string MeetingType, DateTime StartDate, DateTime EndDate, out IList<NoticeAuditDomain> objNoticeAuditDomain, int PageNo = 1, int PageSize = 10)
        {
            IList<MeetingAuditDomain> objMeetingAuditDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetMeetingAuditById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_Number", Number);
            cmd.Parameters.AddWithValue("p_MeetingType", MeetingType);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetMeetingAuditById", ForumId,MeetingId,Number,MeetingType,StartDate,EndDate,PageNo,PageSize))
            {
                objMeetingAuditDomain = ToMeetingAuditDomains(dataReader, out totalCount, out objNoticeAuditDomain);
            }
            conn.Close();
            return objMeetingAuditDomain;
        }


        /// <summary>
        /// Get User Audit by id
        /// </summary>
        /// <param name="UserId">Guid sepcifying UserId</param>
        /// <returns></returns>
        public IList<UserAuditDomain> GetUserAuditByUserId(out int totalCount,out IList<UserDepartmentAuditDomain> objDepartment,Guid UserId, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            IList<UserAuditDomain> objUserAuditDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserAudit", UserId, StartDate, EndDate,PageNo,PageSize))
            {
                objUserAuditDomain = ToUserAuditDomains(dataReader, out totalCount, out objDepartment);
            }
            conn.Close();
            return objUserAuditDomain;
        }


        /// <summary>
        /// Get all User Audit 
        /// </summary>
        /// <returns></returns>
        public IList<UserAuditDomain> GetUserAudit(out int totalCount, out IList<UserDepartmentAuditDomain> objDepartment, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            IList<UserAuditDomain> objUserAuditDomain = null;
        
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUserAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserAudit", EntityId, StartDate, EndDate, PageNo, PageSize))
            {
                objUserAuditDomain = ToUserAuditDomains(dataReader, out totalCount,out objDepartment);
            }
            conn.Close();
            return objUserAuditDomain;
        }


        /// <summary>
        /// Get Forum Audit by id
        /// </summary>
        /// <param name="UserId">Guid sepcifying UserId</param>
        /// <returns></returns>
        public IList<ForumAccessAuditDomain> GetForumAccessAuditByUserId(out int totalCount,Guid UserId, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            IList<ForumAccessAuditDomain> objForumAccessAuditDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAuditForumAccessByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAuditForumAccessByUserId", UserId,StartDate,EndDate,PageNo,PageSize))
            {
                objForumAccessAuditDomain = ToForumAccessAuditDomains(dataReader, out totalCount);
            }
            conn.Close();
            return objForumAccessAuditDomain;
        }


        /// <summary>
        /// Get Forum Audit by id
        /// </summary>
        /// <param name="ForumId">Guid sepcifying ForumId</param>
        /// <returns></returns>
        public IList<ForumAuditDomain> GetForumAuditByForumId(out int totalCount,Guid ForumId,DateTime StartDate,DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            IList<ForumAuditDomain> objForumAccessAuditDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetForumAuditById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetForumAuditById", ForumId,StartDate,EndDate,PageNo,PageSize))
            {
                objForumAccessAuditDomain = ToForumAuditDomains(dataReader,out totalCount);
            }
            conn.Close();
            return objForumAccessAuditDomain;
        }


        /// <summary>
        /// Get Upoad minutes Audit by id
        /// </summary>
        /// <param name="MeetingId">Guid sepcifying MeetingId</param>
        /// <returns></returns>
        public IList<UploadMinuteAuditDomain> GetUploadMinutesAuditByMeetingId(out int totalCount, Guid ForumId, Guid MeetingId, DateTime StartDate, DateTime EndDate, int PageNo = 1, int PageSize = 10)
        {
            IList<UploadMinuteAuditDomain> objUploadMinuteAuditDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUplaodMinuteAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            cmd.Parameters.AddWithValue("p_StartDate", StartDate);
            cmd.Parameters.AddWithValue("p_EndDate", EndDate);
            cmd.Parameters.AddWithValue("p_PageNo", PageNo);
            cmd.Parameters.AddWithValue("p_PageSize", PageSize);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUplaodMinuteAudit", ForumId,MeetingId,StartDate,EndDate,PageNo,PageSize))
            {
                objUploadMinuteAuditDomain = ToUploadMinuteAuditDomains(dataReader,out totalCount);
            }
            conn.Close();
            return objUploadMinuteAuditDomain;
        }

        public string GetAgendaPdfNames(string UploadedAgendaNote, string DeletedAgenda, string AgendaNote)
        {
            StringBuilder sbAgenda = new StringBuilder("");
            string[] strDeletedAgenda = DeletedAgenda.Split(',');
            string[] agendaPdfs = AgendaNote.Split(',');
            if (UploadedAgendaNote.Length > 0)
            {
                string[] agendaNames = UploadedAgendaNote.Split(',');
                for (int i = 0; i <= agendaNames.Count() - 1; i++)
                {
                    if (!strDeletedAgenda.Contains(agendaNames[i].Trim()))
                    {
                        sbAgenda.Append(agendaPdfs[i].Trim() +", " );
                    }
                }
            }

            return sbAgenda.ToString();
        }

        public string GetName(string FullName)
        {
            string Name = "";
            string[] strName = FullName.Split(',');
            for (int i = 0; i <= strName.Count()-1; i++)
            {
                if (i == strName.Count() - 1)
                {
                    Name += string.IsNullOrWhiteSpace(MM.Core.Encryptor.DecryptString(strName[i]))? "": "("+MM.Core.Encryptor.DecryptString(strName[i]) + ")";
                }
                else
                {
                    Name += MM.Core.Encryptor.DecryptString(strName[i]) + " ";
                }
            }
            return Name;
        }


        
        #endregion
    }
}
