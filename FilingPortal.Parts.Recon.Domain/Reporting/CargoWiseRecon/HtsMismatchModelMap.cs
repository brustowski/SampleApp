using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="HtsMismatchModel"/>
    /// </summary>
    internal class HtsMismatchModelMap : ReportModelMap<HtsMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtsMismatchModel"/> class.
        /// </summary>
        public HtsMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.HtsNumberFull).ColumnTitle("HTS Number Full");
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
        }
    }
}