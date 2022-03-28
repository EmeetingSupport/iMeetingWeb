using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
  public  class RollServices
    {
        /// <summary>
        /// Get Roll by roll id
        /// </summary>
        /// <param name="RollMasterId">Guid specifying RollMasterId</param>
        /// <returns></returns>
        public RollDomain GetRollById(Guid RollMasterId)
        {
            return RollDataProvider.Instance.GetRollById(RollMasterId);
        }

      /// <summary>
      /// Get All Rolls
      /// </summary>
      /// <returns></returns>
      public IList<RollDomain> GetAllRolls()
      {
          return RollDataProvider.Instance.Get();
      }

      /// <summary>
      /// Get Roll By UserId
      /// </summary>
      /// <param name="UserId">Guid specifying UserId</param>
      /// <returns></returns>
      public RollDomain GetRollByUserId(Guid UserId)
      {
          return RollDataProvider.Instance.GetRollByUserId(UserId);
      }

      /// <summary>
      /// Insert Roll
      /// </summary>
      /// <param name="RollName">string specifying RollName</param>
      /// <param name="RollMaster">bool specifying Report</param>
      /// <param name="EntityMaster">bool specifying EntityMaster</param>
      /// <param name="UserMaster">bool specifying UserMaster</param>
      /// <param name="ApprovalMaster">bool specifying ApprovalMaster</param>
      /// <param name="AccessRightMaster">bool specifying AccessRightMaster</param>
      /// <param name="Transaction">bool specifying Transaction</param>
      /// <param name="Approval">bool specifying Approval</param>
      /// <param name="View">bool specifying View</param>
      /// <param name="Report">bool specifying Report</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <returns></returns>
      public RollDomain Insert(string RollName, bool RollMaster, bool EntityMaster, bool UserMaster, bool ApprovalMaster, bool AccessRightMaster, bool Transaction, bool Approval, bool View, bool Report, Guid CreatedBy, Guid UpdatedBy)
      {
          RollDomain objRoll = new RollDomain();
          objRoll.AccessRightMaster = AccessRightMaster;
          objRoll.Approval = Approval;
          objRoll.ApprovalMaster = ApprovalMaster;
          objRoll.CreatedBy = CreatedBy;
          objRoll.UpdatedBy = UpdatedBy;
          objRoll.Report = Report;

          objRoll.UserMaster = UserMaster;
          objRoll.EntityMaster = EntityMaster;
          objRoll.RollMaster = RollMaster;
          objRoll.RollName = RollName;
          objRoll.Transaction = Transaction;
          objRoll.View = View;
          objRoll = RollDataProvider.Instance.Insert(objRoll);
          return objRoll;
      }

      /// <summary>
      /// Update roll by id
      /// </summary>
      /// <param name="RollName">string specifying RollName</param>
      /// <param name="RollMaster">bool specifying Report</param>
      /// <param name="EntityMaster">bool specifying EntityMaster</param>
      /// <param name="UserMaster">bool specifying UserMaster</param>
      /// <param name="ApprovalMaster">bool specifying ApprovalMaster</param>
      /// <param name="AccessRightMaster">bool specifying AccessRightMaster</param>
      /// <param name="Transaction">bool specifying Transaction</param>
      /// <param name="Approval">bool specifying Approval</param>
      /// <param name="View">bool specifying View</param>
      /// <param name="Report">bool specifying Report</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <param name="RollMasterId">Guid specifying RollMasterId</param>
      /// <returns></returns>
      public bool Update(string RollName, bool RollMaster, bool EntityMaster, bool UserMaster, bool ApprovalMaster, bool AccessRightMaster, bool Transaction, bool Approval, bool View, bool Report, Guid CreatedBy, Guid UpdatedBy, Guid RollMasterId)
      {
          RollDomain objRoll = new RollDomain();
          objRoll.AccessRightMaster = AccessRightMaster;
          objRoll.Approval = Approval;
          objRoll.ApprovalMaster = ApprovalMaster;
          objRoll.CreatedBy = CreatedBy;
          objRoll.UpdatedBy = UpdatedBy;
          objRoll.Report = Report;

          objRoll.UserMaster = UserMaster;
          objRoll.EntityMaster = EntityMaster;
          objRoll.RollMaster = RollMaster;
          objRoll.RollName = RollName;
          objRoll.Transaction = Transaction;
          objRoll.View = View;
          objRoll.RollMasterId = RollMasterId;

          return RollDataProvider.Instance.Update(objRoll);        
      }

      /// <summary>
      /// Delete Roll by Id
      /// </summary>
      /// <param name="RollMasterId">Guid specifying RollMasterId</param>
      /// <returns></returns>
      public bool Delete(Guid RollMasterId)
      {
          return RollDataProvider.Instance.Delete(RollMasterId);
      }

      /// <summary>
      /// Delete selected rolls
      /// </summary>
      /// <param name="RollMasterIds">string specifying RollMasterIds</param>
      /// <returns></returns>
      public bool DeleteSelected(string RollMasterIds)
      {
          return RollDataProvider.Instance.DeleteSelected(RollMasterIds);
      }
    }
}
