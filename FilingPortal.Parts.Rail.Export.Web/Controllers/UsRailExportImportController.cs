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
using FilingPortal.Domain.Mapping;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Rail.Export.Domain.Enums;
using FilingPortal.Parts.Rail.Export.Domain.Services;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using Framework.Infrastructure;

namespace FilingPortal.Parts.Rail.Export.Web.Controllers
{
    /// <summary>
    /// Provides file import functionality
    /// </summary>
    [RoutePrefix("api/us/export/rail")]
    public class UsRailExportImportController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private readonly IInboundImportExcelFileImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsRailExportImportController"/> class.
        /// </summary>
        /// <param name="importService">The importService<see cref="IInboundImportExcelFileImportService"/></param>
        public UsRailExportImportController(IInboundImportExcelFileImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files with records.
        /// </summary>
        /// <param name="request">The request</param>
        [Route("import")]
        [HttpPost]
        [PermissionRequired(Permissions.ImportInboundRecord)]
        public async Task<IHttpActionResult> ImportFromFile(HttpRequestMessage request)
        {
            IEnumerable<HttpPostedFile> files = await Upload(request);

            if (files == null)
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            HttpPostedFile file = files.First();

            FileProcessingResult processResult = _importService.Process(file.Name, file.Path, User.Identity.Name);

            Clear(files);

            FileProcessingResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }

        private async Task<List<HttpPostedFile>> Upload(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            return !data.Files.Any() ? null : data.Files.Values.ToList();
        }

        private static void Clear(IEnumerable<HttpPostedFile> files)
        {
            foreach (HttpPostedFile item in files)
            {
                try
                {
                    File.Delete(item.Path);
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex, ErrorMessages.FileDeletingError);
                }
            }
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files
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
