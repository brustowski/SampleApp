using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Domain.DocumentTypes
{
    /// <summary>
    /// Interface for repository with filter data
    /// </summary>
    public interface IFilterDataRepository<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Gets the filtered data containing the specified search string and taking limited number of items
        /// </summary>
        /// <param name="search">The search string</param>
        /// <param name="limit">The limited number of items</param>
        IEnumerable<TEntity> GetFilteredData(string search, int limit);
    }
}