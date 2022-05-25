using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Interface for repository of <see cref="RailInboundReadModel"/>
    /// </summary>
    public interface IRailInboundReadModelRepository : ISearchRepository<RailInboundReadModel>
    {
        /// <summary>
        /// Gets the total Rail BD rows by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        /// <returns>TableInfo.</returns>
        TableInfo GetTotalRailBdRows(int filingHeaderId);
        
        /// <summary>
        /// Gets Rail Inbound records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<RailInboundReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);

        /// <summary>
        /// Gets manifest of the selected Inbound record record
        /// </summary>
        /// <param name="manifestRecordId">Inbound record identifier</param>
        string GetRecordManifest(int manifestRecordId);

        /// <summary>
        /// Restore the Rail Inbound record with specified identifier
        /// </summary>
        /// <param name="id">The Rail Inbound record identifier</param>
        void RestoreById(int id);
    }
}