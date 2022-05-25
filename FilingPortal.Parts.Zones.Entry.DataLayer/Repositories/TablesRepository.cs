using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
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
