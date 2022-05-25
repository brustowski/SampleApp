using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Parts.Recon.Domain.Reporting.ReconEntryAssociation
{
    /// <summary>
    /// Represents report model mapping for report record
    /// </summary>
    internal class ModelMap : ReportModelMap<Model>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Ignore(x => x.Id);
            Map(x => x.FilerEntryNo).ColumnTitle("Filer Code + Entry No");
            Map(x => x.DutyFees).ColumnTitle("Calculate Org Duty/Fees");
        }
    }
}