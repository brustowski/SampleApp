using System;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Services.Filing;
using FilingPortal.PluginEngine.Services.Filing.Auto;
using FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors;
using Framework.Domain;
using Framework.Infrastructure;

namespace FilingPortal.PluginEngine.Services
{
    /// <summary>
    /// It's base autofiling service class
    /// </summary>
    /// <typeparam name="TInboundType">Workflow inbound id</typeparam>
    public abstract class AutofileServiceBase<TInboundType> : IAutoFileService<TInboundType>
    where TInboundType : Entity, IAutoFilingEntity, IValidationRequiredEntity
    {
        private readonly IAppSettingsRepository _settingsRepository;
        private readonly IRefileAssistant<TInboundType> _assistant;
        private readonly IAutofileMethodsRepository<TInboundType> _autofileRepository;
        private readonly IValidationRepository<TInboundType> _validationRepository;
        private readonly IUpdateRecordsProcessor<TInboundType> _processor;

        protected AutofileServiceBase(
            IRuleRepository<AutoCreateRecord> autoCreateRecordRepository,
            IAppSettingsRepository settingsRepository, IRefileAssistant<TInboundType> assistant,
            IAutofilingService<TInboundType> procedureService,
            IAutofileMethodsRepository<TInboundType> autofileRepository,
            IValidationRepository<TInboundType> validationRepository
            )
        {
            _settingsRepository = settingsRepository;
            _assistant = assistant;
            _autofileRepository = autofileRepository;
            _validationRepository = validationRepository;

            var notMatched = new NotMatchedProcessor<TInboundType>(autofileRepository);
            var noConfig = new NoConfigProcessor<TInboundType>(autofileRepository, autoCreateRecordRepository);
            var matched = new MatchedProcessor<TInboundType>(procedureService, autofileRepository);
            var noRules = new NoRulesProcessor<TInboundType>(autofileRepository, procedureService);
            var lastProcessor = new LeftProcessor<TInboundType>(autofileRepository);

            notMatched.SetSuccessor(noConfig);
            noConfig.SetSuccessor(matched);
            matched.SetSuccessor(noRules);
            noRules.SetSuccessor(lastProcessor);

            assistant.WatchOver(notMatched);
            assistant.WatchOver(noConfig);
            assistant.WatchOver(matched);
            assistant.WatchOver(noRules);
            assistant.WatchOver(lastProcessor);

            _processor = notMatched;
        }

        /// <summary>
        /// Executes auto-refile and returns report
        /// </summary>
        /// <param name="user">Current user</param>
        public async Task<string> Execute(AppUsersModel user)
        {
            // Create report
            _assistant.CreateErrorReport();

            // Get non-processed records
            var records = _autofileRepository.GetAutoRefileRecords().ToList();
            if (records.Any())
            {
                // Run validation
                _validationRepository.Validate(records);

                await _processor.Process(records, user);
            }

            // Try to find email to send notifications
            AppSettings emails =
                _settingsRepository.Get(WorkflowAutoFileEmailParameterName);

            // Send report if not empty
            try
            {
                await _assistant.SendReport(emails?.Value);
            }
            catch (Exception e)
            {
                AppLogger.Error(e, "Error when sending email");
            }

            return _assistant.PrintReport();
        }

        /// <summary>
        /// Gets auto-file email parameter name
        /// </summary>
        protected abstract string WorkflowAutoFileEmailParameterName { get; }
    }
}
