using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Inbond.Domain.Import.RuleEntry
{
    /// <summary>
    /// Provides Excel file data mapping on Entry Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// The FIRMs code
        /// </summary>
        private string _firmsCode;
        /// <summary>
        /// Gets or sets the FIRMS code
        /// </summary>
        public string FirmsCode
        {
            get => string.IsNullOrWhiteSpace(_firmsCode) ? _firmsCode : _firmsCode.PadLeft(4, '0');
            set => _firmsCode = value;
        }
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
        public string ConfirmationNeeded { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        public override string ToString()
        {
            return string.Join("|", new[]{
                FirmsCode
                ,Importer
                ,Carrier
                ,Consignee
                ,ConsigneeAddress
                ,UsPortOfDestination
                ,EntryType
                ,Shipper
                ,Tariff
                ,PortOfPresentation
                ,ForeignDestination
                ,TransportMode
            });
        }
    }
}
