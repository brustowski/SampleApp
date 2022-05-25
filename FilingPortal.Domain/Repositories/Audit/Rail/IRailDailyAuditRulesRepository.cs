using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Audit.Rail
{
    /// <summary>
    /// Describes the repository of the <see cref="AuditRailDailyRule"/> entity
    /// </summary>
    public interface IRailDailyAuditRulesRepository : IRuleRepository<AuditRailDailyRule>
    {
        /// <summary>
        /// Finds rules to process current record
        /// </summary>
        /// <param name="record">Rail Daily audit record</param>
        Task<IList<AuditRailDailyRule>> FindCorrespondingRules(AuditRailDaily record);
    }
}
