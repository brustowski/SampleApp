using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services;
using Framework.Domain.Paging;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.VesselExport
{
    /// <summary>
    /// Controller provides actions for Vessel Export Filing Grid data
    /// </summary>
    [RoutePrefix("api/export/vessel/single-filing-grid")]
    public class VesselExportSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<VesselExportRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportSingleFilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public VesselExportSingleFilingGridController(ISingleFilingGridService<VesselExportRecord> filingGridService)
        {
            _filingGridService = filingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
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
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _filingGridService.GetData(data);
        }
    }
}