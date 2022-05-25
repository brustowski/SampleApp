using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Rail.Export.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the containers grid
    /// </summary>
    public class ContainersGridConfig : GridConfiguration<InboundRecordContainerViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.ContainersGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ContainerNumber).DisplayName("Container Number").MinWidth(120).DefaultSorted();
            AddColumn(x => x.Type).MinWidth(120);
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").MinWidth(120);
            AddColumn(x => x.GrossWeightUq).DisplayName("Gross Weight UQ").MinWidth(120);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
        }
    }
}
