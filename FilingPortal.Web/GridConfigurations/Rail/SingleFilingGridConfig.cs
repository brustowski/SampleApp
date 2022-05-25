using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class SingleFilingGridConfig : AgileGridConfiguration<RailDefValuesReadModel>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public SingleFilingGridConfig(IAgileConfiguration<RailDefValuesReadModel> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.RailSingleFilingGrid;

        /// <summary>
        /// Configures Rail unique data columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Importer").DisplayName("Importer").MinWidth(200).NotSortable();
            AddColumn("BOLNumber").DisplayName("BoL #").NotSortable();
            AddColumn("ContainerNumber").DisplayName("Container #").NotSortable();
            AddColumn("TrainNumber").DisplayName("Train #").NotSortable();
            AddColumn("PortCode").DisplayName("Port Code").MaxWidth(100).NotSortable();
            AddColumn("GrossWeight").DisplayName("Gross Weight").MinWidth(110).NotSortable();
            AddColumn("GrossWeightUnit").DisplayName("Unit").MaxWidth(60).NotSortable();
            base.ConfigureColumns();
        }
    }
}
