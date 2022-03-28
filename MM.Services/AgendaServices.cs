using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
    public class AgendaServices
    {
        /// <summary>
        /// Get agenda details by id
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public AgendaDomain GetAgendaById(Guid AgendaId)
        {
            AgendaDomain objAgenda = new AgendaDomain();
            objAgenda = AgendaDataProvider.Instance.Get(AgendaId);
            return objAgenda;
        }

        /// <summary>
        /// Get agenda list
        /// </summary>
        /// <returns></returns>
        public IList<AgendaDomain> GetAllAgenda()
        {
            IList<AgendaDomain> objAgenda = null;
            objAgenda = AgendaDataProvider.Instance.Get();
            return objAgenda;
        }

        /// <summary>
        /// Insert Agenda Details
        /// </summary>
        /// <param name="AgendaName">string specifying AgendaName</param>
        /// <param name="AgendaNote">string specifying AgendaNote</param>
        /// <param name="ParentAgendaId">Guid specifying ParentAgendaId</param>
        /// <param name="UploadedAgendaNote">Guid specifying UploadedAgendaNote</param>
        /// <param name="PublishedBy">Guid specifying PublishedBy</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpadatedBy">Guid specifying UpadatedBy</param>
        /// <returns></returns>
        public AgendaDomain InsertAgenda(string AgendaName, string AgendaNote, Guid ParentAgendaId, string UploadedAgendaNote, Guid PublishedBy, Guid CreatedBy, Guid UpadatedBy, string AgendaFile)
        {
            AgendaDomain objAgenda = new AgendaDomain();
            objAgenda.AgendaName = AgendaName;
            objAgenda.AgendaNote = AgendaNote;
            objAgenda.CreatedBy = CreatedBy;
            objAgenda.UpdatedBy = UpadatedBy;
            objAgenda.PublishedBy = PublishedBy;
            objAgenda.UploadedAgendaNote = UploadedAgendaNote;
            return AgendaDataProvider.Instance.Insert(objAgenda, AgendaFile);
        }

        /// <summary>
        /// Update Agenda details by id
        /// </summary>
        /// <param name="AgendaName">string specifying AgendaName</param>
        /// <param name="AgendaNote">string specifying AgendaNote</param>
        /// <param name="ParentAgendaId">Guid specifying ParentAgendaId</param>
        /// <param name="UploadedAgendaNote">Guid specifying UploadedAgendaNote</param>
        /// <param name="PublishedBy">Guid specifying PublishedBy</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="UpadatedBy">Guid specifying UpadatedBy</param>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public bool UpdateAgenda(string AgendaName, string AgendaNote, Guid ParentAgendaId, string UploadedAgendaNote, Guid PublishedBy, Guid CreatedBy, Guid UpadatedBy, Guid AgendaId, string AgendaFile)
        {
            AgendaDomain objAgenda = new AgendaDomain();
            objAgenda.AgendaName = AgendaName;
            objAgenda.AgendaNote = AgendaNote;
            objAgenda.CreatedBy = CreatedBy;
            objAgenda.UpdatedBy = UpadatedBy;
            objAgenda.PublishedBy = PublishedBy;
            objAgenda.UploadedAgendaNote = UploadedAgendaNote;
            objAgenda.AgendaId = AgendaId;
            return AgendaDataProvider.Instance.Update(objAgenda, AgendaFile);
        }

        /// <summary>
        /// Delete  agenda by agenda id
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <returns></returns>
        public bool Delete(Guid AgendaId)
        {
            return AgendaDataProvider.Instance.Delete(AgendaId);
        }

        /// <summary>
        /// Get agenda details by meeting id
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetAgendaByMeetingId(Guid MeetingId)
        {
            return AgendaDataProvider.Instance.GetAgendabyMeetingId(MeetingId);
        }


        /// <summary>
        /// Update Agenda Order
        /// </summary>
        /// <param name="strAgendaOrder">string specifying strAgendaOrder</param>
        /// <param name="strAgendaIds">string specifying strAgendaIds</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool UpdateAgendaOrder(string strAgendaOrder, string strAgendaIds, Guid UserId)
        {
            return AgendaDataProvider.Instance.UpdateAgendaOrders(strAgendaOrder, strAgendaIds, UserId);
        }

        /// <summary>
        /// Get unapproved documents for agenda
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <returns></returns>
        public IList<AgendaDomain> GetUnApprovedDocumet(Guid MeetingId)
        {
            return AgendaDataProvider.Instance.GetUnApprovedDocumet(MeetingId);
        }

        /// <summary>
        /// Update Agenda Status
        /// </summary>
        /// <param name="AgendaId">Guid specifying AgendaId</param>
        /// <param name="Status">string specifying Status</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public bool UpdateAgendaStatus(Guid AgendaId, string Status, Guid UserId)
        {
            return AgendaDataProvider.Instance.UpdateAgendaStatus(AgendaId, Status, UserId);
        }

        /// <summary>
        /// Insert agenda Ack 
        /// </summary>
        /// <param name="MeetingId">Guid specifying MeetingId</param>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <param name="AgendaReadOn">DateTime specifying AgendaReadOn</param>
        /// <param name="NoticeReadOn">DateTime specifying NoticeReadOn</param>
        /// <returns>Int Id</returns>
        public Int32 InsertAgendaAck(Guid MeetingId, Guid UserId, DateTime? AgendaReadOn, DateTime? NoticeReadOn, bool IsNoticeRead, bool IsAgendaRead, Guid PublishVersion, string AccessToken)
        {
            return AgendaDataProvider.Instance.InsertAgendaAck(MeetingId, UserId, AgendaReadOn, NoticeReadOn, IsNoticeRead, IsAgendaRead, PublishVersion, AccessToken);
        }

        public DataSet GetAgendaReadUnread(Guid MeetingId, Guid UserId)
        {
            return AgendaDataProvider.Instance.GetAgendaReadUnread(MeetingId, UserId);
        }

        public IList<IPADomain> GetIPADomainByEntity(Guid EntityId)
        {
            return IPADataprovider.Instance.GetIPADomainByEntity(EntityId);
        }

        /// <summary>
        /// Get agenda Read coun
        /// </summary>
        /// <param name="AgendaId">string sepcifing AgendaId</param>
        /// <param name="UserId">Guid sepcifing UserId</param>
        /// <param name="IsRead">bool specifying IsRead</param>
        /// <returns></returns>
        public DataSet GetAgendaCounter(string AgendaId, Guid UserId, bool IsRead)
        {
            return AgendaDataProvider.Instance.GetAgendaCounter(AgendaId, UserId, IsRead);
        }
    }
}
