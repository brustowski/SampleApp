using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.PluginEngine.Services.Filing.Auto.AutoFilingProcessors;
using Framework.Domain;

namespace FilingPortal.PluginEngine.Services.Filing.Auto
{
    /// <summary>
    /// Describes generic Auto-Refile report writer for specific entities
    /// </summary>
    public interface IRefileAssistant
    {
        /// <summary>
        /// Assistant will create new error report
        /// </summary>
        void CreateErrorReport();

        /// <summary>
        /// Assistant will try to send report to specific addresses
        /// </summary>
        /// <param name="addresses"></param>
        Task SendReport(string addresses);

        /// <summary>
        /// Assistant will print report in readable format
        /// </summary>
        string PrintReport();
    }

    /// <summary>
    /// Describes Auto-Refile report writer for specific entities
    /// </summary>
    /// <typeparam name="T">Entity to write to report</typeparam>
    public interface IRefileAssistant<T> : IRefileAssistant where T : Entity, IAutoFilingEntity
    {
        /// <summary>
        /// Assistant will add error message on update record to the report
        /// </summary>
        /// <param name="updateRecord">Record to report</param>
        /// <param name="errorMessage">Error message</param>
        void WriteError(T updateRecord, string errorMessage);

        /// <summary>
        /// Assistant should watch over events in updateRecordsProcessor
        /// </summary>
        /// <param name="processor">Update records processor</param>
        void WatchOver(IUpdateRecordsProcessor<T> processor);
    }
}