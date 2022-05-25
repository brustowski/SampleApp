using System.Web.Http;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.Truck;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Truck
{
    /// <summary>
    /// Controller provides actions for Inbound Records Unique Data
    /// </summary>
    [RoutePrefix("api/inbound/truck/single-filing-grid")]
    public class TruckSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ITruckSingleFilingGridService _singleFilingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckSingleFilingGridController" /> class
        /// </summary>
        /// <param name="singleFilingGridService">Single filing service</param>
        public TruckSingleFilingGridController(ITruckSingleFilingGridService singleFilingGridService)
        {
            _singleFilingGridService = singleFilingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
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
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _singleFilingGridService.GetData(data);
        }
    }
}