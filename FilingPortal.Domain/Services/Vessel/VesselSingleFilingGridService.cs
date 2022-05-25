using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Infrastructure.Helpers;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Vessel
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class VesselSingleFilingGridService : ISingleFilingGridService<VesselImportRecord>
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<VesselImportDefValueReadModel, VesselImportDefValuesManualReadModel, VesselImportDocument> _service;

        public VesselSingleFilingGridService(
            ISingleFilingGridWorker<VesselImportDefValueReadModel, VesselImportDefValuesManualReadModel, VesselImportDocument> service) => _service = service;
        /// <summary>
        /// Gets single filing grid information
        /// </summary>
        /// <param name="data">Search request</param>
        public SimplePagedResult<dynamic> GetData(SearchRequestModel data)
        {
            IDictionary<int, FPDynObject> results = _service.GetData(data.ExtractFilingHeaders());
            return new SimplePagedResult<dynamic>
            {
                CurrentPage = 1,
                PageSize = results.Count,
                Results = results.Select(x => x.Value)
            };
        }
        /// <summary>
        /// Gets single filing grid records amount
        /// </summary>
        /// <param name="data">Search request</param>
        public int GetTotalMatches(SearchRequestModel data) => _service.GetTotalMatches(data.ExtractFilingHeaders());
    }
}
