using FilingPortal.Domain.Entities.Truck;
using Framework.Domain.Repositories;
using System.Collections.Generic;

namespace FilingPortal.Domain.Repositories.Truck
{
    /// <summary>
    /// Interface for repository of <see cref="TruckInboundReadModel"/>
    /// </summary>
    public interface ITruckInboundReadModelRepository : ISearchRepository<TruckInboundReadModel>
    {
        /// <summary>
        /// Gets the total Truck BD rows by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <returns>TableInfo.</returns>
        TableInfo GetTotalTruckBdRows(int filingHeaderId);
        
        /// <summary>
        /// Gets Truck Inbound records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<TruckInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);
    }
}