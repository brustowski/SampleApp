using FilingPortal.Parts.Common.DataLayer.Services;
using Framework.DataLayer;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Services
{
    /// <summary>
    /// Implements methods to get database structure information for current context
    /// </summary>
    internal class DbStructureService : BaseDbStructureService
    {
        /// <summary>
        /// Creates a new instance of <see cref="DbStructureService"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of Work</param>
        public DbStructureService(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
