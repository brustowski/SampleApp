using System.Web.Http;
using FilingPortal.Parts.Isf.Domain.Enums;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Isf.Web.Controllers
{

    /// <summary>
    /// Controller provides data for Vessel Export Records
    /// </summary>
    [RoutePrefix("api/isf")]
    public class IsfFormConfigController : ApiControllerBase
    {
        /// <summary>
        /// The form factory
        /// </summary>
        private readonly IFormConfigFactory<InboundRecordEditModel> _factory;

        /// <summary>
        ///  Initializes a new instance of the <see cref="IsfFormConfigController" /> class
        /// </summary>
        /// <param name="factory">The forms configuration factory</param>
        public IsfFormConfigController(IFormConfigFactory<InboundRecordEditModel> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Returns add new vessel form configuration
        /// </summary>
        [HttpGet]
        [Route("get-add-form-config")]
        [PermissionRequired(Permissions.AddInboundRecord)]
        public FormConfigModel GetAddFormConfig() => _factory.CreateFormConfig();
    }
}
