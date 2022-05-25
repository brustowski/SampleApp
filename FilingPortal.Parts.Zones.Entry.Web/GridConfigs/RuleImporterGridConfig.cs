using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Entry.Domain.Config;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Zones.Entry.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the importer rule grid
    /// </summary>
    public class RuleImporterGridConfig : RuleGridConfigurationWithUniqueFields<RuleImporterViewModel, RuleImporter>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleImporterGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.ImporterRuleGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Importer).IsKeyField()
                .EditableLookup().DataSourceFrom<AllClientsDataProvider>().KeyField(x => x.ImporterId);

            AddColumn(x => x.Rlf).DisplayName("RLF").EditableText();
           

            AddColumn(x => x.Consignee).DisplayName("Consignee").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.Manufacturer).DisplayName("Manufacturer").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.Supplier).DisplayName("Supplier").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.Seller).DisplayName("Seller").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.SoldToParty).DisplayName("Sold To Party").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.ShipToParty).DisplayName("Ship To Party").EditableLookup()
                .DataSourceFrom<ClientCodeDataProvider>();

            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").EditableText();
            AddColumn(x => x.ReconIssue).DisplayName("Recon. Issue").EditableLookup()
                .DataSourceFrom<ReconIssueDataProvider>();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Importer).DataSourceFrom<ClientCodeDataProvider>();
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
        }
    }
}
