using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Rail.Export.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the import records grid
    /// </summary>
    public class InboundRecordsGridConfig : GridConfiguration<InboundRecordViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.InboundRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "us_rail_export_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Exporter).DisplayName("Exporter").MinWidth(120).DefaultSorted();
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(110);
            AddColumn(x => x.MasterBill).DisplayName("Master Bill").MinWidth(120);
            AddColumn(x => x.Containers).MinWidth(120).NotSortable();
            AddColumn(x => x.TariffType).DisplayName("Tariff Type").MinWidth(90);
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(100);
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(200);
            AddColumn(x => x.CustomsQty).DisplayName("Customs Qty").MinWidth(120);
            AddColumn(x => x.Price).DisplayName("Price").MinWidth(120);
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").MinWidth(120);
            AddColumn(x => x.GrossWeightUOM).DisplayName("Gross Wt Unit").MinWidth(110);
            AddColumn(x => x.LoadDate).DisplayName("Load Date").FixWidth(120);
            AddColumn(x => x.ExportDate).DisplayName("Export Date").FixWidth(120);
            AddColumn(x => x.TerminalAddress).DisplayName("Terminal Address").MinWidth(120);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Exporter).Title("Exporter").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Importer).Title("Importer").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.TariffType).Title("Tariff Type").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.MasterBill).Title("Master Bill");
            TextFilterFor(x => x.Containers).SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.Tariff).Title("Tariff").Advanced();
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            FloatNumberFilterFor(x => x.CustomsQty).Title("Customs Qty").Advanced();
            FloatNumberFilterFor(x => x.Price).Title("Price").Advanced();
            FloatNumberFilterFor(x => x.GrossWeight).Title("Gross Wght").Advanced();
            TextFilterFor(x => x.GrossWeightUOM).Title("Gross Wght UOM").Advanced();
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ExportEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
