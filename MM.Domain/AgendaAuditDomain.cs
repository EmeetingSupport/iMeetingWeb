using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM.Domain
{
    public class AgendaAuditDomain
    {
        public DateTime Date { get; set; }
        public string ForumName { get; set; }

        public string Meeting { get; set; }
        //public string MeetingTime { get; set; }
        //public string MeetingVenue { get; set; }
        //public string MeetingNumber { get; set; }
        public string AgendaTitle { get; set; }

        public string MakerName { get; set; }
        public string CheckerName { get; set; }
        public string UploadedPdfs { get; set; }
        public string IsPublished { get; set; }
        // public Guid PublishedBy { get; set; }

        // public Guid CreatedBy { get; set; }

        public string Status { get; set; }
        public string IsDelete { get; set; }

        //  public string Department { get; set; }

        //  public string IsParent { get; set; }
        public string AuditAction { get; set; }
    }

    public class AgendaReportDomain
    {
        public string ForumName { get; set; }
        public string MeetingDate_Time { get; set; }
        public string AgendaTitle { get; set; }
        public string MakerName { get; set; }
        public string FinalApprover { get; set; }

        public string CS { get; set; }
        public string MD_CEO { get; set; }

        public string[] AgendaChecker { get; set; }
    }

    public class MeetingAuditDomain
    {
        public string Date { get; set; }
        public string ForumName { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingDate { get; set; }

        public string MeetingVenue { get; set; }

        public string MakerName { get; set; }
        public string CheckerName { get; set; }
        public string MeetingStatus { get; set; }
        public string MeetingNumber { get; set; }
        public string IsDelete { get; set; }

        //   public string Notice { get; set; }

        public string AuditAction { get; set; }

    }

    public class NoticeAuditDomain
    {
        public string Date { get; set; }

        public string MakerName { get; set; }
        public string CheckerName { get; set; }
        public string NoticeStatus { get; set; }
        public string IsPublished { get; set; }
        public string Notice { get; set; }
        public string AuditAction { get; set; }
    }

    public class UserAuditDomain
    {
        public string Date { get; set; }
        public string MakerName { get; set; }
        public string CheckerName { get; set; }
        public string UserStatus { get; set; }
        public string IsDelete { get; set; }
        public string AuditAction { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Moblie { get; set; }
        public string Designation { get; set; }


        public string IsChecker { get; set; }
        public string IsMaker { get; set; }
        public string EnabledOnIpad { get; set; }


        public string EnabledOnWeb { get; set; }
        //  public string IsLocked { get; set; }
    }

    public class UserDepartmentAuditDomain
    {
        public string Date { get; set; }
        public string MakerName { get; set; }
        public string DepartmentName { get; set; }
        public string AuditAction { get; set; }
    }

    public class ForumAccessAuditDomain
    {
        public string Date { get; set; }
        public string MakerName { get; set; }
        public string ForumName { get; set; }
        public string Name { get; set; }
        public string IsDelete { get; set; }
        public string AuditAction { get; set; }

    }

    public class ForumAuditDomain
    {
        public string Date { get; set; }
        public string MakerName { get; set; }
        public string ForumName { get; set; }
        //  public string CheckerName { get; set; }
        public string IsDelete { get; set; }
        public string AuditAction { get; set; }

    }


    public class UploadMinuteAuditDomain
    {
        public string Date { get; set; }
        public string ForumName { get; set; }
        public string MeetingTime { get; set; }
        public string MeetingDate { get; set; }
        public string MeetingVenue { get; set; }
        public string MakerName { get; set; }
        //public string CheckerName { get; set; }
        //public string MeetingStatus { get; set; }
        public string MeetingNumber { get; set; }
        public string IsDelete { get; set; }
        public string AuditAction { get; set; }

    }

}
