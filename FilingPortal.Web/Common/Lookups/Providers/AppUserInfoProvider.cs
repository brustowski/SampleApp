using System;
using FilingPortal.Domain.AppSystem.Repositories;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for Consignee code
    /// </summary>
    public class AppUserInfoProvider : ILookupDataProvider
    {
        /// <summary>
        /// Application user repository
        /// </summary>
        private readonly IAppUsersRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppUserInfoProvider"/> class
        /// </summary>
        /// <param name="repository">Application user repository</param>
        public AppUserInfoProvider(IAppUsersRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ApplicationUser;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                var record = _repository.GetUserInfo(searchInfo.Search);
                var result = new List<LookupItem>();
                if (record != null)
                    result.Add(new LookupItem { DisplayValue = record.Id, Value = record.Id });
                return result;
            }

            IList<string> data = _repository.GetLogins(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x, Value = x });
        }
    }
}