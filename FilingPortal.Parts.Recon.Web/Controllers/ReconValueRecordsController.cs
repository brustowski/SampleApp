using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Recon.Domain.Commands;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Domain.Repositories;
using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FluentValidation;
using FluentValidation.Results;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FilingPortal.Parts.Recon.Web.Controllers
{
    /// <summary>
    /// Controller provides data for Value Recon Records
    /// </summary>
    [RoutePrefix("api/recon/value")]
    public class ReconValueRecordsController : ApiControllerBase
    {
        /// <summary>
        /// The repository of inbound records read models
        /// </summary>
        private readonly IValueReconReadModelRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The page configuration container
        /// </summary>
        private readonly IPageConfigContainer _pageConfigContainer;

        /// <summary>
        /// The value recon model validator
        /// </summary>
        private readonly IValidator<ValueReconViewModel> _validator;

        /// <summary>
        /// The command dispatcher
        /// </summary>
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconValueRecordsController"/> class.
        /// </summary>
        /// <param name="repository">The repository of inbound records read models</param>
        /// <param name="searchRequestFactory">The search requests factory</param>
        /// <param name="pageConfigContainer">The command dispatcher</param>
        /// <param name="commandDispatcher">The command dispatcher</param>
        /// <param name="validator">The value recon model validator</param>
        public ReconValueRecordsController(
            IValueReconReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            IPageConfigContainer pageConfigContainer,
            ICommandDispatcher commandDispatcher,
            IValidator<ValueReconViewModel> validator)
        {
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
            _pageConfigContainer = pageConfigContainer;
            _commandDispatcher = commandDispatcher;
            _validator = validator;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewValueRecord)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<ValueReconReadModel>(data);
            return await _repository.GetTotalMatchesAsync<ValueReconReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewValueRecord)]
        public async Task<SimplePagedResult<ValueReconViewModel>> Search([FromBody] SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<ValueReconReadModel>(data);

            SimplePagedResult<ValueReconViewModel> pagedResult = await _repository.GetAllOrderedAsync<ValueReconViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(RecordActionConfigName);

            foreach (ValueReconViewModel record in pagedResult.Results)
            {
                SetValidationErrors(record);

                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }
            return pagedResult;
        }

        /// <summary>
        /// Sets errors occured for record to its model
        /// </summary>
        /// <param name="model">The view model</param>
        private void SetValidationErrors(ValueReconViewModel model)
        {
            ValidationResult result = _validator.Validate(model);
            model.Errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            model.HighlightingType = result.IsValid ? HighlightingType.NoHighlighting
                : result.Errors.Any(x => x.Severity == Severity.Error)
                    ? HighlightingType.Error : HighlightingType.Warning;
        }

        /// <summary>
        /// Gets available actions for selected items
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("available-actions")]
        [PermissionRequired(Permissions.ViewValueRecord)]
        public Actions GetAvailableActions([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = _repository.GetList(ids).Map<ValueReconReadModel, ValueReconViewModel>().ToList();

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(RecordsListConfigName);

            return actionsConfigurator.GetActions(inboundRecords, CurrentUser);
        }

        /// <summary>
        /// Page actions config name
        /// </summary>
        public string PageActionsConfig => PageConfigNames.ValueViewPageActions;

        /// <summary>
        /// Page config name
        /// </summary>
        public string RecordsListConfigName => PageConfigNames.ValueListActions;

        /// <summary>
        /// Single record config name
        /// </summary>
        public string RecordActionConfigName => PageConfigNames.ValueActions;

        /// <summary>
        /// Gets available actions for selected items
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("process")]
        [PermissionRequired(Permissions.ProcessValueRecord)]
        public IHttpActionResult Process([FromBody] IEnumerable<int> ids)
        {
            CommandResult result = _commandDispatcher.Dispatch(new ProcessValueCommand { Ids = ids.ToArray() });
            return result is CommandResult<int> typedResult ? Result(typedResult) : Result(result);
        }

        /// <summary>
        /// Checking records for exportability
        /// </summary>
        /// <param name="data">The search request</param>
        [HttpPost]
        [Route("exportability-check")]
        [PermissionRequired(Permissions.ProcessFtaRecord)]
        public IHttpActionResult ExportabilityCheck([FromBody] SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<ValueReconReadModel>(data);

            var result = _repository.CheckFor<ValueReconReadModel>(x => x.Status == (int)ValueReconStatusValue.Processed, searchRequest);

            return Result(new CommandResult<string>(result ? "" : "Some records needs processing."));
        }
    }
}
