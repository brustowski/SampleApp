using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Zones.Entry.Domain.Config;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.Zones.Entry.Web.GridConfigs
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
            AddColumn("Importer").DisplayName("Importer").DefaultSorted();
            AddColumn("EntryPort").DisplayName("Entry Port");
            AddColumn("ArrivalDate").DisplayName("Arrival Date");
            AddColumn("OwnerRef").DisplayName("Owner Ref");
            AddColumn("FirmsCode").DisplayName("FIRMs Code");

            base.ConfigureColumns();
        }
    }
}
