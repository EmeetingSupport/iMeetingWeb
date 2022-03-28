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
    public class KeyInformationServices
    {
        /// <summary>
        /// Get All key info
        /// </summary>
        /// <returns></returns>
        public IList<KeyInformationDomain> GetAllKeyInfo(Guid EntityId)
        {
            return KeyInformationDataProvider.Instance.GetKeyInfoByEntity(EntityId);
        }

        /// <summary>
        /// Get All key info
        /// </summary>
        /// <returns></returns>
        // IList<KeyInformationDomain>
        public DataSet GetAllKeyInfoWcf(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            return KeyInformationDataProvider.Instance.GetKeyInfo(StartTime,EndTime, EntityId);
        }

        /// <summary>
        /// Get key info by id
        /// </summary>
        /// <param name="keyInfoId">Guid specifying keyInfoId</param>
        /// <returns></returns>
        public KeyInformationDomain GetKeyInfoById(Guid keyInfoId)
        {
            return KeyInformationDataProvider.Instance.Get(keyInfoId);
        }

        /// <summary>
        /// Insert key info 
        /// </summary>
        /// <param name="Name">string specifying Name</param>
        /// <param name="Description">string specifying Description</param>
        /// <param name="Designation">string specifying Designation</param>
        /// <param name="Photograph">string specifying Photograph</param>
        /// <param name="CreatedBy">Guid specifying CreatedBy</param>
        /// <param name="Suffix">String specifying suffix </param>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public KeyInformationDomain InsertkeyInfo(string Name, string Description, string Designation, string Photograph, Guid CreatedBy, string Suffix, Guid EntityId)
        {
            KeyInformationDomain objKeyInfo = new KeyInformationDomain();
            objKeyInfo.Photograph = Photograph;
            objKeyInfo.Name = Name;
            objKeyInfo.CreatedBy = CreatedBy;
            objKeyInfo.UpdatedBy = CreatedBy;
            objKeyInfo.Designation = Designation;
            objKeyInfo.Description = Description;
            objKeyInfo.Suffix = Suffix;
            objKeyInfo = KeyInformationDataProvider.Instance.Insert(objKeyInfo);
            return objKeyInfo;
        }

        /// <summary>
        /// Update Key info
        /// </summary>
        /// <param name="Name">string specifying Name</param>
        /// <param name="Description">string specifying Description</param>
        /// <param name="Designation">string specifying Designation</param>
        /// <param name="Photograph">string specifying Photograph</param>
        /// <param name="UpdateBy">Guid specifying UpdateBy</param>
        /// <param name="KeyInfoId">Guid specifying KeyInfoId</param>
        /// <param name="Suffix">String specifying suffix </param>
        /// <param name="EntityId">Guid specifying EntityId</param>
        /// <returns></returns>
        public bool UpdateKeyInfo(string Name, string Description, string Designation, string Photograph, Guid UpdateBy, Guid KeyInfoId, string Suffix, Guid EntityId)
        {
            KeyInformationDomain objKeyInfo = new KeyInformationDomain();
            objKeyInfo.Photograph = Photograph;
            objKeyInfo.Name = Name;          
            objKeyInfo.UpdatedBy = UpdateBy;
            objKeyInfo.Designation = Designation;
            objKeyInfo.Description = Description;
            objKeyInfo.KeyInformationId = KeyInfoId;
            objKeyInfo.Suffix = Suffix;
            bool status = KeyInformationDataProvider.Instance.UpdateKeyInfo(objKeyInfo);
            return status;
        }

        /// <summary>
        /// Delete Key info by id
        /// </summary>
        /// <param name="KeyInfoId">Guid specifing KeyInfoId</param>
        /// <returns></returns>
        public bool DeleteKeyInfo(Guid KeyInfoId)
        {
            return KeyInformationDataProvider.Instance.DeleteKeyInfoById(KeyInfoId);
        }



    }
}
