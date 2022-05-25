using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Port Rule grid
    /// </summary>
    public class RailRulePortGridConfig : RuleGridConfigurationWithUniqueFields<RailRulePortViewModel, RailRulePort>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RailRulePortGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RailRulePort;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Port).DisplayName("Port").MinWidth(150).DefaultSorted().EditableLookup().DataSourceFrom<PortDataProvider>().Searchable();
            AddColumn(x => x.Origin).DisplayName("Origin").MinWidth(150).EditableLookup().DataSourceFrom<OriginDataProvider>().Searchable();
            AddColumn(x => x.Destination).DisplayName("Destination").MinWidth(150).EditableLookup().DataSourceFrom<DestinationDataProvider>().Searchable();
            AddColumn(x => x.FIRMsCode).DisplayName("FIRMs Code").MinWidth(150).EditableLookup().DataSourceFrom<FIRMsDataProvider>().Searchable();
            AddColumn(x => x.Export).DisplayName("Export").MinWidth(150).EditableLookup().DataSourceFrom<CountryCodeDataProvider>().Searchable();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Port).Title("Port");
            TextFilterFor(x => x.Origin).Title("Origin");
            TextFilterFor(x => x.Destination).Title("Destination");
            TextFilterFor(x => x.FIRMsCode).Title("FIRMs Code");
            TextFilterFor(x => x.Export).Title("Export");
        }
    }
}