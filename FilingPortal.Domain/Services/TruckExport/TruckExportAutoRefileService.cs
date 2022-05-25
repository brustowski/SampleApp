using FilingPortal.Domain.Common.Refile;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors;
using Framework.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Implements Truck export auto refile service
    /// </summary>
    public class TruckExportAutoRefileService : ITruckExportAutoRefileService
    {
        private readonly IRuleRepository<AutoCreateRecord> _autoCreateRecordRepository;
        private readonly IAppSettingsRepository _settingsRepository;
        private readonly IRefileAssistant<TruckExportRecord> _assistant;
        private readonly ITruckExportFilingService _procedureService;
        private readonly ITruckExportRepository _truckExportRepository;

        private readonly IUpdateRecordsProcessor _processor;

        /// <summary>
        /// Creates a new instance of <see cref="TruckExportAutoRefileService"/>
        /// </summary>
        /// <param name="autoCreateRecordRepository">Auto-create configurations repository</param>
        /// <param name="settingsRepository">Settings repository</param>
        /// <param name="assistant">Auto-file assistant</param>
        /// <param name="procedureService">Filing procedure service</param>
        /// <param name="truckExportRepository">Truck export repository</param>
        public TruckExportAutoRefileService(
            IRuleRepository<AutoCreateRecord> autoCreateRecordRepository,
            IAppSettingsRepository settingsRepository,
            IRefileAssistant<TruckExportRecord> assistant,
            ITruckExportFilingService procedureService,
            ITruckExportRepository truckExportRepository)
        {
            _autoCreateRecordRepository = autoCreateRecordRepository;
            _settingsRepository = settingsRepository;
            _assistant = assistant;
            _procedureService = procedureService;
            _truckExportRepository = truckExportRepository;

            var notMatched = new NotMatchedProcessor(truckExportRepository);
            var noConfig = new NoConfigProcessor(truckExportRepository, autoCreateRecordRepository);
            var matched = new MatchedProcessor(procedureService, truckExportRepository);
            var noRules = new NoRulesProcessor(truckExportRepository, procedureService);
            var lastProcessor = new LeftProcessor(truckExportRepository);

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
            var records = _truckExportRepository.GetAutoRefileRecords().ToList();
            if (records.Any())
            {
                // Run validation
                _truckExportRepository.Validate(records);

                await _processor.Process(records, user);
            }

            // Try to find email to send notifications
            AppSettings emails =
                _settingsRepository.Get("TruckExportAutoFileNotificationEmail");

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
    }
}
