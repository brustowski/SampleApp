using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Inbond.Domain.Config;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Inbond.Web.GridConfigs
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
            AddColumn("FirmsCode").DisplayName("FIRMS Code");
            AddColumn("ImporterCode").DisplayName("Importer").MinWidth(120);
            AddColumn("PortOfArrival").DisplayName("Port of Arrival").MinWidth(120);
            AddColumn("ExportConveyance").DisplayName("Conveyance").MinWidth(120);
            AddColumn("ConsigneeCode").DisplayName("Consignee").MinWidth(120);
            AddColumn("Value").DisplayName("Value").MinWidth(120);
            AddColumn("ManifestQty").DisplayName("Manifest Quantity").MinWidth(120);
            AddColumn("ManifestQtyUnit").DisplayName("Manifest Quantity Unit").MinWidth(120);
            AddColumn("Weight").DisplayName("Weight").MinWidth(120);

            base.ConfigureColumns();
        }
    }
}
