using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.CanadaTruckImport.Web.Configs;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Port rules
    /// </summary>
    [RoutePrefix("api/rules/canada-imp-truck/port")]
    public class CanadaTruckImportRulePortController : RuleControllerBase<RulePort, RulePortViewModel, RulePortEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.RulesRecordActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanadaTruckImportRulePortController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Rule repository</param>
        /// <param name="ruleValidator">Rule validator</param>
        /// <param name="ruleService">Rule Service</param>
        public CanadaTruckImportRulePortController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<RulePort> repository,
            IRuleValidator<RulePort> ruleValidator,
            IRuleService<RulePort> ruleService
            ) : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewRules)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewRules)]
        public override async Task<SimplePagedResult<RulePortViewModel>> Search([FromBody] SearchRequestModel data)
            => await base.Search(data);

        /// <summary>
        /// Creates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.EditRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] RulePortEditModel model) => base.Create(model);

        /// <summary>
        /// Updates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.EditRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] RulePortEditModel model) => base.Update(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.EditRules)]
        public override RulePortViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the Rail Description rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.DeleteRules)]
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