using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Recon.Domain.Import.ValueRecon
{
    /// <summary>
    /// Provides Excel file data mapping on Client data value recon import
    /// </summary>
    internal class ModelMap : ParseModelMap<Model>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Model"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("CargoWise Client");

            Map(x => x.Importer);
            Map(x => x.ImporterNo, "Importer No");
            Map(x => x.Filer);
            Map(x => x.EntryNo, "Entry No");
            Map(x => x.LineNumber7501, "7501 Line Number");
            Map(x => x.JobNumber, "Job Number");
            Map(x => x.CalculatedClientReconDueDate, "Calculated Client Recon Due Date");
            Map(x => x.ExportDate, "Export Date");
            Map(x => x.ImportDate, "Import Date");
            Map(x => x.ReleaseDate, "Release Date");
            Map(x => x.EntryPort, "Entry Port");
            Map(x => x.DestinationState, "Destination State");
            Map(x => x.EntryType, "Entry Type");
            Map(x => x.TransportMode, "Transport Mode");
            Map(x => x.Vessel, "Vessel");
            Map(x => x.Voyage, "Voyage Flight");
            Map(x => x.OwnerRef, "Owner Reference");
            Map(x => x.Spi, "SPI");
            Map(x => x.CO, "C/O");
            Map(x => x.ManufacturerMid, "Manufacturer MID");
            Map(x => x.Tariff);
            Map(x => x.GoodsDescription, "Goods Description");
            Map(x => x.Container, "Containers");
            Map(x => x.CustomsAttribute1, "Custom Attrib 1");
            Map(x => x.CustomsQty1, "Customs Qty 1");
            Map(x => x.CustomsUq1, "Customs UQ 1");
            Map(x => x.MasterBill, "Master Bill");
            Map(x => x.LineEnteredValue, "Line Entered Value");
            Map(x => x.InvoiceLineCharges, "Invoice Line Charges Amount");
            Map(x => x.Duty);
            Map(x => x.Hmf, "HMF");
            Map(x => x.Mpf, "MPF");
            Map(x => x.PayableMpf, "Payable MPF");
            Map(x => x.PriorDisclosureMisc, "Prior Disclosure MISC");
            Map(x => x.ProtestPetitionFiledStatMisc, "Protest Petition Filed Stat MISC");
            Map(x => x.FinalUnitPrice, "Final Unit Price");
            Map(x => x.FinalTotalValue, "Final Total Value");
            Map(x => x.ClientNote, "Client Note");
        }
    }
}
