using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Discharge Port data 
    /// </summary>
    public class DischargePortDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// LookupMaster tables repository
        /// </summary>
        private readonly ILookupMasterRepository<LookupMaster> _repository;

        /// <summary>
        /// Initialize Vessel Loading Port handbook repository
        /// </summary>
        /// <param name="repository">Data repository</param>
        public DischargePortDataProvider(ILookupMasterRepository<LookupMaster> repository) => _repository = repository;

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.DischargePorts;

        /// <summary>
        /// Gets the collection of flag items by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<LookupMaster> data = _repository.GetExportDischargePort(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.Value} - {x.DisplayValue}", Value = x.Value });
        }
    }
}