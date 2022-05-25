using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.PluginEngine.GridConfigurations.FilterProviders
{
    /// <summary>
    /// Provider for Filing Status data for filter
    /// </summary>
    public class FilingStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Defines the data repository
        /// </summary>
        private readonly ISearchRepository<HeaderFilingStatus> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingStatusFilterDataProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository</param>
        public FilingStatusFilterDataProvider(ISearchRepository<HeaderFilingStatus> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of filing status items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            var result = new List<LookupItem>(10)
            {
                new LookupItem { Value = null, DisplayValue = "All"  }
            };

            result.AddRange(_repository.GetAll<LookupItem>());

            return result;
        }
    }
}