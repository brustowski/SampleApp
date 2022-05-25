using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="ValueFlagMismatchModel"/>
    /// </summary>
    internal class ValueFlagMismatchModelMap : ReportModelMap<ValueFlagMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueFlagMismatchModelMap"/> class.
        /// </summary>
        public ValueFlagMismatchModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}