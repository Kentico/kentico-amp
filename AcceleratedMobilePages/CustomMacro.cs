using AcceleratedMobilePages;
using CMS;
using CMS.DataEngine;
using CMS.MacroEngine;

// Registers the custom module into the system
[assembly: RegisterModule(typeof(CustomMacro))]

namespace AcceleratedMobilePages
{
    class CustomMacro : Module
    {
        // Module class constructor, the system registers the module under the name "CustomMacros"
        public CustomMacro()
            : base("CustomMacros")
        {
        }

        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            // Registers "CustomNamespace" into the macro engine
            MacroContext.GlobalResolver.SetNamedSourceData("AmpFilter", CustomMacroNamespace.Instance);
        }
    }
}
