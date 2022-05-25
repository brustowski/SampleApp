using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Defines the <see cref="FilingHeader" /> - model for uniting inbond records for filing
    /// </summary>
    public class FilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// For some filing headers users needs confirmation. This flag determines whether confirmation is required for this specific filing header
        /// </summary>
        public bool ConfirmationNeeded { get; set; }

        /// <summary>
        /// Gets or sets the Truck Documents
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

        /// <summary>
        /// Gets or sets the Inbond records, united by this model
        /// </summary>
        public virtual ICollection<InboundRecord> InbondRecords { get; set; } = new List<InboundRecord>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<Document> documents)
        {
            foreach (Document document in documents)
                Documents.Add(document);
        }

        /// <summary>
        /// Adds the inbond records
        /// </summary>
        /// <param name="inbondRecords">The Collection of the <see cref="IEnumerable{InboundRecord}"/></param>
        public void AddInboundRecords(IEnumerable<InboundRecord> inbondRecords)
        {
            foreach (var inbondRecord in inbondRecords)
            {
                InbondRecords.Add(inbondRecord);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => InbondRecords.Select(x => x.Id);
    }
}
