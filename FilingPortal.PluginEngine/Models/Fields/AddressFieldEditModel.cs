using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models.Fields
{
    /// <summary>
    /// Represents address field underlying object
    /// </summary>
    [TsInterface(AutoI = false, FlattenHierarchy = true, IncludeNamespace = false)]
    public class AddressFieldEditModel
    {
        /// <summary>
        /// Gets or sets address identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Address ID
        /// </summary>
        public string AddressId { get; set; }
        /// <summary>
        /// Gets or sets Address Code
        /// </summary>
        public string AddressCode { get; set; }
        /// <summary>
        /// Gets or sets whether address is manually overriden or not
        /// </summary>
        public bool Override { get; set; }
        /// <summary>
        /// Gets or sets company name
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Gets or sets address' first part
        /// </summary>
        public string Addr1 { get; set; }
        /// <summary>
        /// Gets or sets address' second part
        /// </summary>
        public string Addr2 { get; set; }
        /// <summary>
        /// Gets or sets country code
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Gets or sets City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets Postal Code
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// Gets or sets State Codes
        /// </summary>
        public string StateCode { get; set; }
    }
}
