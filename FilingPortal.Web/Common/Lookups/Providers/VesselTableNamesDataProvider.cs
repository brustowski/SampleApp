using FilingPortal.Domain.Repositories.VesselImport;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class VesselTableNamesDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Vessel tables repository
        /// </summary>
        private readonly ITablesRepository<VesselImportTable> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The vessel tables repository</param>
        public VesselTableNamesDataProvider(ITablesRepository<VesselImportTable> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.VesselTableNames;

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