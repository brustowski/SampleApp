using FilingPortal.Parts.Common.Domain.Entities;

namespace FilingPortal.DataLayer.Repositories.Common
{
    using Framework.DataLayer;
    using Framework.Domain.Repositories;

    /// <summary>
    /// Represents the repository of the <see cref="HeaderMappingStatus" />
    /// </summary>
    internal class MappingStatusRepository : SearchRepository<HeaderMappingStatus>, ISearchRepository<HeaderMappingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work</param>
        public MappingStatusRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}
