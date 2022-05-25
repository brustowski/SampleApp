using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents EPA TSCA data provider
    /// </summary>
    public class EpaTscaDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpaTscaDataProvider"/> class
        /// </summary>
        /// <param name="repository">Handbook table repository</param>
        public EpaTscaDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.EpaTsca;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "epa_tsca";
    }
}