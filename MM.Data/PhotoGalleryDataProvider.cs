using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MM.Core;
using MM.Domain;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using MySql.Data.MySqlClient;

namespace MM.Data
{
    public class PhotoGalleryDataProvider : DataProvider
    {
        #region "Singleton"
        private static volatile PhotoGalleryDataProvider _instance;
        private static object _synRoot = new object();

        private PhotoGalleryDataProvider()
        {
        }

        public static PhotoGalleryDataProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PhotoGalleryDataProvider();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region "Fill"
        /// <summary>
        /// Fill Key info. details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        private PhotoGalleryDomain ToPhotoGalleryDomain(IDataReader dr)
        {
            return new PhotoGalleryDomain()
        {
            PhotoGalleryId = Null.SetNullGuid(dr["PhotoGalleryId"]),
            Photograph = Null.SetNullString(dr["Photograph"]),
            CreatedBy = Null.SetNullGuid(dr["CreatedBy"]),
            UpdatedBy = Null.SetNullGuid(dr["UpdatedBy"]),
            UpdatedOn = Null.SetNullDateTime(dr["UpdateOn"]),
            CreatedOn = Null.SetNullDateTime(dr["CreatedOn"]),
            EntityId = Null.SetNullGuid(dr["EntityId"]),
            Description = Null.SetNullString(dr["Description"]),
            IsActive = Null.SetNullBoolean(dr["IsActive"])
        };

        }

        /// <summary>
        /// Get All Key info details
        /// </summary>
        /// <param name="dr">IDataReader sepcifing dr</param>
        /// <returns></returns>
        public IList<PhotoGalleryDomain> ToPhotoGalleryDomains(IDataReader dr)
        {
            IList<PhotoGalleryDomain> objKeyInfoDomain = new List<PhotoGalleryDomain>();
            while (dr.Read())
            {
                objKeyInfoDomain.Add(ToPhotoGalleryDomain(dr));
            }
            return objKeyInfoDomain;
        }
        #endregion

         #region "Get"
        /// <summary>
        /// Get Photo by id
        /// </summary>
        /// <param name="PhotoId">Guid specifying PhotoId</param>
        /// <returns></returns>
        public PhotoGalleryDomain Get(Guid PhotoId)
        {
            PhotoGalleryDomain objPhotoGallery = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPhotoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_PhotoGalleryId", PhotoId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            //using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetPhotoById", PhotoId))
            {
                if (!dataReader.Read())
                {
                    return null;
                }
                objPhotoGallery = ToPhotoGalleryDomain(dataReader);
            }
            conn.Close();
            return objPhotoGallery;
        }

        /// <summary>
        /// Get All Key information 
        /// </summary>
        /// <returns></returns>
        public IList<PhotoGalleryDomain> GetPhotoByEntity(Guid EntityId)
        {
            IList<PhotoGalleryDomain> objPhoto = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetAllPhoto", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetAllPhoto", EntityId))
            {
                objPhoto = ToPhotoGalleryDomains(dataReader);
            }
            conn.Close();
            return objPhoto;
        }

        /// <summary>
        /// Get All Photos for wcf
        /// </summary>
        /// <returns></returns>
        public IList<PhotoGalleryDomain> GetPhotoInfo(DateTime StartTime, DateTime EndTime, Guid EntityId)
        {
            IList<PhotoGalleryDomain> objPhoto = null;

            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand("stp_GetPhotoGalleryWcf", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_StartTime", StartTime);
            cmd.Parameters.AddWithValue("EndTime", EndTime);
            cmd.Parameters.AddWithValue("EntityId", EntityId);
            conn.Open();
            MySqlDataReader dataReader = cmd.ExecuteReader();
            

            // using (SqlDataReader dataReader = SqlHelper.ExecuteReader(ConnectionString, "stp_GetPhotoGalleryWcf", StartTime, EndTime, EntityId))
            {
                objPhoto = ToPhotoGalleryDomains(dataReader);
            }
            conn.Close();
            return objPhoto;
        }
        #endregion

        #region "Modify"
        /// <summary>
        /// Insert Photo 
        /// </summary>
        /// <param name="PhotoDomain">Class object specifying PhotoDomain</param>
        /// <returns></returns>
        public PhotoGalleryDomain Insert(PhotoGalleryDomain PhotoDomain)
        {


            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_InsertPhoto", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_Description", PhotoDomain.Description);
            cmd.Parameters.AddWithValue("p_Photograph", PhotoDomain.Photograph);
            cmd.Parameters.AddWithValue("p_CreatedBy", PhotoDomain.CreatedBy);
            cmd.Parameters.AddWithValue("p_UpdatedBy", PhotoDomain.UpdatedBy);
            cmd.Parameters.AddWithValue("p_EntityId", PhotoDomain.EntityId);
            PhotoDomain.PhotoGalleryId = (Guid)(cmd.ExecuteScalar());
            conn.Close();
            //PhotoDomain.PhotoGalleryId = (Guid)SqlHelper.ExecuteScalar(ConnectionString, "stp_InsertPhoto", PhotoDomain.Description, PhotoDomain.Photograph, PhotoDomain.CreatedBy, PhotoDomain.UpdatedBy, PhotoDomain.EntityId);
            return PhotoDomain;
        }

        /// <summary>
        /// Delete photo by id
        /// </summary>
        /// <param name="PhotoId">Guid specifiying PhotoId</param>
        /// <returns></returns>
        public bool DeletePhotoById(Guid PhotoId)
        {
            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_DeletePhotoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_PhotoGalleryId", PhotoId);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;




           // return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_DeletePhotoById", PhotoId)) > 0;
        }

        /// <summary>
        /// Update KeyPhotoGallery
        /// </summary>
        /// <param name="KeyInfo">Class object specifying PhotoDomain</param>
        /// <returns></returns>
        public bool UpdatePhoto(PhotoGalleryDomain PhotoDomain)
        {

            bool res = false;
            string constr = ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("stp_UpdatePhotoById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_PhotoGalleryId", PhotoDomain.PhotoGalleryId);
            cmd.Parameters.AddWithValue("p_Description", PhotoDomain.Description);
            cmd.Parameters.AddWithValue("p_Photograph", PhotoDomain.Photograph);
            cmd.Parameters.AddWithValue("p_EntityId", PhotoDomain.EntityId);
            cmd.Parameters.AddWithValue("p_UpdatedBy", PhotoDomain.UpdatedBy);
            int a = Convert.ToInt32(cmd.ExecuteNonQuery());
            if (a > 0) { res = true; }
            conn.Close();
            return res;




            //return Convert.ToInt32(SqlHelper.ExecuteNonQuery(ConnectionString, "stp_UpdatePhotoById", PhotoDomain.PhotoGalleryId, PhotoDomain.Description, PhotoDomain.Photograph, PhotoDomain.UpdatedBy, PhotoDomain.EntityId)) > 0;
        }
        #endregion
    }
}
