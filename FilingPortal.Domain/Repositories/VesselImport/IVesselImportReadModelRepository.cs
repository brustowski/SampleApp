using System.Collections.Generic;
using FilingPortal.Domain.Entities.Vessel;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.VesselImport
{
    /// <summary>
    /// Interface for repository of <see cref="VesselImportReadModel"/>
    /// </summary>
    public interface IVesselImportReadModelRepository : ISearchRepository<VesselImportReadModel>
    {
        /// <summary>
        /// Gets the total Vessel BD rows by filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        TableInfo GetTotalVesselBdRows(int filingHeaderId);
        
        /// <summary>
        /// Gets Vessel Import records with specified by list of the Filing Header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">List of the Filing Header identifiers</param>
        IEnumerable<VesselImportReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);
    }
}