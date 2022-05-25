using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FilingPortal.Web.App_Start
{
    /// <summary>
    /// Provides methods to configure application view engines
    /// </summary>
    public static class ViewEngineConfig
    {
        /// <summary>
        /// Configure Application view engines 
        /// </summary>
        /// <param name="viewEngines">Collection of the application view engines</param>
        public static void ConfigureViewEngine(this ICollection<IViewEngine> viewEngines)
        {
            RazorViewEngine razorEngine = viewEngines.OfType<RazorViewEngine>().FirstOrDefault();
            if (razorEngine == null)
            {
                razorEngine = new RazorViewEngine();
                viewEngines.Add(razorEngine);
            }
            razorEngine.ViewLocationFormats = razorEngine.ViewLocationFormats.Concat(new string[] { "~/ui/{0}.cshtml" }).ToArray();
        }
    }
}