using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal class NoConfigProcessor : BaseProcessor
    {
        private readonly IRuleRepository<AutoCreateRecord> _autoCreateRecordRepository;

        public NoConfigProcessor(ITruckExportRepository inboundRepository,
            IRuleRepository<AutoCreateRecord> autoCreateRecordRepository): base(inboundRepository)
        {
            _autoCreateRecordRepository = autoCreateRecordRepository;
        }

        protected override IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records)
        {
            // Try to find configuration for each record
            var configurations = _autoCreateRecordRepository.GetAll().ToList();

            var recordsWithConfig =
                records.Where(x => configurations.Any(y => y.ImporterExporter == x.Exporter)).ToList();
            return records.Except(recordsWithConfig).ToList();
        }

        protected override
            IList<TruckExportRecord> Run(IList<TruckExportRecord> records, AppUsersModel user)
        {
            foreach (TruckExportRecord inboundRecord in records)
            {
                // For not found configurations
                //  send error
                SendError(inboundRecord, "Auto-file configuration not found");
            }

            return Enumerable.Empty<TruckExportRecord>().ToList();
        }
    }
}