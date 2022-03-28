using MM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MM.Services
{
   public class ProceedingService
    {

        /// <summary>
        /// Get All proceedings
        /// </summary>
        /// <returns></returns>

        public DataSet GetAllProceedingInfoWcf(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            DataSet ds = ProcedingDataProvider.Instance.GetProceedingInfo(StartTime, EndTime, EntityId);
            return ds;
        }
    }
}
