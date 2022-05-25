using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.Controllers;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Provides actions for file template management
    /// </summary>
    [RoutePrefix("api/file-template")]
    public class FileTemplateController : ApiControllerBase
    {
        /// <summary>
        /// The grid configuration registry
        /// </summary>
        private readonly IGridConfigRegistry _gridConfigRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTemplateController"/> class.
        /// </summary>
        /// <param name="gridConfigRegistry">The grid configuration registry</param>
        public FileTemplateController(IGridConfigRegistry gridConfigRegistry)
        {
            _gridConfigRegistry = gridConfigRegistry;
        }

        /// <summary>
        /// Gets file template name by grid name
        /// </summary>
        /// <param name="gridName">The grid name</param>
        [HttpGet]
        [Route("by-grid-name/{gridName}")]
        [PermissionRequired]
        public HttpResponseMessage GetByGridName(string gridName)
        {
            var name = _gridConfigRegistry.GetGridConfig(gridName)?.TemplateFileName;

            if (string.IsNullOrWhiteSpace(name))
            {
                throw CreateResponseException(HttpStatusCode.BadRequest, $"The file template name was not found for grid name: {gridName}.");
            }

            var path = HostingEnvironment.MapPath("~/bin/App_Data/Templates");

            return SendAsFileStream(name, $"{path}/{name}");
        }
    }
}
