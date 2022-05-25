using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Truck Export Records grid
    /// </summary>
    internal class TruckExportRecordsModelMap : ReportModelMap<TruckExportReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRecordsModelMap"/> class.
        /// </summary>
        public TruckExportRecordsModelMap()
        {
            Map(x => x.Exporter);
            Map(x => x.Importer);
            Map(x => x.TariffType).ColumnTitle("Tariff Type");
            Map(x => x.Tariff);
            Map(x => x.RoutedTran).ColumnTitle("Routed Tran");
            Map(x => x.SoldEnRoute).ColumnTitle("Sold en route");
            Map(x => x.MasterBill).ColumnTitle("Master Bill");
            Map(x => x.Origin).ColumnTitle("Origin");
            Map(x => x.Export).ColumnTitle("Export");
            Map(x => x.ExportDate).ColumnTitle("Export date").UseFormatter<DateTimeFormatter>(); ;
            Map(x => x.ECCN).ColumnTitle("ECCN");
            Map(x => x.GoodsDescription).ColumnTitle("Goods Description");
            Map(x => x.CustomsQty).ColumnTitle("Customs Qty");
            Map(x => x.Price).ColumnTitle("Price");
            Map(x => x.GrossWeight).ColumnTitle("Gross Weight");
            Map(x => x.GrossWeightUOM).ColumnTitle("Gross Wt Unit");
            Map(x => x.Hazardous).ColumnTitle("Hazardous");
            Map(x => x.OriginIndicator).ColumnTitle("Origin Indicator");
            Map(x => x.GoodsOrigin).ColumnTitle("Goods Origin");
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Map(x => x.FilingNumber).ColumnTitle("Job #");
            Map(x => x.JobStatus).ColumnTitle("Job Status").UseFormatter<EnumFormatter<JobStatus>>();
            Ignore(x => x.IsDeleted);
            Ignore(x => x.FilingHeaderId);
            Ignore(x => x.Id);
        }
    }
}