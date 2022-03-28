using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Domain;
using System.Data;
using MM.Core;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;
using System.Web;

namespace MM.Data
{
    public class UserDataProvider : DataProvider
    {
        #region "Singleton"

        private static volatile UserDataProvider _instance;
        private static object _synRoot = new Object();

        private UserDataProvider()
        {
        }

        public static UserDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion


        #region "Fill"
        /// <summary>
        /// Fill User Detail
        /// </summary>s
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private UserDomain ToUserDomain(IDataReader dr)
        {
            string MiddleName = "";
            ClsMain clsmain = new ClsMain();
            //mssql use
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MiddleName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)

            if (clsmain.checkColumExist("MiddleName", dr))
            {
                MiddleName = Encryptor.DecryptString(Null.SetNullString(dr["MiddleName"]));
            }

            string PassportNumber = "";

            //mssql use
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "PassportNumber");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)

            if (clsmain.checkColumExist("PassportNumber", dr))
            {
                PassportNumber = Encryptor.DecryptString(Null.SetNullString(dr["PassportNumber"]));
            }

            string RollName = "";

            //mssql use
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "RollName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)

            if (clsmain.checkColumExist("RollName", dr))
            {
                RollName = Encryptor.DecryptString(Null.SetNullString(dr["RollName"]));
            }

            bool IsLocked = false;

            //mssql use
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "IsLocked");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            if (clsmain.checkColumExist("IsLocked", dr))
            {
                IsLocked = Null.SetNullBoolean(dr["IsLocked"]);
            }

            bool IsSuperAdmin = false;

            //mssql use
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "IsSuperAdmin");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)

            if (clsmain.checkColumExist("IsSuperAdmin", dr))
            {
                IsSuperAdmin = Null.SetNullBoolean(dr["IsSuperAdmin"]);
            }

            string LoginRequest = string.Empty;
            if (clsmain.checkColumExist("LoginRequest", dr))
            {
                LoginRequest = Null.SetNullString(dr["LoginRequest"]);
            }

            string EntityIds = string.Empty;
            if (clsmain.checkColumExist("EntityIds", dr))
            {
                EntityIds = Null.SetNullString(dr["EntityIds"]);
            }

            string EntityNames = string.Empty;
            if (clsmain.checkColumExist("EntityNames", dr))
            {
                EntityNames = Null.SetNullString(dr["EntityNames"]);
            }

            return new UserDomain()
            {
                UserId = Null.SetNullGuid(dr["UserId"]),
                FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])),
                //   Email = Null.SetNullString(dr["Email"]),
                UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"])),
                Password = Encryptor.DecryptString(Null.SetNullString(dr["Password"])),
                //  ContactNo = Null.SetNullString(dr["ContactNo"]),
                PANNo = Encryptor.DecryptString(Null.SetNullString(dr["PANNo"])),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),

                Photograph = Encryptor.DecryptString(Null.SetNullString(dr["Photograph"])),
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                OfficeAddress = Encryptor.DecryptString(Null.SetNullString(dr["OfficeAddress"])),
                ResidentialAddress = Encryptor.DecryptString(Null.SetNullString(dr["ResidentialAddress"])),
                DINNumber = Encryptor.DecryptString(Null.SetNullString(dr["DINNumber"])),

                OfficePhone = Encryptor.DecryptString(Null.SetNullString(dr["OfficePhone"])),
                ResidencePhone = Encryptor.DecryptString(Null.SetNullString(dr["ResidencePhone"])),
                Mobile = Encryptor.DecryptString(Null.SetNullString(dr["Moblie"])),
                EmailID1 = Encryptor.DecryptString(Null.SetNullString(dr["EmailID1"])),
                EmailID2 = Encryptor.DecryptString(Null.SetNullString(dr["EmailID2"])),
                SecretaryName = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryName"])),
                SecretaryOfficePhone = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryOfficePhone"])),
                SecretaryResidentalPhone = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryResidentalPhone"])),
                SecretaryMobile = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryMobile"])),
                SecretaryEmailID1 = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryEmailID1"])),
                SecretaryEmailID2 = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryEmailID2"])),
                Designation = Encryptor.DecryptString(Null.SetNullString(dr["Designation"])),
                IsMaker = Null.SetNullBoolean(dr["IsMaker"]),
                IsChecker = Null.SetNullBoolean(dr["IsChecker"]),
                IsEnabledOnIpad = Null.SetNullBoolean(dr["EnabledOnIpad"]),
                IsEnabledOnWebApp = Null.SetNullBoolean(dr["EnabledOnWeb"]),
                UserChecker = Null.SetNullGuid(dr["UserChecker"]),
                Suffix = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])),
                MiddleName = MiddleName,
                PassportNumber = PassportNumber,
                RollName = RollName,
                IsLocked = IsLocked,
                IsSuperAdmin = IsSuperAdmin,
                LoginRequest = LoginRequest,
                EntityIds = EntityIds,
                EntityNames = EntityNames
            };
        }

        private UserDomain ToUserDomainWithUdId(IDataReader dr)
        {
            ClsMain clsmain = new ClsMain();
            string MiddleName = "";
            if (clsmain.checkColumExist("MiddleName", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "MiddleName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                MiddleName = Encryptor.DecryptString(Null.SetNullString(dr["MiddleName"]));
            }

            string PassportNumber = "";
            if (clsmain.checkColumExist("PassportNumber", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "PassportNumber");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                PassportNumber = Encryptor.DecryptString(Null.SetNullString(dr["PassportNumber"]));
            }

            string RollName = "";
            if (clsmain.checkColumExist("RollName", dr))
            //dr.GetSchemaTable().DefaultView.RowFilter = string.Format("ColumnName= '{0}'", "RollName");
            //if (dr.GetSchemaTable().DefaultView.Count > 0)
            {
                RollName = Encryptor.DecryptString(Null.SetNullString(dr["RollName"]));
            }

            return new UserDomain()
            {
                UserId = Null.SetNullGuid(dr["UserId"]),
                FirstName = Encryptor.DecryptString(Null.SetNullString(dr["FirstName"])),
                LastName = Encryptor.DecryptString(Null.SetNullString(dr["LastName"])),
                //   Email = Null.SetNullString(dr["Email"]),
                UserName = Encryptor.DecryptString(Null.SetNullString(dr["UserName"])),
                Password = Encryptor.DecryptString(Null.SetNullString(dr["Password"])),
                //  ContactNo = Null.SetNullString(dr["ContactNo"]),
                PANNo = Encryptor.DecryptString(Null.SetNullString(dr["PANNo"])),
                CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
                CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
                UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
                UpdatedOn = Null.SetNullDateTime(dr["UpdatedOn"]),

                Photograph = Encryptor.DecryptString(Null.SetNullString(dr["Photograph"])),
                EntityId = Null.SetNullGuid(dr["EntityId"]),
                OfficeAddress = Encryptor.DecryptString(Null.SetNullString(dr["OfficeAddress"])),
                ResidentialAddress = Encryptor.DecryptString(Null.SetNullString(dr["ResidentialAddress"])),
                DINNumber = Encryptor.DecryptString(Null.SetNullString(dr["DINNumber"])),

                OfficePhone = Encryptor.DecryptString(Null.SetNullString(dr["OfficePhone"])),
                ResidencePhone = Encryptor.DecryptString(Null.SetNullString(dr["ResidencePhone"])),
                Mobile = Encryptor.DecryptString(Null.SetNullString(dr["Moblie"])),
                EmailID1 = Encryptor.DecryptString(Null.SetNullString(dr["EmailID1"])),
                EmailID2 = Encryptor.DecryptString(Null.SetNullString(dr["EmailID2"])),
                SecretaryName = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryName"])),
                SecretaryOfficePhone = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryOfficePhone"])),
                SecretaryResidentalPhone = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryResidentalPhone"])),
                SecretaryMobile = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryMobile"])),
                SecretaryEmailID1 = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryEmailID1"])),
                SecretaryEmailID2 = Encryptor.DecryptString(Null.SetNullString(dr["SecretaryEmailID2"])),
                Designation = Encryptor.DecryptString(Null.SetNullString(dr["Designation"])),
                IsMaker = Null.SetNullBoolean(dr["IsMaker"]),
                IsChecker = Null.SetNullBoolean(dr["IsChecker"]),
                IsEnabledOnIpad = Null.SetNullBoolean(dr["EnabledOnIpad"]),
                IsEnabledOnWebApp = Null.SetNullBoolean(dr["EnabledOnWeb"]),
                UserChecker = Null.SetNullGuid(dr["UserChecker"]),
                Suffix = Encryptor.DecryptString(Null.SetNullString(dr["Suffix"])),
                DeviceToken = Null.SetNullString(dr["DeviceToken"]),
                MiddleName = MiddleName,
                PassportNumber = PassportNumber,
                RollName = RollName
            };
        }


        /// <summary>
        /// Get list of Users wit UdID
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UserDomain> ToUserDomainsWithUdID(IDataReader dr)
        {
            IList<UserDomain> user = new List<UserDomain>();
            while (dr.Read())
            {
                user.Add(ToUserDomainWithUdId(dr));
            }
            return user;
        }


        /// <summary>
        /// Get list of Users
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private IList<UserDomain> ToUserDomains(IDataReader dr)
        {
            IList<UserDomain> user = new List<UserDomain>();
            while (dr.Read())
            {
                user.Add(ToUserDomain(dr));
            }
            return user;
        }
        #endregion

        #region "Get"

        /// <summary>
        /// Get User Detail
        /// </summary>
        /// <param name="UserDomainId">Guid specifying UserDomainId</param>
        /// <returns></returns>      
        public UserDomain Get(Guid UserDomainId)
        {
            UserDomain UserDomain = null;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserDetailsById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserDomainId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserDetailsById", UserDomainId))
            {
                if (!dataReader.Read())
                    return null;
                UserDomain = ToUserDomain(dataReader);
            }
            conn.Close();
            return UserDomain;
        }
        /// <summary>
        /// Get User Domains Details
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> Get()
        {
            IList<UserDomain> UserDomains = null;
            ///Commnet on 29 03 2014 for get only checkres by entityId
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetails"))           
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUserDetailsWithRollByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetailsWithRollByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        /// <summary>
        /// Get User Domains Details by entity
        /// </summary>
        /// <returns></returns>
        //public IList<UserDomain> GetByEntity(Guid EntityId)
        //{
        //    IList<UserDomain> UserDomains = null;
        //    ///Commnet on 29 03 2014 for get only checkres by entityId
        //    Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());
        //    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetailsByEntity", EntityId))           
        //    {
        //        UserDomains = ToUserDomains(dataReader);
        //    }
        //    return UserDomains;
        //}

        /// <summary>
        /// Get All Checker details 
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllChecker()
        {
            IList<UserDomain> UserDomains = null;
            UserDomain objUser = new UserDomain();
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllCkeckerByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllCkecker"))       
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllCkeckerByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        /// <summary>
        /// Get All Checker details 
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllChecker(Guid ForumId)
        {
            IList<UserDomain> UserDomains = null;
            UserDomain objUser = new UserDomain();
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllCkeckerByEntityANdForum", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //  using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllCkecker"))       
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllCkeckerByEntityANdForum", EntityId, ForumId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        /// <summary>
        /// Get All Checker details by Entity
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllCheckerByEntity(Guid EntityId)
        {
            IList<UserDomain> UserDomains = null;
            UserDomain objUser = new UserDomain();

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllCkeckerByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllCkeckerByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        ///// <summary>
        ///// For Authorized Login
        ///// </summary>
        ///// <param name="UserDomainName"></param>
        ///// <param name="UserDomainPassword"></param>
        ///// <returns></returns>
        //public int GetLogin(string UserDomainName, string UserDomainPassword)
        //{

        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "sp_GetAuthoriziedUserDomainName", UserDomainName, UserDomainPassword));

        //}

        /// <summary>
        /// Get User Password By User-Name
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="QuestionId">Guid specifying QuestionId</param>
        /// <param name="Answer">string specifying Answer</param>
        /// <returns></returns>
        public UserDomain GetPassword(string UserName, Guid QuestionId, string Answer)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserpassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_QuestionId", QuestionId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //  UserDomain objadmin = new UserDomain();
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserpassword", Encryptor.EncryptString(UserName), QuestionId))
            {
                UserDomain objUser = new UserDomain();
                if (!dataReader.Read())
                    return null;
                else
                {
                    string DbAnswer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"]));
                    if (DbAnswer.ToLower().Equals(Answer.ToLower()))
                    {
                        return new UserDomain
                        {

                            Password = Encryptor.DecryptString(Null.SetNullString(dataReader["Password"])),
                            EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"])),

                        };
                    }

                }


                // objadmin = ToUserDomain(dataReader);

            }
            conn.Close();
            return null;

        }

        /// <summary>
        /// Get User Password By User-Name
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <returns></returns>
        public DataTable GetPasswordByUsername(string UserName)
        {
            //  UserDomain objadmin = new UserDomain();
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserpasswordByUsername", Encryptor.EncryptString(UserName)))
            //{
            //    UserDomain objUser = new UserDomain();
            //    if (!dataReader.Read())
            //        return null;
            //    else
            //    {
            //        return new UserDomain
            //        {
            //            Password = Encryptor.DecryptString(Null.SetNullString(dataReader["Password"])),
            //            EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"])),
            //            Designation = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"])) //Answer
            //        };
            //    }

            //}

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserpasswordByUsername", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);


            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUserpasswordByUsername", Encryptor.EncryptString(UserName));
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            conn.Close();
            return null;

        }

        /// <summary>
        /// Get User Password By User-Name
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <returns></returns>
        public DataTable GetPasswordByUsername_Ipad(string UserName, string MobileNo)
        {
            //  UserDomain objadmin = new UserDomain();
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserpasswordByUsername", Encryptor.EncryptString(UserName)))
            //{
            //    UserDomain objUser = new UserDomain();
            //    if (!dataReader.Read())
            //        return null;
            //    else
            //    {
            //        return new UserDomain
            //        {
            //            Password = Encryptor.DecryptString(Null.SetNullString(dataReader["Password"])),
            //            EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"])),
            //            Designation = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"])) //Answer
            //        };
            //    }

            //}

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserpasswordByUsername_IPad", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_MobileNo", Encryptor.EncryptString(MobileNo));

            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);


            //            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUserpasswordByUsername", Encryptor.EncryptString(UserName));
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            conn.Close();
            return null;

        }

        /// <summary>
        /// Change Password Functionality
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="OldPass">string specifying OldPass</param>
        /// <param name="NewPass">string specifying NewPass</param>
        /// <returns></returns>
        public bool ChangePassword(Guid UserId, string OldPass, string NewPass)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UserChangePassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_OldPassWord", Encryptor.EncryptString(OldPass));
            cmd.Parameters.AddWithValue("p_NewPassword", Encryptor.EncryptString(NewPass));
            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UserChangePassword", UserId, Encryptor.EncryptString(OldPass), Encryptor.EncryptString(NewPass))) > 0;
        }



        /// <summary>
        /// Change Password Functionality
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="OldPass">string specifying OldPass</param>
        /// <param name="NewPass">string specifying NewPass</param>
        /// <returns></returns>
        public bool ChangePasswordWeb(Guid UserId, string OldPass, string NewPass)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UserChangePasswordWeb", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_OldPassWord", Encryptor.EncryptString(OldPass));
            cmd.Parameters.AddWithValue("p_NewPassword", Encryptor.EncryptString(NewPass));
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UserChangePasswordWeb", UserId, Encryptor.EncryptString(OldPass), Encryptor.EncryptString(NewPass))) > 0;
        }

        /// <summary>
        /// top password
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OldPass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        public DataTable ChangePasswordWebNew(Guid UserId, string NewPass)
        {
            DataTable dt = new DataTable();
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UserChangePasswordWebNew", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            //cmd.Parameters.AddWithValue("p_OldPassWord", Encryptor.EncryptString(OldPass));
            cmd.Parameters.AddWithValue("p_NewPassword", Encryptor.EncryptString(NewPass));
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            conn.Close();

            //DataSet ds = new DataSet();
            //MySqlDataAdapter da = new MySqlDataAdapter();
            //da.SelectCommand = cmd;
            //da.Fill(ds);

            ////DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUserByActivationCode", Guid.Parse(activationtoken.ToString()));// Encryptor.EncryptString(UserName));
            //if (ds.Tables.Count > 0)
            //{
            //    return ds.Tables[0];
            //}
            //conn.Close();

            return dt;
        }

        /// <summary>
        /// Validate email address
        /// </summary>
        /// <param name="UserDomainEmail">string specifying UserDomainEmail</param>
        /// <returns></returns>
        public bool ValidateEmail(string UserDomainEmail)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("sp_CheckUserDomainEMail", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserDomainEmail.Trim());
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "sp_CheckUserDomainEMail", UserDomainEmail.Trim())) == 0;
        }

        /// <summary>
        /// get user list only for super admin
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetUserListForSuperAdmin()
        {
            IList<UserDomain> UserDomains = null;
            ///Commnet on 29 03 2014 for get only checkres by entityId
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetails"))           
            //Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUserDetailsForSuperAdmin", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetailsWithRollByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }
        #endregion

        #region "Modify"

        /// <summary>
        /// Insert User Detail
        /// </summary>
        /// <param name="UserDomain">Class object specifying UserDomain</param>
        /// <param name="Entity">Guid specifying Entity</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public UserDomain Insert(UserDomain UserDomain, string Entity, Guid UserId, string SendToChecker)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = "UserId";
            p.DbType = DbType.Guid;
            p.Direction = ParameterDirection.Output;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertUserDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_FirstName", Encryptor.EncryptString(UserDomain.FirstName));
            cmd.Parameters.AddWithValue("p_LastName", Encryptor.EncryptString(UserDomain.LastName));
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserDomain.UserName));
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(UserDomain.Password));
            cmd.Parameters.AddWithValue("p_PANNo", Encryptor.EncryptString(UserDomain.PANNo));
            cmd.Parameters.AddWithValue("p_CreatedBy", Guid.Parse(UserDomain.CreatedBy.ToString()));
            cmd.Parameters.AddWithValue("p_UpdatedBy", Guid.Parse(UserDomain.UpdatedBy.ToString()));
            cmd.Parameters.AddWithValue("p_IsActive", true);
            cmd.Parameters.AddWithValue("p_Photograph", Encryptor.EncryptString(UserDomain.Photograph));
            cmd.Parameters.AddWithValue("p_OfficeAddress", Encryptor.EncryptString(UserDomain.OfficeAddress));
            cmd.Parameters.AddWithValue("p_ResidentialAddress", Encryptor.EncryptString(UserDomain.ResidentialAddress));
            cmd.Parameters.AddWithValue("p_DINNumber", Encryptor.EncryptString(UserDomain.DINNumber));
            cmd.Parameters.AddWithValue("p_OfficePhone", Encryptor.EncryptString(UserDomain.OfficePhone));
            cmd.Parameters.AddWithValue("p_ResidencePhone", Encryptor.EncryptString(UserDomain.ResidencePhone));
            cmd.Parameters.AddWithValue("p_Moblie", Encryptor.EncryptString(UserDomain.Mobile));
            cmd.Parameters.AddWithValue("p_EmailID1", Encryptor.EncryptString(UserDomain.EmailID1));
            cmd.Parameters.AddWithValue("p_EmailID2", Encryptor.EncryptString(UserDomain.EmailID2));
            cmd.Parameters.AddWithValue("p_SecretaryName", Encryptor.EncryptString(UserDomain.SecretaryName));
            cmd.Parameters.AddWithValue("p_SecretaryResidentalPhone", Encryptor.EncryptString(UserDomain.SecretaryResidentalPhone));
            cmd.Parameters.AddWithValue("p_SecretaryOfficePhone", Encryptor.EncryptString(UserDomain.SecretaryOfficePhone));
            cmd.Parameters.AddWithValue("p_SecretaryMobile", Encryptor.EncryptString(UserDomain.SecretaryMobile));
            cmd.Parameters.AddWithValue("p_SecretaryEmailID1", Encryptor.EncryptString(UserDomain.SecretaryEmailID1));
            cmd.Parameters.AddWithValue("p_SecretaryEmailID2", Encryptor.EncryptString(UserDomain.SecretaryEmailID2));
            cmd.Parameters.AddWithValue("p_Designation", Encryptor.EncryptString(UserDomain.Designation));
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_Entity", Entity);
            cmd.Parameters.AddWithValue("p_IsMaker", UserDomain.IsMaker);
            cmd.Parameters.AddWithValue("p_IsChecker", UserDomain.IsChecker);
            cmd.Parameters.AddWithValue("p_EnabledOnIpad", UserDomain.IsEnabledOnIpad);
            cmd.Parameters.AddWithValue("p_EnabledOnWebApp", UserDomain.IsEnabledOnWebApp);
            cmd.Parameters.AddWithValue("p_UserChecker", UserDomain.UserChecker);
            cmd.Parameters.AddWithValue("p_Suffix", Encryptor.EncryptString(UserDomain.Suffix));
            cmd.Parameters.AddWithValue("p_SendToChecker", SendToChecker);
            cmd.Parameters.AddWithValue("p_MiddleName", Encryptor.EncryptString(UserDomain.MiddleName));
            cmd.Parameters.AddWithValue("p_MoblieNo", UserDomain.Mobile);
            cmd.Parameters.AddWithValue("p_EmailId", UserDomain.EmailID1);
            cmd.Parameters.AddWithValue("p_PassportNumber", Encryptor.EncryptString(UserDomain.PassportNumber));
            cmd.Parameters.AddWithValue("p_IsSuperAdmin", UserDomain.IsSuperAdmin);

            UserDomain.UserId = (Guid)(cmd.ExecuteScalar());
            conn.Close();


            //UserDomain.FoodPreference,
            //           UserDomain.UserId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertUserDetails", Encryptor.EncryptString(UserDomain.FirstName), Encryptor.EncryptString(UserDomain.LastName),
            //               Encryptor.EncryptString(UserDomain.UserName), Encryptor.EncryptString(UserDomain.Password), Encryptor.EncryptString(UserDomain.PANNo), Guid.Parse(UserDomain.CreatedBy.ToString()), Guid.Parse(UserDomain.UpdatedBy.ToString()), true, Encryptor.EncryptString(UserDomain.Photograph), Encryptor.EncryptString(UserDomain.OfficeAddress), Encryptor.EncryptString(UserDomain.ResidentialAddress), Encryptor.EncryptString(UserDomain.DINNumber), Encryptor.EncryptString(UserDomain.OfficePhone), Encryptor.EncryptString(UserDomain.ResidencePhone), Encryptor.EncryptString(UserDomain.Mobile), Encryptor.EncryptString(UserDomain.EmailID1), Encryptor.EncryptString(UserDomain.EmailID2), Encryptor.EncryptString(UserDomain.SecretaryName), Encryptor.EncryptString(UserDomain.SecretaryResidentalPhone), Encryptor.EncryptString(UserDomain.SecretaryOfficePhone), Encryptor.EncryptString(UserDomain.SecretaryMobile), Encryptor.EncryptString(UserDomain.SecretaryEmailID1), Encryptor.EncryptString(UserDomain.SecretaryEmailID2), Encryptor.EncryptString(UserDomain.Designation), UserId,
            //Entity, UserDomain.IsMaker, UserDomain.IsChecker, UserDomain.IsEnabledOnIpad, UserDomain.IsEnabledOnWebApp, UserDomain.UserChecker, Encryptor.EncryptString(UserDomain.Suffix), SendToChecker, Encryptor.EncryptString(UserDomain.MiddleName), UserDomain.Mobile, UserDomain.EmailID1, Encryptor.EncryptString(UserDomain.PassportNumber), UserDomain.IsSuperAdmin);

            return UserDomain;
        }
        /// <summary>
        /// Delete User By ID
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool Delete(Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteUserById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteUserById", UserId)) > 0;
        }

        /// <summary>
        /// Delete seleted User by id
        /// </summary>
        /// <param name="UserIds">string specifying UserIds</param>
        /// <returns></returns>
        public bool DeleteSelected(string UserIds)
        {
            bool res = false;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteSelectedUserById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserIds", UserIds);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteSelectedUserById", UserIds)) > 0;
        }
        /// <summary>
        ///  Update User Details
        /// </summary>
        /// <param name="UserDomain">Class object specifying UserDomain</param>
        /// <param name="EntityIds">Guid specifying EntityIds</param>
        /// <returns></returns>
        public bool Update(UserDomain UserDomain, string EntityIds, string strDeleteEntityIds, string SendToChecker)
        {
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateUserById", UserDomain.UserId, UserDomain.FirstName, UserDomain.LastName, UserDomain.UserName, UserDomain.Password, UserDomain.PANNo, UserDomain.UpdatedBy, UserDomain.EntityId, UserDomain.DINNumber, UserDomain.Photograph, UserDomain.EmailID1, UserDomain.EmailID2, UserDomain.Mobile, UserDomain.OfficeAddress, UserDomain.OfficePhone, UserDomain.ResidencePhone, UserDomain.ResidentialAddress, UserDomain.SecretaryEmailID1, UserDomain.SecretaryEmailID2, UserDomain.SecretaryMobile, UserDomain.SecretaryName, UserDomain.SecretaryOfficePhone, UserDomain.SecretaryResidentalPhone, UserDomain.Designation)) > 0;

            string str = Encryptor.EncryptString(UserDomain.PANNo);
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateUserById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_FirstName", Encryptor.EncryptString(UserDomain.FirstName));
            cmd.Parameters.AddWithValue("p_LastName", Encryptor.EncryptString(UserDomain.LastName));
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserDomain.UserName));
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(UserDomain.Password));
            cmd.Parameters.AddWithValue("p_UpdatedBy", Guid.Parse(UserDomain.UpdatedBy.ToString()));
            cmd.Parameters.AddWithValue("p_Photograph", Encryptor.EncryptString(UserDomain.Photograph));
            cmd.Parameters.AddWithValue("p_OfficeAddress", Encryptor.EncryptString(UserDomain.OfficeAddress));
            cmd.Parameters.AddWithValue("p_ResidentialAddress", Encryptor.EncryptString(UserDomain.ResidentialAddress));
            cmd.Parameters.AddWithValue("p_DINNumber", Encryptor.EncryptString(UserDomain.DINNumber));
            cmd.Parameters.AddWithValue("p_PANNo", Encryptor.EncryptString(UserDomain.PANNo));
            cmd.Parameters.AddWithValue("p_OfficePhone", Encryptor.EncryptString(UserDomain.OfficePhone));
            cmd.Parameters.AddWithValue("p_ResidencePhone", Encryptor.EncryptString(UserDomain.ResidencePhone));
            cmd.Parameters.AddWithValue("p_Moblie", Encryptor.EncryptString(UserDomain.Mobile));
            cmd.Parameters.AddWithValue("p_EmailID1", Encryptor.EncryptString(UserDomain.EmailID1));
            cmd.Parameters.AddWithValue("p_EmailID2", Encryptor.EncryptString(UserDomain.EmailID2));
            cmd.Parameters.AddWithValue("p_SecretaryName", Encryptor.EncryptString(UserDomain.SecretaryName));
            cmd.Parameters.AddWithValue("p_SecretaryResidentalPhone", Encryptor.EncryptString(UserDomain.SecretaryResidentalPhone));
            cmd.Parameters.AddWithValue("p_SecretaryOfficePhone", Encryptor.EncryptString(UserDomain.SecretaryOfficePhone));
            cmd.Parameters.AddWithValue("p_SecretaryMobile", Encryptor.EncryptString(UserDomain.SecretaryMobile));
            cmd.Parameters.AddWithValue("p_SecretaryEmailID1", Encryptor.EncryptString(UserDomain.SecretaryEmailID1));
            cmd.Parameters.AddWithValue("p_SecretaryEmailID2", Encryptor.EncryptString(UserDomain.SecretaryEmailID2));
            cmd.Parameters.AddWithValue("p_Designation", Encryptor.EncryptString(UserDomain.Designation));
            cmd.Parameters.AddWithValue("p_UserId", UserDomain.UserId);
            cmd.Parameters.AddWithValue("p_Entity", EntityIds);
            cmd.Parameters.AddWithValue("p_DeleteEntity", strDeleteEntityIds);
            cmd.Parameters.AddWithValue("p_IsMaker", UserDomain.IsMaker);
            cmd.Parameters.AddWithValue("p_IsChecker", UserDomain.IsChecker);
            cmd.Parameters.AddWithValue("p_IsEnabledOnIpad", UserDomain.IsEnabledOnIpad);
            cmd.Parameters.AddWithValue("p_IsEnabledOnWebApp", UserDomain.IsEnabledOnWebApp);
            cmd.Parameters.AddWithValue("p_UserChecker", UserDomain.UserChecker);
            cmd.Parameters.AddWithValue("p_Suffix", Encryptor.EncryptString(UserDomain.Suffix));
            cmd.Parameters.AddWithValue("p_SendToChecker", SendToChecker);
            cmd.Parameters.AddWithValue("p_MiddleName", Encryptor.EncryptString(UserDomain.MiddleName));
            cmd.Parameters.AddWithValue("p_MobileNo", UserDomain.Mobile);
            cmd.Parameters.AddWithValue("p_EmailId", UserDomain.EmailID1);
            cmd.Parameters.AddWithValue("p_PassportNumber", Encryptor.EncryptString(UserDomain.PassportNumber));
            cmd.Parameters.AddWithValue("p_IsSuperAdmin", UserDomain.IsSuperAdmin);

            int a = Convert.ToInt16(cmd.ExecuteNonQuery());

            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //UserDomain.FoodPreference,UserDomain.FoodPreference,
            //           return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateUserById",
            //                Encryptor.EncryptString(UserDomain.FirstName), Encryptor.EncryptString(UserDomain.LastName),
            //               Encryptor.EncryptString(UserDomain.UserName), Encryptor.EncryptString(UserDomain.Password), Guid.Parse(UserDomain.UpdatedBy.ToString()), Encryptor.EncryptString(UserDomain.Photograph), Encryptor.EncryptString(UserDomain.OfficeAddress), Encryptor.EncryptString(UserDomain.ResidentialAddress), Encryptor.EncryptString(UserDomain.DINNumber), Encryptor.EncryptString(UserDomain.PANNo), Encryptor.EncryptString(UserDomain.OfficePhone), Encryptor.EncryptString(UserDomain.ResidencePhone), Encryptor.EncryptString(UserDomain.Mobile), Encryptor.EncryptString(UserDomain.EmailID1), Encryptor.EncryptString(UserDomain.EmailID2), Encryptor.EncryptString(UserDomain.SecretaryName), Encryptor.EncryptString(UserDomain.SecretaryResidentalPhone), Encryptor.EncryptString(UserDomain.SecretaryOfficePhone), Encryptor.EncryptString(UserDomain.SecretaryMobile), Encryptor.EncryptString(UserDomain.SecretaryEmailID1), Encryptor.EncryptString(UserDomain.SecretaryEmailID2), Encryptor.EncryptString(UserDomain.Designation), UserDomain.UserId,
            //EntityIds, strDeleteEntityIds, UserDomain.IsMaker, UserDomain.IsChecker, UserDomain.IsEnabledOnIpad, UserDomain.IsEnabledOnWebApp, UserDomain.UserChecker, Encryptor.EncryptString(UserDomain.Suffix), SendToChecker, Encryptor.EncryptString(UserDomain.MiddleName), UserDomain.Mobile, UserDomain.EmailID1, Encryptor.EncryptString(UserDomain.PassportNumber), UserDomain.IsSuperAdmin)) > 0;
        }



        /// <summary>
        /// check User login 
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="Password">string specifying Password</param>
        /// <returns></returns>
        public UserDomain UserLogin(string UserName, string Password)
        {
            UserDomain objUserDomain = null;
            // string str = Encryptor.EncryptString(UserName);
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UserLogin", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(Password));
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //mssql reader
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserLogin", Encryptor.EncryptString(UserName), Encryptor.EncryptString(Password)))
            {
                if (!dataReader.Read())
                    return null;
                objUserDomain = ToUserDomain(dataReader);

                objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);

            }
            conn.Close();
            return objUserDomain;
        }

        public UserDomain UserLoginDownload(string UserName, string Password)
        {
            UserDomain objUserDomain = null;
            // string str = Encryptor.EncryptString(UserName);
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UserLoginDownload", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(Password));
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //mssql reader
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserLogin", Encryptor.EncryptString(UserName), Encryptor.EncryptString(Password)))
            {
                if (!dataReader.Read())
                    return null;
                else
                {
                    objUserDomain = new UserDomain();
                    objUserDomain.UserId = Null.SetNullGuid(dataReader["UserId"]);
                    objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(dataReader["FirstName"]));
                    objUserDomain.LastName = Encryptor.DecryptString(Null.SetNullString(dataReader["LastName"]));
                    objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                    objUserDomain.SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]);
                    objUserDomain.Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"]));
                    objUserDomain.Token = Null.SetNullString(dataReader["Token"]);
                    objUserDomain.IsEnabledOnIpad = Null.SetNullBoolean(dataReader["EnabledOnIpad"]);
                    objUserDomain.Suffix = Encryptor.DecryptString(Null.SetNullString(dataReader["Suffix"]));

                    //Key encryption
                    objUserDomain.EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dataReader["Encryption Key"]));

                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);

                    objUserDomain.OTP = Null.SetNullString(dataReader["OTP"]);

                    //Session timeout
                    objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);
                    objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(dataReader["Mobile"]));

                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);
                    //objUserDomain.OTPPassword = Null.SetNullString(dataReader["OTPPassword"]);
                    objUserDomain.EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"]));
                }

            }
            conn.Close();
            return objUserDomain;
        }

        public UserDomain ResendOTP(Guid UserId)
        {
            UserDomain objUserDomain = null;
            // string str = Encryptor.EncryptString(UserName);
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_ResendOTP", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //mssql reader
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserLogin", Encryptor.EncryptString(UserName), Encryptor.EncryptString(Password)))
            {
                if (!dataReader.Read())
                    return null;
                else
                {
                    objUserDomain = new UserDomain();
                    objUserDomain.UserId = Null.SetNullGuid(dataReader["UserId"]);
                    objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(dataReader["FirstName"]));
                    objUserDomain.LastName = Encryptor.DecryptString(Null.SetNullString(dataReader["LastName"]));
                    objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                    objUserDomain.SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]);
                    objUserDomain.Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"]));
                    objUserDomain.Token = Null.SetNullString(dataReader["Token"]);
                    objUserDomain.IsEnabledOnIpad = Null.SetNullBoolean(dataReader["EnabledOnIpad"]);
                    objUserDomain.Suffix = Encryptor.DecryptString(Null.SetNullString(dataReader["Suffix"]));

                    //Key encryption
                    objUserDomain.EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dataReader["Encryption Key"]));

                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);

                    objUserDomain.OTP = Null.SetNullString(dataReader["OTP"]);

                    //Session timeout
                    objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);
                    objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(dataReader["Mobile"]));

                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);
                    //objUserDomain.OTPPassword = Null.SetNullString(dataReader["OTPPassword"]);
                    objUserDomain.EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"]));
                }

            }
            conn.Close();
            return objUserDomain;
        }

        public bool iPadUserOTPStatus(Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateOTPStatusById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeleteUserById", UserId)) > 0;
        }

        /// <summary>
        /// Get User by UserName
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <returns></returns>
        public UserDomain GetUserByUserName(string UserName)
        {
            UserDomain objUserDomain = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByUserName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByUserName", Encryptor.EncryptString(UserName)))
            {
                if (!dataReader.Read())
                    return null;
                objUserDomain = ToUserDomain(dataReader);

                objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);
            }
            conn.Close();

            return objUserDomain;
        }
        /// <summary>
        /// check User ipad login 
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="Password">string specifying Password</param>
        /// <returns></returns>
        public UserDomain UserLoginIpad(string UserName, string Password, string UdId, string DeviceToken, Guid UserId)
        {
            UserDomain objUserDomain = null;
            string str = Encryptor.EncryptString(UserName);

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UserLogin_Ipad", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(Password));
            cmd.Parameters.AddWithValue("p_UdId", UdId);
            cmd.Parameters.AddWithValue("p_DeviceToken", DeviceToken);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserLogin_Ipad", Encryptor.EncryptString(UserName), Encryptor.EncryptString(Password), UdId, DeviceToken, UserId))
            {
                if (!dataReader.Read())
                    return null;
                else
                {
                    objUserDomain = new UserDomain();
                    objUserDomain.UserId = Null.SetNullGuid(dataReader["UserId"]);
                    objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(dataReader["FirstName"]));
                    objUserDomain.LastName = Encryptor.DecryptString(Null.SetNullString(dataReader["LastName"]));
                    objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                    objUserDomain.SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]);
                    objUserDomain.Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"]));
                    objUserDomain.Token = Null.SetNullString(dataReader["Token"]);
                    objUserDomain.IsEnabledOnIpad = Null.SetNullBoolean(dataReader["EnabledOnIpad"]);
                    objUserDomain.Suffix = Encryptor.DecryptString(Null.SetNullString(dataReader["Suffix"]));

                    //Key encryption
                    objUserDomain.EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dataReader["Encryption Key"]));

                    //Session timeout
                    objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);
                    objUserDomain.IsDeviceLost = Null.SetNullBoolean(dataReader["IsLost"]);
                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);
                    objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(dataReader["Mobile"]));
                    objUserDomain.OTP = Null.SetNullString(dataReader["OTP"]);

                    //OTP Status
                    //objUserDomain.SentOTP = Null.SetNullBoolean(dataReader["SentOTP"]);

                    objUserDomain.EmailID1 = Encryptor.DecryptString(Null.SetNullString(dataReader["EmailID1"]));
                    objUserDomain.IsLocked = Null.SetNullBoolean(dataReader["IsLocked"]);

                }

            }
            conn.Close();
            return objUserDomain;
        }

        /// <summary>
        /// Get User Details for Ipda
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="UdId">string specifying Password</param>
        /// <returns></returns>
        public UserDomain UserLoginIpadByUserName(string UserName, string UdId, string DeviceToken)
        {
            UserDomain objUserDomain = null;
            string str = Encryptor.EncryptString(UserName);

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserForIpad", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_udid", UdId);
            cmd.Parameters.AddWithValue("p_devicetoken", DeviceToken);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForIpad", Encryptor.EncryptString(UserName), UdId, DeviceToken))
            {
                if (!dataReader.Read())
                    return null;
                else
                {
                    objUserDomain = new UserDomain();
                    objUserDomain.UserId = Null.SetNullGuid(dataReader["UserId"]);
                    objUserDomain.FirstName = Encryptor.DecryptString(Null.SetNullString(dataReader["FirstName"]));
                    objUserDomain.LastName = Encryptor.DecryptString(Null.SetNullString(dataReader["LastName"]));
                    objUserDomain.IsAuthorized = Null.SetNullBoolean(dataReader["IsAuthorized"]);
                    objUserDomain.SecurityQuestionId = Null.SetNullGuid(dataReader["SecurityQuestionId"]);
                    objUserDomain.Answer = Encryptor.DecryptString(Null.SetNullString(dataReader["Answer"]));
                    objUserDomain.Token = Null.SetNullString(dataReader["Token"]);
                    objUserDomain.IsEnabledOnIpad = Null.SetNullBoolean(dataReader["EnabledOnIpad"]);
                    objUserDomain.Suffix = Encryptor.DecryptString(Null.SetNullString(dataReader["Suffix"]));

                    //Key encryption
                    objUserDomain.EncryptionKey = Encryptor.DecryptString(Null.SetNullString(dataReader["Encryption Key"]));

                    objUserDomain.OTPId = Null.SetNullGuid(dataReader["OTPId"]);

                    objUserDomain.OTP = Null.SetNullString(dataReader["OTP"]);

                    //Session timeout
                    objUserDomain.SessionTimeout = Null.SetNullInteger(dataReader["Session Time Out"]);
                    objUserDomain.IsDeviceLost = Null.SetNullBoolean(dataReader["IsLost"]);
                    objUserDomain.Mobile = Encryptor.DecryptString(Null.SetNullString(dataReader["Mobile"]));
                }

            }
            conn.Close();
            return objUserDomain;
        }

        /// <summary>
        /// Get user having access rights
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserByAccessRight()
        {

            IList<UserDomain> UserDomains = null;

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByAccessRight"))
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UserByAccessRightByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByAccessRightByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        /// <summary>
        ///Get All User For Attendance
        /// </summary>
        /// <param name="MeetingId"></param>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForAttendace(Guid MeetingId)
        {
            IList<UserDomain> UserDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByForumAccessForUserAttendance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_MeetingId", MeetingId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByForumAccessForUserAttendance", MeetingId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;

        }
        /// <summary>
        /// Get user having access rights by entity
        /// </summary>
        /// <returns></returns>
        //public IList<UserDomain> GetAllUserByAccessRightByEntity(Guid EntityId)
        //{

        //    IList<UserDomain> UserDomains = null;

        //    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByAccessRightByEntity", EntityId))
        //    {
        //        UserDomains = ToUserDomains(dataReader);
        //    }
        //    return UserDomains;
        //}

        /// <summary>
        /// Get user having forum access rights
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserByForumAccess()
        {

            IList<UserDomain> UserDomains = null;
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByForumAccess"))
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_UserByForumAccessByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByForumAccessByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }


        /// <summary>
        /// Get All User for Access Right
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForAccessRight()
        {
            IList<UserDomain> UserDomains = null;
            //Commented for Relience
            //   using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForAccessRight"))
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserForAccessRightByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForAccessRightByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;

        }

        /// <summary>
        /// Get All User for Access Right by Entity
        /// </summary>
        /// <returns></returns>
        //public IList<UserDomain> GetAllUserForAccessRightByEntity(Guid EntityId)
        //{
        //    IList<UserDomain> UserDomains = null;
        //    //Commented for Relience

        //    using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForAccessRightByEntity", EntityId))
        //    {
        //        UserDomains = ToUserDomains(dataReader);
        //    }
        //    return UserDomains;

        //}
        /// <summary>
        /// Get All User for Reports
        /// </summary>
        /// <param name="ForumId">Guid specifying forumId</param>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForReport(Guid ForumId)
        {
            IList<UserDomain> UserDomains = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByForumAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();




            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByForumAccess", ForumId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;

        }



        /// <summary>
        /// Get All User for for ipad notifications
        /// </summary>
        /// <param name="ForumId">Guid specifying forumId</param>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForIpadNotification(Guid ForumId)
        {
            IList<UserDomain> UserDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByIpadNofication", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByIpadNofication", ForumId))
            {
                UserDomains = ToUserDomainsWithUdID(dataReader);
            }
            conn.Close();
            return UserDomains;

        }

        /// <summary>
        /// Get All User for for ipad notifications
        /// </summary>
        /// <param name="ForumId">Guid specifying forumId</param>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForEmailNotification(Guid ForumId)
        {
            IList<UserDomain> UserDomains = null;
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByEmailNofications", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ForumId", ForumId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserByEmailNofications", ForumId))
            {
                UserDomains = ToUserDomainsWithUdID(dataReader);
            }
            conn.Close();
            return UserDomains;

        }

        /// <summary>
        /// Get All User for user Forum  
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserFoUserForum()
        {
            IList<UserDomain> UserDomains = null;

            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForUserForum"))
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserForUserForumByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();



            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetUserForUserForumByEntity", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;

        }
        /// <summary>
        /// Get password for wcf services
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="QuestionId">Guid specifying QuestionId</param>
        /// <param name="Answer">string specifying Answer</param>
        /// <returns></returns>
        public string GetForgetPassword(string UserName, Guid QuestionId, string Answer)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPasswordForWCF", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_QuestionId", QuestionId);
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //DataSet ds = (DataSet)SqlHelper.ExecuteDataset(ConnectionString, "stp_GetPasswordForWCF", UserName, QuestionId);
            string password = "";
            if (ds.Tables.Count > 0)
            {
                string Ans = Encryptor.DecryptString(ds.Tables[0].Rows[0]["Answer"].ToString());
                if (Ans.ToLower().Equals(Answer.ToLower()))
                {
                    password = Encryptor.DecryptString(ds.Tables[0].Rows[0]["Password"].ToString());
                }
            }
            conn.Close();
            return password;
        }

        /// <summary>
        /// Check emailid existance
        /// </summary>
        /// <param name="strEmail">string specifying strEmail</param>
        /// <returns></returns>
        public int CheckEmailExistance(string strEmail, Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_CheckEmailIdExistance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Email", Encryptor.EncryptString(strEmail));
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //return (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_CheckEmailIdExistance", Encryptor.EncryptString(strEmail), UserId);
        }


        /// <summary>
        /// Check Username existance
        /// </summary>
        /// <param name="strEmail">string specifying strEmail</param>
        /// <returns></returns>
        public int CheckUserNameExistance(string UserName, Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_CheckUserNameExitance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //return (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_CheckUserNameExitance", Encryptor.EncryptString(UserName), UserId);
        }


        public int CheckUserIdDepartmentExitance(string UserName, Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_CheckUserIdDepartmentExitance", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //return (int)SqlHelper.ExecuteScalar(ConnectionString, "stp_CheckUserNameExitance", Encryptor.EncryptString(UserName), UserId);
        }

        /// <summary>
        /// Insert or update device token
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <returns></returns>
        public bool InsertDeviceToken(string DeviceToken, Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertDeviceToken", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_DeviceToken", DeviceToken);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_InsertDeviceToken", DeviceToken, UserId)) > 0;
        }

        /// <summary>
        /// Insert user device log in database
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <param name="Mac">String specifying mac address</param>
        /// <param name="DeviceTime">String specifying time on device</param>
        /// <param name="OperationName">string specifying operation name</param>
        ///    /// <param name="Response">String specifying response</param>
        /// <param name="EntityId">string specifying entityid </param>
        /// <param name="Mapper">guid specifying mapper</param>
        /// <returns></returns>
        public bool InsertDeviceLog_old(string DeviceToken, Guid UserId, string Mac, string DeviceTime, string OperationName, Guid EntityId, string Request, string Response, Guid Mapper, string AppVersion)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertDeviceLog", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_DeviceTime", DeviceTime);
            cmd.Parameters.AddWithValue("p_OperationName", OperationName);
            cmd.Parameters.AddWithValue("p_Mac", Mac);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_DeviceToken", DeviceToken);
            cmd.Parameters.AddWithValue("p_Request", Request);
            cmd.Parameters.AddWithValue("p_Response", Response);
            cmd.Parameters.AddWithValue("p_Mapper", Mapper);
            cmd.Parameters.AddWithValue("p_AppVersion", AppVersion);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_InsertDeviceLog", UserId, DeviceTime, OperationName, Mac, EntityId, DeviceToken, Request, Response, Mapper, AppVersion)) > 0;
        }

        /// <summary>
        /// Insert user device log in database
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <param name="Mac">String specifying mac address</param>
        /// <param name="DeviceTime">String specifying time on device</param>
        /// <param name="OperationName">string specifying operation name</param>
        /// <param name="Response">String specifying response</param>
        /// <param name="EntityId">string specifying entityid </param>
        /// <param name="Mapper">guid specifying mapper</param>
        /// <returns></returns>
        public DataSet InsertDeviceLog(string DeviceToken, Guid UserId, string Mac, string DeviceTime, string OperationName, Guid EntityId, string Request, string Response, Guid Mapper, string AppVersion, string RequestToken, string IsExpired, Guid LoginUserId, string UserIpAddress, string UserName)
        {
            //            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertDeviceLog_New", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_DeviceTime", (object)(string.IsNullOrEmpty(DeviceTime) ? string.Empty : DeviceTime));
            cmd.Parameters.AddWithValue("p_OperationName", OperationName);
            cmd.Parameters.AddWithValue("p_Mac", (object)(string.IsNullOrEmpty(Mac) ? string.Empty : Mac));
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            cmd.Parameters.AddWithValue("p_UdId", (object)(string.IsNullOrEmpty(DeviceToken) ? string.Empty : DeviceToken));
            cmd.Parameters.AddWithValue("p_Request", Request);
            cmd.Parameters.AddWithValue("p_Response", Response);
            cmd.Parameters.AddWithValue("p_Mapper", Mapper);
            cmd.Parameters.AddWithValue("p_AppVersion", (object)(string.IsNullOrEmpty(AppVersion) ? string.Empty : AppVersion));
            cmd.Parameters.AddWithValue("p_RequestToken", (object)(string.IsNullOrEmpty(RequestToken) ? string.Empty : RequestToken));
            cmd.Parameters.AddWithValue("p_IsExpired", 0);
            cmd.Parameters.AddWithValue("p_LoginUserId", LoginUserId);
            cmd.Parameters.AddWithValue("p_UserIpAddress", UserIpAddress);
            cmd.Parameters.AddWithValue("p_UserName", UserName);

            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            return ds;

            //return SqlHelper.ExecuteDataset(ConnectionString, "stp_InsertDeviceLog_New", UserId, DeviceTime, OperationName, Mac, EntityId, DeviceToken, Request, Response, Mapper, AppVersion, RequestToken, IsExpired, LoginUserId);
        }


        /// <summary>
        /// Update device log 
        /// </summary>
        /// <param name="Response">String specifying response</param>
        /// <param name="Mapper">guid specifying mapper</param>
        /// <returns></returns>
        public bool UpdateDeviceLog(string Response, Guid Mapper, string AccessToken, string IsExpired)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateDeviceLog", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_AccessToken", AccessToken);
            cmd.Parameters.AddWithValue("p_IsExpired", IsExpired);
            cmd.Parameters.AddWithValue("p_Response", Response);
            cmd.Parameters.AddWithValue("p_Mapper", Mapper);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateDeviceLog", Response, Mapper, AccessToken, IsExpired)) > 0;
        }

        /// <summary>
        /// Get all ipad users
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllIpadUser()
        {
            IList<UserDomain> UserDomains = null;
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_UserByForumAccess"))
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetIpadUsers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetIpadUsers", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        /// <summary>
        /// Get user devices by userid
        /// </summary>
        /// <param name="UserId">string specifying userid</param>
        /// <returns></returns>
        public DataSet GetUserDevices(string UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_getUserDevices", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", UserId);
            conn.Open();
            //MySqlDataReader dataReader = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            //DataSet ds = (DataSet)SqlHelper.ExecuteDataset(ConnectionString, "stp_getUserDevices", UserId);
            return ds;
        }

        /// <summary>
        /// Insert lost device infromation in database
        /// </summary>
        /// <param name="UdId">string specifying udid</param>
        /// <returns></returns>
        public Int32 InsertLostDevice(string UdId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertLostDevices", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UdId", UdId);
            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;

            // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertLostDevices", UdId));
        }

        /// <summary>
        /// Insert lost device infromation in database
        /// </summary>
        /// <param name="UserId">string specifying udid</param>
        /// <returns></returns>
        public Int32 InsertDeviceLostUser(Guid UserId)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertDeviceLostUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int res = Convert.ToInt32(cmd.ExecuteScalar());

            conn.Close();
            return res;
            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_InsertDeviceLostUser", UserId));
        }
        /// <summary>
        /// set Data delete flag in database
        /// </summary>
        /// <param name="UdId">string specifying udid</param>
        /// <returns></returns>
        public bool DataClean(string UdId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateLostDeviceStatus", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UdId", UdId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateLostDeviceStatus", UdId)) > 0;
        }

        /// <summary>
        /// Get users lost device infromation in database
        /// </summary>
        /// <param name="UdId">string specifying UserId</param>
        /// <returns></returns>
        public int IsUserLostDevice(Guid UserId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_GetLostDeviceUsers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);

            int res = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            return res;
            //  return (int)(SqlHelper.ExecuteScalar(ConnectionString, "stp_GetLostDeviceUsers", UserId));
        }

        /// <summary>
        /// Get Get Count Of Users And Directors With Roll By Entity by EntityId
        /// </summary>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public DataSet GetCntOfUserAndDirectorsWithRollByEntity(Guid EntityId)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetCntOfUserAndDirectorsWithRollByEntity", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_EntityId", EntityId);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.SelectCommand = cmd;
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public DataSet LockAccount(string Username)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_LockUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(Username));
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds); conn.Close();
            return ds;


            //return SqlHelper.ExecuteDataset(ConnectionString, "stp_LockUser", Encryptor.EncryptString(Username));
        }

        /// <summary>
        /// Unlock User by id
        /// </summary>
        /// <param name="Username">Guid specifying username</param>
        /// <returns></returns>
        public bool UnlockAccount(Guid UserId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UnlockUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a == 1)
            {
                res = true;
            }
            conn.Close();
            //bool res =Convert.ToBoolean(cmd.ExecuteScalar());
            //return Convert.ToBoolean(cmd.ExecuteScalar());
            return res;
            // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UnlockUser", UserId)) > 0;
        }


        /// <summary>
        /// Update Lost device user
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool UpdateDeviceLostUser(Guid UserId)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateDeviceLostUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;

            //return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_UpdateDeviceLostUser", UserId)) > 0;
        }

        /// <summary>
        /// VerifyOTP
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="TimeOut">int specifying TimeOut</param>
        /// <param name="OTP">string specifying OTP</param>
        /// <param name="OTPId">Guid specifying OTPId</param>
        /// <returns></returns>
        public bool VerifyOTP(Guid UserId, int TimeOut, string OTP, Guid OTPId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_VerifyOtp", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Time", TimeOut);
            cmd.Parameters.AddWithValue("p_OTPId", OTPId);
            cmd.Parameters.AddWithValue("p_OTP", OTP);
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;



            // return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, "stp_VerifyOtp", TimeOut,OTPId,OTP,UserId)) > 0;
        }
        public IList<UserDomain> GetAuditUser()
        {
            IList<UserDomain> UserDomains = null;
            ///Commnet on 29 03 2014 for get only checkres by entityId
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetails"))           
            Guid EntityId = Guid.Parse(System.Web.HttpContext.Current.Session["EntityId"].ToString());

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllUserDetailsByEntityForAudit", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();


            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllUserDetailsByEntityForAudit", EntityId))
            {
                UserDomains = ToUserDomains(dataReader);
            }
            conn.Close();
            return UserDomains;
        }

        public bool UpdatePasswordResetLink(Guid ActivationToken)
        {
            DateTime getdate = DateTime.Now.AddDays(-1);
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateForgotPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_ActivationCode", ActivationToken);
            cmd.Parameters.AddWithValue("p_PasswordResetDate", getdate);
            int a = Convert.ToInt32(cmd.ExecuteScalar());
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateForgotPassword", ActivationToken, getdate)) > 0;
        }


        public DataTable GetUserByActivationCode(Guid activationtoken)
        {
            //DataSet ds = SqlHelper.ExecuteDataset()

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_GetUserByActivationCode", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_activationtoken", activationtoken);
            //int a = Convert.ToInt32(cmd.ExecuteScalar());
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            //DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, "stp_GetUserByActivationCode", Guid.Parse(activationtoken.ToString()));// Encryptor.EncryptString(UserName));
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            conn.Close();
            return null;

        }


        public bool UpdateByEmailLink(UserDomain UserDomain)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateUserByIdEmailLink", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Password", Encryptor.EncryptString(UserDomain.Password));
            cmd.Parameters.AddWithValue("p_UserId", UserDomain.UserId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", UserDomain.UpdatedBy);
            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;


            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdateUserByIdEmailLink",
            //    Encryptor.EncryptString(UserDomain.Password), UserDomain.UserId, Guid.Parse(UserDomain.UpdatedBy.ToString()))) > 0;
        }


        public UserDomain InsertPasswordResetLink(UserDomain userDomain)
        {
            // string EncryptionKey = System.Web.HttpContext.Current.Session["EncryptionKey"].ToString();
            //string EmailKey = MM.Extended.Encryptor.KeyValue;
            //Random random = new Random();
            //EmailKey = EmailKey + random.Next(1000, 9999).ToString();
            DateTime getdate = DateTime.Now.AddDays(1);

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertForgotPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserID", userDomain.UserId);
            cmd.Parameters.AddWithValue("p_ActivationCode", userDomain.ActivationCode);
            cmd.Parameters.AddWithValue("p_PasswordResetDate", getdate);
            //userDomain.UserId = (Guid)(cmd.ExecuteScalar());
            cmd.ExecuteScalar();
            conn.Close();
            //userDomain.UserId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertForgotPassword", userDomain.UserId, userDomain.ActivationCode, getdate);
            return userDomain;

        }

        /// <summary>
        /// iPad User lock
        /// </summary>
        /// <param name="DeviceToken"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool iPadUserLock(string UserName)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_LockUseriPad", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }

        /// <summary>
        /// Get User Password by UserName and Mobile no
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="MobileNo"></param>
        /// <returns></returns>
        public DataTable GetPasswordByUsernameAndMobile(string UserName, string MobileNo)
        {

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetUserpasswordByUsernameAndMobile", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));
            cmd.Parameters.AddWithValue("p_MobileNo", Encryptor.EncryptString(MobileNo));
            conn.Open();
            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            conn.Close();
            return null;

        }

        /// <summary>
        /// login and logout logs
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="OperationName"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool InsertWebLog(string UserName, string OperationName, string Status, string Id, string Reason)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string strURL = context.Request.Url.ToString();
            string strForm = context.Request.Form.ToString();
            string strQueryString = context.Request.QueryString.ToString();
            string strServerName = context.Request.ServerVariables["SERVER_NAME"];
            string strUserAgent = context.Request.UserAgent;
            string strUserIP = GetUserIP(); //context.Request.UserHostAddress;
            string strUserHostName = context.Request.UserHostName;
            string UserPFId = string.Empty;
            if (HttpContext.Current.Session["UserPFId"] != null)
            {
                UserPFId = Convert.ToString(HttpContext.Current.Session["UserPFId"].ToString());
            }
            else
            {
                UserPFId = UserName;
            }

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertWebLogs", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", UserPFId);
            cmd.Parameters.AddWithValue("p_OperationName", OperationName);
            cmd.Parameters.AddWithValue("p_Status", Status);
            cmd.Parameters.AddWithValue("p_Url", strURL);
            cmd.Parameters.AddWithValue("p_Form", strForm);
            cmd.Parameters.AddWithValue("p_QueryString", strQueryString);
            cmd.Parameters.AddWithValue("p_ServerName", strServerName);
            cmd.Parameters.AddWithValue("p_UserAgent", strUserAgent);
            cmd.Parameters.AddWithValue("p_UserIP", strUserIP);
            cmd.Parameters.AddWithValue("p_UserHostName", strUserHostName);
            cmd.Parameters.AddWithValue("p_ActivityId", Id);
            cmd.Parameters.AddWithValue("p_Reason", Reason);
            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;

        }

        private static string GetUserIP()
        {
            string visitorsIPAddress = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                visitorsIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                string[] splitIP = visitorsIPAddress.Split(',');
                if (splitIP[0] != "")
                {
                    visitorsIPAddress = splitIP[0].ToString();
                }
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                visitorsIPAddress = HttpContext.Current.Request.UserHostAddress;
            }
            return visitorsIPAddress;
        }

        /// <summary>
        /// update login request
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UpdateLoginRequest(Guid UserId, string LoginRequest)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdateLoginRequest", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_LoginRequest", LoginRequest);
            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }

        /// <summary>
        /// check user exists or not
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable iPadUserExists(string UserName)
        {
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_GetUserExists", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserName", Encryptor.EncryptString(UserName));

            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            conn.Close();
            return null;
        }

        public bool DeleteEntityByUserEntity(Guid UserId, Guid EntityId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeleteEntityByUserId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);

            int a = cmd.ExecuteNonQuery();
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }



        public static string PostSMS(string URL, string data, string authInfo)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL);

            httpRequest.Method = "POST";

            httpRequest.Accept = "application/x-www-form-urlencoded";

            httpRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] bytedata = Encoding.UTF8.GetBytes(data);

            Stream requestStream = httpRequest.GetRequestStream();

            requestStream.Write(bytedata, 0, bytedata.Length);

            requestStream.Close();

            //httpRequest.Headers.Add("Authorization", authInfo);
            httpRequest.Headers.Add("Authorization", "Basic " + authInfo);

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            string resulXmlFromWebService = string.Empty;

            using (StreamReader srd = new StreamReader(httpResponse.GetResponseStream()))
            {
                resulXmlFromWebService = srd.ReadToEnd();
            }

            return resulXmlFromWebService;

        }

        /// <summary>
        /// device lost change password
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="OldPass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        public bool DeviceLostChangePassword(Guid UserId, string NewPass)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UserDeviceLostChangePassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_UserId", UserId);
            cmd.Parameters.AddWithValue("p_NewPassword", Encryptor.EncryptString(NewPass));
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;
        }

        #endregion
    }
}
