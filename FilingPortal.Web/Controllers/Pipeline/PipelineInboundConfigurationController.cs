using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.Pipeline;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller that provides actions for Inbound Record field configuration
    /// </summary> 
    [RoutePrefix("api/inbound/pipeline/field-config")]
    public class PipelineInboundConfigurationController : ApiControllerBase
    {
        /// <summary>
        /// The configuration builder
        /// </summary>
        private readonly IPipelineInboundFieldsConfigurationBuilder _configurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundConfigurationController"/> class
        /// </summary>
        public PipelineInboundConfigurationController(IPipelineInboundFieldsConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        /// <summary>
        /// Gets configuration by filing header identifier
        /// </summary>
        [HttpPost]
        [Route("{filingHeaderId:int}")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public InboundRecordFieldConfiguration GetConfiguration(int filingHeaderId)
        {
            return _configurationBuilder.Build(filingHeaderId);
        }
    }
}
