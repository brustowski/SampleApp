﻿using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
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
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;
using Framework.Infrastructure;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller provides actions for Pipeline Batch Code Rules
    /// </summary>
    [RoutePrefix("api/rules/pipeline/batch-code")]
    public class PipelineRuleBatchCodeController : RuleControllerBase<PipelineRuleBatchCode, PipelineRuleBatchCodeViewModel, PipelineRuleBatchCodeEditModel>
    {
        /// <summary>
        /// Provides Page configuration name
        /// </summary>
        public override string PageConfigurationName => PageConfigNames.PipelineRuleConfigName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleBatchCodeController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">The rules repository</param>
        /// <param name="ruleService">The rule service</param>
        /// <param name="ruleValidator">The rule validator</param>
        public PipelineRuleBatchCodeController(
            IPageConfigContainer pageConfigContainer,
            ISearchRequestFactory searchRequestFactory,
            IRuleRepository<PipelineRuleBatchCode> repository,
            IRuleService<PipelineRuleBatchCode> ruleService,
            IRuleValidator<PipelineRuleBatchCode> ruleValidator
            )
            : base(pageConfigContainer, searchRequestFactory, repository, ruleService, ruleValidator)
        { }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.PipelineViewInboundRecordRules)]
        public override Task<int> GetTotalMatches(SearchRequestModel data) => base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.PipelineViewInboundRecordRules)]
        public override Task<SimplePagedResult<PipelineRuleBatchCodeViewModel>> Search([FromBody]SearchRequestModel data) => base.Search(data);

        /// <summary>
        /// Creates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.PipelineEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] PipelineRuleBatchCodeEditModel model) => base.Create(model);

        /// <summary>
        /// Updates the rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.PipelineEditInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] PipelineRuleBatchCodeEditModel model) => base.Update(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.PipelineEditInboundRecordRules)]
        public override PipelineRuleBatchCodeViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        /// Deletes the rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.PipelineDeleteInboundRecordRules)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);

        /// <summary>
        /// Adds the existing rule error to the specified validation result
        /// </summary>
        /// <param name="result">The validation result</param>
        public override void AddRuleExistingError(ValidationResultWithFieldsErrorsViewModel result)
        {
            result.AddFieldError(PropertyExpressionHelper.GetPropertyName<PipelineRuleBatchCodeEditModel>(x => x.BatchCode),
                string.Empty);
        }
    }
}