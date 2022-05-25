namespace FilingPortal.Domain.DTOs
{
    /// <summary>
    /// Represents parsed Rail Manifest
    /// </summary>
    public class Manifest
    {
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets Supplier
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets EquipmentInitial
        /// </summary>
        public string EquipmentInitial { get; set; }
        /// <summary>
        /// Gets or sets EquipmentNumber
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// Gets or sets IssuerCode
        /// </summary>
        public string IssuerCode { get; set; }
        /// <summary>
        /// Gets or sets BillofLading
        /// </summary>
        public string BillofLading { get; set; }
        /// <summary>
        /// Gets or sets PortofUnlading
        /// </summary>
        public string PortofUnlading { get; set; }
        /// <summary>
        /// Gets or sets Description1
        /// </summary>
        public string Description1 { get; set; }
        /// <summary>
        /// Gets or sets ManifestUnits
        /// </summary>
        public string ManifestUnits { get; set; }
        /// <summary>
        /// Gets or sets Weight
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// Gets or sets WeightUnit
        /// </summary>
        public string WeightUnit { get; set; }
        /// <summary>
        /// Gets or sets ReferenceNumber1
        /// </summary>
        public string ReferenceNumber1 { get; set; }
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets raw manifest text
        /// </summary>
        public string ManifestText { get; set; }
    }
}