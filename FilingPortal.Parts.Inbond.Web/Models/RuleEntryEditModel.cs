namespace FilingPortal.Parts.Inbond.Web.Models
{
    /// <summary>
    /// Represents the FIRMs Code rule edit model
    /// </summary>
    public class RuleEntryEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the FIRMs Code Identifier
        /// </summary>
        public string FirmsCodeId { get; set; }
        /// <summary>
        /// Gets or sets the Importer Identifier
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer address identifier
        /// </summary>
        public string ImporterAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets the Consignee Identifier
        /// </summary>
        public string ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets the Consignee address identifier
        /// </summary>
        public string ConsigneeAddressId { get; set; }
        /// <summary>
        /// Gets or sets the US Port of Destination
        /// </summary>
        public string UsPortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets the Entry Type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets the Shipper Identifier
        /// </summary>
        public string ShipperId { get; set; }
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
        public bool ConfirmationNeeded { get; set; }
    }
}
