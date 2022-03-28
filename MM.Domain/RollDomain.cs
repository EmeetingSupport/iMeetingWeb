using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Domain
{
    public class RollDomain
    {
        #region "Public properties of Roll"
        public Guid RollMasterId { get; set; }
        public string RollName { get; set; }
        public bool EntityMaster { get; set; }
        public bool UserMaster { get; set; }
        public bool AccessRightMaster { get; set; }
        public bool ApprovalMaster { get; set; }

        public bool Transaction { get; set; }
        public bool View { get; set; }
        public bool Report { get; set; }
        public bool Approval { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }

        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool RollMaster { get; set; }


        public bool Meeting { get; set; }
        public bool Agenda { get; set; }
        public bool UploadMinutes { get; set; }
        public bool MasterAgenda { get; set; }
        public bool BoardofDirectore { get; set; }
        public bool PhotoGallery { get; set; }
        public bool GlobalSetting { get; set; }
        public bool IpadNotification { get; set; }
        public bool EmailNotification { get; set; }
        public bool MeetingView { get; set; }
        public bool AgendaView { get; set; }
        public bool MinutesView { get; set; }

        public bool NoticeView { get; set; }
        public bool RevisedPdf { get; set; }

        public bool MeetingSchedule { get; set; }
        public bool AboutUs { get; set; }

        public bool DraftMOM { get; set; }
        
        public bool MasterClassification { get; set; }

        public bool DeviceManager { get; set; }
        public bool PublishHistory { get; set; }
        public bool EmailHistory { get; set; }

        public bool SectionMaster { get; set; }

        public bool HeaderMaster { get; set; }
        public bool AgendaMarker { get; set; }



        public bool SerialNoMaster { get; set; }
        public bool TabSetting { get; set; }

        public bool ProceedingMaster { get; set; }

        public bool ProceedingView { get; set; }
        public bool AttendanceView { get; set; }

        public bool MeetingRetention { get; set; }
        
        #endregion
    }
}
