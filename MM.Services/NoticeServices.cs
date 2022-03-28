﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MM.Core;
using MM.Data;
using MM.Domain;
namespace MM.Services
{
  public class NoticeServices
    {
      /// <summary>
      /// Get notice details by id
      /// </summary>
      /// <param name="noticeId">Guid specifying noticeId</param>
      /// <returns></returns>
      public NoticeDomain Get(Guid noticeId)
      {
          return NoticeDataProvider.Instance.Get(noticeId);
      }

      /// <summary>
      /// Get notice details 
      /// </summary>
      /// <returns></returns>
      public IList<NoticeDomain> Get()
      {
          return NoticeDataProvider.Instance.Get();
      }

      /// <summary>
      /// Get notice list with meeting
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public IList<NoticeDomain> GetNoticeWithMeeting(Guid UserId)
      {
          return NoticeDataProvider.Instance.GetNoticeWithMeeting(UserId);
      }

      /// <summary>
      /// Get unapproved notice
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public IList<NoticeDomain> GetUnapprovedNotice(Guid UserId)
      {
          return NoticeDataProvider.Instance.GetUnapprovedNotice(UserId);
      }

      /// <summary>
      /// Insert Notice detaisl 
      /// </summary>
      /// <param name="noticeId">Guid specifying noticeId</param>
      /// <param name="meetingId">Guid specifying meetingId</param>
      /// <param name="noticeMessage">string specifying noticeMessage</param>
      /// <param name="noticeChecker">Guid specifying noticeChecker</param>
      /// <param name="createdBy">Guid specifying createdBy</param>
      /// <returns></returns>
      public NoticeDomain Insert(Guid meetingId, string noticeMessage, Guid noticeChecker, Guid createdBy, string IsApprove)
      {
          NoticeDomain objNotice = new NoticeDomain();
          objNotice.MeetingId = meetingId;
          objNotice.CreatedBy = createdBy;
          objNotice.NoticeChecker = noticeChecker;
          objNotice.NoticeMessage = noticeMessage;
          objNotice.UpdatedBy = createdBy;
          objNotice.IsApproved = IsApprove;
          objNotice = NoticeDataProvider.Instance.Insert(objNotice);
          return objNotice;
      }

      /// <summary>
      /// Update notice detaisl
      /// </summary>
      /// <param name="noticeId">Guid specifying noticeId</param>
      /// <param name="meetingId">Guid specifying meetingId</param>
      /// <param name="noticeMessage">string specifying noticeMessage</param>
      /// <param name="noticeChecker">Guid specifying noticeChecker</param>
      /// <param name="createdBy">Guid specifying createdBy</param>
      /// <returns></returns>
      public bool Update(Guid noticeId, Guid meetingId, string noticeMessage, Guid noticeChecker, Guid createdBy, string IsApprove)
      {
          NoticeDomain objNotice = new NoticeDomain();
          objNotice.MeetingId = meetingId;
        //  objNotice.CreatedBy = createdBy;
          objNotice.NoticeChecker = noticeChecker;
          objNotice.NoticeMessage = noticeMessage;
          objNotice.UpdatedBy = createdBy;
          objNotice.NoticeId = noticeId;
          objNotice.IsApproved = IsApprove;
          return NoticeDataProvider.Instance.Update(objNotice);
      }

      /// <summary>
      /// delete notice by id
      /// </summary>
      /// <param name="noticeId"></param>
      /// <returns></returns>
      public bool Delete(Guid noticeId)
      {
          return NoticeDataProvider.Instance.Delete(noticeId);
      }
    }
}
