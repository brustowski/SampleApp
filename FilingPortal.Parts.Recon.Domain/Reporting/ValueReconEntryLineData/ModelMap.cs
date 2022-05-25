using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.ValueReconEntryLineData
{
    /// <summary>
    /// Represents report model mapping for report
    /// </summary>
    internal class ModelMap : ReportModelMap<Model>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.ImportEntryRef).ColumnTitle("Import Entry Ref.");
            Map(x => x.OriginLineNumber).ColumnTitle("Orig. LNO.");
            Map(x => x.OriginCustomsValue).ColumnTitle("Orig. Customs Value");
            Map(x => x.Org).ColumnTitle("ORG");
            Map(x => x.OriginTariff).ColumnTitle("Orig. Tariff");
            Map(x => x.OriginProvProgTariff).ColumnTitle("Orig. Prov/Prog. Tariff");
            Map(x => x.OriginQty1).ColumnTitle("Orig. Qty 1");
            Map(x => x.OriginUq1).ColumnTitle("UQ");
            Map(x => x.OriginSpi).ColumnTitle("Orig. SPI");
            Map(x => x.OriginDuty).ColumnTitle("Orig. Duty");
            Map(x => x.OriginHmfAmount).ColumnTitle("Orig. HMF Amount");
            Map(x => x.OriginMpfAmount).ColumnTitle("Orig. MPF Amount");
            Map(x => x.ReconTariff).ColumnTitle("Recon Tariff");
            Map(x => x.ReconTariffRecProvProgTariff).ColumnTitle("Rec Tariff Rec Prov/Prog. Tariff");
            Map(x => x.ReconQty1).ColumnTitle("Rec Qty 1");
            Map(x => x.ReconUq1).ColumnTitle("UQ");
            Map(x => x.ReconSpi).ColumnTitle("Rec SPI");
        }
    }
}