using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MM.Data;
using System.Configuration;
using MM.Data;
using MM.Core;

namespace MeetingMinder.Web
{
    public partial class DbConvert : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

        private void ConvertAboutUs()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetAboutUsData();
                string EncryptionKey = "";
                int Id = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    EncryptionKey = dr["EncryptionKey"].ToString();
                    EncryptionKey = Encryptor.DecryptString(EncryptionKey);
                    EncryptionKey = AesEncryption.EncryptString(EncryptionKey);
                    Id = Convert.ToInt32(dr["Id"]);
                    int count = DbCOnvertDataProvider.UpdateAboutUs(EncryptionKey, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertAboutUs() ,Exception:" + ex);
            }
        }

        private void ConvertAgendaDetails()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetAgendaDetails();
                string EncryptionKey = "";
                string AgendaKey = "";
                int Id = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    AgendaKey = dr["AgendaKey"].ToString();
                    AgendaKey = Encryptor.DecryptString(AgendaKey);
                    AgendaKey = AesEncryption.EncryptString(AgendaKey);

                    EncryptionKey = dr["EncryptionKey"].ToString();
                    EncryptionKey = Encryptor.DecryptString(EncryptionKey);
                    EncryptionKey = AesEncryption.EncryptString(EncryptionKey);
                    Id = Convert.ToInt32(dr["AgendaDetailsId"]);
                    int count = DbCOnvertDataProvider.UpdateAgendaDetails(AgendaKey,EncryptionKey, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertAgendaDetails() ,Exception:" + ex);
            }
        }

        private void ConvertEntity()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetEntity();
                string EncryptionKey = "";
                string EntityName = "";
                string EntityShortName = "";
                string EntityLogo = "";
                string EntityMeeting = "";

                Guid Id ;

                foreach (DataRow dr in dt.Rows)
                {
                    EntityName = dr["EntityName"].ToString();
                    EntityName = Encryptor.DecryptString(EntityName);
                    EntityName = AesEncryption.EncryptString(EntityName);

                    EntityShortName = dr["EntityShortName"].ToString();
                    EntityShortName = Encryptor.DecryptString(EntityShortName);
                    EntityShortName = AesEncryption.EncryptString(EntityShortName);

                    EntityLogo = dr["EntityLogo"].ToString();
                    EntityLogo = Encryptor.DecryptString(EntityLogo);
                    EntityLogo = AesEncryption.EncryptString(EntityLogo);

                    EntityMeeting = dr["EntityMeeting"].ToString();
                    EntityMeeting = Encryptor.DecryptString(EntityMeeting);
                    EntityMeeting = AesEncryption.EncryptString(EntityMeeting);

                    EncryptionKey = dr["EncryptionKey"].ToString();
                    EncryptionKey = Encryptor.DecryptString(EncryptionKey);
                    EncryptionKey = AesEncryption.EncryptString(EncryptionKey);
                    Id = Guid.Parse(dr["EntityId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateEntity(EntityName, EntityShortName, EntityLogo, EntityMeeting, EncryptionKey, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertEntity() ,Exception:" + ex);
            }
        }

        private void ConvertForum()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetForum();
                
                string ForumName = "";
                string ForumShortName = "";
              

                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    ForumName = dr["ForumName"].ToString();
                    ForumName = Encryptor.DecryptString(ForumName);
                    ForumName = AesEncryption.EncryptString(ForumName);

                    ForumShortName = dr["ForumShortName"].ToString();
                    ForumShortName = Encryptor.DecryptString(ForumShortName);
                    ForumShortName = AesEncryption.EncryptString(ForumShortName);

                   
                    Id = Guid.Parse(dr["ForumId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateForum(ForumName,ForumShortName, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertForum() ,Exception:" + ex);
            }
        }

        private void ConvertGlobalSetting()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetGlobalSetting();

                string Value = "";
               


                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    Value = dr["Value"].ToString();
                    Value = Encryptor.DecryptString(Value);
                    Value = AesEncryption.EncryptString(Value);

                    Id = Guid.Parse(dr["Id"].ToString());
                    int count = DbCOnvertDataProvider.UpdateGlobalSetting(Value, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertGlobalSetting() ,Exception:" + ex);
            }
        }

        private void ConvertMeeting()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetMeeting();

                string MeetingDate = "";
                string MeetingVenue = "";
                string MeetingTime = "";



                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    MeetingDate = dr["MeetingDate"].ToString();
                    MeetingDate = Encryptor.DecryptString(MeetingDate);
                    MeetingDate = AesEncryption.EncryptString(MeetingDate);

                    MeetingVenue = dr["MeetingVenue"].ToString();
                    MeetingVenue = Encryptor.DecryptString(MeetingVenue);
                    MeetingVenue = AesEncryption.EncryptString(MeetingVenue);

                    MeetingTime = dr["MeetingTime"].ToString();
                    MeetingTime = Encryptor.DecryptString(MeetingTime);
                    MeetingTime = AesEncryption.EncryptString(MeetingTime);

                    Id = Guid.Parse(dr["MeetingId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateMeeting(MeetingDate,MeetingVenue,MeetingTime, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertMeeting() ,Exception:" + ex);
            }
        }

        private void ConvertRollMaster()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetRollMaster();

                string RollName = "";
               



                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    RollName = dr["RollName"].ToString();
                    RollName = Encryptor.DecryptString(RollName);
                    RollName = AesEncryption.EncryptString(RollName);

                   

                    Id = Guid.Parse(dr["RollMasterId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateRollMaster(RollName, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertRollMaster() ,Exception:" + ex);
            }
        }

        private void ConvertUploadMinute()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetUploadMinute();

                string EncryptionKey = "";
                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    EncryptionKey = dr["EncryptionKey"].ToString();
                    EncryptionKey = Encryptor.DecryptString(EncryptionKey);
                    EncryptionKey = AesEncryption.EncryptString(EncryptionKey);



                    Id = Guid.Parse(dr["UploadMinuteId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateUploadMinute(EncryptionKey, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertUploadMinute() ,Exception:" + ex);
            }
        }

        private void ConvertUser()
        {
            try
            {
                DataTable dt = DbCOnvertDataProvider.GetUser();

                string FirstName = "";
                string LastName = "";
                string Email = "";
                string UserName = "";
                string Password = "";
                string ContactNo = "";
                string PANNo = "";
                string Photograph = "";
                string OfficeAddress = "";
                string ResidentialAddress = "";
                string DINNumber = "";
                string OfficePhone = "";
                string ResidencePhone = "";
                string Moblie = "";
                string EmailID1 = "";
                string EmailID2 = "";
                string SecretaryName = "";
                string SecretaryResidentalPhone = "";
                string SecretaryOfficePhone = "";
                string SecretaryMobile = "";
                string SecretaryEmailID1 = "";
                string SecretaryEmailID2 = "";
                string Designation = "";
                string Answer = "";
                string UDId = "";
                string Suffix = "";
                string MiddleName = "";
                string PassportNumber = "";             
                Guid Id;

                foreach (DataRow dr in dt.Rows)
                {
                    FirstName = dr["FirstName"].ToString();
                    FirstName = Encryptor.DecryptString(FirstName);
                    FirstName = AesEncryption.EncryptString(FirstName);

                    LastName = dr["LastName"].ToString();
                    LastName = Encryptor.DecryptString(LastName);
                    LastName = AesEncryption.EncryptString(LastName);

                    Email = dr["Email"].ToString();
                    Email = Encryptor.DecryptString(Email);
                    Email = AesEncryption.EncryptString(Email);

                    UserName = dr["UserName"].ToString();
                    UserName = Encryptor.DecryptString(UserName);
                    UserName = AesEncryption.EncryptString(UserName);

                    Password = dr["Password"].ToString();
                    Password = Encryptor.DecryptString(Password);
                    Password = AesEncryption.EncryptString(Password);

                    ContactNo = dr["ContactNo"].ToString();
                    ContactNo = Encryptor.DecryptString(ContactNo);
                    ContactNo = AesEncryption.EncryptString(ContactNo);

                    PANNo = dr["PANNo"].ToString();
                    PANNo = Encryptor.DecryptString(PANNo);
                    PANNo = AesEncryption.EncryptString(PANNo);

                    Photograph = dr["Photograph"].ToString();
                    Photograph = Encryptor.DecryptString(Photograph);
                    Photograph = AesEncryption.EncryptString(Photograph);

                    OfficeAddress = dr["OfficeAddress"].ToString();
                    OfficeAddress = Encryptor.DecryptString(OfficeAddress);
                    OfficeAddress = AesEncryption.EncryptString(OfficeAddress);

                    ResidentialAddress = dr["ResidentialAddress"].ToString();
                    ResidentialAddress = Encryptor.DecryptString(ResidentialAddress);
                    ResidentialAddress = AesEncryption.EncryptString(ResidentialAddress);

                    DINNumber = dr["DINNumber"].ToString();
                    DINNumber = Encryptor.DecryptString(DINNumber);
                    DINNumber = AesEncryption.EncryptString(DINNumber);

                    OfficePhone = dr["OfficePhone"].ToString();
                    OfficePhone = Encryptor.DecryptString(OfficePhone);
                    OfficePhone = AesEncryption.EncryptString(OfficePhone);

                    ResidencePhone = dr["ResidencePhone"].ToString();
                    ResidencePhone = Encryptor.DecryptString(ResidencePhone);
                    ResidencePhone = AesEncryption.EncryptString(ResidencePhone);

                    Moblie = dr["Moblie"].ToString();
                    Moblie = Encryptor.DecryptString(Moblie);
                    Moblie = AesEncryption.EncryptString(Moblie);

                    EmailID1 = dr["EmailID1"].ToString();
                    EmailID1 = Encryptor.DecryptString(EmailID1);
                    EmailID1 = AesEncryption.EncryptString(EmailID1);

                    EmailID2 = dr["EmailID2"].ToString();
                    EmailID2 = Encryptor.DecryptString(EmailID2);
                    EmailID2 = AesEncryption.EncryptString(EmailID2);

                    SecretaryName = dr["SecretaryName"].ToString();
                    SecretaryName = Encryptor.DecryptString(SecretaryName);
                    SecretaryName = AesEncryption.EncryptString(SecretaryName);

                    SecretaryResidentalPhone = dr["SecretaryResidentalPhone"].ToString();
                    SecretaryResidentalPhone = Encryptor.DecryptString(SecretaryResidentalPhone);
                    SecretaryResidentalPhone = AesEncryption.EncryptString(SecretaryResidentalPhone);

                    SecretaryOfficePhone = dr["SecretaryOfficePhone"].ToString();
                    SecretaryOfficePhone = Encryptor.DecryptString(SecretaryOfficePhone);
                    SecretaryOfficePhone = AesEncryption.EncryptString(SecretaryOfficePhone);

                    SecretaryMobile = dr["SecretaryMobile"].ToString();
                    SecretaryMobile = Encryptor.DecryptString(SecretaryMobile);
                    SecretaryMobile = AesEncryption.EncryptString(SecretaryMobile);

                    SecretaryEmailID1 = dr["SecretaryEmailID1"].ToString();
                    SecretaryEmailID1 = Encryptor.DecryptString(SecretaryEmailID1);
                    SecretaryEmailID1 = AesEncryption.EncryptString(SecretaryEmailID1);

                    SecretaryEmailID2 = dr["SecretaryEmailID2"].ToString();
                    SecretaryEmailID2 = Encryptor.DecryptString(SecretaryEmailID2);
                    SecretaryEmailID2 = AesEncryption.EncryptString(SecretaryEmailID2);

                    Designation = dr["Designation"].ToString();
                    Designation = Encryptor.DecryptString(Designation);
                    Designation = AesEncryption.EncryptString(Designation);

                    Answer = dr["Answer"].ToString();
                    Answer = Encryptor.DecryptString(Answer);
                    Answer = AesEncryption.EncryptString(Answer);

                    UDId = dr["UDId"].ToString();
                    UDId = Encryptor.DecryptString(UDId);
                    UDId = AesEncryption.EncryptString(UDId);

                    Suffix = dr["Suffix"].ToString();
                    Suffix = Encryptor.DecryptString(Suffix);
                    Suffix = AesEncryption.EncryptString(Suffix);

                    MiddleName = dr["MiddleName"].ToString();
                    MiddleName = Encryptor.DecryptString(MiddleName);
                    MiddleName = AesEncryption.EncryptString(MiddleName);

                    PassportNumber = dr["PassportNumber"].ToString();
                    PassportNumber = Encryptor.DecryptString(PassportNumber);
                    PassportNumber = AesEncryption.EncryptString(PassportNumber);


                    Id = Guid.Parse(dr["UserId"].ToString());
                    int count = DbCOnvertDataProvider.UpdateUser(FirstName, LastName, Email, UserName, Password, ContactNo, PANNo, Photograph, OfficeAddress, ResidentialAddress, DINNumber, OfficePhone, ResidencePhone, Moblie, EmailID1, EmailID2, SecretaryName, SecretaryResidentalPhone, SecretaryOfficePhone, SecretaryMobile, SecretaryEmailID1, SecretaryEmailID2, Designation, Answer, UDId, Suffix, MiddleName, PassportNumber, Id);
                }
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertUser() ,Exception:" + ex);
            }
        }

        //protected void btnConvert_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
                
                
                
               
               
               
               
               
               

               
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        protected void btnConvertAboutUs_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertAboutUs();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertAboutUs() ,Exception:" + ex);
            }
        }

        protected void btnConvertAgendaDetails_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertAgendaDetails();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertAgendaDetails() ,Exception:" + ex);
            }
        }

        protected void btnConvertEntity_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertEntity();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertEntity() ,Exception:" + ex);
            }
        }

        protected void btnConvertForum_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertForum();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertForum() ,Exception:" + ex);
            }
        }

        protected void btnConvertGlobalSetting_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertGlobalSetting();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertGlobalSetting() ,Exception:" + ex);
            }
        }

        protected void btnConvertMeeting_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertMeeting();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertMeeting() ,Exception:" + ex);
            }
        }

        protected void btnConvertRollMaster_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertRollMaster();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertRollMaster() ,Exception:" + ex);
            }
        }

        protected void btnConvertUploadMinute_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertUploadMinute();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertUploadMinute() ,Exception:" + ex);
            }
        }

        protected void btnConvertUser_Click(object sender, EventArgs e)
        {
            try
            {
                ConvertUser();
            }
            catch (Exception ex)
            {

                Response.Write("Method:ConvertUser() ,Exception:" + ex);
            }
        }
    }
}