using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Pipeline;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Consignee-Importer Rule grid
    /// </summary>
    public class PipelineRuleConsigneeImporterGridConfig : RuleGridConfigurationWithUniqueFields<PipelineRuleConsigneeImporterViewModel, PipelineRuleConsigneeImporter>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public PipelineRuleConsigneeImporterGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineRuleConsigneeImporter;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ImporterFromTicket).DisplayName("Importer from Ticket").MinWidth(150).DefaultSorted().EditableText();
            AddColumn(x => x.ImporterCode).DisplayName("Importer").MinWidth(150).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ImporterFromTicket).Title("Importer from Ticket");
            TextFilterFor(x => x.ImporterCode).Title("Importer");
        }
    }
}