using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using Framework.DataLayer;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Repositories.Common
{
    /// <summary>
    /// Represents the repository of the <see cref="EntryStatus" />
    /// </summary>
    public class EntryStatusRepository : RepositoryWithTypedId<EntryStatus, int>, IEntryStatusRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public EntryStatusRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Get all Entry Statuses filtered by specified status type
        /// </summary>
        public IEnumerable<EntryStatus> GetFilteredByStatusType(string status)
        {
            return Set.Where(x => x.StatusType.Equals(status)).ToList();
        }
    }
}