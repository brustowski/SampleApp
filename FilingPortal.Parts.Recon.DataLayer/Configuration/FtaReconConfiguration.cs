using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides FTA Recon entity type configuration
    /// </summary>
    public class FtaReconConfiguration : EntityTypeConfiguration<FtaRecon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconConfiguration"/> class.
        /// </summary>
        public FtaReconConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FtaReconConfiguration(string schema)
        {
            ToTable("fta_recon", schema);

            HasKey(x => x.Id);

            Property(x => x.ReconNfJobNumber).HasMaxLength(35);
            Property(x => x.ReconIssueCode).HasMaxLength(100);
            Property(x => x.ReconEntryLineSpi).IsMaxLength();
            Property(x => x.ImportDeclarationEntryNumber).HasMaxLength(35);
            Property(x => x.ImportJobDeclarationReference).HasMaxLength(35);
            Property(x => x.DeclarationFtaRecon).HasMaxLength(100);

            Property(x => x.FtaEligibility).HasMaxLength(1).IsRequired();
            Property(x => x.ClientNote).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
