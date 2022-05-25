using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for Export Entry Status data for filter
    /// </summary>
    public class ExportEntryStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Defines the data repository
        /// </summary>
        private readonly IEntryStatusRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportEntryStatusFilterDataProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository</param>
        public ExportEntryStatusFilterDataProvider(IEntryStatusRepository repository)
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

            result.AddRange(_repository.GetFilteredByStatusType("export")
                .Select(x => new LookupItem { Value = x.Code, DisplayValue = $"{x.Code}-{x.Description}" }));
            return result;
        }
    }
}