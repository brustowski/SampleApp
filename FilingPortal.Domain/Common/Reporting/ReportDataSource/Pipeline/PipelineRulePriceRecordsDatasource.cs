using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource.Pipeline
{
    /// <summary>
    /// Class that represents data source for Pipeline Price rule records for reports
    /// </summary>
    class PipelineRulePriceRecordsDataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulePriceRecordsDataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public PipelineRulePriceRecordsDataSource(IRuleRepository<PipelineRulePrice> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.PipelineRulePrice;

        public IRuleRepository<PipelineRulePrice> SearchRepository { get; }
        /// <summary>
        /// Gets all available records as <see cref="SimplePagedResult{TEntityDto}"/> specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="request">The <see cref="SearchRequest"/> object</param>
        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(SearchRequest request) where TEntity : class,new()
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
