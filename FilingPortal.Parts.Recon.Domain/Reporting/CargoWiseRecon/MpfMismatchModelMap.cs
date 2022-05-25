using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="MpfMismatchModel"/>
    /// </summary>
    internal class MpfMismatchModelMap : ReportModelMap<MpfMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpfMismatchModel"/> class.
        /// </summary>
        public MpfMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.Mpf).ColumnTitle("MPF");
            Map(x => x.LineMpfAmount).ColumnTitle("Line MPF Amount");
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
        }
    }
}