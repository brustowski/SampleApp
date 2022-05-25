using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Final Destination data 
    /// </summary>
    public class FinalDestinationDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initialize Final Destination handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public FinalDestinationDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.CargowiseFinalDestinations;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "cw_final_destinations";
    }
}