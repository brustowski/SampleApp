using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Represents rail inbound model came from UI
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true)]
    public class RailInboundEditModel
    {
        /// <summary>
        /// Edit model id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Gets or sets consignee full name
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets importer full name
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Supplier full name
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets Equipment Initial
        /// </summary>
        public string EquipmentInitial { get; set; }
        /// <summary>
        /// Gets or sets Equipment Number
        /// </summary>
        public string EquipmentNumber { get; set; }
        /// <summary>
        /// Gets or sets Issuer Code
        /// </summary>
        public string IssuerCode { get; set; }
        /// <summary>
        /// Gets or sets Bill of Lading
        /// </summary>
        public string BillOfLading { get; set; }
        /// <summary>
        /// Gets or sets Port Of Unlading
        /// </summary>
        public string PortOfUnlading { get; set; }
        /// <summary>
        /// Gets or sets Manifest Units
        /// </summary>
        public string ManifestUnits { get; set; }
        /// <summary>
        /// Gets or sets weight
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// Gets or sets weight units
        /// </summary>
        public string WeightUnit { get; set; }
        /// <summary>
        ///  Gets or sets Reference Number
        /// </summary>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Gets or sets Destination
        /// </summary>
        public string Destination { get; set; }
    }
}