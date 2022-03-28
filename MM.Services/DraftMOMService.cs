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
   public class DraftMOMService
    {
        /// <summary>
        /// Get data for wcf services
        /// </summary>
        /// <param name="userId">Guid specifying UserId</param>
        /// <param name="startTime">DateTime specifying startTime</param>
        /// <returns></returns>
        public DataSet GetDraftMOMWcf(Guid userId, DateTime startTime)
        {
            return DraftMOMDataProvider.Instance.DraftMOMWcf(userId, startTime);
        }

    }
}
