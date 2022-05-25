using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.PluginEngine.DataProviders.FilterProviders
{
    /// <summary>
    /// Provider for importer lookup data provider with long title in value
    /// </summary>
    public class ClientFullNameWithEinDataProvider : IFilterDataProvider
    {
        private const string CodeType = "EIN";
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientFullNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public ClientFullNameWithEinDataProvider(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            IEnumerable<Client> data = _repository.GetAll(searchInfo.Search, CodeType, searchInfo.Limit);
            return data.Select(x => new LookupItem { DisplayValue = $"{x.ClientCode} - {x.ClientName} {GetEin(x)}", Value = x.ClientName });
        }

        private static string GetEin(Client record)
        {
            return string.Join("/", record.ClientNumbers.Where(x => x.CodeType == CodeType).Select(x => $"{x.CodeType}: {x.RegNumber}"));
        }
    }
}