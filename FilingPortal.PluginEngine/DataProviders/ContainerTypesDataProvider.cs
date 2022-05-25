using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for Container Types data 
    /// </summary>
    public class ContainerTypesDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initialize container types handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public ContainerTypesDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.ContainerTypes;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "container_types";
    }
}