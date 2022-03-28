using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MM.Data
{
    #region "Public property of PhotoGallery"
    public  class PhotoGalleryDomain
    {
        public Guid PhotoGalleryId { get; set; }       
        public string Description { get; set; }
        public string Photograph { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive  { get; set; }
         public Guid EntityId { get; set; }   
    }
    #endregion
}
