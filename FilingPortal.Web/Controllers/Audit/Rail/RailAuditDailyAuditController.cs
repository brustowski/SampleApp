using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
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
using Newtonsoft.Json.Linq;

namespace FilingPortal.Web.Controllers.Audit.Rail
{
    /// <summary>
    /// Controller for Rail Audit - Daily audit
    /// </summary>
    [RoutePrefix("api/audit/rail/daily-audit")]
    public class RailAuditDailyAuditController : ApiControllerBase
    {
        private readonly IRailDailyAuditRepository _repository;
        private readonly ISingleRecordTypedValidator<FieldsValidationResult, AuditRailDaily> _singleInboundRecordValidator;
        private readonly ISearchRequestFactory _searchRequestFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="RailAuditDailyAuditController"/>
        /// </summary>
        /// <param name="searchRequestFactory">Search request factory</param>
        /// <param name="repository">Rail Daily Audits repository</param>
        /// <param name="singleInboundRecordValidator">Record validator</param>
        public RailAuditDailyAuditController(
            ISearchRequestFactory searchRequestFactory,
            IRailDailyAuditRepository repository,
            ISingleRecordTypedValidator<FieldsValidationResult, AuditRailDaily> singleInboundRecordValidator)
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
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public async Task<int> GetTotalMatches(SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<AuditRailDaily>(data);
            return await _repository.GetTotalMatchesAsync<AuditRailDaily>(searchRequest);
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("search")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public async Task<SimplePagedResult<DailyAuditViewModel>> Search([FromBody]SearchRequestModel data)
        {
            SearchRequest searchRequest = _searchRequestFactory.Create<AuditRailDaily>(data);

            SimplePagedResult<AuditRailDaily> pagedResult = await _repository.GetAllOrderedAsync<AuditRailDaily>(searchRequest);

            SimplePagedResult<DailyAuditViewModel> result = pagedResult.Map<SimplePagedResult<AuditRailDaily>, SimplePagedResult<DailyAuditViewModel>>();

            foreach (AuditRailDaily record in pagedResult.Results)
            {
                DailyAuditViewModel model = result.Results.FirstOrDefault(x => x.Id == record.Id);
                if (model == null) continue;
                await SetValidationErrors(record, model);
                SetHighlightingType(model);
            }

            return result;
        }

        /// <summary>
        /// Searches the specified data
        /// </summary>
        /// <param name="data">The data</param>
        [HttpPost]
        [Route("report")]
        [PermissionRequired(Permissions.AuditRailDailyAudit)]
        public void Report([FromBody]SearchRequestModel data)
        {
            var lastModifiedDate = (JArray)data.FilterSettings.Filters
                .FirstOrDefault(x => x.FieldName == "LastModifiedDate")?.Values[0].Value;

            DateTime dtFrom = DateTime.Today;
            DateTime dtTo = DateTime.Today;
            
            if (lastModifiedDate != null)
            {
                dtFrom = Convert.ToDateTime(lastModifiedDate[0]);
                dtTo = Convert.ToDateTime(lastModifiedDate[1]);
            }

            _repository.LoadFromCargoWise(dtFrom, dtTo);
        }

        /// <summary>
        /// Sets errors occured for Inbound Record to its model
        /// </summary>
        /// <param name="record">The Inbound Record</param>
        /// <param name="model">The Inbound Record model</param>
        private async Task SetValidationErrors(AuditRailDaily record, DailyAuditViewModel model)
        {
            model.Errors = await _singleInboundRecordValidator.GetErrorsAsync(record) ?? new List<FieldsValidationResult>();
        }

        /// <summary>
        /// Sets the highlighting type for the specified Inbound Record item
        /// </summary>
        /// <param name="model">The Inbound Record item</param>
        private void SetHighlightingType(DailyAuditViewModel model)
        {
            model.HighlightingType = model.Errors.Any(x => x.Severity == Severity.Error)
                ? HighlightingType.Error
                : model.Errors.Any(x => x.Severity == Severity.Warning) ? HighlightingType.Warning : HighlightingType.Success;
        }
    }
}