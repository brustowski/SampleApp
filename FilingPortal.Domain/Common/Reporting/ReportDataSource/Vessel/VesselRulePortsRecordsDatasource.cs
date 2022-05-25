using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource.Vessel
{
    /// <summary>
    /// Class that represents data source for Vessel Rule Ports records for reports
    /// </summary>
    class VesselRulePortsRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortsRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public VesselRulePortsRecordsDataSource(IRuleRepository<VesselRulePort> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.VesselRulePort;

        public IRuleRepository<VesselRulePort> SearchRepository { get; }
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
