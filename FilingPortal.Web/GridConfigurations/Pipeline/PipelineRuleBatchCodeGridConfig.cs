using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Pipeline;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Batch Code Rule grid
    /// </summary>
    public class PipelineRuleBatchCodeGridConfig : RuleGridConfigurationWithUniqueFields<PipelineRuleBatchCodeViewModel, PipelineRuleBatchCode>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public PipelineRuleBatchCodeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineRuleBatchCode;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.BatchCode).DisplayName("Batch Code").MinWidth(150).DefaultSorted().EditableText();
            AddColumn(x => x.Product).DisplayName("Product").MinWidth(150).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.BatchCode).Title("Batch Code");
            TextFilterFor(x => x.Product).Title("Product");
        }
    }
}