using System;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;

namespace Kentico.AcceleratedMobilePages
{
    public partial class AmpFilterInfo
    {
        /// <summary>
        /// Gets or sets the display name of the AMP Page.
        /// </summary>
        protected override string ObjectDisplayName
        {
            get
            {
                // Get document name
                TreeProvider tp = new TreeProvider();
                TreeNode tn = tp.SelectSingleNode(new Guid(PageNodeGUID), LocalizationContext.CurrentCulture.CultureCode, SiteContext.CurrentSiteName);
                return tn != null ? tn.DocumentName : base.ObjectDisplayName;
            }
            set
            {
                base.ObjectDisplayName = value;
            }
        }
    }
}