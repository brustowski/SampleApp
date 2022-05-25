using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.CanadaTruckImport.Domain.Commands;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Web.Configs;
using FilingPortal.Parts.CanadaTruckImport.Web.Models;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Commands;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Controllers
{
    /// <summary>
    /// Controller provides data for inbound Records
    /// </summary>
    [RoutePrefix("api/canada-imp-truck")]
    public class CanadaTruckImportInboundRecordsController : InboundControllerBase<InboundReadModel, InboundRecordViewModel>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanadaTruckImportInboundRecordsController"/> class.
        /// </summary>
        /// <param name="repository">Inbound records read model repository</param>
        /// <param name="searchRequestFactory">The repository of zones records read models</param>
        /// <param name="commandDispatcher">The search request factory</param>
        /// <param name="pageConfigContainer">The command dispatcher</param>
        /// <param name="selectedRecordValidator">The page configuration container</param>
        /// <param name="singleRecordValidator">The single record validator</param>
        public CanadaTruckImportInboundRecordsController(
            IInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<InboundReadModel> selectedRecordValidator,
            ISingleRecordValidator<InboundReadModel> singleRecordValidator
            ) : base(repository, searchRequestFactory, pageConfigContainer, selectedRecordValidator, singleRecordValidator, null)
        {
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
