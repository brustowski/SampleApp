namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Describes Importer Supplier Rail Rule View Model
    /// </summary>
    public class RailRuleImporterSupplierEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Importer name
        /// </summary>
        public string ImporterName { get; set; }
        /// <summary>
        /// Gets or sets Supplier name
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// Gets or sets Product Description
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets Main Supplier
        /// </summary>
        public string MainSupplier { get; set; }
        /// <summary>
        /// Gets or sets Main Supplier Address
        /// </summary>
        public string MainSupplierAddress { get; set; }
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Destination State
        /// </summary>
        public string DestinationState { get; set; }
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets Manufacturer
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// Gets or sets Manufacturer Address
        /// </summary>
        public string ManufacturerAddress { get; set; }
        /// <summary>
        /// Gets or sets Seller
        /// </summary>
        public string Seller { get; set; }
        /// <summary>
        /// Gets or sets Sold-to-Party
        /// </summary>
        public string SoldToParty { get; set; }
        /// <summary>
        /// Gets or sets Ship-to-party
        /// </summary>
        public string ShipToParty { get; set; }
        /// <summary>
        /// Gets or sets Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets Relationship
        /// </summary>
        public string Relationship { get; set; }
        /// <summary>
        /// Gets or sets DFT
        /// </summary>
        public string DFT { get; set; }
        /// <summary>
        /// Gets or sets  Value Recon
        /// </summary>
        public string ValueRecon { get; set; }
        /// <summary>
        /// Gets or sets FTA Recon
        /// </summary>
        public string FTARecon { get; set; }
        /// <summary>
        /// Gets or sets Payment Type
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// Gets or sets Broker-to-Pay
        /// </summary>
        public string BrokerToPay { get; set; }
        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets Freight composite 
        /// </summary>
        public RailRuleFreightComposite FreightComposite { get; set; }
    }
}