using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FilingPortal.Web.Common.Plugins;

namespace FilingPortal.Web
{
    /// <summary>
    /// Class  providing application route configuration
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes
        /// </summary>
        /// <param name="routes">The routes</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapMvcAttributeRoutes();

            new[]
            {
                "imports/{*url}",
                "exports/{*url}",
                "zones/{*url}", // TODO: change routes mechanism to provide routes from plugin itself
                "isf/{*url}",
                "rules/{*url}",
                "admin/{*url}",
                "client-management",
                "send-request",
                "audit/{*url}",
                "recon/{*url}"
            }.ToList().ForEach(
                route =>
                {
                    routes.MapRoute(
                        name:
                        String.Format(
                            "{0}_front",
                            Regex.Replace(route, @"[^A-Za-z\/]", Guid.NewGuid().ToString().Substring(0, 5))),
                        url: route,
                        defaults: new
                        {
                            controller = "Home",
                            action = "Index",
                            id = UrlParameter.Optional
                        }
                    );
                });

            routes.MapRoute(
                name: "Filing",
                url: "mvc/{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}