using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="PayableMpfMismatchModel"/>
    /// </summary>
    internal class PayableMpfMismatchModelMap : ReportModelMap<PayableMpfMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayableMpfMismatchModel"/> class.
        /// </summary>
        public PayableMpfMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.PayableMpf).ColumnTitle("Payable MPF");
            Map(x => x.TotalPaidMpfAmount).ColumnTitle("Total Paid MPF Amount");
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
        }
    }
}