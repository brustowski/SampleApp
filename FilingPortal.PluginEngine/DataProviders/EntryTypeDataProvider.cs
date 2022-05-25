using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Represents the data provider for the <see cref="EntryType"/>
    /// </summary>
    public class EntryTypeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Repository
        /// </summary>
        private readonly IDataProviderRepository<EntryType, string> _repository;
        /// <summary>
        /// Initialize a new instance of the <see cref="EntryTypeDataProvider"/> class
        /// </summary>
        /// <param name="repository">The repository</param>
        public EntryTypeDataProvider(IDataProviderRepository<EntryType, string> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.EntryTypes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var results = new List<EntryType>();

            if (searchInfo.SearchByKey)
            {
                EntryType result = _repository.Get(searchInfo.Search);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            else
            {
                results.AddRange(_repository.Search(searchInfo.Search, searchInfo.Limit));
            }

            return results.Select(entryType => new LookupItem { DisplayValue = $"{entryType.Id} - {entryType.Description}", Value = entryType.Id });
        }
    }
}