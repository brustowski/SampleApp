using System.Collections.Generic;
using FilingPortal.Parts.Inbond.Domain.Entities;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the Inbond Read Model
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
    }
}