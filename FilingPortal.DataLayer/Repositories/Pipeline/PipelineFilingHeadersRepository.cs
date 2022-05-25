using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.DataLayer.Repositories.Pipeline
{
    /// <summary>
    /// Represents the repository of the <see cref="PipelineFilingHeader"/>
    /// </summary>
    public class PipelineFilingHeadersRepository : BaseFilingHeadersRepository<PipelineFilingHeader>, IPipelineFilingHeaderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingHeadersRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public PipelineFilingHeadersRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }
        /// <summary>
        /// The name of the create entry record stored procedure
        /// </summary>
        protected override string CreateEntryProcedureName => "sp_imp_pipeline_create_entry_records";
        /// <summary>
        /// The name of the delete entry record stored procedure
        /// </summary>
        protected override string DeleteEntryProcedureName => "sp_imp_pipeline_delete_entry_records";
        /// <summary>
        /// Finds the Pipeline Filing Headers by inbound record identifiers
        /// </summary>
        /// <param name="ids">The inbound records identifiers</param>
        /// <returns>The <see cref="IEnumerable{PipelineFilingHeader}"/></returns>
        public override IEnumerable<PipelineFilingHeader> FindByInboundRecordIds(IEnumerable<int> ids) =>
            Set.Where(x => x.PipelineInbounds.Select(r => r.Id).Intersect(ids).Any());
        /// <summary>
        /// Gets the section by its name
        /// </summary>
        /// <param name="sectionName">The section name</param>
        protected override BaseSection GetSection(string sectionName)
        {
            var context = (FilingPortalContext)UnitOfWork.Context;
            return context.PipelineSections.FirstOrDefault(x => x.Name == sectionName);
        }
    }
}
