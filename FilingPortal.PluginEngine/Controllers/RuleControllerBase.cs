using FilingPortal.Domain;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Provides base functionality for Rules controllers
    /// </summary>
    /// <typeparam name="TRuleEntity">Rule entity</typeparam>
    /// <typeparam name="TRuleViewModel">Rule view model</typeparam>
    /// <typeparam name="TRuleEditModel">Rule edit model</typeparam>
    public abstract class RuleControllerBase<TRuleEntity, TRuleViewModel, TRuleEditModel> : ApiControllerBase
        where TRuleEntity : Entity, IRuleEntity, new()
        where TRuleViewModel : ViewModelWithActions, new()
    {
        /// <summary>
        /// The page configuration container
        /// </summary>
        protected readonly IPageConfigContainer PageConfigContainer;

        /// <summary>
        /// The Rule repository
        /// </summary>
        protected readonly IRuleRepository<TRuleEntity> Repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        protected readonly ISearchRequestFactory SearchRequestFactory;

        /// <summary>
        /// The Rule service
        /// </summary>
        private readonly IRuleService<TRuleEntity> _ruleService;

        /// <summary>
        /// The Rule validator
        /// </summary>
        private readonly IRuleValidator<TRuleEntity> _ruleValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleControllerBase{TRuleEntity, TRuleViewModel, TRuleEditModel}"/> class.
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The Rule repository</param>
        /// <param name="ruleService">The Rule Service</param>
        /// <param name="ruleValidator">The Rule validator</param>
        protected RuleControllerBase(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<TRuleEntity> repository,
            IRuleService<TRuleEntity> ruleService,
            IRuleValidator<TRuleEntity> ruleValidator
            )
        {
            PageConfigContainer = pageConfigContainer;
            Repository = repository;
            SearchRequestFactory = searchRequestFactory;
            _ruleValidator = ruleValidator;
            _ruleService = ruleService;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        public virtual async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<TRuleViewModel>(data);
            return await Repository.GetTotalMatchesAsync<TRuleViewModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        public virtual async Task<SimplePagedResult<TRuleViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<TRuleViewModel>(data);

            SimplePagedResult<TRuleViewModel> result = await Repository.GetAllAsync<TRuleViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(PageConfigurationName);

            foreach (TRuleViewModel record in result.Results)
            {
                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return result;
        }

        /// <summary>
        /// Updates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Update([FromBody] TRuleEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            TRuleEntity rule = model.Map<TRuleEditModel, TRuleEntity>();
            rule.CreatedUser = CurrentUser.Id;

            if (_ruleValidator.IsDuplicate(rule))
            {
                validationResult.SetCommonError(ValidationMessages.RuleAlreadyExists);
                AddRuleExistingError(validationResult);
            }

            if (validationResult.IsValid)
            {
                OperationResult result = _ruleService.Update(rule);
                validationResult.SetCommonError(result.Errors);
            }

            return validationResult;
        }

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Create([FromBody] TRuleEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (!validationResult.IsValid)
            {
                return validationResult;
            }

            TRuleEntity rule = model.Map<TRuleEditModel, TRuleEntity>();
            rule.CreatedUser = CurrentUser.Id;

            if (_ruleValidator.IsDuplicate(rule))
            {
                validationResult.SetCommonError(ValidationMessages.RuleAlreadyExists);
                AddRuleExistingError(validationResult);
            }

            if (validationResult.IsValid)
            {
                _ruleService.Create(rule);
            }

            return validationResult;
        }

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        public virtual TRuleViewModel GetNewRow()
        {
            return new TRuleViewModel();
        }

        /// <summary>
        /// Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();
            OperationResult result = _ruleService.Delete(ruleId);
            validationResult.SetCommonError(result.Errors);
            return validationResult;
        }

        /// <summary>
        /// Gets the PageConfigurationName
        /// </summary>
        public abstract string PageConfigurationName { get; }

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        public abstract void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result);
    }
}
