using FilingPortal.Domain.Services;
using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleVendor;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Vendor Rule grid
    /// </summary>
    public class RuleVendorGridConfig : RuleGridConfigurationWithUniqueFields<RuleVendorViewModel, RuleVendor>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleVendorGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RuleVendor;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Vendor).IsKeyField()
                .EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.VendorId);
            AddColumn(x => x.Importer).DefaultSorted().EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ImporterId);
            AddColumn(x => x.Exporter).EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ExporterId);
            AddColumn(x => x.ExportState).DisplayName("Export State").EditableLookup().DataSourceFrom<StatesDataProvider>();
            AddColumn(x => x.DirectShipPlace).DisplayName("Direct Ship Place").EditableLookup().DataSourceFrom<UnlocoDictionaryDataProvider>();
            AddColumn(x => x.NoPackages).DisplayName("No. Packages").EditableNumber();
            AddColumn(x => x.CountryOfOrigin).DisplayName("Country Of Origin").EditableLookup().DataSourceFrom<CountryCodeDataProvider>();
            AddColumn(x => x.OrgState).DisplayName("ORG State").EditableLookup().DataSourceFrom<StatesDataProvider>();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Vendor);
            TextFilterFor(x => x.Importer);
            TextFilterFor(x => x.Exporter);
            TextFilterFor(x => x.ExportState).Title("Export States");
            TextFilterFor(x => x.DirectShipPlace).Title("Direct Ship Place");
            TextFilterFor(x => x.NoPackages).Title("No. Packages");
            TextFilterFor(x => x.CountryOfOrigin).Title("Country Of Origin");
            TextFilterFor(x => x.OrgState).Title("ORG State");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
        }
    }
}