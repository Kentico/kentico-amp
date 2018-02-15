using System;
using System.Collections.Generic;

using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.Modules;
using CMS.OutputFilter;

using Kentico.AcceleratedMobilePages;
using NuGet;

[assembly: RegisterModule(typeof(AmpFilterModule))]
namespace Kentico.AcceleratedMobilePages
{
    public class AmpFilterModule : Module
    {
        private AmpFilter ampFilter;


		/// <summary>
        /// Module class constructor, inherits from the base constructor with the code name of the module as the parameter
        /// </summary>
        public AmpFilterModule()
            : base("AcceleratedMobilePages")
        {
        }


		/// <summary>
        /// Initializes the module. Called when the application starts.
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            // Instantiates the AMP filter logic
            ampFilter = new AmpFilter();

            // Ensures that the output filter instance is created on every request
            RequestEvents.PostMapRequestHandler.Execute += PostMapRequestHandler_Execute;

            // Update nuspec manifest on module package creation
            ModulePackagingEvents.Instance.BuildNuSpecManifest.After += BuildNuSpecManifest_After;
        }


        /// <summary>
        /// Handler for PostMapRequest
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void PostMapRequestHandler_Execute(object sender, EventArgs e)
        {
            // Creates an output filter instance
            ResponseOutputFilter.EnsureOutputFilter();

            // Assigns a handler to the OutputFilterContext.CurrentFilter.OnAfterFiltering event
            OutputFilterContext.CurrentFilter.OnAfterFiltering += ampFilter.OnFilterActivated;
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
                    new ManifestDependency() { Id = "HtmlAgilityPack", Version = "1.4.9.5" },
                    new ManifestDependency() { Id = "NuGet.Core", Version = "2.14.0" }
                };

                var manifestDependencySet = new ManifestDependencySet() { Dependencies = dependencies };
                metadata.DependencySets = new List<ManifestDependencySet>() { manifestDependencySet };
            }            
        }
    }
}