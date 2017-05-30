using System;
using CMS.DataEngine;

namespace AcceleratedMobilePages
{    
    /// <summary>
    /// Class providing AmpFilterInfo management.
    /// </summary>
    public class AmpFilterInfoProvider : AbstractInfoProvider<AmpFilterInfo, AmpFilterInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public AmpFilterInfoProvider()
            : base(AmpFilterInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the AmpFilterInfo objects.
        /// </summary>
        public static ObjectQuery<AmpFilterInfo> GetAmpFilters()
        {
            return ProviderObject.GetAmpFiltersInternal();
        }


        /// <summary>
        /// Returns AmpFilterInfo with specified ID.
        /// </summary>
        /// <param name="id">AmpFilterInfo ID</param>
        public static AmpFilterInfo GetAmpFilterInfo(int id)
        {
            return ProviderObject.GetAmpFilterInfoInternal(id);
        }


        /// <summary>
        /// Returns AmpFilterInfo with specified GUID.
        /// </summary>
        /// <param name="guid">AmpFilterInfo GUID</param>                
        public static AmpFilterInfo GetAmpFilterInfo(Guid guid)
        {
            return ProviderObject.GetAmpFilterInfoInternal(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified AmpFilterInfo.
        /// </summary>
        /// <param name="infoObj">AmpFilterInfo to be set</param>
        public static void SetAmpFilterInfo(AmpFilterInfo infoObj)
        {
            ProviderObject.SetAmpFilterInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified AmpFilterInfo.
        /// </summary>
        /// <param name="infoObj">AmpFilterInfo to be deleted</param>
        public static void DeleteAmpFilterInfo(AmpFilterInfo infoObj)
        {
            ProviderObject.DeleteAmpFilterInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes AmpFilterInfo with specified ID.
        /// </summary>
        /// <param name="id">AmpFilterInfo ID</param>
        public static void DeleteAmpFilterInfo(int id)
        {
            AmpFilterInfo infoObj = GetAmpFilterInfo(id);
            DeleteAmpFilterInfo(infoObj);
        }

        #endregion


        #region "Public methods - Advanced"


        /// <summary>
        /// Returns a query for all the AmpFilterInfo objects of a specified site.
        /// </summary>
        /// <param name="siteId">Site ID</param>
        public static ObjectQuery<AmpFilterInfo> GetAmpFilters(int siteId)
        {
            return ProviderObject.GetAmpFiltersInternal(siteId);
        }
        
        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the AmpFilterInfo objects.
        /// </summary>
        protected virtual ObjectQuery<AmpFilterInfo> GetAmpFiltersInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Returns AmpFilterInfo with specified ID.
        /// </summary>
        /// <param name="id">AmpFilterInfo ID</param>        
        protected virtual AmpFilterInfo GetAmpFilterInfoInternal(int id)
        {	
            return GetInfoById(id);
        }


        /// <summary>
        /// Returns AmpFilterInfo with specified GUID.
        /// </summary>
        /// <param name="guid">AmpFilterInfo GUID</param>
        protected virtual AmpFilterInfo GetAmpFilterInfoInternal(Guid guid)
        {
            return GetInfoByGuid(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified AmpFilterInfo.
        /// </summary>
        /// <param name="infoObj">AmpFilterInfo to be set</param>        
        protected virtual void SetAmpFilterInfoInternal(AmpFilterInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified AmpFilterInfo.
        /// </summary>
        /// <param name="infoObj">AmpFilterInfo to be deleted</param>        
        protected virtual void DeleteAmpFilterInfoInternal(AmpFilterInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion


        #region "Internal methods - Advanced"


        /// <summary>
        /// Returns a query for all the AmpFilterInfo objects of a specified site.
        /// </summary>
        /// <param name="siteId">Site ID</param>
        protected virtual ObjectQuery<AmpFilterInfo> GetAmpFiltersInternal(int siteId)
        {
            return GetObjectQuery().OnSite(siteId);
        }    
        
        #endregion		
    }
}