using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Truck
{
    /// <summary>
    /// Interface for repository of <see cref="TruckFilingHeader"/>
    /// </summary>
    public interface ITruckFilingHeadersRepository : IFilingHeaderRepository<TruckFilingHeader>, IFilingSectionRepository
    {
    }
}