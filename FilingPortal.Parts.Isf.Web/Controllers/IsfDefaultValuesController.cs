using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Isf.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for Default Values
    /// </summary>
    [RoutePrefix("api/rules/isf/default-values")]
    public class IsfDefaultValuesController : DefaultValuesControllerBase<DefValueReadModel, DefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsfDefaultValuesController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="repository">Default Values repository</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="defValueService">The Rule service</param>
        public IsfDefaultValuesController(
            IPageConfigContainer pageConfigContainer,
            IDefValuesReadModelRepository<DefValueReadModel> repository,
            ISearchRequestFactory searchRequestFactory,
            IDefValueService<DefValue> defValueService) : base(pageConfigContainer, repository, searchRequestFactory, defValueService)
        {
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewConfiguration)]
        public override async Task<int> GetTotalMatches(SearchRequestModel data) => await base.GetTotalMatches(data);

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewConfiguration)]
        public override async Task<SimplePagedResult<DefValuesViewModel>> Search([FromBody] SearchRequestModel data) =>
            await base.Search(data);

        /// <summary>
        /// Creates the Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Create([FromBody] DefValuesEditModel model) =>
            base.Create(model);

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public override DefValuesViewModel GetNewRow() => base.GetNewRow();

        /// <summary>
        /// Updates the Rail Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Update([FromBody] DefValuesEditModel model) =>
            base.Update(model);

        /// <summary>
        ///  Deletes the Default Value rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.DeleteConfiguration)]
        public override ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId) => base.Delete(ruleId);
    }
}