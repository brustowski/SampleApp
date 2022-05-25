using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.VesselExport
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class VesselExportSingleFilingGridConfig : AgileGridConfiguration<VesselExportDefValue>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public VesselExportSingleFilingGridConfig(IAgileConfiguration<VesselExportDefValue> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.VesselExportSingleFilingGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Usppi").DisplayName("USPPI").MinWidth(200).DefaultSorted();
            AddColumn("Importer").DisplayName("Consignee").MinWidth(200);
            AddColumn("TariffType").DisplayName("Tariff Type").MinWidth(200);
            AddColumn("Tariff").DisplayName("Tariff").MinWidth(100);

            base.ConfigureColumns();
        }
    }
}
