using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Web.Controllers.Vessel
{
    /// <summary>
    /// Controller provides actions for Vessel Rule Port
    /// </summary>
    [RoutePrefix("api/rules/vessel/port")]
    public class VesselRulePortController : RuleControllerBase<VesselRulePort, VesselRulePortViewModel, VesselRulePortEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.VesselRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The Rule repository</param>
        /// <param name="ruleValidator">The Rule validator</param>
        /// <param name="ruleService">The Rule Service</param>
        public VesselRulePortController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<VesselRulePort> repository,
            IRuleService<VesselRulePort> ruleService,
            IRuleValidator<VesselRulePort> ruleValidator
            )
            : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewImportRecordRules)]
        public override async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<VesselRulePortViewModel>(data);
            return await Repository.GetTotalMatchesAsync<VesselRulePortViewModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.VesselViewImportRecordRules)]
        public override async Task<SimplePagedResult<VesselRulePortViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<VesselRulePortViewModel>(data);

            SimplePagedResult<VesselRulePortViewModel> pagedResult = await Repository.GetAllAsync<VesselRulePortViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(PageConfigurationName);

            foreach (VesselRulePortViewModel record in pagedResult.Results)
            {
                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return pagedResult;
        }

        /// <summary>
        /// Updates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.VesselEditImportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] VesselRulePortEditModel model) => base.Update(model);

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.VesselEditImportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] VesselRulePortEditModel model) => base.Create(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.VesselEditImportRecordRules)]
        public override VesselRulePortViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.VesselDeleteImportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<VesselRulePortEditModel>(x => x.FirmsCodeId),
                string.Empty);
        }
    }
}