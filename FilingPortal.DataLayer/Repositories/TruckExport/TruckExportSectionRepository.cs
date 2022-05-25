using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.TruckExport
{
    /// <summary>
    /// Represents repository of the Truck Export Section entity
    /// </summary>
    class TruckExportSectionRepository : SearchRepository<TruckExportSection>, ITruckExportSectionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TruckExportSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}