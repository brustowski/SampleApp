using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Value Records entity type configuration
    /// </summary>
    public class ValueReconConfiguration : EntityTypeConfiguration<ValueRecon>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconConfiguration"/> class.
        /// </summary>
        public ValueReconConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public ValueReconConfiguration(string schema)
        {
            ToTable("value_recon", schema);

            HasKey(x => x.Id);

            Property(x => x.ClientNote).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
