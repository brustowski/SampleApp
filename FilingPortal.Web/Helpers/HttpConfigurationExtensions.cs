using System.Web.Http;

namespace FilingPortal.Web.Helpers
{
    /// <summary>
    /// Provide helpers for Http Configuration
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Provides inheritance of route attributes in api controllers
        /// </summary>
        /// <param name="config">Http configuration</param>
        public static void MapInheritedAttributeRoutes(this HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new InheritanceDirectRouteProvider());
        }
    }
}