using System.Collections.Generic;
using FilingPortal.Domain.Entities.TruckExport;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.TruckExport
{
    /// <summary>
    /// Describes the repository of the Truck Export Read Model
    /// </summary>
    public interface ITruckExportReadModelRepository : ISearchRepository<TruckExportReadModel>
    {
        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId);

        /// <summary>
        /// Gets Truck Export Records by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<TruckExportReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Gets the collection of the users specified by search request
        /// </summary>
        /// <param name="request">The search request</param>
        IEnumerable<string> GetUsers(SearchRequest request);
    }
}