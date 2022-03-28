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
 public class PhotoGalleryService
    {
     /// <summary>
     /// Get Photo gallery by id
     /// </summary>
     /// <param name="PhotoId">Guid specifying PhotoId</param>
     /// <returns></returns>
     public PhotoGalleryDomain Get(Guid PhotoId)
     {
         return PhotoGalleryDataProvider.Instance.Get(PhotoId);
     }

     /// <summary>
     /// Get all photos
     /// </summary>
     /// <returns></returns>
     public IList<PhotoGalleryDomain> GetAllPhoto(Guid EntityId)
     {
         return PhotoGalleryDataProvider.Instance.GetPhotoByEntity(EntityId);
     }

     /// <summary>
     /// Get photo details for wcf
     /// </summary>
     /// <param name="StartTime">DateTime specifying StartTime</param>
     /// <param name="EndTime">DateTime specifying EndTime</param>
     /// <returns></returns>
     public IList<PhotoGalleryDomain> GetAllPhotoWcf(DateTime StartTime, DateTime EndTime, Guid EntityId)
     {
         return PhotoGalleryDataProvider.Instance.GetPhotoInfo(StartTime, EndTime, EntityId);
     }

     /// <summary>
     /// Insert photo details 
     /// </summary>
     /// <param name="Photograph">string specifying Photograph</param>
     /// <param name="Description">string specifying Description</param>
     /// <param name="CreatedBy">Guid specifying CreatedBy</param>
     /// <param name="EntityId">Guid specifying EntityId</param>
     /// <returns></returns>
     public PhotoGalleryDomain Insert(string Photograph, string Description, Guid CreatedBy, Guid EntityId)
     {
         PhotoGalleryDomain objPhoto = new PhotoGalleryDomain();
         objPhoto.Photograph = Photograph;
         objPhoto.Description = Description;
         objPhoto.EntityId = EntityId;
         objPhoto.CreatedBy = CreatedBy;
         objPhoto = PhotoGalleryDataProvider.Instance.Insert(objPhoto);
         return objPhoto;
     }

     /// <summary>
     /// Update photo details
     /// </summary>
     /// <param name="Photograph">string specifying Photograph</param>
     /// <param name="Description">string specifying Description</param>
     /// <param name="CreatedBy">Guid specifying CreatedBy</param>
     /// <param name="EntityId">Guid specifying EntityId</param>
     /// <param name="PhotoId">Guid specifying PhotoId</param>
     /// <returns></returns>
     public bool Update(string Photograph, string Description, Guid CreatedBy, Guid EntityId, Guid PhotoId)
     {
         PhotoGalleryDomain objPhoto = new PhotoGalleryDomain();
         objPhoto.Photograph = Photograph;
         objPhoto.Description = Description;
         objPhoto.EntityId = EntityId;
         objPhoto.CreatedBy = CreatedBy;
         objPhoto.PhotoGalleryId = PhotoId;
         return PhotoGalleryDataProvider.Instance.UpdatePhoto(objPhoto);
     }

     /// <summary>
     /// Delete photo details 
     /// </summary>
     /// <param name="PhotoId">Guid specifying PhotoId</param>
     /// <returns></returns>
     public bool Delete(Guid PhotoId)
     {
         return PhotoGalleryDataProvider.Instance.DeletePhotoById(PhotoId);
     }
    }
}
