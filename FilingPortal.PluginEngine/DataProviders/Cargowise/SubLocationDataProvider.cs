using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Sub-location data 
    /// </summary>
    public class SubLocationDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Handbooks repository
        /// </summary>
        private readonly IHandbookRepository _repository;

        /// <summary>
        /// Initialize Vessel Discharge Terminal handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public SubLocationDataProvider(IHandbookRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.CargowiseSubLocation;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey)
            {
                var options = _repository.GetOptions("cw_sub_locations", searchInfo.Search);
                var record = options.SingleOrDefault(x => x.Value == searchInfo.Search);
                var result = new List<LookupItem> { record };
                return result;
            }

            return _repository.GetOptions("cw_sub_locations", searchInfo.Search, searchInfo.Limit);
        }
    }
}