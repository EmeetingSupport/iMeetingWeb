using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Domain;
using MM.Data;
using System.Data;

namespace MM.Services
{
    public class UserServices
    {
        /// <summary>
        /// Get user List
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetUserList()
        {
            IList<UserDomain> objUserList = new List<UserDomain>();
            objUserList = UserDataProvider.Instance.Get();
            return objUserList;
        }

        /// <summary>
        ///  Get User By user id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public UserDomain GetUserById(Guid UserId)
        {
            UserDomain objUserDomain = new UserDomain();
            objUserDomain = UserDataProvider.Instance.Get(UserId);
            return objUserDomain;
        }

        /// <summary>
        ///  Insert User details
        /// </summary>
        /// <param name="FirstName">string specifying FirstName</param>
        /// <param name="LastName">string specifying LastName</param>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="Password">string specifying Password</param>
        /// <param name="PANNo">string specifying PANNo</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="Photograph">string specifying Photograph</param>
        /// <param name="OfficeAddress">string specifying OfficeAddress</param>
        /// <param name="ResidentialAddress">string specifying ResidentialAddress</param>
        /// <param name="DINNumber">string specifying DINNumber</param>
        /// <param name="OfficePhone">string specifying OfficePhone</param>
        /// <param name="ResidencePhone">string specifying ResidencePhone</param>
        /// <param name="Mobile">string specifying Mobile</param>
        /// <param name="EmailID1">string specifying EmailID1</param>
        /// <param name="EmailID2">string specifying EmailID2</param>
        /// <param name="SecretaryName">string specifying SecretaryName</param>
        /// <param name="SecretaryResidentalPhone">string specifying SecretaryResidentalPhone</param>
        /// <param name="SecretaryOfficePhone">string specifying SecretaryOfficePhone</param>
        /// <param name="SecretaryMobile">string specifying SecretaryMobile</param>
        /// <param name="SecretaryEmailID1">string specifying SecretaryEmailID1</param>
        /// <param name="SecretaryEmailID2">string specifying SecretaryEmailID2</param>
        /// <param name="Designation">string specifying Designation</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="Entity">string specifying Entity</param>
        /// <param name="IsMaker">bool specifying IsMaker</param>
        /// <param name="IsEnabledOnIpad">bool specifying IsEnabledOnIpad</param>
        /// <param name="IsEnabledOnWebApp">bool specifying IsEnabledOnWebApp</param>
        /// <param name="UserChecker">Guid specifying UserChecker</param>
        /// <returns></returns>
        public UserDomain InserUser(string FirstName, string LastName,
                 string UserName, string Password, string PANNo, Guid CreatedBy, Guid UpdatedBy, string Photograph, string OfficeAddress, string ResidentialAddress, string DINNumber, string OfficePhone, string ResidencePhone, string Mobile, string EmailID1, string EmailID2, string SecretaryName, string SecretaryResidentalPhone, string SecretaryOfficePhone, string SecretaryMobile, string SecretaryEmailID1, string SecretaryEmailID2, string Designation, Guid UserId, string Entity, bool
            IsMaker, bool IsChecker, bool IsEnabledOnIpad, bool IsEnabledOnWebApp, Guid UserChecker, string SendToChecker)
        {
            UserDomain objUserDomain = new UserDomain();
            objUserDomain.FirstName = FirstName;
            objUserDomain.LastName = LastName;
            objUserDomain.CreatedBy = CreatedBy;
            objUserDomain.UpdatedBy = UpdatedBy;
            objUserDomain.Designation = Designation;
            objUserDomain.DINNumber = DINNumber;
            objUserDomain.EmailID1 = EmailID1;
            objUserDomain.EmailID2 = EmailID2;
            objUserDomain.Mobile = Mobile;
            objUserDomain.OfficeAddress = OfficeAddress;
            objUserDomain.OfficePhone = OfficePhone;
            objUserDomain.PANNo = PANNo;
            objUserDomain.Password = Password;
            objUserDomain.Photograph = Photograph;
            objUserDomain.ResidencePhone = ResidencePhone;
            objUserDomain.ResidentialAddress = ResidentialAddress;
            objUserDomain.SecretaryEmailID1 = SecretaryEmailID1;
            objUserDomain.SecretaryEmailID2 = SecretaryEmailID2;
            objUserDomain.SecretaryMobile = SecretaryMobile;
            objUserDomain.SecretaryName = SecretaryName;
            objUserDomain.SecretaryOfficePhone = SecretaryOfficePhone;
            objUserDomain.SecretaryResidentalPhone = SecretaryResidentalPhone;
            // objUserDomain.UserId = UserId;
            objUserDomain.UserName = UserName;
            objUserDomain.IsChecker = IsChecker;
            objUserDomain.IsMaker = IsMaker;
            objUserDomain.IsEnabledOnIpad = IsEnabledOnIpad;
            objUserDomain.IsEnabledOnWebApp = IsEnabledOnWebApp;
            objUserDomain.UserChecker = UserChecker;

            objUserDomain = UserDataProvider.Instance.Insert(objUserDomain, Entity, UserId, SendToChecker);
            return objUserDomain;
        }


        /// <summary>
        ///  Update user details by id
        /// </summary>
        /// <param name="FirstName">string specifying FirstName</param>
        /// <param name="LastName">string specifying LastName</param>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="Password">string specifying Password</param>
        /// <param name="PANNo">string specifying PANNo</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <param name="Photograph">string specifying Photograph</param>
        /// <param name="OfficeAddress">string specifying OfficeAddress</param>
        /// <param name="ResidentialAddress">string specifying ResidentialAddress</param>
        /// <param name="DINNumber">string specifying DINNumber</param>
        /// <param name="OfficePhone">string specifying OfficePhone</param>
        /// <param name="ResidencePhone">string specifying ResidencePhone</param>
        /// <param name="Mobile">string specifying Mobile</param>
        /// <param name="EmailID1">string specifying EmailID1</param>
        /// <param name="EmailID2">string specifying EmailID2</param>
        /// <param name="SecretaryName">string specifying SecretaryName</param>
        /// <param name="SecretaryResidentalPhone">string specifying SecretaryResidentalPhone</param>
        /// <param name="SecretaryOfficePhone">string specifying SecretaryOfficePhone</param>
        /// <param name="SecretaryMobile">string specifying SecretaryMobile</param>
        /// <param name="SecretaryEmailID1">string specifying SecretaryEmailID1</param>
        /// <param name="SecretaryEmailID2">string specifying SecretaryEmailID2</param>
        /// <param name="Designation">string specifying Designation</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="Entity">string specifying Entity</param>
        /// <param name="strDeleteEntityIds">string specifying strDeleteEntityIds</param>
        /// <param name="IsMaker">bool specifying IsMaker</param>
        /// <param name="IsEnabledOnIpad">bool specifying IsEnabledOnIpad</param>
        /// <param name="IsEnabledOnWebApp">bool specifying IsEnabledOnWebApp</param>
        /// <param name="UserChecker">Guid specifying UserChecker</param>
        /// <returns></returns>
        public bool UpdateUser(string FirstName, string LastName, string SendToChecker,
                    string UserName, string Password, string PANNo, string CreatedBy, string UpdatedBy, string Photograph, string OfficeAddress, string ResidentialAddress, string DINNumber, string OfficePhone, string ResidencePhone, string Mobile, string EmailID1, string EmailID2, string SecretaryName, string SecretaryResidentalPhone, string SecretaryOfficePhone, string SecretaryMobile, string SecretaryEmailID1, string SecretaryEmailID2, string Designation, string UserId, string EntityIds, string strDeleteEntityIds, bool IsMaker, bool IsChecker, bool IsEnabledOnIpad, bool IsEnabledOnWebApp, Guid UserChecker)
        {
            UserDomain objUserDomain = new UserDomain();
            objUserDomain.FirstName = FirstName;
            objUserDomain.LastName = LastName;
            objUserDomain.CreatedBy = Guid.Parse(CreatedBy);
            objUserDomain.UpdatedBy = Guid.Parse(UpdatedBy);
            objUserDomain.Designation = Designation;
            objUserDomain.DINNumber = DINNumber;
            objUserDomain.EmailID1 = EmailID1;
            objUserDomain.EmailID2 = EmailID2;
            objUserDomain.Mobile = Mobile;
            objUserDomain.OfficeAddress = OfficeAddress;
            objUserDomain.OfficePhone = OfficePhone;
            objUserDomain.PANNo = PANNo;
            objUserDomain.Password = Password;
            objUserDomain.Photograph = Photograph;
            objUserDomain.ResidencePhone = ResidencePhone;
            objUserDomain.ResidentialAddress = ResidentialAddress;
            objUserDomain.SecretaryEmailID1 = SecretaryEmailID1;
            objUserDomain.SecretaryEmailID2 = SecretaryEmailID2;
            objUserDomain.SecretaryMobile = SecretaryMobile;
            objUserDomain.SecretaryName = SecretaryName;
            objUserDomain.SecretaryOfficePhone = SecretaryOfficePhone;
            objUserDomain.SecretaryResidentalPhone = SecretaryResidentalPhone;
            objUserDomain.UserId = Guid.Parse(UserId);
            objUserDomain.UserName = UserName;

            objUserDomain.IsChecker = IsChecker;
            objUserDomain.IsMaker = IsMaker;

            objUserDomain.IsEnabledOnIpad = IsEnabledOnIpad;
            objUserDomain.IsEnabledOnWebApp = IsEnabledOnWebApp;
            objUserDomain.UserChecker = UserChecker;

            return UserDataProvider.Instance.Update(objUserDomain, EntityIds, strDeleteEntityIds, SendToChecker);
        }

        /// <summary>
        /// Delete user details by user id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool DeleteUser(Guid UserId)
        {
            return UserDataProvider.Instance.Delete(UserId);
        }

        /// <summary>
        /// Delete seleted user details
        /// </summary>
        /// <param name="UserIds">string specifying UserIds</param>
        /// <returns></returns>
        public bool DeleteSelectedUser(string UserIds)
        {
            return UserDataProvider.Instance.DeleteSelected(UserIds);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="OldPass">string specifying OldPass</param>
        /// <param name="NewPass">string specifying NewPass</param>
        /// <returns></returns>
        public bool ChangePassword(Guid UserId, string OldPass, string NewPass)
        {
            return UserDataProvider.Instance.ChangePassword(UserId, OldPass, NewPass);
        }

        /// <summary>
        /// Get User Password by user name
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <returns></returns>
        public UserDomain GetPassword(string UserName, Guid QuestionId, string Answer)
        {
            return UserDataProvider.Instance.GetPassword(UserName, QuestionId, Answer);
        }
        /// <summary>
        /// get user security question with answer
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public SecurityQuestionDomain GetSecurityQuestion(Guid UserId)
        {
            return SecurityQuestionDataProvider.Instance.GetSecurityQuestion(UserId);
        }

        /// <summary>
        /// User login by user name and password
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="Password">string specifying Password</param>
        /// <returns></returns>
        public UserDomain UserLogin(string UserName, string Password)
        {
            return UserDataProvider.Instance.UserLogin(UserName, Password);
        }

        /// <summary>
        /// User login by user name and password
        /// </summary>
        /// <param name="UserName">string specifying userName</param>
        /// <param name="Password">string specifying password</param>
        /// <param name="UdId">string specifying UdId </param>
        /// <returns></returns>
        public UserDomain UserLoginIpad(string UserName, string Password, string UdId, string DeviceToken, Guid UserId)
        {
            return UserDataProvider.Instance.UserLoginIpad(UserName, Password, UdId, DeviceToken, UserId);
        }

        /// <summary>
        /// User login by user name for Ipad
        /// </summary>
        /// <param name="UserName">string specifying userName</param>
        /// <param name="UdId">string specifying UdId </param>
        /// <returns></returns>
        public UserDomain UserLoginIpad(string UserName, string UdId, string DeviceToken)
        {
            return UserDataProvider.Instance.UserLoginIpadByUserName(UserName, UdId, DeviceToken);
        }

        public bool iPadUserOTPStatus(Guid UserId)
        {
            return UserDataProvider.Instance.iPadUserOTPStatus(UserId);
        }

        /// <summary>
        /// Get All User For access rights
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserByAccessRight()
        {
            return UserDataProvider.Instance.GetAllUserByAccessRight();
        }

        /// <summary>
        /// Get All User For Access rights
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllUserForUserId()
        {
            return UserDataProvider.Instance.GetAllUserForAccessRight();
        }
        /// <summary>
        /// Get All Checker List
        /// </summary>
        /// <returns></returns>
        public IList<UserDomain> GetAllChecker()
        {
            return UserDataProvider.Instance.GetAllChecker();
        }

        /// <summary>
        /// Forget password service
        /// </summary>
        /// <param name="UserName">string specifying UserName</param>
        /// <param name="QuestionId">Guid specifying QuestionId</param>
        /// <param name="Answer">string specifying Answer</param>
        /// <returns></returns>
        public string ForgetPassword(string UserName, Guid QuestionId, string Answer)
        {
            return UserDataProvider.Instance.GetForgetPassword(UserName, QuestionId, Answer);
        }

        /// <summary>
        /// Get Password by user name
        /// </summary>
        /// <param name="UserName">string specifying UserName </param>
        /// <returns></returns>
        public DataTable GetPasswordByUserName(string UserName)
        {
            return UserDataProvider.Instance.GetPasswordByUsername(UserName);
        }

        /// <summary>
        /// Insert or update device token
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <returns></returns>
        public bool InsertDeviceToken(string DeviceToken, Guid UserId)
        {
            return UserDataProvider.Instance.InsertDeviceToken(DeviceToken, UserId);
        }

        /// <summary>
        /// Insert user device log in database
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <param name="Mac">String specifying mac address</param>
        /// <param name="DeviceTime">String specifying time on device</param>
        /// <param name="OperationName">string specifying operation name</param>
        /// <param name="EntityId">string specifying entityid </param>
        /// <returns></returns>
        public bool InsertDeviceLog_old(string DeviceToken, Guid UserId, string Mac, string DeviceTime, string OperationName, Guid EntityId, string Requset, string Resposne, Guid Mapper, string AppVersion)
        {
            return UserDataProvider.Instance.InsertDeviceLog_old(DeviceToken, UserId, Mac, DeviceTime, OperationName, EntityId, Requset, Resposne, Mapper, AppVersion);
        }


        /// <summary>
        /// Insert user device log in database
        /// </summary>
        /// <param name="DeviceToken">string specifying device token</param>
        /// <param name="UserId">Guid specifying userId</param>
        /// <param name="Mac">String specifying mac address</param>
        /// <param name="DeviceTime">String specifying time on device</param>
        /// <param name="OperationName">string specifying operation name</param>
        /// <param name="EntityId">string specifying entityid </param>
        /// <returns></returns>
        public DataSet InsertDeviceLog(string DeviceToken, Guid UserId, string Mac, string DeviceTime, string OperationName, Guid EntityId, string Requset, string Resposne, Guid Mapper, string AppVersion, string RequestToken, string IsExpired, Guid LoginUserId, string UserIpAddress, string UserName)
        {
            return UserDataProvider.Instance.InsertDeviceLog(DeviceToken, UserId, Mac, DeviceTime, OperationName, EntityId, Requset, Resposne, Mapper, AppVersion, RequestToken, IsExpired, LoginUserId, UserIpAddress, UserName);
        }

        /// <summary>
        /// Update device log 
        /// </summary>
        /// <param name="Response">String specifying response</param>
        /// <param name="Mapper">guid specifying mapper</param>
        /// <returns></returns>
        public bool UpdateDeviceLog(string Response, Guid Mapper, string AccessToken, string IsExpired)
        {
            return UserDataProvider.Instance.UpdateDeviceLog(Response, Mapper, AccessToken, IsExpired);
        }


        /// <summary>
        /// Get users lost device infromation in database
        /// </summary>
        /// <param name="UdId">string specifying UserId</param>
        /// <returns></returns>
        public int IsUserLostDevice(Guid UserId)
        {
            return UserDataProvider.Instance.IsUserLostDevice(UserId);
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
            return UserDataProvider.Instance.VerifyOTP(UserId, TimeOut, OTP, OTPId);
        }

        public bool iPadUserLock(string UserName)
        {
            return UserDataProvider.Instance.iPadUserLock(UserName);
        }

        public DataTable iPadUserExists(string UserName)
        {
            return UserDataProvider.Instance.iPadUserExists(UserName);
        }

        public UserDomain Get(Guid UserId)
        {
            return UserDataProvider.Instance.Get(UserId);
        }
    }
}
