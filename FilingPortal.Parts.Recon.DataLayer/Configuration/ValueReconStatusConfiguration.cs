using FilingPortal.Parts.Recon.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Value Recon Status entity type configuration
    /// </summary>
    public class ValueReconStatusConfiguration : EntityTypeConfiguration<ValueReconStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconStatusConfiguration"/> class.
        /// </summary>
        public ValueReconStatusConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconStatusConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public ValueReconStatusConfiguration(string schema)
        {
            ToTable("value_recon_status", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasMaxLength(20).IsRequired();
            Property(x => x.Code).HasMaxLength(3);
        }
    }
}
