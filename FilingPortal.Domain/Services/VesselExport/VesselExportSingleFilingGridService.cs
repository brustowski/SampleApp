using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Infrastructure.Helpers;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services.VesselExport
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class VesselExportSingleFilingGridService : ISingleFilingGridService<VesselExportRecord>
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<VesselExportDefValue, VesselExportDefValuesManualReadModel, VesselExportDocument> _service;
        
        /// <summary>
        /// The uniqueDataRepository of Vessel Export records
        /// </summary>
        private readonly IVesselExportReadModelRepository _uniqueDataRepository;


        public VesselExportSingleFilingGridService(
            ISingleFilingGridWorker<VesselExportDefValue, VesselExportDefValuesManualReadModel, VesselExportDocument> service
            , IVesselExportReadModelRepository uniqueDataRepository)
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
            List<int> filingHeadersIds = data.ExtractFilingHeaders().ToList();

            var results = new List<dynamic>();
            IDictionary<int, FPDynObject> singleFilingData = _service.GetData(filingHeadersIds);
            IOrderedEnumerable<VesselExportReadModel> uniqueData = _uniqueDataRepository.GetByFilingHeaderIds(filingHeadersIds.ToArray())
                .OrderBy(x => x.FilingHeaderId);

            uniqueData.ForEach(uniqueRecord =>
            {
                if (uniqueRecord.FilingHeaderId == null) return;
                FPDynObject filingHeaderData = singleFilingData[(int)uniqueRecord.FilingHeaderId];

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
