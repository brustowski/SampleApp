using System;
using FilingPortal.Domain.Entities.Audit.Rail;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Audit.Rail
{
    /// <summary>
    /// Describes the repository of the <see cref="AuditRailDaily"/> entity
    /// </summary>
    public interface IRailDailyAuditRepository : ISearchRepository<AuditRailDaily>
    {
        /// <summary>
        /// Loads Audit data from CargoWise
        /// </summary>
        /// <param name="dtFrom">Date From</param>
        /// <param name="dtTo">Date To</param>
        int LoadFromCargoWise(DateTime dtFrom, DateTime dtTo);
    }
}
