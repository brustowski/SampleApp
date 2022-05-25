using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Truck;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Class describing the configuration for the Truck Port Rule grid
    /// </summary>
    public class TruckRulePortGridConfig : RuleGridConfigurationWithUniqueFields<TruckRulePortViewModel, TruckRulePort>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public TruckRulePortGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckRulePort;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.EntryPort).DisplayName("Entry Port").MinWidth(150).DefaultSorted().EditableLookup().DataSourceFrom<PortDataProvider>().Searchable();
            AddColumn(x => x.ArrivalPort).DisplayName("Arrival Port").MinWidth(150).EditableLookup().DataSourceFrom<PortDataProvider>().Searchable();
            AddColumn(x => x.FIRMsCode).DisplayName("FIRMs Code").MinWidth(150).EditableLookup().DataSourceFrom<FIRMsDataProvider>().Searchable();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.EntryPort).Title("Entry Port");
            TextFilterFor(x => x.ArrivalPort).Title("Arrival Port");
            TextFilterFor(x => x.FIRMsCode).Title("FIRMs Code");
        }
    }
}