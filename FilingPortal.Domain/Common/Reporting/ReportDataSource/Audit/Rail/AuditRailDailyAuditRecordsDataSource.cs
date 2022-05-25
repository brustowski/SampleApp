using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Mapping;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource.Audit.Rail
{
    /// <summary>
    /// Class that represents data source for Audit Rail Daily audit records for reports
    /// </summary>
    internal class AuditRailDailyAuditRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyAuditRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository">Rail daily audit repository</param>
        /// <param name="requestFactory">Request factory</param>
        public AuditRailDailyAuditRecordsDataSource(
            ISearchRepository<AuditRailDaily> searchRepository,
            ISearchRequestFactory requestFactory)
        {
            SearchRepository = searchRepository;
            _requestFactory = requestFactory;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.AuditRailDailyAudit;

        public ISearchRepository<AuditRailDaily> SearchRepository { get; }

        private readonly ISearchRequestFactory _requestFactory;

        /// <summary>
        /// Gets all available records as <see cref="SimplePagedResult{TEntityDto}"/> specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(SearchRequest request) where TEntity : class, new()
        {
            SearchRequest r = _requestFactory.Create<AuditRailDaily>(request.RequestModel);

            SimplePagedResult<AuditRailDaily> data = await SearchRepository.GetAllAsync<AuditRailDaily>(r);

            IEnumerable<TEntity> mappedResults = data.Results.Map<AuditRailDaily, TEntity>();

            return mappedResults;
        }

        /// <summary>
        /// Gets total matches of records specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<int> GetTotalMatches<TEntity>(SearchRequest request) where TEntity : class
        {
            SearchRequest r = _requestFactory.Create<AuditRailDaily>(request.RequestModel);
            return await SearchRepository.GetTotalMatchesAsync<AuditRailDaily>(r);
        }
    }
}
