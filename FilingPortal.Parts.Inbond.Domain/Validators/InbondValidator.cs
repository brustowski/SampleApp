using System.Collections.Generic;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="InboundReadModel"/> record validation
    /// </summary>
    internal class InbondValidator : BaseSingleRecordValidator<InboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, InboundReadModel record)
        {
            if (!record.HasEntryRule)
            {
                errors.Add($"Entry rule is missing");
            }

            return errors;
        }
    }
}
