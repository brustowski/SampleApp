using System.Web.Http;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Models;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Controller providing the application version information
    /// </summary>
    [RoutePrefix("api/appversion")]
    public class AppVersionController : ApiControllerBase
    {
        /// <summary>
        /// Gets the application version model
        /// </summary>
        [HttpGet]
        [Route("get")]
        public AppVersionViewModel Get()
        {
            return new AppVersionViewModel
            {
                ShortAppVersion = AppVersion.GetShortAppVersion(),
                AppVersion = AppVersion.GetAppVersion(),
                AppBuildDate = AppVersion.GetAppBuildDate().ToString()
            };
        }
    }
}