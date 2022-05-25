using System.Collections.Generic;
using FilingPortal.Domain.Entities.Vessel;

namespace FilingPortal.Domain.Validators.Vessel
{
    /// <summary>
    /// Provides methods for single <see cref="VesselImportReadModel"/> record validation
    /// </summary>
    internal class VesselImportValidator : BaseSingleRecordValidator<VesselImportReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, VesselImportReadModel record)
        {
            if (record.HasPortRule == 0)
                errors.Add($"Rule is missing for FIRMs Code = \"{record.FirmsCode}\"");

            if (record.HasProductRule == 0)
                errors.Add($"Rule is missing for Classification = \"{record.Classification}\"");

            return errors;
        }
    }
}
