using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Rail.Export.Web.GridConfigs
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class FilingGridConfig : AgileGridConfiguration<DefValue>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public FilingGridConfig(IAgileConfiguration<DefValue> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.FilingGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Exporter").DisplayName("Exporter").MinWidth(120).DefaultSorted();
            AddColumn("Importer").DisplayName("Consignee").MinWidth(120);
            AddColumn("TariffType").DisplayName("Tariff Type").MinWidth(120);
            AddColumn("Tariff").DisplayName("Tariff").MinWidth(100);
            AddColumn("MasterBill").DisplayName("Master Bill").MinWidth(100);

            base.ConfigureColumns();
        }
    }
}
