using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Defines the configuration for single-filing grid
    /// </summary>
    public class TruckSingleFilingGridConfig : AgileGridConfiguration<TruckDefValueReadModel>
    {
        /// <summary>
        /// Creates configuration
        /// </summary>
        /// <param name="repository">The repository configuration based on"/></param>
        public TruckSingleFilingGridConfig(IAgileConfiguration<TruckDefValueReadModel> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the GridName
        /// </summary>
        public override string GridName => GridNames.TruckSingleFilingGrid;
    }
}
