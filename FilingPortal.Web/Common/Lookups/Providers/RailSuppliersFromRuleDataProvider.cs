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
    /// Provider for Rail Importer-Supplier rule Suppliers
    /// </summary>
    public class RailSuppliersFromRuleDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Importer-supplier rules repository
        /// </summary>
        private readonly IRailRuleImporterSupplierRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailSuppliersFromRuleDataProvider"/> class
        /// </summary>
        /// <param name="repository">Importer-supplier rules repository</param>
        public RailSuppliersFromRuleDataProvider(IRailRuleImporterSupplierRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.RailRuleSupplierNames;

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

            string searchData = searchInfo.Search.ToLower().Trim();

            IList<string> data = _repository.GetSuppliers(searchData); // Already sorted

            var equal = new List<string>();
            var beginsWith = new List<string>();
            var contains = new List<string>();

            foreach (string record in data)
            {
                if (record.ToLower() == searchData)
                    equal.Add(record);
                else // Not equal
                {
                    if (record.ToLower().StartsWith(searchData))
                        beginsWith.Add(record);
                    else // only contains
                        if (contains.Count <= searchInfo.Limit)
                        contains.Add(record);
                }
            }

            IEnumerable<string> result = equal.Union(beginsWith).Union(contains).Take(searchInfo.Limit);

            return result.Select(x => x.Map<string, LookupItem>());
        }
    }
}