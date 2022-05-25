using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Repositories.AppSystem
{
    /// <summary>
    /// Implements repository for 
    /// </summary>
    internal class AppSettingsRepository : RepositoryWithTypedId<AppSettings, string>, IAppSettingsRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="AppSettingsRepository"/>
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public AppSettingsRepository(IUnitOfWorkFactory unitOfWork) : base(unitOfWork)
        {

        }
    }
}
