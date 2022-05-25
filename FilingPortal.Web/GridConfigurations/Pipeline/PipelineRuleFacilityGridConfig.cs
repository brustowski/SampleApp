using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Pipeline;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Facility Rule grid
    /// </summary>
    public class PipelineRuleFacilityGridConfig : RuleGridConfigurationWithUniqueFields<PipelineRuleFacilityViewModel, PipelineRuleFacility>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public PipelineRuleFacilityGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineRuleFacility;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Facility).DisplayName("Facility").MinWidth(150).DefaultSorted().EditableText();
            AddColumn(x => x.Port).DisplayName("Entry/Discharge Port").MinWidth(110).DefaultSorted().EditableText();
            AddColumn(x => x.DestinationState).DisplayName("Dest. State").MinWidth(100).EditableLookup().DataSourceFrom<StatesDataProvider>().Searchable();
            AddColumn(x => x.Origin).DisplayName("Origin").MinWidth(110).EditableLookup().DataSourceFrom<OriginDataProvider>().Searchable();
            AddColumn(x => x.Destination).DisplayName("Destination").MinWidth(110).EditableLookup().DataSourceFrom<DestinationDataProvider>().Searchable();
            AddColumn(x => x.FIRMsCode).DisplayName("FIRMs Code").MinWidth(110).EditableLookup().DataSourceFrom<FIRMsDataProvider>().Searchable();
            AddColumn(x => x.Issuer).DisplayName("Issuer").MinWidth(120).EditableText();
            AddColumn(x => x.Pipeline).DisplayName("Pipeline").MinWidth(170).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Facility).Title("Facility");
            TextFilterFor(x => x.Port).Title("Entry/Discharge Port");
            TextFilterFor(x => x.DestinationState).Title("Dest. State").Advanced();
            TextFilterFor(x => x.Origin).Title("Origin").Advanced();
            TextFilterFor(x => x.FIRMsCode).Title("FIRMs Code").Advanced();
            TextFilterFor(x => x.Destination).Title("Destination").Advanced();
            TextFilterFor(x => x.Issuer).Title("Issuer").Advanced();
            TextFilterFor(x => x.Pipeline).Title("Pipeline").Advanced();
        }
    }
}