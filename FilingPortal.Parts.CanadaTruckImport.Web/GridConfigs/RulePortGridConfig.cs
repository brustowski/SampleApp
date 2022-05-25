using FilingPortal.Domain.Services;
using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Port Rule grid
    /// </summary>
    public class RulePortGridConfig : RuleGridConfigurationWithUniqueFields<RulePortViewModel, RulePort>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RulePortGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RulePortRecords;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.PortOfClearance).DisplayName("Port of Clearance").DefaultSorted().IsKeyField()
                .EditableLookup().DataSourceFrom<PortsOfClearanceDataProvider>();
            AddColumn(x => x.SubLocation).DisplayName("Sub-location")
                .EditableLookup().DataSourceFrom<SubLocationDataProvider>();
            AddColumn(x => x.FirstPortOfArrival).DisplayName("First Port of Arrival")
                .EditableLookup().DataSourceFrom<FirstPortArrivalDataProvider>();
            AddColumn(x => x.FinalDestination).DisplayName("Final Destination")
                .EditableLookup().DataSourceFrom<FinalDestinationDataProvider>();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.PortOfClearance).Title("Port of Clearance");
            TextFilterFor(x => x.SubLocation).Title("Sub-location");
            TextFilterFor(x => x.FirstPortOfArrival).Title("First Port of Arrival");
            TextFilterFor(x => x.FinalDestination).Title("Final Destination");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
        }
    }
}