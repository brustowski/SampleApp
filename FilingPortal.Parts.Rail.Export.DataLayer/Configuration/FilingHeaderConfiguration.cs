using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Filing Header type Configuration
    /// </summary>
    public class FilingHeaderConfiguration : EntityTypeConfiguration<FilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingHeaderConfiguration"/> class.
        /// </summary>
        public FilingHeaderConfiguration() : this("us_exp_rail") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FilingHeaderConfiguration(string schema)
        {
            ToTable("filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.CreatedDate).IsRequired();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.MappingStatus).HasColumnType("tinyint");
            Property(x => x.FilingStatus).HasColumnType("tinyint");
            Property(x => x.JobLink).IsMaxLength();

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
