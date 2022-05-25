using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Truck
{
    /// <summary>
    /// Interface for repository of <see cref="TruckInbound"/>
    /// </summary>
    public interface ITruckInboundRepository : IInboundRecordsRepository<TruckInbound>
    {
        /// <summary>
        /// Checks whether specified record is not duplicating any other records
        /// </summary>
        /// <param name="record">The record to be checked</param>
        bool IsDuplicate(TruckInbound record);
    }
}