using System;
using Framework.Domain;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents the Inbound record
    /// </summary>
    public class InboundParsedData : Entity
    {
        /// <summary>
        /// Gets or sets Filer Code
        /// </summary>
        public string FilerCode { get; set; }
        /// <summary>
        /// Gets or sets Entry Number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets Check Digit
        /// </summary>
        public string CheckDigit { get; set; }

        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public DateTime? ImportDate { get; set; }

        /// <summary>
        /// Gets or sets Team No
        /// </summary>
        public string TeamNo { get; set; }

        /// <summary>
        /// Gets or sets NAFTA Recon
        /// </summary>
        public bool? NaftaRecon { get; set; }

        /// <summary>
        /// Gets or sets Recon Issue
        /// </summary>
        public string ReconIssue { get; set; }

        /// <summary>
        /// Gets or sets Ultimate Destination State
        /// </summary>
        public string UltimateDestinationState { get; set; }

        /// <summary>
        /// Gets or sets Application Begin Date
        /// </summary>
        public DateTime? ApplicationBeginDate { get; set; }

        /// <summary>
        /// Gets or sets Application End Date
        /// </summary>
        public DateTime? ApplicationEndDate { get; set; }

        /// <summary>
        /// Gets or sets Total Entered Value
        /// </summary>
        public decimal? TotalEnteredValue { get; set; }

        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public decimal? GrossWgt { get; set; }
        /// <summary>
        /// Gets or sets Gross weight unit
        /// </summary>
        public string GrossWgtUnit { get; set; }

        /// <summary>
        /// Gets or sets FTZ number
        /// </summary>
        public string FtzNumber { get; set; }
        /// <summary>
        /// Gets or sets Charges sum
        /// </summary>
        public decimal? Charges { get; set; }

        /// <summary>
        /// Gets or sets inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }
    }
}