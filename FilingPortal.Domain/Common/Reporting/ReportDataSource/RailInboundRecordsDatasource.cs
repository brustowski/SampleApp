using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource
{
    /// <summary>
    /// Class that represents data source for Rail inbound records for reports
    /// </summary>
    internal class RailInboundRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository">The data repository</param>
        public RailInboundRecordsDataSource(IRailInboundReadModelRepository searchRepository)
        {
            SearchRepository = searchRepository;
        }
        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.InboundRecords;
        /// <summary>
        /// Gets the repository
        /// </summary>
        public IRailInboundReadModelRepository SearchRepository { get; }
        /// <summary>
        /// Gets all available records as <see cref="SimplePagedResult{TEntityDto}"/> specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(SearchRequest request) where TEntity : class, new()
            => (await SearchRepository.GetAllAsync<TEntity>(request)).Results;
        /// <summary>
        /// Gets total matches of records specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<int> GetTotalMatches<TEntity>(SearchRequest request) where TEntity : class
            => await SearchRepository.GetTotalMatchesAsync<TEntity>(request);
    }
}
