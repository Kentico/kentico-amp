using System;

using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.OutputFilter;

using Kentico.AcceleratedMobilePages;

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
    }
}