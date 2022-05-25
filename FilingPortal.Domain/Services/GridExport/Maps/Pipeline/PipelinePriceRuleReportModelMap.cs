using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Pipeline;

namespace FilingPortal.Domain.Services.GridExport.Maps.Pipeline
{
    /// <summary>
    /// Class describing report model mapping for the Pipeline Price rule grid
    /// </summary>
    internal class PipelinePriceRuleReportModelMap : ReportModelMap<PipelineRulePriceReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelinePriceRuleReportModelMap"/> class.
        /// </summary>
        public PipelinePriceRuleReportModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}