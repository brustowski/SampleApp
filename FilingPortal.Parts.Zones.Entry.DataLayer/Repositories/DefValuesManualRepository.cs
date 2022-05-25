using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="DefValueManual"/>
    /// </summary>
    public class DefValuesManualRepository : BaseDefValuesManualRepository<DefValueManual>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public DefValuesManualRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork) { }
    }
}
