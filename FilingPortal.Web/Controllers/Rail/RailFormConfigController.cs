using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.Controllers.Rail
{

    /// <summary>
    /// Controller provides data for Rail Inbound Records
    /// </summary>
    [RoutePrefix("api/inbound/rail")]
    public class RailFormConfigController : ApiControllerBase
    {
        /// <summary>
        /// The Add new rail configuration factory
        /// </summary>
        private readonly IFormConfigFactory<RailInboundEditModel> _factory;

        /// <summary>
        ///  Initializes a new instance of the <see cref="RailFormConfigController" /> class
        /// </summary>
        /// <param name="factory">The forms configuration factory</param>
        public RailFormConfigController(IFormConfigFactory<RailInboundEditModel> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Returns add new rail inbound form configuration
        /// </summary>
        [HttpGet]
        [Route("get-add-form-config")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public FormConfigModel GetAddFormConfig() => _factory.CreateFormConfig();
    }
}
