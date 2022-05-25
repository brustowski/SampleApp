using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Inbond.Domain.Commands;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Enums;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Commands;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Inbond.Web.Controllers
{
    /// <summary>
    /// Controller provides data for Inbond Import Records
    /// </summary>
    [RoutePrefix("api/zones/in-bond")]
    public class InbondController : InboundControllerBase<InboundReadModel, InbondViewModel>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="InbondController"/> class.
        /// </summary>
        /// <param name="repository">The repository of zones records read models</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedRecordValidator">The validator for selected record set </param>
        /// <param name="singleRecordValidator">The single record validator</param>
        /// <param name="specificationBuilder">Specification builder</param>
        public InbondController(
            IInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<InboundReadModel> selectedRecordValidator,
            ISingleRecordValidator<InboundReadModel> singleRecordValidator,
            ISpecificationBuilder specificationBuilder
            ) : base(repository, searchRequestFactory, pageConfigContainer, selectedRecordValidator, singleRecordValidator, specificationBuilder)
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
        public override async Task<SimplePagedResult<InbondViewModel>> Search([FromBody]SearchRequestModel data) => await base.Search(data);

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
            var result = _commandDispatcher.Dispatch(new InbondMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Page actions config name
        /// </summary>
        public override string PageActionsConfig => PageConfigNames.InbondViewPageActions;

        /// <summary>
        /// Page config name
        /// </summary>
        public override string RecordsListConfigName => PageConfigNames.InbondListActions;

        /// <summary>
        /// Single record config name
        /// </summary>
        public override string RecordActionConfigName => PageConfigNames.InbondActions;
    }
}
