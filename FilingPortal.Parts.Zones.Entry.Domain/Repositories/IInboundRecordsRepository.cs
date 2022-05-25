using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.Domain.Repositories
{
    /// <summary>
    /// Interface for repository of <see cref="InboundRecord"/>
    /// </summary>
    public interface IInboundRecordsRepository : IInboundRecordsRepository<InboundRecord>
    {
        /// <summary>
        /// Checks if a record is present in the database
        /// </summary>
        /// <param name="record">The record to check</param>
        bool IsDuplicated(InboundRecord record);

        /// <summary>
        /// Gets matched entity from repository
        /// </summary>
        /// <param name="item">Imported entry</param>
        InboundRecord GetMatchedEntity(CUSTOMS_ENTRY_FILEENTRY item);

        /// <summary>
        /// Removes inbound lines from record
        /// </summary>
        /// <param name="item">Record</param>
        void ClearLines(InboundRecord item);
        /// <summary>
        /// Removes inbound notes from record
        /// </summary>
        /// <param name="item">Record</param>
        void ClearNotes(InboundRecord item);

        /// <summary>
        /// Removes parsed data from record
        /// </summary>
        /// <param name="item">Record</param>
        void ClearParsedData(InboundRecord item);
    }
}
