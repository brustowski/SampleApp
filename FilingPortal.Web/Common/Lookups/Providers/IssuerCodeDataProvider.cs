using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Issuer Code
    /// </summary>
    public class IssuerCodeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// LookupMaster tables repository
        /// </summary>
        private readonly IDataProviderRepository<IssuerCode, int> _repository;

        /// <summary>
        /// Initialize a new instance of the <see cref="IssuerCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">LookupMaster table repository</param>
        public IssuerCodeDataProvider(IDataProviderRepository<IssuerCode, int> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.IssuerCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<IssuerCode> data = _repository.Search(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.Code} - {x.Name}", Value = x.Code });
        }
    }
}