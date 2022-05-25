using System.Web.Http;
using FilingPortal.Domain.Mapping;
using Newtonsoft.Json;

namespace FilingPortal.Web
{
    /// <summary>
    /// Class providing Web API application configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers routes and services using the specified configuration
        /// </summary>
        /// <param name="config">The configuration</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = FormatsContainer.US_DATETIME_FORMAT;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
