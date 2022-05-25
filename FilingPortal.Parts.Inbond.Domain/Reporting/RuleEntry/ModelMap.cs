using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.RuleEntry
{
    /// <summary>
    /// Class describing report model mapping
    /// </summary>
    internal class ModelMap : ReportModelMap<RuleEntryReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}