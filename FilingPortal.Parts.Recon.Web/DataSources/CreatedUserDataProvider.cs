using FilingPortal.Parts.Recon.Domain.Repositories;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Recon.Web.DataSources
{
    /// <summary>
    /// Provider for entity Created User data
    /// </summary>
    public class CreatedUserDataProvider<TEntity> : IFilterDataProvider where TEntity : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the data source repository
        /// </summary>
        private readonly IAuditableEntityRepository<TEntity> _repository;
        /// <summary>
        /// Initialize a new instance of the <see cref="CreatedUserDataProvider{TEntity}"/> class
        /// </summary>
        /// <param name="repository">The data source repository</param>
        public CreatedUserDataProvider(IAuditableEntityRepository<TEntity> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the collection of items specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
           string[] users = _repository.GetCreatedUsers(searchInfo.Search).ToArray();
            var result = new List<LookupItem>(users.Count() + 1) { new LookupItem { Value = null, DisplayValue = "All" } };
            result.AddRange(users.Select(x => new LookupItem { DisplayValue = x, Value = x }));
            return result;
        }
    }
}