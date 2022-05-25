using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Isf.Web.Common.Providers
{
    /// <summary>
    /// Provider for Entry Status data for filter
    /// </summary>
    internal class IsfEntryStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Defines the data repository
        /// </summary>
        private readonly IEntryStatusRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsfEntryStatusFilterDataProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository</param>
        public IsfEntryStatusFilterDataProvider(IEntryStatusRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of mapping status items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var result = new List<LookupItem>()
            {
                new LookupItem { Value = null, DisplayValue = "All"  }
            };

            result.AddRange(_repository.GetFilteredByStatusType("isf")
                .Select(x => new LookupItem { Value = x.Code, DisplayValue = $"{x.Code}-{x.Description}" }));
            return result;
        }
    }
}
