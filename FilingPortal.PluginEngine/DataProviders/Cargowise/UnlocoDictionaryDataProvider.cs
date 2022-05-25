using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Unloco codes data 
    /// </summary>
    public class UnlocoDictionaryDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Cargowise UNLOCO dictionary repository
        /// </summary>
        private readonly IUnlocoDictionaryRepository _repository;

        /// <summary>
        /// Initialize UNLOCO dictionary repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public UnlocoDictionaryDataProvider(IUnlocoDictionaryRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.CargowiseUnlocoDictionary;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<UnlocoDictionaryEntry> entries;
            if (searchInfo.SearchByKey)
            {
                UnlocoDictionaryEntry record = _repository.GetByCode(searchInfo.Search);
                entries = record != null
                    ? new List<UnlocoDictionaryEntry> { record }
                    : Enumerable.Empty<UnlocoDictionaryEntry>();
            }
            else
            {
                entries = string.IsNullOrWhiteSpace(searchInfo.DependOn)
                    ? _repository.Search(searchInfo.Search, searchInfo.Limit)
                    : _repository.Search(searchInfo.Search, searchInfo.Limit, searchInfo.DependValue);
            }

            return entries.Select(record => new LookupItem { DisplayValue = $"{record.Unloco} - {record.PortName}", Value = record.Unloco });
        }
    }
}