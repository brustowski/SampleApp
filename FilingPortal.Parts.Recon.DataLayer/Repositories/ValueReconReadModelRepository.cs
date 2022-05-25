using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.DataLayer;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="ValueReconReadModel"/>
    /// </summary>
    public class ValueReconReadModelRepository : SearchRepository<ValueReconReadModel>, IValueReconReadModelRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconReadModelRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ValueReconReadModelRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Checks entities against a condition
        /// </summary>
        /// <param name="expression">Condition to check</param>
        /// <param name="searchRequest">Search request</param>
        public bool CheckFor<TEntityDto>(Expression<Func<TEntityDto, bool>> expression, SearchRequest searchRequest) where TEntityDto : class
        {
            if (searchRequest.Specification is ISpecification<TEntityDto> entitySpecification)
            {
                return ProjectTo<TEntityDto>().Where(entitySpecification.GetExpression()).All(expression);
            }

            return false;
        }

        /// <summary>
        /// Get collection of the processed and filtered records
        /// </summary>
        /// <typeparam name="TEntity">Type of the model</typeparam>
        /// <param name="request">search request</param>
        public async Task<IEnumerable<TEntity>> GetProcessedAsync<TEntity>(SearchRequest request) where TEntity : class, new()
        {
            IQueryable<ValueReconReadModel> src = Set.Where(x => x.Status == (int) ValueReconStatusValue.Processed)
                    .Where(x => x.PsaReason == null || x.PsaReason == string.Empty ||
                                x.PsaFiledDate != null) // PSA exclude flagged and not filed
                    .Where(x => x.PsaReason520d == null || x.PsaReason520d == string.Empty ||
                                x.PsaFiledDate520d != null) // PSA520d exclude flagged and not filed
                ;
            IQueryable<TEntity> source;
            if (request.Specification is ISpecification<ValueReconReadModel> entitySpecification)
            {
                source = ProjectTo<TEntity>(src.Where(entitySpecification.GetExpression()));
            }
            else
            {
                source = ProjectTo<TEntity>(src);
                if (request.Specification is ISpecification<TEntity> dtoSpecification)
                {
                    source = source.Where(dtoSpecification.GetExpression());
                }
            }

            List<TEntity> result = await source.ToListAsync();
            return result;
        }

        /// <summary>
        /// Get number of the matched processed and filtered records
        /// </summary>
        /// <typeparam name="TEntity">Type of the model</typeparam>
        /// <param name="request">search request</param>
        public async Task<int> GetTotalProcessedAsync<TEntity>(SearchRequest request) where TEntity : class
        {
            IQueryable<ValueReconReadModel> src = Set.Where(x => x.Status == (int) ValueReconStatusValue.Processed)
                .Where(x => x.PsaReason == null || x.PsaReason == string.Empty ||
                            x.PsaFiledDate != null) // PSA exclude flagged and not filed
                .Where(x => x.PsaReason520d == null || x.PsaReason520d == string.Empty ||
                            x.PsaFiledDate520d != null); // PSA520d exclude flagged and not filed
            switch (request.Specification)
            {
                case ISpecification<ValueReconReadModel> entitySpecification:
                    return await src.CountAsync(entitySpecification.GetExpression());
                case ISpecification<TEntity> dtoSpecification:
                    return await ProjectTo<TEntity>(src).CountAsync(dtoSpecification.GetExpression());
                default:
                    return await Set.CountAsync();
            }
        }
    }
}
