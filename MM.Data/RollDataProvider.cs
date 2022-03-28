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
    public class RollDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile RollDataProvider _instance;
        private static object synRoot = new Object();

        private RollDataProvider()
        {

        }

        public static RollDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new RollDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Get Rolls Details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private RollDomain ToRollDomain(IDataReader dr)
        {
            bool Section = false;
            ClsMain clsmain = new ClsMain();
            if (clsmain.checkColumExist("SectionMaster", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SectionMaster");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                Section = Null.SetNullBoolean(dr["SectionMaster"]);
            }

            bool HeaderMaster = false;
            if (clsmain.checkColumExist("HeaderMaster", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "HeaderMaster");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                HeaderMaster = Null.SetNullBoolean(dr["HeaderMaster"]);
            }

            bool AgendaMarker = false;
            if (clsmain.checkColumExist("AgendaMarker", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "AgendaMarker");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                AgendaMarker = Null.SetNullBoolean(dr["AgendaMarker"]);
            }


            bool SerialNoMaster = false;
            if (clsmain.checkColumExist("SerialNoMaster", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "SerialNoMaster");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                SerialNoMaster = Null.SetNullBoolean(dr["SerialNoMaster"]);
            }

            bool TabSetting = false;
            if (clsmain.checkColumExist("TabSetting", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "TabSetting");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                TabSetting = Null.SetNullBoolean(dr["TabSetting"]);
            }

            bool ProceedingMaster = false;
            if (clsmain.checkColumExist("ProceedingMaster", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "ProceedingMaster");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                ProceedingMaster = Null.SetNullBoolean(dr["ProceedingMaster"]);
            }

            bool ProceedingView = false;
            if (clsmain.checkColumExist("ProceedingView", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "ProceedingView");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                ProceedingView = Null.SetNullBoolean(dr["ProceedingView"]);
            }

            bool AttendanceView = false;
            if (clsmain.checkColumExist("AttendanceView", dr))
            //    dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "AttendanceView");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                AttendanceView = Null.SetNullBoolean(dr["AttendanceView"]);
            }

            bool MeetingRetention = false;
            if (clsmain.checkColumExist("MeetingRetention", dr))
            {
                MeetingRetention = Null.SetNullBoolean(dr["MeetingRetention"]);
            }

            return new RollDomain
            {
                RollMasterId = Null.SetNullGuid(dr["RollMasterId"]),
                AccessRightMaster = Null.SetNullBoolean(dr["AccessRightMaster"]),
                ApprovalMaster = Null.SetNullBoolean(dr["ApprovalMaster"]),
                EntityMaster = Null.SetNullBoolean(dr["EntityMaster"]),
                Approval = Null.SetNullBoolean(dr["Approval"]),
                Report = Null.SetNullBoolean(dr["Report"]),
                RollName = Encryptor.DecryptString(Null.SetNullString(dr["RollName"])),
                Transaction = Null.SetNullBoolean(dr["Transaction"]),
                UserMaster = Null.SetNullBoolean(dr["UserMaster"]),
                View = Null.SetNullBoolean(dr["View"]),

                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),

                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),

                RollMaster = Null.SetNullBoolean(dr["RollMaster"]),

                Agenda = Null.SetNullBoolean(dr["Agenda"]),
                AgendaView = Null.SetNullBoolean(dr["AgendaView"]),
                BoardofDirectore = Null.SetNullBoolean(dr["BoardofDirectore"]),
                EmailNotification = Null.SetNullBoolean(dr["EmailNotification"]),
                GlobalSetting = Null.SetNullBoolean(dr["GlobalSetting"]),
                IpadNotification = Null.SetNullBoolean(dr["IpadNotification"]),
                MasterAgenda = Null.SetNullBoolean(dr["MasterAgenda"]),
                Meeting = Null.SetNullBoolean(dr["Meeting"]),
                MeetingView = Null.SetNullBoolean(dr["MeetingView"]),
                MinutesView = Null.SetNullBoolean(dr["MinutesView"]),
                PhotoGallery = Null.SetNullBoolean(dr["PhotoGallery"]),

                UploadMinutes = Null.SetNullBoolean(dr["UploadMinutes"]),
                NoticeView = Null.SetNullBoolean(dr["NoticeView"]),
                RevisedPdf = Null.SetNullBoolean(dr["RevisedPdf"]),

                AboutUs = Null.SetNullBoolean(dr["AboutUs"]),
                MeetingSchedule = Null.SetNullBoolean(dr["MeetingSchedule"]),
                MasterClassification = Null.SetNullBoolean(dr["MasterClassification"]),
                DraftMOM = Null.SetNullBoolean(dr["DraftMOM"]),
                DeviceManager = Null.SetNullBoolean(dr["DeviceManager"]),
                PublishHistory = Null.SetNullBoolean(dr["PublishHistory"]),
                EmailHistory = Null.SetNullBoolean(dr["EmailHistory"]),
                SectionMaster = Section,
                HeaderMaster = HeaderMaster,
                AgendaMarker = AgendaMarker,

                AttendanceView = AttendanceView,
                ProceedingMaster = ProceedingMaster,
                SerialNoMaster = SerialNoMaster,
                ProceedingView = ProceedingView,
                TabSetting = TabSetting,
                MeetingRetention = MeetingRetention
                //UserId = Null.SetNullGuid(dr["AccessRight"]),
                //FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                //LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])), 
            };
        }
        /// <summary>
        /// Get All Roll details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<RollDomain> ToRollDomains(IDataReader dr)
        {
            IList<RollDomain> objRollDomain = new List<RollDomain>();
            while (dr.Read())
            {
                objRollDomain.Add(ToRollDomain(dr));
            }
            return objRollDomain;
        }
        #endregion

        #region "Get"
        /// <summary>
        /// Get Roll By Id
        /// </summary>
        /// <param name="RollMasterId">Guid specifying RollMasterId</param>
        /// <returns></returns>
        public RollDomain GetRollById(Guid RollMasterId)
        {
            RollDomain objRollDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetRollById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_RollMasterId", RollMasterId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //   using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetRollById", RollMasterId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objRollDomain = ToRollDomain(dataReader);
            }
            conn.Close();
            return objRollDomain;
        }

        /// <summary>
        /// Get  all Roll domain
        /// </summary>
        /// <returns></returns>
        public IList<RollDomain> Get()
        {
            IList<RollDomain> objRollDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllRolls", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllRolls", EntityId))
            {
                objRollDomains = ToRollDomains(dataReader);
            }
            conn.Close();
            return objRollDomains;
        }

        /// <summary>
        /// Get Roll by UserID 
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public RollDomain GetRollByUserId(Guid UserId)
        {
            RollDomain objRollDomain = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetRollByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetRollByUserId", UserId, EntityId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objRollDomain = ToRollDomain(dataReader);
            }
            conn.Close();
            return objRollDomain;
        }
        #endregion

        #region "Modify"

        /// <summary>
        /// Insert Roll details
        /// </summary>
        /// <param name="objRollDomain">Class object specifying objRollDomain</param>
        /// <returns></returns>
        public RollDomain Insert(RollDomain objRollDomain)
        {
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertRoll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_RollName", Encryptor.EncryptString(objRollDomain.RollName));
            cmd.Parameters.AddWithValue("p_EntityMaster", objRollDomain.EntityMaster);
            cmd.Parameters.AddWithValue("p_UserMaster", objRollDomain.UserMaster);
            cmd.Parameters.AddWithValue("p_AccessRightMaster", objRollDomain.AccessRightMaster);
            cmd.Parameters.AddWithValue("p_ApprovalMaster", objRollDomain.ApprovalMaster);
            cmd.Parameters.AddWithValue("p_Transaction", objRollDomain.Transaction);
            cmd.Parameters.AddWithValue("p_Approval", objRollDomain.Approval);
            cmd.Parameters.AddWithValue("p_View", objRollDomain.View);
            cmd.Parameters.AddWithValue("p_Report", objRollDomain.Report);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objRollDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_CreatedBy", objRollDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_RollMaster", objRollDomain.RollMaster);
            cmd.Parameters.AddWithValue("p_Meeting", objRollDomain.Meeting);
            cmd.Parameters.AddWithValue("p_Agenda", objRollDomain.Agenda);
            cmd.Parameters.AddWithValue("p_UploadMinutes", objRollDomain.UploadMinutes);
            cmd.Parameters.AddWithValue("p_MasterAgenda", objRollDomain.MasterAgenda);
            cmd.Parameters.AddWithValue("p_BoardofDirectore", objRollDomain.BoardofDirectore);
            cmd.Parameters.AddWithValue("p_PhotoGallery", objRollDomain.PhotoGallery);
            cmd.Parameters.AddWithValue("p_GlobalSetting", objRollDomain.GlobalSetting);
            cmd.Parameters.AddWithValue("p_IpadNotification", objRollDomain.IpadNotification);
            cmd.Parameters.AddWithValue("p_EmailNotification", objRollDomain.EmailNotification);
            cmd.Parameters.AddWithValue("p_MeetingView", objRollDomain.MeetingView);
            cmd.Parameters.AddWithValue("p_AgendaView", objRollDomain.AgendaView);
            cmd.Parameters.AddWithValue("p_MinutesView", objRollDomain.MinutesView);
            cmd.Parameters.AddWithValue("p_NoticeView", objRollDomain.NoticeView);
            cmd.Parameters.AddWithValue("p_RevisedPdf", objRollDomain.RevisedPdf);
            cmd.Parameters.AddWithValue("p_MeetingSchedule", objRollDomain.MeetingSchedule);
            cmd.Parameters.AddWithValue("p_AboutUs", objRollDomain.AboutUs);
            cmd.Parameters.AddWithValue("p_MasterClassification", objRollDomain.MasterClassification);
            cmd.Parameters.AddWithValue("p_DraftMOM", objRollDomain.DraftMOM);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_DeviceManager", objRollDomain.DeviceManager);
            cmd.Parameters.AddWithValue("p_PublishHistory", objRollDomain.PublishHistory);
            cmd.Parameters.AddWithValue("p_EmailHistory", objRollDomain.EmailHistory);
            cmd.Parameters.AddWithValue("p_SectionMaster", objRollDomain.SectionMaster);
            cmd.Parameters.AddWithValue("p_HeaderMaster", objRollDomain.HeaderMaster);
            cmd.Parameters.AddWithValue("p_AgendaMarker", objRollDomain.AgendaMarker);
            cmd.Parameters.AddWithValue("p_SerialNoMaster", objRollDomain.SerialNoMaster);
            cmd.Parameters.AddWithValue("p_TabSetting", objRollDomain.TabSetting);
            cmd.Parameters.AddWithValue("p_ProceedingMaster", objRollDomain.ProceedingMaster);
            cmd.Parameters.AddWithValue("p_ProceedingView", objRollDomain.ProceedingView);
            cmd.Parameters.AddWithValue("p_AttendanceView", objRollDomain.AttendanceView);
            cmd.Parameters.AddWithValue("p_MeetingRetention", objRollDomain.MeetingRetention);

            objRollDomain.RollMasterId = (Guid)(cmd.ExecuteScalar());

            //objRollDomain.RollMasterId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertRoll", Encryptor.EncryptString(objRollDomain.RollName), objRollDomain.EntityMaster, objRollDomain.UserMaster, objRollDomain.AccessRightMaster, objRollDomain.ApprovalMaster, objRollDomain.Transaction, objRollDomain.Approval, objRollDomain.View, objRollDomain.Report, objRollDomain.CreatedBy, objRollDomain.UpdatedBy, objRollDomain.RollMaster, objRollDomain.Meeting, objRollDomain.Agenda, objRollDomain.UploadMinutes, objRollDomain.MasterAgenda, objRollDomain.BoardofDirectore, objRollDomain.PhotoGallery, objRollDomain.GlobalSetting, objRollDomain.IpadNotification, objRollDomain.EmailNotification, objRollDomain.MeetingView, objRollDomain.AgendaView, objRollDomain.MinutesView, objRollDomain.NoticeView, objRollDomain.RevisedPdf, objRollDomain.MeetingSchedule, objRollDomain.AboutUs, objRollDomain.MasterClassification, objRollDomain.DraftMOM, EntityId, objRollDomain.DeviceManager, objRollDomain.PublishHistory, objRollDomain.EmailHistory, objRollDomain.SectionMaster, objRollDomain.HeaderMaster, objRollDomain.AgendaMarker,objRollDomain.SerialNoMaster,objRollDomain.TabSetting, objRollDomain.ProceedingMaster, objRollDomain.ProceedingView,objRollDomain.AttendanceView );
            conn.Close();
            return objRollDomain;
        }

        /// <summary>
        /// Update Roll Details
        /// </summary>
        /// <param name="objRollDomain">Class object specifying objRollDomain</param>
        /// <returns></returns>
        public bool Update(RollDomain objRollDomain)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateRoll", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_RollName", Encryptor.EncryptString(objRollDomain.RollName));
            cmd.Parameters.AddWithValue("p_EntityMaster", objRollDomain.EntityMaster);
            cmd.Parameters.AddWithValue("p_UserMaster", objRollDomain.UserMaster);
            cmd.Parameters.AddWithValue("p_AccessRightMaster", objRollDomain.AccessRightMaster);
            cmd.Parameters.AddWithValue("p_ApprovalMaster", objRollDomain.ApprovalMaster);
            cmd.Parameters.AddWithValue("p_Transaction", objRollDomain.Transaction);
            cmd.Parameters.AddWithValue("p_Approval", objRollDomain.Approval);
            cmd.Parameters.AddWithValue("p_View", objRollDomain.View);
            cmd.Parameters.AddWithValue("p_Report", objRollDomain.Report);
            cmd.Parameters.AddWithValue("p_RollMasterId", objRollDomain.RollMasterId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", objRollDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_RollMaster", objRollDomain.RollMaster);
            cmd.Parameters.AddWithValue("p_Meeting", objRollDomain.Meeting);
            cmd.Parameters.AddWithValue("p_Agenda", objRollDomain.Agenda);
            cmd.Parameters.AddWithValue("p_UploadMinutes", objRollDomain.UploadMinutes);
            cmd.Parameters.AddWithValue("p_MasterAgenda", objRollDomain.MasterAgenda);
            cmd.Parameters.AddWithValue("p_BoardofDirectore", objRollDomain.BoardofDirectore);
            cmd.Parameters.AddWithValue("p_PhotoGallery", objRollDomain.PhotoGallery);
            cmd.Parameters.AddWithValue("p_GlobalSetting", objRollDomain.GlobalSetting);
            cmd.Parameters.AddWithValue("p_IpadNotification", objRollDomain.IpadNotification);
            cmd.Parameters.AddWithValue("p_EmailNotification", objRollDomain.EmailNotification);
            cmd.Parameters.AddWithValue("p_MeetingView", objRollDomain.MeetingView);
            cmd.Parameters.AddWithValue("p_AgendaView", objRollDomain.AgendaView);
            cmd.Parameters.AddWithValue("p_MinutesView", objRollDomain.MinutesView);
            cmd.Parameters.AddWithValue("p_NoticeView", objRollDomain.NoticeView);
            cmd.Parameters.AddWithValue("p_RevisedPdf", objRollDomain.RevisedPdf);
            cmd.Parameters.AddWithValue("p_MeetingSchedule", objRollDomain.RevisedPdf);
            cmd.Parameters.AddWithValue("p_AboutUs", objRollDomain.AboutUs);
            cmd.Parameters.AddWithValue("p_MasterClassification", objRollDomain.MasterClassification);
            cmd.Parameters.AddWithValue("p_DraftMOM", objRollDomain.DraftMOM);
            cmd.Parameters.AddWithValue("p_DeviceManager", objRollDomain.DeviceManager);
            cmd.Parameters.AddWithValue("p_PublishHistory", objRollDomain.PublishHistory);
            cmd.Parameters.AddWithValue("p_EmailHistory", objRollDomain.EmailHistory);
            cmd.Parameters.AddWithValue("p_SectionMaster", objRollDomain.SectionMaster);
            cmd.Parameters.AddWithValue("p_HeaderMaster", objRollDomain.HeaderMaster);
            cmd.Parameters.AddWithValue("p_AgendaMarker", objRollDomain.AgendaMarker);
            cmd.Parameters.AddWithValue("p_SerialNoMaster", objRollDomain.SerialNoMaster);
            cmd.Parameters.AddWithValue("p_TabSetting", objRollDomain.TabSetting);
            cmd.Parameters.AddWithValue("p_ProceedingMaster", objRollDomain.ProceedingMaster);
            cmd.Parameters.AddWithValue("p_ProceedingView", objRollDomain.ProceedingView);
            cmd.Parameters.AddWithValue("p_AttendanceView", objRollDomain.AttendanceView);
            cmd.Parameters.AddWithValue("p_MeetingRetention", objRollDomain.MeetingRetention);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateRoll", Encryptor.EncryptString(objRollDomain.RollName), objRollDomain.EntityMaster, objRollDomain.UserMaster, objRollDomain.AccessRightMaster, objRollDomain.ApprovalMaster, objRollDomain.Transaction, objRollDomain.Approval, objRollDomain.View, objRollDomain.Report, objRollDomain.RollMasterId, objRollDomain.UpdatedBy, objRollDomain.RollMaster, objRollDomain.Meeting, objRollDomain.Agenda, objRollDomain.UploadMinutes, objRollDomain.MasterAgenda, objRollDomain.BoardofDirectore, objRollDomain.PhotoGallery, objRollDomain.GlobalSetting, objRollDomain.IpadNotification, objRollDomain.EmailNotification, objRollDomain.MeetingView, objRollDomain.AgendaView, objRollDomain.MinutesView, objRollDomain.NoticeView, objRollDomain.RevisedPdf, objRollDomain.MeetingSchedule, objRollDomain.AboutUs, objRollDomain.MasterClassification, objRollDomain.DraftMOM, objRollDomain.DeviceManager, objRollDomain.PublishHistory, objRollDomain.EmailHistory, objRollDomain.SectionMaster, objRollDomain.HeaderMaster, objRollDomain.AgendaMarker, objRollDomain.SerialNoMaster, objRollDomain.TabSetting, objRollDomain.ProceedingMaster, objRollDomain.ProceedingView, objRollDomain.AttendanceView
            //    )) > 0;
        }

        /// <summary>
        /// Delete Roll by Id
        /// </summary>
        /// <param name="RollMasterId">Class object specifying objRollDomain</param>
        /// <returns></returns>
        public bool Delete(Guid RollMasterId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteRollById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_RollMasterId", RollMasterId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteRollById", RollMasterId)) > 0;
        }

        /// <summary>
        /// Delete seleted Rolls by id
        /// </summary>
        /// <param name="RollMasterId">string specifying RollMasterId</param>
        /// <returns></returns>
        public bool DeleteSelected(string RollMasterId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedRolls", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_RollIds", RollMasterId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;




            // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSelectedRolls", RollMasterId)) > 0;
        }
        #endregion
    }
}
