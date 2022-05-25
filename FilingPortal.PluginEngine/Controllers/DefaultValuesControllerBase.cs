using System.Collections.Generic;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using Framework.Domain.Paging;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Controller provides actions for Default Values
    /// </summary>
    public class DefaultValuesControllerBase<TReadModel, TDefValue> : InboundControllerBase<TReadModel, DefValuesViewModel>
        where TReadModel : BaseDefValueReadModel
        where TDefValue : BaseDefValue, new()

    {
        /// <summary>
        /// The DefValue service 
        /// </summary>
        private readonly IDefValueService<TDefValue> _defValueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValuesControllerBase{TReadModel, TDefValue}" /> class
        /// </summary>
        /// <param name="pageConfigContainer">The page configuration container</param>
        /// <param name="repository">Default Values repository</param>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="defValueService">The Rule service</param>
        protected DefaultValuesControllerBase(
            IPageConfigContainer pageConfigContainer,
            IDefValuesReadModelRepository<TReadModel> repository,
            ISearchRequestFactory searchRequestFactory,
            IDefValueService<TDefValue> defValueService) : base(repository, searchRequestFactory, pageConfigContainer, null,
            null, null)
        {
            _defValueService = defValueService;
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        public override async Task<SimplePagedResult<DefValuesViewModel>> Search(SearchRequestModel data)
        {
            SearchRequest searchRequest = SearchRequestFactory.Create<DefValuesViewModel>(data);

            SimplePagedResult<DefValuesViewModel> result = await Repository.GetAllOrderedAsync<DefValuesViewModel>(searchRequest);

            IPageConfiguration actionsConfigurator = PageConfigContainer.GetDefValueActionsConfig();

            foreach (DefValuesViewModel record in result.Results)
            {
                record.Actions = actionsConfigurator.GetActions(record, CurrentUser);
            }

            return result;
        }

        /// <summary>
        /// Creates the Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Create([FromBody] DefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            IEnumerable<FieldErrorViewModel> fieldsErrors =
                GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                TDefValue defValue = model.Map<DefValuesEditModel, TDefValue>();
                defValue.CreatedUser = CurrentUser.Id;
                _defValueService.Create(defValue, model.TableName, model.DefaultValue);
            }

            return validationResult;
        }

        /// <summary>
        /// Gets default model data for adding new row
        /// </summary>
        public virtual DefValuesViewModel GetNewRow()
        {
            return new DefValuesViewModel();
        }

        /// <summary>
        /// Updates the Rail Default Value rule using the specified view model
        /// </summary>
        /// <param name="model">The model</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Update([FromBody] DefValuesEditModel model)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();

            IEnumerable<FieldErrorViewModel> fieldsErrors =
                GetValidationResultForFields("model", ModelState);
            validationResult.AddFieldErrors(fieldsErrors);

            if (validationResult.IsValid)
            {
                TDefValue defValue = model.Map<DefValuesEditModel, TDefValue>();
                defValue.CreatedUser = CurrentUser.Id;
                OperationResult result = _defValueService.Update(defValue, model.TableName, model.DefaultValue);
                validationResult.SetCommonError(result.Errors);
            }

            return validationResult;
        }

        /// <summary>
        ///  Deletes the Rail Default Value rule with specified identifier
        /// </summary>
        /// <param name="ruleId">Identifier of the Rule to delete</param>
        public virtual ValidationResultWithFieldsErrorsViewModel Delete([FromUri] int ruleId)
        {
            var validationResult = new ValidationResultWithFieldsErrorsViewModel();
            OperationResult result = _defValueService.Delete(ruleId);
            validationResult.SetCommonError(result.Errors);
            return validationResult;
        }

        /// <summary>
        /// Deletes Vessel Export record by the specified record identifier
        /// </summary>
        /// <param name="recordIds">The record identifiers</param>
        public override IHttpActionResult Delete(int[] recordIds)
        {
            foreach (var recordId in recordIds)
            {
                Delete(recordId);
            }

            return Ok();
        }

        /// <summary>
        /// Page actions config name
        /// </summary>
        public override string PageActionsConfig { get; } = "Not required";

        /// <summary>
        /// Page config name
        /// </summary>
        public override string RecordsListConfigName { get; } = "Not required";

        /// <summary>
        /// Single record config name
        /// </summary>
        public override string RecordActionConfigName { get; } = "Not required";
    }
}