using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Isf.Domain.Commands;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Domain.Enums;
using FilingPortal.Parts.Isf.Domain.Repositories;
using FilingPortal.Parts.Isf.Web.Configs;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Commands;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Isf.Web.Controllers
{
    /// <summary>
    /// Controller provides data for inbound Records
    /// </summary>
    [RoutePrefix("api/isf")]
    public class IsfInboundRecordsController : InboundControllerBase<InboundReadModel, InboundRecordViewModel>
    {
        private readonly IInboundRecordsRepository _inboundRepository;
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsfInboundRecordsController"/> class.
        /// </summary>
        /// <param name="inboundRepository">Inbound Records repository</param>
        /// <param name="repository">Inbound records read model repository</param>
        /// <param name="searchRequestFactory">The repository of zones records read models</param>
        /// <param name="commandDispatcher">The search request factory</param>
        /// <param name="pageConfigContainer">The command dispatcher</param>
        /// <param name="selectedRecordValidator">The page configuration container</param>
        /// <param name="singleRecordValidator">The single record validator</param>
        public IsfInboundRecordsController(
            IInboundRecordsRepository inboundRepository,
            IInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<InboundReadModel> selectedRecordValidator,
            ISingleRecordValidator<InboundReadModel> singleRecordValidator
            ) : base(repository, searchRequestFactory, pageConfigContainer, selectedRecordValidator, singleRecordValidator, null)
        {
            _inboundRepository = inboundRepository;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override async Task<int> GetTotalMatches(SearchRequestModel data) => await base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override async Task<SimplePagedResult<InboundRecordViewModel>> Search([FromBody]SearchRequestModel data) => await base.Search(data);

        /// <summary>
        /// Gets available actions for selected items
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override Actions GetAvailableActions([FromBody] IEnumerable<int> ids) => base.GetAvailableActions(ids);

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids) =>
            base.ValidateSelectedRecords(ids);

        /// <summary>
        /// Deletes zone record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("delete")]
        [PermissionRequired(Permissions.DeleteInboundRecord)]
        public override IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            var result = _commandDispatcher.Dispatch(new InboundMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Saves inbound record to repository
        /// </summary>
        /// <param name="model">Viewmodel</param>
        [HttpPost]
        [Route("save-inbound")]
        [PermissionRequired(Permissions.AddInboundRecord)]
        public ValidationResultWithFieldsErrorsViewModel<int?> AddOrEdit(InboundRecordEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel<int?> { Data = null };

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                InboundRecord record = model.Map<InboundRecordEditModel, InboundRecord>();
                record.CreatedUser = CurrentUser.Id;

                CommandResult result = _commandDispatcher.Dispatch(new InboundAddOrEditCommand { Record = record });

                if (result is CommandResult<int> intResult)
                {
                    validationResult.Data = intResult.Value;
                }
                else validationResult.SetCommonError(string.Join("; ", result.Errors.Select(x => x.ErrorMessage).ToArray()));
            }
            else
            {
                validationResult.SetCommonError("Error occured while saving ISF record");
            }

            return validationResult;
        }
        /// <summary>
        /// Returns model for editing
        /// </summary>
        /// <param name="recordId">Record id</param>
        [HttpGet]
        [Route("{recordId:int}")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public InboundRecordEditModel GetInboundRecord(int recordId) => _inboundRepository.Get(recordId).Map<InboundRecord, InboundRecordEditModel>();

        /// <summary>
        /// Page actions config name
        /// </summary>
        public override string PageActionsConfig => PageConfigNames.InboundViewPageActions;

        /// <summary>
        /// Page config name
        /// </summary>
        public override string RecordsListConfigName => PageConfigNames.InboundListActions;

        /// <summary>
        /// Single record config name
        /// </summary>
        public override string RecordActionConfigName => PageConfigNames.InboundActions;
    }
}
