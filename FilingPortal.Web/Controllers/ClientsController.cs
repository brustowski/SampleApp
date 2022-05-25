using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Models.ClientManagement;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Clients management
    /// </summary>
    [RoutePrefix("api/clients")]
    public class ClientsController : ApiControllerBase
    {
        /// <summary>
        /// The repository for Clients
        /// </summary>
        private readonly IClientRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController" /> class
        /// </summary>
        /// <param name="repository">Clients repository</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        public ClientsController(
            IClientRepository repository,
            ISearchRequestFactory searchRequestFactory
            )
        {
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewClients)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<Client>(data);
            return await _repository.GetTotalMatchesAsync<Client>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewClients)]
        public async Task<SimplePagedResult<ClientViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<Client>(data);

            SimplePagedResult<Client> pagedResult = await _repository.GetAllAsync<Client>(searchRequest);

            SimplePagedResult<ClientViewModel> result = pagedResult.Map<SimplePagedResult<Client>, SimplePagedResult<ClientViewModel>>();

            return result;
        }
    }
}