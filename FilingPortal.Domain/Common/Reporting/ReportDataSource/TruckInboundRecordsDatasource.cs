using FilingPortal.Domain.Repositories.Truck;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource
{
    /// <summary>
    /// Class that represents data source for Truck Export records for reports
    /// </summary>
    internal class TruckInboundRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository">Search repository</param>
        public TruckInboundRecordsDataSource(ITruckInboundReadModelRepository searchRepository)
        {
            SearchRepository = searchRepository;
        }
        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.TruckInboundRecords;
        /// <summary>
        /// Gets the repository
        /// </summary>
        public ITruckInboundReadModelRepository SearchRepository { get; }
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