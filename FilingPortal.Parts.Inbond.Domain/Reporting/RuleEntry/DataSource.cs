using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Inbond.Domain.Config;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.RuleEntry
{
    /// <summary>
    /// Class that represents data source for report
    /// </summary>
    internal class DataSource : IReportDatasource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource" /> class
        /// </summary>
        /// <param name="searchRepository"></param>
        public DataSource(IRuleRepository<Entities.RuleEntry> searchRepository)
        {
            SearchRepository = searchRepository;
        }

        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        public string Name => GridNames.RuleEntry;

        /// <summary>
        /// Gets the Rule repository
        /// </summary>
        public IRuleRepository<Entities.RuleEntry> SearchRepository { get; }

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
