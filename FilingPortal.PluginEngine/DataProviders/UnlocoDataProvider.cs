using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for UNLOCO data 
    /// </summary>
    public class UnlocoDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Vessel Loading Port handbook repository
        /// </summary>
        private readonly IDomesticPortsRepository _repository;

        /// <summary>
        /// Initialize Vessel Loading Port handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public UnlocoDataProvider(IDomesticPortsRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.UNLOCOs;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var list = new List<string>();

            if (searchInfo.SearchByKey)
            {
                string item = _repository.SearchUNLOCO(searchInfo.Search, 1).FirstOrDefault();
                if (item != null)
                {
                    list.Add(item);
                }
            }
            else
            {
                list.AddRange(_repository.SearchUNLOCO(searchInfo.Search, searchInfo.Limit));
            }

            return list.Select(x => new LookupItem { DisplayValue = x, Value = x });
        }
    }
}