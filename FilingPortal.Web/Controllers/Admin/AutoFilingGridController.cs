using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models.Admin;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Admin
{
    /// <summary>
    /// Controller provides actions for Auto-Create management
    /// </summary>
    [RoutePrefix("api/auto-create")]
    public class AutoFilingGridController : RuleControllerBase<AutoCreateRecord, AutoCreateRecordViewModel, AutoCreateRecordEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.AdminRulesPageActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFilingGridController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The Rule repository</param>
        /// <param name="ruleValidator">The Rule validator</param>
        /// <param name="ruleService">The Rule Service</param>
        public AutoFilingGridController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<AutoCreateRecord> repository,
            IRuleService<AutoCreateRecord> ruleService,
            IRuleValidator<AutoCreateRecord> ruleValidator
        )
            : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        {
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override Task<SimplePagedResult<AutoCreateRecordViewModel>> Search([FromBody]SearchRequestModel data) => base.Search(data);

        /// <summary>
        /// Updates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] AutoCreateRecordEditModel model) => base.Update(model);

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] AutoCreateRecordEditModel model) => base.Create(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override AutoCreateRecordViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.AdminAutoCreateConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {

        }
    }
}