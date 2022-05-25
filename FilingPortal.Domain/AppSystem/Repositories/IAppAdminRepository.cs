using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Domain.AppSystem.Repositories
{
    using Framework.Domain.Repositories;

    /// <summary>
    /// Describes Search repository for <see cref="AppAdmin"/> entities
    /// </summary>
    public interface IAppAdminRepository : ISearchRepository<AppAdmin>
    {
    }
}
