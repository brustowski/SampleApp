using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
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
