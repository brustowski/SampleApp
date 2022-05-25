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
    public class RailTableNamesDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Rail tables repository
        /// </summary>
        private readonly ITablesRepository<RailTables> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The rail tables repository</param>
        public RailTableNamesDataProvider(ITablesRepository<RailTables> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.RailTableNames;

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