﻿using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Common;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Rail Default Values
    /// </summary>
    [RoutePrefix("api/rules/rail/default-values")]
    public class RailDefaultValuesController : ApiControllerBase
    {
        /// <summary>
        /// The page configuration container
        /// </summary>
        private readonly IPageConfigContainer _pageConfigContainer;

        /// <summary>
        /// The repository for Rail Default Values
        /// </summary>
        private readonly IRailDefValuesReadModelRepository _repository;

        /// <summary>
        /// The search request factory
        /// </summary>
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// The DefValue service 
        /// </summary>
        private readonly IDefValueService<RailDefValues> _defValueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefaultValuesController" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="repository">Rail Default Values repository</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="defValueService">The Rule service</param>
        public RailDefaultValuesController(
            IPageConfigContainer pageConfigContainer,
            IRailDefValuesReadModelRepository repository,
            ISearchRequestFactory searchRequestFactory,
            IDefValueService<RailDefValues> defValueService)
        {
            _pageConfigContainer = pageConfigContainer;
            _repository = repository;
            _searchRequestFactory = searchRequestFactory;
            _defValueService = defValueService;
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
            SearchRequest searchRequest = _searchRequestFactory.Create<DefValuesViewModel>(data);
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
            SearchRequest searchRequest = _searchRequestFactory.Create<DefValuesViewModel>(data);

            SimplePagedResult<DefValuesViewModel> result = await _repository.GetAllAsync<DefValuesViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = _pageConfigContainer.GetPageConfig(PageConfigNames.DefValueActionsConfigName);

            foreach (DefValuesViewModel record in result.Results)
            {
                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return result;
        }

        /// <summary>
        /// Creates the Rail Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("create")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Create([FromBody] RailDefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            System.Collections.Generic.IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                var defValue = model.Map<DefValuesEditModel, RailDefValues>();
                defValue.CreatedUser = CurrentUser.Id;
                _defValueService.Create(defValue, model.TableName, model.DefaultValue);
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
        /// Updates the Rail Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("update")]
        [PermissionRequired(Permissions.EditConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Update([FromBody] RailDefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            System.Collections.Generic.IEnumerable<FieldErrorViewModel> fieldsErrors = GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                var defValue = model.Map<DefValuesEditModel, RailDefValues>();
                defValue.CreatedUser = CurrentUser.Id;
                var result = _defValueService.Update(defValue, model.TableName, model.DefaultValue);
                validationResult.SetCommonError(result.Errors);
            }

            return validationResult;
        }

        /// <summary>
        ///  Deletes the Rail Default Value rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        [HttpPost]
        [Route("delete/{ruleId:int}")]
        [PermissionRequired(Permissions.DeleteConfiguration)]
        public ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();
            Domain.Common.OperationResult.OperationResult result = _defValueService.Delete(ruleId);
            validationResult.SetCommonError(result.Errors);
            return validationResult;
        }
    }
}