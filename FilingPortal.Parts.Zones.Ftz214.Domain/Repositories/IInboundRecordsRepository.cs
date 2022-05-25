using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Repositories
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
        InboundRecord GetMatchedEntity(FTZ_214FTZ_ADMISSION item);

        /// <summary>
        /// Removes inbound lines from record
        /// </summary>
        /// <param name="item">Record</param>
        void ClearParsedLines(InboundRecord item);

        /// <summary>
        /// Removes parsed data from record
        /// </summary>
        /// <param name="item">Record</param>
        void ClearParsedData(InboundRecord item);
    }
}
