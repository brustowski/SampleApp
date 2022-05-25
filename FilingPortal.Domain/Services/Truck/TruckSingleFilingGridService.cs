using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Infrastructure.Helpers;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Truck
{
    /// <summary>
    /// Service that prepares SingleFilingGrid
    /// </summary>
    public class TruckSingleFilingGridService : ITruckSingleFilingGridService
    {
        /// <summary>
        /// Single filing grid service worker
        /// </summary>
        private readonly ISingleFilingGridWorker<TruckDefValueReadModel, TruckDefValueManualReadModel, TruckDocument> _service;

        public TruckSingleFilingGridService(
            ISingleFilingGridWorker<TruckDefValueReadModel, TruckDefValueManualReadModel, TruckDocument> service) => _service = service;
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
