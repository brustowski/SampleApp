using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="AceReportRecord"/>
    /// </summary>
    internal class AceReportRecordMap : ReportModelMap<AceReportRecord>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportRecordMap"/> class.
        /// </summary>
        public AceReportRecordMap()
        {
            Ignore(x => x.Id);
            Ignore(x => x.CreatedUser);
            Ignore(x => x.CreatedDate);
            Map(x => x.NaftaReconciliationIndicator).ColumnTitle("NAFTA Reconciliation Indicator");
            Map(x => x.ReconciliationFtaDueDate).ColumnTitle("Reconciliation FTA Due Date");
            Map(x => x.ReconciliationFtaStatus).ColumnTitle("Reconciliation FTA Status");
            Map(x => x.LineSpiCode).ColumnTitle("Line SPI Code");
            Map(x => x.HtsNumberFull).ColumnTitle("HTS Number Full");
            Map(x => x.TotalPaidMpfAmount).ColumnTitle("Total Paid MPF Amount");
            Map(x => x.LineMpfAmount).ColumnTitle("Line MPF Amount");
        }
    }
}