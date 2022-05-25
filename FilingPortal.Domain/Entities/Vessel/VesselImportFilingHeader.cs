using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines the <see cref="VesselImportFilingHeader" /> - model for uniting vessel records for filing
    /// </summary>
    public class VesselImportFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets the Truck Documents
        /// </summary>
        public virtual ICollection<VesselImportDocument> Documents { get; set; } = new List<VesselImportDocument>();

        /// <summary>
        /// Gets or sets the Vessel Inbound records, united by this model
        /// </summary>
        public virtual ICollection<VesselImportRecord> VesselInbounds { get; set; } = new List<VesselImportRecord>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<VesselImportDocument> documents)
        {
            foreach (VesselImportDocument railDocument in documents)
                Documents.Add(railDocument);
        }

        /// <summary>
        /// Adds the vessel inbound records
        /// </summary>
        /// <param name="vesselInbounds">The Collection of the <see cref="IEnumerable{VesselImport}"/></param>
        public void AddVesselInbounds(IEnumerable<VesselImportRecord> vesselInbounds)
        {
            foreach (VesselImportRecord vesselInbound in vesselInbounds)
            {
                VesselInbounds.Add(vesselInbound);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => VesselInbounds.Select(x => x.Id);
    }
}
