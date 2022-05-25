using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Zones.Entry.Domain.Commands;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Enums;
using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using FilingPortal.Parts.Zones.Entry.Web.Configs;
using FilingPortal.Parts.Zones.Entry.Web.Models;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Commands;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Zones.Entry.Web.Controllers
{
    /// <summary>
    /// Controller provides data for inbound Records
    /// </summary>
    [RoutePrefix("api/zones/entry-06")]
    public class ZonesEntryInboundRecordsController : InboundControllerBase<InboundReadModel, InboundRecordViewModel>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IClientRepository _clientRepository;
        private readonly IInboundRecordsRepository _inboundRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesEntryInboundRecordsController"/> class.
        /// </summary>
        /// <param name="repository">Inbound records read model repository</param>
        /// <param name="searchRequestFactory">The repository of zones records read models</param>
        /// <param name="commandDispatcher">The search request factory</param>
        /// <param name="pageConfigContainer">The command dispatcher</param>
        /// <param name="selectedRecordValidator">The page configuration container</param>
        /// <param name="singleRecordValidator">The single record validator</param>
        /// <param name="specificationBuilder">Specification builder</param>
        /// <param name="clientRepository">Clients repository</param>
        /// <param name="inboundRepository">Inbound records repository</param>
        public ZonesEntryInboundRecordsController(
            IInboundReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            ICommandDispatcher commandDispatcher,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<InboundReadModel> selectedRecordValidator,
            ISingleRecordValidator<InboundReadModel> singleRecordValidator,
            ISpecificationBuilder specificationBuilder,
            IClientRepository clientRepository,
            IInboundRecordsRepository inboundRepository
            ) : base(repository, searchRequestFactory, pageConfigContainer, selectedRecordValidator, singleRecordValidator, specificationBuilder)
        {
            _commandDispatcher = commandDispatcher;
            _clientRepository = clientRepository;
            _inboundRepository = inboundRepository;
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
        public override async Task<SimplePagedResult<InboundRecordViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<InboundReadModel>(data);
            SimplePagedResult<InboundReadModel> pagedResult = await Repository.GetAllOrderedAsync<InboundReadModel>(searchRequest);

            IEnumerable<InboundRecordViewModel> viewModels = pagedResult.Results.Map<InboundReadModel, InboundRecordViewModel>();

            var result = viewModels.ToList();
            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(PageConfigNames.InboundActions);

            foreach (InboundReadModel record in pagedResult.Results)
            {
                InboundRecordViewModel model = result.FirstOrDefault(x => x.Id == record.Id);
                if (model == null)
                {
                    continue;
                }

                SetValidationErrors(record, model);
                SetHighlightingType(model);
                SetAvailableImporters(model);

                model.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            var resultWithUpdatedInfo = new SimplePagedResult<InboundRecordViewModel>()
            {
                CurrentPage = pagedResult.CurrentPage,
                PageSize = pagedResult.PageSize,
                Results = result,
            };
            return resultWithUpdatedInfo;
        }

        private void SetAvailableImporters(InboundRecordViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Ein) && model.Importer == null)
            {
                model.AvailableImporters = _clientRepository.GetByRegNumber(model.Ein).Select(x => new LookupItem<Guid>()
                {
                    Value = x.Id,
                    DisplayValue = $"{x.ClientCode} - {x.ClientName}"
                }).ToList();
            }
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        [HttpPost]
        [Route("validate-selected-records")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = Repository.GetList(ids).ToList();

            InboundRecordValidationResult validationResult = SelectedRecordValidator.Validate(inboundRecords);
            InboundRecordValidationViewModel result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            return result;
        }

        /// <summary>
        /// Deletes zone record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        [HttpPost]
        [Route("delete")]
        [PermissionRequired(Permissions.DeleteInboundRecord)]
        public override IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            CommandResult result = _commandDispatcher.Dispatch(new InboundMassDeleteCommand { RecordIds = recordIds });
            return Result(result);
        }

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        [HttpPost]
        [Route("validate/filtered")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override IHttpActionResult Validate([FromBody] FiltersSet filtersSet) => base.Validate(filtersSet);

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        [HttpPost]
        [Route("validate")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public override IHttpActionResult Validate(int[] ids)
        {
            CommandResult result = _commandDispatcher.Dispatch(new InboundValidationCommand { RecordIds = ids });

            return Result(result);
        }

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="id">The inbound record id</param>
        /// <param name="importerChangeModel">Importer Change model</param>
        [HttpPost]
        [Route("set-importer/{id}")]
        [PermissionRequired(Permissions.ImportInboundRecord)]
        public IHttpActionResult SetImporter(int id, [FromBody] ImporterChangeModel importerChangeModel)
        {
            InboundRecord inboundRecord = _inboundRepository.Get(id);
            IList<Client> clients = _clientRepository.GetByRegNumber(inboundRecord.Ein);
            if (!inboundRecord.ImporterId.HasValue && clients.Any(x => x.Id == importerChangeModel.ClientId))
            {
                inboundRecord.ImporterId = importerChangeModel.ClientId;
                _inboundRepository.Update(inboundRecord);
                _inboundRepository.Save();
            }

            return Ok();
        }

        /// <summary>
        /// Page actions config name
        /// </summary>
        public override string PageActionsConfig => PageConfigNames.InboundViewPageActions;

        /// <summary>
        /// Page config name
        /// </summary>
        public override string RecordsListConfigName => null;

        /// <summary>
        /// Single record config name
        /// </summary>
        public override string RecordActionConfigName => PageConfigNames.InboundActions;
    }
}
