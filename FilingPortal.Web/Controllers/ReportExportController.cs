using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.DTOs.ReviewSectionModels;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers
{

    /// <summary>
    /// Controller that provides actions for report creation of the grid data
    /// </summary>
    [RoutePrefix("api/reports")]
    public class ReportExportController : ApiControllerBase
    {
        /// <summary>
        /// The exporting service
        /// </summary>
        private readonly IReportExportingService _exportingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportExportController"/> class
        /// </summary>
        /// <param name="exportingService">The exporting service</param>
        public ReportExportController(IReportExportingService exportingService)
        {
            _exportingService = exportingService;
        }

        /// <summary>
        /// Exports grid data specified by grid name to Excel file using serialized search data
        /// </summary>
        /// <param name="gridName">Grid name</param>
        /// <param name="data">The serialized search data</param>
        [HttpGet]
        [Route("ExportToExcel")]
        [PermissionRequired]
        public async Task<FileExportResult> ExportToExcel(string gridName, string data)
        {
            SearchRequestModel searchRequestModel = FromBase64String<SearchRequestModel>(data);

            FileExportResult fileContent = await _exportingService.GetExportingResult(gridName, searchRequestModel);

            return fileContent;
        }

        /// <summary>
        /// Downloads the report file.
        /// </summary>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>HttpResponseMessage.</returns>
        [HttpGet]
        [Route("Download")]
        [PermissionRequired]
        public HttpResponseMessage DownloadExportToExcel(string documentName, string fileName)
        {
            return SendAsFileStream(documentName, fileName);
        }

        /// <summary>
        /// Exports grid data specified by grid name to Excel file using serialized search data and download it.
        /// </summary>
        /// <param name="gridName">Grid name</param>
        /// <param name="data">The serialized search data</param>
        [HttpGet]
        [Route("DownloadExportToExcel")]
        [PermissionRequired]
        public async Task<HttpResponseMessage> ExportToExcelDownload(string gridName, string data)
        {
            FileExportResult fileContent = await ExportToExcel(gridName, data);

            return SendAsFileStream(fileContent.DocumentExternalName, fileContent.FileName);
        }

        /// <summary>
        /// Exports grid data specified by grid name to Excel file using serialized search data and download it.
        /// </summary>
        /// <param name="model">Current information on review and create screen</param>
        [HttpPost]
        [Route("form-data")]
        [PermissionRequired]
        public HttpResponseMessage ExportReviewSectionGrid(ReviewSectionExportModel model)
        {
            FileExportResult fileContent = _exportingService.GetExportingResult(model);

            return SendAsFileStream(fileContent.DocumentExternalName, fileContent.FileName);
        }
    }
}