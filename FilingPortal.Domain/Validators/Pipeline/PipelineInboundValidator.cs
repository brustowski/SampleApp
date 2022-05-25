using System.Collections.Generic;
using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Validators.Pipeline
{
    /// <summary>
    /// Class for single Inbound Record validation
    /// </summary>
    internal class PipelineInboundValidator : BaseSingleRecordValidator<PipelineInboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, PipelineInboundReadModel record)
        {
            if (record.HasImporterRule == 0)
                errors.Add($"Importer rule is missing for Importer = \"{record.Importer}\"");

            if (record.HasBatchRule == 0)
                errors.Add($"Batch rule is missing for Batch = \"{record.Batch}\"");

            if (record.HasFacilityRule == 0)
                errors.Add($"Facility rule is missing for Facility = \"{record.Facility}\"");

            if (record.HasPriceRule == 0)
                errors.Add($"Price rule is missing for Importer = \"{record.Importer}\", batch code (optional) = \"{record.Batch}\" and facility (optional) = \"{record.Facility}\"");


            return errors;
        }
    }
}
