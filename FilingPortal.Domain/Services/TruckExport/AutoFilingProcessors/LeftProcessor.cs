using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    internal class LeftProcessor : BaseProcessor
    {
        public LeftProcessor(ITruckExportRepository inboundRepository) : base(inboundRepository)
        {
        }

        protected override IList<TruckExportRecord> FindRecordsToProcess(IList<TruckExportRecord> records)
        {
            return records;
        }

        protected override IList<TruckExportRecord> Run(IList<TruckExportRecord> keyValuePairs, AppUsersModel user)
        {
            foreach (TruckExportRecord truckExportRecord in keyValuePairs)
            {
                SendError(truckExportRecord, "Record was not processed for some reason");
            }

            return new List<TruckExportRecord>();
        }

        
    }
}