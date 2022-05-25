using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Client Records entity type configuration
    /// </summary>
    public class AceReportRecordConfiguration : EntityTypeConfiguration<AceReportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportRecordConfiguration"/> class.
        /// </summary>
        public AceReportRecordConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AceReportRecordConfiguration(string schema)
        {
            ToTable("inbound_ace_report", schema);

            HasKey(x => x.Id);

            Property(x => x.ImporterName).HasMaxLength(255);
            Property(x => x.ImporterNumber).HasMaxLength(20);
            Property(x => x.BondNumber).HasMaxLength(20);
            Property(x => x.SuretyCode).HasMaxLength(3);
            Property(x => x.EntryTypeCode).HasMaxLength(3);
            Property(x => x.EntrySummaryNumber).IsRequired().HasMaxLength(11);
            Property(x => x.EntrySummaryLineNumber).IsRequired();
            Property(x => x.ReconciliationIndicator).HasMaxLength(1);
            Property(x => x.ReconciliationIssueCode).HasMaxLength(5);
            Property(x => x.ReconciliationOtherEntrySummaryNumber).HasMaxLength(11);
            Property(x => x.ReconciliationFtaEntrySummaryNumber).HasMaxLength(11);
            Property(x => x.NaftaReconciliationIndicator).HasMaxLength(1);
            Property(x => x.ReviewTeamNumber).HasMaxLength(5);
            Property(x => x.CountryOfOriginCode).HasMaxLength(2);
            Property(x => x.LineSpiCode).HasMaxLength(2);
            Property(x => x.HtsNumberFull).HasMaxLength(10);

            HasIndex(x => new {x.EntrySummaryNumber, x.EntrySummaryLineNumber}).IsUnique();
        }
    }
}
