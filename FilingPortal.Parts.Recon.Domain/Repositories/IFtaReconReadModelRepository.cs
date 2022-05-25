using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the repository of the FTA Recon read models
    /// </summary>
    public interface IFtaReconReadModelRepository : ISearchRepository<FtaReconReadModel>
    {
        /// <summary>
        /// Checks entities against a condition
        /// </summary>
        /// <param name="expression">Condition to check</param>
        /// <param name="searchRequest">Search request</param>
        bool CheckFor<TEntityDto>(Expression<Func<TEntityDto, bool>> expression, SearchRequest searchRequest) where TEntityDto : class;
        /// <summary>
        /// Get collection of the processed and filtered records
        /// </summary>
        /// <typeparam name="TEntity">Type of the model</typeparam>
        /// <param name="request">search request</param>
        Task<IEnumerable<TEntity>> GetProcessedAsync<TEntity>(SearchRequest request) where TEntity : class, new();
        /// <summary>
        /// Get number of the matched processed and filtered records
        /// </summary>
        /// <typeparam name="TEntity">Type of the model</typeparam>
        /// <param name="request">search request</param>
        Task<int> GetTotalProcessedAsync<TEntity>(SearchRequest request) where TEntity : class;
    }
}