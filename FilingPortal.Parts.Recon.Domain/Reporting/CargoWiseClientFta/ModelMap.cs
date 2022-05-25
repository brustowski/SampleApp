using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseClientFta
{
    /// <summary>
    /// Represents report model mapping for CargoWise Client record
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
            Map(x => x.NaftaRecon).ColumnTitle("NAFTA Recon");
            Map(x => x.Calculated520DDueDate).ColumnTitle("Calculated 520D Due Date");
            Map(x => x.Voyage).ColumnTitle("Voyage Flight");
            Map(x => x.OwnerRef).ColumnTitle("Owner Reference");
            Map(x => x.CO).ColumnTitle("C/O");
            Map(x => x.Spi).ColumnTitle("SPI");
            Map(x => x.ManufacturerMid).ColumnTitle("Manufacturer MID");
            Map(x => x.Container).ColumnTitle("Containers");
            Map(x => x.CustomsAttribute1).ColumnTitle("Custom Attrib 1");
            Map(x => x.CustomsQty1).ColumnTitle("Customs Qty 1");
            Map(x => x.CustomsUq1).ColumnTitle("Customs UQ 1");
            Map(x => x.Mpf).ColumnTitle("MPF");
            Map(x => x.PayableMpf).ColumnTitle("Payable MPF");
            Map(x => x.PriorDisclosureMisc).ColumnTitle("Prior Disclosure MISC");
            Map(x => x.ProtestPetitionFiledStatMisc).ColumnTitle("Protest Petition Filed Stat MISC");
            Map(x => x.Nafta303ClaimStatMisc).ColumnTitle("NAFTA 303 claim stat MISC");
            Map(x => x.FtaEligibility).ColumnTitle("FTA Eligibility");
            Map(x => x.ClientNote).ColumnTitle("Client Note");
        }
    }
}