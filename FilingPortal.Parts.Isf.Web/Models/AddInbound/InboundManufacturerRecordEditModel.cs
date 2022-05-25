using FilingPortal.PluginEngine.Models.Fields;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Models.AddInbound
{
    /// <summary>
    /// Manufacturer edit model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true, Name = "IsfInboundManufacturerRecordEditModel")]
    public class InboundManufacturerRecordEditModel
    {
        /// <summary>
        /// Gets or sets manufacturer record id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int InboundRecordId { get; set; }

        /// <summary>
        /// Gets or sets Manufacturer is
        /// </summary>
        public string ManufacturerId { get; set; }
        /// <summary>
        /// Gets or sets the address id
        /// </summary>
        public AddressFieldEditModel Address { get; set; }
        /// <summary>
        /// Gets or sets Country Of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets HTS numbers
        /// </summary>
        public string HtsNumbers { get; set; }
    }
}
