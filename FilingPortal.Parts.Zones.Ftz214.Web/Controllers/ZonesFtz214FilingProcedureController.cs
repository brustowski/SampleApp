using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.FieldConfigurations;
using System.Web.Http;
using Permissions = FilingPortal.Parts.Zones.Ftz214.Domain.Enums.Permissions;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Controllers
{
    /// <summary>
    /// Controller that provides actions for Inbound Records
    /// </summary>
    [RoutePrefix("api/zones/ftz-214/filing")]
    public class ZonesFtz214FilingProcedureController : FilingControllerBase<InboundRecord, FilingHeader, DefValueManual, DefValueManualReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesFtz214FilingProcedureController"/> class
        /// </summary>
        /// <param name="procedureService">The filing procedure service</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="configurationFactory">Fields configuration builder</param>
        /// <param name="parametersService">The filing parameters service</param>
        /// <param name="inboundRecordsRepository"></param>
        public ZonesFtz214FilingProcedureController(
            IFilingService<InboundRecord> procedureService,
            IFilingHeadersRepository filingHeaderRepository,
            IFilingConfigurationFactory<DefValueManualReadModel> configurationFactory,
            IFilingParametersService<DefValueManual, DefValueManualReadModel> parametersService,
            IValidationRepository<InboundRecord> inboundRecordsRepository
            ) : base(procedureService, filingHeaderRepository, filingHeaderRepository, configurationFactory, inboundRecordsRepository, parametersService)
        {

        }

        /// <summary>
        /// Returns permission for filing
        /// </summary>
        protected override int FilePermission => (int)Permissions.FileInboundRecord;

        /// <summary>
        /// Returns permission for view
        /// </summary>
        protected override int ViewPermission => (int)Permissions.ViewInboundRecord;
    }
}