using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Models.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Class describing the configuration for the Inbound Records Unique Data grid
    /// </summary>
    public class InboundUniqueDataGridConfig : GridConfiguration<InboundUniqueDataItemViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.InboundRecordsUniqueData;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(200).NotSortable();
            AddColumn(x => x.BOLNumber).DisplayName("BoL #").NotSortable();
            AddColumn(x => x.ContainerNumber).DisplayName("Container #").NotSortable();
            AddColumn(x => x.TrainNumber).DisplayName("Train #").NotSortable();
            AddColumn(x => x.PortCode).DisplayName("Port Code").MaxWidth(100).NotSortable();
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").MinWidth(100).NotSortable();
            AddColumn(x => x.GrossWeightUnit).DisplayName("Unit").MaxWidth(60).NotSortable();
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