using System.Collections.Generic;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Validators
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
            if (record.HasConsigneeRule == 0)
                errors.Add($"Consignee rule is missing for Importer = \"{record.Importer}\"");

            if (record.HasExporterConsigneeRule == 0)
                errors.Add($"Exporter-Consignee rule is missing for Exporter = \"{record.Exporter}\" and Importer = \"{record.Importer}\"");

            return errors;
        }
    }
}
