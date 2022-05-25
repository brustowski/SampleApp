using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the ACE report records
    /// </summary>
    public interface IAceReportRecordsRepository : ISearchRepository<AceReportRecord>
    {
        /// <summary>
        /// Gets inbound records by entry numbers list
        /// </summary>
        /// <param name="entryNumbers">Entry numbers list</param>
        Task<IList<AceReportRecord>> GetByEntryNumbers(IEnumerable<string> entryNumbers);
        /// <summary>
        /// Clears the ACE Report table
        /// </summary>
        void Clear();
        /// <summary>
        /// Checks if the repository contains any records
        /// </summary>
        bool IsEmpty();
        /// <summary>
        /// Gets missing in CW report records 
        /// </summary>
        Task<IEnumerable<AceReportRecord>> GetMissingRecords();
    }
}