using FilingPortal.Domain.Entities.Rail;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.Rail
{
    /// <summary>
    /// Represents repository of the Rail Section entity
    /// </summary>
    class RailSectionRepository : SearchRepository<RailSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailSectionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public RailSectionRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}