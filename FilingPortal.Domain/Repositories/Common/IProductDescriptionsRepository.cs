using System.Collections.Generic;
using FilingPortal.Domain.Entities.Handbooks;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Common
{
    /// <summary>
    /// Describes methods working with discharge terminals handbook
    /// </summary>
    public interface IProductDescriptionsRepository : ISearchRepository<ProductDescriptionHandbookRecord>
    {
        /// <summary>
        /// Searches for product descriptions in repository
        /// </summary>
        /// <param name="searchInfoSearch">Search request</param>
        /// <param name="searchInfoLimit">Max items limit</param>
        /// <param name="tariffId">State to search within</param>
        IList<ProductDescriptionHandbookRecord> Search(string searchInfoSearch, int searchInfoLimit, int tariffId);
    }
}
