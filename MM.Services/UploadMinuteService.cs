﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
  public class UploadMinuteService
    {
      /// <summary>
      /// Get all upload Minutes
      /// </summary>
      /// <returns></returns>
      public IList<UploadMinutesDomain> GetAllUpoadMinutes()
      {
          return UploadMintesDataProvider.Instance.Get();
      }

      /// <summary>
      /// Get upload mintutes by id
      /// </summary>
      /// <param name="UploadMinuteId">Guid specifying UploadMinuteId</param>
      /// <returns></returns>
      public UploadMinutesDomain GetUploadMinutesById(Guid UploadMinuteId)
      {
          return UploadMintesDataProvider.Instance.Get(UploadMinuteId);
      }

      /// <summary>
      /// Insert Uplaod minutes
      /// </summary>
      /// <param name="UploadFile">Guid specifying UploadFile</param>
      /// <param name="MeetingId">Guid specifying MeetingId</param>
      /// <param name="UploadMinuteChecker">Guid specifying UploadMinuteChecker</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <returns></returns>
      public UploadMinutesDomain Insert(string UploadFile, Guid MeetingId, Guid UploadMinuteChecker, Guid CreatedBy, Guid UpdatedBy, string SendToChecker)
      {
          UploadMinutesDomain objUpload = new UploadMinutesDomain();
          objUpload.UploadFile = UploadFile;
          objUpload.UpdatedBy = UpdatedBy;
          objUpload.CreatedBy = CreatedBy;
          objUpload.UplaodMinuteChecker = UploadMinuteChecker;

          objUpload.IsApproved =  SendToChecker;
          objUpload = UploadMintesDataProvider.Instance.Insert(objUpload);
          return objUpload;
      }

      /// <summary>
      /// Update Upload minutes
      /// </summary>
      /// <param name="UploadFile">Guid specifying UploadFile</param>
      /// <param name="MeetingId">Guid specifying MeetingId</param>
      /// <param name="UploadMinuteChecker">Guid specifying UploadMinuteChecker</param>
      /// <param name="CreatedBy">Guid specifying CreatedBy</param>
      /// <param name="UpdatedBy">Guid specifying UpdatedBy</param>
      /// <param name="UploadMinuteId">Guid specifying UploadMinuteId</param>
      /// <returns></returns>
      public bool Update(string UploadFile, Guid MeetingId, Guid UploadMinuteChecker, Guid CreatedBy, Guid UpdatedBy, Guid UploadMinuteId, string SendToChecker)
      {
          UploadMinutesDomain objUpload = new UploadMinutesDomain();
          objUpload.UploadFile = UploadFile;
          objUpload.UpdatedBy = UpdatedBy;
          objUpload.CreatedBy = CreatedBy;
          objUpload.UplaodMinuteChecker = UploadMinuteChecker;
          objUpload.UploadMinuteId = UploadMinuteId;

          objUpload.IsApproved =  SendToChecker;
          return UploadMintesDataProvider.Instance.Update(objUpload);
      }

      /// <summary>
      /// Delete upload minutes
      /// </summary>
      /// <param name="UploadMinuteId">Guid specifying UploadMinuteId</param>
      /// <returns></returns>
      public bool Delete(Guid UploadMinuteId)
      {
          return UploadMintesDataProvider.Instance.Delete(UploadMinuteId);
      }

    }
}
