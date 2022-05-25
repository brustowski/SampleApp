using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class RailTableColumnsDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Rail tables repository
        /// </summary>
        private readonly ITablesRepository<RailTables> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailTableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The rail tables repository</param>
        public RailTableColumnsDataProvider(ITablesRepository<RailTables> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.RailTableColumns;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IQueryable<RailTables> data;

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