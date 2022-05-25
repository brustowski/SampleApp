using FilingPortal.Parts.Recon.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Recon.DataLayer.Repositories
{
    /// <summary>
    /// Provides the repository of <see cref="FtaReconStatus"/>
    /// </summary>
    public class FtaReconStatusRepository : RepositoryWithTypedId<FtaReconStatus, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconStatusRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        public FtaReconStatusRepository(IUnitOfWorkFactory<UnitOfWorkContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
