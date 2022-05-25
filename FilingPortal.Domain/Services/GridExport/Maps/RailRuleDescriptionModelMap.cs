using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Rail Description Rule Records grid
    /// </summary>
    internal class RailRuleDescriptionModelMap : ReportModelMap<RailRuleDescription>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleDescriptionModelMap"/> class.
        /// </summary>
        public RailRuleDescriptionModelMap()
        {
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Ignore(x => x.Id);
        }
    }
}