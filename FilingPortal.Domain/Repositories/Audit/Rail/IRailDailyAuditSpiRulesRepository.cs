using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Audit.Rail
{
    /// <summary>
    /// Describes the repository of the <see cref="AuditRailDailySpiRule"/> entity
    /// </summary>
    public interface IRailDailyAuditSpiRulesRepository : IRuleRepository<AuditRailDailySpiRule>
    {
        /// <summary>
        /// Finds spi rules to process current record
        /// </summary>
        /// <param name="record">Rail Daily audit record</param>
        Task<IList<AuditRailDailySpiRule>> FindCorrespondingRules(AuditRailDaily record);
    }
}
