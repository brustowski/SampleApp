using FilingPortal.Domain.Entities.Rail;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Domain.Repositories.Rail
{
    /// <summary>
    /// Describes repository of <see cref="RailFilingData"/>
    /// </summary>
    public interface IRailFilingDataRepository : IFilingDataRepository<RailFilingData>
    {
        /// <summary>
        /// Provides Rail Filing Data result with Summary Row
        /// </summary>
        /// <param name="request"><see cref="SearchRequest"/></param>
        Task<PagedResultWithSummaryRow<RailFilingData>> GetAllWithSummaryAsync(SearchRequest request);

        /// <summary>
        /// Counts BDParsed Records by filing headers ids
        /// </summary>
        /// <param name="filingHeadersIds">Filing headers ids</param>
        int CountByFilingNumbers(int[] filingHeadersIds);
    }
}