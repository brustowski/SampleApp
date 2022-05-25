using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.AppSystem
{
    /// <summary>
    /// Describes application settings repository
    /// </summary>
    public interface IAppSettingsRepository : IRepositoryWithTypeId<AppSettings, string>
    {
    }
}
