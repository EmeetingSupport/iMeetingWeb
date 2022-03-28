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
    public class EntityServices
    {


        /// <summary>
        /// Get Entity Details By EntityID
        /// </summary>
        /// <param name="EntityID">Guid specifying EntityID</param>
        /// <returns></returns>
        public EntityDomain GetEntityById(Guid EntityID)
        {
            EntityDomain objEntity = new EntityDomain();
            objEntity = EntityDataProvider.Instance.Get(EntityID);
            return objEntity;
        }


       /// <summary>
       /// Get All Entity's
       /// </summary>
       /// <returns></returns>
        public IList<EntityDomain> GetAllEntityServices()
        {
            IList<EntityDomain> objEntityList = new List<EntityDomain>();
            objEntityList = EntityDataProvider.Instance.Get();
            return objEntityList;
        }

        /// <summary>
        /// Get Entity by User Id
        /// </summary>
        /// <param name="UserId">Guid specifying UserId</param>
        /// <returns></returns>
        public IList<EntityDomain> GetAllEntityByUsderId(Guid UserId)
        {
            return EntityDataProvider.Instance.GetEntityByUserId(UserId);
        }

      /// <summary>
      /// To Insert Entity
      /// </summary>
      /// <param name="EntityName">string specifying EntityName</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <returns></returns>
        public Guid InsertEntity(string EntityName, Guid CreatedBy, Guid UpdatedBy, string EntityShortName, string EntityLogo, Guid EntityChecker, bool IsEnable, string EntityMeeting, string sendToChecker)
        {

            EntityDomain objEntity = new EntityDomain();
            objEntity.EntityName = EntityName;
            objEntity.CreatedBy = CreatedBy;
            objEntity.UpdatedBy = UpdatedBy;

            objEntity.IsEnable = IsEnable;
            objEntity.EntityShortName = EntityShortName;
            objEntity.EntityLogo = EntityLogo;
            objEntity.EntityChecker = EntityChecker;
            objEntity.EntityMeeting = EntityMeeting;

            objEntity = EntityDataProvider.Instance.Insert(objEntity, sendToChecker);
            return objEntity.EntityId;
        }

     /// <summary>
     /// Delete Entity
     /// </summary>
     /// <param name="EntityID">Guid specifying EntityID</param>
     /// <returns></returns>
        public bool DeleteEntity(Guid EntityID)
        {
            return EntityDataProvider.Instance.Delete(EntityID);
        }

      
      /// <summary>
      /// Update Entity
      /// </summary>
      /// <param name="EntityID">Guid specifying EntityId</param>
      /// <param name="EntityName">string specifying EntityName</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <returns></returns>
        public bool UpdateEntity(Guid EntityID, string EntityName, Guid UpdatedBy, string EntityShortName, string EntityLogo, Guid EntityChecker, bool IsEnable, string EntityMeeting, string sendToChecker)
        {

            EntityDomain objEntity = new EntityDomain();
            objEntity.EntityId = EntityID;
            objEntity.EntityName = EntityName;
            objEntity.UpdatedBy = UpdatedBy;

            objEntity.IsEnable = IsEnable;
            objEntity.EntityShortName = EntityShortName;
            objEntity.EntityLogo = EntityLogo;
            objEntity.EntityChecker = EntityChecker;
            objEntity.EntityMeeting = EntityMeeting;

            return EntityDataProvider.Instance.Update(objEntity, sendToChecker);
            
        }

        /// <summary>
        /// Delete Seleted Entity
        /// </summary>
        /// <param name="EntityIds">string specifying EntityIds</param>
        /// <returns></returns>
        public bool DeleteSeletedEntity(string EntityIds)
        {
            return EntityDataProvider.Instance.DeleteSelectedEntity(EntityIds);
        }
    }
}
