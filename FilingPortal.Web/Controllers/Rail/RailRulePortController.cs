﻿using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
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
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Port Rail Rules
    /// </summary>
    [RoutePrefix("api/rules/rail/port")]
    public class RailRulePortController : RuleControllerBase<RailRulePort, RailRulePortViewModel, RailRulePortEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.RailRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailRulePortController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Port Rail Rules repository</param>
        /// <param name="ruleService">Rail Port Rule service</param>
        /// <param name="ruleValidator">The Rail Port rule validator</param>       
        public RailRulePortController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<RailRulePort> repository,
            IRuleService<RailRulePort> ruleService,
            IRuleValidator<RailRulePort> ruleValidator
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
        public override Task<SimplePagedResult<RailRulePortViewModel>> Search([FromBody]SearchRequestModel data) => base.Search(data);

        /// <summary>
        /// Creates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] RailRulePortEditModel model) => base.Create(model);

        /// <summary>
        /// Updates the Rail Description rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] RailRulePortEditModel model) => base.Update(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.RailEditInboundRecordRules)]
        public override RailRulePortViewModel GetNewRow() => base.GetNewRow();

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
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<RailRulePortEditModel>(x => x.Port),
                string.Empty);
        }
    }
}