using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TruckTableNamesDataProvider : BaseTableNamesDataProvider<TruckTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.TruckTableNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public TruckTableNamesDataProvider(ITablesRepository<TruckTable> repository) : base(repository)
        {
        }
    }
}