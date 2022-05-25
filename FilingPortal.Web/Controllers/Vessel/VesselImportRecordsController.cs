using FilingPortal.Domain.Commands;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.VesselImport;
using FilingPortal.Domain.Validators;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.Controllers.Vessel
{
    /// <summary>
    /// Controller provides data for Vessel Inbound Records
    /// </summary>
    [RoutePrefix("api/inbound/vessel")]
    public class VesselImportRecordsController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Vessel Inbound records read models
        /// </summary>
        private readonly IVesselImportReadModelRepository _repository;

        /// <summary>
        /// The repository of Vessel records
        /// </summary>
        private readonly IVesselImportRepository _importRepository;

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
        private readonly IListInboundValidator<VesselImportReadModel> _selectedInboundRecordValidator;

        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<VesselImportReadModel> _singleInboundRecordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportRecordsController"/> class.
        /// </summary>
        /// <param name="repository">Vessel Grid records repository</param>
        /// <param name="importRepository">Inbound records repository</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedInboundRecordValidator">The validator for selected inbound record set </param>
        /// <param name="singleInboundRecordValidator">The single inbound record validator</param>
        public VesselImportRecordsController(
            IVesselImportReadModelRepository repository,
            IVesselImportRepository importRepository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<VesselImportReadModel> selectedInboundRecordValidator,
            ISingleRecordValidator<VesselImportReadModel> singleInboundRecordValidator)
        {
            _repository = repository;
            _importRepository = importRepository;
            _searchRequestFactory = searchRequestFactory;
            _commandDispatcher = commandDispatcher;
            _pageConfigContainer = pageConfigContainer;
            _selectedInboundRecordValidator = selectedInboundRecordValidator;
            _singleInboundRecordValidator = singleInboundRecordValidator;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<VesselImportReadModel>(data);
            return await _repository.GetTotalMatchesAsync<VesselImportReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public async Task<SimplePagedResult<VesselImportViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<VesselImportReadModel>(data);

            SimplePagedResult<VesselImportReadModel> pagedResult = await _repository.GetAllOrderedAsync<VesselImportReadModel>(searchRequest);

            SimplePagedResult<VesselImportViewModel> result = pagedResult.Map<SimplePagedResult<VesselImportReadModel>, SimplePagedResult<VesselImportViewModel>>();

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselImportActions);

            foreach (VesselImportReadModel record in pagedResult.Results)
            {
                VesselImportViewModel model = result.Results.FirstOrDefault(x => x.Id == record.Id);
                if (model == null)
                {
                    continue;
                }

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
        private void SetValidationErrors(VesselImportReadModel record, VesselImportViewModel model) => model.Errors = _singleInboundRecordValidator.GetErrors(record) ?? new List<string>();

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(VesselImportViewModel model)
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
        [PermissionRequired(Permissions.VesselDeleteImportRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            CommandResult result = _commandDispatcher.Dispatch(new VesselImportMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public Actions GetAvailableActions([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselListImportActions);

            return actionsConfigurator.GetActions(inboundRecords, CurrentUser);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            InboundRecordValidationResult validationResult = _selectedInboundRecordValidator.Validate(inboundRecords);
            InboundRecordValidationViewModel result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselListImportActions);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }

        /// <summary>
        /// Adds new vessel to repository
        /// </summary>
        /// <param name="model">Viewmodel</param>
        [HttpPost]
        [Route("save-inbound")]
        [PermissionRequired(Permissions.VesselAddImportRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> AddNewVessel(VesselImportEditModel model) => AddOrEditVessel(model);

        /// <summary>
        /// Adds new vessel to repository
        /// </summary>
        /// <param name="model">Viewmodel</param>
        /// <param name="recordId">Id of record to edit</param>
        [HttpPost]
        [Route("save-inbound/{recordId:int}")]
        [PermissionRequired(Permissions.VesselAddImportRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> EditInboundVessel([FromBody] VesselImportEditModel model, [FromUri] int recordId) => AddOrEditVessel(model, recordId);

        /// <summary>
        /// Executes add or edit vessel command
        /// </summary>
        /// <param name="model">Vessel import view model</param>
        /// <param name="recordId">Existing record id</param>
        private ValidationResultWithFieldsErrorsViewModel<int?> AddOrEditVessel(VesselImportEditModel model, int? recordId = null)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel<int?> { Data = null };

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                VesselImportRecord record = model.Map<VesselImportEditModel, VesselImportRecord>();
                record.CreatedUser = CurrentUser.Id;

                CommandResult result = _commandDispatcher.Dispatch(new VesselImportAddOrUpdateCommand { Record = record, RecordId = recordId });

                if (result is CommandResult<int> intResult)
                {
                    validationResult.Data = intResult.Value;
                }
                else validationResult.SetCommonError(string.Join("; ", result.Errors.Select(x=>x.ErrorMessage).ToArray()));
            }
            else
            {
                validationResult.SetCommonError("Error occured while saving vessel record");
            }

            return validationResult;
        }

        /// <summary>
        /// Returns model for editing
        /// </summary>
        /// <param name="recordId">Record id</param>
        [HttpGet]
        [Route("{recordId:int}")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public VesselImportEditModel GetVessel(int recordId) => _importRepository.Get(recordId).Map<VesselImportRecord, VesselImportEditModel>();
    }
}
