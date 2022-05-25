using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Implements methods working with discharge terminals handbook
    /// </summary>
    internal class ProductDescriptionsRepository : SearchRepository<ProductDescriptionHandbookRecord>, IProductDescriptionsRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="ProductDescriptionsRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public ProductDescriptionsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Searches for product description in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="tariffId">Tariff Id</param>
        public IList<ProductDescriptionHandbookRecord> Search(string searchInfoSearch, int searchInfoLimit, int tariffId)
        {
            IQueryable<ProductDescriptionHandbookRecord> query = Set.Where(x => x.TariffId == tariffId);

            if (!string.IsNullOrWhiteSpace(searchInfoSearch))
            {
                query = query.Where(x => x.Name.Contains(searchInfoSearch));
            }

            if (searchInfoLimit > 0)
            {
                query = query.Take(searchInfoLimit);
            }

            return query.OrderBy(x => x.Name).ToList();
        }
    }
}
