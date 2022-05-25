using FilingPortal.Domain.Common;
using FilingPortal.Domain.Infrastructure.Helpers;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Repositories;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.Zones.Entry.Domain.Services
{
    /// <summary>
    /// Service that prepares Single Filing Grid
    /// </summary>
    public class FilingGridService : ISingleFilingGridService<InboundRecord>
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<DefValue, DefValueManualReadModel, Document> _service;

        /// <summary>
        /// The inbound records repository
        /// </summary>
        private readonly IInboundReadModelRepository _uniqueDataRepository;

        /// <summary>
        /// Creates a new instance of <see cref="FilingGridService"/>
        /// </summary>
        /// <param name="service">Filing grid worker</param>
        /// <param name="uniqueDataRepository">Inbound read models repository</param>
        public FilingGridService(
            ISingleFilingGridWorker<DefValue, DefValueManualReadModel, Document> service
            , IInboundReadModelRepository uniqueDataRepository)
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
            IDictionary<int, FPDynObject> singleFilingData = _service.GetData(filingHeadersIds);
            IOrderedEnumerable<InboundReadModel> uniqueData = _uniqueDataRepository.GetByFilingHeaderIds(filingHeadersIds.ToArray())
                .OrderBy(x => x.FilingHeaderId);

            uniqueData.ForEach(uniqueRecord =>
            {
                if (uniqueRecord.FilingHeaderId == null)
                {
                    return;
                }

                FPDynObject filingHeaderData = singleFilingData.ContainsKey((int)uniqueRecord.FilingHeaderId)
                    ? singleFilingData[(int)uniqueRecord.FilingHeaderId]
                    : new FPDynObject();

                IDictionary<string, object> uniqueDataDictionary = uniqueRecord.GetKeyValueMap();
                IDictionary<string, object> singleFilingDictionary = filingHeaderData.GetProperties();

                singleFilingDictionary.ToList().ForEach(x => uniqueDataDictionary[x.Key] = x.Value);

                var row = new FPDynObject(uniqueDataDictionary.ToDictionary(x => x.Key, y => y.Value));

                results.Add(row);
            });

            return new SimplePagedResult<dynamic>
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
