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
  public  class ForumServices
    {
      /// <summary>
      /// Get Forum Details By ID
      /// </summary>
      /// <param name="ForumID">Guid specifying ForumID</param>
      /// <returns></returns>
      public ForumDomain GetForumById(Guid ForumID)
      {
          ForumDomain objForum = new ForumDomain();
          objForum = ForumDataProvider.Instance.Get(ForumID);
          return objForum;
      }


     /// <summary>
     /// Get All Forum's
     /// </summary>
     /// <returns></returns>
      public DataTable GetAllForumServices()
      {
          IList<ForumDomain> objForumList = new List<ForumDomain>();
          objForumList = ForumDataProvider.Instance.Get();
          return objForumList.AsDataTable();
      }

     /// <summary>
     /// Insert Forum
     /// </summary>
     /// <param name="ForumName">string specifying ForumName</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <param name="EntityID">Guid specifying EntityID</param>
     /// <returns></returns>
      public Guid InsertForum(string ForumName,string SendToChecker, Guid CreatedBy, Guid UpdatedBy,Guid EntityID, string ForumShortName, Guid ForumChecker, bool IsEnable, string MembersInfo) 
      {

          ForumDomain objForum = new ForumDomain();
          objForum.ForumName = ForumName;
          objForum.EntityId = EntityID;
          objForum.CreatedBy = CreatedBy;
          objForum.UpdatedBy = UpdatedBy;

          objForum.ForumShortName = ForumShortName;
          objForum.ForumChecker = ForumChecker;
          objForum.IsEnable = IsEnable;
          objForum.MembersInfo = MembersInfo;
          objForum = ForumDataProvider.Instance.Insert(objForum, SendToChecker);
          return objForum.ForumId;
      }

     /// <summary>
     /// Delete ForumID
     /// </summary>
      /// <param name="ForumID">Guid specifying ForumID</param>
     /// <returns></returns>
      public bool DeleteForum(Guid ForumID)
      {
          return ForumDataProvider.Instance.Delete(ForumID);
      }


     /// <summary>
     /// Update Forum Details
     /// </summary>
     /// <param name="ForumID">Guid specifying ForumID</param>
     /// <param name="ForumName">string specifying ForumName</param>
     /// <param name="UpdatedBy">Guid sepcifying UpdatedBy</param>
      /// <param name="EntityID">Guid specifying EntityID</param>
     /// <returns></returns>
      //public bool UpdateForum(Guid ForumID,string SendToChecker, string ForumName, Guid UpdatedBy, Guid EntityID, string ForumShortName, Guid ForumChecker, bool IsEnable, string MembersInfo)
      //{

      //    ForumDomain objForum = new ForumDomain();
      //    objForum.ForumId = ForumID;
      //    objForum.EntityId = EntityID;
      //    objForum.ForumName = ForumName;
      //    objForum.UpdatedBy = UpdatedBy;
      //    objForum.MembersInfo = MembersInfo;
      //    objForum.ForumShortName = ForumShortName;
      //    objForum.ForumChecker = ForumChecker;
      //    objForum.IsEnable = IsEnable;

      //    return ForumDataProvider.Instance.Update(objForum, SendToChecker);

      //}

      ///// <summary>
      ///// Get Forum Details By Entity ID
      ///// </summary>
      ///// <param name="ForumID"></param>
      ///// <returns></returns>
      //public DataTable GetForumByEntityId(Guid EntityId)
      //{
      //    IList<ForumDomain> objForumList = new List<ForumDomain>();
      //    objForumList = ForumDataProvider.Instance.GetForumByEntityId(EntityId);
      //    return objForumList.AsDataTable();    
      //}


      /// <summary>
      /// Delete Seleted Forums by ID
      /// </summary>
      /// <param name="ForumIDs">Guid specifying ForumIDs</param>
      /// <returns></returns>
     public  bool DeleteForum(string ForumIDs)
      {
          return ForumDataProvider.Instance.DeleteSelectedForum(ForumIDs);
      }

      /// <summary>
      /// Get Forum By EntityId
      /// </summary>
      /// <param name="EntityId">Guid specifying EntityId</param>
      /// <returns></returns>
     public DataTable GetForumByEntityId(Guid EntityId)
     {
         IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumByEntityId(EntityId);
         if (objForum.Count > 0)
         {
             return objForum.AsDataTable();
         }
         return null;
     }

      /// <summary>
      /// Get forums accessible to user
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
     public DataTable GetForumByUserId(Guid UserId)
     {
         IList<ForumDomain> objForum = ForumDataProvider.Instance.GetForumsByUserId(UserId);
         if (objForum.Count > 0)
         {
             return objForum.AsDataTable();
         }
         return null; 
     }
    }


  
}
