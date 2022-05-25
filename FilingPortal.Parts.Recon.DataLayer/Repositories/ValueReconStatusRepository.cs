using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="ValueReconStatus"/>
    /// </summary>
    public class ValueReconStatusRepository : RepositoryWithTypedId<ValueReconStatus, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public ValueReconStatusRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
