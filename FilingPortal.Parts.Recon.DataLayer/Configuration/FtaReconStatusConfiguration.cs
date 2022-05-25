using FilingPortal.Parts.Recon.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides FTA Recon Status entity type configuration
    /// </summary>
    public class FtaReconStatusConfiguration : EntityTypeConfiguration<FtaReconStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconStatusConfiguration"/> class.
        /// </summary>
        public FtaReconStatusConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconStatusConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FtaReconStatusConfiguration(string schema)
        {
            ToTable("fta_recon_status", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasMaxLength(20).IsRequired();
            Property(x => x.Code).HasMaxLength(3);
        }
    }
}
