using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Zones.Ftz214.Domain.Enums;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Services;
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

namespace FilingPortal.Parts.Zones.Ftz214.Web.Controllers
{
    /// <summary>
    /// Provides import functionality for inbound records
    /// </summary>
    [RoutePrefix("api/zones/ftz-214")]
    public class ZonesFtz214ImportController : ApiControllerBase
    {
        /// <summary>
        /// File import service
        /// </summary>
        private readonly IXmlFileImportService _importService;

        /// <summary>
        /// XML files repository
        /// </summary>
        private readonly IInboundXmlRepository _xmlRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesFtz214ImportController"/> class.
        /// </summary>
        /// <param name="importService">The XML import service<see cref="IXmlFileImportService"/></param>
        /// <param name="xmlRepository">Inbound XML repository</param>
        public ZonesFtz214ImportController(IXmlFileImportService importService, IInboundXmlRepository xmlRepository)
        {
            _importService = importService;
            _xmlRepository = xmlRepository;
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

            FileProcessingResultViewModel<ExcelFileValidationError> result =
                processResult.Map<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }

        /// <summary>
        /// Parse and validate the uploaded XML files with Zones Ftz214 06 record
        /// </summary>
        /// <param name="request">The request</param>
        [Route("upload-xml")]
        [HttpPost]
        [PermissionRequired(Permissions.ImportInboundRecord)]
        public async Task<IHttpActionResult> ProcessXmlFile(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            var processResults = new List<FileProcessingDetailedResult>();

            foreach (HttpPostedFile file in data.Files.Values)
            {
                processResults.Add(_importService.Process(file.Name, file.Path, User.Identity.Name));
            }

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

            var unitedResult = new FileProcessingDetailedResultViewModel<ExcelFileValidationError>
            {
                Updated = processResults.Sum(x => x.Updated),
                Inserted = processResults.Sum(x => x.Inserted),
                FileName = string.Join(", ", processResults.Select(x => x.FileName).Take(3)),
                CommonErrors = processResults.SelectMany(x => x.CommonErrors),
                Count = processResults.Count,
                ParsingErrors = processResults.SelectMany(x => x.ParsingErrors)
                    .Map<RowError, ExcelFileValidationError>(),
                ValidationErrors = processResults.SelectMany(x => x.ValidationErrors)
                    .Map<RowError, ExcelFileValidationError>()
            };


            return Ok(unitedResult);
        }

        /// <summary>
        /// Parse and validate the uploaded XML files with Zones Ftz214 06 record
        /// </summary>
        [Route("auto-process")]
        [HttpPost]
        [PermissionRequired(Permissions.ImportInboundRecord)]
        public async Task<IHttpActionResult> ProcessInboundXmls()
        {
            IList<Domain.Entities.InboundXml> files = await _xmlRepository.GetUnprocessedFiles();

            var processResults = new List<FileProcessingResult>();

            if (files.Any())
            {
                foreach (Domain.Entities.InboundXml inboundXml in files)
                {
                    FileProcessingResult processingResult =
                        _importService.Process(inboundXml.FileName, inboundXml.Content, User.Identity.Name);

                    if (processingResult.IsValid)
                    {
                        _xmlRepository.Delete(inboundXml);
                    }
                    else
                    {
                        inboundXml.Status = 1;
                        inboundXml.ValidationResult = $"Common: {string.Join(", ", processingResult.CommonErrors)}, " +
                                                      $"Parsing: {string.Join(", ", processingResult.ParsingErrors.Select(x => x.ToString()))}, " +
                                                      $"Validation: {string.Join(", ", processingResult.ValidationErrors.Select(x => x.ToString()))}";
                    }

                    processResults.Add(processingResult);
                }

                await _xmlRepository.SaveAsync();
            }

            IEnumerable<FileProcessingResultViewModel<XmlFileValidationError>> result =
                processResults.Map<FileProcessingResult, FileProcessingResultViewModel<XmlFileValidationError>>();

            return Ok(result);
        }
    }
}
