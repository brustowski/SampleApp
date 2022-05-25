using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Audit.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Audit.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Controllers.Audit.Rail
{
    /// <summary>
    /// Controller for Rail Audit - Train Consist Sheet
    /// </summary>
    [RoutePrefix("api/audit/rail/train-consist-sheet")]
    public class RailAuditTrainConsistSheetController : ApiControllerBase
    {
        private readonly IAuditTrainConsistSheetRepository _repository;
        private readonly ISingleRecordValidator<AuditRailTrainConsistSheet> _singleInboundRecordValidator;
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="RailAuditTrainConsistSheetController"/>
        /// </summary>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Train Consist Sheets repository</param>
        /// <param name="singleInboundRecordValidator">Record validator</param>
        public RailAuditTrainConsistSheetController(
            ISearchRequestFactory searchRequestFactory,
            IAuditTrainConsistSheetRepository repository,
            ISingleRecordValidator<AuditRailTrainConsistSheet> singleInboundRecordValidator)
        {
            _searchRequestFactory = searchRequestFactory;
            _repository = repository;
            _singleInboundRecordValidator = singleInboundRecordValidator;
        }

        /// <summary>
        /// Gets the total matches by specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("gettotalmatches")]
        [PermissionRequired(Permissions.AuditRailImportTrainConsistSheet)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<AuditRailTrainConsistSheet>(data);
            return await _repository.GetTotalMatchesAsync<AuditRailTrainConsistSheet>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.AuditRailImportTrainConsistSheet)]
        public async Task<SimplePagedResult<TrainConsistSheetViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<AuditRailTrainConsistSheet>(data);

            SimplePagedResult<AuditRailTrainConsistSheet> pagedResult = await _repository.GetAllOrderedAsync<AuditRailTrainConsistSheet>(searchRequest);

            SimplePagedResult<TrainConsistSheetViewModel> result = pagedResult.Map<SimplePagedResult<AuditRailTrainConsistSheet>, SimplePagedResult<TrainConsistSheetViewModel>>();

            foreach (var record in pagedResult.Results)
            {
                var model = result.Results.FirstOrDefault(x => x.Id == record.Id);
                if (model == null) continue;
                await SetValidationErrors(record, model);
                SetHighlightingType(model);
            }

            return result;
        }

        /// <summary>
        /// Verifies the specified data
        /// </summary>
        [HttpPost]
        [Route("verify")]
        [PermissionRequired(Permissions.AuditRailImportTrainConsistSheet)]
        public void Verify() => _repository.Verify(CurrentUser.Id);

        /// <summary>
        /// Verifies the specified data
        /// </summary>
        [HttpDelete]
        [Route("clear")]
        [PermissionRequired(Permissions.AuditRailImportTrainConsistSheet)]
        public void RemoveAll() => _repository.Delete(_repository.GetAll().Select(x => x.Id));

        /// <summary>
        /// Sets errors occured for Inbound Record to its model
        /// </summary>
        /// <param name="record">The Inbound Record</param>
        /// <param name="model">The Inbound Record model</param>
        private async Task SetValidationErrors(AuditRailTrainConsistSheet record, TrainConsistSheetViewModel model)
        {
            model.Errors = await _singleInboundRecordValidator.GetErrorsAsync(record) ?? new List<string>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(TrainConsistSheetViewModel model)
        {
            model.HighlightingType = model.Errors.Any()
                ? HighlightingType.Error
                : model.Status == "Matched" ? HighlightingType.Success : HighlightingType.NoHighlighting;
        }
    }
}