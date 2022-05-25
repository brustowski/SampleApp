using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    using FilingPortal.Domain.AppSystem.Repositories;
    using Framework.DataLayer;

    /// <summary>
    /// Defines Search repository for <see cref="AppAdmin"/> entities
    /// </summary>
    public class AppAdminRepository : SearchRepository<AppAdmin>, IAppAdminRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppAdminRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unitOfWork<see cref="IUnitOfWorkFactory"/></param>
        public AppAdminRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {
        }
    }
}
