using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.SiteProvider;

namespace AcceleratedMobilePages.AcceleratedMobilePages
{
    public static class CheckStateHelper
    {
		/// <summary>
        /// Returns the state of the filter:
        ///  DISABLED - filter is disabled for current page
        ///  ENABLED_AND_ACTIVE - filter is enabled for current page and activated (page is being accessed from AMP domain)
        ///  ENABLED_AND_INACTIVE - filter is enabled for current page but not active
        /// </summary>
        public static int CheckFilterState()
        {
            // Check if AMP filter is enabled in site settings
            if (!SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".AMPFilterEnabled"))
            {
                return Constants.DISABLED;
            }

            // Apply filter only in LiveSite or Preview mode
            if (PortalContext.ViewMode != ViewModeEnum.LiveSite && PortalContext.ViewMode != ViewModeEnum.Preview)
            {
                return Constants.DISABLED;
            }

            // Check if current page is allowed to use AMP filter
            ObjectQuery<AmpFilterInfo> q = AmpFilterInfoProvider.GetAmpFilters().WhereEquals("PageNodeGuid", DocumentContext.CurrentPageInfo.NodeGUID.ToString());
            if (q.Count == 0)
            {
                return Constants.DISABLED;
            }

            // Check if the domain from which we are accessing is set for AMP filter
            string currentDomain = CMSHttpContext.Current.Request.Url.IsDefaultPort ? CMSHttpContext.Current.Request.Url.Host : CMSHttpContext.Current.Request.Url.Host + ":" + CMSHttpContext.Current.Request.Url.Port;
            string ampDomain = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterDomainAlias");
            if (!currentDomain.Equals(ampDomain))
            {
                return Constants.ENABLED_AND_INACTIVE;
            }

            return Constants.ENABLED_AND_ACTIVE;
        }
    }
}