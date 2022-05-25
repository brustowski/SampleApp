using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal class MatchedProcessor : BaseProcessor
    {
        private readonly ITruckExportFilingService _procedureService;

        public MatchedProcessor(ITruckExportFilingService procedureService, ITruckExportRepository repository) : base(repository)
        {
            _procedureService = procedureService;
        }

        protected override IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records)
        {
            return records.Where(x => x.ValidationPassed).ToList();
        }

        protected override IList<TruckExportRecord> Run(IList<TruckExportRecord> records, AppUsersModel user)
        {
            OperationResultWithValue<int[]> updateResult =
                _procedureService.Update(records.Select(x => x.Id), user);

            _procedureService.AutoFile(updateResult.Value, user.Id);

            return new List<TruckExportRecord>();
        }
    }
}