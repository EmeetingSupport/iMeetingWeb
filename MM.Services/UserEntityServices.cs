using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Data;
using MM.Domain;
using System.Data;

namespace MM.Services
{
  public  class UserEntityServices
    {
        /// <summary>
        /// Get Entity by user id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<UserEntityDomain> GetEntitybyUserId(string UserId)
        {
            return UserEntityDataProvider.Instance.GetEntityListByUserId(Guid.Parse(UserId));
        }

        /// <summary>
        /// Get data for wcf services
        /// </summary>
        /// <param name="userId">Guid specifying UserId</param>
        /// <param name="startTime">DateTime specifying startTime</param>
        /// <param name="endTime">DateTime specifying endTime</param>
        /// <param name="udid">string specifying udid</param>
        /// <returns></returns>
        public DataSet GetDataForWcf(Guid userId, DateTime startTime, DateTime endTime, string password)
        {
            return UserEntityDataProvider.Instance.GetDetailsForWcf(userId, startTime, endTime, password);
        }


        /// <summary>
        /// Get data by entity for wcf  services
        /// </summary>
        /// <param name="userId">Guid specifying UserId</param>
        /// <param name="startTime">DateTime specifying startTime</param>
        /// <param name="endTime">DateTime specifying endTime</param>
        /// <param name="udid">string specifying udid</param>
        /// <returns></returns>
        public DataSet GetDataForWcfWithEntity(Guid userId, DateTime startTime, DateTime endTime, Guid entityid, string udid)
        {
            return UserEntityDataProvider.Instance.GetDetailsForWcfByEntiy(userId, startTime, endTime, udid, entityid);
        }

        /// <summary>
        /// Get data for wcf services
        /// </summary>
        /// <param name="userId">Guid specifying UserId</param>
        /// <param name="password">string specifying password</param>     
        /// <returns></returns>
        public DataSet GetDataForWcfData(Guid userId, string password)
        {
            return UserEntityDataProvider.Instance.GetDetailsForWcfData(userId, password);
        }

        /// <summary>
        /// Get data by entity for wcf services
        /// </summary>
        /// <param name="userId">Guid specifying UserId</param>
        /// <param name="entityid">Guid specifying entityid</param>
        /// <param name="endTime">string specifying udid</param>
        /// <returns></returns>
        public DataSet GetDataForWcfDataByEntity(Guid userId, Guid entityid, string udid)
        {
            return UserEntityDataProvider.Instance.GetDetailsForWcfDataByEntity(userId, udid, entityid);
        }

      /// <summary>
      /// Get events details for Event Calender
      /// </summary>
      /// <returns></returns>
        public DataSet GetEventsDetailsWcf()
        {
            return UserEntityDataProvider.Instance.GetEventsDetailsWcf();
        }

        /// <summary>
        /// Get events details for Event Calender by UserId
        /// </summary>
        /// <returns></returns>
        public DataSet GetEventsDetailsByUserWcf(Guid UserId, DateTime StartTime)
        {
            return UserEntityDataProvider.Instance.GetEventsDetailsByUserIdWcf(UserId, StartTime);
        }

      /// <summary>
      /// Insert Event details
      /// </summary>
      /// <param name="StartTime"></param>
      /// <param name="EndTime"></param>
      /// <param name="EventDescription"></param>
      /// <param name="EventTitle"></param>
      /// <returns></returns>
        public Int64 InsertEvents(DateTime StartTime, DateTime EndTime, string EventDescription, string EventTitle, Guid userId, string eventDay)
        {
            return UserEntityDataProvider.Instance.InsertEvent("", "", "", EventTitle, "", "", EventDescription, StartTime, EndTime, userId, eventDay);
        }

      /// <summary>
      /// Update User event details
      /// </summary>
      /// <param name="id"></param>
      /// <param name="nameOfVisitor"></param>
      /// <param name="designtionOfVisitor"></param>
      /// <param name="nameOfVisitorOrganisation"></param>
      /// <param name="appointment"></param>
      /// <param name="nameDesignation"></param>
      /// <param name="uploadPhoto"></param>
      /// <param name="attachments"></param>
      /// <param name="information"></param>
      /// <param name="start"></param>
      /// <param name="end"></param>
      /// <param name="IsActive"></param>
      /// <param name="eventDay"></param>
      /// <returns></returns>
        public bool UpdateEvents(Int32 id, String nameOfVisitor, String designtionOfVisitor, String nameOfVisitorOrganisation, String appointment, String nameDesignation, String uploadPhoto, String attachments, String information, string start, string end, bool IsActive, string eventDay)
        {
            return UserEntityDataProvider.Instance.UpdateEvent(id, nameOfVisitor, designtionOfVisitor, nameOfVisitorOrganisation, appointment, nameDesignation, uploadPhoto, attachments, information, start, end, IsActive, eventDay);
        }

      /// <summary>
      /// Delete Event by id
      /// </summary>
      /// <param name="id">Integer specifying id</param>
      /// <returns></returns>
        public DataSet DeleteEvent(Int32 id)
        {
            return UserEntityDataProvider.Instance.DeleteEvent(id);
        }

      /// <summary>
      /// Update MOM of event
      /// </summary>
        /// <param name="id">Integer specifying id</param>
      /// <param name="MOM">string specifying mom</param>
      /// <returns></returns>
        public bool UpdateEventMOM(Int32 id, string MOM)
        {
            return UserEntityDataProvider.Instance.UpdateEventMOM(id, MOM);
        }

        /// <summary>
        /// Get User delete staus by Id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public DataSet GetUserStatusById(Guid UserId)
        {
            return UserEntityDataProvider.Instance.GetAuthoizedUser(UserId);
        }

        /// <summary>
        /// set Data delete flag in database
        /// </summary>
        /// <param name="UdId">string specifying udid</param>
        /// <returns></returns>
        public bool DataClean(string UdId)
        {
            return UserDataProvider.Instance.DataClean(UdId);
        }

        /// <summary>
        /// Get Published Agenda By MeetingId
        /// </summary>
        /// <param name="MeetingId"></param>
        /// <returns></returns>
        public DataSet GetPublishedAgendaByMeetingId(Guid MeetingId)
        {
            return UserEntityDataProvider.Instance.GetPublishedAgendaByMeetingId(MeetingId);
        }

    }
}
