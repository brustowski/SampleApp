using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for CargoWise Internal record
    /// </summary>
    internal class AllMismatchesModelMap : ReportModelMap<AllMismatchesModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllMismatchesModel"/> class.
        /// </summary>
        public AllMismatchesModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.LineNumber7501).ColumnTitle("7501 Line Number");
            Map(x => x.NaftaRecon).ColumnTitle("NAFTA Recon");
            Map(x => x.FtaReconFiling).ColumnTitle("FTA Recon Filing");
            Map(x => x.Calculated520DDueDate).ColumnTitle("Calculated 520D Due Date");
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaFiledBy).ColumnTitle("PSA Filed By");
            Map(x => x.PsaFiledDate).ColumnTitle("PSA Filed Date");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
            Map(x => x.PsaFiledDate520d).ColumnTitle("PSA Filed Date 520d");
            Map(x => x.PscReasonCodesHeader).ColumnTitle("PSC Reason Codes (Header)");
            Map(x => x.PscReasonCodesLine).ColumnTitle("PSC Reason Codes (Line)");
            Map(x => x.PscExplanation).ColumnTitle("PSC Explanation");
            Map(x => x.LiqDate).ColumnTitle("Liq. Date");
            Map(x => x.LiqType).ColumnTitle("Liq. Type");
            Map(x => x.Voyage).ColumnTitle("Voyage Flight");
            Map(x => x.OwnerRef).ColumnTitle("Owner Reference");
            Map(x => x.EnsStatusDescription).ColumnTitle("ENS Status Description");
            Map(x => x.CO).ColumnTitle("C/O");
            Map(x => x.Spi).ColumnTitle("SPI");
            Map(x => x.ManufacturerMid).ColumnTitle("Manufacturer MID");
            Map(x => x.Container).ColumnTitle("Containers");
            Map(x => x.CustomsAttribute1).ColumnTitle("Custom Attrib 1");
            Map(x => x.CustomsQty1).ColumnTitle("Customs Qty 1");
            Map(x => x.CustomsUq1).ColumnTitle("Customs UQ 1");
            Map(x => x.InvoiceLineCharges).ColumnTitle("Invoice Line Charges Amount");
            Map(x => x.Hmf).ColumnTitle("HMF");
            Map(x => x.Mpf).ColumnTitle("MPF");
            Map(x => x.PayableMpf).ColumnTitle("Payable MPF");
            Map(x => x.EnsStatus).ColumnTitle("ENS Status");
            Map(x => x.PriorDisclosureMisc).ColumnTitle("Prior Disclosure MISC");
            Map(x => x.ProtestPetitionFiledStatMisc).ColumnTitle("Protest Petition Filed Stat MISC");
            Map(x => x.Nafta303ClaimStatMisc).ColumnTitle("NAFTA 303 claim stat MISC");
            Map(x => x.CbpForm28Action).ColumnTitle("CBP Form 28 Action");
            Map(x => x.CbpForm29Action).ColumnTitle("CBP Form 29 Action");
        }
    }
}