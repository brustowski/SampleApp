using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Provides the repository of <see cref="VesselImportRecord"/>
    /// </summary>
    public class VesselImportRepository : SearchRepository<VesselImportRecord>, IVesselImportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Gets inbound records by filing header id
        /// </summary>
        /// <param name="filingHeaderId">Filing header id</param>
        public IEnumerable<VesselImportRecord> GetByFilingId(int filingHeaderId)
        {
            return Set.Where(x => x.FilingHeaders.Select(f => f.Id).Contains(filingHeaderId));
        }
    }
}
