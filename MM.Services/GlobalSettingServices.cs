using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Data;
using MM.Domain;

namespace MM.Services
{
   public class GlobalSettingServices
    {
       /// <summary>
       /// Get all globlal setting
       /// </summary>
       /// <returns></returns>
       public IList<GlobalSettingDomain> GetAllGlobalSetting(Guid EntityId)
       {
           return GlobalSettingDataProvider.Instance.GetGlobalSettingByEntity(EntityId);
       }

       /// <summary>
       /// Get Global Setting by id
       /// </summary>
       /// <param name="GlobalSettingId">Guid specifying GlobalSettingId</param>
       /// <returns></returns>
       public GlobalSettingDomain GetGlobalSettingByid(Guid GlobalSettingId)
       {
           return GlobalSettingDataProvider.Instance.GetByGlobalSettingID(GlobalSettingId);
       }

       /// <summary>
       /// Update global setting
       /// </summary>
       /// <param name="val"></param>
       /// <param name="GlobalSettingId">Guid specifying GlobalSettingId</param>
       /// <returns></returns>
       public bool UpdateGlobalSetting(string val, Guid GlobalSettingId)
       {
           GlobalSettingDomain objGlobal = new GlobalSettingDomain();
           objGlobal.Value = val;
           objGlobal.Id = GlobalSettingId;

           return GlobalSettingDataProvider.Instance.Update(objGlobal);
       }
    }
}
