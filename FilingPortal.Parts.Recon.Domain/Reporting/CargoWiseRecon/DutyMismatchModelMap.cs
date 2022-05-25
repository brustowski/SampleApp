using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="DutyMismatchModel"/>
    /// </summary>
    internal class DutyMismatchModelMap : ReportModelMap<DutyMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DutyMismatchModel"/> class.
        /// </summary>
        public DutyMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
        }
    }
}