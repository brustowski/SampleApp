using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    public interface IUpdateRecordsProcessor
    {
        event ErrorHandler Notify;
        void SetSuccessor(IUpdateRecordsProcessor processor);
        Task Process(IList<TruckExportRecord> records, AppUsersModel user);
    }
}