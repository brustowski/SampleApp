using FilingPortal.Domain.Repositories.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for handbook lookup data provider
    /// </summary>
    public class HandbookDataProvider : IHandbookDataProvider
    {
        /// <summary>
        /// Handbooks repository
        /// </summary>
        private readonly IHandbookRepository _repository;

        /// <summary>
        /// Registry of the Handbook Lookup Data Providers
        /// </summary>
        private readonly IHandbookDataProviderRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandbookDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        /// <param name="registry">Registry of the Handbook Lookup Data Providers</param>
        public HandbookDataProvider(IHandbookRepository repository, IHandbookDataProviderRegistry registry)
        {
            _repository = repository;
            _registry = registry;
        }

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="handbookName">Handbook name</param>
        /// <param name="searchInfo">The search information</param>
        public IEnumerable<LookupItem<string>> GetData(string handbookName, SearchInfo searchInfo)
        {
            return handbookName.StartsWith("system.")
                ? GetDataFromSystemHandbook(handbookName, searchInfo)
                : GetDataFromUserDefinedHandbook(handbookName, searchInfo);
        }

        private IEnumerable<LookupItem<string>> GetDataFromSystemHandbook(string handbookName, SearchInfo searchInfo)
        {
            var providerName = handbookName.Split('.')[1];
            ILookupDataProvider provider = _registry.GetProvider(providerName);
            return provider.GetData(searchInfo).Select(x => new LookupItem<string> { DisplayValue = x.DisplayValue, Value = Convert.ToString(x.Value), IsDefault = x.IsDefault });

        }

        private IEnumerable<LookupItem<string>> GetDataFromUserDefinedHandbook(string handbookName, SearchInfo searchInfo)
        {
            IList<LookupItem<string>> results = _repository.GetOptions(handbookName, searchInfo.Search, searchInfo.Limit);
            results.ForEach(x =>
            {
                if (string.IsNullOrWhiteSpace(x.DisplayValue))
                {
                    x.DisplayValue = x.Value?.ToString();
                }
            });
            return results;
        }

    }
}