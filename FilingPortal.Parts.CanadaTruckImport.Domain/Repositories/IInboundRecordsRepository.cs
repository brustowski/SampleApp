using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Repositories
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
    }
}
