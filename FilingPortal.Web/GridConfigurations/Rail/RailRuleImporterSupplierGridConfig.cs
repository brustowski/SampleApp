using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Importer Supplier Rule grid
    /// </summary>
    public class RailRuleImporterSupplierGridConfig : RuleGridConfigurationWithUniqueFields<RailRuleImporterSupplierViewModel, RailRuleImporterSupplier>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RailRuleImporterSupplierGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RailRuleImporterSupplier;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ImporterName).DisplayName("Importer Name").MinWidth(200).DefaultSorted().EditableText();
            AddColumn(x => x.SupplierName).DisplayName("Shipper Name").MinWidth(200).EditableText();
            AddColumn(x => x.ProductDescription).DisplayName("Product Description").MinWidth(200).Searchable()
                .EditableLookup().DataSourceFrom<RailProductDescriptionsDataProvider>();
            AddColumn(x => x.Port).MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<PortDataProvider>();
            AddColumn(x => x.Destination).MinWidth(130).Searchable()
                .EditableLookup().DataSourceFrom<StatesDataProvider>();
            AddColumn(x => x.MainSupplier).DisplayName("Main Supplier").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<SupplierCodeDataProvider>();
            AddColumn(x => x.MainSupplierAddress).DisplayName("Main Supplier Address").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ClientAddressCodeDataProvider>()
                .DependsOn<RailRuleImporterSupplierViewModel>(x => x.MainSupplier);
            AddColumn(x => x.Importer).DisplayName("Imp. Code").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.DestinationState).DisplayName("Destination State").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<StatesDataProvider>();
            AddColumn(x => x.Consignee).DisplayName("Consignee").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.Manufacturer).DisplayName("Manufacturer").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<SupplierCodeDataProvider>();
            AddColumn(x => x.ManufacturerAddress).DisplayName("Manufacturer Address").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ClientAddressCodeDataProvider>()
                .DependsOn<RailRuleImporterSupplierViewModel>(x => x.Manufacturer);
            AddColumn(x => x.Seller).DisplayName("Seller").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<SupplierCodeDataProvider>();
            AddColumn(x => x.SoldToParty).DisplayName("Sold to party").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.ShipToParty).DisplayName("Ship to party").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>();
            AddColumn(x => x.CountryOfOrigin).DisplayName("Country of Origin").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<CountryCodeDataProvider>();
            AddColumn(x => x.Relationship).DisplayName("Relationship").MinWidth(110)
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.DFT).DisplayName("DFT").MinWidth(110).EditableText();
            AddColumn(x => x.ValueRecon).DisplayName("Value Recon").MinWidth(150)
                .EditableLookup().DataSourceFrom<ValueReconeDataProvider>();
            AddColumn(x => x.FTARecon).DisplayName("FTA Recon").MinWidth(110)
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.PaymentType).DisplayName("Payment Type").MinWidth(200)
                .EditableLookup().DataSourceFrom<PaymentTypeDataProvider>();
            AddColumn(x => x.BrokerToPay).DisplayName("Broker to pay").MinWidth(110)
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.Value).DisplayName("Value").EditableFloatNumber();
            AddColumn(x => x.FreightComposite).DisplayName("Freight/UOM").EditableComposite()
                .AddSubColumn(ColumnBuilder<RailRuleFreightComposite>.CreateFor(x=>x.Freight)
                    .DisplayName("Freight").EditableFloatNumber().Build())
                .AddSubColumn(ColumnBuilder<RailRuleFreightComposite>.CreateFor(x=>x.FreightTypeDescription)
                    .DisplayName("Freight Type").EditableLookup().DataSourceFrom<FreightTypeDataProvider>().KeyField(x=>x.FreightType).Build());
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ImporterName).Title("Importer Name");
            TextFilterFor(x => x.SupplierName).Title("Supplier Name");
            TextFilterFor(x => x.ProductDescription).Title("Product Description");
            TextFilterFor(x => x.Port).Title("Port");
            TextFilterFor(x => x.Destination).Title("Destination");
            TextFilterFor(x => x.MainSupplier).Title("Main Supplier").Advanced();
            TextFilterFor(x => x.MainSupplierAddress).Title("Main Supplier Address").Advanced();
            TextFilterFor(x => x.Importer).Title("Imp. Code").Advanced();
            TextFilterFor(x => x.Consignee).Title("Consignee").Advanced();
            TextFilterFor(x => x.Manufacturer).Title("Manufacturer");
            TextFilterFor(x => x.ManufacturerAddress).Title("Manufacturer Address");
            TextFilterFor(x => x.Seller).Title("Seller");
            TextFilterFor(x => x.SoldToParty).Title("Sold to party").Advanced();
            TextFilterFor(x => x.ShipToParty).Title("Ship to party").Advanced();
            TextFilterFor(x => x.CountryOfOrigin).Title("Country of Origin").Advanced();
            TextFilterFor(x => x.Relationship).Title("Relationship").Advanced();
            TextFilterFor(x => x.DFT).Title("DFT").Advanced();
            TextFilterFor(x => x.ValueRecon).Title("Value Recon").Advanced();
            TextFilterFor(x => x.FTARecon).Title("FTA Recon").Advanced();
            NumberFilterFor(x => x.PaymentType).Title("Payment Type").Advanced();
            TextFilterFor(x => x.BrokerToPay).Title("Broker to pay").Advanced();
            FloatNumberFilterFor(x => x.Value).Title("Value").Advanced();
        }
    }
}