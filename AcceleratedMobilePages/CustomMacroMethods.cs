using AcceleratedMobilePages.AcceleratedMobilePages;
using CMS.MacroEngine;

namespace AcceleratedMobilePages
{
    public class CustomMacroMethods : MacroMethodContainer
    {
        [MacroMethod(typeof(bool), "Returns true if the current page is AMP page.", 1)]
        public static object IsAmpPage(EvaluationContext context, params object[] parameters)
        {
            return (CheckStateHelper.GetFilterState() == Constants.ENABLED_AND_ACTIVE);
        }
    }
}
