using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Recon.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Web.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="InboundRecord"/> record validation
    /// </summary>
    public class InboundRecordsValidator : ISingleRecordValidator<InboundRecord>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        private List<string> AddCustomErrors(List<string> errors, InboundRecord record)
        {
            return errors;
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<string> GetErrors(InboundRecord record)
        {
            var errors = new List<string>();
            return AddCustomErrors(errors, record);
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public Task<List<string>> GetErrorsAsync(InboundRecord record)
        {
            return Task.Run(() => GetErrors(record));
        }
    }
}
