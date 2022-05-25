using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Country data 
    /// </summary>
    public class CountryNameDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Vessel Loading Port handbook repository
        /// </summary>
        private readonly IDataProviderRepository<Country, int> _repository;

        /// <summary>
        /// Initialize Vessel Loading Port handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public CountryNameDataProvider(IDataProviderRepository<Country, int> repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.CountryNames;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<Country> data = _repository.Search(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.Name, Value = x.Name });
        }
    }
}