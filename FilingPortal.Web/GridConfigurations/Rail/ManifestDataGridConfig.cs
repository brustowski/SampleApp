using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class ManifestDataGridConfig : GridConfiguration<RailDefValuesReadModel>
    {
        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.RailManifestDataGrid;

        /// <summary>
        /// Configures Rail unique data columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("BOLNumber").DisplayName("BoL #").NotSortable();
            AddColumn("ContainerNumber").DisplayName("Container #").NotSortable();
            AddColumn("GrossWeight").DisplayName("Gross Weight").MinWidth(110).NotSortable();
            AddColumn("GrossWeightUnit").DisplayName("Unit").MaxWidth(60).NotSortable();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
        }
    }
}
