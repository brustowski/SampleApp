using Framework.Domain;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Domain.Repositories
{
    /// <summary>
    /// Describes the Auditable Entity repository
    /// </summary>
    public interface IAuditableEntityRepository<TAuditableEntity> where TAuditableEntity : AuditableEntity
    {
        /// <summary>
        /// Gets the collection of the users specified by search text
        /// </summary>
        /// <param name="searchText">The search text</param>
        IEnumerable<string> GetCreatedUsers(string searchText);
    }
}
