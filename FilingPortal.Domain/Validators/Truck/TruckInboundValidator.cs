using System.Collections.Generic;
using FilingPortal.Domain.Entities.Truck;

namespace FilingPortal.Domain.Validators.Truck
{
    /// <summary>
    /// Provides methods for single <see cref="TruckInboundReadModel"/> record validation
    /// </summary>
    internal class TruckInboundValidator : BaseSingleRecordValidator<TruckInboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, TruckInboundReadModel record)
        {
            if (record.HasImporterRule == 0)
                errors.Add($"Importer rule is missing for Importer = \"{record.Importer}\"");

            if (record.HasPortRule == 0)
                errors.Add($"Ports rule is missing for this record");

            return errors;
        }
    }
}
