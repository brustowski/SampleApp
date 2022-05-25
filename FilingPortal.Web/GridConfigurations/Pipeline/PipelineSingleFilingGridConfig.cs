using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class PipelineSingleFilingGridConfig : AgileGridConfiguration<PipelineDefValueReadModel>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public PipelineSingleFilingGridConfig(IAgileConfiguration<PipelineDefValueReadModel> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.PipelineSingleFilingGrid;

        /// <summary>
        /// Executes requests to prepare column configuration
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Importer").DisplayName("Importer").MinWidth(200).NotSortable();
            AddColumn("Batch").DisplayName("Batch").NotSortable();
            AddColumn("TicketNumber").DisplayName("Ticket #").NotSortable();
            AddColumn("Facility").DisplayName("Facility").NotSortable();
            AddColumn("Quantity").DisplayName("Quantity").NotSortable();
            AddColumn("API").DisplayName("API").NotSortable();
            base.ConfigureColumns();
        }
    }
}
