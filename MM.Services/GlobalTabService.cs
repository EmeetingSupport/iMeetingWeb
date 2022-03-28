using MM.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MM.Services
{
    public class GlobalTabService
    {

        /// <summary>
        /// Get All key info
        /// </summary>
        /// <returns></returns>
        // IList<KeyInformationDomain>
        public DataSet GetAllGlobalTabInfoWcf(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            DataSet ds = GlobalTabDataProvider.Instance.GetGlobalTabInfo(StartTime, EndTime, EntityId);
            return ds;
        }
    }
}
