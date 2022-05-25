using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public abstract class BaseTableColumnsDataProvider<TTables> : ILookupDataProvider
    where TTables : BaseTable
    {
        /// <summary>
        /// Tables repository
        /// </summary>
        private readonly ITablesRepository<TTables> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTableColumnsDataProvider{TTables}"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        protected BaseTableColumnsDataProvider(ITablesRepository<TTables> repository)
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
            IQueryable<TTables> data;

            if (!string.IsNullOrWhiteSpace(searchInfo.DependOn))
            {
                if (string.IsNullOrWhiteSpace(searchInfo.DependValue))
                {
                    return Enumerable.Empty<LookupItem>();
                }

                data = _repository.GetByTableName(searchInfo.DependValue);
            }
            else
            {
                data = _repository.GetAllAsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                string searched = searchInfo.Search.ToLower();
                data = data.Where(x => x.ColumnName.ToLower().Contains(searched));
            }

            if (searchInfo.Limit > 0)
            {
                data = data.Take(searchInfo.Limit);
            }

            return data.Select(x => new LookupItem { DisplayValue = x.ColumnName, Value = x.ColumnName });
        }
    }
}