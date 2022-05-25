using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Rail Importer rule Records grid
    /// </summary>
    internal class RailRuleImporterSupplierModelMap : ReportModelMap<RailRuleImporterSupplier>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleImporterSupplierModelMap"/> class.
        /// </summary>
        public RailRuleImporterSupplierModelMap()
        {
            Map(x => x.DFT).ColumnTitle("DFT");
            Map(x => x.FTARecon).ColumnTitle("FTA Recon");
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Ignore(x => x.Id);
        }
    }
}