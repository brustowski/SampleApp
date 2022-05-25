using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseClientValue
{
    /// <summary>
    /// Represents report model mapping for CargoWise Client Value record
    /// </summary>
    internal class ModelMap : ReportModelMap<Model>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.LineNumber7501).ColumnTitle("7501 Line Number");
            Map(x => x.Voyage).ColumnTitle("Voyage Flight");
            Map(x => x.OwnerRef).ColumnTitle("Owner Reference");
            Map(x => x.CO).ColumnTitle("C/O");
            Map(x => x.Spi).ColumnTitle("SPI");
            Map(x => x.ManufacturerMid).ColumnTitle("Manufacturer MID");
            Map(x => x.Container).ColumnTitle("Containers");
            Map(x => x.CustomsAttribute1).ColumnTitle("Custom Attrib 1");
            Map(x => x.CustomsQty1).ColumnTitle("Customs Qty 1");
            Map(x => x.CustomsUq1).ColumnTitle("Customs UQ 1");
            Map(x => x.LineEnteredValue).ColumnTitle("Line Entered Value");
            Map(x => x.InvoiceLineCharges).ColumnTitle("Invoice Line Charges Amount");
            Map(x => x.Hmf).ColumnTitle("HMF");
            Map(x => x.Mpf).ColumnTitle("MPF");
            Map(x => x.PayableMpf).ColumnTitle("Payable MPF");
            Map(x => x.PriorDisclosureMisc).ColumnTitle("Prior Disclosure MISC");
            Map(x => x.ProtestPetitionFiledStatMisc).ColumnTitle("Protest Petition Filed Stat MISC");
            Map(x => x.FinalUnitPrice).ColumnTitle("Final Unit Price");
            Map(x => x.FinalTotalValue).ColumnTitle("Final Total Value");
            Map(x => x.ClientNote).ColumnTitle("Client Note");
        }
    }
}