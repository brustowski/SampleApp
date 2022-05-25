using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="EntryType" />
    /// </summary>
    public class EntryTypeRepository : RepositoryWithTypedId<EntryType, string>, IDataProviderRepository<EntryType, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryTypeRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public EntryTypeRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Searches for records in the repository specified by search request
        /// </summary>
        /// <param name="searchInfo">Search request</param>
        /// <param name="limit">Max items limit</param>
        public IList<EntryType> Search(string searchInfo, int limit)
        {
            IQueryable<EntryType> query = Set.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchInfo))
            {
                query = query.Where(x => x.Id.StartsWith(searchInfo) || x.Description.Contains(searchInfo));
            }

            return query.OrderBy(x => x.Id).Take(limit).ToList();
        }
    }
}