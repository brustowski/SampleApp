using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Audit.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Audit.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Daily SPI Audit rules grid
    /// </summary>
    public class DailyAuditSpiRulesGridConfig : GridConfigurationWithUniqueFields<DailyAuditSpiRuleViewModel, AuditRailDailySpiRule>
    {
        /// <summary>
        /// Creates a new instance of <see cref="DailyAuditSpiRulesGridConfig"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public DailyAuditSpiRulesGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.AuditRailDailyAuditSpiRules;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ImporterCode).DisplayName("Importer").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>().Searchable().DefaultSorted();
            AddColumn(x => x.SupplierCode).DisplayName("Supplier").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>().Searchable();
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").EditableText();
            AddColumn(x => x.DestinationState).DisplayName("Destination State").EditableLookup()
                .DataSourceFrom<StatesDataProvider>().Searchable();
            AddColumn(x => x.CustomsAttrib4).DisplayName("Customs Attrib 4").EditableText();
            AddColumn(x => x.DateFrom).DisplayName("Date From").EditableDate();
            AddColumn(x => x.DateTo).DisplayName("Date To").EditableDate();
            AddColumn(x => x.Spi).DisplayName("SPI").EditableText();

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
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.DestinationState).Title("Destination State").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.CustomsAttrib4).Title("Customs Attrib 4").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.DateFrom).Title("Date From");
            DateRangeFilterFor(x => x.DateTo).Title("Date To");
            DateRangeFilterFor(x => x.CreatedDate).Title("Creation Date");
        }
    }
}