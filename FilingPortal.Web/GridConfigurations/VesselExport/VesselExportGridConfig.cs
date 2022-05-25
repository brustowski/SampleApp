using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.VesselExport;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.VesselExport
{

    /// <summary>
    /// Class describing the configuration for the Vessel Export records grid
    /// </summary>
    public class VesselExportGridConfig : GridConfiguration<VesselExportViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselExportRecords;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Usppi).DisplayName("USPPI").MinWidth(120).DefaultSorted();
            AddColumn(x => x.Address);
            AddColumn(x => x.Contact);
            AddColumn(x => x.Phone);
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(120);
            AddColumn(x => x.Vessel);
            AddColumn(x => x.ExportDate).DisplayName("Export Date");
            AddColumn(x => x.LoadPort).DisplayName("Load Port");
            AddColumn(x => x.DischargePort).DisplayName("Discharge Port");
            AddColumn(x => x.CountryOfDestination).DisplayName("Country of Destination");
            AddColumn(x => x.TariffType).DisplayName("Tariff Type").MinWidth(90);
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(100);
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(200);
            AddColumn(x => x.OriginIndicator).DisplayName("Origin Indicator").MinWidth(75);
            AddColumn(x => x.Quantity);
            AddColumn(x => x.Weight);
            AddColumn(x => x.Value);
            AddColumn(x => x.TransportRef).DisplayName("Transport Ref");
            AddColumn(x => x.Container);
            AddColumn(x => x.InBond).DisplayName("In-Bond");
            AddColumn(x => x.SoldEnRoute).DisplayName("Sold en route").MinWidth(100);
            AddColumn(x => x.ExportAdjustmentValue).DisplayName("Export Adjustment Value");
            AddColumn(x => x.OriginalItn).DisplayName("Original ITN");
            AddColumn(x => x.RoutedTransaction).DisplayName("Routed Transaction");
            AddColumn(x => x.Description);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Usppi).Title("USPPI").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Address).SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.Contact).SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.Phone).SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.Importer).Title("Importer").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ExportEntryStatusFilterDataProvider>().NotSearch();
            TextFilterFor(x => x.Vessel).SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            DateRangeFilterFor(x => x.ExportDate).Title("Export Date");
            TextFilterFor(x => x.LoadPort).Title("Load Port").Advanced();
            TextFilterFor(x => x.DischargePort).Title("Discharge Port").Advanced();
            TextFilterFor(x => x.CountryOfDestination).Title("Country of Destination").Advanced();
            TextFilterFor(x => x.TariffType).Title("Tariff Type").Advanced();
            TextFilterFor(x => x.Tariff).Title("Tariff").Advanced();
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            TextFilterFor(x => x.OriginalItn).Title("Origin Indicator").Advanced();
            NumberFilterFor(x => x.Quantity).Advanced();
            NumberFilterFor(x => x.Weight).Advanced();
            NumberFilterFor(x => x.Value).Advanced();
            TextFilterFor(x => x.TransportRef).Title("Transport Ref").Advanced();
            TextFilterFor(x => x.Container).Advanced();
            TextFilterFor(x => x.InBond).Title("In-Bond").Advanced();
            TextFilterFor(x => x.SoldEnRoute).Title("Sold en route").Advanced();
            TextFilterFor(x => x.ExportAdjustmentValue).Title("Export Adjustment Value").Advanced();
            TextFilterFor(x => x.OriginalItn).Title("Original ITN").Advanced();
            TextFilterFor(x => x.RoutedTransaction).Title("Routed Transaction").Advanced();
            TextFilterFor(x => x.Description).Advanced();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
