using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Rail.Export.Domain.Entities
{
    /// <summary>
    /// Defines the <see cref="FilingHeader" /> - model for uniting inbound records for filing
    /// </summary>
    public class FilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets whether filing header information is confirmed
        /// </summary>
        public bool Confirmed { get; set; }

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
                Documents.Add(document);
        }

        /// <summary>
        /// Adds the inbound records
        /// </summary>
        /// <param name="inboundRecords">The Collection of the <see cref="IEnumerable{InboundRecord}"/></param>
        public void AddInboundRecords(IEnumerable<InboundRecord> inboundRecords)
        {
            foreach (var inboundRecord in inboundRecords)
            {
                InboundRecords.Add(inboundRecord);
            }
        }

        /// <summary>
        /// Gets if record can be confirmed
        /// </summary>
        public bool CanBeConfirmed => CanBeEdited || MappingStatus == Common.Domain.Enums.MappingStatus.Open;

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => InboundRecords.Select(x => x.Id);
    }
}
