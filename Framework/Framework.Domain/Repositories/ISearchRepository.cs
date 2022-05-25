using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace Framework.Domain.Repositories
{
    public interface ISearchRepository
    {
        Task<TableInfo> GetTotalRowsAsync(string tableName);
        Task<int> GetTotalMatchesAsync<TDto>(SearchRequest request) where TDto : class;
        Task<SimplePagedResult<TEntityDto>> GetAllAsync<TEntityDto>(SearchRequest request) where TEntityDto : class;
    }

    public interface ISearchRepository<TEntity> : IRepository<TEntity>, ISearchRepository where TEntity : Entity
    {
        PagedResult<TEntityDto> GetAll<TEntityDto>(SearchRequest request) where TEntityDto : class;

        IEnumerable<TEntityDto> GetAll<TEntityDto>() where TEntityDto : class;

        IEnumerable<TEntityDto> GetAll<TEntityDto>(ISpecification specification, int limit, bool distinct) where TEntityDto : class;
        Task<SimplePagedResult<TEntityDto>> GetAllOrderedAsync<TEntityDto>(SearchRequest request) where TEntityDto : class;
    }
}