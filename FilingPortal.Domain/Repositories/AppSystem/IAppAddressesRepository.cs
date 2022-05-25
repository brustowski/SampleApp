using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.AppSystem
{
    /// <summary>
    /// Describes the Addresses repository
    /// </summary>
    public interface IAppAddressesRepository : ISearchRepository<AppAddress>
    {
        /// <summary>
        /// Gets the untracked value by id
        /// </summary>
        /// <param name="id">The address id</param>
        AppAddress GetUntracked(int id);
    }
}
