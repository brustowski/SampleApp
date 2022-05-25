using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors;

namespace FilingPortal.Domain.Common.Refile.TruckExport
{
    /// <summary>
    /// Truck export refile assistant
    /// </summary>
    public class TruckExportRefileAssistant : BaseRefileAssistant, IRefileAssistant<TruckExportRecord>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TruckExportRefileAssistant"/>
        /// </summary>
        /// <param name="emailNotificationService"></param>
        public TruckExportRefileAssistant(
            IEmailNotificationService emailNotificationService) : base(emailNotificationService)
        { }

        /// <summary>
        /// Assistant will add new information to report
        /// </summary>
        /// <param name="record">Update record</param>
        /// <param name="error">Error</param>
        public void Write(TruckExportRecord record, string error)
        {
            Entries.Add($"{record.Exporter}, {record.MasterBill}: {error}");
        }

        /// <summary>
        /// Assistant should watch over events in updateRecordsProcessor
        /// </summary>
        /// <param name="processor"></param>
        public void WatchOver(IUpdateRecordsProcessor processor)
        {
            processor.Notify += Write;
        }
    }
}