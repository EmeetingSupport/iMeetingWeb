using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM.Data
{
    public class DbCOnvertDataProvider
    {
        public static string ConnectionString()
        {

            DataProvider dbProvider = new DataProvider();
            string con = dbProvider.ConnectionString;
            return con;
        }

        /// <summary>
        /// Get AboutUs Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAboutUsData()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select Id,EncryptionKey from AboutUs", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update AboutUs Data
        /// </summary>
        /// <param name="EncryptionKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateAboutUs(string EncryptionKey, int Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update AboutUS set EncryptionKey='" + EncryptionKey + "' where id=" + Id + "";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);
          

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get AgendaDetails Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAgendaDetails()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select AgendaDetailsId,AgendaKey,EncryptionKey from AgendaDetails where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }


        /// <summary>
        /// Update AgendaDetails Data
        /// </summary>
        /// <param name="AgendaKey"></param>
        /// <param name="EncryptionKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateAgendaDetails(string AgendaKey,string EncryptionKey, int Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update AgendaDetails set EncryptionKey='" + EncryptionKey + "',AgendaKey='"+ AgendaKey + "' where AgendaDetailsId=" + Id + "";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get Entity Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEntity()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select EntityId,EntityName,EntityShortName,EntityLogo,EntityMeeting,EncryptionKey from Entity where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update Entity Data
        /// </summary>
        /// <param name="EntityName"></param>
        /// <param name="EntityShortName"></param>
        /// <param name="EntityLogo"></param>
        /// <param name="EntityMeeting"></param>
        /// <param name="EncryptionKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateEntity(string EntityName,string EntityShortName,string EntityLogo,string EntityMeeting, string EncryptionKey, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update Entity set EntityName='"+EntityName+ "',EntityShortName='"+ EntityShortName + "',EntityLogo='"+ EntityLogo + "',EntityMeeting='"+ EntityMeeting + "', EncryptionKey='" + EncryptionKey + "' where EntityId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            //catch { }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get Forum Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForum()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select ForumId,ForumName,ForumShortName from Forum where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update Forum Data
        /// </summary>
        /// <param name="ForumName"></param>
        /// <param name="ForumShortName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateForum(string ForumName, string ForumShortName, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update Forum set ForumName='" + ForumName + "',ForumShortName='" + ForumShortName + "' where ForumId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get GlobalSetting Data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetGlobalSetting()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select Id,[Key],[Value] from GlobalSetting where [Key]='Encryption Key'", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update GlobalSetting Data
        /// </summary>
        /// <returns></returns>
        public static int UpdateGlobalSetting(string value  , Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update GlobalSetting set value='" + value + "' where Id='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            //catch { }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get Meeting data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMeeting()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select MeetingId,MeetingDate,MeetingVenue,MeetingTime from Meeting where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update Meeting data
        /// </summary>
        /// <param name="MeetingDate"></param>
        /// <param name="MeetingVenue"></param>
        /// <param name="MeetingTime"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateMeeting(string MeetingDate, string MeetingVenue,string MeetingTime, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update Meeting set MeetingDate='" + MeetingDate + "',MeetingVenue='"+ MeetingVenue + "',MeetingTime='"+ MeetingTime + "' where MeetingId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get Roll Master data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRollMaster()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select RollMasterId,RollName from RollMaster where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update Roll Master data
        /// </summary>
        /// <param name="RollName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateRollMaster(string RollName, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update RollMaster set RollName='" + RollName + "' where RollMasterId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get Upload Minute data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUploadMinute()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select UploadMinuteId,EncryptionKey from UploadMinute where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update uploadMinute data
        /// </summary>
        /// <param name="EncryptionKey"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateUploadMinute(string EncryptionKey, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update UploadMinute set EncryptionKey='" + EncryptionKey + "' where UploadMinuteId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            //catch { }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }

        /// <summary>
        /// Get User data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUser()
        {


            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("select UserId,FirstName,LastName,Email,UserName,Password,ContactNo,PANNo,Photograph,OfficeAddress,ResidentialAddress,DINNumber,OfficePhone,ResidencePhone,Moblie,EmailID1,EmailID2,SecretaryName,SecretaryResidentalPhone,SecretaryOfficePhone,SecretaryMobile,SecretaryEmailID1,SecretaryEmailID2,Designation,Answer,UDId,Suffix,MiddleName,PassportNumber from [User] where IsActive=1", objConn);
            DataTable dt = new DataTable();
            try
            {
                objConn.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Update user data
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="Email"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="ContactNo"></param>
        /// <param name="PANNo"></param>
        /// <param name="Photograph"></param>
        /// <param name="OfficeAddress"></param>
        /// <param name="ResidentialAddress"></param>
        /// <param name="DINNumber"></param>
        /// <param name="OfficePhone"></param>
        /// <param name="ResidencePhone"></param>
        /// <param name="Moblie"></param>
        /// <param name="EmailID1"></param>
        /// <param name="EmailID2"></param>
        /// <param name="SecretaryName"></param>
        /// <param name="SecretaryResidentalPhone"></param>
        /// <param name="SecretaryOfficePhone"></param>
        /// <param name="SecretaryMobile"></param>
        /// <param name="SecretaryEmailID1"></param>
        /// <param name="SecretaryEmailID2"></param>
        /// <param name="Designation"></param>
        /// <param name="Answer"></param>
        /// <param name="UDId"></param>
        /// <param name="Suffix"></param>
        /// <param name="MiddleName"></param>
        /// <param name="PassportNumber"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static int UpdateUser(string FirstName, string LastName, string Email, string UserName, string Password, string ContactNo, string PANNo, string Photograph, string OfficeAddress, string ResidentialAddress, string DINNumber, string OfficePhone, string ResidencePhone, string Moblie, string EmailID1, string EmailID2, string SecretaryName, string SecretaryResidentalPhone, string SecretaryOfficePhone, string SecretaryMobile, string SecretaryEmailID1, string SecretaryEmailID2, string Designation, string Answer, string UDId, string Suffix, string MiddleName, string PassportNumber, Guid Id)
        {
            int iRowsAffected = -1;
            string con = ConnectionString();
            SqlConnection objConn = new SqlConnection(con);

            string strStoredProcedure = "Update [User] set FirstName='" + FirstName + "',LastName='"+ LastName + "',Email='"+ Email + "',UserName='"+ UserName + "',Password='"+ Password + "',ContactNo='"+ ContactNo + "',PANNo='"+ PANNo + "',Photograph='"+ Photograph + "',OfficeAddress='"+ OfficeAddress + "',ResidentialAddress='"+ ResidentialAddress + "',DINNumber='"+ DINNumber + "',OfficePhone='"+ OfficePhone + "',ResidencePhone='"+ ResidencePhone + "',Moblie='"+ Moblie + "',EmailID1='"+ EmailID1 + "',EmailID2='"+ EmailID2+ "',SecretaryName='"+ SecretaryName + "',SecretaryResidentalPhone='"+ SecretaryResidentalPhone + "',SecretaryOfficePhone='"+ SecretaryOfficePhone + "',SecretaryMobile='"+ SecretaryMobile + "',SecretaryEmailID1='"+ SecretaryEmailID1 + "',SecretaryEmailID2='"+ SecretaryEmailID2 + "',Designation='"+ Designation + "',Answer='"+ Answer + "',UDId='"+ UDId + "',Suffix='"+ Suffix + "',MiddleName='"+ MiddleName + "',PassportNumber='"+ PassportNumber + "' where UserId='" + Id + "'";
            SqlCommand cmd = new SqlCommand(strStoredProcedure, objConn);

            try
            {
                objConn.Open();
                iRowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);                
            }
            finally
            {
                objConn.Close();

                cmd.Dispose();
                objConn.Dispose();
            }
            return iRowsAffected;
        }
    }
}
