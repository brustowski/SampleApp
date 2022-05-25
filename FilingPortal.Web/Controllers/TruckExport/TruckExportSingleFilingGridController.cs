using System.Web.Http;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller provides actions for Truck Export Filing Grid data
    /// </summary>
    [RoutePrefix("api/export/truck/single-filing-grid")]
    public class TruckExportSingleFilingGridController : ApiControllerBase
    {
        /// <summary>
        /// Single filing grid service that processes all requests and response data
        /// </summary>
        private readonly ISingleFilingGridService<TruckExportRecord> _filingGridService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportSingleFilingGridController" /> class
        /// </summary>
        /// <param name="filingGridService">Single filing service</param>
        public TruckExportSingleFilingGridController(ISingleFilingGridService<TruckExportRecord> filingGridService)
        {
            _filingGridService = filingGridService;
        }

        /// <summary>
        /// Gets the total matches of Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
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
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public SimplePagedResult<dynamic> Search([FromBody]SearchRequestModel data)
        {
            return _filingGridService.GetData(data);
        }
    }
}