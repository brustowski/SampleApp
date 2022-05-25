using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for HTS numbers data 
    /// </summary>
    public class HtsNumbersDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// HTS Tariff repository
        /// </summary>
        private readonly IHtsNumbersRepository _htsRepository;

        /// <summary>
        /// Initialize HTS numbers repository
        /// </summary>
        /// <param name="htsRepository">HTS Tariff table Repository</param>
        public HtsNumbersDataProvider(IHtsNumbersRepository htsRepository)
        {
            _htsRepository = htsRepository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.HtsNumbers;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (string.IsNullOrWhiteSpace(searchInfo.DependValue))
            {
                IEnumerable<string> data = _htsRepository.GetHtsNumbers(searchInfo.Search, searchInfo.Limit);
                return data.Select(x => new LookupItem { DisplayValue = x, Value = x });
            }

            return new List<LookupItem>();
        }
    }
}