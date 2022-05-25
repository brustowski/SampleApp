using System;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.RuleEntry
{
    /// <summary>
    /// Describes Rule FIRMs Code Report Model
    /// </summary>
    public class RuleEntryReportModel
    {
        /// <summary>
        /// Gets or sets the Rule Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Get or sets the Importer address
        /// </summary>
        public string ImporterAddress { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Get or sets the Consignee address
        /// </summary>
        public string ConsigneeAddress { get; set; }
        /// <summary>
        /// Gets or sets the US Port of Destination
        /// </summary>
        public string UsPortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets the Entry Type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets the Shipper
        /// </summary>
        public string Shipper { get; set; }
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets the Port of Presentation
        /// </summary>
        public string PortOfPresentation { get; set; }
        /// <summary>
        /// Gets or sets the Foreign Destination
        /// </summary>
        public string ForeignDestination { get; set; }
        /// <summary>
        /// Gets or sets the Transport Mode
        /// </summary>
        public string TransportMode { get; set; }
        /// <summary>
        /// Gets or sets whether this rule requires confirmation on review
        /// </summary>
        public int ConfirmationNeeded { get; set; }
        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        public string CreatedUser { get; set; }
    }
}