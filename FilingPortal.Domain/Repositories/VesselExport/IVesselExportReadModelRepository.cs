using FilingPortal.Domain.Entities.VesselExport;
using Framework.Domain.Repositories;
using System.Collections.Generic;

namespace FilingPortal.Domain.Repositories.VesselExport
{
    /// <summary>
    /// Describes the repository of the Vessel Export Read Model
    /// </summary>
    public interface IVesselExportReadModelRepository : ISearchRepository<VesselExportReadModel>
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
        IEnumerable<VesselExportReadModel> GetByFilingHeaderIds(IEnumerable<int> filingHeaderIds);
    }
}