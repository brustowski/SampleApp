using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Interface for Report data sources
    /// </summary>
    public interface IReportDatasource
    {
        /// <summary>
        /// Gets the name of the data source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets all available records as <see cref="SimplePagedResult{TEntityDto}"/> specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="searchRequest">The <see cref="SearchRequest"/> object</param>
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(SearchRequest searchRequest) where TEntity : class, new();

        /// <summary>
        /// Gets total matches of records specified by <see cref="SearchRequest"/> model
        /// </summary>
        /// <typeparam name="TEntity">Type of the records</typeparam>
        /// <param name="searchRequest">The <see cref="SearchRequest"/> object</param>
        Task<int> GetTotalMatches<TEntity>(SearchRequest searchRequest) where TEntity : class;
    }
}
