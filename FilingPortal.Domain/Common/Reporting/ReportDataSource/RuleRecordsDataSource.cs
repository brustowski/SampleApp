using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting.ReportDataSource
{
    /// <summary>
    /// Class that represents data source for rule records for reports
    /// </summary>
    public class RuleRecordsDataSource<TRuleEntity> : IReportDatasource where TRuleEntity: Entity, IRuleEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleRecordsDataSource{TRuleEntity}" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        /// <param name="gridName"></param>
        public RuleRecordsDataSource(IRuleRepository<TRuleEntity> searchRepository, string gridName)
        {
            SearchRepository = searchRepository;
            Name = gridName;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name { get; }

        public IRuleRepository<TRuleEntity> SearchRepository { get; }
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
