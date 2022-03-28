﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
    public class MeetingServices
    {
        /// <summary>
        /// Get Meeting Details By Meeting Id
        /// </summary>
        /// <param name="MeetingID">Guid specifying MeetingID</param>
        /// <returns></returns>
        public MeetingDomain GetMeetingById(Guid MeetingID)
        {
            MeetingDomain objMeeting = new MeetingDomain();
            objMeeting = MeetingDataProvider.Instance.Get(MeetingID);
            return objMeeting;
        }

        /// <summary>
        /// Get Meeting All Details
        /// </summary>
        /// <returns></returns>
        public IList<MeetingDomain> GetAllMeetingDetails()
        {
            IList<MeetingDomain> objMeeting = null;
            objMeeting = MeetingDataProvider.Instance.Get();
            return objMeeting;
        }

        /// <summary>
        /// Get meeting By Forum id
        /// </summary>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <returns></returns>
        public IList<MeetingDomain> GetMeetingByForumId(Guid ForumId)
        {
            IList<MeetingDomain> objMeeting = null;
            objMeeting = MeetingDataProvider.Instance.GetMeetingByFroumID(ForumId);
            return objMeeting;
        }

        /// <summary>
        /// Get Meeting with entity details
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public MeetingDomain GetMeetingWithEntity(Guid MeetingId)
        {
            return MeetingDataProvider.Instance.GetMeetingWithEntity(MeetingId);
        }

        /// <summary>
        /// Insert Meeting Details
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="MeetingVenue">string specifying MeetingVenue</param>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <param name="MeetingDate">string specifying MeetingDate</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <returns></returns>
        public Guid InsertMeeting(string MeetingVenue, Guid ForumId, string MeetingDate, Guid CreatedBy, Guid UpdatedBy, Guid MeetingChecker, string MeetingTime, string SendToChecker)
        {
            MeetingDomain objMeeting = new MeetingDomain();
            objMeeting.MeetingVenue = MeetingVenue;
            objMeeting.MeetingDate = MeetingDate;
            objMeeting.UpdatedBy = UpdatedBy;
            objMeeting.CreatedBy = CreatedBy;
            objMeeting.ForumId = ForumId;
            objMeeting.MeetingChecker = MeetingChecker;
            objMeeting.MeetingTime = MeetingTime;

            objMeeting = MeetingDataProvider.Instance.Insert(objMeeting, SendToChecker);

            return objMeeting.MeetingId;
        }

        /// <summary>
        /// Update Meeting Details
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="MeetingVenue">string specifying MeetingVenue</param>
        /// <param name="ForumId">Guid specifying ForumId</param>
        /// <param name="MeetingDate">string specifying MeetingDate</param>
        /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
        /// <returns></returns>
        public bool UpdateMeeting(Guid MeetingId, string MeetingVenue, Guid ForumId, string MeetingDate, Guid UpdatedBy, Guid MeetingChecker, string MeetingTime, string SendToChecker)
        {
            MeetingDomain objMeeting = new MeetingDomain();
            objMeeting.MeetingVenue = MeetingVenue;
            objMeeting.MeetingDate = MeetingDate;
            objMeeting.UpdatedBy = UpdatedBy;
            objMeeting.ForumId = ForumId;
            objMeeting.MeetingId = MeetingId;
            objMeeting.MeetingChecker = MeetingChecker;
            objMeeting.MeetingTime = MeetingTime;

            return MeetingDataProvider.Instance.Update(objMeeting, SendToChecker);
        }

        /// <summary>
        /// Delete Meeting By Id
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public bool DeteleMeeting(Guid MeetingId, string DeleteReason)
        {
            return MeetingDataProvider.Instance.Delete(MeetingId, DeleteReason);
        }

        /// <summary>
        /// Delete Seleted Meetings By id
        /// </summary>
        /// <param name="MeetingIds">string specifying MeetingIds</param>
        /// <returns></returns>
        public bool DeleteSeletedMeeting(string MeetingIds)
        {
            return MeetingDataProvider.Instance.DeleteSelectedMeeting(MeetingIds);
        }

        /// <summary>
        /// Approve or reject meeting
        /// </summary>
        /// <param name="Status">string specifying status</param>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public bool UpdateMeetingStatus(string Status, Guid MeetingId)
        {
            return MeetingDataProvider.Instance.UpdateMeetingStatus(Status, MeetingId);
        }
    }
}
