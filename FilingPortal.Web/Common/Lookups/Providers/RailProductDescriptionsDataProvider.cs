using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for rail product description
    /// </summary>
    public class RailProductDescriptionsDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Rule descriptions repository
        /// </summary>
        private readonly IRailRuleProductDescriptionsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailProductDescriptionsDataProvider"/> class
        /// </summary>
        /// <param name="repository">Product descriptions repository</param>
        public RailProductDescriptionsDataProvider(IRailRuleProductDescriptionsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.RailProductDescriptions;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                return null;
            }

            var searchData = searchInfo.Search.ToLower().Trim();

            IList<string> data = _repository.GetDescriptions(searchData); // Already sorted

            List<string> equal = new List<string>();
            List<string> beginsWith = new List<string>();
            List<string> contains = new List<string>();

            foreach (string description in data)
            {
                if (description.ToLower() == searchData)
                    equal.Add(description);
                else // Not equal
                {
                    if (description.ToLower().StartsWith(searchData))
                        beginsWith.Add(description);
                    else // only contains
                        if (contains.Count <= searchInfo.Limit)
                        contains.Add(description);
                }
            }

            var result = equal.Union(beginsWith).Union(contains).Take(searchInfo.Limit);

            return result.Select(x => x.Map<string, LookupItem>());
        }
    }
}