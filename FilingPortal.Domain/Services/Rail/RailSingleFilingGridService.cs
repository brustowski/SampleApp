using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Infrastructure.Helpers;
using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class RailSingleFilingGridService : IRailSingleFilingGridService
    {

        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<RailDefValuesReadModel, RailDefValuesManualReadModel, RailDocument> _service;
        /// <summary>
        /// Rail unique data repository
        /// </summary>
        private readonly IRailFilingDataRepository _uniqueDataRepository;

        /// <summary>
        /// Filing Headers repository
        /// </summary>
        private readonly IRailFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailSingleFilingGridService"/> class.
        /// </summary>
        public RailSingleFilingGridService(
            ISingleFilingGridWorker<RailDefValuesReadModel, RailDefValuesManualReadModel, RailDocument> service,
            IRailFilingDataRepository uniqueDataRepository,
            IRailFilingHeadersRepository filingHeadersRepository)
        {
            _service = service;
            _uniqueDataRepository = uniqueDataRepository;
            _filingHeadersRepository = filingHeadersRepository;
        }
        /// <summary>
        /// Gets single filing grid information
        /// </summary>
        /// <param name="data">Search request</param>
        public SimplePagedResult<dynamic> GetData(SearchRequestModel data)
        {
            List<int> filingHeadersIds = data.ExtractFilingHeaders().ToList();

            IOrderedEnumerable<KeyValuePair<int, FPDynObject>> singleFilingData = _service.GetData(filingHeadersIds).OrderBy(x => x.Key);
            Dictionary<int, IDictionary<string, object>> resultDictionary = singleFilingData.ToDictionary(x => x.Key, pair => pair.Value.GetProperties());

            IList<RailFilingData> uniqueData = _uniqueDataRepository
                .GetByFilingNumbers(filingHeadersIds.ToArray());

            uniqueData.ForEach(uniqueRecord =>
            {
                if (!resultDictionary.ContainsKey(uniqueRecord.FilingHeaderId))
                {
                    resultDictionary.Add(uniqueRecord.FilingHeaderId, new Dictionary<string, object>());
                }

                IDictionary<string, object> filingHeaderData = resultDictionary[uniqueRecord.FilingHeaderId];

                IDictionary<string, object> uniqueDataDictionary = uniqueRecord.GetKeyValueMap();

                uniqueDataDictionary.ToList().ForEach(x =>
                {
                    if (filingHeaderData.ContainsKey(x.Key) && filingHeaderData[x.Key].ToString() == x.Value.ToString())
                    {
                        return;
                    }

                    if (filingHeaderData.ContainsKey(x.Key))
                    {
                        // value changed
                        if (filingHeaderData[x.Key] is List<object> valuesList)
                        {
                            valuesList.Add(x.Value);
                        }
                        else
                        {
                            filingHeaderData[x.Key] = new List<object>
                                {filingHeaderData[x.Key], x.Value};
                        }
                    }
                    else
                    {
                        filingHeaderData[x.Key] = x.Value;
                    }
                });
            });

            var filingHeadersInfo = _filingHeadersRepository.GetList(filingHeadersIds).ToDictionary(x => x.Id,
                x => new { x.GrossWeightSummary, x.GrossWeightSummaryUnit, IsConsolidated = x.RailBdParseds.Count > 1 });

            filingHeadersInfo.ForEach(x =>
            {
                IDictionary<string, object> resultRecord = resultDictionary[x.Key];
                resultRecord.Add("IsConsolidated", x.Value.IsConsolidated);
                if (x.Value.IsConsolidated)
                {
                    resultRecord["GrossWeight"] = x.Value.GrossWeightSummary;
                    resultRecord["GrossWeightUnit"] = x.Value.GrossWeightSummaryUnit;
                }
            });

            return new SimplePagedResult<dynamic>()
            {
                CurrentPage = 1,
                PageSize = resultDictionary.Count,
                Results = resultDictionary.Values.Select(x => new FPDynObject(x.ToDictionary(pair => pair.Key, pair => pair.Value)))
            };
        }
        /// <summary>
        /// Gets single filing grid records amount
        /// </summary>
        /// <param name="data">Search request</param>
        public int GetTotalMatches(SearchRequestModel data) => _service.GetTotalMatches(data.ExtractFilingHeaders());

        /// <summary>
        /// Returns manifest data count for search request
        /// </summary>
        /// <param name="data">Search request</param>
        public int GetManifestDataCount(SearchRequestModel data)
        {
            var filingHeadersIds = data.ExtractFilingHeaders().ToList();

            return _uniqueDataRepository.CountByFilingNumbers(filingHeadersIds.ToArray());
        }

        /// <summary>
        /// Returns manifest data for search request
        /// </summary>
        /// <param name="data">Search request</param>
        public SimplePagedResult<RailFilingData> GetManifestData(SearchRequestModel data)
        {
            var filingHeadersIds = data.ExtractFilingHeaders().ToList();

            IList<RailFilingData> uniqueData = _uniqueDataRepository
                .GetByFilingNumbers(filingHeadersIds.ToArray());

            return new SimplePagedResult<RailFilingData>()
            {
                CurrentPage = 1,
                PageSize = uniqueData.Count,
                Results = uniqueData
                    .OrderBy(x => x.FilingHeaderId)
            };
        }
    }
}
