using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal class NotMatchedProcessor : BaseProcessor
    {
        public NotMatchedProcessor(ITruckExportRepository inboundRepository) : base(inboundRepository)
        {
        }

        protected override IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records)
        {
            return records.Where(x => !x.IsUpdate).ToList();
        }

        protected override IList<TruckExportRecord> Run(IList<TruckExportRecord> keyValuePairs, AppUsersModel user)
        {
            return new List<TruckExportRecord>();
        }
    }
}
