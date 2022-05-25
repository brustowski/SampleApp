using FilingPortal.Domain.Enums;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.VesselExport;

namespace FilingPortal.Web.Controllers.VesselExport
{

    /// <summary>
    /// Controller provides data for Vessel Export Records
    /// </summary>
    [RoutePrefix("api/export/vessel")]
    public class VesselExportFormConfigController : ApiControllerBase
    {
        /// <summary>
        /// The Manifest factory
        /// </summary>
        private readonly IFormConfigFactory<VesselExportEditModel> _factory;

        /// <summary>
        ///  Initializes a new instance of the <see cref="VesselExportFormConfigController" /> class
        /// </summary>
        /// <param name="factory">The forms configuration factory</param>
        public VesselExportFormConfigController(IFormConfigFactory<VesselExportEditModel> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Returns add new vessel form configuration
        /// </summary>
        [HttpGet]
        [Route("get-add-form-config")]
        [PermissionRequired(Permissions.VesselAddExportRecord)]
        public FormConfigModel GetAddFormConfig() => _factory.CreateFormConfig();
    }
}
