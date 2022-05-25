using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Represents repository of the Vessel Export Section entity
    /// </summary>
    class VesselExportSectionRepository : SearchRepository<VesselExportSection>, IVesselExportSectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}