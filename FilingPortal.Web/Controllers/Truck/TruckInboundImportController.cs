using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Controllers.Truck
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using FilingPortal.Domain;
    using FilingPortal.Domain.Common.Import.Models;
    using FilingPortal.Domain.Common.Parsing;
    using FilingPortal.Domain.Enums;
    using FilingPortal.Domain.Mapping;
    using FilingPortal.Domain.Services.Truck;
    using FilingPortal.Infrastructure.Common;
    using FilingPortal.Web.Models;
    using Framework.Infrastructure;

    /// <summary>
    /// Provides import functionality for Truck Inbound records
    /// </summary>
    [RoutePrefix("api/inbound/truck")]
    public class TruckInboundImportController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private ITruckInboundExcelFileImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundImportController"/> class.
        /// </summary>
        /// <param name="importService">The importService<see cref="ITruckInboundExcelFileImportService"/></param>
        public TruckInboundImportController(ITruckInboundExcelFileImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files with Truck Inbound records.
        /// </summary>
        /// <param name="request">The request</param>
        [Route("upload")]
        [HttpPost]
        [PermissionRequired(Permissions.TruckImportInboundRecord)]
        public async Task<IHttpActionResult> ProcessFile(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            var data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            var file = data.Files.First().Value;

            var processResult = _importService.Process(file.Name, file.Path, User.Identity.Name);

            foreach (var item in data.Files)
            {
                try
                {
                    File.Delete(item.Value.Path);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex, ErrorMessages.FileDeletingError);
                }
            }

            var result = processResult.Map<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }
    }
}
