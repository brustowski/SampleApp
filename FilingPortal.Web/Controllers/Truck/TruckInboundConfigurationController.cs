using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.Truck;

namespace FilingPortal.Web.Controllers.Truck
{
    /// <summary>
    /// Controller that provides actions for Inbound Record field configuration
    /// </summary> 
    [RoutePrefix("api/inbound/truck/field-config")]
    public class TruckInboundConfigurationController : ApiControllerBase
    {
        /// <summary>
        /// The configuration builder
        /// </summary>
        private readonly ITruckInboundFieldsConfigurationBuilder _configurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundConfigurationController"/> class
        /// </summary>
        public TruckInboundConfigurationController(ITruckInboundFieldsConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder;
        }

        /// <summary>
        /// Gets configuration by filing header identifier
        /// </summary>
        [HttpPost]
        [Route("{filingHeaderId:int}")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public InboundRecordFieldConfiguration GetConfiguration(int filingHeaderId)
        {
            return _configurationBuilder.Build(filingHeaderId);
        }
    }
}
