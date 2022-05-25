using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors
{
    /// <summary>
    /// Describes update records processor
    /// </summary>
    /// <typeparam name="TInboundType">Auto-file entity</typeparam>
    public interface IUpdateRecordsProcessor<TInboundType> where TInboundType : IAutoFilingEntity
    {
        /// <summary>
        /// Notifies about errors in processing
        /// </summary>
        event ErrorHandler<TInboundType> Notify;
        /// <summary>
        /// Sets up next processor for other records
        /// </summary>
        /// <param name="processor">Next processor</param>
        void SetSuccessor(IUpdateRecordsProcessor<TInboundType> processor);
        /// <summary>
        /// Processes records
        /// </summary>
        /// <param name="records">Records to process</param>
        /// <param name="user">User that executes processing</param>
        Task Process(IList<TInboundType> records, AppUsersModel user);
    }
}