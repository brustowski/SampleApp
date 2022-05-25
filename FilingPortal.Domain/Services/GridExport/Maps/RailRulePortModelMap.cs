using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Rail Port rule Records grid
    /// </summary>
    internal class RailRulePortModelMap : ReportModelMap<RailRulePort>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRulePortModelMap"/> class.
        /// </summary>
        public RailRulePortModelMap()
        {
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Ignore(x => x.Id);
        }
    }
}