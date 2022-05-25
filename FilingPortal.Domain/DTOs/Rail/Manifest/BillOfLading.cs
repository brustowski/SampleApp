namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Bill of Lading
    /// </summary>
    public class BillOfLading
    {
        /// <summary>
        /// Gets or sets the Bill of Lading Number
        /// </summary>
        public string BillOfLadingNumber { get; set; }
        /// <summary>
        /// Gets or sets the Issuer SCAC Code
        /// </summary>
        public string IssuerScacCode { get; set; }
        /// <summary>
        /// Gets or sets the Foreign Port of Lading
        /// </summary>
        public string ForeignPortOfLading { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Quantity
        /// </summary>
        public int ManifestQuantity { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Quantity Unit
        /// </summary>
        public string ManifestQuantityUnit { get; set; }
        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Gets or sets the Weight Unit
        /// </summary>
        public string WeightUnit { get; set; }
        /// <summary>
        /// Gets or sets the Bill of Lading Status
        /// </summary>
        public int BillOfLadingStatus { get; set; }
        /// <summary>
        /// Gets or sets the Master In-Bind Indicator
        /// </summary>
        public string MasterInBondIndicator { get; set; }
        /// <summary>
        /// Gets or sets the Measurement
        /// </summary>
        public int Measurement { get; set; }
        /// <summary>
        /// Gets or sets the Measurement unit
        /// </summary>
        public string MeasurementUnit { get; set; }
        /// <summary>
        /// Gets or sets the Place of Receipt by Pre-carrier
        /// </summary>
        public string PlaceOfReceiptByPreCarrier { get; set; }
    }
}
