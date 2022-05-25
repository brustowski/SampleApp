using FilingPortal.Domain.Enums;
using FilingPortal.Web.Models.Vessel;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;

namespace FilingPortal.Web.Controllers.Vessel
{

    /// <summary>
    /// Controller provides data for Vessel Inbound Records
    /// </summary>
    [RoutePrefix("api/inbound/vessel")]
    public class VesselFormConfigController : ApiControllerBase
    {
        /// <summary>
        /// The Manifest factory
        /// </summary>
        private readonly IFormConfigFactory<VesselImportEditModel> _factory;

        /// <summary>
        ///  Initializes a new instance of the <see cref="VesselFormConfigController" /> class
        /// </summary>
        /// <param name="factory">The forms configuration factory</param>
        public VesselFormConfigController(IFormConfigFactory<VesselImportEditModel> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Returns add new vessel form configuration
        /// </summary>
        [HttpGet]
        [Route("get-add-form-config")]
        [PermissionRequired(Permissions.VesselAddImportRecord)]
        public FormConfigModel GetAddFormConfig() => _factory.CreateFormConfig();
    }
}
