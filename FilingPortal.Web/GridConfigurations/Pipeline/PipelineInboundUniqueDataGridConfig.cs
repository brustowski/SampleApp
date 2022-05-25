using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Models.Pipeline;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Provides configuration for the Pipeline Inbound Records Unique Data grid
    /// </summary>
    public class PipelineInboundUniqueDataGridConfig : GridConfiguration<PipelineInboundUniqueDataViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineInboundUniqueDataGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(200).NotSortable();
            AddColumn(x => x.Batch).DisplayName("Batch").NotSortable();
            AddColumn(x => x.TicketNumber).DisplayName("Ticket #").NotSortable();
            AddColumn(x => x.Facility).DisplayName("Facility").MaxWidth(95).NotSortable();
            AddColumn(x => x.Quantity).DisplayName("Quantity").NotSortable();
            AddColumn(x => x.API).DisplayName("API").NotSortable();
            AddColumn(x => x.ExportDate).DisplayName("Export Date").FixWidth(120).NotSortable();
            AddColumn(x => x.ImportDate).DisplayName("Import Date").FixWidth(120).NotSortable();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.FilingHeaderId).SetOperand(FilterOperands.Equal);
        }
    }
}