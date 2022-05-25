using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.VesselExport
{
    /// <summary>
    /// Class for repository of <see cref="VesselExportDefValueManual"/>
    /// </summary>
    public class VesselExportDefValuesManualRepository : BaseDefValuesManualRepository<VesselExportDefValueManual>, IVesselExportDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_exp_vessel_update_entry";

        /// <summary>
        /// Gets the name of the recalculate stored procedure
        /// </summary>
        protected override string RecalculateProcedureName => "sp_exp_vessel_recalculate";

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public VesselExportDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
