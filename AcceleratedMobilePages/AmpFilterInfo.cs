using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using AcceleratedMobilePages;
using CMS;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;

[assembly: RegisterObjectType(typeof(AmpFilterInfo), AmpFilterInfo.OBJECT_TYPE)]
    
namespace AcceleratedMobilePages
{
    /// <summary>
    /// AmpFilterInfo data container class.
    /// </summary>
	[Serializable]
    public class AmpFilterInfo : AbstractInfo<AmpFilterInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "acceleratedmobilepages.ampfilter";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AmpFilterInfoProvider), OBJECT_TYPE, "AcceleratedMobilePages.AmpFilter", "AmpFilterID", "AmpFilterLastModified", "AmpFilterGuid", null, null, null, "SiteID", null, null)
        {
			ModuleName = "AcceleratedMobilePages",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>
            {
			    new ObjectDependency("SiteID", "cms.site", ObjectDependencyEnum.Required), 
			    new ObjectDependency("StylesheetID", "cms.cssstylesheet", ObjectDependencyEnum.Required) 
            }
        };

        #endregion


        #region "Properties"

        /// <summary>
        /// Gets or sets the display name of the AMP Page.
        /// </summary>
        protected override string ObjectDisplayName
        {
            get
            {
                // Get document name
                TreeProvider tp = new TreeProvider();
                TreeNode tn = tp.SelectSingleNode(new Guid(PageNodeGuid), LocalizationContext.CurrentCulture.CultureCode, SiteContext.CurrentSiteName);
                return tn != null ? tn.DocumentName : base.ObjectDisplayName;
            }
            set
            {
                base.ObjectDisplayName = value;
            }
        }

        /// <summary>
        /// Amp filter ID
        /// </summary>
        [DatabaseField]
        public virtual int AmpFilterID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("AmpFilterID"), 0);
            }
            set
            {
                SetValue("AmpFilterID", value);
            }
        }


        /// <summary>
        /// Site ID
        /// </summary>
        [DatabaseField]
        public virtual int SiteID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SiteID"), 0);
            }
            set
            {
                SetValue("SiteID", value);
            }
        }


        /// <summary>
        /// Page node guid
        /// </summary>
        [DatabaseField]
        public virtual string PageNodeGuid
        {
            get
            {
                return ValidationHelper.GetString(GetValue("PageNodeGuid"), String.Empty);
            }
            set
            {
                SetValue("PageNodeGuid", value);
            }
        }


        /// <summary>
        /// Use the default stylesheet, same for every AMP page.
        /// </summary>
        [DatabaseField]
        public virtual bool UseDefaultStylesheet
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("UseDefaultStylesheet"), true);
            }
            set
            {
                SetValue("UseDefaultStylesheet", value);
            }
        }


        /// <summary>
        /// Stylesheet ID
        /// </summary>
        [DatabaseField]
        public virtual int StylesheetID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("StylesheetID"), 0);
            }
            set
            {
                SetValue("StylesheetID", value, 0);
            }
        }


        /// <summary>
        /// Amp filter guid
        /// </summary>
        [DatabaseField]
        public virtual Guid AmpFilterGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("AmpFilterGuid"), Guid.Empty);
            }
            set
            {
                SetValue("AmpFilterGuid", value);
            }
        }


        /// <summary>
        /// Amp filter last modified
        /// </summary>
        [DatabaseField]
        public virtual DateTime AmpFilterLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("AmpFilterLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("AmpFilterLastModified", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            AmpFilterInfoProvider.DeleteAmpFilterInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            AmpFilterInfoProvider.SetAmpFilterInfo(this);
        }

        #endregion


        #region "Constructors"

		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected AmpFilterInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty AmpFilterInfo object.
        /// </summary>
        public AmpFilterInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new AmpFilterInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public AmpFilterInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}