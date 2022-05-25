using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.Controllers.VesselExport
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using FilingPortal.Domain.Commands;
    using FilingPortal.Domain.Common;
    using FilingPortal.Domain.Entities.VesselExport;
    using FilingPortal.Domain.Enums;
    using FilingPortal.Domain.Mapping;
    using FilingPortal.Domain.Repositories.VesselExport;
    using FilingPortal.Domain.Validators;
    using FilingPortal.Web.Models;
    using FilingPortal.Web.Models.VesselExport;
    using FilingPortal.Web.PageConfigs.Common;
    using Framework.Domain.Commands;
    using Framework.Domain.Paging;

    /// <summary>
    /// Controller provides data for Vessel Export Records
    /// </summary>
    [RoutePrefix("api/export/vessel")]
    public class VesselExportController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Vessel Export records read models
        /// </summary>
        private readonly IVesselExportReadModelRepository _repository;
        /// <summary>
        /// The repository of Vessel Export records
        /// </summary>
        private readonly IVesselExportRepository _exportRepository;

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
        private readonly IListInboundValidator<VesselExportReadModel> _selectedRecordValidator;


        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<VesselExportReadModel> _singleRecordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportController"/> class.
        /// </summary>
        /// <param name="repository">The repository of Vessel Export records read models</param>
        /// <param name="exportRepository">The repository of Vessel Export records</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedRecordValidator">The validator for selected record set </param>
        /// <param name="singleRecordValidator">The single record validator</param>
        public VesselExportController(
            IVesselExportReadModelRepository repository,
            IVesselExportRepository exportRepository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<VesselExportReadModel> selectedRecordValidator,
            ISingleRecordValidator<VesselExportReadModel> singleRecordValidator
            )
        {
            _repository = repository;
            _exportRepository = exportRepository;
            _searchRequestFactory = searchRequestFactory;
            _commandDispatcher = commandDispatcher;
            _pageConfigContainer = pageConfigContainer;
            _selectedRecordValidator = selectedRecordValidator;
            _singleRecordValidator = singleRecordValidator;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<VesselExportReadModel>(data);
            return await _repository.GetTotalMatchesAsync<VesselExportReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public async Task<SimplePagedResult<VesselExportViewModel>> Search([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<VesselExportReadModel>(data);

            var pagedResult = await _repository.GetAllOrderedAsync<VesselExportReadModel>(searchRequest);

            var result = pagedResult.Map<SimplePagedResult<VesselExportReadModel>, SimplePagedResult<VesselExportViewModel>>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselExportActions);

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
        /// Sets errors occured for Vessel Export Record to its model
        /// </summary>
        /// <param name="record">The Export Record</param>
        /// <param name="model">The Export Record model</param>
        private void SetValidationErrors(VesselExportReadModel record, VesselExportViewModel model)
        {
            model.Errors = _singleRecordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Vessel Export Record
        /// </summary>
        /// <param name="model">The Vessel Export Record item</param>
        private void SetHighlightingType(VesselExportViewModel model)
        {
            model.HighlightingType = model.Errors.Any()
                ? HighlightingType.Error
                : HighlightingType.NoHighlighting;
        }

        /// <summary>
        /// Deletes Vessel Export record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("delete")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new VesselExportMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public Actions GetAvailableActions([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselExportListActions);

            return actionsConfigurator.GetActions(inboundRecords, CurrentUser);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            var validationResult = _selectedRecordValidator.Validate(inboundRecords);
            var result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.VesselExportListActions);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }

        /// <summary>
        /// Adds new vessel to repository
        /// </summary>
        /// <param name="model">Viewmodel</param>
        [HttpPost]
        [Route("save-inbound")]
        [PermissionRequired(Permissions.VesselAddExportRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> AddNewVessel(VesselExportEditModel model) => AddOrEditVessel(model);

        /// <summary>
        /// Adds new vessel to repository
        /// </summary>
        /// <param name="model">Viewmodel</param>
        /// <param name="recordId">Id of record to edit</param>
        [HttpPost]
        [Route("save-inbound/{recordId:int}")]
        [PermissionRequired(Permissions.VesselAddExportRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> EditInboundVessel([FromBody] VesselExportEditModel model, [FromUri] int recordId) => AddOrEditVessel(model, recordId);

        /// <summary>
        /// Executes add or edit vessel command
        /// </summary>
        /// <param name="model">Vessel import view model</param>
        /// <param name="recordId">Existing record id</param>
        private ValidationResultWithFieldsErrorsViewModel<int?> AddOrEditVessel(VesselExportEditModel model, int? recordId = null)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel<int?> { Data = null };

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                VesselExportRecord record = model.Map<VesselExportEditModel, VesselExportRecord>();
                record.CreatedUser = CurrentUser.Id;

                CommandResult result = _commandDispatcher.Dispatch(new VesselExportAddOrUpdateCommand { Record = record, RecordId = recordId });

                if (result is CommandResult<int> intResult)
                {
                    validationResult.Data = intResult.Value;
                }
                else validationResult.SetCommonError(string.Join("; ", result.Errors.Select(x => x.ErrorMessage).ToArray()));
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
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public VesselExportEditModel GetVessel(int recordId) => _exportRepository.Get(recordId).Map<VesselExportRecord, VesselExportEditModel>();
    }
}
