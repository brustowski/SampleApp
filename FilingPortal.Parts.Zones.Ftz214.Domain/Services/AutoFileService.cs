using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.PluginEngine.Services;
using FilingPortal.PluginEngine.Services.Filing.Auto;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Services
{
    /// <summary>
    /// Describes autofile service for autofile entities
    /// </summary>
    internal class AutoFileService : AutofileServiceBase<InboundRecord>
    {
        /// <summary>
        /// Creates a new instance of <see cref="AutoFileService"/>
        /// </summary>
        /// <param name="autoCreateRecordRepository">Auto-file configuration repository</param>
        /// <param name="settingsRepository">Settings repository</param>
        /// <param name="assistant">Report assistant</param>
        /// <param name="procedureService">Autofile procedure service</param>
        /// <param name="autofileRepository">Auto-file entities repository</param>
        /// <param name="validationRepository">Validation methods repository</param>
        public AutoFileService(IRuleRepository<AutoCreateRecord> autoCreateRecordRepository,
            IAppSettingsRepository settingsRepository,
            IRefileAssistant<InboundRecord> assistant,
            IAutofilingService<InboundRecord> procedureService,
            IAutofileMethodsRepository<InboundRecord> autofileRepository,
            IValidationRepository<InboundRecord> validationRepository) : base(autoCreateRecordRepository, settingsRepository, assistant, procedureService, autofileRepository, validationRepository)
        {
        }

        /// <summary>
        /// Gets auto-file email parameter name
        /// </summary>
        protected override string WorkflowAutoFileEmailParameterName => null;
    }
}
