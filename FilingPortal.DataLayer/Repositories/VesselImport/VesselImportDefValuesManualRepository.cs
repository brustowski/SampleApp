using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories.VesselImport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselImport
{
    /// <summary>
    /// Class for repository of <see cref="VesselImportDefValueManual"/>
    /// </summary>
    public class VesselImportDefValuesManualRepository : BaseDefValuesManualRepository<VesselImportDefValueManual>, IVesselImportDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_imp_vessel_update_entry";

        /// <summary>
        /// Gets the name of the recalculate stored procedure
        /// </summary>
        protected override string RecalculateProcedureName => "sp_imp_vessel_recalculate";

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselImportDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
