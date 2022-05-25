using System.Collections.Generic;
using FilingPortal.Domain.Entities.Audit.Rail;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Audit.Rail
{
    /// <summary>
    /// Describes the repository of the <see cref="AuditRailTrainConsistSheet"/> entity
    /// </summary>
    public interface IAuditTrainConsistSheetRepository : ISearchRepository<AuditRailTrainConsistSheet>
    {
        /// <summary>
        /// Returns Train consist sheets created by specific user
        /// </summary>
        /// <param name="userAccount">User login</param>
        /// <returns></returns>
        IEnumerable<AuditRailTrainConsistSheet> GetAll(string userAccount);

        /// <summary>
        /// Runs stored procedure to verify rail train consist sheet data
        /// </summary>
        /// <param name="userAccount">User who starts verification process</param>
        void Verify(string userAccount);
    }
}
