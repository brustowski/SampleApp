using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Rail Inbound Records 
    /// </summary>
    [RoutePrefix("api/inbound/rail")]
    public class InboundRecordsController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Rail Inbound records
        /// </summary>
        private readonly IRailInboundReadModelRepository _railInboundReadModelRepository;

        /// <summary>
        /// Rail imports records repository
        /// </summary>
        private readonly IBdParsedRepository _bdParsedRepository;

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
        private readonly IRailImportRecordsFilingValidator _selectedInboundRecordValidator;

        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<RailInboundReadModel> _singleInboundRecordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsController" /> class
        /// </summary>
        /// <param name="railInboundReadModelRepository">Rail Inbound records repository</param>
        /// <param name="bdParsedRepository">Rail imports records repository</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedInboundRecordValidator">The validator for selected inbound record set </param>
        /// <param name="singleInboundRecordValidator">The single inbound record validator</param>
        public InboundRecordsController(
            IRailInboundReadModelRepository railInboundReadModelRepository,
            IBdParsedRepository bdParsedRepository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IRailImportRecordsFilingValidator selectedInboundRecordValidator,
            ISingleRecordValidator<RailInboundReadModel> singleInboundRecordValidator)
        {
            _railInboundReadModelRepository = railInboundReadModelRepository;
            _bdParsedRepository = bdParsedRepository;
            _searchRequestFactory = searchRequestFactory;
            _pageConfigContainer = pageConfigContainer;
            _selectedInboundRecordValidator = selectedInboundRecordValidator;
            _singleInboundRecordValidator = singleInboundRecordValidator;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            EnsureStatusFieldFilterSettings(data);
            var searchRequest = _searchRequestFactory.Create<RailInboundReadModel>(data);
            return await _railInboundReadModelRepository.GetTotalMatchesAsync<RailInboundReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public async Task<SimplePagedResult<InboundRecordItemViewModel>> Search([FromBody]SearchRequestModel data)
        {
            EnsureStatusFieldFilterSettings(data);
            var searchRequest = _searchRequestFactory.Create<RailInboundReadModel>(data);

            var pagedResult = await _railInboundReadModelRepository.GetAllOrderedAsync<RailInboundReadModel>(searchRequest);

            var result = pagedResult.Map<SimplePagedResult<RailInboundReadModel>, SimplePagedResult<InboundRecordItemViewModel>>();

            //TODO: make generic GetPageConfig<RailInboundReadModel> and get configuration by type
            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.SingleInboundRecordConfigName);

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
        /// Ensure Status field filter settings of the <see cref="SearchRequestModel"/>
        /// </summary>
        /// <param name="model">Search Request</param>
        private void EnsureStatusFieldFilterSettings(SearchRequestModel model)
        {
            var propertyName = nameof(RailInboundReadModel.Status);
            if (!model.FilterSettings.Filters.Any(x => x.FieldName.Equals(propertyName)))
            {
                var filter = FilterBuilder.CreateFor<RailInboundReadModel>(x => x.Status)
                    .Operand(FilterOperands.Custom)
                    .AddValue(RailInboundRecordStatus.Open)
                    .Build();
                model.FilterSettings.Filters.Add(filter);
            }
        }

        /// <summary>
        /// Sets errors occured for Inbound Record to its model
        /// </summary>
        /// <param name="record">The Inbound Record</param>
        /// <param name="model">The Inbound Record model</param>
        private void SetValidationErrors(RailInboundReadModel record, InboundRecordItemViewModel model)
        {
            model.Errors = _singleInboundRecordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(InboundRecordItemViewModel model)
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
        [PermissionRequired(Permissions.RailDeleteInboundRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new RailInboundMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Restores Inbound records by the specified record identifiers
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("restore")]
        [PermissionRequired(Permissions.RailDeleteInboundRecord)]
        public IHttpActionResult Restore([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new RailInboundMassRestoreCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _railInboundReadModelRepository.GetList(ids).ToList();

            var validationResult = _selectedInboundRecordValidator.Validate(inboundRecords);
            var result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.InboundRecordListConfigName);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }

        /// <summary>
        /// Validates the filtered records
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        [HttpPost]
        [Route("validate-filtered-records")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public InboundRecordValidationViewModel ValidateFilteredRecords([FromBody]FiltersSet filtersSet)
        {
            var propertyName = nameof(RailInboundReadModel.Status);
            var idx = filtersSet.Filters.FindIndex(x => x.FieldName.Equals(propertyName));
            if (idx != -1)
            {
                filtersSet.Filters.RemoveAt(idx);
            }
            Filter filter = FilterBuilder.CreateFor<RailInboundReadModel>(x => x.Status)
                    .Operand(FilterOperands.Custom)
                    .AddValue(RailInboundRecordStatus.Open)
                    .Build();
            filtersSet.Filters.Add(filter);

            var validationResult = _selectedInboundRecordValidator.Validate(filtersSet);

            var result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.FilteredRailRecordsActionsConfig);
            result.Actions = actionsConfigurator.GetActions(validationResult, CurrentUser);

            return result;
        }

        /// <summary>
        /// Executes add or edit rail inbound command
        /// </summary>
        /// <param name="model">Rail inbound edit model</param>
        [HttpPost]
        [Route("save-inbound")]
        [PermissionRequired(Permissions.RailFileInboundRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> AddOrEditRecord(RailInboundEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel<int?> { Data = null };

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                RailBdParsed record = model.Map<RailInboundEditModel, RailBdParsed>();
                record.CreatedUser = CurrentUser.Id;

                CommandResult result = _commandDispatcher.Dispatch(new RailImportAddOrUpdateCommand { Record = record, RecordId = model.Id });

                if (result == null)
                {
                    validationResult.SetCommonError("Error occured while saving rail record");
                }
                else
                {
                    if (result is CommandResult<int> intResult)
                    {
                        validationResult.Data = intResult.Value;
                    }
                    else
                        validationResult.SetCommonError(string.Join("; ",
                            result.Errors.Select(x => x.ErrorMessage).ToArray()));
                }
            }
            else
            {
                validationResult.SetCommonError("Error occured while saving rail record");
            }

            return validationResult;
        }

        /// <summary>
        /// Returns model for editing
        /// </summary>
        /// <param name="recordId">Record id</param>
        [HttpGet]
        [Route("{recordId:int}")]
        [PermissionRequired(Permissions.RailViewManifest)]
        public RailInboundEditModel GetEditModel(int recordId) => _bdParsedRepository.Get(recordId).Map<RailBdParsed, RailInboundEditModel>();
    }
}
