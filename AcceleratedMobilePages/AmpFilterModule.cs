using System;
using AcceleratedMobilePages;
using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.OutputFilter;

[assembly: RegisterModule(typeof(AmpFilterModule))]

namespace AcceleratedMobilePages
{
    class AmpFilterModule : Module
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

            // Ensures that the an output filter instance is created on every request
            RequestEvents.PostMapRequestHandler.Execute += PostMapRequestHandler_Execute;

            // Instantiates the AMP filter logic
            ampFilter = new AmpFilter();
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