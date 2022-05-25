using FilingPortal.Domain.Entities.VesselExport;
using System.Collections.Generic;

namespace FilingPortal.Domain.Validators.VesselExport
{
    /// <summary>
    /// Provides methods for single <see cref="VesselExportReadModel"/> record validation
    /// </summary>
    internal class VesselExportValidator : BaseSingleRecordValidator<VesselExportReadModel>
    {
        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected override List<string> AddCustomErrors(List<string> errors, VesselExportReadModel record)
        {
            if (record.HasUsppiConsigneeRule == 0)
                errors.Add($"USPPI-Consignee rule is missing for USPPI = \"{record.Usppi}\" and Consignee = \"{record.Importer}\"");

            return errors;
        }
    }
}
