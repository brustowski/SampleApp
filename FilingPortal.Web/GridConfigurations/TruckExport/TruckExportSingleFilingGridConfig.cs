using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.TruckExport
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class TruckExportSingleFilingGridConfig : AgileGridConfiguration<TruckExportDefValue>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public TruckExportSingleFilingGridConfig(IAgileConfiguration<TruckExportDefValue> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.TruckExportSingleFilingGrid;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn("Exporter").DisplayName("Exporter").MinWidth(200).DefaultSorted();
            AddColumn("Importer").DisplayName("Consignee").MinWidth(200);
            AddColumn("TariffType").DisplayName("Tariff Type").MinWidth(200);
            AddColumn("Tariff").DisplayName("Tariff").MinWidth(100);
            AddColumn("MasterBill").DisplayName("Master Bill").MinWidth(100);
            AddColumn("Origin").DisplayName("Origin").MinWidth(70);
            AddColumn("Export").DisplayName("Export").MinWidth(80);

            base.ConfigureColumns();
        }
    }
}
