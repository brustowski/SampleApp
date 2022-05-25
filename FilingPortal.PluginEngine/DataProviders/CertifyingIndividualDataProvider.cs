using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents Certifying Individual data provider
    /// </summary>
    public class CertifyingIndividualDataProvider : HandbookDataProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CertifyingIndividualDataProvider"/> class
        /// </summary>
        /// <param name="repository">Handbook table repository</param>
        public CertifyingIndividualDataProvider(IHandbookRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.CertifyingIndividual;

        /// <summary>
        /// Gets handbook name
        /// </summary>
        public override string HandbookName => "cerifying_individual";
    }
}