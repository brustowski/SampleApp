using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Infrastructure.Helpers;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class PipelineSingleFilingGridService : IPipelineSingleFilingGridService
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<PipelineDefValueReadModel, PipelineDefValueManualReadModel, PipelineDocument> _service;

        private readonly IPipelineFilingDataRepository _uniqueDataRepository;

        public PipelineSingleFilingGridService(
            ISingleFilingGridWorker<PipelineDefValueReadModel, PipelineDefValueManualReadModel, PipelineDocument> service,
            IPipelineFilingDataRepository uniqueDataRepository)
        {
            _service = service;
            _uniqueDataRepository = uniqueDataRepository;
        }

        /// <summary>
        /// Gets single filing grid information
        /// </summary>
        /// <param name="data">Search request</param>
        public SimplePagedResult<dynamic> GetData(SearchRequestModel data)
        {
            var filingHeadersIds = data.ExtractFilingHeaders().ToList();

            var results = new List<dynamic>();
            var singleFilingData = _service.GetData(filingHeadersIds);
            var uniqueData = _uniqueDataRepository
                .GetByFilingNumbers(filingHeadersIds.ToArray())
                .OrderBy(x => x.FilingHeaderId);

            uniqueData.ForEach(uniqueRecord =>
            {
                var filingHeaderData = singleFilingData[uniqueRecord.FilingHeaderId];

                IDictionary<string, object> uniqueDataDictionary = uniqueRecord.GetKeyValueMap();
                IDictionary<string, object> singleFilingDictionary = filingHeaderData.GetProperties();

                singleFilingDictionary.ToList().ForEach(x => uniqueDataDictionary[x.Key] = x.Value);

                results.Add(new FPDynObject(uniqueDataDictionary.ToDictionary(x => x.Key, y => y.Value)));
            });

            return new SimplePagedResult<dynamic>()
            {
                CurrentPage = 1,
                PageSize = results.Count,
                Results = results
            };
        }
        /// <summary>
        /// Gets single filing grid records amount
        /// </summary>
        /// <param name="data">Search request</param>
        public int GetTotalMatches(SearchRequestModel data) => _service.GetTotalMatches(data.ExtractFilingHeaders());
    }
}
