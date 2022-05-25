using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="FtaFlagMismatchModel"/>
    /// </summary>
    internal class FtaFlagMismatchModelMap : ReportModelMap<FtaFlagMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaFlagMismatchModelMap"/> class.
        /// </summary>
        public FtaFlagMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.NaftaReconciliationIndicator).ColumnTitle("NAFTA Reconciliation Indicator");
            Map(x => x.NaftaRecon).ColumnTitle("NAFTA Recon");
        }
    }
}