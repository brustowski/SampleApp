using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Pipeline;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Price Rule grid
    /// </summary>
    public class PipelineRulePriceGridConfig : RuleGridConfigurationWithUniqueFields<PipelineRulePriceViewModel, PipelineRulePrice>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public PipelineRulePriceGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineRulePrice;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "pipeline_rule_price.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Importer).EditableLookup().DataSourceFrom<ImporterDataProvider>()
                .KeyField(x => x.ImporterId).DefaultSorted();
            AddColumn(x => x.CrudeType).DisplayName("Crude Type").EditableLookup().DataSourceFrom<PipelineBatchCodesDataProvider>()
                .KeyField(x => x.CrudeTypeId);
            AddColumn(x => x.Facility).EditableLookup().DataSourceFrom<PipelineFacilitiesDataProvider>()
                .KeyField(x => x.FacilityId);
            AddColumn(x => x.Pricing).EditableFloatNumber();
            AddColumn(x => x.Freight).EditableFloatNumber();

        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer);
            TextFilterFor(x => x.CrudeType).Title("Crude Type");
            TextFilterFor(x => x.Facility).Title("Facility");
        }
    }
}