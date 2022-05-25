using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Processes records without configuration
    /// </summary>
    /// <typeparam name="TAutofileEntityType">Auto-file entity</typeparam>
    internal class NoConfigProcessor<TAutofileEntityType> : BaseProcessor<TAutofileEntityType>
        where TAutofileEntityType : Entity, IAutoFilingEntity
    {
        private readonly IRuleRepository<AutoCreateRecord> _autoCreateRecordRepository;

        /// <summary>
        /// Creates a new instance of <see cref="NoConfigProcessor{TAutofileEntityType}"/>
        /// </summary>
        /// <param name="inboundRepository">Repository of auto-file entities</param>
        /// <param name="autoCreateRecordRepository">Repository of auto-create configurations</param>
        public NoConfigProcessor(IRepository<TAutofileEntityType> inboundRepository,
            IRuleRepository<AutoCreateRecord> autoCreateRecordRepository) : base(inboundRepository)
        {
            _autoCreateRecordRepository = autoCreateRecordRepository;
        }

        /// <summary>
        /// Finds records with config in auto-filing repository
        /// </summary>
        /// <param name="records">Records</param>
        protected override IList<TAutofileEntityType> FindRecordsToProcess(IList<TAutofileEntityType> records)
        {
            // Try to find configuration for each record
            var configurations = _autoCreateRecordRepository.GetAll().ToList();

            var recordsWithConfig =
                records.Where(x => configurations.Any(y => y.ImporterExporter == x.AutoFileConfigId)).ToList();
            return records.Except(recordsWithConfig).ToList();
        }
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        protected override IList<TAutofileEntityType> Run(IList<TAutofileEntityType> records, AppUsersModel user)
        {
            foreach (TAutofileEntityType inboundRecord in records)
            {
                // For not found configurations
                //  send error
                SendError(inboundRecord, "Auto-file configuration not found");
            }

            return Enumerable.Empty<TAutofileEntityType>().ToList();
        }
    }
}