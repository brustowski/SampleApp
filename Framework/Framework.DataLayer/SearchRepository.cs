using AutoMapper.QueryableExtensions;
using Framework.Domain;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Framework.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.DataLayer
{
    public abstract class SearchRepository<TEntity> : Repository<TEntity>, ISearchRepository<TEntity> where TEntity : Entity
    {
        protected SearchRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
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
        protected IQueryable<TEntityDto> ProjectTo<TEntityDto>(IQueryable<TEntity> source)
        {
            return typeof(TEntity) == typeof(TEntityDto) ? source.Cast<TEntityDto>() : source.ProjectTo<TEntityDto>();
        }

        public IEnumerable<TEntityDto> GetAll<TEntityDto>() where TEntityDto : class
            => Set.ProjectTo<TEntityDto>().AsEnumerable();

        public PagedResult<TEntityDto> GetAll<TEntityDto>(SearchRequest request) where TEntityDto : class
        {
            IQueryable<TEntityDto> projectTo = ProjectTo<TEntityDto>();

            if (request.Specification != null)
            {
                var specification = (ISpecification<TEntityDto>)request.Specification;
                projectTo = projectTo.Where(specification.GetExpression());
            }

            IQueryable<TEntityDto> entityDtos = projectTo.OrderByField(request.SortingSettings);

            if (request.PagingSettings != null)
            {
                entityDtos = entityDtos.ToPage(request.PagingSettings);
            }

            Console.WriteLine(entityDtos.ToString());
            var resultItems = entityDtos.ToList();

            PagedResult<TEntityDto> result = MakePagedResult(request.PagingSettings, resultItems, projectTo.Count());
            return result;
        }

        public async Task<TableInfo> GetTotalRowsAsync(string tableName)
        {
            var sql = @"select sum (spart.rows) as [RowCount] " +
                         "from sys.partitions spart " +
                         string.Format("where spart.object_id = object_id('{0}')", tableName) +
                         " and spart.index_id < 2";

            TableInfo result = await UnitOfWork.Context.Database.SqlQuery<TableInfo>(sql).SingleAsync();

            return result;
        }


        public async Task<int> GetTotalMatchesAsync<TDto>(SearchRequest request) where TDto : class
        {
            IQueryable<TDto> projectTo = ProjectTo<TDto>();

            if (request.Specification != null)
            {
                var specification = (ISpecification<TDto>)request.Specification;
                projectTo = projectTo.Where(specification.GetExpression());
            }

            return await projectTo.CountAsync();
        }

        public async Task<SimplePagedResult<TEntityDto>> GetAllAsync<TEntityDto>(SearchRequest request) where TEntityDto : class
        {
            request.SortingSettings = new SortingSettings();
            return await GetAllOrderedAsync<TEntityDto>(request);
        }

        public async Task<SimplePagedResult<TEntityDto>> GetAllOrderedAsync<TEntityDto>(SearchRequest request) where TEntityDto : class
        {
            IQueryable<TEntityDto> source;
            if (request.Specification is ISpecification<TEntity> entitySpecification)
            {
                source = ProjectTo<TEntityDto>(Set.Where(entitySpecification.GetExpression()));
            }
            else
            {
                source = ProjectTo<TEntityDto>();
                if (request.Specification is ISpecification<TEntityDto> dtoSpecification)
                {
                    source = source.Where(dtoSpecification.GetExpression());
                }
            }
            if (request.SortingSettings != null)
            {
                source = source.OrderByField(request.SortingSettings);
            }
            if (request.PagingSettings != null)
            {
                source = source.ToPage(request.PagingSettings);
            }

            List<TEntityDto> resultItems = await source.ToListAsync();
            SimplePagedResult<TEntityDto> result = MakeSimplePagedResult(request.PagingSettings, resultItems);
            return result;
        }

        private PagedResult<TEntityDto> MakePagedResult<TEntityDto>(PagingSettings pagingSettings, IEnumerable<TEntityDto> resultItems, int rowCount) where TEntityDto : class
        {
            var totalRecords = Set.Count();
            PagedResult<TEntityDto> result = pagingSettings == null
                ? new PagedResult<TEntityDto>
                {
                    CurrentPage = 1,
                    PageSize = rowCount,
                    TotalRecords = rowCount,
                }
                : new PagedResult<TEntityDto>
                {
                    CurrentPage = pagingSettings.PageNumber,
                    PageSize = pagingSettings.PageSize,
                    TotalRecords = totalRecords
                };
            result.Results = resultItems;
            result.RowCount = rowCount;
            var pageCount = (double)result.RowCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
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
        public IEnumerable<TEntityDto> GetAll<TEntityDto>(ISpecification specification, int limit = 10000, bool distinct = false)
            where TEntityDto : class
        {
            IQueryable<TEntity> set = Set.AsQueryable();

            if (specification != null)
            {
                set = set.Where((specification as ISpecification<TEntity>).GetExpression());
            }

            IQueryable<TEntityDto> result = set.ProjectTo<TEntityDto>();

            if (distinct)
            {
                result = result.Distinct();
            }

            return result.Take(limit).AsEnumerable();
        }
    }
}