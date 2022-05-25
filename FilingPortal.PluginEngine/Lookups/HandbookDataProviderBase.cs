using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.Lookups
{
    /// <summary>
    /// Implements standard handbook data provider
    /// </summary>
    public abstract class HandbookDataProviderBase : ILookupDataProvider
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Gets handbook name
        /// </summary>
        public abstract string HandbookName { get; }

        /// <summary>
        /// Handbooks repository
        /// </summary>
        protected readonly IHandbookRepository HandbookRepository;

        /// <summary>
        /// Initialize handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        protected HandbookDataProviderBase(IHandbookRepository repository) => HandbookRepository = repository;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public virtual IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey)
            {
                var options = HandbookRepository.GetOptions(HandbookName, searchInfo.Search);
                var record = options.SingleOrDefault(x => x.Value == searchInfo.Search);
                List<LookupItem> result = new List<LookupItem> { record };
                return result;
            }

            return HandbookRepository.GetOptions(HandbookName, searchInfo.Search, searchInfo.Limit);
        }
    }
}
