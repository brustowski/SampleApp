using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the Pipeline Filing Header Entity
    /// </summary>
    public class PipelineFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets the Rail Parsed records 
        /// </summary>
        public virtual ICollection<PipelineInbound> PipelineInbounds { get; set; } = new List<PipelineInbound>();

        /// <summary>
        /// Gets or sets the Rail Documents
        /// </summary>
        public virtual ICollection<PipelineDocument> PipelineDocuments { get; set; } = new List<PipelineDocument>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<PipelineDocument> documents)
        {
            foreach (PipelineDocument railDocument in documents)
            {
                PipelineDocuments.Add(railDocument);
            }
        }

        /// <summary>
        /// Adds the Pipeline Inbound records
        /// </summary>
        /// <param name="records">The records</param>
        public void AddPipelineInbounds(IEnumerable<PipelineInbound> records)
        {
            foreach (PipelineInbound record in records)
            {
                PipelineInbounds.Add(record);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => PipelineInbounds.Select(x => x.Id);
    }
}
