using CMS.DataEngine;
using CMS.SiteProvider;
using System.Collections.Generic;

namespace Kentico.AcceleratedMobilePages
{
    public static class Settings
    {
        /// <summary>
        /// Gets extensions that the system adds to page URLs on current site.
        /// </summary>
        public static string CmsFriendlyUrlExtension => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSFriendlyURLExtension").Split(';')[0];


        /// <summary>
        /// Indicates if AMP Filter is enabled on current site.
        /// </summary>
        public static bool AmpFilterEnabled => SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".AMPFilterEnabled");


        /// <summary>
        /// Gets domain alias for serving AMP pages on current site.
        /// </summary>
        public static string AmpFilterDomainAlias => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterDomainAlias");


        /// <summary>
        /// Gets list of font providers allowed for serving via link HTML tag on current site.
        /// </summary>
        public static IList<string> AmpFilterFontProviders => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterFontProviders").Split('\n');


        /// <summary>
        /// Gets URL for the amp-form script on current site.
        /// </summary>
        public static string AmpFilterFormScriptUrl => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterFormScriptUrl");


        /// <summary>
        /// Gets URL for the amp-video script on current site.
        /// </summary>
        public static string AmpFilterVideoScriptUrl => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterVideoScriptUrl");


        /// <summary>
        /// Gets URL for the amp-audio script on current site.
        /// </summary>
        public static string AmpFilterAudioScriptUrl => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterAudioScriptUrl");


        /// <summary>
        /// Gets URL for the amp-iframe script on current site.
        /// </summary>
        public static string AmpFilterIframeScriptUrl => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterIframeScriptUrl");


        /// <summary>
        /// Gets URL for the runtime script on current site.
        /// </summary>
        public static string AmpFilterRuntimeScriptUrl => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterRuntimeScriptUrl");


        /// <summary>
        /// Gets default stylesheet for AMP pages on current site.
        /// </summary>
        public static string AmpFilterDefaultCSS => SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".AMPFilterDefaultCSS");
    }
}