using System.Web.Http;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Zones.Entry.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Filing Grid data
    /// </summary>
    [RoutePrefix("api/zones/entry-06/filing-grid")]
    public class ZonesEntryFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<InboundRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesEntryFilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public ZonesEntryFilingGridController(ISingleFilingGridService<InboundRecord> filingGridService)
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