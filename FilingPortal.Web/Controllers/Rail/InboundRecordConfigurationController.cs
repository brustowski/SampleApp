using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller that provides actions for Inbound Record field configuration
    /// </summary> 
    [RoutePrefix("api/inbound/rail/field-config")]
    public class InboundRecordConfigurationController : ApiControllerBase
    {
        /// <summary>
        /// The configuration builder
        /// </summary>
        private readonly IInboundRecordConfigurationBuilder _configurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfigurationController"/> class
        /// </summary>
        public InboundRecordConfigurationController(IInboundRecordConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        /// <summary>
        /// Gets configuration by filing header identifier
        /// </summary>
        [HttpPost]
        [Route("{filingHeaderId:int}")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public InboundRecordFieldConfiguration GetConfiguration(int filingHeaderId)
        {
            return _configurationBuilder.Build(filingHeaderId);
        }

        /// <summary>
        /// Gets configuration for single-filing by filing header identifiers
        /// </summary>
        [HttpPost]
        [Route("single-filing")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public InboundRecordFieldConfiguration SingleFilingGridContents([FromBody] int[] filingHeaderIds)
        {
            using (new MonitoredScope("Get single filing data"))
            {
                return _configurationBuilder.BuildSingleFiling(filingHeaderIds);
            }
        }
    }
}
