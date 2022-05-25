using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseInternal
{
    /// <summary>
    /// Represents the data source for CargoWise Internal report
    /// </summary>
    class DataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource" /> class
        /// </summary>
        /// <param name="searchRepository">Repository of <see cref="InboundRecordReadModel"/></param>
        public DataSource(ISearchRepository<InboundRecordReadModel> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => ReportNames.CargoWiseInternal;

        /// <summary>
        /// Repository of <see cref="InboundRecordReadModel"/>
        /// </summary>
        private ISearchRepository<InboundRecordReadModel> SearchRepository { get; }
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
