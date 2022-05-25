using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Class for repository of <see cref="RailDefValuesManual"/>
    /// </summary>
    public class RailDefValuesManualRepository : BaseDefValuesManualRepository<RailDefValuesManual>, IRailDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_imp_rail_update_entry";

        /// <summary>
        /// Gets the name of the recalculate stored procedure
        /// </summary>
        protected override string RecalculateProcedureName => "sp_imp_rail_recalculate";

        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}