using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.TruckExport
{
    /// <summary>
    /// Describes Filing Header Entity for Truck Export entities
    /// </summary>
    public class TruckExportFilingHeader : FilingHeaderNew
    {
        /// <summary>
        /// Determines whether this record can be refiled
        /// </summary>
        public override bool CanBeRefiled => CanBeEdited && !string.IsNullOrWhiteSpace(FilingNumber) && TruckExports.All(x=>x.IsUpdate);

        /// <summary>
        /// Gets or sets value indicating that this record was updated
        /// </summary>
        public bool IsUpdated { get; set; }

        /// <summary>
        /// Gets or sets the Documents
        /// </summary>
        public virtual ICollection<TruckExportDocument> Documents { get; set; } = new List<TruckExportDocument>();

        /// <summary>
        /// Gets or sets the Truck Exports records, united by this model
        /// </summary>
        public virtual ICollection<TruckExportRecord> TruckExports { get; set; } = new List<TruckExportRecord>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<TruckExportDocument> documents)
        {
            foreach (TruckExportDocument railDocument in documents)
            {
                Documents.Add(railDocument);
            }
        }

        /// <summary>
        /// Adds the Truck Export records
        /// </summary>
        /// <param name="truckExports">Collection of the Trcuk Export Records</param>
        public void AddRecords(IEnumerable<TruckExportRecord> truckExports)
        {
            foreach (TruckExportRecord record in truckExports)
            {
                TruckExports.Add(record);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => TruckExports.Select(x => x.Id);
    }
}
