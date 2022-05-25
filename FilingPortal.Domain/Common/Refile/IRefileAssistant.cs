using System.Threading.Tasks;
using FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors;
using Framework.Domain;

namespace FilingPortal.Domain.Common.Refile
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
    public interface IRefileAssistant<in T> : IRefileAssistant where T : Entity
    {
        /// <summary>
        /// Assistant will add error message on update record to the report
        /// </summary>
        /// <param name="updateRecord"></param>
        /// <param name="errorMessage"></param>
        void Write(T updateRecord, string errorMessage);

        /// <summary>
        /// Assistant should watch over events in updateRecordsProcessor
        /// </summary>
        /// <param name="processor">Update records processor</param>
        void WatchOver(IUpdateRecordsProcessor processor);
    }
}