namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Tariff Detail
    /// </summary>
    public class TariffDetail
    {
        /// <summary>
        /// Gets or sets the Harmonized Tariff Number
        /// </summary>
        public string HarmonizedTariffNumber { get; set; }
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// Gets or sets the Weight Unit
        /// </summary>
        public string WeightUnit { get; set; }
    }
}
