using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.PluginEngine.DataProviders
{
    /// <summary>
    /// The Client Codes lookup data provider
    /// </summary>
    public class ClientCodeDataProvider : ILookupDataProvider, IUiAvailable, IFilterDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public ClientCodeDataProvider(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ClientCode;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (searchInfo.SearchByKey && !string.IsNullOrWhiteSpace(searchInfo.Search))
            {
                Client record = _repository.GetClientByCode(searchInfo.Search);
                var result = new List<LookupItem>();
                if (record != null)
                {
                    result.Add(new LookupItem { DisplayValue = $"{record.ClientCode} - {record.ClientName}", Value = record.ClientCode });
                }

                return result;
            }

            IEnumerable<Client> data = _repository.GetAll(searchInfo.Search, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.ClientCode} - {x.ClientName}", Value = x.ClientCode });
        }
    }
}