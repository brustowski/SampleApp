using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
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
            AddColumn("Vendor").DisplayName("Vendor").MinWidth(200).DefaultSorted();
            AddColumn("Port").MinWidth(100);
            AddColumn("ParsNumber").DisplayName("PARS#").MinWidth(100);
            AddColumn("OwnersReference").DisplayName("Owners Reference").MinWidth(100);

            base.ConfigureColumns();
        }
    }
}
