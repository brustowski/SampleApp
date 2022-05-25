using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Truck;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Class describing the configuration for the Truck Importer Rule grid
    /// </summary>
    public class TruckRuleImporterGridConfig : RuleGridConfiguration<TruckRuleImporterViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckRuleImporter;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.CWIOR).DisplayName("Importer").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.CWSupplier).DisplayName("Supplier").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<SupplierCodeDataProvider>();
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.SupplierMID).DisplayName("Supplier MID").MinWidth(150).EditableText();
            AddColumn(x => x.ManufacturerMID).DisplayName("Manufacturer MID").MinWidth(150).EditableText();
            AddColumn(x => x.ReconIssue).DisplayName("Recon Issue").MinWidth(150)
                .EditableLookup().DataSourceFrom<ValueReconeDataProvider>();
            AddColumn(x => x.NAFTARecon).DisplayName("NAFTA Recon").MinWidth(150)
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.TransactionsRelated).DisplayName("Transactions Related").MinWidth(150)
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.EntryPort).DisplayName("Entry Port").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<PortDataProvider>();
            AddColumn(x => x.ArrivalPort).DisplayName("Arrival Port").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<PortDataProvider>();
            AddColumn(x => x.DestinationState).DisplayName("Destination State").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<StatesDataProvider>();
            AddColumn(x => x.CE).DisplayName("Country of Export").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<CountryCodeDataProvider>();
            AddColumn(x => x.CO).DisplayName("Country of Origin").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<CountryCodeDataProvider>();
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<TariffDataProvider>();
            AddColumn(x => x.SPI).DisplayName("SPI").MinWidth(150).EditableText();
            AddColumn(x => x.CustomQuantity).DisplayName("Custom Quantity").EditableFloatNumber();
            AddColumn(x => x.CustomUQ).DisplayName("Custom UQ").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.InvoiceQTY).DisplayName("Invoice QTY").EditableFloatNumber();
            AddColumn(x => x.InvoiceUQ).DisplayName("Invoice UQ").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.LinePrice).DisplayName("Line Price").EditableFloatNumber();
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").EditableFloatNumber();
            AddColumn(x => x.GrossWeightUQ).DisplayName("Gross Weight UQ").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(150).EditableText();
            AddColumn(x => x.CustomAttrib1).DisplayName("Custom Attrib 1").MinWidth(150).EditableText();
            AddColumn(x => x.CustomAttrib2).DisplayName("Custom Attrib 2").MinWidth(150).EditableText();
            AddColumn(x => x.ProductID).DisplayName("Product ID").MinWidth(150).EditableText();
            AddColumn(x => x.Charges).DisplayName("Charges").EditableFloatNumber();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.CWIOR).Title("Importer");
            TextFilterFor(x => x.CWSupplier).Title("Supplier");
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee").Advanced();
            TextFilterFor(x => x.SupplierMID).Title("Supplier MID").Advanced();
            TextFilterFor(x => x.ManufacturerMID).Title("Manufacturer MID").Advanced();
            TextFilterFor(x => x.ReconIssue).Title("Recon Issue").Advanced();
            TextFilterFor(x => x.NAFTARecon).Title("NAFTA Recon").Advanced();
            TextFilterFor(x => x.TransactionsRelated).Title("Transactions Related").Advanced();
            TextFilterFor(x => x.EntryPort).Title("Entry Port").Advanced();
            TextFilterFor(x => x.ArrivalPort).Title("Arrival Port").Advanced();
            TextFilterFor(x => x.DestinationState).Title("Destination State").Advanced();
            TextFilterFor(x => x.CE).Title("Country of Export").Advanced();
            TextFilterFor(x => x.CO).Title("Country of Origin").Advanced();
            TextFilterFor(x => x.Tariff).Title("Tariff").Advanced();
            TextFilterFor(x => x.SPI).Title("SPI").Advanced();
            FloatNumberFilterFor(x => x.CustomQuantity).Title("Custom Quantity").Advanced();
            TextFilterFor(x => x.CustomUQ).Title("Custom UQ").Advanced();
            FloatNumberFilterFor(x => x.InvoiceQTY).Title("Invoice QTY").Advanced();
            TextFilterFor(x => x.InvoiceUQ).Title("Invoice UQ").Advanced();
            FloatNumberFilterFor(x => x.LinePrice).Title("Line Price").Advanced();
            FloatNumberFilterFor(x => x.GrossWeight).Title("Gross Weight").Advanced();
            TextFilterFor(x => x.GrossWeightUQ).Title("Gross Weight UQ").Advanced();
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            TextFilterFor(x => x.CustomAttrib1).Title("Custom Attrib 1").Advanced();
            TextFilterFor(x => x.CustomAttrib2).Title("Custom Attrib 2").Advanced();
            TextFilterFor(x => x.ProductID).Title("Product ID").Advanced();
            FloatNumberFilterFor(x => x.Charges).Title("Charges").Advanced();
        }
    }
}