using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for First Ports of Arrival data 
    /// </summary>
    public class FirstPortArrivalDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initialize First Port of arrival handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public FirstPortArrivalDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.CargowisePortsOfArrival;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "cw_ports_of_arrival";
    }
}