using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Framework.Domain;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;

namespace Framework.DataLayer
{
    public abstract class SearchRepositoryWithTypedId<TEntity,TId> : RepositoryWithTypedId<TEntity, TId>, ISearchRepository where TEntity : EntityWithTypedId<TId>
    {
        protected SearchRepositoryWithTypedId(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }

        protected IQueryable<TEntityDto> ProjectTo<TEntityDto>()
        {
            if (typeof(TEntity) == typeof(TEntityDto))
            {
                return Set.Cast<TEntityDto>();
            }
            return Set.ProjectTo<TEntityDto>();
        }

        public async Task<TableInfo> GetTotalRowsAsync(string tableName)
        {
            string sql = @"select sum (spart.rows) as [RowCount] " +
                         "from sys.partitions spart " +
                         string.Format("where spart.object_id = object_id('{0}')", tableName) +
                         " and spart.index_id < 2";

            var result = await UnitOfWork.Context.Database.SqlQuery<TableInfo>(sql).SingleAsync();

            return result;
        }


        public async Task<int> GetTotalMatchesAsync<TDto>(SearchRequest request) where TDto : class
        {
            var projectTo = ProjectTo<TDto>();

            if (request.Specification != null)
            {
                var specification = (ISpecification<TDto>)request.Specification;
                projectTo = projectTo.Where(specification.GetExpression());
            }

            return await projectTo.CountAsync();
        }

        public async Task<SimplePagedResult<TEntityDto>> GetAllAsync<TEntityDto>(SearchRequest request) where TEntityDto : class
        {
            return await SearchOnAsync(ProjectTo<TEntityDto>(), request);
        }

        protected async Task<SimplePagedResult<TDto>> SearchOnAsync<TDto>(IQueryable<TDto> source, SearchRequest request) where TDto : class
        {
            if (request.Specification is ISpecification<TDto> specification)
            {
                source = source.Where(specification.GetExpression());
            }
            if (request.SortingSettings != null)
            {
                source = source.OrderByField(request.SortingSettings);
            }
            if (request.PagingSettings != null)
            {
                source = source.ToPage(request.PagingSettings);
            }
            var resultItems = await source.ToListAsync();
            var result = MakeSimplePagedResult(request.PagingSettings, resultItems);
            return result;
        }

        private SimplePagedResult<TEntityDto> MakeSimplePagedResult<TEntityDto>(PagingSettings pagingSettings, IEnumerable<TEntityDto> resultItems) where TEntityDto : class
        {
            var result = new SimplePagedResult<TEntityDto>
            {
                CurrentPage = pagingSettings.PageNumber,
                PageSize = pagingSettings.PageSize,
                Results = resultItems,
            };
            return result;
        }
    }
}