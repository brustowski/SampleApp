using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Zones.Entry.Domain.Reporting.RuleImporter
{
    /// <summary>
    /// Class describing report model mapping for inbound record
    /// </summary>
    internal class ModelMap : ReportModelMap<RuleImporterReportModel>, IReportModelMap
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