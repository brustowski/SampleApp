using FilingPortal.Domain.Validators;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using System.Collections.Generic;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="InboundReadModel"/> record validation
    /// </summary>
    internal class InboundRecordValidator : BaseSingleRecordValidator<InboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, InboundReadModel record)
        {
            if (record.HasVendorRule == 0)
            {
                errors.Add($"Rule is missing for Vendor = \"{record.Vendor}\"");
            }

            if (record.HasPortRule == 0)
            {
                errors.Add($"Rule is missing for Port = \"{record.Port}\"");
            }

            if (record.HasProductRule == 0)
            {
                errors.Add($"Rule is missing for Product = \"{record.Product}\"");
            }

            return errors;
        }
    }
}
