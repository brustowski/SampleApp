using System;
using System.Collections.Generic;
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
using FilingPortal.Domain.Services.Audit.Rail;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Audit.Rail
{
    /// <summary>
    /// Controller provides actions for Audit Rail Train consist sheet importing
    /// </summary>
    [RoutePrefix("api/audit/rail/train-consist-sheet")]
    public class RailAuditTrainConsistSheetImportingController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private readonly IRailAuditExcelFileImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailAuditTrainConsistSheetImportingController"/> class.
        /// </summary>
        /// <param name="importService">The importService <see cref="IRailAuditExcelFileImportService"/></param>
        public RailAuditTrainConsistSheetImportingController(IRailAuditExcelFileImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files with Truck Export records.
        /// </summary>
        /// <param name="request">The request</param>
        [Route("upload")]
        [HttpPost]
        [PermissionRequired(Permissions.AuditRailImportTrainConsistSheet)]
        public async Task<IHttpActionResult> ProcessFile(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            HttpPostedFile file = data.Files.First().Value;

            FileProcessingResult processResult = _importService.Process(file.Name, file.Path, User.Identity.Name);

            foreach (KeyValuePair<string, HttpPostedFile> item in data.Files)
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

            FileProcessingResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }
    }
}