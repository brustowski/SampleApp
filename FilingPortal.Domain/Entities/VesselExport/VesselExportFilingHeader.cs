using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.VesselExport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes Filing Header Entity for Vessel Export entities
    /// </summary>
    public class VesselExportFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets the Documents
        /// </summary>
        public virtual ICollection<VesselExportDocument> Documents { get; set; } = new List<VesselExportDocument>();

        /// <summary>
        /// Gets or sets the Vessel Exports records, united by this model
        /// </summary>
        public virtual ICollection<VesselExportRecord> VesselExports { get; set; } = new List<VesselExportRecord>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<VesselExportDocument> documents)
        {
            foreach (VesselExportDocument railDocument in documents)
                Documents.Add(railDocument);
        }

        /// <summary>
        /// Adds the Vessel Export records
        /// </summary>
        /// <param name="vesselExports">Collection of the Trcuk Export Records</param>
        public void AddRecords(IEnumerable<VesselExportRecord> vesselExports)
        {
            foreach (VesselExportRecord record in vesselExports)
            {
                VesselExports.Add(record);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => VesselExports.Select(x => x.Id);
    }
}
