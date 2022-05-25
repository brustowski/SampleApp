using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Entity represents UNLOCO dictionary entry
    /// </summary>
    public class UnlocoDictionaryEntry : Entity
    {
        /// <summary>
        /// Gets or sets UNLOCO code
        /// </summary>
        public string Unloco { get; set; }
        /// <summary>
        /// Gets or sets Port name
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// Gets or sets Port Diacriticals
        /// </summary>
        public string PortDiacriticals { get; set; }
        /// <summary>
        /// Gets or sets Country Code
        /// </summary>
        public string CountryCode { get; set; }
    }
}
