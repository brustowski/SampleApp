using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.PluginEngine.Services.Filing.Auto;
using FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors;

namespace FilingPortal.Parts.Zones.Entry.Domain.Services.Refile
{
    /// <summary>
    /// Truck export refile assistant
    /// </summary>
    public class RefileAssistant : BaseRefileAssistant, IRefileAssistant<InboundRecord>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RefileAssistant"/>
        /// </summary>
        /// <param name="emailNotificationService"></param>
        public RefileAssistant(
            IEmailNotificationService emailNotificationService) : base(emailNotificationService)
        { }

        /// <summary>
        /// Assistant will add new information to report
        /// </summary>
        /// <param name="record">Record to report</param>
        /// <param name="error">Error message</param>
        public void WriteError(InboundRecord record, string error)
        {
            Entries.Add($"{record.EntryNo}: {error}");
        }

        /// <summary>
        /// Assistant should watch over events in updateRecordsProcessor
        /// </summary>
        /// <param name="processor"></param>
        public void WatchOver(IUpdateRecordsProcessor<InboundRecord> processor)
        {
            processor.Notify += WriteError;
        }
    }
}