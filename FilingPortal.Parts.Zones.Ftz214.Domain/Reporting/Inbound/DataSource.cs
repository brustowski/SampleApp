using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Zones.Ftz214.Domain.Config;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Reporting.Inbound
{
    /// <summary>
    /// Class that represents data source for inbound records for reports
    /// </summary>
    class DataSource : IReportDatasource
    {
        /// <summary>
        /// The report data source
        /// </summary>
        private ISearchRepository<InboundReadModel> SearchRepository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public DataSource(ISearchRepository<InboundReadModel> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.InboundRecords;

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
