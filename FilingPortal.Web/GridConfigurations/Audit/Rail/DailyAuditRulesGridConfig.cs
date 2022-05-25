using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Audit.Rail;
using FilingPortal.Web.Models.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Audit.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Daily Audit rules grid
    /// </summary>
    public class DailyAuditRulesGridConfig : GridConfigurationWithUniqueFields<DailyAuditRuleViewModel, AuditRailDailyRule>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public DailyAuditRulesGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.AuditRailDailyAuditRules;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ImporterCode).DisplayName("Importer").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>().Searchable().DefaultSorted();
            AddColumn(x => x.SupplierCode).DisplayName("Supplier Code").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>().Searchable();
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee Code").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>().Searchable();
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").EditableText();
            AddColumn(x => x.Tariff).EditableLookup().DataSourceFrom<TariffDataProvider>();
            AddColumn(x => x.PortCode).DisplayName("Port").EditableLookup().DataSourceFrom<PortDataProvider>().Searchable();
            AddColumn(x => x.DestinationState).DisplayName("Destination State").EditableLookup().DataSourceFrom<StatesDataProvider>().Searchable();
            AddColumn(x => x.CustomsAttrib4).DisplayName("Customs Attrib 4").EditableText();
            AddColumn(x => x.Carrier).DisplayName("Carrier").EditableLookup().DataSourceFrom<IssuerCodeDataProvider>().Searchable();
            AddColumn(x => x.ExportingCountry).DisplayName("Country of Export").EditableLookup()
                .DataSourceFrom<CountryCodeDataProvider>().Searchable();
            AddColumn(x => x.CountryOfOrigin).DisplayName("Country of Origin").EditableLookup()
                .DataSourceFrom<CountryCodeDataProvider>().Searchable();
            AddColumn(x => x.UltimateConsigneeName).DisplayName("Ultimate Consignee Name").EditableText();
            AddColumn(x => x.FirmsCode).DisplayName("FIRMs Code").EditableLookup().DataSourceFrom<FIRMsDataProvider>()
                .Searchable();
            AddColumn(x => x.ApiFrom).DisplayName("API from").EditableFloatNumber();
            AddColumn(x => x.ApiTo).DisplayName("API to").EditableFloatNumber();
            AddColumn(x => x.CustomsQty).DisplayName("Customs Qty per container").EditableFloatNumber();
            AddColumn(x => x.CustomsQtyUnit).DisplayName("Customs Qty Unit").EditableLookup()
                .DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.UnitPrice).DisplayName("Unit Price").EditableFloatNumber();
            AddColumn(x => x.ValueRecon).DisplayName("Value Recon").EditableText();
            AddColumn(x => x.NaftaRecon).DisplayName("NAFTA Recon").EditableLookup()
                .DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.ManufacturerMid).DisplayName("Manufacturer MID").EditableText();
            AddColumn(x => x.SupplierMid).DisplayName("Supplier MID").EditableText();
            AddColumn(x => x.InvoiceQtyUnit).DisplayName("Invoice Qty Unit").EditableLookup()
                .DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.GrossWeightUq).DisplayName("Gross Weight Unit").EditableLookup()
                .DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.CustomsAttrib1).DisplayName("Customs Attrib 1").EditableText();
            AddColumn(x => x.TransactionsRelated).DisplayName("Transaction Related").EditableLookup()
                .DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.FreightComposite).DisplayName("Freight/UOM").EditableComposite()
                .AddSubColumn(ColumnBuilder<RailRuleFreightComposite>.CreateFor(x=>x.Freight)
                    .DisplayName("Freight").EditableFloatNumber().Build())
                .AddSubColumn(ColumnBuilder<RailRuleFreightComposite>.CreateFor(x=>x.FreightTypeDescription)
                    .DisplayName("Freight Type").EditableLookup().DataSourceFrom<FreightTypeDataProvider>().KeyField(x=>x.FreightType).Build());
            AddColumn(x => x.CreatedDate).DisplayName("Last Modified Date");
            AddColumn(x => x.CreatedUser).DisplayName("Last Modified By");
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ImporterCode).Title("Importer").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.SupplierCode).Title("Supplier").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Tariff).SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.PortCode).Title("Port Code").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.DestinationState).Title("Destination State").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.CustomsAttrib4).Title("Customs Attrib 4").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.UltimateConsigneeName).Title("Ult. Consignee Name").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.SupplierMid).Title("Supplier MID").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Creation Date");
        }
    }
}