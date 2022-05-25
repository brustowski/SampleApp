using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.Domain.Validators.Audit.Rail
{
    /// <summary>
    /// Provides methods for single <see cref="AuditRailDailySpiRule"/> record validation
    /// </summary>
    public class RailDailyAuditSpiRuleInboundValidator : ISingleRecordValidator<AuditRailDailySpiRule>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected List<string> AddCustomErrors(List<string> errors, AuditRailDailySpiRule record)
        {
            // Add additional business logic errors
            return errors;
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<string> GetErrors(AuditRailDailySpiRule record)
        {
            var errors = new List<string>();
            return AddCustomErrors(errors, record);
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(AuditRailDailySpiRule record)
        {
            return Task.Run(() => GetErrors(record));
        }
    }
}
