using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.Inbound
{

    /// <summary>
    /// Provides Excel file data mapping on Canada Truck Import Model
    /// </summary>
    internal class InboundImportModelMap : ParseModelMap<InboundImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundImportModel"/> class.
        /// </summary>
        public InboundImportModelMap()
        {
            Sheet("Batch Log");

            Map(p => p.Vendor, "Vendor");
            Map(p => p.Port, "Port");
            Map(p => p.ParsNumber, "PARS#");
            Map(p => p.Eta, "ETA");
            Map(p => p.OwnersReference, "Owners Reference");
            Map(p => p.GrossWeight, "Gross Weight");
            Map(p => p.DirectShipDate, "Direct Ship Date");
            Map(p => p.Consignee, "Consignee");
            Map(p => p.ProductCode, "Product");
            Map(p => p.InvoiceQty, "Invoice Quantity");
            Map(p => p.LinePrice, "Line Price");
        }
    }
}
