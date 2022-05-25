using FilingPortal.Domain.Entities.Handbooks;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="IssuerCode"/>
    /// </summary>
    public class IssuerCodeConfiguration : EntityTypeConfiguration<IssuerCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerCodeConfiguration"/> class.
        /// </summary>
        public IssuerCodeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IssuerCodeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public IssuerCodeConfiguration(string schema)
        {
            ToTable("handbook_issuer_code", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Code).HasColumnName("value").HasMaxLength(4).IsRequired();
            Property(x => x.Name).HasColumnName("display_value");
            Property(x => x.TransportationMode).HasMaxLength(2);

            HasIndex(x => x.Code).HasName("Idx_value").IsUnique();
        }
    }
}
