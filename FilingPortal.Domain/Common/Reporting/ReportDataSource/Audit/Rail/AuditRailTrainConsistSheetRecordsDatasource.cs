using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource.Audit.Rail
{
    /// <summary>
    /// Class that represents data source for Audit Rail Train Consist sheet records for reports
    /// </summary>
    class AuditRailTrainConsistSheetRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailTrainConsistSheetRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public AuditRailTrainConsistSheetRecordsDataSource(ISearchRepository<AuditRailTrainConsistSheet> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.AuditRailTrainConsistSheet;

        public ISearchRepository<AuditRailTrainConsistSheet> SearchRepository { get; }
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
