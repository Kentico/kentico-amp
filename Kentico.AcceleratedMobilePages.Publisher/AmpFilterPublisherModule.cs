using System;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Modules;

using Kentico.AcceleratedMobilePages.Publisher;
using NuGet;

[assembly: RegisterModule(typeof(AmpFilterPublisherModule))]

namespace Kentico.AcceleratedMobilePages.Publisher
{
    public class AmpFilterPublisherModule : Module
    {
        /// <summary>
        /// Module class constructor
        /// </summary>
        public AmpFilterPublisherModule()
            : base("AcceleratedMobilePages.Publisher")
        {
        }


        /// <summary>
        /// Initializes the module
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            // Update nuspec manifest on module package creation
            ModulePackagingEvents.Instance.BuildNuSpecManifest.After += BuildNuSpecManifest_After;
        }


        /// <summary>
        /// Updates nuspec manifest
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void BuildNuSpecManifest_After(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (e.ResourceName.Equals("Kentico.AcceleratedMobilePages", StringComparison.InvariantCultureIgnoreCase))
            {
                var metadata = e.Manifest.Metadata;

                metadata.RequireLicenseAcceptance = true;
                metadata.LicenseUrl = "https://github.com/Kentico/kentico-amp/blob/master/LICENSE";
                metadata.IconUrl = "http://www.kentico.com/favicon.ico";
                metadata.Copyright = "© 2018 Kentico Software";
                metadata.Language = "en-US";
                metadata.Tags = "kentico amp";

                var dependencies = new List<ManifestDependency>()
                {
                    new ManifestDependency() { Id = "HtmlAgilityPack", Version = "1.4.9.5" }
                };

                var manifestDependencySet = new ManifestDependencySet() { Dependencies = dependencies };
                metadata.DependencySets = new List<ManifestDependencySet>() { manifestDependencySet };
            }
        }
    }
}
