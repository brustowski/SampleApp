using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using Framework.DataLayer;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Repositories
{
    /// <summary>
    /// Class for repository of <see cref="ProductCode"/>
    /// </summary>
    public class ProductCodeRepository : RepositoryWithTypedId<ProductCode, Guid>, IProductCodeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodeRepository"/> class
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ProductCodeRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<ProductCode> Search(string searchInfo, int limit)
        {
            IQueryable<ProductCode> exact = Set.Where(x => x.Code == searchInfo);
            IQueryable<ProductCode> codes = Set.AsQueryable();
            IQueryable<ProductCode> descriptions = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                codes = codes.Where(x => x.Code.StartsWith(searchInfo));
                descriptions = descriptions.Where(x => x.Code.StartsWith(searchInfo));
            }

            if (limit > 0)
            {
                codes = codes.Take(limit);
                descriptions = descriptions.Take(limit);
            }

            IEnumerable<ProductCode> union = exact.Union(codes).Union(descriptions)
                .AsEnumerable().DistinctBy(x => new { x.Code, x.Description });
            return union.OrderBy(x => x.Code).ToList();
        }

        /// <summary>
        /// Checks if product with specified code exists
        /// </summary>
        /// <param name="productCode">The product code to check</param>
        public bool IsExist(string productCode)
        {
            return Set.Any(x => x.Code == productCode);
        }

        /// <summary>
        /// Gets the Product by specified code
        /// </summary>
        /// <param name="productCode">The Product code</param>
        public ProductCode GetByCode(string productCode)
        {
            return Set.FirstOrDefault(x => x.Code == productCode);
        }
    }
}
