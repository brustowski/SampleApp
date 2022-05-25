using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.Models.Pipeline;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller provides actions for Pipeline Inbound Records Unique Data
    /// </summary>
    [RoutePrefix("api/inbound/pipeline/unique-data")]
    public class PipelineInboundUniqueDataController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Pipeline Inbound records
        /// </summary>
        private readonly IPipelineFilingDataRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundUniqueDataController" /> class
        /// </summary>
        /// <param name="repository">The repository of Inbound records</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        public PipelineInboundUniqueDataController(
            IPipelineFilingDataRepository repository,
            ISearchRequestFactory searchRequestFactory
            )
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
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public async Task<int> GetTotalMatches([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<PipelineFilingData>(data);

            return await _repository.GetTotalMatchesAsync<PipelineFilingData>(searchRequest);
        }

        /// <summary>
        /// Searches for Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public async Task<SimplePagedResult<PipelineInboundUniqueDataViewModel>> Search([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<PipelineFilingData>(data);

            var pagedResult = await _repository.GetAllAsync<PipelineFilingData>(searchRequest);

            var result = pagedResult.Map<SimplePagedResult<PipelineFilingData>, SimplePagedResult<PipelineInboundUniqueDataViewModel>>();

            return result;
        }
    }
}