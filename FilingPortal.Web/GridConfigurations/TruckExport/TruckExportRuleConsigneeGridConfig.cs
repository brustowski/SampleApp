using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.TruckExport;

namespace FilingPortal.Web.GridConfigurations.TruckExport
{
    /// <summary>
    /// Class describing the configuration for the Truck Export Consignee Rule grid
    /// </summary>
    public class TruckExportRuleConsigneeGridConfig : RuleGridConfigurationWithUniqueFields<TruckExportRuleConsigneeViewModel, TruckExportRuleConsignee>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public TruckExportRuleConsigneeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckExportRuleConsignee;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee").MinWidth(200).DefaultSorted()
                .EditableLookup().DataSourceFrom<ImporterCodeDataProvider>().KeyField(x => x.ConsigneeCode).Searchable();
            AddColumn(x => x.Country).DisplayName("Country of Destination").MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<CountryCodeDataProvider>();
            AddColumn(x => x.Destination).DisplayName("Destination").MinWidth(150)
                .EditableLookup().DataSourceFrom<UnlocoDictionaryDataProvider>()
                .DependsOn<TruckExportRuleConsigneeViewModel>(x => x.Country);
            AddColumn(x => x.UltimateConsigneeType).DisplayName("Ultimate Consignee Type")
                .EditableLookup().DataSourceFrom<ConsigneeTypeLookupDataProvider>().MinWidth(200);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee");
            TextFilterFor(x => x.Country).Title("Country of Destination");
            TextFilterFor(x => x.Destination).Title("Destination");
            TextFilterFor(x => x.UltimateConsigneeType).Title("Ultimate Consignee Type");
        }
    }
}