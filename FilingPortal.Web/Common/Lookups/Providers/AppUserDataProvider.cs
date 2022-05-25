using System;
using FilingPortal.Domain.AppSystem.Repositories;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for App users data
    /// </summary>
    public class AppUserDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Application users data repository
        /// </summary>
        private readonly IAppUsersDataRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppUserDataProvider"/> class
        /// </summary>
        /// <param name="repository">Application user data repository</param>
        public AppUserDataProvider(IAppUsersDataRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ApplicationUserData;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                var record = _repository.GetUserData(searchInfo.Search);
                var result = new List<LookupItem>();
                if (record != null)
                    result.Add(new LookupItem { DisplayValue = record.Broker, Value = record.Id });
                return result;
            }

            IList<AppUsersData> data = _repository.GetData(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = x.Broker, Value = x.Id });
        }
    }
}