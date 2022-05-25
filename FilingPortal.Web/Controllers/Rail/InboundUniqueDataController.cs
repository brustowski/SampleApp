using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Models.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Inbound Records Unique Data
    /// </summary>
    [RoutePrefix("api/inbound/rail/unique-data")]
    public class InboundUniqueDataController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Rail Inbound records
        /// </summary>
        private readonly IRailFilingDataRepository _repository;


        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundUniqueDataController" /> class
        /// </summary>
        /// <param name="repository">The repository of Rail Inbound records</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        public InboundUniqueDataController(IRailFilingDataRepository repository,
            ISearchRequestFactory searchRequestFactory)
        {
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public async Task<int> GetTotalMatches([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<RailFilingData>(data);

            return await _repository.GetTotalMatchesAsync<RailFilingData>(searchRequest);
        }

        /// <summary>
        /// Searches for Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public async Task<PagedResultWithSummaryRow<InboundUniqueDataItemViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<RailFilingData>(data);

            PagedResultWithSummaryRow<RailFilingData> pagedResult = await _repository.GetAllWithSummaryAsync(searchRequest);

            PagedResultWithSummaryRow<InboundUniqueDataItemViewModel> result = pagedResult.Map<PagedResultWithSummaryRow<RailFilingData>, PagedResultWithSummaryRow<InboundUniqueDataItemViewModel>>();
            return result;
        }
    }
}