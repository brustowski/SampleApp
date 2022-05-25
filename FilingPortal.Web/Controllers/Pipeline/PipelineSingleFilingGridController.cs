using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller provides actions for Inbound Records Unique Data
    /// </summary>
    [RoutePrefix("api/inbound/pipeline/single-filing-grid")]
    public class PipelineSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly IPipelineSingleFilingGridService _singleFilingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineSingleFilingGridController" /> class
        /// </summary>
        /// <param name="singleFilingGridService">Single filing service</param>
        public PipelineSingleFilingGridController(IPipelineSingleFilingGridService singleFilingGridService)
        {
            _singleFilingGridService = singleFilingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public int GetTotalMatches([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetTotalMatches(data);
        }

        /// <summary>
        /// Searches for Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetData(data);
        }
    }
}