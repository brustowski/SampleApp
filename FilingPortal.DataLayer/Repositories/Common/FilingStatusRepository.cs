using FilingPortal.Parts.Common.Domain.Entities;

namespace FilingPortal.DataLayer.Repositories.Common
{
    using FilingPortal.Domain.Entities;
    using Framework.DataLayer;
    using Framework.Domain.Repositories;

    /// <summary>
    /// Represents the repository of the <see cref="HeaderFilingStatus" />
    /// </summary>
    internal class FilingStatusRepository : SearchRepository<HeaderFilingStatus>, ISearchRepository<HeaderFilingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public FilingStatusRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}
