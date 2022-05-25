using System.Web.Http;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Enums;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Rail.Export.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Filing Grid data
    /// </summary>
    [RoutePrefix("api/us/export/rail/filing-grid")]
    public class UsRailExportFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<InboundRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsRailExportFilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public UsRailExportFilingGridController(ISingleFilingGridService<InboundRecord> filingGridService)
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