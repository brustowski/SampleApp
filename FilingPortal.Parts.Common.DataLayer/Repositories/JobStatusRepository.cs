using FilingPortal.Parts.Common.Domain.Entities;
using Framework.DataLayer;
using Framework.Domain.Repositories;

namespace FilingPortal.Parts.Common.DataLayer.Repositories
{
    /// <summary>
    /// Represents the repository of the <see cref="HeaderJobStatus" />
    /// </summary>
    internal class JobStatusRepository : SearchRepository<HeaderJobStatus>, ISearchRepository<HeaderJobStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public JobStatusRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
