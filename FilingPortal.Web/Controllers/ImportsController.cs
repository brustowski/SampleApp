using FilingPortal.Domain;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Import.TemplateEngine;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FilingPortal.Web.Controllers
{

    /// <summary>
    /// Provides import functionality for Truck Export records
    /// </summary>
    [RoutePrefix("api/imports")]
    public class ImportsController : ApiControllerBase
    {
        /// <summary>
        /// The template configuration registry
        /// </summary>
        private readonly IImportConfigurationRegistry _registry;
        /// <summary>
        /// The template service
        /// </summary>
        private readonly ITemplateService _service;
        /// <summary>
        /// The template processing service factory
        /// </summary>
        private readonly ITemplateProcessingServiceFactory _factory;
        /// <summary>
        /// The documents repository
        /// </summary>
        private readonly IAppDocumentRepository _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportsController"/> class
        /// </summary>
        /// <param name="registry">The template configuration registry</param>
        /// <param name="service">The template service</param>
        /// <param name="factory">The template processing service factory</param>
        /// <param name="documentRepository">The document repository</param>
        public ImportsController(
            IImportConfigurationRegistry registry
            , ITemplateService service
            , ITemplateProcessingServiceFactory factory
            , IAppDocumentRepository documentRepository)
        {
            _registry = registry;
            _service = service;
            _factory = factory;
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Gets file template name by template name
        /// </summary>
        /// <param name="templateName">The template name</param>
        [HttpGet]
        [Route("templates/{templateName}")]
        [PermissionRequired]
        public HttpResponseMessage GetTemplateByName(string templateName)
        {
            try
            {
                IImportConfiguration configuration = _registry.GetConfiguration(templateName);

                if (configuration == null)
                {
                    var message = $"The template with '{templateName}' was not found.";
                    AppLogger.Error(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, message);
                }

                if (!CurrentUser.HasPermissions(configuration.Permissions))
                {
                    AppLogger.Error(ErrorMessages.InsufficientPermissions);
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions);
                }

                FileExportResult result = _service.Create(configuration);
                return SendAsFileStream(result.DocumentExternalName, result.FileName);
            }
            catch (Exception exception)
            {
                var message = $"An unexpected error occurred. Please provide valid data or contact administrator. {exception.Message}";
                AppLogger.Error(exception, message);
                throw CreateResponseException(HttpStatusCode.BadRequest, message);
            }
        }

        /// <summary>
        /// Parse and validate the uploaded Excel file
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="gridName">Grid name</param>
        [Route("upload/grids/{gridName}")]
        [HttpPost]
        [PermissionRequired]
        public async Task<IHttpActionResult> ProcessFile(HttpRequestMessage request, string gridName)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            HttpPostedFile file = data.Files.First().Value;

            IImportConfiguration configuration;

            try
            {
                configuration = _registry.GetConfiguration(gridName);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(e.Message);
            }

            if (!CurrentUser.HasPermissions(configuration.Permissions))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                return ResponseMessage(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions));
            }

            ITemplateProcessingService instance = _factory.Create(configuration);

            FileProcessingResult processResult = instance.Process(file.Name, file.Path, User.Identity.Name);

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

        /// <summary>
        /// Uploading a file to the system for further processing
        /// </summary>
        /// <param name="request">The request, should contains the file and the file type</param>
        [Route("uploads")]
        [HttpPost]
        [PermissionRequired]
        public async Task<IHttpActionResult> UploadFile(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            HttpPostedFile file = data.Files.First().Value;

            var document = new AppDocument { FileContent = File.ReadAllBytes(file.Path), FileName = file.Name, CreatedUser = CurrentUser.Id, CreatedDate = DateTime.Now };
            _documentRepository.Add(document);
            await _documentRepository.SaveAsync();

            return Ok(document.Id);
        }

        /// <summary>
        /// Checks whether specified file can be imported or not
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="gridName">The name of the grid</param>
        [Route("uploads/{fileId}/{gridName}")]
        [HttpGet]
        [PermissionRequired]
        public IHttpActionResult CheckUploadedFile(string fileId, string gridName = null)
        {
            if (!int.TryParse(fileId, out var id))
            {
                return BadRequest("Document identifier format mismatch");
            }

            if (!_documentRepository.IsExist(id))
            {
                return BadRequest("Document with specified identifier is not found");
            }

            IImportConfiguration configuration = _registry.GetConfiguration(gridName);
            if (!CurrentUser.HasPermissions(configuration.Permissions))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                return ResponseMessage(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions));
            }

            ITemplateProcessingService instance = _factory.Create(configuration);

            AppDocument document = _documentRepository.GetDocument(id);
            FileProcessingDetailedResult processResult = instance.Verify(document);
            FileProcessingResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingDetailedResult, FileProcessingDetailedResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }

        /// <summary>
        /// Imports data from the specified file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// /// <param name="gridName">The name of the grid</param>
        [Route("uploads/{fileId}/{gridName}")]
        [HttpPost]
        [PermissionRequired]
        public IHttpActionResult ImportDataFromUploadedFile(string fileId, string gridName)
        {
            if (!int.TryParse(fileId, out var id))
            {
                return BadRequest("Document identifier format mismatch");
            }

            if (!_documentRepository.IsExist(id))
            {
                return BadRequest("Document with specified identifier is not found");
            }

            IImportConfiguration configuration = _registry.GetConfiguration(gridName);
            if (!CurrentUser.HasPermissions(configuration.Permissions))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                return ResponseMessage(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions));
            }

            ITemplateProcessingService instance = _factory.Create(configuration);

            AppDocument document = _documentRepository.GetDocument(id);
            FileProcessingDetailedResult processResult = instance.Import(document, CurrentUser.Id);

            FileProcessingResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingDetailedResult, FileProcessingDetailedResultViewModel<ExcelFileValidationError>>();

            return Ok(result);
        }

        /// <summary>
        /// Delete specified file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        [Route("uploads/{fileId}")]
        [HttpDelete]
        [PermissionRequired]
        public IHttpActionResult DeleteUploadedFile(string fileId)
        {
            if (!int.TryParse(fileId, out var id))
            {
                return BadRequest("Document identifier format mismatch");
            }

            if (!_documentRepository.IsExist(id))
            {
                return BadRequest("Document with specified identifier is not found");
            }

            _documentRepository.Delete(id);

            return Ok();
        }

        /// <summary>
        /// Parses the uploaded Excel file
        /// </summary>
        /// <param name="request">The request</param>
        [Route("form-data")]
        [HttpPost]
        [PermissionRequired]
        public async Task<IHttpActionResult> ProcessFormData(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }
            HttpPostedFile file = data.Files.First().Value;

            string section;
            string workflow;
            int parentRecordId;
            int filingRecordId;
            try
            {
                section = data.Fields["section"].Value;
                workflow = data.Fields["workflow"].Value;
                parentRecordId = Convert.ToInt32(data.Fields["parentId"].Value);
                filingRecordId = Convert.ToInt32(data.Fields["filingHeaderId"].Value);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest($"The request does not contains section, filing header id or parent record id data");
            }

            var configuration = (IFormImportConfiguration)_registry.GetConfiguration(workflow);
            if (!CurrentUser.HasPermissions(configuration.Permissions))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                return ResponseMessage(ActionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions));
            }

            configuration.ParentRecordId = parentRecordId;
            configuration.FilingHeaderId = filingRecordId;
            configuration.Section = section;

            IFormDataTemplateProcessingService instance = _factory.Create(configuration);
            FileProcessingDetailedResult processResult = instance.Process(file.Name, file.Path, configuration);

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

            FileProcessingDetailedResultViewModel<ExcelFileValidationError> result = processResult.Map<FileProcessingDetailedResult, FileProcessingDetailedResultViewModel<ExcelFileValidationError>>();
            return Ok(result);
        }
    }
}