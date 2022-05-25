using AutoMapper;
using FilingPortal.Domain;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.TruckExport;
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
using FilingPortal.Web.Models.TruckExport;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Services.TruckExport;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller provides data for Truck Export Records
    /// </summary>
    [RoutePrefix("api/export/truck")]
    public class TruckExportController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Truck Export records
        /// </summary>
        private readonly ITruckExportReadModelRepository _repository;

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
        private readonly IListInboundValidator<TruckExportReadModel> _selectedRecordValidator;


        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<TruckExportReadModel> _singleRecordValidator;

        /// <summary>
        /// Files upload service
        /// </summary>
        private readonly IFilingHeaderDocumentUpdateService<TruckExportDocumentDto> _filingHeaderDocumentUpdateService;

        /// <summary>
        /// The search specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Truck Export repository
        /// </summary>
        private readonly ITruckExportRepository _truckExportRepository;

        /// <summary>
        /// The Job number service
        /// </summary>
        private readonly ITruckExportJobNumberService _jobNumberService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportController"/> class.
        /// </summary>
        /// <param name="repository">Truck export records repository</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedRecordValidator">The validator for selected record set </param>
        /// <param name="singleRecordValidator">The single record validator</param>
        /// <param name="filingHeaderDocumentUpdateService">Files upload service</param>
        /// <param name="specificationBuilder">The search specification builder</param>
        /// <param name="truckExportRepository">Truck Export repository</param>
        /// <param name="jobNumberService">The job number service</param>
        public TruckExportController(
            ITruckExportReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<TruckExportReadModel> selectedRecordValidator,
            ISingleRecordValidator<TruckExportReadModel> singleRecordValidator,
            IFilingHeaderDocumentUpdateService<TruckExportDocumentDto> filingHeaderDocumentUpdateService,
            ISpecificationBuilder specificationBuilder,
            ITruckExportRepository truckExportRepository, 
            ITruckExportJobNumberService jobNumberService)
        {
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
            _commandDispatcher = commandDispatcher;
            _pageConfigContainer = pageConfigContainer;
            _selectedRecordValidator = selectedRecordValidator;
            _singleRecordValidator = singleRecordValidator;
            _filingHeaderDocumentUpdateService = filingHeaderDocumentUpdateService;
            _specificationBuilder = specificationBuilder;
            _truckExportRepository = truckExportRepository;
            _jobNumberService = jobNumberService;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<TruckExportReadModel>(data);
            return await _repository.GetTotalMatchesAsync<TruckExportReadModel>(searchRequest);
        }



        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public async Task<SimplePagedResult<TruckExportViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<TruckExportReadModel>(data);
            SimplePagedResult<TruckExportReadModel> pagedResult = await _repository.GetAllOrderedAsync<TruckExportReadModel>(searchRequest);

            IEnumerable<TruckExportViewModel> viewModels = pagedResult.Results.Map<TruckExportReadModel, TruckExportViewModel>();

            var result = viewModels.ToList();
            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.TruckExportActions);

            foreach (TruckExportReadModel record in pagedResult.Results)
            {
                TruckExportViewModel model = result.FirstOrDefault(x => x.Id == record.Id);
                if (model == null)
                {
                    continue;
                }

                SetValidationErrors(record, model);
                SetHighlightingType(model);

                model.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            var resultWithUpdatedInfo = new SimplePagedResult<TruckExportViewModel>()
            {
                CurrentPage = pagedResult.CurrentPage,
                PageSize = pagedResult.PageSize,
                Results = result,
            };
            return resultWithUpdatedInfo;
        }

        /// <summary>
        /// Sets errors occured for Truck Export Record to its model
        /// </summary>
        /// <param name="record">The Export Record</param>
        /// <param name="model">The Export Record model</param>
        private void SetValidationErrors(TruckExportReadModel record, TruckExportViewModel model)
        {
            model.Errors = _singleRecordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Truck Export Record
        /// </summary>
        /// <param name="model">The Truck Export Record item</param>
        private void SetHighlightingType(TruckExportViewModel model)
        {
            model.HighlightingType = model.Errors.Any()
                ? HighlightingType.Error
                : HighlightingType.NoHighlighting;
        }

        /// <summary>
        /// Deletes Truck Export record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("delete")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            CommandResult result = _commandDispatcher.Dispatch(new TruckExportMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            InboundRecordValidationResult validationResult = _selectedRecordValidator.Validate(inboundRecords);
            InboundRecordValidationViewModel result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            return result;
        }

        /// <summary>
        /// Uploads file for truck export records
        /// </summary>
        /// <param name="request">The request</param>
        [Route("documents-upload")]
        [HttpPost]
        [PermissionRequired(Permissions.TruckFileExportRecord)]
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

            TruckExportDocumentDto dto = Mapper.Map<TruckExportDocumentDto>(data);

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

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        [HttpPost]
        [Route("validate/filtered")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public IHttpActionResult Update([FromBody]FiltersSet filtersSet)
        {
            using (new MonitoredScope("Starting Validation process"))
            {
                ISpecification specification = _specificationBuilder.Build<TruckExportReadModel>(filtersSet);
                IEnumerable<int> ids = _repository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);

                _truckExportRepository.Validate(ids);

                return Ok();
            }
        }

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("validate")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public IHttpActionResult Update([FromBody] int[] ids)
        {
            using (new MonitoredScope("Starting Validation process"))
            {
                _truckExportRepository.Validate(ids);

                return Ok();
            }
        }

        /// <summary>
        /// Populate inbound records with specified ids with job numbers
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("job-numbers")]
        [PermissionRequired(Permissions.TruckFileExportRecord)]
        public IHttpActionResult GetJobNumbers([FromBody] int[] ids)
        {
            if (ids.Length == 0)
            {
                return Ok(0);
            }

            var result = _jobNumberService.EnsureJobNumberForRecords(ids, CurrentUser.Id);

            return Ok(result);
        }
    }
}
