using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.Infrastructure.Common;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Provides import functionality for Truck Export records
    /// </summary>
    [RoutePrefix("api/export/truck")]
    public class TruckExportImportController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private readonly ITruckExportExcelFileImportService _importService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportImportController"/> class.
        /// </summary>
        /// <param name="importService">The importService<see cref="ITruckExportExcelFileImportService"/></param>
        public TruckExportImportController(ITruckExportExcelFileImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Parse and validate the uploaded Excel files with Truck Export records. 
        /// </summary>
        /// <param name="request">The request</param>
        [Route("import")]
        [HttpPost]
        [PermissionRequired(Permissions.TruckImportExportRecord)]
        public async Task<IHttpActionResult> ImportFromFile(HttpRequestMessage request)
        {
            IEnumerable<HttpPostedFile> files = await Upload(request);

            if (files == null)
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            HttpPostedFile file = files.First();

            FileProcessingDetailedResult processResult = _importService.Process(file.Name, file.Path, User.Identity.Name);

            Clear(files);

            FileProcessingDetailedResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingDetailedResult, FileProcessingDetailedResultViewModel<ExcelFileValidationError>>();

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
    }
}
