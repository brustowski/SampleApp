using System.Collections.Generic;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Repositories
{
    /// <summary>
    /// Describes Common data provider Repository
    /// </summary>
    public interface IDataProviderRepository<TData, in TId>
        where TData : EntityWithTypedId<TId>
    {
        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        IList<TData> Search(string searchInfo, int limit);

        /// <summary>
        /// Gets data by id
        /// </summary>
        /// <param name="id">Data identifier</param>
        TData Get(TId id);
    }
}
