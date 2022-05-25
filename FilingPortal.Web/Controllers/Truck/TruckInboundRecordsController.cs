using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.Controllers.Truck
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using FilingPortal.Domain.Commands;
    using FilingPortal.Domain.Common;
    using FilingPortal.Domain.Entities.Truck;
    using FilingPortal.Domain.Enums;
    using FilingPortal.Domain.Mapping;
    using FilingPortal.Domain.Repositories.Truck;
    using FilingPortal.Domain.Validators;
    using FilingPortal.Web.Models.Truck;
    using FilingPortal.Web.PageConfigs.Common;
    using Framework.Domain.Commands;
    using Framework.Domain.Paging;

    /// <summary>
    /// Controller provides data for Truck Inbound Records
    /// </summary>
    [RoutePrefix("api/inbound/truck")]
    public class TruckInboundRecordsController : ApiControllerBase
    {
        /// <summary>
        /// The repository of Truck Inbound records
        /// </summary>
        private readonly ITruckInboundReadModelRepository _repository;

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
        private readonly IListInboundValidator<TruckInboundReadModel> _selectedInboundRecordValidator;


        /// <summary>
        /// The single inbound record validator
        /// </summary>
        private readonly ISingleRecordValidator<TruckInboundReadModel> _singleInboundRecordValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundRecordsController"/> class.
        /// </summary>
        /// <param name="repository">Trucktruck Inbound records repository</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedInboundRecordValidator">The validator for selected inbound record set </param>
        /// <param name="singleInboundRecordValidator">The single inbound record validator</param>
        public TruckInboundRecordsController(
            ITruckInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<TruckInboundReadModel> selectedInboundRecordValidator,
            ISingleRecordValidator<TruckInboundReadModel> singleInboundRecordValidator)
        {
            _repository = repository;
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
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<TruckInboundReadModel>(data);
            return await _repository.GetTotalMatchesAsync<TruckInboundReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public async Task<SimplePagedResult<TruckInboundViewModel>> Search([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<TruckInboundReadModel>(data);

            var pagedResult = await _repository.GetAllOrderedAsync<TruckInboundReadModel>(searchRequest);

            var result = pagedResult.Map<SimplePagedResult<TruckInboundReadModel>, SimplePagedResult<TruckInboundViewModel>>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.TruckInboundActions);

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
        private void SetValidationErrors(TruckInboundReadModel record, TruckInboundViewModel model)
        {
            model.Errors = _singleInboundRecordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(TruckInboundViewModel model)
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
        [PermissionRequired(Permissions.TruckDeleteInboundRecord)]
        public IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new TruckInboundMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public Actions GetAvailableActions([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.TruckListInboundActions);

            return actionsConfigurator.GetActions(inboundRecords, CurrentUser);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).ToList();

            var validationResult = _selectedInboundRecordValidator.Validate(inboundRecords);
            var result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.TruckListInboundActions);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }
    }
}
