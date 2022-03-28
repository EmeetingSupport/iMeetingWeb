using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;


namespace MM.Services
{
  public class AccessRightService
    {
      /// <summary>
      /// Get All Access Right
      /// </summary>
      /// <returns></returns>
      public IList<AccessRightDomain> GetAllAccessRight()
      {
          return AccessRightDataProvider.Instance.Get();
      }

      /// <summary>
      /// Get Access rights by user id
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public IList<AccessRightDomain> GetAccessRightByUserId(Guid UserId)
      {
          return AccessRightDataProvider.Instance.GetAccessRightByUserId(UserId);
      }
      
      ///// <summary>
      ///// Insert Access rights
      ///// </summary>
      ///// <param name="AccessRightSql"></param>
      ///// <returns></returns>
      //public bool InserAccessRight(string AccessRightSql)
      //{
      //    return AccessRightDataProvider.Instance.Insert(AccessRightSql);
      //}

    /// <summary>
      /// Insert Access rights
    /// </summary>
      /// <param name="UserIds">Guid specifying UserIds</param>
      /// <param name="AccessRights">string specifying CreatedBy</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <param name="RollId">Guid specifying RollId</param>
    /// <returns></returns>
      public bool InsertAccessRight(string UserIds, string AccessRights, Guid CreatedBy, Guid UpdatedBy, Guid RollId)
      {
          return AccessRightDataProvider.Instance.InserAccessRight(UserIds, AccessRights, CreatedBy, UpdatedBy, RollId);
      }

      /// <summary>
      /// Update Access rights
      /// </summary>
      /// <param name="UserIds">Guid specifying UserIds</param>
      /// <param name="AccessRights">string specifying AccessRights</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <param name="RollId">Guid specifying RollId</param>
      /// <param name="InsertUserIds">string specifying InsertUserIds</param>
      /// <param name="InsertAccessRights">string specifying InsertAccessRights</param>
      /// <param name="DeleteUserIds">string specifying DeleteUserIds</param>
      /// <returns></returns>
      public bool UpdateAccessRight(string UserIds, string AccessRights, Guid CreatedBy, Guid UpdatedBy, Guid RollId, string InsertUserIds, string InsertAccessRights, string DeleteUserIds)
      {
          return AccessRightDataProvider.Instance.UpdateAccessRight(UserIds, AccessRights, CreatedBy, UpdatedBy, RollId, InsertUserIds, InsertAccessRights, DeleteUserIds);
      }

      /// <summary>
      /// delete Access rights by id
      /// </summary>
      /// <param name="AccessRightId">Guid specifying AccessRightId</param>
      /// <returns></returns>
      public bool DeleteAccessRightById(Guid AccessRightId)
      {
          return AccessRightDataProvider.Instance.DeleteAccessRight(AccessRightId);
      }

      /// <summary>
      /// Delete Access rights by Access right id
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public bool DeleteAccessRightsByUserId(Guid UserId)
      {
          return AccessRightDataProvider.Instance.DeleteAccessRightByuserId(UserId);
      }

      /// <summary>
      /// Delete selected access rights by user id
      /// </summary>
      /// <param name="UserId">string specifying UserId</param>
      /// <returns></returns>
      public bool DeleteSelectedAccessRightByUserId(string UserId)
      {
          return AccessRightDataProvider.Instance.DeleteSelectedAccessRightByUserId(UserId);

      }
  }
}
