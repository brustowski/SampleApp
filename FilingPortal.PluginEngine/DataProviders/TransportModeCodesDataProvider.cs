using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for transport mode codes data provider
    /// </summary>
    public class TransportModeCodesDataProvider : IFilterDataProvider, ILookupDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly ITransportModeRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportModeCodesDataProvider"/> class
        /// </summary>
        /// <param name="repository">Transport mode repository</param>
        public TransportModeCodesDataProvider(ITransportModeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var results = new List<LookupItem>();

            IEnumerable<string> codes = _repository.GetCodes();

            if (searchInfo.SearchByKey)
            {
                string result = codes.FirstOrDefault(x => x == searchInfo.Search);
                if (result != null)
                {
                    results.Add(new LookupItem { DisplayValue = result, Value = result });
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchInfo.Search))
                    codes = codes.Where(x => x.ToLower().Contains(searchInfo.Search.ToLower()));
                else
                    results.Add(new LookupItem {DisplayValue = "All", Value = null});
                results = results.Union(codes.Select(x => new LookupItem { DisplayValue = x, Value = x })).ToList();
            }

            return results;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.TransportModeCode;
    }
}