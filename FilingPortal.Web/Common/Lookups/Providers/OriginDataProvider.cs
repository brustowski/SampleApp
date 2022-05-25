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
    /// Provider for Origin code
    /// </summary>
    public class OriginDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// LookupMaster tables repository
        /// </summary>
        private readonly ILookupMasterRepository<LookupMaster> _repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="OriginDataProvider"/> class
        /// </summary>
        /// <param name="repository">LookupMaster table repository</param>
        public OriginDataProvider(ILookupMasterRepository<LookupMaster> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.OriginCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<LookupMaster> data = _repository.GetOriginCodes(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.DisplayValue, Value = x.Value });
        }
    }
}