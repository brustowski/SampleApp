using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provides list of available handbooks
    /// </summary>
    public class HandbooksDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// The user defined repository
        /// </summary>
        private readonly IHandbookRepository _repository;

        /// <summary>
        /// The data provider registry
        /// </summary>
        private readonly IHandbookDataProviderRegistry _dataProviderRegistry;

        /// <summary>
        /// Creates a new instance of <see cref="HandbooksDataProvider"/>
        /// </summary>
        /// <param name="repository">Repository of the user defined handbooks</param>
        /// <param name="dataProviderRegistry">The data provider registry</param>
        public HandbooksDataProvider(IHandbookRepository repository, IHandbookDataProviderRegistry dataProviderRegistry)
        {
            _repository = repository;
            _dataProviderRegistry = dataProviderRegistry;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.Handbooks;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<LookupItem> userDefinedProviders = _repository.GetHandbooks().Select(x => new LookupItem { DisplayValue = x, Value = x });

            IEnumerable<LookupItem> systemProviders = _dataProviderRegistry.GetAll()
                .Select(x => new LookupItem { DisplayValue = x.Name, Value = $"system.{x.Name}" });

            IEnumerable<LookupItem> consolidated = userDefinedProviders.Concat(systemProviders);

            if (searchInfo.SearchByKey)
            {
                return consolidated.Where(x => x.Value.Equals(searchInfo.Search));
            }

            return string.IsNullOrWhiteSpace(searchInfo.Search)
                ? consolidated
                : consolidated.Where(x => x.DisplayValue.IndexOf(searchInfo.Search, StringComparison.InvariantCultureIgnoreCase) != -1);
        }
    }
}