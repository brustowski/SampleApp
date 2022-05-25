using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="QuantityMismatchModel"/>
    /// </summary>
    internal class QuantityMismatchModelMap : ReportModelMap<QuantityMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityMismatchModel"/> class.
        /// </summary>
        public QuantityMismatchModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.PsaReason).ColumnTitle("PSA Reason");
            Map(x => x.PsaReason520d).ColumnTitle("PSA Reason 520d");
        }
    }
}