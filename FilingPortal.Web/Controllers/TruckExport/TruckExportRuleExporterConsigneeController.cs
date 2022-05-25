using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
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
using FilingPortal.Web.Models.TruckExport;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller provides actions for Truck Export Consignee Rule 
    /// </summary>
    [RoutePrefix("api/rules/export/truck/exporter-consignee")]
    public class TruckExportRuleExporterConsigneeController : RuleControllerBase<TruckExportRuleExporterConsignee, TruckExportRuleExporterConsigneeViewModel, TruckExportRuleExporterConsigneeEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.TruckExportRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleExporterConsigneeController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The Rule repository</param>
        /// <param name="ruleValidator">The Rule validator</param>
        /// <param name="ruleService">The Rule Service</param>
        public TruckExportRuleExporterConsigneeController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<TruckExportRuleExporterConsignee> repository,
            IRuleService<TruckExportRuleExporterConsignee> ruleService,
            IRuleValidator<TruckExportRuleExporterConsignee> ruleValidator
            )
            : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.TruckViewExportRecordRules)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.TruckViewExportRecordRules)]
        public override Task<SimplePagedResult<TruckExportRuleExporterConsigneeViewModel>> Search([FromBody]SearchRequestModel data) => base.Search(data);

        /// <summary>
        /// Updates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.TruckEditExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] TruckExportRuleExporterConsigneeEditModel model) => base.Update(model);

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.TruckEditExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] TruckExportRuleExporterConsigneeEditModel model) => base.Create(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.TruckEditExportRecordRules)]
        public override TruckExportRuleExporterConsigneeViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        ///  Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.TruckDeleteExportRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<TruckExportRuleExporterConsigneeEditModel>(x => x.Exporter),
                string.Empty);
        }
    }
}