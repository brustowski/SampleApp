using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    /// <summary>
    /// Represents the Rail Filing Header Entity
    /// </summary>
    public class RailFilingHeader : FilingHeaderOld
    {
        /// <summary>
        /// Gets or sets the Calculated Gross Weight Summary 
        /// </summary>
        public decimal? GrossWeightSummary { get; set; }

        /// <summary>
        /// Gets or sets the Calculated Gross Weight Unit
        /// </summary>
        public string GrossWeightSummaryUnit { get; set; }

        /// <summary>
        /// Gets or sets the Rail Parsed records 
        /// </summary>
        public virtual ICollection<RailBdParsed> RailBdParseds { get; set; } = new List<RailBdParsed>();

        /// <summary>
        /// Gets or sets the Rail Documents
        /// </summary>
        public virtual ICollection<RailDocument> RailDocuments { get; set; } = new List<RailDocument>();

        /// <summary>
        /// Adds the specified documents
        /// </summary>
        /// <param name="documents">The documents</param>
        public void AddDocuments(IEnumerable<RailDocument> documents)
        {
            foreach (RailDocument railDocument in documents)
            {
                RailDocuments.Add(railDocument);
            }
        }

        /// <summary>
        /// Adds the Rail Parsed records
        /// </summary>
        /// <param name="bdParsedRecords">The bd parsed records</param>
        public void AddRailBdParseds(IEnumerable<RailBdParsed> bdParsedRecords)
        {
            foreach (RailBdParsed railBdParsed in bdParsedRecords)
            {
                RailBdParseds.Add(railBdParsed);
            }
        }

        /// <summary>
        /// Gets inbound records ids
        /// </summary>
        public override IEnumerable<int> GetInboundRecordIds() => RailBdParseds.Select(x => x.Id);
    }
}
