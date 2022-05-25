using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.DataLayer;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Provides repository of <see cref="RailFilingData"/>
    /// </summary>
    public class RailFilingDataRepository : SearchRepository<RailFilingData>, IRailFilingDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailFilingDataRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailFilingDataRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
        /// <summary>
        /// Provides Rail Filing Data result with Summary Row
        /// </summary>
        /// <param name="request"><see cref="SearchRequest"/></param>
        public async Task<PagedResultWithSummaryRow<RailFilingData>> GetAllWithSummaryAsync(SearchRequest request)
        {
            SimplePagedResult<RailFilingData> simplePageResult = await GetAllAsync<RailFilingData>(request);
            var result = new PagedResultWithSummaryRow<RailFilingData>(simplePageResult);
            if (!simplePageResult.Results.Any()) return result;
            var filingHeader = await UnitOfWork.Context.Set<RailFilingHeader>().FindAsync(simplePageResult.Results.First().FilingHeaderId);
            result.Summary = new RailFilingData
            {
                GrossWeight = filingHeader.GrossWeightSummary.Value,
                GrossWeightUnit = filingHeader.GrossWeightSummaryUnit
            };
            return result;
        }
        /// <summary>
        /// Counts BDParsed Records by filing headers ids
        /// </summary>
        /// <param name="filingHeadersIds">Filing headers ids</param>
        public int CountByFilingNumbers(int[] filingHeadersIds) => Set.Count(x => filingHeadersIds.Contains(x.FilingHeaderId));

        /// <summary>
        /// Gets unique data for selected filing headers
        /// </summary>
        /// <param name="ids">Filing Headers ids</param>
        public IList<RailFilingData> GetByFilingNumbers(params int[] ids) => Set.Where(x => ids.Contains(x.FilingHeaderId)).ToList();
    }
}