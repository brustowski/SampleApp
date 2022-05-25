using FilingPortal.Domain.Entities.Pipeline;
using Framework.Domain.Repositories;
using System.Collections.Generic;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Describes repository of the <see cref="PipelineInboundReadModel"/> records
    /// </summary>
    public interface IPipelineInboundReadModelRepository : ISearchRepository<PipelineInboundReadModel>
    {
        /// <summary>
        /// Gets Truck Inbound records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<PipelineInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);
    }
}
