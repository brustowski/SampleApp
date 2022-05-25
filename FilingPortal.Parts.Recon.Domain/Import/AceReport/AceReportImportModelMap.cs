using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Recon.Domain.Import.AceReport
{
    /// <summary>
    /// Provides Excel file data mapping on ace report import
    /// </summary>
    internal class AceReportImportModelMap : ParseModelMap<AceReportImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportImportModel"/> class.
        /// </summary>
        public AceReportImportModelMap()
        {
            Sheet("Main Report");

            Map(x => x.ImporterName, "Importer Name");
            Map(x => x.ImporterNumber, "Importer Number");
            Map(x => x.BondType, "Bond Type");
            Map(x => x.BondNumber, "Bond Number");
            Map(x => x.SuretyCode, "Surety Code");
            Map(x => x.EntryTypeCode, "Entry Type Code");
            Map(x => x.EntrySummaryNumber, "Entry Summary Number");
            Map(x => x.EntrySummaryLineNumber, "Entry Summary Line Number");
            Map(x => x.ImportationDate, "Importation Date");
            Map(x => x.EntrySummaryDate, "Entry Summary Date");
            Map(x => x.ReconciliationIndicator, "Reconciliation Indicator");
            Map(x => x.ReconciliationIssueCode, "Reconciliation Issue Code");
            Map(x => x.ReconciliationOtherDueDate, "Reconciliation (Other) Due Date");
            Map(x => x.ReconciliationOtherStatus, "Reconciliation (Other) Status");
            Map(x => x.ReconciliationOtherEntrySummaryNumber, "Reconciliation (Other) Entry Summary Number");
            Map(x => x.NaftaReconciliationIndicator, "NAFTA Reconciliation Indicator");
            Map(x => x.ReconciliationFtaDueDate, "Reconciliation (FTA) Due Date");
            Map(x => x.ReconciliationFtaStatus, "Reconciliation (FTA) Status");
            Map(x => x.ReconciliationFtaEntrySummaryNumber, "Reconciliation (FTA) Entry Summary Number");
            Map(x => x.ReviewTeamNumber, "Review Team Number");
            Map(x => x.CountryOfOriginCode, "Country of Origin Code");
            Map(x => x.LineSpiCode, "Line SPI Code");
            Map(x => x.ManufacturerId, "Manufacturer ID");
            Map(x => x.HtsNumberFull, "HTS Number - Full");
            Map(x => x.LineTariffQuantity, "Line Tariff Quantity (1)");
            Map(x => x.LineGoodsValueAmount, "Line Goods Value Amount");
            Map(x => x.LineDutyAmount, "Line Duty Amount");
            Map(x => x.TotalPaidMpfAmount, "Total Paid MPF Amount");
            Map(x => x.LineMpfAmount, "Line MPF Amount");
            Map(x => x.LineHmfAmount, "Line HMF Amount");
            Map(x => x.LatestPscFiledDate, "Latest PSC Filed Date");
        }
    }
}
