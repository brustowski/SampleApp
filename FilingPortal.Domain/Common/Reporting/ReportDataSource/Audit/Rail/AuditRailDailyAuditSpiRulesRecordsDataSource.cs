using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Repositories.Audit.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource.Audit.Rail
{
    /// <summary>
    /// Class that represents data source for Audit Rail Daily audit rules records for reports
    /// </summary>
    class AuditRailDailyAuditSpiRulesRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyAuditSpiRulesRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public AuditRailDailyAuditSpiRulesRecordsDataSource(IRailDailyAuditSpiRulesRepository searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.AuditRailDailyAuditSpiRules;

        public IRailDailyAuditSpiRulesRepository SearchRepository { get; }
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
