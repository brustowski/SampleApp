using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Description Rail Rules
    /// </summary>
    [RoutePrefix("api/rules/rail/description")]
    public class RailRuleDescriptionController : RuleControllerBase<RailRuleDescription, RailRuleDescriptionViewModel, RailRuleDescriptionEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.RailRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleDescriptionController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Description Rail Rule repository</param>
        /// <param name="ruleService">Rail Description Rule service</param>
        /// <param name="ruleValidator">The Rail Description rule validator</param>
        public RailRuleDescriptionController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<RailRuleDescription> repository,
            IRuleService<RailRuleDescription> ruleService,
            IRuleValidator<RailRuleDescription> ruleValidator
            ) : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.RailViewInboundRecordRules)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.RailViewInboundRecordRules)]
        public override Task<SimplePagedResult<RailRuleDescriptionViewModel>> Search([FromBody]SearchRequestModel data) => base.Search(data);

        /// <summary>
        /// Creates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] RailRuleDescriptionEditModel model) => base.Create(model);

        /// <summary>
        /// Updates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] RailRuleDescriptionEditModel model) => base.Update(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override RailRuleDescriptionViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the Rail Description rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.RailDeleteInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRuleDescriptionEditModel>(x => x.Description1), string.Empty);
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRuleDescriptionEditModel>(x => x.Importer), string.Empty);
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRuleDescriptionEditModel>(x => x.Supplier), string.Empty);
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRuleDescriptionEditModel>(x => x.Port), string.Empty);
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRuleDescriptionEditModel>(x => x.Destination), string.Empty);
        }
    }
}