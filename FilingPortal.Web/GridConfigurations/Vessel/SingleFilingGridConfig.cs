using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Grids;

namespace FilingPortal.Web.GridConfigurations.Vessel
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class SingleFilingGridConfig : AgileGridConfiguration<VesselImportDefValueReadModel>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public SingleFilingGridConfig(IAgileConfiguration<VesselImportDefValueReadModel> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.VesselSingleFilingGrid;
    }
}
