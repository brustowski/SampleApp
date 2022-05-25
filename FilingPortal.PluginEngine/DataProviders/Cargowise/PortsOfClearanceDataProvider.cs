using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.PluginEngine.DataProviders.Cargowise
{
    /// <summary>
    /// Provider for Cargowise ports of clearance codes data 
    /// </summary>
    public class PortsOfClearanceDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Cargowise ports of clearance repository
        /// </summary>
        private readonly IPortsOfClearanceRepository _repository;

        /// <summary>
        /// Initialize Vessel Discharge Terminal handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public PortsOfClearanceDataProvider(IPortsOfClearanceRepository repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.CargowisePortsOfClearance;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey)
            {
                PortOfClearance record = _repository.GetByCode(searchInfo.Search);
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = $"{record.Code} - {record.Office}", Value = record.Code });
                }

                return result;
            }

            IEnumerable<PortOfClearance> data;
            data = _repository.Search(searchInfo.Search, searchInfo.Limit);

            return data.Select(x => new LookupItem { DisplayValue = $"{x.Code} - {x.Office}", Value = x.Code });
        }
    }
}