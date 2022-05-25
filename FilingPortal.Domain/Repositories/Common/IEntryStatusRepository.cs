using FilingPortal.Domain.Entities.Handbooks;
using System.Collections.Generic;

namespace FilingPortal.Domain.Repositories.Common
{
    /// <summary>
    /// Describes Entry status data provider Repository
    /// </summary>
    public interface IEntryStatusRepository
    {
        /// <summary>
        /// Get all Entry Statuses filtered by specified status type
        /// </summary>
        IEnumerable<EntryStatus> GetFilteredByStatusType(string status);
    }
}
