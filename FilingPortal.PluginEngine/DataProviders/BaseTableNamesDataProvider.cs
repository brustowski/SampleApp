using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public abstract class BaseTableNamesDataProvider<TTables> : ILookupDataProvider
    where TTables : BaseTable
    {
        /// <summary>
        /// Tables repository
        /// </summary>
        private readonly ITablesRepository<TTables> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTableNamesDataProvider{TTables}"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        protected BaseTableNamesDataProvider(ITablesRepository<TTables> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IQueryable<string> data = _repository.GetTableNames();

            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                string searched = searchInfo.Search.ToLower();
                data = data.Where(x => x.ToLower().Contains(searched));
            }

            if (searchInfo.Limit > 0)
            {
                data = data.Take(searchInfo.Limit);
            }

            return data.Select(x => new LookupItem { DisplayValue = x, Value = x });
        }
    }
}