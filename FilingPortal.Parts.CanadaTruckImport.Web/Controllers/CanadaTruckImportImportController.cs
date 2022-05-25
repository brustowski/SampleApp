using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Mapping;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.CanadaTruckImport.Domain.Services;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models;
using Framework.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Controllers
{
    /// <summary>
    /// Provides import functionality for Truck Export records
    /// </summary>
    [RoutePrefix("api/canada-imp-truck")]
    public class CanadaTruckImportImportController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private readonly IInboundImportExcelFileImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanadaTruckImportImportController"/> class.
        /// </summary>
        /// <param name="importService">The importService<see cref="IInboundImportExcelFileImportService"/></param>
        public CanadaTruckImportImportController(IInboundImportExcelFileImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files with Truck Export records.
        /// </summary>
        /// <param name="request">The request</param>
        [Route("upload")]
        [HttpPost]
        [PermissionRequired(Permissions.ImportInboundRecord)]
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

            foreach (System.Collections.Generic.KeyValuePair<string, HttpPostedFile> item in data.Files)
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

            FileProcessingResultViewModel<ExcelFileValidationError> result =
                processResult.Map<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }
    }
}
