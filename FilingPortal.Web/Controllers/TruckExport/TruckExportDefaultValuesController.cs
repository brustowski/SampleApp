using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller provides actions for Truck Export Default Values
    /// </summary>
    [RoutePrefix("api/rules/export/truck/default-values")]
    public class TruckExportDefaultValuesController : ApiControllerBase
    {
        /// <summary>
        /// The page configuration container
        /// </summary>
        private readonly IPageConfigContainer _pageConfigContainer;

        /// <summary>
        /// The default value repository
        /// </summary>
        private readonly ITruckExportDefValuesReadModelRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The DefValue service 
        /// </summary>
        private readonly IDefValueService<TruckExportDefValue> _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefaultValuesController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="repository">The default value repository</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="service">The DefValue service</param>
        public TruckExportDefaultValuesController(
            IPageConfigContainer pageConfigContainer,
            ITruckExportDefValuesReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            IDefValueService<TruckExportDefValue> service)
        {
            _pageConfigContainer = pageConfigContainer;
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
            _service = service;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.ViewConfiguration)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<DefValuesViewModel>(data);
            return await _repository.GetTotalMatchesAsync<DefValuesViewModel>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.ViewConfiguration)]
        public async Task<SimplePagedResult<DefValuesViewModel>> Search([FromBody]SearchRequestModel data)
        {
            var searchRequest = _searchRequestFactory.Create<DefValuesViewModel>(data);

            var result = await _repository.GetAllAsync<DefValuesViewModel>(searchRequest);

            var actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.DefValueActionsConfigName);

            foreach (var record in result.Results)
            {
                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return result;
        }

        /// <summary>
        /// Creates the Default Value Rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Create([FromBody] DefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            var fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                var defValue = model.Map<DefValuesEditModel, TruckExportDefValue>();
                defValue.CreatedUser = CurrentUser.Id;
                _service.Create(defValue, model.TableName, model.DefaultValue);
            }

            return validationResult;
        }

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        [HttpGet]
        [Route("getNew")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public DefValuesViewModel GetNewRow()
        {
            return new DefValuesViewModel();
        }

        /// <summary>
        /// Updates the Default Value Rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Update([FromBody] DefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            var fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                var defValue = model.Map<DefValuesEditModel, TruckExportDefValue>();
                defValue.CreatedUser = CurrentUser.Id;
                var result = _service.Update(defValue, model.TableName, model.DefaultValue);
                validationResult.SetCommonError(result.Errors);
            }

            return validationResult;
        }

        /// <summary>
        ///  Deletes the Default Value rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.DeleteConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();
            var result = _service.Delete(ruleId);
            validationResult.SetCommonError(result.Errors);
            return validationResult;
        }
    }
}