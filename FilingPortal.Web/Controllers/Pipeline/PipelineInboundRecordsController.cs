using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using FilingPortal.Domain;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller provides data for Pipeline Inbound Records 
    /// </summary>
    [RoutePrefix("api/inbound/pipeline")]
    public class PipelineInboundRecordsController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Pipeline Inbound records
        /// </summary>
        private readonly IPipelineInboundReadModelRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The command dispatcher
        /// </summary>
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// The page configuration container
        /// </summary>
        private readonly IPageConfigContainer _pageConfigContainer;

        /// <summary>
        /// The validator for selected inbound record set 
        /// </summary>
        private readonly IListInboundValidator<PipelineInboundReadModel> _recordsValidator;

        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<PipelineInboundReadModel> _recordValidator;

        /// <summary>
        /// Files upload service
        /// </summary>
        private readonly IFilingHeaderDocumentUpdateService<PipelineDocumentDto> _filingHeaderDocumentUpdateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundRecordsController" /> class
        /// </summary>
        /// <param name="repository">Pipeline Inbound records repository</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="recordsValidator">The validator for selected inbound record set </param>
        /// <param name="recordValidator">The single inbound record validator</param>
        /// <param name="filingHeaderDocumentUpdateService">Files upload service</param>
        public PipelineInboundRecordsController(
            IPipelineInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<PipelineInboundReadModel> recordsValidator,
            ISingleRecordValidator<PipelineInboundReadModel> recordValidator,
            IFilingHeaderDocumentUpdateService<PipelineDocumentDto> filingHeaderDocumentUpdateService)
        {
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
            _commandDispatcher = commandDispatcher;
            _pageConfigContainer = pageConfigContainer;
            _recordsValidator = recordsValidator;
            _recordValidator = recordValidator;
            _filingHeaderDocumentUpdateService = filingHeaderDocumentUpdateService;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<PipelineInboundReadModel>(data);
            return await _repository.GetTotalMatchesAsync<PipelineInboundReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public async Task<SimplePagedResult<PipelineViewModel>> Search([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<PipelineInboundReadModel>(data);

            var pagedResult = await _repository.GetAllOrderedAsync<PipelineInboundReadModel>(searchRequest);

            var result = pagedResult.Map<SimplePagedResult<PipelineInboundReadModel>, SimplePagedResult<PipelineViewModel>>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.PipelineInboundActions);

            foreach (var record in pagedResult.Results)
            {
                var model = result.Results.FirstOrDefault(x => x.Id == record.Id);
                if (model == null) continue;
                SetValidationErrors(record, model);
                SetHighlightingType(model);

                model.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return result;
        }

        /// <summary>
        /// Sets errors occured for Inbound Record to its model
        /// </summary>
        /// <param name="record">The Inbound Record</param>
        /// <param name="model">The Inbound Record model</param>
        private void SetValidationErrors(PipelineInboundReadModel record, PipelineViewModel model)
        {
            model.Errors = _recordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(PipelineViewModel model)
        {
            model.HighlightingType = model.Errors.Any()
                ? HighlightingType.Error
                : HighlightingType.NoHighlighting;
        }

        /// <summary>
        /// Deletes Inbound record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("delete")]
        [PermissionRequired(Permissions.PipelineDeleteInboundRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new PipelineInboundMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            var validationResult = _recordsValidator.Validate(inboundRecords);
            var result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.PipelineListInboundActions);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }

        /// <summary>
        /// Uploads file for pipeline import records
        /// </summary>
        /// <param name="request">The request</param>
        [Route("documents-upload")]
        [HttpPost]
        [PermissionRequired(Permissions.PipelineFileInboundRecord)]
        public async Task<IHttpActionResult> ProcessFile(HttpRequestMessage request)
        {
            var folder = DirectoryHelpers.EnsureWorkingFolder("FilingPortal/Upload");

            HttpPostedData data = await ParseMultipartRequest(request, folder);

            if (!data.Files.Any())
            {
                return BadRequest(ErrorMessages.NoFilesInRequest);
            }

            if (string.IsNullOrWhiteSpace(data.Fields["docType"]?.Value))
            {
                return BadRequest("No document type provided");
            }

            PipelineDocumentDto dto = Mapper.Map<PipelineDocumentDto>(data);

            var inboundRecords = data.Fields["recordIds"].Value.Split(',').Select(x => Convert.ToInt32(x)).ToArray();

            _filingHeaderDocumentUpdateService.UploadDocumentsToInboundRecords(inboundRecords, new[] { dto });

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

            return Ok();
        }
    }
}