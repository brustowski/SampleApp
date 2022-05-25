using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// Data provider for US States
    /// </summary>
    public class StatesDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// USStates tables repository
        /// </summary>
        private readonly IUSStatesRepository<USStates> _repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="StatesDataProvider"/> class
        /// </summary>
        /// <param name="repository"> US_States repository </param>
        public StatesDataProvider(IUSStatesRepository<USStates> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.StateNames;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<USStates> data = _repository.GetStateData(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.StateName + " - " + x.StateCode, Value = x.StateCode });
        }
    }
}