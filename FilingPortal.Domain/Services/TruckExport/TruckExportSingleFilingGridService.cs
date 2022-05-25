using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Infrastructure.Helpers;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.TruckExport;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class TruckExportSingleFilingGridService : ISingleFilingGridService<TruckExportRecord>
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<TruckExportDefValue, TruckExportDefValuesManualReadModel, TruckExportDocument> _service;
        
        /// <summary>
        /// The uniqueDataRepository of Truck Export records
        /// </summary>
        private readonly ITruckExportReadModelRepository _uniqueDataRepository;


        public TruckExportSingleFilingGridService(
            ISingleFilingGridWorker<TruckExportDefValue, TruckExportDefValuesManualReadModel, TruckExportDocument> service
            , ITruckExportReadModelRepository uniqueDataRepository)
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
            var uniqueData = _uniqueDataRepository.GetByFilingHeaderIds(filingHeadersIds.ToArray())
                .OrderBy(x => x.FilingHeaderId);

            uniqueData.ForEach(uniqueRecord =>
            {
                var filingHeaderData = singleFilingData[(int) uniqueRecord.FilingHeaderId];

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
