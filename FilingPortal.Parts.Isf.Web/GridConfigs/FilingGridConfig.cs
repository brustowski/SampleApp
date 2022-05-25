using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Isf.Domain.Config;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Isf.Web.GridConfigs
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
            AddColumn("ImporterCode").DisplayName("Importer").MinWidth(120).DefaultSorted();
            AddColumn("BuyerCode").DisplayName("Buyer").MinWidth(120);
            AddColumn("ConsigneeCode").DisplayName("Consignee").MinWidth(120);
            AddColumn("MblScacCode").DisplayName("MBL SCAC Code");
            AddColumn("OwnerRef").DisplayName("Owner Ref").MinWidth(120);
            AddColumn("SellerCode").DisplayName("Seller").MinWidth(120);
            AddColumn("ShipToCode").DisplayName("Ship To").MinWidth(120);
            AddColumn("ContainerStuffingLocationCode").DisplayName("Container stuffing location").MinWidth(120);
            AddColumn("ConsolidatorCode").DisplayName("Consolidator/Forwarder:").MinWidth(120);

            base.ConfigureColumns();
        }
    }
}
