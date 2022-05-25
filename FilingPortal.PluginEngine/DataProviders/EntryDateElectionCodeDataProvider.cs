using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents Entry Date Election Code data provider
    /// </summary>
    public class EntryDateElectionCodeDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryDateElectionCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">Handbook table repository</param>
        public EntryDateElectionCodeDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.EntryDateElectionCode;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "entry_date_election_code";
    }
}