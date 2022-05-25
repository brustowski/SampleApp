using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models.Audit.Rail;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Audit.Rail
{
    /// <summary>
    /// Controller for Rail Audit - Daily audit SPI rules
    /// </summary>
    [RoutePrefix("api/audit/rail/daily-audit-spi-rules")]
    public class RailAuditDailyAuditSpiRulesController : RuleControllerBase<AuditRailDailySpiRule, DailyAuditSpiRuleViewModel, DailyAuditSpiRuleEditModel>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RailAuditDailyAuditSpiRulesController"/>
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Port Rail Rules repository</param>
        /// <param name="ruleService">Rail Port Rule service</param>
        /// <param name="ruleValidator">The Rail Port rule validator</param>       
        public RailAuditDailyAuditSpiRulesController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<AuditRailDailySpiRule> repository,
            IRuleService<AuditRailDailySpiRule> ruleService,
            IRuleValidator<AuditRailDailySpiRule> ruleValidator) : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public override Task<SimplePagedResult<DailyAuditSpiRuleViewModel>> Search([FromBody] SearchRequestModel data) =>
            base.Search(data);

        /// <summary>
        /// Gets the PageConfigurationName
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.AuditRailDailyAuditRulesConfigName;

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] DailyAuditSpiRuleEditModel model) => base.Create(model);

        /// <summary>
        /// Updates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] DailyAuditSpiRuleEditModel model) => base.Update(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public override DailyAuditSpiRuleViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the Rail Description rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
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