using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Provider for status data for filter
    /// </summary>
    public class StatusFilterDataProvider<TStatus, TId> : IFilterDataProvider where TStatus : BaseStatus<TId>
    {
        /// <summary>
        /// Statuses repository
        /// </summary>
        private readonly IRepositoryWithTypeId<TStatus, TId> _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusFilterDataProvider{TStatus,TId}"/> class.
        /// </summary>
        /// <param name="repository">Statuses repository</param>
        public StatusFilterDataProvider(IRepositoryWithTypeId<TStatus, TId> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of mapping status items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            return _repository.GetAll().Select(x => new LookupItem { DisplayValue = x.Name, Value = x.Id });
        }
    }
}