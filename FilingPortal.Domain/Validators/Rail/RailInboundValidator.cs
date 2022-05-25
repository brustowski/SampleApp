using FilingPortal.Domain.Entities.Rail;
using System.Collections.Generic;

namespace FilingPortal.Domain.Validators.Rail
{
    /// <summary>
    /// Provides methods for single <see cref="RailInboundReadModel"/> record validation
    /// </summary>
    public class RailInboundValidator : BaseSingleRecordValidator<RailInboundReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, RailInboundReadModel record)
        {
            if (!HasAppliedImporterSupplierRule(record))
            {
                errors.Add($"Rule is missed for Importer = \"{record.BdParsedImporterConsignee}\" and Supplier= \"{record.BdParsedSupplier}\"");
            }

            if (!HasAppliedHtsRule(record))
            {
                errors.Add(
                    $"Rule is missed for Description 1 = \"{record.BdParsedDescription1}\", Importer = \"{record.Importer}\", Supplier = \"{record.Supplier}\", Port (optional) = \"{record.PortCode}\" and Destination (optional) = \"{record.Destination}\"");
            }

            return errors;
        }

        /// <summary>
        /// Determines whether the specified record has applied HTS rule
        /// </summary>
        /// <param name="record">The record</param>
        private static bool HasAppliedHtsRule(RailInboundReadModel record)
        {
            return !string.IsNullOrEmpty(record.HTS);
        }

        /// <summary>
        /// Determines whether the specified record has applied Importer-Supplier rule
        /// </summary>
        /// <param name="record">The record</param>
        private static bool HasAppliedImporterSupplierRule(RailInboundReadModel record)
        {
            return !string.IsNullOrEmpty(record.Importer) && !string.IsNullOrEmpty(record.Supplier);
        }
    }
}
