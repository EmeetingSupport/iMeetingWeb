using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
#region "User Property"
    /// <summary>
    /// Property Of User Table
    /// </summary>
    public class UserDomain
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MiddleName { get; set; }

      //  public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
      //  public string ContactNo { get; set; }
        public string PANNo { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string Designation
        {
            get;
            set;
        }
        public Guid EntityId
        {
            get;
            set;
        }

        public string OfficeAddress
        {
            get;
            set;
        }
        public string ResidentialAddress
        {
            get;
            set;
        }
        public string DINNumber
        {
            get;
            set;
        }

        public string Photograph
        {
            get;
            set;
        }

        public string OfficePhone
        {
            get;
            set;
        }
        public string ResidencePhone
        {
            get;
            set;
        }
        public string Mobile
        {
            get;
            set;
        }
        public string EmailID1
        {
            get;
            set;
        }
        public string EmailID2
        {
            get;
            set;
        }

        public string SecretaryName
        {
            get;
            set;
        }

        public string SecretaryResidentalPhone
        {
            get;
            set;
        }

        public string SecretaryOfficePhone
        {
            get;
            set;
        }
        public string SecretaryMobile
        {
            get;
            set;
        }

        public string SecretaryEmailID1
        {
            get;
            set;
        }

        public string SecretaryEmailID2
        {
            get;
            set;
        }

        public bool IsAuthorized
        {
            get;
            set;
        }

         public bool IsChecker { get; set; }
         public bool IsMaker { get; set; }
         public bool IsEnabledOnIpad { get; set; }
         public bool IsEnabledOnWebApp { get; set; }
         public Guid UserChecker { get; set; }

         public Guid SecurityQuestionId
         {
             get;
             set;
         }

         public string Answer
         {
             get;
             set;
         }

         public string Token
         {
             get;
             set;
         }

         public string Suffix
         {
             get;
             set;
         }

         public string DeviceToken
         {
             get;
             set;
         }

         public string FoodPreference
         {
             get;
             set;
         }

         public string EncryptionKey
         {
             get;
             set;
         }

         public int SessionTimeout
         {
             get;
             set;
         }

         public bool IsDeviceLost
         {
             get;
             set;
         }


         public string RollName
         {
             get;
             set;
         }

        public string PassportNumber { get; set; }

        public bool IsLocked { get; set; }

        public bool IsSuperAdmin { get; set; }
        public Guid OTPId { get; set; }
        public string OTP { get; set; }

        public Guid ActivationCode { get; set; }
        public string Status { get; set; }
        public bool SentOTP { get; set; }

        public bool hideButton { get; set; }

        public string LoginRequest { get; set; }
        
        public string EntityIds { get; set; }

        public string EntityNames { get; set; }
        #endregion
    }
}
