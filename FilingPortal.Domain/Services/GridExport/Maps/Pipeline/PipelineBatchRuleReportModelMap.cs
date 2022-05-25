using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Domain.Services.GridExport.Maps.Pipeline
{
    /// <summary>
    /// Class describing report model mapping for the Pipeline Batch Code rule grid
    /// </summary>
    internal class PipelineBatchRuleReportModelMap : ReportModelMap<PipelineRuleBatchCode>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineBatchRuleReportModelMap"/> class.
        /// </summary>
        public PipelineBatchRuleReportModelMap()
        {
            Ignore(x => x.PipelinePriceRules);
            Ignore(x => x.Id);
        }
    }
}