using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Defines Filing header repository
    /// </summary>
    /// <typeparam name="TFilingHeader"></typeparam>
    public interface IFilingHeaderRepository<TFilingHeader> : IRepository<TFilingHeader> where TFilingHeader: BaseFilingHeader
    {
        /// <summary>
        /// Finds the Filing Headers by inbound record identifiers
        /// </summary>
        /// <param name="inboundRecordIds">The inbound records identifiers</param>
        IEnumerable<TFilingHeader> FindByInboundRecordIds(IEnumerable<int> inboundRecordIds);

        /// <summary>
        /// Fills the data for header by specified filing header identifier using stored procedure
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        int FillDataForFilingHeader(int headerId, string userAccount = null);

        /// <summary>
        /// Fills the data for header by specified filing header identifier using stored procedure
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        int RefillDataForFilingHeader(int headerId, string userAccount = null);

        /// <summary>
        /// Calls File procedure for the records with specified filing header identifier
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        void FileRecordsWithHeader(int headerId);

        /// <summary>
        /// Calls Cancel filing process procedure for the filing header specified by identifier
        /// </summary>
        /// <param name="headerId">The filing header identifier</param>
        void CancelFilingProcess(int headerId);
    }
}
