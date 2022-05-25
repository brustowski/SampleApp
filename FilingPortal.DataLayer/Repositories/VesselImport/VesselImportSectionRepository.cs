using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Represents repository of the Vessel Import Section entity
    /// </summary>
    public class VesselImportSectionRepository : SearchRepository<VesselImportSection>, IVesselImportSectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}