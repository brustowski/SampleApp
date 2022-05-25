namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Equipment Detail
    /// </summary>
    public class EquipmentDetail
    {
        /// <summary>
        /// Gets or sets the Equipment number
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// Gets or sets the Seal Number 1
        /// </summary>
        public string SealNumber1 { get; set; }
        /// <summary>
        /// Gets or sets the Seal Number 2
        /// </summary>
        public string SealNumber2 { get; set; }
        /// <summary>
        /// Gets or sets the Container Equipment Type Code
        /// </summary>
        public string ContainerEquipmentTypeCode { get; set; }
        /// <summary>
        /// Gets or sets the Load Empty Status Code
        /// </summary>
        public string LoadEmptyStatusCode { get; set; }
    }
}
