using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Provider for Domestic ports data 
    /// </summary>
    public class DomesticPortDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Domestic ports handbook repository
        /// </summary>
        private readonly IDomesticPortsRepository _repository;

        /// <summary>
        /// Initialize Domestic ports handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public DomesticPortDataProvider(IDomesticPortsRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.DomesticPorts;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<DomesticPort> data = _repository.Search(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.PortCode, Value = x.PortCode });
        }
    }
}