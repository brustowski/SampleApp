using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps.VesselExport
{
    /// <summary>
    /// Class describing  report model mapping for the Vessel Export Records grid
    /// </summary>
    internal class VesselExportRecordsModelMap : ReportModelMap<VesselExportReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRecordsModelMap"/> class.
        /// </summary>
        public VesselExportRecordsModelMap()
        {
            Map(x => x.Usppi).ColumnTitle("USPPI");
            Map(x => x.Importer).ColumnTitle("Importer");
            Map(x => x.Vessel);
            Map(x => x.ExportDate).ColumnTitle("Export Date");
            Map(x => x.LoadPort).ColumnTitle("Load Port");
            Map(x => x.DischargePort).ColumnTitle("Discharge Port");
            Map(x => x.CountryOfDestination).ColumnTitle("Country of Destination");
            Map(x => x.TariffType).ColumnTitle("Tariff Type");
            Map(x => x.Tariff).ColumnTitle("Tariff");
            Map(x => x.GoodsDescription).ColumnTitle("Goods Description");
            Map(x => x.OriginIndicator).ColumnTitle("Origin Indicator");
            Map(x => x.Quantity);
            Map(x => x.Weight);
            Map(x => x.Value);
            Map(x => x.TransportRef).ColumnTitle("Transport Ref");
            Map(x => x.Container);
            Map(x => x.InBond).ColumnTitle("In-Bond");
            Map(x => x.SoldEnRoute).ColumnTitle("Sold en route");
            Map(x => x.ExportAdjustmentValue).ColumnTitle("Export Adjustment Value");
            Map(x => x.OriginalItn).ColumnTitle("Original ITN");
            Map(x => x.RoutedTransaction).ColumnTitle("Routed Transaction");
            Map(x => x.Description);
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Map(x => x.FilingNumber).ColumnTitle("Job #");
            Map(x => x.MappingStatus).ColumnTitle("Mapping Status").UseFormatter<EnumFormatter<MappingStatus>>();
            Map(x => x.FilingStatus).ColumnTitle("Filing Status").UseFormatter<EnumFormatter<FilingStatus>>();
            Ignore(x => x.IsDeleted);
            Ignore(x => x.FilingHeaderId);
            Ignore(x => x.Id);
        }
    }
}