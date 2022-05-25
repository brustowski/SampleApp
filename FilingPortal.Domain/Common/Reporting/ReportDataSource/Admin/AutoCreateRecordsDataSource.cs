using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Common.Reporting.ReportDatasource.Admin
{
    /// <summary>
    /// Class that represents data source for Auto-create records for reports
    /// </summary>
    internal class AutoCreateRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCreateRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository">Repository of <see cref="AutoCreateRecord"/></param>
        public AutoCreateRecordsDataSource(IRuleRepository<AutoCreateRecord> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.AutoCreateRecords;

        /// <summary>
        /// Repository of <see cref="AutoCreateRecord"/>
        /// </summary>
        public IRuleRepository<AutoCreateRecord> SearchRepository { get; }
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
