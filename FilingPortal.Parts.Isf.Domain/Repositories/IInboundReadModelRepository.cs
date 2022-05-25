using System.Collections.Generic;
using FilingPortal.Parts.Isf.Domain.Entities;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Isf.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the inbound read models
    /// </summary>
    public interface IInboundReadModelRepository : ISearchRepository<InboundReadModel>
    {
        /// <summary>
        /// Gets the total rows by Filing Header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        TableInfo GetTotalRowsByFilingHeaderId(int filingHeaderId);

        /// <summary>
        /// Gets Vessel Export Records by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<InboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Gets created users list
        /// </summary>
        /// <param name="searchRequest">Search request</param>
        IList<string> GetUsers(SearchRequest searchRequest);
    }
}