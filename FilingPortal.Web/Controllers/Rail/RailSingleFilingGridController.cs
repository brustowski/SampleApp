using System.Web.Http;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.Rail;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Inbound Records Unique Data
    /// </summary>
    [RoutePrefix("api/inbound/rail/single-filing-grid")]
    public class RailSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly IRailSingleFilingGridService _singleFilingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundUniqueDataController" /> class
        /// </summary>
        /// <param name="singleFilingGridService">Single filing service</param>
        public RailSingleFilingGridController(IRailSingleFilingGridService singleFilingGridService)
        {
            _singleFilingGridService = singleFilingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
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
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetData(data);
        }

        /// <summary>
        /// Gets the total matches of Manifest Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("manifest-data/getTotalMatches")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public int GetManifestDataCount([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetManifestDataCount(data);
        }

        /// <summary>
        /// Searches for Manifest Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("manifest-data/search")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public SimplePagedResult<RailFilingData> GetManifestData([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetManifestData(data);
        }
    }
}