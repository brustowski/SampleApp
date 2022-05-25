using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Rail.RuleImporterSupplier
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Importer Supplier Rule");

            Map(p => p.ImporterName, "Importer Name");
            Map(p => p.SupplierName, "Shipper Name");
            Map(p => p.ProductDescription, "Product Description");
            Map(p => p.Port, "Port");
            Map(p => p.Destination, "Destination");
            Map(p => p.MainSupplier, "Main Supplier");
            Map(p => p.MainSupplierAddress, "Main Supplier Address");
            Map(p => p.Importer, "Imp. Code");
            Map(p => p.DestinationState, "Destination State");
            Map(p => p.Consignee, "Consignee");
            Map(p => p.Manufacturer, "Manufacturer");
            Map(p => p.ManufacturerAddress, "Manufacturer Address");
            Map(p => p.Seller, "Seller");
            Map(p => p.SoldToParty, "Sold To Party");
            Map(p => p.ShipToParty, "Ship To Party");
            Map(p => p.CountryOfOrigin, "Country Of Origin");
            Map(p => p.Relationship, "Relationship");
            Map(p => p.DFT, "DFT");
            Map(p => p.ValueRecon, "Value Recon");
            Map(p => p.FTARecon, "FTA Recon");
            Map(p => p.PaymentType, "Payment Type");
            Map(p => p.BrokerToPay, "Broker To Pay");
            Map(p => p.Value, "Value");
            Map(p => p.Freight, "Freight");
            Map(p => p.FreightType, "Freight Type");
        }
    }
}
