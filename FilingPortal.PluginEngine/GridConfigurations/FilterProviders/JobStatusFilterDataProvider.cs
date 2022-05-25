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
    /// Provider for Job Status data for filter
    /// </summary>
    public class JobStatusFilterDataProvider : IFilterDataProvider
    {
        /// <summary>
        /// Defines the data repository
        /// </summary>
        private readonly ISearchRepository<HeaderJobStatus> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusFilterDataProvider"/> class.
        /// </summary>
        /// <param name="repository">The repository</param>
        public JobStatusFilterDataProvider(ISearchRepository<HeaderJobStatus> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of mapping status items by specified search information
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