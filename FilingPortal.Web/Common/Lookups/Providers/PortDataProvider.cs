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
    /// Provider for importer supplier code
    /// </summary>
    public class PortDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// LookupMaster tables repository
        /// </summary>
        private readonly ILookupMasterRepository<LookupMaster> _repository;


        /// <summary>
        /// Initialize a new instance of the <see cref="PortDataProvider"/> class
        /// </summary>
        /// <param name="repository">LookupMaster table repository</param>
        public PortDataProvider(ILookupMasterRepository<LookupMaster> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.PortCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<LookupMaster> data = _repository.GetPortCodes(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.DisplayValue, Value = x.Value });
        }
    }
}