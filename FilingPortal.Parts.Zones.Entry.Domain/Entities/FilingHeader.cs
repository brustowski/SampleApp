using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Defines the <see cref="FilingHeader" /> - model for uniting inbound records for filing
    /// </summary>
    public class FilingHeader : FilingHeaderNew
    {
        /// <summary>
        /// Gets or sets the filing header's Documents list
        /// </summary>
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

        /// <summary>
        /// Gets or sets the inbound records, united by this model
        /// </summary>
        public virtual ICollection<InboundRecord> InboundRecords { get; set; } = new List<InboundRecord>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<Document> documents)
        {
            foreach (Document document in documents)
            {
                Documents.Add(document);
            }
        }

        /// <summary>
        /// Adds the inbound records
        /// </summary>
        /// <param name="inboundRecords">The Collection of the <see cref="IEnumerable{InboundRecord}"/></param>
        public void AddInboundRecords(IEnumerable<InboundRecord> inboundRecords)
        {
            foreach (InboundRecord inboundRecord in inboundRecords)
            {
                InboundRecords.Add(inboundRecord);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => InboundRecords.Select(x => x.Id);

        /// <summary>
        /// Determines whether this record can be refiled
        /// </summary>
        public override bool CanBeRefiled => CanBeEdited && !string.IsNullOrWhiteSpace(FilingNumber);
    }
}
