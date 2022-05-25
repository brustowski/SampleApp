using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Filing Header type Configuration
    /// </summary>
    public class FilingHeaderConfiguration : EntityTypeConfiguration<FilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingHeaderConfiguration"/> class.
        /// </summary>
        public FilingHeaderConfiguration() : this("zones_entry") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FilingHeaderConfiguration(string schema)
        {
            ToTable("filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.JobStatus).HasColumnType("int");

            Property(x => x.LastModifiedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); // it should be read only!

            HasMany(t => t.InboundRecords).WithMany(t => t.FilingHeaders)
                .Map(m =>
                {
                    m.ToTable("filing_detail", schema);
                    m.MapLeftKey("filing_header_id");
                    m.MapRightKey("inbound_id");
                });
        }
    }
}
