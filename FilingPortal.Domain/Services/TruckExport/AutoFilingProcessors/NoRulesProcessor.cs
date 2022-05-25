using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal class NoRulesProcessor : BaseProcessor
    {
        private readonly ITruckExportFilingService _procedureService;

        public NoRulesProcessor(ITruckExportRepository repository, ITruckExportFilingService procedureService): base(repository)
        {
            _procedureService = procedureService;
        }

        protected override IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records)
        {
            return records.Where(x => !x.ValidationPassed).ToList();
        }

        protected override IList<TruckExportRecord> Run(IList<TruckExportRecord> records, AppUsersModel user)
        {
            _procedureService.Update(records.Select(x => x.Id), user);

            foreach (TruckExportRecord inboundRecord in records)
            {
                // For not found configurations
                //  send email
                SendError(inboundRecord, "Rule not found or other error occured");
            }

            return new List<TruckExportRecord>();
        }
    }
}