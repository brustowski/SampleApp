using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="Tables"/>
    /// </summary>
    public class TablesRepository : BaseTablesRepository<Tables>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public TablesRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        { }
    }
}
