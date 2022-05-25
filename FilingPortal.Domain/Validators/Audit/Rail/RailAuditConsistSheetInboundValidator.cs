using System.Collections.Generic;
using System.Threading.Tasks;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.Domain.Validators.Audit.Rail
{
    /// <summary>
    /// Provides methods for single <see cref="AuditRailTrainConsistSheet"/> record validation
    /// </summary>
    public class RailAuditConsistSheetInboundValidator : ISingleRecordValidator<AuditRailTrainConsistSheet>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected List<string> AddCustomErrors(List<string> errors, AuditRailTrainConsistSheet record)
        {
            if (record.Status == "Not matched")
            {
                errors.Add($"Record was found in CargoWise, but was not found in imported records");
            }

            if (record.Status == "Not found")
            {
                errors.Add($"Record was not found in CargoWise");
            }

            return errors;
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<string> GetErrors(AuditRailTrainConsistSheet record)
        {
            var errors = new List<string>();
            return AddCustomErrors(errors, record);
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(AuditRailTrainConsistSheet record)
        {
            return Task.Run(() => GetErrors(record));
        }
    }
}
