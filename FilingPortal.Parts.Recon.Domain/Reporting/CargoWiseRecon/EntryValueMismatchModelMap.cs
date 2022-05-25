using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents report model mapping for <see cref="EntryValueMismatchModel"/>
    /// </summary>
    internal class EntryValueMismatchModelMap : ReportModelMap<EntryValueMismatchModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryValueMismatchModel"/> class.
        /// </summary>
        public EntryValueMismatchModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}