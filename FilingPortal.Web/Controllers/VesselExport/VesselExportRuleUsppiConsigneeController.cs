using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.VesselExport;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Web.Controllers.VesselExport
{
    /// <summary>
    /// Controller provides actions for Vessel Export USPPI-Consignee Rule 
    /// </summary>
    [RoutePrefix("api/rules/export/vessel/usppi-consignee")]
    public class VesselExportRuleUsppiConsigneeController : RuleControllerBase<VesselExportRuleUsppiConsignee, VesselExportRuleUsppiConsigneeViewModel, VesselExportRuleUsppiConsigneeEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.VesselExportRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRuleUsppiConsigneeController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The Rule repository</param>
        /// <param name="ruleValidator">The Rule validator</param>
        /// <param name="ruleService">The Rule Service</param>
        public VesselExportRuleUsppiConsigneeController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<VesselExportRuleUsppiConsignee> repository,
            IRuleService<VesselExportRuleUsppiConsignee> ruleService,
            IRuleValidator<VesselExportRuleUsppiConsignee> ruleValidator
            )
            : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.VesselViewExportRecordRules)]
        public override async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<VesselExportRuleUsppiConsigneeViewModel>(data);
            return await Repository.GetTotalMatchesAsync<VesselExportRuleUsppiConsigneeViewModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.VesselViewExportRecordRules)]
        public override async Task<SimplePagedResult<VesselExportRuleUsppiConsigneeViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<VesselExportRuleUsppiConsigneeViewModel>(data);

            SimplePagedResult<VesselExportRuleUsppiConsigneeViewModel> pagedResult = await Repository.GetAllAsync<VesselExportRuleUsppiConsigneeViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetPageConfig(PageConfigurationName);

            foreach (VesselExportRuleUsppiConsigneeViewModel record in pagedResult.Results)
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
        [PermissionRequired(Permissions.VesselEditExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] VesselExportRuleUsppiConsigneeEditModel model) => base.Update(model);

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.VesselEditExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] VesselExportRuleUsppiConsigneeEditModel model) => base.Create(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.VesselEditExportRecordRules)]
        public override VesselExportRuleUsppiConsigneeViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.VesselDeleteExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<VesselExportRuleUsppiConsigneeEditModel>(x => x.Usppi),
                string.Empty);
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<VesselExportRuleUsppiConsigneeEditModel>(x => x.Consignee),
                string.Empty);
        }
    }
}