using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Truck;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Class describing the configuration for the Inbound Records Unique Data grid
    /// </summary>
    public class TruckInboundUniqueDataGridConfig : GridConfiguration<TruckInboundUniqueDataViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckInboundUniqueDataGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).MinWidth(200).NotSortable();
            AddColumn(x => x.Supplier).MinWidth(200).NotSortable();
            AddColumn(x => x.PAPs).DisplayName("PAPs").NotSortable();
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