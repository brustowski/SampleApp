using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;
using System.Web.Http;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Filing Grid data
    /// </summary>
    [RoutePrefix("api/zones/ftz-214/filing-grid")]
    public class ZonesFtz214FilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<InboundRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesFtz214FilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public ZonesFtz214FilingGridController(ISingleFilingGridService<InboundRecord> filingGridService)
        {
            _filingGridService = filingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public int GetTotalMatches([FromBody]SearchRequestModel data)
        {
            return _filingGridService.GetTotalMatches(data);
        }

        /// <summary>
        /// Searches for Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _filingGridService.GetData(data);
        }
    }
}