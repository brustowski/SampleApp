using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Defines the repository of the Vessel Export
    /// </summary>
    public class VesselExportRepository : SearchRepository<VesselExportRecord>, IVesselExportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<VesselExportRecord> GetByFilingId(int filingHeaderId) =>
            Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
    }
}
