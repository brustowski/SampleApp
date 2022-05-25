using FilingPortal.Domain.Repositories.Clients;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for importer supplier code
    /// </summary>
    public class ImporterCodeDataProvider : ILookupDataProvider
    {
        /// <summary>
        /// Client tables repository
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterCodeDataProvider"/> class
        /// </summary>
        /// <param name="repository">Client table repository</param>
        public ImporterCodeDataProvider(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public string Name => DataProviderNames.ImporterCodes;

        /// <summary>
        /// Gets the data by specified search information
        /// </summary>
        /// <param name="searchInfo">The search information</param>

        public IEnumerable<LookupItem> GetData(SearchInfo searchInfo)
        {
            if (string.IsNullOrEmpty(searchInfo.Search))
            {
                return null;
            }

            IEnumerable<Client> data = _repository.GetImporters(searchInfo.Search, searchInfo.Limit);

            return data.Select(x => new LookupItem { DisplayValue = x.ClientCode, Value = x.ClientCode });
        }
    }
}