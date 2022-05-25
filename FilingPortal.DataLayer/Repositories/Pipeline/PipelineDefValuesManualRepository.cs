using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Class for repository of <see cref="PipelineDefValueManual"/>
    /// </summary>
    public class PipelineDefValuesManualRepository : BaseDefValuesManualRepository<PipelineDefValueManual>, IPipelineDefValuesManualRepository
    {
        /// <summary>
        /// Gets the name of the recalculate stored procedure
        /// </summary>
        protected override string RecalculateProcedureName => "sp_imp_pipeline_recalculate";

        /// <summary>
        /// Gets the name of the update entry stored procedure
        /// </summary>
        protected override string UpdateEntryProcedureName => "sp_imp_pipeline_update_entry";

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesManualRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineDefValuesManualRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
    }
}
