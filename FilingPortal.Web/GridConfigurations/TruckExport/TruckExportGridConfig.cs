using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.TruckExport;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.TruckExport
{
    /// <summary>
    /// Class describing the configuration for the Truck Export records grid
    /// </summary>
    public class TruckExportGridConfig : GridConfiguration<TruckExportViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckExportRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "truck_export_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Exporter).DisplayName("Exporter").MinWidth(120).DefaultSorted();
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(120);
            AddColumn(x => x.TariffType).DisplayName("Tariff Type").MinWidth(90);
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(100);
            AddColumn(x => x.RoutedTran).DisplayName("Routed Tran").MinWidth(110);
            AddColumn(x => x.SoldEnRoute).DisplayName("Sold en route").MinWidth(100);
            AddColumn(x => x.MasterBill).DisplayName("Master Bill").MinWidth(120);
            AddColumn(x => x.Origin).DisplayName("Origin").MinWidth(75);
            AddColumn(x => x.Export).DisplayName("Export").MinWidth(75);
            AddColumn(x => x.ExportDate).DisplayName("Export date").MinWidth(100);
            AddColumn(x => x.ECCN).DisplayName("ECCN").MinWidth(70);
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(200);
            AddColumn(x => x.CustomsQty).DisplayName("Customs Qty").MinWidth(120);
            AddColumn(x => x.Price).DisplayName("Price").MinWidth(120);
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").MinWidth(120);
            AddColumn(x => x.GrossWeightUOM).DisplayName("Gross Wt Unit").MinWidth(110);
            AddColumn(x => x.Hazardous).DisplayName("Hazardous").MinWidth(110);
            AddColumn(x => x.OriginIndicator).DisplayName("Origin Indicator").MinWidth(110);
            AddColumn(x => x.GoodsOrigin).DisplayName("Goods Origin").MinWidth(110);
            AddColumn(x => x.CreatedDate).DisplayName("Created Date").FixWidth(120);
            AddColumn(x => x.ModifiedDate).DisplayName("Modified Date").FixWidth(120);
            AddColumn(x => x.EntryCreatedDate).DisplayName("Entry Created Date").FixWidth(120);
            AddColumn(x => x.EntryModifiedDate).DisplayName("Entry Modified Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Exporter).Title("Exporter").DataSourceFrom<ClientCodeDataProvider>().SetOperand(FilterOperands.Equal);
            SelectFilterFor(x => x.Importer).Title("Importer").DataSourceFrom<ClientCodeDataProvider>().SetOperand(FilterOperands.Equal);
            TextFilterFor(x => x.MasterBill).Title("Master Bill");
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Record Created Date");
            DateRangeFilterFor(x => x.ModifiedDate).Title("Record Modified Date");
            SelectFilterFor(x => x.ModifiedUser).Title("Record Modified User").DataSourceFrom<UpdatedByUserTruckExportFilterDataProvide>().NotSearch();
            DateRangeFilterFor(x => x.EntryCreatedDate).Title("Entry Created Date");
            DateRangeFilterFor(x => x.EntryModifiedDate).Title("Entry Modified Date");
            SelectFilterFor(x => x.IsAuto).Title("Filing Method").DataSourceFrom<FileUploadMethodFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.ValidationPassed).Title("Validation Passed").DataSourceFrom<ValidationPassedFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.JobStatus).Title("Job Status").DataSourceFrom<JobStatusFilterDataProvider>().NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ExportEntryStatusFilterDataProvider>().NotSearch();

            TextFilterFor(x => x.TariffType).Title("Tariff Type").SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.Tariff).Title("Tariff").Advanced();
            TextFilterFor(x => x.RoutedTran).Title("Routed Tran").Advanced();
            TextFilterFor(x => x.SoldEnRoute).Title("Sold en route").Advanced();
            TextFilterFor(x => x.Origin).Title("Origin").Advanced();
            TextFilterFor(x => x.Export).Title("Export").Advanced();
            DateRangeFilterFor(x => x.ExportDate).Title("Export date").Advanced();
            TextFilterFor(x => x.ECCN).Title("ECCN").Advanced();
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            FloatNumberFilterFor(x => x.CustomsQty).Title("Customs Qty").Advanced();
            FloatNumberFilterFor(x => x.Price).Title("Price").Advanced();
            FloatNumberFilterFor(x => x.GrossWeight).Title("Gross Wght").Advanced();
            TextFilterFor(x => x.GrossWeightUOM).Title("Gross Wght UOM").Advanced();
            TextFilterFor(x => x.Hazardous).Title("Hazardous").Advanced();
        }
    }
}
