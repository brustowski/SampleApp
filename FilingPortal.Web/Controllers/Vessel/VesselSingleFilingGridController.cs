using System.Web.Http;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Vessel
{
    /// <summary>
    /// Controller provides actions for Vessel Import Filing Grid data
    /// </summary>
    [RoutePrefix("api/inbound/vessel/single-filing-grid")]
    public class VesselSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<VesselImportRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselSingleFilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public VesselSingleFilingGridController(ISingleFilingGridService<VesselImportRecord> filingGridService)
        {
            _filingGridService = filingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
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
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _filingGridService.GetData(data);
        }
    }
}