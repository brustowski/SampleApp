using System.Linq;
using System.Web.Http;
using FilingPortal.Domain.Infrastructure.Helpers;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Enums;
using FilingPortal.Parts.Rail.Export.Domain.Repositories;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Rail.Export.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Containers Grid data
    /// </summary>
    [RoutePrefix("api/us/export/rail/containers-grid")]
    public class UsRailExportContainersGridController : ApiControllerBase
    {
        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsRailExportFilingGridController" /> class
        /// </summary>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public UsRailExportContainersGridController(IInboundRecordsRepository inboundRecordsRepository)
        {
            _inboundRecordsRepository = inboundRecordsRepository;
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
            var filingHeaderId = data.ExtractFilingHeaders().First();

            var records = _inboundRecordsRepository.GetByFilingId(filingHeaderId);

            return records.Sum(x => x.Containers.Count);
        }

        /// <summary>
        /// Searches for Unique Data items by specified searching data
        /// </summary>
        /// <param name="data">The searching data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public SimplePagedResult<InboundRecordContainerViewModel> Search([FromBody]SearchRequestModel data)
        {
            var filingHeaderId = data.ExtractFilingHeaders().First();

            var records = _inboundRecordsRepository.GetByFilingId(filingHeaderId);

            var containers = records.SelectMany(x => x.Containers).ToList();

            return new SimplePagedResult<InboundRecordContainerViewModel>()
            {
                CurrentPage = 1,
                PageSize = containers.Count(),
                Results = containers.Map<InboundRecordContainer, InboundRecordContainerViewModel>()
            };
        }
    }
}