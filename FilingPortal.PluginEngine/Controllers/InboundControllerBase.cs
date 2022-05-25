using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Controller for grid search operations
    /// </summary>
    public abstract class InboundControllerBase<TReadModel, TViewModel> : ApiControllerBase
    where TReadModel : Entity
    where TViewModel : ViewModelWithActions, IModelWithStringValidation
    {
        /// <summary>
        /// The repository of inbound records read models
        /// </summary>
        protected readonly ISearchRepository<TReadModel> Repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        protected readonly ISearchRequestFactory SearchRequestFactory;

        /// <summary>
        /// The page configuration container
        /// </summary>
        protected readonly IPageConfigContainer PageConfigContainer;
        /// <summary>
        /// The validator for selected inbound record set 
        /// </summary>
        protected readonly IListInboundValidator<TReadModel> SelectedRecordValidator;


        /// <summary>
        /// The single inbound record validator
        /// </summary>
        protected readonly ISingleRecordValidator<TReadModel> SingleRecordValidator;

        /// <summary>
        /// Specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundControllerBase{TReadModel,TViewModel}"/> class.
        /// </summary>
        /// <param name="repository">The repository of read model records</param>
        /// <param name="searchRequestFactory">The search request factory</param>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="selectedRecordValidator">The validator for selected record set </param>
        /// <param name="singleRecordValidator">The single record validator</param>
        /// <param name="specificationBuilder">Specification builder</param>
        protected InboundControllerBase(
            ISearchRepository<TReadModel> repository,
            ISearchRequestFactory searchRequestFactory,
            IPageConfigContainer pageConfigContainer,
            IListInboundValidator<TReadModel> selectedRecordValidator,
            ISingleRecordValidator<TReadModel> singleRecordValidator,
            ISpecificationBuilder specificationBuilder
        )
        {
            Repository = repository;
            SearchRequestFactory = searchRequestFactory;
            PageConfigContainer = pageConfigContainer;
            SelectedRecordValidator = selectedRecordValidator;
            SingleRecordValidator = singleRecordValidator;
            _specificationBuilder = specificationBuilder;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        public virtual async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<TReadModel>(data);
            return await Repository.GetTotalMatchesAsync<TReadModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        public virtual async Task<SimplePagedResult<TViewModel>> Search(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<TReadModel>(data);

            SimplePagedResult<TReadModel> pagedResult = await Repository.GetAllOrderedAsync<TReadModel>(searchRequest);

            SimplePagedResult<TViewModel> result = pagedResult.Map<SimplePagedResult<TReadModel>, SimplePagedResult<TViewModel>>();

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(RecordActionConfigName);

            foreach (TReadModel record in pagedResult.Results)
            {
                TViewModel model = result.Results.FirstOrDefault(x => x.Id == record.Id);
                if (model == null) continue;

                SetValidationErrors(record, model);
                SetHighlightingType(model);
                OnModelResolve(record, model);

                model.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }
            return result;
        }

        /// <summary>
        /// Additional model updates
        /// </summary>
        /// <param name="record">The record</param>
        /// <param name="model">The view model</param>
        protected virtual void OnModelResolve(TReadModel record, TViewModel model)
        {

        }

        /// <summary>
        /// Sets errors occured for record to its model
        /// </summary>
        /// <param name="record">The record</param>
        /// <param name="model">The view model</param>
        protected void SetValidationErrors(TReadModel record, TViewModel model)
        {
            model.Errors = SingleRecordValidator.GetErrors(record) ?? new List<string>();
        }

        /// <summary>
        /// Deletes Vessel Export record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>

        public virtual IHttpActionResult Delete([FromBody] int[] recordIds)
        {
            return BadRequest("Delete operation is not allowed");
        }

        /// <summary>
        /// Validates inbound records with specified ids
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// </summary>
        public virtual IHttpActionResult Validate([FromBody]FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<TReadModel>(filtersSet);
            IEnumerable<int> ids = Repository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);

            Validate(ids.ToArray());

            return Ok();
        }

        /// <summary>
        /// Validates inbound records with specified ids
        /// </summary>
        /// <param name="ids">The inbound record ids</param>
        public virtual IHttpActionResult Validate([FromBody] int[] ids)
        {
            return BadRequest("Validation not implemented");
        }

        /// <summary>
        /// Page actions config name
        /// </summary>
        public abstract string PageActionsConfig { get; }
        /// <summary>
        /// Page config name
        /// </summary>
        public abstract string RecordsListConfigName { get; }
        /// <summary>
        /// Single record config name
        /// </summary>
        public abstract string RecordActionConfigName { get; }

        /// <summary>
        /// Gets available actions for selected items
        /// </summary>
        /// <param name="ids">The ids</param>
        public virtual Actions GetAvailableActions([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = Repository.GetList(ids).ToList();

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(RecordsListConfigName);

            return actionsConfigurator.GetActions(inboundRecords, CurrentUser);
        }

        /// <summary>
        /// Validates the selected records
        /// </summary>
        /// <param name="ids">The ids</param>
        public virtual InboundRecordValidationViewModel ValidateSelectedRecords([FromBody] IEnumerable<int> ids)
        {
            var inboundRecords = Repository.GetList(ids).ToList();

            InboundRecordValidationResult validationResult = SelectedRecordValidator.Validate(inboundRecords);
            InboundRecordValidationViewModel result = validationResult.Map<InboundRecordValidationResult, InboundRecordValidationViewModel>();

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(RecordsListConfigName);

            result.Actions = actionsConfigurator.GetActions(inboundRecords, CurrentUser);

            return result;
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        protected void SetHighlightingType(TViewModel model)
        {
            model.HighlightingType = model.Errors.Any()
                ? HighlightingType.Error
                : HighlightingType.NoHighlighting;
        }
    }
}